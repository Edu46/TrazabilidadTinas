using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogisticaERP.Clases;
using LogisticaERP.Controles.BusquedaGenerica;
using System.Web.Services;
using LogisticaERP.LogisticaSOA;
using DevExpress.Web;
using System.Text;
using LogisticaERP.SeguridadERPSOA;
using Newtonsoft.Json;
using LogisticaERP.Clases.RecepcionarASN;
using LogisticaERP.Clases.RecepcionarASN.OracleCloud;
using LogisticaERP.Clases.RecepcionarASN.OracleCloud.DTO;
using LogisticaERP.Clases.RecepcionarASN.OracleEBS12;

namespace LogisticaERP.Catalogos.OracleCloud
{
    public partial class RecepcionarASN : PaginaBase
    {
        private const string _sessionGVASN = "LogisticaERP>Catalogos>OracleCloud>RecepcionarASN>GVASN";
        private const string _sessionGVEnvios = "LogisticaERP>Catalogos>OracleCloud>RecepcionarASN>GVEnvios";
        private const string _sessionGVEnvioEntrega = "LogisticaERP>Catalogos>OracleCloud>RecepcionarASN>GVEnvioEntregas";
        private const string _sessionGVValidacionLTEA = "LogisticaERP>Catalogos>OracleCloud>RecepcionarASN>GVValidacionLTEA";
        private const string _sessionGVValidacionLEC = "LogisticaERP>Catalogos>OracleCloud>RecepcionarASN>GVValidacionLEC";
        private const string _sessionAlmacenesCloud = "LogisticaERP>Catalogos>OracleCloud>RecepcionarASN>GVValidacionLEC";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // valida si hay una empresa seleccionada
                if (Session["EmpresaID"] == null || string.IsNullOrEmpty(Session["EmpresaID"].ToString()))
                {
                    hfEmpresaCloudException.Value = "No existe una empresa seleccionada, sera redireccionado a la pagina de Inicio!";
                }
                else
                {
                    decimal idEmpresaSIP = Convert.ToDecimal(Session["EmpresaID"]);
                    var empresaIntegracion = new GPO_EMPRESAS_INTEGRACIONES().ObtenerEmpresasIntegraciones(null, idEmpresaSIP, null, null, null, null, null).FirstOrDefault();
                    // valida si la empresa actual cuenta con un mapeo en empresas integraciones para oracle cloud
                    if (empresaIntegracion == null || !empresaIntegracion.Id_empresa_eb12.HasValue)
                        hfEmpresaCloudException.Value = "La Empresa seleccionada no cuenta con un mapeo en Empresas Integraciones, favor de comunicarlo al departamento de TI-Aplicaciones!";
                    else
                    {
                        hfIdEmpresaEBS12.Value = empresaIntegracion.Id_empresa_eb12.Value.ToString();
                        var organizations = new INVOrganization().ObtenerOrganizaciones(empresaIntegracion.Id_empresa_cloud.Value)
                            .GroupBy(x => new { x.OrganizationCode })
                            .Select(x => new { x.Key.OrganizationCode })
                            .ToList();

                        LLenarDropDownList(ddlAlmacenOrigen, organizations, "OrganizationCode", "OrganizationCode", true);
                        LLenarDropDownList(ddlAlmacenDestino, organizations, "OrganizationCode", "OrganizationCode", true);
                        HttpContext.Current.Session[_sessionGVASN] = null;
                        HttpContext.Current.Session[_sessionGVEnvios] = null;
                        HttpContext.Current.Session[_sessionGVEnvioEntrega] = null;
                        HttpContext.Current.Session[_sessionGVValidacionLEC] = null;
                        HttpContext.Current.Session[_sessionGVValidacionLTEA] = null;
                    }
                }
            }

            LLenarGridView();
        }

        protected void GVValidacionLTEA_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data && e.Row.ID != null)
            {
                var rowGV = (HttpContext.Current.Session[_sessionGVValidacionLTEA] as List<GridViewValidacion>).FirstOrDefault(x => x.ASNLineNumber == Convert.ToInt32(e.GetValue("ASNLineNumber")));
                if (rowGV.ASNItemNumber == rowGV.ShipmentItemNumber)
                {
                    if (rowGV.ASNQuantity == rowGV.ShipmentQuantity)
                        e.Row.Cells[0].Text = "<i class='fa fa-check' style='font-size: 18px;color: limegreen;cursor: pointer;'title='El SKU (ASN) cubre la cantidad solicitada en el envio'></i>";
                    else if (rowGV.ASNQuantity < rowGV.ShipmentQuantity)
                        e.Row.Cells[0].Text = "<i class='fa fa-adjust' style='font-size: 18px;color: limegreen;cursor: pointer;' title='El SKU (ASN) cubre parcialmente la cantidad solicitada en el envio'></i>";
                    else
                        e.Row.Cells[0].Text = "<i class='fa fa-times' style='font-size: 18px;color: red;cursor: pointer;' title='La cantidad del SKU (ASN) supera la cantidad solicitada en el envio'></i>";
                }
                else
                {
                    if (!string.IsNullOrEmpty(rowGV.ShipmentItemNumber) && string.IsNullOrEmpty(rowGV.ASNItemNumber))
                        e.Row.Cells[0].Text = "<i class='fa fa-clock-o' style='font-size: 18px;color: #FF9800;cursor: pointer;' title='El SKU (envio) se encuentra en estado pendiente por lotear'></i>";
                    else
                        e.Row.Cells[0].Text = "<i class='fa fa-times' style='font-size: 18px;color: red;cursor: pointer;' title='El SKU (ASN) no existe en el envio'></i>";
                }
            }
        }

        protected void GVValidacionLEC_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data && e.Row.ID != null)
            {
                var rowGV = (HttpContext.Current.Session[_sessionGVValidacionLEC] as List<GridViewValidacion>).FirstOrDefault(x => x.ASNLineNumber == Convert.ToInt32(e.GetValue("ASNLineNumber")));
                if (rowGV.ASNItemNumber == rowGV.ShipmentItemNumber && rowGV.ASNQuantity == rowGV.ShipmentQuantity)
                {
                    e.Row.Cells[0].Text = "<i class='fa fa-check' style='font-size: 18px;color: limegreen;cursor: pointer;'></i>";
                }
                else
                {
                    e.Row.Cells[0].Text = "<i class='fa fa-times' style='font-size: 18px;color: red;cursor: pointer;'></i>";
                }
            }
        }

        #endregion

        #region Methods

        [WebMethod(EnableSession = true)]
        public static void ActualizarASNEstadoDocumento(string noASN, decimal idEstadoDocumento)
        {
            try
            {
                var usuarioSesion = Newtonsoft.Json.JsonConvert.DeserializeObject<UsuarioSesion>(System.Web.HttpContext.Current.Session["UsuarioSesionSerializado"].ToString());
                new LOGASN().ActualizarASNEstadoDocumento(noASN, idEstadoDocumento, null, usuarioSesion.Usuario.Clave_usuario);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static void EstablecerGVASN(decimal idASN)
        {
            try
            {
                HttpContext.Current.Session[_sessionGVASN] = idASN > 0 ?
                    new LOGASN().ObtenerASNMaestroDetalle(idASN, null, null).FirstOrDefault().ASNDetalles.ToList() : null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static void EstablecerGVValidacionLEC(decimal noEnvio)
        {
            try
            {
                if (noEnvio == 0)
                {
                    HttpContext.Current.Session[_sessionGVEnvios] = null;
                    HttpContext.Current.Session[_sessionGVValidacionLEC] = null;
                    return;
                }

                int count = 1;
                var gridViewValidacion = (HttpContext.Current.Session[_sessionGVASN] as List<ASNDetalle>)
                    .GroupBy(x => new { x.CodProducto, x.NoProducto })
                    .Select(x => new GridViewValidacion
                    {
                        ASNLineNumber = count++,
                        ASNItemNumber = x.Key.CodProducto,
                        ASNItemDescription = x.Key.NoProducto,
                        ASNQuantity = x.Sum(y => y.Cantidad.Value)
                    })
                    .ToList();

                var envios = (HttpContext.Current.Session[_sessionGVEnvios] as List<SaleOrderDelivery>)
                    .Where(x => x.DeliveryID == noEnvio)
                    .ToList();


                envios.ForEach(x =>
                {
                    var item = gridViewValidacion.FirstOrDefault(y => y.ASNItemNumber == x.OrderedItem);
                    if (item != null)
                    {
                        item.ShipmentItemNumber = x.OrderedItem;
                        item.ShipmentItemDescription = x.OrderedItemDescription;
                        item.ShipmentQuantity = Convert.ToDecimal(x.OrderedQuantity);
                    }
                    else
                    {
                        gridViewValidacion.Add(new GridViewValidacion
                        {
                            ASNLineNumber = count++,
                            ShipmentItemNumber = x.OrderedItem,
                            ShipmentItemDescription = x.OrderedItemDescription,
                            ShipmentQuantity = Convert.ToDecimal(x.OrderedQuantity)
                        });
                    }
                });

                HttpContext.Current.Session[_sessionGVValidacionLEC] = gridViewValidacion;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static void EstablecerGVValidacionLTEA(string noASN, string noEnvio)
        {
            try
            {
                // si no se recibe el Número de Envio, se limpian las sesiones
                if (string.IsNullOrEmpty(noEnvio))
                {
                    HttpContext.Current.Session[_sessionGVEnvios] = null;
                    HttpContext.Current.Session[_sessionGVEnvioEntrega] = null;
                    HttpContext.Current.Session[_sessionGVValidacionLTEA] = null;
                    return;
                }

                // Se inicializar el gridview de validación con el ASN agrupado por sku
                int count = 1;
                var asnDetalles = (HttpContext.Current.Session[_sessionGVASN] as List<ASNDetalle>);
                var gridViewValidacion = asnDetalles
                    .GroupBy(x => new { x.CodProducto, x.NoProducto })
                    .OrderBy(x => x.Key.CodProducto)
                    .Select(x => new GridViewValidacion
                    {
                        ASNLineNumber = count++,
                        ASNItemNumber = x.Key.CodProducto,
                        ASNItemDescription = x.Key.NoProducto,
                        ASNQuantity = x.Sum(y => y.Cantidad.Value)
                    })
                    .ToList();

                // Se consultan todos los ASN's que se hayan asignado previamente al envio
                var asns = new LOGASN().ObtenerASNMaestroDetalle(null, null, noEnvio);
                // Se consulta si existe el asn con el que se esta trabajando,
                // de ser verdadero, se remueve de la lista de asn's
                var asn = asns.FirstOrDefault(x => x.NoASN == noASN);
                if (asn != null)
                    asns.Remove(asn);
                
                // Se obtienen todas las distribuciones de los asn's para trabajar comodamente
                // solo se añaden aquellos que no han sido procesados en oracle cloud
                var asnsDetallesDistribuciones = new List<ASNDetalleDistribucion>();
                if (asns != null)
                {
                    asns.ForEach(x =>
                        x.ASNDetalles.ToList().ForEach(y =>
                        {
                            var add = y.ASNDetallesDistribuciones.Where(z => !z.FlagProcesado);
                            asnsDetallesDistribuciones.AddRange(add);
                        })
                    );
                }

                // Se disminuye del envio las cantidades ya loteadas previamente por otros asn's
                // y remueve las lineas que con cantidad igual a 0
                var envioEntregas = (HttpContext.Current.Session[_sessionGVEnvios] as List<PickSlipPICO>)
                    .Where(x => x.SalesOrderNumber == noEnvio)
                    .Select(x =>
                    {
                        var CloudPickedQuantity = asnsDetallesDistribuciones
                            .Where(y => y.CloudPickSlipID == Convert.ToDecimal(x.PickSlipID)
                                && y.CloudPickSlipLine == Convert.ToDecimal(x.LineNumber)
                                && y.CodProducto == x.ItemNumber)
                            .Sum(y => y.CloudPickedQuantity);

                        x.Quantity = (Convert.ToDecimal(x.Quantity) - CloudPickedQuantity).ToString();
                        return x;
                    })
                    .Where(x => Convert.ToDecimal(x.Quantity) > 0)
                    .ToList();

                // Se almacenan los pick slips en sesión para su uso een la operación LotearTransferenciaEntreAlmacen
                HttpContext.Current.Session[_sessionGVEnvioEntrega] = envioEntregas;

                // Se agrupa las lineas del envio(pick slips) para cotejar en el gridview de validación
                var envioEntregasGB = envioEntregas.GroupBy(x => new { x.ItemNumber, x.ItemDescription })
                    .Select(x => new
                    {
                        ShipmentItemNumber = x.Key.ItemNumber,
                        ShipmentItemDescription = x.Key.ItemDescription,
                        ShipmentQuantity = x.Sum(y => Convert.ToDecimal(y.Quantity))
                    })
                    .ToList();

                // se empatan los artículos del ASN contra los artículos del envio
                // se concatenan los pick slips en los que se puede encontrar un mismo artículo
                envioEntregasGB.ForEach(x =>
                {
                    var item = gridViewValidacion.FirstOrDefault(y => y.ASNItemNumber == x.ShipmentItemNumber);
                    if (item != null)
                    {
                        item.ShipmentItemNumber = x.ShipmentItemNumber;
                        item.ShipmentItemDescription = x.ShipmentItemDescription;
                        item.ShipmentQuantity = x.ShipmentQuantity;
                    }
                    else
                    {
                        gridViewValidacion.Add(new GridViewValidacion
                        {
                            ASNLineNumber = count++,
                            ShipmentItemNumber = x.ShipmentItemNumber,
                            ShipmentItemDescription = x.ShipmentItemDescription,
                            ShipmentQuantity = x.ShipmentQuantity
                        });
                    }
                });

                // se valida si el ASN puede ser asignado al envio
                // se toma en consideración las siguientes validaciones:
                // 1) Debe existir el SKU del ASN en el envio
                // 2) Debe sers la cantidad de un SKU del ASN menor igual a la cantidad del SKU del envio
                // 3) Puede no existir en el ASN artículos que si existen en el envio, pero que seran loteados por otro ASN
                gridViewValidacion.ForEach(x =>
                {
                    if (x.ASNItemNumber == x.ShipmentItemNumber)
                    {
                        if (x.ASNQuantity <= x.ShipmentQuantity)
                            x.EnumEstadoLinea = EstadoLinea.VALIDA;
                        else
                            x.EnumEstadoLinea = EstadoLinea.NO_VALIDA;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(x.ShipmentItemNumber) && string.IsNullOrEmpty(x.ASNItemNumber))
                            x.EnumEstadoLinea = EstadoLinea.PENDIENTE_LOTEAR;
                        else
                            x.EnumEstadoLinea = EstadoLinea.NO_VALIDA;
                    }
                });

                HttpContext.Current.Session[_sessionGVValidacionLTEA] = gridViewValidacion;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static void EstablecerModalASN()
        {
            try
            {
                var parametro = new ParametroGridView();
                //campos a visualizar
                parametro.Campos.Add(new Campo("IdASN", true));
                parametro.Campos.Add(new Campo("No. ASN", "NoASN", true, 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Fecha ASN", "FechaASN", true, 200, typeof(string), HorizontalAlign.Left));
                parametro.Campos.Add(new Campo("No. Factura", "NoFactura", true, 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("FechaFactura", "FechaFactura", true, 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("RFC Proveedor", "RFCProveedor", true, 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Proveedor", "NombreProveedor", true, 200, typeof(string), HorizontalAlign.Left));
                parametro.Campos.Add(new Campo("Estado ASN", "EstadoDocumento", true, 200, typeof(string), HorizontalAlign.Center));
                //fuente de datos
                parametro.DataSource = new LOGASN().ObtenerASNs();
                //configuraciones
                parametro.FunctionCallback = "establecerASN";
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
        public static void EstablecerModalEnvioLEC(decimal idEmpresaEBS12)
        {
            try
            {
                var parametro = new ParametroGridView();
                //campos a visualizar
                parametro.Campos.Add(new Campo("DeliveryID", true));
                parametro.Campos.Add(new Campo("Fecha Creación", "CreationDate", true, 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("No. Pedido Venta", "SalesOrderNumber", true, 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("No. Envio", "DeliveryID", true, 200, typeof(string), HorizontalAlign.Center));
                //fuente de datos
                var envios = new OMSaleOrder().ObtenerPedidosVentasEnvios(idEmpresaEBS12);
                HttpContext.Current.Session[_sessionGVEnvios] = envios;
                parametro.DataSource = envios
                    .GroupBy(x => new { x.OrderedDate, x.OrderNumber, x.DeliveryID, x.FromSubinventoryCode })
                    .Select(x => new GridViewEnvioModal
                    {
                        CreationDate = x.Key.OrderedDate,
                        SalesOrderNumber = x.Key.OrderNumber,
                        DeliveryID = x.Key.DeliveryID,
                        FromSubinventoryCode = x.Key.FromSubinventoryCode                        
                    })
                    .ToList();
                //configuraciones
                parametro.FunctionCallback = "establecerGVValidacionLEC";
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
        public static void EstablecerModalEnvioLTEA(string almacenOrigen, string almacenDestino)
        {
            try
            {
                var parametro = new ParametroGridView();
                //campos a visualizar
                parametro.Campos.Add(new Campo("SalesOrderNumber", true));
                parametro.Campos.Add(new Campo("Fecha Creación", "CreationDate", true, 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("No. Envio", "SalesOrderNumber", true, 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Almacen Origen", "SourceOrganizationCode", true, 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Almacen Destino", "DestinationOrganizationCode", true, 200, typeof(string), HorizontalAlign.Center));
                //fuente de datos
                var envios = new INVPickSlipPICO().ObtenerEnviosDetalles(almacenOrigen, almacenDestino);
                HttpContext.Current.Session[_sessionGVEnvios] = envios;
                parametro.DataSource = envios
                    .GroupBy(x => new { x.CreationDate, x.SalesOrderNumber, x.SourceOrganizationCode, x.DestinationOrganizationCode })
                    .Select(x => new PickSlipPICO
                    {
                        CreationDate = x.Key.CreationDate,
                        SalesOrderNumber = x.Key.SalesOrderNumber,
                        SourceOrganizationCode = x.Key.SourceOrganizationCode,
                        DestinationOrganizationCode = x.Key.DestinationOrganizationCode
                    })
                    .ToList();
                //configuraciones
                parametro.FunctionCallback = "establecerGVValidacionLTEA";
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

        [WebMethod]
        public static bool ExistenLineasIncompletasLTEA()
        {
            try
            {
                bool existenLineasIncompletas = (HttpContext.Current.Session[_sessionGVValidacionLTEA] as List<GridViewValidacion>)
                    .Exists(x => x.ASNQuantity < x.ShipmentQuantity && x.ASNQuantity > 0);
                return existenLineasIncompletas;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static void LotearEntregaACliente(string noASN, string noEnvio)
        {
            try
            {
                var usuarioSesion = Newtonsoft.Json.JsonConvert.DeserializeObject<UsuarioSesion>(System.Web.HttpContext.Current.Session["UsuarioSesionSerializado"].ToString());
                var validaciones = (HttpContext.Current.Session[_sessionGVValidacionLEC] as List<GridViewValidacion>);
                if (validaciones.Exists(x => x.ASNItemNumber != x.ShipmentItemNumber || x.ASNQuantity != x.ShipmentQuantity))
                {
                    throw new Exception("El envio seleccionado no empata con la información del ASN, favor de revisar.");
                }

                new LOGASN().ActualizarASNEstadoDocumento(noASN, 4, noEnvio, usuarioSesion.Usuario.Clave_usuario);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static object LotearTransferenciaEntreAlmacen(string noASN, string noEnvio, bool esLoteo, string almacenCloud)
        {
            try
            {
                var validaciones = (HttpContext.Current.Session[_sessionGVValidacionLTEA] as List<GridViewValidacion>);
                if (validaciones.Exists(x => x.EnumEstadoLinea == EstadoLinea.NO_VALIDA))
                    return new { Error = true, Mensaje = "El envio seleccionado no empata con la información del ASN, favor de revisar." };

                // Se obtienen todos los sku´s que pasaron los filtros de validación
                var skusValidos = validaciones.Where(x => x.EnumEstadoLinea == EstadoLinea.VALIDA).ToList();
                // Se obtienen las lineas del asn de los sku´s que pasaron los filtros de validación validación
                var asnDetalles = (HttpContext.Current.Session[_sessionGVASN] as List<ASNDetalle>)
                    .Where(x => skusValidos.Select(y => y.ASNItemNumber).Contains(x.CodProducto))
                    .ToList();
                // Se obtienen las lineas del envio que aun no se han loteado aun
                var envioEntregas = (HttpContext.Current.Session[_sessionGVEnvioEntrega] as List<PickSlipPICO>);

                var asnDetallesDistribuciones = new List<ASNDetalleDistribucion>();
                while (asnDetalles.Any(x => x.Cantidad > 0))
                {
                    foreach (var ad in asnDetalles.Where(x => x.Cantidad > 0))
                    {
                        var ee = envioEntregas.FirstOrDefault(x => x.ItemNumber == ad.CodProducto && Convert.ToDecimal(x.Quantity) > 0);
                        var asnDetalleDistribucion = new ASNDetalleDistribucion
                        {
                            IdASNDetalleDistribucion = 0,
                            IdASNDetalle = ad.IdASNDetalle,
                            CloudDeliveryID = Convert.ToDecimal(ee.SalesOrderNumber),
                            CloudPickSlipID = Convert.ToDecimal(ee.PickSlipID),
                            CloudSubinventoryCode = ee.SourceSubinventoryCode,
                            CloudPickSlipLine = Convert.ToDecimal(ee.LineNumber),
                            FlagProcesado = false,
                            Activo = true,
                            Borrado = false
                        };
                        asnDetallesDistribuciones.Add(asnDetalleDistribucion);


                        if (ad.Cantidad >= Convert.ToDecimal(ee.Quantity))
                        {

                            asnDetalleDistribucion.CloudPickedQuantity = Convert.ToDecimal(ee.Quantity);
                            ad.Cantidad -= Convert.ToDecimal(ee.Quantity);
                            ee.Quantity = "0";
                        }
                        else
                        {
                            asnDetalleDistribucion.CloudPickedQuantity = ad.Cantidad.Value;
                            ee.Quantity = (Convert.ToDecimal(ee.Quantity) - ad.Cantidad).ToString();
                            ad.Cantidad = 0;
                        }
                    }
                }

                var sb = new StringBuilder();
                // Se guarda la asignación de lotes en las lineas del ASN
                var logASN = new LOGASN();
                var logASNDetalleDistribucion = new LOGASNDetalleDistribucion();


                //falta actualizar la operación siguiente para que guarde en el encabezado el numero de envio al que se asigno el asn.
                // esto para encontrarlo en la busqueda para el loteo y envio a cloud


                logASNDetalleDistribucion.GuardarASNDetalleDistribucion(noASN, noEnvio, asnDetallesDistribuciones);
                // Si esLoteo es verdadero, se lotea la transferencia entre almacen en oracle cloud
                if (esLoteo)
                {
                    // Se obtienen todos los detalles de los ASN relacionados al envio
                    var asns = logASN.ObtenerASNMaestroDetalle(null, noASN, null);
                    // Se obtienen todas las distribuciones de los asn's para trabajar comodamente
                    // solo se añaden aquellos que no han sido procesados en oracle cloud
                    var asnsDetallesDistribuciones = new List<ASNDetalleDistribucion>();
                    if (asns != null)
                    {
                        asns.ForEach(x =>
                            x.ASNDetalles.ToList().ForEach(y =>
                            {
                                var add = y.ASNDetallesDistribuciones.Where(z => !z.FlagProcesado);
                                asnsDetallesDistribuciones.AddRange(add);
                            })
                        );
                    }

                    //Eliminar configuracion de almacen **CLOUD                    
                    INVENTORY_ORGANIZATION_CLOUD org = new INVENTORY_ORGANIZATION_CLOUD();

                    decimal? idOrganizacion = org.GetOrganization(almacenCloud);

                    org.invOrganization.invOrgParameters.Add(new INVENTORY_ORGANIZATION_CLOUD.invOrg
                    {
                        AllowNegativeOnhandTransactionsFlag = "Y",
                        NegativeInvReceiptFlag = true
                    });
                
                    if (!org.ActualizarAlmacen(org.invOrganization , idOrganizacion.HasValue ? idOrganizacion.Value : 0))
                        return new { Error = false, Mensaje = "Se encontrado un error al actualiza configuracón del almacen." };


                    // Agrupamos los detalles del ASN por pick slipt para iteralos
                    foreach (var pickSlipID in asnsDetallesDistribuciones.GroupBy(x => x.CloudPickSlipID).Select(x => x.Key))
                    {
                        var pickTransactions = new PickTransaction();
                        // Se agrupa los detalles del ASN pick slip por linea, cantidad y subinventario
                        foreach (var asnDistGB in asnsDetallesDistribuciones
                            .Where(x => x.CloudPickSlipID == pickSlipID)
                            .GroupBy(x => new { x.CloudSubinventoryCode, x.CloudPickSlipLine })
                            .Select(x => new { x.Key.CloudSubinventoryCode, x.Key.CloudPickSlipLine, CloudPickedQuantity = x.Sum(y => y.CloudPickedQuantity) }))
                        {
                            // Se crea el objecto pickLine para la API de oracle cloud
                            var pl = new PickLines();
                            pl.PickSlip = Convert.ToInt64(pickSlipID);
                            pl.PickSlipLine = Convert.ToInt64(asnDistGB.CloudPickSlipLine);
                            pl.PickedQuantity = Convert.ToDecimal(asnDistGB.CloudPickedQuantity);
                            pl.SubinventoryCode = asnDistGB.CloudSubinventoryCode;

                            // Se crea un objeto de tipo LotItemLots y se asocia al objeto PickLIne
                            // (asignación de lotes por pick slip)
                            foreach (var asnDist in asnsDetallesDistribuciones.Where(x => x.CloudPickSlipID == pickSlipID && x.CloudPickSlipLine == asnDistGB.CloudPickSlipLine))
                            {
                                pl.lotItemLots.Add(new LotItemLots
                                {
                                    Lot = string.Format("{0}#{1}", asnDist.Lote, asnDist.NoPallet),
                                    Quantity = asnDist.CloudPickedQuantity
                                });
                                asnDist.FlagProcesado = true;
                            }

                            pickTransactions.pickLines.Add(pl);
                        }

                        // Se envia a lotear el pick slip armardo
                        var invPT = new INVPickTransaction().CreatePickTransactions(pickTransactions);
                        if (invPT.ReturnStatus.Equals("Error"))
                        {
                            sb.AppendLine(string.Format("Estado: {0}, Codigo Error: {1}, Excepción: {2}, Nota Seleccion: {3}, Linea Nota Seleccion: {4}",
                                invPT.ReturnStatus, invPT.ErrorCode, invPT.ErrorExplanation, invPT.PickSlip, invPT.PickSlipLine));
                        }
                        else
                        {
                            // Si no hubo error en el loteo oracle cloud
                            // 1) Se actualizan todas las lineas de ASN como FlagProcesado = 1
                            logASNDetalleDistribucion.GuardarASNDetalleDistribucion(null, null, asnsDetallesDistribuciones.Where(x => x.CloudPickSlipID == pickSlipID).ToList());
                            // 2) Si un envio se compuso de mas de un ASN
                            //    Se agrupa los detalles del ASN por Número de ASN y se iteran para su actualización de estado
                            var usuarioSesion = JsonConvert.DeserializeObject<UsuarioSesion>(HttpContext.Current.Session["UsuarioSesionSerializado"].ToString());
                            foreach (var asn in asnsDetallesDistribuciones.GroupBy(x => x.NoASN))
                                logASN.ActualizarASNEstadoDocumento(asn.Key, 6, null, usuarioSesion.Usuario.Clave_usuario);
                        }
                    }
                    

                    INVENTORY_ORGANIZATION_CLOUD orgF = new INVENTORY_ORGANIZATION_CLOUD();

                    orgF.invOrganization.invOrgParameters.Add(new INVENTORY_ORGANIZATION_CLOUD.invOrg
                    {
                        AllowNegativeOnhandTransactionsFlag = "N",
                        NegativeInvReceiptFlag = false
                    });

                    if (!org.ActualizarAlmacen(orgF.invOrganization, idOrganizacion.HasValue ? idOrganizacion.Value : 0))
                        return new { Error = false, Mensaje = "Se encontrado un error al actualiza configuracón del almacen." };


                    if (string.IsNullOrEmpty(sb.ToString()))
                        return new { Error = false, Mensaje = "Se ha enviado el Loteo de Transferencia Entre Almacen con exito." };
                    else
                        return new { Error = true, Mensaje = sb.ToString() };
                }
                else
                {
                    return new { Error = false, Mensaje = "Se guardado temporalmente el loteo con exito." };
                }
            }
            catch (Exception exception)
            {
                return new { Error = true, Mensaje = exception.Message };
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

        private void LLenarGridView()
        {
            GVASN.DataSource = HttpContext.Current.Session[_sessionGVASN];
            GVASN.KeyFieldName = "IdASNDetalle";
            GVASN.DataBind();

            GVValidacionLTEA.DataSource = HttpContext.Current.Session[_sessionGVValidacionLTEA];
            GVValidacionLTEA.KeyFieldName = "ASNLineNumber";
            GVValidacionLTEA.DataBind();

            GVValidacionLEC.DataSource = HttpContext.Current.Session[_sessionGVValidacionLEC];
            GVValidacionLEC.KeyFieldName = "ASNLineNumber";
            GVValidacionLEC.DataBind();
        }

        #endregion

        #region Enums

        public enum EstadoLinea
        {
            VALIDA
            ,NO_VALIDA
            ,PENDIENTE_LOTEAR
        }

        #endregion

        #region Classes

        [Serializable]
        public class GridViewValidacion : ICloneable
        {
            public GridViewValidacion()
            {
                ASNLineNumber = 0;
                ASNItemNumber = string.Empty;
                ASNItemDescription = string.Empty;
                ASNQuantity = 0;
                ShipmentItemNumber = string.Empty;
                ShipmentItemDescription = string.Empty;
                ShipmentQuantity = 0;
                ShipmentPickSlipID = string.Empty;
            }

            public int ASNLineNumber { get; set; }
            public string ASNItemNumber { get; set; }
            public string ASNItemDescription { get; set; }
            public decimal ASNQuantity { get; set; }
            public string ShipmentItemNumber { get; set; }
            public string ShipmentItemDescription { get; set; }
            public decimal ShipmentQuantity { get; set; }
            public string ShipmentPickSlipID { get; set; }
            public EstadoLinea EnumEstadoLinea { get; set; }

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }

        [Serializable]
        public class GridViewEnvioModal
        {
            public string CreationDate { get; set; }
            public string SalesOrderNumber { get; set; }
            public decimal DeliveryID { get; set; }
            public string FromSubinventoryCode { get; set; }
        }

        #endregion

        protected void btnExportarInterno_Click(object sender, EventArgs e)
        {
            GVExportar.WriteXlsToResponse("ASN" + "_" + DateTime.Now, true);
        }

    }
}