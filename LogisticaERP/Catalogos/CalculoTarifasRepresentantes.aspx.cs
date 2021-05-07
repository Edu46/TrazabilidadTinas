using LogisticaERP.Clases;
using LogisticaERP.Clases.CalculoTarifaRepresentante;
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
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP.Catalogos
{
    public partial class CalculoTarifasRepresentantes : PaginaBase
    {
        private static readonly string _keyCTRGVEntregas = "LogisticaERP>Catalogos>CalculoTarifasRepresentantes>GVEntregas";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // se limpia la sesión del gridview de entregas
                HttpContext.Current.Session[_keyCTRGVEntregas] = new List<CalculoTarifaRepresentanteDetalle>();

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
                }
            }

            LLenarGVEntregas();
        }

        #endregion

        #region Events

        [WebMethod(EnableSession = true)]
        public static object GuardarCalculo(decimal idCalculoTarifaRepresentante, string numViaje, string proveedor, string totalCajasEstandar, string totalTransporte)
        {
            try
            {
                if ((HttpContext.Current.Session[_keyCTRGVEntregas] as List<CalculoTarifaRepresentanteDetalle>).Exists(x => !string.IsNullOrEmpty(x.Excepcion)))
                    return new { Error = true, Mensaje = "Existen entregas con excepciones, favor de resolverlas antes de guardar el Calculo." };

                var empresa = HttpContext.Current.Session["LOG_Empresa"] as Empresa;
                var ctrd = (HttpContext.Current.Session[_keyCTRGVEntregas] as List<CalculoTarifaRepresentanteDetalle>).FirstOrDefault();

                var logiCalculoTarifaRepresentante = new LOGICalculoTarifaRepresentante();
                idCalculoTarifaRepresentante = logiCalculoTarifaRepresentante.GuardarCalculoTarifaRepresentante(new CalculoTarifaRepresentante
                {
                    IDCalculoTarifaRepresentante = idCalculoTarifaRepresentante,
                    IDEmpresa = empresa.Id_empresa,
                    NumViaje = numViaje,
                    Almacen = ctrd.CodigoAlmacenEBS12,
                    Transportista = proveedor,
                    TotalCajasEstandar = Convert.ToDecimal(totalCajasEstandar),
                    TotalTransporte = Convert.ToDecimal(totalTransporte),
                    Activo = true,
                    Borrado = false,
                    CalculoTarifaRepresentanteDetalles = (HttpContext.Current.Session[_keyCTRGVEntregas] as List<CalculoTarifaRepresentanteDetalle>).ToArray()
                });

                return new { Error = false, Mensaje = string.Format("Se han guardado los datos con exito.") };
            }
            catch (Exception exception)
            {
                return new { Error = true, Mensaje = exception.Message };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object CrearOrdenCompraCloud(decimal idCalculoTarifaRepresentante, string numViaje, string fechaViaje, string proveedor, string totalCajasEstandar, string totalTransporte)
        {
            try
            {
                DateTime dtFechaViaje = Convert.ToDateTime(fechaViaje);

                if ((HttpContext.Current.Session[_keyCTRGVEntregas] as List<CalculoTarifaRepresentanteDetalle>).Exists(x => !string.IsNullOrEmpty(x.Excepcion)))
                    return new { Error = true, Mensaje = "Existen entregas con excepciones, favor de resolverlas antes de generar la Orden de Compra en Cloud." };

                var usuarioSesion = Newtonsoft.Json.JsonConvert.DeserializeObject<UsuarioSesion>(System.Web.HttpContext.Current.Session["UsuarioSesionSerializado"].ToString());
                var empresa = HttpContext.Current.Session["LOG_Empresa"] as Empresa;
                var empresaConfiguracion = new GPO_EMPRESAS_INTEGRACIONES().ObtenerEmpresasIntegraciones(null, empresa.Id_empresa, null, null, null, null, null).FirstOrDefault();
                var ctrd = (HttpContext.Current.Session[_keyCTRGVEntregas] as List<CalculoTarifaRepresentanteDetalle>).FirstOrDefault();
                var almacenOrigen = new INVOrganizationUnit().ObtenerAlmacenesParaTarifasDeViajes(Convert.ToInt64(empresaConfiguracion.Id_empresa_eb12.Value), 2)
                    .FirstOrDefault(x => x.IDAlmacen == ctrd.IDAlmacenEBS12);

                //Valida informacion completa el viaje.
                var viaje = new LOGIViaje().ObtenerViajes(null, null, null, numViaje, Convert.ToDecimal(HttpContext.Current.Session["EmpresaID"]), null, null, true, null, null, true).FirstOrDefault();

                if (string.IsNullOrEmpty(viaje.Chofer) || string.IsNullOrEmpty(viaje.Marca) || string.IsNullOrEmpty(viaje.Modelo) || string.IsNullOrEmpty(viaje.PlacaTractor) || string.IsNullOrEmpty(viaje.PlacaCaja)
                    || string.IsNullOrEmpty(viaje.NumeroSerieCaja) || string.IsNullOrEmpty(viaje.NumeroECOCaja) || string.IsNullOrEmpty(viaje.NumeroRuta) || string.IsNullOrEmpty(viaje.NumeroConvoy) || string.IsNullOrEmpty(viaje.NumeroSello)
                    || string.IsNullOrEmpty(viaje.NumeroCartaPorte) || viaje.IDProveedorOracle <= 0)
                {
                    return new { Error = true, Mensaje = "Es necesario capturar toda la información del Número de Viaje para poder continuar." };
                }


                if (almacenOrigen != null && string.IsNullOrEmpty(almacenOrigen.CentroCosto))
                    return new { Error = true, Mensaje = "El almacen no tiene un centro de costo asignado en Oracle EBS 12." };

                var tarifaConfiguracion = new LOGI_TARIFAS().ObtenerTarifaConfiguracion(null, empresa.Id_empresa, almacenOrigen.CentroCosto);

                Tuple<string, long> locationCodeAndLegalEntityID = new INVInventoryOrganization().GetLocationCodeAndLegalEntityID(ctrd.CodigoAlmacenEBS12);
                if(locationCodeAndLegalEntityID == null)
                    return new { Error = true, Mensaje = "El código de ubicación del almacen no se encontro en Oracle Cloud." };

                long supplierID = new POSupplier().GetSupplierID(proveedor);
                if(supplierID == 0)
                    return new { Error = true, Mensaje = "El proveedor no se encontro en Oracle Cloud." };

                string supplierSite = new POSupplierSite().GetSupplierSite(supplierID, empresaConfiguracion.Clave_cloud);
                if (string.IsNullOrEmpty(supplierSite))
                    return new { Error = true, Mensaje = "El sitio del proveedor no se encontro en Oracle Cloud." };

                long agentID = new POProcurementAgent().GetProcurementAgentID(usuarioSesion.Usuario.Email, Convert.ToInt64(empresaConfiguracion.Id_empresa_cloud));
                if (agentID == 0)
                    return new { Error = true, Mensaje = string.Format("El agente de compra no se encontro en Oracle Cloud (correo: {0}).", usuarioSesion.Usuario.Email) };

                string[] combinacionContableCuentaTransferencia = tarifaConfiguracion.Combinacion_pedido_venta.Split('.'); //tarifaConfiguracion.Combinacion_transferencia.Split('.');
                var vcarEnvelope = new ValidateAndCreateAccountReq.Envelope();
                vcarEnvelope.Body.ValidateAndCreateAccounts.ValidationInputRowList.Add(new ValidateAndCreateAccountReq.ValidationInputRowList
                {
                    Segment1 = combinacionContableCuentaTransferencia[0],
                    Segment2 = combinacionContableCuentaTransferencia[1],
                    Segment3 = combinacionContableCuentaTransferencia[2],
                    Segment4 = combinacionContableCuentaTransferencia[3],
                    Segment5 = combinacionContableCuentaTransferencia[4],
                    Segment6 = combinacionContableCuentaTransferencia[5],
                    Segment7 = combinacionContableCuentaTransferencia[6],
                    Segment8 = combinacionContableCuentaTransferencia[7],
                    Segment9 = combinacionContableCuentaTransferencia[8],
                    LedgerName = tarifaConfiguracion.Nombre_libro_cuenta_cloud
                });
                var validateAndCreatAccount = new AccountCombination().ValidateAndCreateAccount(vcarEnvelope);
                if(validateAndCreatAccount.Item1.Equals("Invalid"))
                    return new { Error = true, Mensaje = validateAndCreatAccount.Item2 };

                var logiCalculoTarifaRepresentante = new LOGICalculoTarifaRepresentante();
                idCalculoTarifaRepresentante = logiCalculoTarifaRepresentante.GuardarCalculoTarifaRepresentante(new CalculoTarifaRepresentante
                {
                    IDCalculoTarifaRepresentante = idCalculoTarifaRepresentante,
                    IDEmpresa = empresa.Id_empresa,
                    NumViaje = numViaje,
                    Almacen = ctrd.CodigoAlmacenEBS12,
                    Transportista = proveedor,
                    TotalCajasEstandar = Convert.ToDecimal(totalCajasEstandar),
                    TotalTransporte = Convert.ToDecimal(totalTransporte),
                    Activo = true,
                    Borrado = false,
                    CalculoTarifaRepresentanteDetalles = (HttpContext.Current.Session[_keyCTRGVEntregas] as List<CalculoTarifaRepresentanteDetalle>).ToArray()
                });

                

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
                createOrderEntry.ReferenceNumber = idCalculoTarifaRepresentante.ToString();
                createOrderEntry.PurchaseOrderEntryLine = new List<CreatePurchaseOrderReq.PurchaseOrderEntryLine>
                {
                    new CreatePurchaseOrderReq.PurchaseOrderEntryLine
                    {
                        ItemNumber = tarifaConfiguracion.Clave_articulo_tarifa,
                        ItemDescription = string.Format("VIAJE NO. {0}", numViaje),
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
                    }
                };

                var result = new PurchaseOrder().CrearOrdenCompraCloud(cporEnvelope);
                logiCalculoTarifaRepresentante.ActualizarCalculoTarifaRepresentanteOC(idCalculoTarifaRepresentante, Convert.ToDecimal(result.Item1), result.Item2);

                return new { Error = false, Mensaje = string.Format("La orden de compra con folio {0} se creo con exito en Oracle Cloud.", result.Item2), IDCalculo = idCalculoTarifaRepresentante };
            }
            catch (Exception exception)
            {
                return new { Error = true, Mensaje = exception.Message, Nuevo = true ,IDCalculo = idCalculoTarifaRepresentante };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object EstablecerGVEntrega(decimal idEmpresa, string numViaje)
        {
            try
            {
                var calculoTarifaRepresentante = new LOGICalculoTarifaRepresentante().ObtenerCalculoTarifaRepresentante(idEmpresa, numViaje, false);
                HttpContext.Current.Session[_keyCTRGVEntregas] = calculoTarifaRepresentante.CalculoTarifaRepresentanteDetalles.ToList();

                return new
                {
                    calculoTarifaRepresentante.TotalCajasEstandar,
                    calculoTarifaRepresentante.TotalTransporte,
                    calculoTarifaRepresentante.IDCalculoTarifaRepresentante,
                    calculoTarifaRepresentante.IDOrdenCompra,
                    calculoTarifaRepresentante.FolioOrdenCompra
                };
            }
            catch (Exception exception)
            {                
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static void EstablecerModalViajes()
        {
            try
            {
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
                parametro.DataSource = new LOGIViaje().ObtenerViajes(null, "ENTREGA A CLIENTE", null, null, Convert.ToDecimal(HttpContext.Current.Session["EmpresaID"]), null, "REPRESENTANTE", true, null, null, true);
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

        [WebMethod(EnableSession = true)]
        public static void LimpiarGVEntrega()
        {
            try
            {
                HttpContext.Current.Session[_keyCTRGVEntregas] = new List<CalculoTarifaRepresentanteDetalle>();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void LLenarGVEntregas()
        {
            GVEntrega.DataSource = (HttpContext.Current.Session[_keyCTRGVEntregas] as List<CalculoTarifaRepresentanteDetalle>).ToList();
            GVEntrega.KeyFieldName = "IDEntregaEBS12";
            GVEntrega.DataBind();
        }

        #endregion

        protected void btnExportarInterno_Click(object sender, EventArgs e)
        {
            GVExportar.WriteXlsToResponse("CALCULOREPRESENTANTE" + "_" + DateTime.Now, true);
        }
    }
}