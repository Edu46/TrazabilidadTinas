using LogisticaERP.Clases;
using LogisticaERP.Clases.CalculoTarifaRepresentante.OracleCloud;
using LogisticaERP.Clases.CalculoTarifaRepresentante.OracleCloud.REST;
using LogisticaERP.Clases.CalculoTarifaRepresentante.OracleCloud.SOAP;
using LogisticaERP.Clases.Viajes;
using LogisticaERP.Clases.Viajes.OracleEBS12;
using LogisticaERP.Controles.BusquedaGenerica;
using LogisticaERP.GrupoPinsaSOA;
using LogisticaERP.LogisticaSOA;
using LogisticaERP.SeguridadERPSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP.Catalogos
{
    public partial class CalculoTarifasPorteadores : PaginaBase
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // valida si hay una empresa seleccionada
                if (Session["EmpresaID"] == null || string.IsNullOrEmpty(Session["EmpresaID"].ToString()))
                {
                    hfEmpresaCloudException.Value = "No existe una empresa seleccionada, sera redireccionado a la pagina de Inicio.";
                }
                else
                {
                    decimal idEmpresaSIP = Convert.ToDecimal(Session["EmpresaID"]);
                    var empresaIntegracion = new GPO_EMPRESAS_INTEGRACIONES().ObtenerEmpresasIntegraciones(null, idEmpresaSIP, null, null, null, null, null).FirstOrDefault();
                    // valida si la empresa actual cuenta con un mapeo en empresas integraciones para oracle cloud
                    if (empresaIntegracion == null ||
                        !empresaIntegracion.Id_empresa_eb12.HasValue ||
                        empresaIntegracion.Id_empresa_eb12 <= 0 ||
                        !empresaIntegracion.Id_empresa_cloud.HasValue ||
                        empresaIntegracion.Id_empresa_cloud <= 0)
                        hfEmpresaCloudException.Value = "La Empresa seleccionada no cuenta con un mapeo en Empresas Integraciones, favor de comunicarlo al departamento de TI-Aplicaciones.";
                    else
                    {
                        LLenarDropDownList(ddlCodigoTransporte, new LOGI_CODIGOS_TRANSPORTES().ObtenerCodigosTransportes(null, idEmpresaSIP), "Id_codigo_transporte", "Codigo_transporte");
                        LLenarDropDownList(ddlEstado, new GPO_ESTADOS().ObtenerEstados(), "Id_estado", "Nombre");
                    }
                }
            }
        }

        #endregion

        #region Methods

        [WebMethod(EnableSession = true)]
        public static object CalcularTarifa(string numViaje, string fechaViaje, decimal idProveedorEBS12, decimal idAlmacenEBS12, decimal idMunicipio,
            decimal idCiudad, decimal idCodigoTransporte, bool esBackhaul, bool esCaseta)
        {
            try
            {
                var usuarioSesion = Newtonsoft.Json.JsonConvert.DeserializeObject<UsuarioSesion>(System.Web.HttpContext.Current.Session["UsuarioSesionSerializado"].ToString());
                if (usuarioSesion != null)
                {
                    var listaUsuarioModuloFuncionalidad = usuarioSesion.Usuario.ListaUsuarioFuncionalidad.Where(x => x.Id_modulo == decimal.Parse(Id_Modulo)).ToList();
                    string pagina = HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.IndexOf('/') == 0 ? 1 : 0).ToUpperInvariant();
                    var usuarioFuncionalidad = listaUsuarioModuloFuncionalidad.Where(x =>
                        !string.IsNullOrEmpty(x.Uri)
                        && x.Uri
                            .Substring(x.Uri.IndexOf('/') == 0 ? 1 : 0)
                            .ToUpperInvariant()
                            .Contains(pagina))
                            .FirstOrDefault();

                    // Si el usuario es administrador tiene permiso a todo
                    if (!usuarioSesion.Usuario.Es_admin && (usuarioFuncionalidad == null || !usuarioFuncionalidad.Es_guardar.HasValue && !usuarioFuncionalidad.Es_guardar.Value))
                    {
                        return new { Error = true, Mensaje = "El usuario no tiene privilegios para calcular tarifas." };
                    }
                }

                var viaje = new LOGIViaje().ObtenerViajes(null, null, null, numViaje, Convert.ToDecimal(HttpContext.Current.Session["EmpresaID"]), null, null, true, null, null, true).FirstOrDefault();

                if (string.IsNullOrEmpty(viaje.Chofer) || string.IsNullOrEmpty(viaje.Marca) || string.IsNullOrEmpty(viaje.Modelo) || string.IsNullOrEmpty(viaje.PlacaTractor) || string.IsNullOrEmpty(viaje.PlacaCaja)
                    || string.IsNullOrEmpty(viaje.NumeroSerieCaja) || string.IsNullOrEmpty(viaje.NumeroECOCaja) || string.IsNullOrEmpty(viaje.NumeroRuta) || string.IsNullOrEmpty(viaje.NumeroConvoy) || string.IsNullOrEmpty(viaje.NumeroSello)
                    || string.IsNullOrEmpty(viaje.NumeroCartaPorte) || viaje.IDProveedorOracle <= 0)
                {
                    return new { Error = true, Mensaje = "Es necesario capturar toda la información del Número de Viaje para poder continuar." };
                }

                var calculoTarifa = new LOGI_CALCULOS_TARIFAS().ObtenerCalculosTarifasPorteadores(numViaje, true, false).FirstOrDefault();
                var tarifas = new LOGI_TARIFAS().ObtenerTarifa(null, null, null, idMunicipio, idCiudad, 1, idCodigoTransporte,
                    null, null, idAlmacenEBS12, idProveedorEBS12, esBackhaul, true);

                if (tarifas.Any())
                {
                    DateTime dtFechaViaje = Convert.ToDateTime(fechaViaje);
                    tarifas = tarifas.Where(x => x.Fecha_inicio <= dtFechaViaje && x.Fecha_fin >= dtFechaViaje).ToList();

                    if (tarifas.Count() == 0)
                    {
                        throw new Exception("La tarifa de viaje se encuentra fuera de los rangos de fechas.");
                    }
                    else if (tarifas.Count() > 1)
                    {
                        throw new Exception("La tarifa de viaje se encuentra dentro de varios rangos de fechas.");
                    }
                    else
                    {
                        var tarifa = tarifas.First();
                        new LOGI_CALCULOS_TARIFAS().GrabarCalculoTarifaPorteador(new CalculoTarifa
                        {
                            Id_calculo_tarifa = calculoTarifa != null ? calculoTarifa.Id_calculo_tarifa : 0,
                            Id_tarifa = tarifa.Id_tarifa,
                            Num_viaje = numViaje,
                            Pedido_venta = true,
                            Tranferencia = false,
                            Caseta = esCaseta,
                            Backhaul = esBackhaul,
                            Monto_transporte = tarifa.Monto_tarifa,
                            Monto_caseta = tarifa.Monto_caseta,
                            Activo = true
                        });
                        calculoTarifa = new LOGI_CALCULOS_TARIFAS().ObtenerCalculosTarifasPorteadores(numViaje, true, false).FirstOrDefault();

                        return new
                        {
                            Error = false,
                            CalculoTarifa = calculoTarifa
                        };
                    }
                }
                else
                {
                    throw new Exception("No existe información de Tarifas relacionada con los valores seleccionados.");
                }
            }
            catch (Exception exception)
            {
                return new { Error = true, Mensaje = exception.Message, IDTarifa = string.Empty, MontoTarifa = string.Empty, MontoCaseta = string.Empty };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object CrearOrdenCompraCloud(decimal idCalculoTarifaPorteador, decimal idTarifa, string numViaje, decimal idAlmacenEBS12, string fechaViaje, string proveedor, bool esBackhaul, bool esCaseta, string totalTransporte, string totalCasetas)
        {
            try
            {
                DateTime dtFechaViaje = Convert.ToDateTime(fechaViaje);

                var usuarioSesion = Newtonsoft.Json.JsonConvert.DeserializeObject<UsuarioSesion>(System.Web.HttpContext.Current.Session["UsuarioSesionSerializado"].ToString());
                if (usuarioSesion != null)
                {
                    var listaUsuarioModuloFuncionalidad = usuarioSesion.Usuario.ListaUsuarioFuncionalidad.Where(x => x.Id_modulo == decimal.Parse(Id_Modulo)).ToList();
                    string pagina = HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.IndexOf('/') == 0 ? 1 : 0).ToUpperInvariant();
                    var usuarioFuncionalidad = listaUsuarioModuloFuncionalidad.Where(x =>
                        !string.IsNullOrEmpty(x.Uri)
                        && x.Uri
                            .Substring(x.Uri.IndexOf('/') == 0 ? 1 : 0)
                            .ToUpperInvariant()
                            .Contains(pagina))
                            .FirstOrDefault();

                    // Si el usuario es administrador tiene permiso a todo
                    if (!usuarioSesion.Usuario.Es_admin && (usuarioFuncionalidad == null || !usuarioFuncionalidad.Es_autorizar.HasValue && !usuarioFuncionalidad.Es_autorizar.Value))
                    {
                        return new { Error = true, Mensaje = "El usuario no tiene privilegios para generar ordenes de compras en oracle cloud." };
                    }
                }


                //Valida informacion completa el viaje.
                var viaje = new LOGIViaje().ObtenerViajes(null, null, null, numViaje, Convert.ToDecimal(HttpContext.Current.Session["EmpresaID"]), null, null, true, null, null, true).FirstOrDefault();

                if (string.IsNullOrEmpty(viaje.Chofer) || string.IsNullOrEmpty(viaje.Marca) || string.IsNullOrEmpty(viaje.Modelo) || string.IsNullOrEmpty(viaje.PlacaTractor) || string.IsNullOrEmpty(viaje.PlacaCaja)
                    || string.IsNullOrEmpty(viaje.NumeroSerieCaja) || string.IsNullOrEmpty(viaje.NumeroECOCaja) || string.IsNullOrEmpty(viaje.NumeroRuta) || string.IsNullOrEmpty(viaje.NumeroConvoy) || string.IsNullOrEmpty(viaje.NumeroSello)
                    || string.IsNullOrEmpty(viaje.NumeroCartaPorte) || viaje.IDProveedorOracle <= 0)
                {
                    return new { Error = true, Mensaje = "Es necesario capturar toda la información del Número de Viaje para poder continuar." };
                }


                var empresa = HttpContext.Current.Session["LOG_Empresa"] as Empresa;
                var empresaConfiguracion = new GPO_EMPRESAS_INTEGRACIONES().ObtenerEmpresasIntegraciones(null, empresa.Id_empresa, null, null, null, null, null).FirstOrDefault();
                var almacenOrigen = new INVOrganizationUnit().ObtenerAlmacenesParaTarifasDeViajes(Convert.ToInt64(empresaConfiguracion.Id_empresa_eb12.Value), 1)
                    .FirstOrDefault(x => x.IDAlmacen == idAlmacenEBS12);

                if (almacenOrigen != null && string.IsNullOrEmpty(almacenOrigen.CentroCosto))
                    return new { Error = true, Mensaje = "El almacen no tiene un centro de costo asignado en Oracle EBS 12." };

                var tarifaConfiguracion = new LOGI_TARIFAS().ObtenerTarifaConfiguracion(idTarifa, empresa.Id_empresa, almacenOrigen.CentroCosto);

                Tuple<string, long> locationCodeAndLegalEntityID = new INVInventoryOrganization().GetLocationCodeAndLegalEntityID(almacenOrigen.CodigoAlmacen);
                if (locationCodeAndLegalEntityID == null)
                    return new { Error = true, Mensaje = "El código de ubicación del almacen no se encontro en Oracle Cloud." };

                long supplierID = new POSupplier().GetSupplierID(proveedor);
                if (supplierID == 0)
                    return new { Error = true, Mensaje = "El proveedor no se encontro en Oracle Cloud." };

                string supplierSite = new POSupplierSite().GetSupplierSite(supplierID, empresaConfiguracion.Clave_cloud);
                if (string.IsNullOrEmpty(supplierSite))
                    return new { Error = true, Mensaje = "El sitio del proveedor no se encontro en Oracle Cloud." };

                long agentID = new POProcurementAgent().GetProcurementAgentID(usuarioSesion.Usuario.Email, Convert.ToInt64(empresaConfiguracion.Id_empresa_cloud));
                if (agentID == 0)
                    return new { Error = true, Mensaje = string.Format("El agente de compra no se encontro en Oracle Cloud (correo: {0}).", usuarioSesion.Usuario.Email) };

                // validación de cuenta de transporte
                string[] combinacionContableCuenta = esBackhaul ?
                    tarifaConfiguracion.Combinacion_backhaul.Split('.') :
                    tarifaConfiguracion.Combinacion_pedido_venta.Split('.');
                var vcarEnvelope = new ValidateAndCreateAccountReq.Envelope();
                vcarEnvelope.Body.ValidateAndCreateAccounts.ValidationInputRowList.Add(new ValidateAndCreateAccountReq.ValidationInputRowList
                {
                    Segment1 = combinacionContableCuenta[0],
                    Segment2 = combinacionContableCuenta[1],
                    Segment3 = combinacionContableCuenta[2],
                    Segment4 = combinacionContableCuenta[3],
                    Segment5 = combinacionContableCuenta[4],
                    Segment6 = combinacionContableCuenta[5],
                    Segment7 = combinacionContableCuenta[6],
                    Segment8 = combinacionContableCuenta[7],
                    Segment9 = combinacionContableCuenta[8],
                    LedgerName = tarifaConfiguracion.Nombre_libro_cuenta_cloud
                });
                var validateAndCreatAccount = new AccountCombination().ValidateAndCreateAccount(vcarEnvelope);
                if (validateAndCreatAccount.Item1.Equals("Invalid"))
                    return new { Error = true, Mensaje = validateAndCreatAccount.Item2 };

                // validación de cuenta de caseta
                Tuple<string, string, string> validateAndCreateAccountCaseta = null;
                if (esCaseta)
                {
                    string[] combinacionContableCuenta2 = esBackhaul ?
                    tarifaConfiguracion.Combinacion_backhaul.Split('.') :
                    tarifaConfiguracion.Combinacion_caseta.Split('.');
                    var vcarEnvelope2 = new ValidateAndCreateAccountReq.Envelope();
                    vcarEnvelope2.Body.ValidateAndCreateAccounts.ValidationInputRowList.Add(new ValidateAndCreateAccountReq.ValidationInputRowList
                    {
                        Segment1 = combinacionContableCuenta2[0],
                        Segment2 = combinacionContableCuenta2[1],
                        Segment3 = combinacionContableCuenta2[2],
                        Segment4 = combinacionContableCuenta2[3],
                        Segment5 = combinacionContableCuenta2[4],
                        Segment6 = combinacionContableCuenta2[5],
                        Segment7 = combinacionContableCuenta2[6],
                        Segment8 = combinacionContableCuenta2[7],
                        Segment9 = combinacionContableCuenta2[8],
                        LedgerName = tarifaConfiguracion.Nombre_libro_cuenta_cloud
                    });
                    validateAndCreateAccountCaseta = new AccountCombination().ValidateAndCreateAccount(vcarEnvelope2);
                    if (validateAndCreateAccountCaseta.Item1.Equals("Invalid"))
                        return new { Error = true, Mensaje = validateAndCreateAccountCaseta.Item2 };
                }

                var ctp = new CalculoTarifa
                {
                    Id_calculo_tarifa = idCalculoTarifaPorteador,
                    Id_tarifa = idTarifa,
                    Num_viaje = numViaje,
                    Pedido_venta = true,
                    Tranferencia = false,
                    Caseta = esCaseta,
                    Backhaul = esBackhaul,
                    Monto_transporte = Convert.ToDecimal(totalTransporte),
                    Monto_caseta = Convert.ToDecimal(totalCasetas),
                    Activo = true
                };
                ctp.Id_calculo_tarifa = new LOGI_CALCULOS_TARIFAS().GrabarCalculoTarifaPorteador(ctp);

                var cporEnvelope = new CreatePurchaseOrderReq.Envelope();
                var createOrderEntry = cporEnvelope.Body.CreatePurchaseOrder.CreateOrderEntry;
                createOrderEntry.ProcurementBusinessUnit = empresaConfiguracion.Clave_cloud;
                createOrderEntry.RequisitioningBusinessUnit = empresaConfiguracion.Clave_cloud;
                createOrderEntry.Supplier = proveedor;
                createOrderEntry.BuyerId = agentID.ToString();
                createOrderEntry.SupplierSiteCode = supplierSite;
                createOrderEntry.CurrencyCode = tarifaConfiguracion.Moneda;
                createOrderEntry.DocumentDescription = string.Format("VIAJE NO. {0}", numViaje);
                createOrderEntry.InterfaceSourceCode = "TARIFAS";
                createOrderEntry.ReferenceNumber = ctp.Id_calculo_tarifa.ToString();
                createOrderEntry.PurchaseOrderEntryLine.Add(new CreatePurchaseOrderReq.PurchaseOrderEntryLine
                {
                    ItemNumber = tarifaConfiguracion.Clave_articulo_tarifa,
                    ItemDescription = string.Format("TRANSPORTE - VIAJE NO. {0}", numViaje),
                    UnitOfMeasure = tarifaConfiguracion.Udm,
                    Quantity = new CreatePurchaseOrderReq.Quantity { UnitCode = tarifaConfiguracion.Udm, Text = "1" },
                    Price = new CreatePurchaseOrderReq.Price { CurrencyCode = tarifaConfiguracion.Moneda, Text = totalTransporte },
                    PurchaseOrderEntrySchedule = new List<CreatePurchaseOrderReq.PurchaseOrderEntrySchedule>
                    {
                        new CreatePurchaseOrderReq.PurchaseOrderEntrySchedule
                        {
                            ShipToOrganizationCode = almacenOrigen.CodigoAlmacen,
                            ShipToLocationCode = locationCodeAndLegalEntityID.Item1,
                            PurchaseOrderEntryDistribution = new List<CreatePurchaseOrderReq.PurchaseOrderEntryDistribution>
                            {
                                new CreatePurchaseOrderReq.PurchaseOrderEntryDistribution
                                {
                                    BudgetDate = dtFechaViaje.ToString("yyyy-MM-dd"), //DateTime.Now.Date.ToString("yyyy-MM-dd"),
                                    POChargeAccountId = validateAndCreatAccount.Item3,
                                    RequesterEmail = usuarioSesion.Usuario.Email
                                }
                            }
                        }
                    },
                    LineFlexfield = new List<CreatePurchaseOrderReq.LineFlexfield>
                    {
                        new CreatePurchaseOrderReq.LineFlexfield
                        {
                            Clasificacion = "SERVICIOS",
                            //FlexContext = empresaConfiguracion.Clave_cloud,
                            //Viaje = string.Format("VIAJE NO. {0}", numViaje)
                        }
                    }
                });

                if (esCaseta)
                {
                    createOrderEntry.PurchaseOrderEntryLine.Add(new CreatePurchaseOrderReq.PurchaseOrderEntryLine
                    {
                        ItemNumber = tarifaConfiguracion.Clave_articulo_caseta,
                        ItemDescription = string.Format("CASETAS - VIAJE NO. {0}", numViaje),
                        UnitOfMeasure = tarifaConfiguracion.Udm,
                        Quantity = new CreatePurchaseOrderReq.Quantity { UnitCode = tarifaConfiguracion.Udm, Text = "1" },
                        Price = new CreatePurchaseOrderReq.Price { CurrencyCode = tarifaConfiguracion.Moneda, Text = totalCasetas },
                        PurchaseOrderEntrySchedule = new List<CreatePurchaseOrderReq.PurchaseOrderEntrySchedule>
                        {
                            new CreatePurchaseOrderReq.PurchaseOrderEntrySchedule
                            {
                                ShipToOrganizationCode = almacenOrigen.CodigoAlmacen,
                                ShipToLocationCode = locationCodeAndLegalEntityID.Item1,
                                PurchaseOrderEntryDistribution = new List<CreatePurchaseOrderReq.PurchaseOrderEntryDistribution>
                                {
                                    new CreatePurchaseOrderReq.PurchaseOrderEntryDistribution
                                    {
                                        BudgetDate = dtFechaViaje.ToString("yyyy-MM-dd"),
                                        POChargeAccountId = validateAndCreateAccountCaseta.Item3,
                                        RequesterEmail = usuarioSesion.Usuario.Email
                                    }
                                }
                            }
                        },
                        LineFlexfield = new List<CreatePurchaseOrderReq.LineFlexfield>
                        {
                            new CreatePurchaseOrderReq.LineFlexfield
                            {
                                Clasificacion = "SERVICIOS",
                                //FlexContext = empresaConfiguracion.Clave_cloud,
                                //Viaje = string.Format("VIAJE NO. {0}", numViaje)
                            }
                        }
                    });
                }

                var result = new PurchaseOrder().CrearOrdenCompraCloud(cporEnvelope);
                ctp.Id_orden_compra = Convert.ToDecimal(result.Item1);
                ctp.Folio_orden_compra = result.Item2;
                new LOGI_CALCULOS_TARIFAS().GrabarCalculoTarifaPorteador(ctp);
                return new { Error = false, Mensaje = string.Format("La orden de compra con folio {0} se creo con exito en Oracle Cloud.", result.Item2) };
            }
            catch (Exception exception)
            {
                return new { Error = true, Mensaje = exception.Message };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object EstablecerCalculoTarifaPorteador(string numViaje)
        {
            try
            {
                return new LOGI_CALCULOS_TARIFAS().ObtenerCalculosTarifasPorteadores(numViaje, true, false).FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static void EstablecerModalViajes(string fechaInicio, string fechaFin)
        {
            try
            {
                DateTime fi = Convert.ToDateTime(fechaInicio);
                DateTime ff = Convert.ToDateTime(fechaFin);
                var parametro = new ParametroGridView();
                //campos a visualizar
                parametro.Campos.Add(new Campo("IDViaje", true));
                parametro.Campos.Add(new Campo("Folio Viaje", "FolioViaje", 100, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Descripcion", "Descripcion", 200, typeof(string), HorizontalAlign.Left));
                parametro.Campos.Add(new Campo("Fecha Envio", "FechaEnvio", 100, typeof(DateTime), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Nombre Almacen", "NombreAlmacenOracle", 200, typeof(string), HorizontalAlign.Left));
                parametro.Campos.Add(new Campo("Proveedor", "ProveedorOracle", 300, typeof(string), HorizontalAlign.Left));
                parametro.Campos.Add(new Campo("Folio OC", "Folio_OC", 100, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Inf. Completa", "InformacionCompleta", 70, typeof(bool), HorizontalAlign.Center));
                //fuente de datos
                parametro.DataSource = new LOGIViaje().ObtenerViajes(null, "ENTREGA A CLIENTE", null, null, Convert.ToDecimal(HttpContext.Current.Session["EmpresaID"]), null, null, true, fi, ff, true)
                    .Where(x => x.TipoAlmacenOracle != "REPRESENTANTE")
                    .ToList();
                //configuraciones
                parametro.FunctionCallback = "establecerViaje";
                parametro.PageSize = 20;
                parametro.NumericButtonCount = 12;
                //guarda configuración de parametros
                parametro.EstablecerParametros();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void LLenarDropDownList(DropDownList dropDownList, object dataSource, string valueField, string textField, bool addZeroValue = true)
        {
            dropDownList.DataSource = dataSource;
            dropDownList.DataValueField = valueField;
            dropDownList.DataTextField = textField;
            dropDownList.DataBind();
            if (addZeroValue)
                dropDownList.Items.Insert(0, new ListItem() { Text = "..:: Seleccione ::..", Value = "0" });
        }

        [WebMethod(EnableSession = true)]
        public static object ObtenerCiudades(decimal idMunicipio)
        {
            try
            {
                return new GPO_CIUDADES().ObtenerCiudades(null, null, null, idMunicipio);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static object ObtenerMunicipios(decimal idEstado)
        {
            try
            {
                return new GPO_MUNICIPIOS().ObtenerMunicipios(null, null, null, idEstado);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion
    }
}