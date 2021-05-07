using System;
using System.Web;
using System.Linq;
using System.Web.UI;
using LogisticaERP.Clases;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using LogisticaERP.GrupoPinsaSOA;
using System.Runtime.Serialization;
using LogisticaERP.LogisticaSOA;
using DevExpress.Web;
using System.ServiceModel;
using System.Web.Services;
using System.Globalization;
using LogisticaERP.Clases.Viajes;

namespace LogisticaERP.Catalogos
{
    public partial class CalculoTarifas : PaginaBase
	{


        protected void Page_Load(object sender, EventArgs e)
		{

			if (!IsPostBack)
			{
                Session["CTAR_Municipios"] = null;
                Session["CTAR_VIAJES"] = null;
                CargaInicial();
            }
            else
            {
               
            }

		}

        private void CargaInicial()
        {
            try
            {

                var estados = new GPO_ESTADOS().ObtenerEstados(null, null, null, 1);
                if (estados.Any())
                {
                    ddlEstado.DataSource = estados;
                    ddlEstado.DataValueField = "Id_estado";
                    ddlEstado.DataTextField = "Nombre";
                    ddlEstado.DataBind();
                }
                else 
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe Estados para el país Mexico.");

                var municipios = new GPO_MUNICIPIOS().ObtenerMunicipios();
                if (municipios.Any())
                    Session["CTAR_Municipios"] = municipios;
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe Municipios para el país Mexico.");

                var ciudades = new GPO_CIUDADES().ObtenerCiudades();

                if (ciudades.Any())
                    Session["CTAR_Ciudades"] = ciudades;
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe Ciudades para el país Mexico.");

                EBS12_ALMACENES almanceOrigen = new EBS12_ALMACENES();
                if (almanceOrigen.ObtenerAlmacenes(Convert.ToDecimal(Session["EmpresaID"]), 0)) 
                    Session["CTAR_Almacenes"] = almanceOrigen.Almacenes.items.ToList();
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, almanceOrigen.Almacenes.mensaje);

                LOGI_CODIGOS_TRANSPORTES codigo = new LOGI_CODIGOS_TRANSPORTES();
                codigo.CodigosTarifas.Id_empresa = Convert.ToDecimal(Session["EmpresaID"]);
                codigo.CodigosTarifas.Activo = true;

                if (codigo.Busqueda())
                {
                    ddlCodTransporte.DataSource = codigo.ListaCodigosTarifas;
                    ddlCodTransporte.DataValueField = "Id_codigo_transporte";
                    ddlCodTransporte.DataTextField = "Codigo_transporte";
                    ddlCodTransporte.DataBind();
                }
                   
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existen codigos de Transporte para la empresa seleccionada.");


            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, Faultexc.Detail.ExcDetalle.Mensaje);
            }
            catch (FaultException<GrupoPinsaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, Faultexc.Detail.ExcDetalle.Mensaje);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }
        }

        private void MostrarMensaje(ControladorMensajes.TipoMensaje tipo, dynamic sms, int? tiempoMostrar = 7000)
        {
            ControladorMensajes.MostrarMensaje(UpdatePanel1, string.Format("TAR_{0}{1}", tipo, new Random().Next(0, 99)), tipo, sms, tiempoMostrar);
        }

        protected void ddlMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCiudades(Convert.ToDecimal(ddlMunicipio.SelectedValue));
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMunicipios(Convert.ToDecimal(ddlEstado.SelectedValue));
        }

        protected void btnCargarInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Limpiar();

                LOGI_CALCULOS_TARIFAS calculo = new LOGI_CALCULOS_TARIFAS();
                calculo.CalculoTarifas.Num_viaje = txtNumViaje.Text;
                //calculo.CalculoTarifas.Pedido_venta =  rbPedidoVenta.Checked;
                calculo.CalculoTarifas.Tranferencia = true;//rbTransaferencia.Checked;

                if (calculo.Busqueda())
                {
                    CalculoTarifa tarifa = new CalculoTarifa();
                    tarifa = calculo.ListaCalculoTarifas.FirstOrDefault();

                    ddlEstado.SelectedValue = tarifa.Id_estado.ToString();
                    CargarMunicipios(tarifa.Id_estado);
                    ddlMunicipio.SelectedValue = tarifa.Id_municipio.ToString();
                    CargarCiudades(tarifa.Id_municipio);
                    ddlCiudad.SelectedValue = tarifa.Id_ciudad.ToString();
                    ddlTipoTarifa.SelectedValue = tarifa.Id_tipo_tarifa.ToString();
                    ddlCodTransporte.SelectedValue = tarifa.Id_codigo_transporte.ToString();
                    chkCasetas.Checked = tarifa.Caseta;
                    chkBackhaul.Checked = tarifa.Backhaul;
                    txtFolioOC.Text = tarifa.Folio_orden_compra;
                    txtMontoTransporte.Text = String.Format("{0:C4}", tarifa.Monto_transporte);
                    txtMontoCaseta.Text = String.Format("{0:C4}", tarifa.Monto_caseta);
                    hdfIdTarifa.Value = tarifa.Id_tarifa.ToString();
                    hdfIdCalculoTarifa.Value = tarifa.Id_calculo_tarifa.ToString();
                    

                    if (tarifa.Folio_orden_compra != null)
                    {
                        btnCalcular.Enabled = false;
                        btnOracle.Enabled = false;
                    }

                    if (tarifa.Tranferencia)
                    {
                        var viaje = new LOGIViaje().ObtenerViajes(null, "TRANSFERENCIA", null, txtNumViaje.Text, null, null, null, true, null, null, true).FirstOrDefault();


                        if (viaje != null)
                        {
                            Session["CTAR_VIAJES"] = viaje;
                            txtAlmacen.Text = (Session["CTAR_Almacenes"] as List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>)
                                .Where(x => x.idAlmacen == viaje.IDAlmacenOracle)
                                .FirstOrDefault().codAlmacen;
                            txtDescripcion.Text = viaje.Descripcion;
                            txtTransportista.Text = viaje.ProveedorOracle;
                            hdfIdProveedorEbs12.Value = viaje.IDProveedorOracle.ToString();
                            txtNumViaje.Text = viaje.FolioViaje;
                            hdfIdAlmacenOracle.Value = viaje.IDAlmacenOracle.ToString();
                        }

                        /*CLOUD_TRANSFERENCIAS transferencia = new CLOUD_TRANSFERENCIAS();

                        if (transferencia.ObtenerTransferencia(Convert.ToInt64(txtNumViaje.Text)))
                        {
                            txtAlmacen.Text = transferencia.Transferencias.almacenOrigen;
                            txtDescripcion.Text = transferencia.Transferencias.descripcion;
                            txtTransportista.Text = transferencia.Transferencias.transportista;
                            txtNumViaje.Text = transferencia.Transferencias.numTransferencia.ToString();
                        }*/



                    }

                    if (tarifa.Pedido_venta)
                    {
                        EBS12_VIAJES_PEDIDOS_VENTAS viaje = new EBS12_VIAJES_PEDIDOS_VENTAS();

                        if (viaje.ObtenerViajesPedidosVentas(Convert.ToInt64(txtNumViaje.Text)))
                        {
                            txtAlmacen.Text = viaje.ViajesPedidosVentas.codAlmacenOrigen;
                            txtDescripcion.Text = viaje.ViajesPedidosVentas.rutaViaje; //"PEDIDO DE VENTA NO. " + viaje.ViajesPedidosVentas.orderNumber.ToString();
                            txtTransportista.Text = viaje.ViajesPedidosVentas.transportista;
                            hdfIdProveedorEbs12.Value = viaje.ViajesPedidosVentas.idProveedor.ToString();
                            txtNumViaje.Text = viaje.ViajesPedidosVentas.idViaje.ToString();
                            hdfIdAlmacenOracle.Value = viaje.ViajesPedidosVentas.idAlmacenOrigen.ToString();
                        }
                        else
                            MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe información del Pedido de Venta con el Numero de Viaje.");
                    }

                }
                else
                {

                    var viaje = new LOGIViaje().ObtenerViajes(null, null, null, txtNumViaje.Text, null, null, null, true, null, null, true).FirstOrDefault();
                    if (viaje != null)
                    {
                        Session["CTAR_VIAJES"] = viaje;
                        txtAlmacen.Text = (Session["CTAR_Almacenes"] as List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>)
                            .Where(x => x.idAlmacen == viaje.IDAlmacenOracle)
                            .FirstOrDefault().codAlmacen;
                        txtDescripcion.Text = viaje.Descripcion;
                        txtTransportista.Text = viaje.ProveedorOracle;
                        hdfIdProveedorEbs12.Value = viaje.IDProveedorOracle.ToString();
                        txtNumViaje.Text = viaje.FolioViaje;
                        hdfIdAlmacenOracle.Value = viaje.IDAlmacenOracle.ToString();
                    }
                    else
                        MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe información de la Tranferencia con el Numero de Viaje.");
                  
                    /*
                    if (rbTransaferencia.Checked)
                    {
                        CLOUD_TRANSFERENCIAS transferencia = new CLOUD_TRANSFERENCIAS();

                        if (transferencia.ObtenerTransferencia(Convert.ToInt64(txtNumViaje.Text)))
                        {
                            txtAlmacen.Text = transferencia.Transferencias.almacenOrigen;
                            txtDescripcion.Text = transferencia.Transferencias.descripcion;
                            txtTransportista.Text = transferencia.Transferencias.transportista;
                            txtNumViaje.Text = transferencia.Transferencias.numTransferencia.ToString();
                        }
                        else
                            MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe información de la Tranferencia con el Numero de Viaje.");

                        var viaje = new LOGIViaje().ObtenerViajes(null, null, null, txtNumViaje.Text, null, null, null, true, null, null, true).FirstOrDefault();
                        if (viaje != null)
                        {
                            Session["CTAR_VIAJES"] = viaje;
                            txtAlmacen.Text = (Session["CTAR_Almacenes"] as List<EBS12_ALMACENES.Almacen.ItemsAlmacenes>)
                                .Where(x => x.idAlmacen == viaje.IDAlmacenOracle)
                                .FirstOrDefault().codAlmacen;
                            txtDescripcion.Text = viaje.Descripcion;
                            txtTransportista.Text = viaje.ProveedorOracle;
                            hdfIdProveedorEbs12.Value = viaje.IDProveedorOracle.ToString();
                            txtNumViaje.Text = viaje.FolioViaje;
                            hdfIdAlmacenOracle.Value = viaje.IDAlmacenOracle.ToString();
                        }
                        else
                            MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe información de la Tranferencia con el Numero de Viaje.");
                    }
                    else if (rbPedidoVenta.Checked)
                    {
                        EBS12_VIAJES_PEDIDOS_VENTAS viaje = new EBS12_VIAJES_PEDIDOS_VENTAS();

                        if (viaje.ObtenerViajesPedidosVentas(Convert.ToInt64(txtNumViaje.Text)))
                        {
                            txtAlmacen.Text = viaje.ViajesPedidosVentas.codAlmacenOrigen;
                            txtDescripcion.Text = viaje.ViajesPedidosVentas.rutaViaje; //"PEDIDO DE VENTA NO. " + viaje.ViajesPedidosVentas.orderNumber.ToString();
                            txtTransportista.Text = viaje.ViajesPedidosVentas.transportista;
                            hdfIdProveedorEbs12.Value = viaje.ViajesPedidosVentas.idProveedor.ToString();
                            txtNumViaje.Text = viaje.ViajesPedidosVentas.idViaje.ToString();
                            hdfIdAlmacenOracle.Value = viaje.ViajesPedidosVentas.idAlmacenOrigen.ToString();
                        }
                        else
                            MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe información del Pedido de Venta con el Numero de Viaje.");

                    }
                    else
                    {
                        MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "Es necesario seleccionar un Tipo de Viaje.");
                    }
                */
                }

            }      
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, Faultexc.Detail.ExcDetalle.Mensaje);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }

        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                if (!EsUsuarioGuardar)
                {
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "El usuario no tiene los permisos requeridos.", 7000);
                    return;
                }

                Calcular();

            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, Faultexc.Detail.ExcDetalle.Mensaje);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }
        }

        protected void btnOracle_Click(object sender, EventArgs e)
        {
            try
            {
                //Valida que selecciono una tarifa

                if (!EsUsuarioAutorizar)
                {
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "El usuario no tiene los permisos requeridos.", 7000);
                    return;
                }

                /*if (Convert.ToDecimal(hdfIdTarifa.Value) <= 0)
                {
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Es necesario realizar el calculo del Viaje.");
                    return;
                }

                if (Convert.ToDecimal(hdfIdCalculoTarifa.Value) <= 0)
                {
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Es necesario guardar el calculo del Viaje.");
                    return;
                }*/

                Calcular();

                string nombreProveedor = "";
                decimal montoTransporte = 0;
                decimal montoCaseta = 0;

                montoTransporte = decimal.Parse(txtMontoTransporte.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("es-MX"));
                montoCaseta = chkCasetas.Checked  ? decimal.Parse(txtMontoCaseta.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("es-MX")) : 0;

                //Buscar centro de costo a EBS12.

                //Validar que todos los datos del viaje esten completos.

                Viaje viaje = new Viaje();

                if (Session["CTAR_VIAJES"] != null)
                {
                    viaje = (Session["CTAR_VIAJES"] as Viaje);

                    if (string.IsNullOrEmpty(viaje.Chofer) || string.IsNullOrEmpty(viaje.Marca) || string.IsNullOrEmpty(viaje.Modelo) || string.IsNullOrEmpty(viaje.PlacaTractor) || string.IsNullOrEmpty(viaje.PlacaCaja)
                        || string.IsNullOrEmpty(viaje.NumeroSerieCaja) || string.IsNullOrEmpty(viaje.NumeroECOCaja) || string.IsNullOrEmpty(viaje.NumeroRuta) || string.IsNullOrEmpty(viaje.NumeroConvoy) || string.IsNullOrEmpty(viaje.NumeroSello)
                        || string.IsNullOrEmpty(viaje.NumeroCartaPorte) || viaje.IDProveedorOracle <= 0)
                    {
                        MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Es necesario capturar toda la información del Número de Viaje para poder continuar.");
                        return;
                    }

                }
                else
                {
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "No existe el viaje de la Transferencia.");
                    return;
                }


                CLOUD_TRANSFERENCIAS transferencia = new CLOUD_TRANSFERENCIAS();

                if (transferencia.ObtenerTransferencia(txtNumViaje.Text))
                {
                    hdfAlmacenDestino.Value = transferencia.Transferencias.almacenDestino;
                }

                //Buscar el nombre del proveedor a EBS12

                EBS12_TRANSPORTISTAS tranportista = new EBS12_TRANSPORTISTAS();

                if (tranportista.ObtenerTransportistaId(Convert.ToInt64(hdfIdProveedorEbs12.Value)))
                {
                    nombreProveedor = tranportista.Transportistas.items.FirstOrDefault().nombreTransportista;
                }
                

                /*

                if (rbTransaferencia.Checked)
                {

                    //Validar que todos los datos del viaje esten completos.

                    Viaje viaje = new Viaje();

                    if (Session["CTAR_VIAJES"] != null)
                    {
                        viaje = (Session["CTAR_VIAJES"] as Viaje);
                        
                        if ( string.IsNullOrEmpty(viaje.Chofer) ||  string.IsNullOrEmpty(viaje.Marca) || string.IsNullOrEmpty(viaje.Modelo) || string.IsNullOrEmpty(viaje.PlacaTractor) || string.IsNullOrEmpty(viaje.PlacaCaja)
                            || string.IsNullOrEmpty(viaje.NumeroSerieCaja) || string.IsNullOrEmpty(viaje.NumeroECOCaja) ||string.IsNullOrEmpty(viaje.NumeroRuta) || string.IsNullOrEmpty(viaje.NumeroConvoy) || string.IsNullOrEmpty(viaje.NumeroSello)
                            || string.IsNullOrEmpty(viaje.NumeroCartaPorte) || viaje.IDProveedorOracle <= 0)
                        {
                            MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Es necesario capturar toda la información del Número de Viaje para poder continuar.");
                            return;
                        }

                    }
                    else
                    {
                        MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "No existe el viaje de la Transferencia.");
                        return; 
                    }


                    CLOUD_TRANSFERENCIAS transferencia = new CLOUD_TRANSFERENCIAS();

                    if (transferencia.ObtenerTransferencia(txtNumViaje.Text))
                    {
                        hdfAlmacenDestino.Value = transferencia.Transferencias.almacenDestino;
                    }

                    //Buscar el nombre del proveedor a EBS12

                    EBS12_TRANSPORTISTAS tranportista = new EBS12_TRANSPORTISTAS();

                    if (tranportista.ObtenerTransportistaId(Convert.ToInt64(hdfIdProveedorEbs12.Value)))
                    {
                        nombreProveedor = tranportista.Transportistas.items.FirstOrDefault().nombreTransportista;
                    }
                }
                else
                {
                    nombreProveedor = txtTransportista.Text;
                }
                 
                 **/

                EBS12_ALMACENES almancenebs12 = new EBS12_ALMACENES();

                string almacen = "";
                string centroCosto = "";

                almacen =  txtAlmacen.Text;

                //almacen = rbTransaferencia.Checked && ddlTipoTarifa.SelectedValue == "2" ? hdfAlmacenDestino.Value : txtAlmacen.Text;

                if (almancenebs12.ObtenerAlmacen(almacen))
                    centroCosto = almancenebs12.Almacenes.items.FirstOrDefault().centroCosto;
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, almancenebs12.Almacenes.mensaje);


                ClaseHttpCliente cliente = new ClaseHttpCliente();
                LOGI_CALCULOS_TARIFAS calculo = new LOGI_CALCULOS_TARIFAS();
                LOGI_TARIFAS tarifa = new LOGI_TARIFAS();


                //Buscar informacion de conf.

               // hdfIdCalculoTarifa.Value = calculo.CalculoTarifas.Id_calculo_tarifa.ToString();

                if (tarifa.BusquedaConfiguracion(Convert.ToDecimal(hdfIdTarifa.Value), (decimal?)null, centroCosto))
                {
                    TarifaConfiguracion configuracion = new TarifaConfiguracion();
                    configuracion = tarifa.ListaTarifasConf.FirstOrDefault();

                    List<CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea> lineasOC = new List<CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea>();

                    lineasOC.Add(new CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea()
                    {
                        itemNumber = configuracion.Clave_articulo_tarifa,
                        itemDescription = "TRANSPORTE - " + txtDescripcion.Text + " - Viaje: " + txtNumViaje.Text,
                        price = montoTransporte,
                        quantity = 1,
                        unitOfMeasure = configuracion.Udm,
                        atributo1 = "SERVICIOS",
                        Schedule = (new List<CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea.Entrada>(){
                                                                new CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea.Entrada(){
                                                                shipToOrganizationCode = txtAlmacen.Text,
                                                                       
                                                                Distribution = (new List<CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea.Entrada.Distribucion>(){
                                                                                new CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea.Entrada.Distribucion(){
                                                                                    budgetDate = viaje.FechaEnvio.ToString("yyyy-MM-dd"), 
                                                                                    chargeAccount = chkBackhaul.Checked ? configuracion.Combinacion_backhaul :  configuracion.Combinacion_transferencia,
                                                                                    requesterEmail = UsuarioSesion.Usuario.Email // "mponce@pinsa.com" //
                                                                                }
                                                                                }).ToList()
                                                            }
                                                            }).ToList()
                    });

                    if (chkCasetas.Checked)
                    {
                        lineasOC.Add(new CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea()
                        {
                            itemNumber = configuracion.Clave_articulo_caseta,
                            itemDescription = "CASETAS - " + txtDescripcion.Text + " - Viaje: " + txtNumViaje.Text,
                            price = montoCaseta,
                            quantity = 1,
                            unitOfMeasure = configuracion.Udm,
                            atributo1 = "SERVICIOS",
                            Schedule = (new List<CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea.Entrada>(){
                                                                new CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea.Entrada(){
                                                                shipToOrganizationCode = txtAlmacen.Text,
                                                                Distribution = (new List<CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea.Entrada.Distribucion>(){
                                                                                new CLOUD_ORDEN_COMPRA.OrdenCompraCloud.Linea.Entrada.Distribucion(){
                                                                                    budgetDate =viaje.FechaEnvio.ToString("yyyy-MM-dd"),  //DateTime.Now.Date.ToString("yyyy-MM-dd"), 
                                                                                    chargeAccount = chkBackhaul.Checked ? configuracion.Combinacion_backhaul : configuracion.Combinacion_caseta,
                                                                                    requesterEmail = UsuarioSesion.Usuario.Email // "mponce@pinsa.com" //
                                                                                }
                                                                                }).ToList()
                                                            }
                                                            }).ToList()
                        });
                    }

                    // Se inserta la orden de compra

                    CLOUD_ORDEN_COMPRA orden = new CLOUD_ORDEN_COMPRA();
                    orden.OrdenCompra.procurementBusinessId = Convert.ToInt64(configuracion.Id_empresa_cloud);
                    orden.OrdenCompra.procurementBusinessUnit = configuracion.Clave_cloud;
                    orden.OrdenCompra.requisitioningBusinessUnit = configuracion.Clave_cloud;
                    orden.OrdenCompra.buyerEmail = UsuarioSesion.Usuario.Email; //mponce@pinsa.com"; //
                    orden.OrdenCompra.supplier = nombreProveedor;//txtTransportista.Text;
                    orden.OrdenCompra.currencyCode = configuracion.Moneda;
                    orden.OrdenCompra.documentDescription = "Viaje No. " + txtNumViaje.Text;
                    orden.OrdenCompra.interfaceSourceCode = "TARIFAS";
                    orden.OrdenCompra.referenceNumber = hdfIdCalculoTarifa.Value; //calculo.CalculoTarifas.Id_calculo_tarifa.ToString();
                    orden.OrdenCompra.ledgerName = configuracion.Nombre_libro_cuenta_cloud;
                    orden.OrdenCompra.Line = lineasOC;

                    string JsonMaestro = "";

                    CLOUD_ORDEN_COMPRA ordencompra = new CLOUD_ORDEN_COMPRA();
                    var Json = orden; //new { requisicion };

                    JsonMaestro = Newtonsoft.Json.JsonConvert.SerializeObject(Json);
                    ordencompra = ordencompra.GrabarOrdenCompra(JsonMaestro);

                    if (ordencompra.OrdenCompra.resultado == "Si")
                    {
                        calculo.CalculoTarifas.Id_calculo_tarifa = Convert.ToDecimal(hdfIdCalculoTarifa.Value);
                        calculo.CalculoTarifas.Id_tarifa = Convert.ToDecimal(hdfIdTarifa.Value);
                        calculo.CalculoTarifas.Num_viaje = txtNumViaje.Text;
                        //calculo.CalculoTarifas.Pedido_venta = rbPedidoVenta.Checked;
                        calculo.CalculoTarifas.Tranferencia = true;//rbTransaferencia.Checked;
                        calculo.CalculoTarifas.Caseta = chkCasetas.Checked;
                        calculo.CalculoTarifas.Backhaul = chkBackhaul.Checked;
                        calculo.CalculoTarifas.Monto_transporte = montoTransporte;
                        calculo.CalculoTarifas.Monto_caseta = montoCaseta;
                        calculo.CalculoTarifas.Activo = true;
                        calculo.CalculoTarifas.Folio_orden_compra = ordencompra.OrdenCompra.orderNumber;
                        calculo.CalculoTarifas.Id_orden_compra = ordencompra.OrdenCompra.pOHeaderId;

                        if (calculo.Grabar())
                        {
                            txtFolioOC.Text = ordencompra.OrdenCompra.orderNumber;
                            MostrarMensaje(ControladorMensajes.TipoMensaje.Exito, "Se ha generado la Orden de Compra: " + ordencompra.OrdenCompra.orderNumber + " en oracle Cloud.");
                            btnCalcular.Enabled = false;
                            btnOracle.Enabled = false;
                        }
                    }
                    else
                    {
                        MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, ordencompra.OrdenCompra.mensaje);
                    }
                }
                else
                {
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "No existe configuración de Tarifas para la empresa seleccionada.");
                    return;

                }
                //}
            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, Faultexc.Detail.ExcDetalle.Mensaje);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }
        }

        private void CargarMunicipios(decimal IdEstado)
        {
            try
            {
                ddlMunicipio.Items.Clear();
                ddlMunicipio.Items.Insert(0, new ListItem("..:: Seleccionar ::..", "0"));
                ddlMunicipio.DataBind();

                if (Convert.ToDecimal(IdEstado) > 0)
                {
                    if (Session["CTAR_Municipios"] != null)
                    {
                        ddlMunicipio.DataSource = (Session["CTAR_Municipios"] as List<Municipio>).Where(x => x.Id_estado == IdEstado).OrderBy(x => x.Nombre_municipio).ToList();
                        ddlMunicipio.DataValueField = "Id_municipio";
                        ddlMunicipio.DataTextField = "Nombre_municipio";
                        ddlMunicipio.DataBind();

                    }

                }
            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, Faultexc.Detail.ExcDetalle.Mensaje);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }
        }

        private void CargarCiudades(decimal IdMunicipio)
        {
            try
            {
                ddlCiudad.Items.Clear();
                ddlCiudad.Items.Insert(0, new ListItem("..:: Seleccionar ::..", "0"));
                ddlCiudad.DataBind();

                if (Convert.ToDecimal(IdMunicipio) > 0)
                {
                    if (Session["CTAR_Ciudades"] != null)
                    {
                        ddlCiudad.DataSource = (Session["CTAR_Ciudades"] as List<Ciudad>).Where(x => x.Id_municipio == IdMunicipio).OrderBy(x => x.Nombre_ciudad).ToList();
                        ddlCiudad.DataValueField = "Id_ciudad";
                        ddlCiudad.DataTextField = "Nombre_ciudad";
                        ddlCiudad.DataBind();

                    }

                }
            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, Faultexc.Detail.ExcDetalle.Mensaje);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }
        }

        private void Calcular()
        {

            // Validar campos obligatorios

            if (!ValidarCampos()) return;

            //Validar que exista viaje relacionado con transferencias.

           // if (rbTransaferencia.Checked)
            //{

            /*
            CLOUD_TRANSFERENCIAS transferencia = new CLOUD_TRANSFERENCIAS();

            if (transferencia.ObtenerTransferencia(txtNumViaje.Text))
            {
                hdfAlmacenDestino.Value = transferencia.Transferencias.almacenDestino;
            }
            else
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No se han relacionado Envíos al Viaje seleccionado, favor de realizarlo para continuar.");
                return;
            }
             * 
             * */
        //}

            CLOUD_TRANSFERENCIAS transferencia = new CLOUD_TRANSFERENCIAS();

            if (transferencia.ObtenerTransferencia(txtNumViaje.Text))
            {
                hdfAlmacenDestino.Value = transferencia.Transferencias.almacenDestino;
            }
            else
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No se han relacionado Envíos al Viaje seleccionado, favor de realizarlo para continuar.");
                return;
            }
             

            LOGI_TARIFAS tarifa = new LOGI_TARIFAS();
            tarifa.Tarifas.Id_municipio = Convert.ToDecimal(ddlMunicipio.SelectedValue);
            tarifa.Tarifas.Id_tipo_tarifa = Convert.ToDecimal(ddlTipoTarifa.SelectedValue);
            tarifa.Tarifas.Id_codigo_transporte = Convert.ToDecimal(ddlCodTransporte.SelectedValue);
            // tarifa.Tarifas.Nombre_proveedor_ebs12 = txtTransportista.Text;
            // tarifa.Tarifas.Nombre_almacen_ebs12 = txtAlmacen.Text;
            tarifa.Tarifas.Id_ciudad = Convert.ToDecimal(ddlCiudad.SelectedValue);
            tarifa.Tarifas.Backhaul = chkBackhaul.Checked;
            tarifa.Tarifas.Id_proveedor_ebs12 = Convert.ToDecimal(hdfIdProveedorEbs12.Value);
            tarifa.Tarifas.Id_almacen_origen_ebs12 = Convert.ToDecimal(hdfIdAlmacenOracle.Value);
            tarifa.Tarifas.Activo = true;

            if (tarifa.Busqueda())
            {

                DateTime datetoday = DateTime.Now.Date;


                var ListaTarifas = tarifa.ListaTarifas.Where(x => x.Fecha_inicio <= datetoday && x.Fecha_fin >= datetoday).ToList();

                if (ListaTarifas.Count() > 1)
                {
                    hdfIdTarifa.Value = "-1";
                    txtMontoTransporte.Text = String.Format("{0:C4}", 0);
                    txtMontoCaseta.Text = String.Format("{0:C4}", 0);
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "La tarifa de viaje se encuentra dentro de varios rangos de fechas.");
                    return;

                }
                else if (ListaTarifas.Count() <= 0)
                {
                    hdfIdTarifa.Value = "-1";
                    txtMontoTransporte.Text = String.Format("{0:C4}", 0);
                    txtMontoCaseta.Text = String.Format("{0:C4}", 0);
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "La tarifa de viaje se encuentra fuera de los rangos de fechas.");
                    return;

                }

                Tarifa tar = new Tarifa();
                tar = ListaTarifas.FirstOrDefault();

                DateTime fechaInicio = tar.Fecha_inicio.Date;
                DateTime fechaFin = tar.Fecha_fin.Date;

                if (datetoday >= fechaInicio && datetoday <= fechaFin)
                {
                    hdfIdTarifa.Value = tar.Id_tarifa.ToString();
                    txtMontoTransporte.Text = String.Format("{0:C4}", tar.Monto_tarifa);
                    txtMontoCaseta.Text = chkCasetas.Checked ? String.Format("{0:C4}", tar.Monto_caseta) : String.Format("{0:C4}", 0);

                    //Grabar el calculo de tarifa.

                    decimal montoTransporte = 0;
                    decimal montoCaseta = 0;

                    montoTransporte = decimal.Parse(txtMontoTransporte.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("es-MX"));
                    montoCaseta = decimal.Parse(txtMontoCaseta.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("es-MX"));

                    LOGI_CALCULOS_TARIFAS calculo = new LOGI_CALCULOS_TARIFAS();

                    calculo.CalculoTarifas.Id_calculo_tarifa = Convert.ToDecimal(hdfIdCalculoTarifa.Value);
                    calculo.CalculoTarifas.Id_tarifa = Convert.ToDecimal(hdfIdTarifa.Value);
                    calculo.CalculoTarifas.Num_viaje = txtNumViaje.Text;
                    //calculo.CalculoTarifas.Pedido_venta = rbPedidoVenta.Checked;
                    calculo.CalculoTarifas.Tranferencia = true; //rbTransaferencia.Checked;
                    calculo.CalculoTarifas.Caseta = chkCasetas.Checked;
                    calculo.CalculoTarifas.Backhaul = chkBackhaul.Checked;
                    calculo.CalculoTarifas.Monto_transporte = montoTransporte;
                    calculo.CalculoTarifas.Monto_caseta = montoCaseta;

                    calculo.CalculoTarifas.Activo = true;

                    if (calculo.Grabar())
                    {
                        hdfIdCalculoTarifa.Value = calculo.CalculoTarifas.Id_calculo_tarifa.ToString();
                    }
                }
            }
            else
            {
                hdfIdTarifa.Value = "-1";
                txtMontoTransporte.Text = String.Format("{0:C4}", 0);
                txtMontoCaseta.Text = String.Format("{0:C4}", 0);
                MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No existe información de Tarifas relacionada con los valores seleccionados.");
            }
            
        }

        private bool ValidarCampos()
        {
            bool respuesta = true;
            string mensaje = string.Empty;


            if (txtDescripcion.Text == "" || txtAlmacen.Text == "")
                mensaje += "* Favor de seleccionar un <strong>No. de Viaje</strong>.<br>";
            if (Convert.ToDecimal(ddlMunicipio.SelectedValue) <= 0)
                mensaje += "* Favor de seleccionar un <strong>Municipio</strong>.<br>";
            if (Convert.ToDecimal(ddlTipoTarifa.SelectedValue) <= 0)
                mensaje += "* Favor de seleccionar un <strong>Tipo de Tarifa</strong>.<br>";
            if (Convert.ToDecimal(ddlCodTransporte.SelectedValue) <= 0)
                mensaje += "* Favor de seleccionar un <strong>Codigo de Transporte</strong>.<br>";

            //if (decimal.Parse(hdfIdMoneda.Value) != decimal.Parse(hdfIdMonedaFuncional.Value))
            //    if (string.IsNullOrEmpty(txtTC.Text))
            //        mensaje += "* Favor de seleccionar un <strong>Tipo Cambio</strong>.<br>";


            if (mensaje != string.Empty)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "<span style='font-size: 18px; font-weight: bold !important; font-family: 'Quantico', sans-serif; letter-spacing: -1px; padding: 0px 20px 10px 0px!important; display: block;'>¡Los siguientes campos son requeridos!</span>" + mensaje);
                respuesta = false;
            }


            return respuesta;
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNumViaje.Text = "";
            //rbPedidoVenta.Checked = false;
            //rbTransaferencia.Checked = false;
            txtDescripcion.Text = "";
            txtAlmacen.Text = "";
            txtTransportista.Text = "";
            txtFolioOC.Text = "";
            ddlEstado.SelectedIndex = -1;
            CargarMunicipios(-1);
            CargarCiudades(-1);
            ddlTipoTarifa.SelectedIndex = -1;
            ddlCodTransporte.SelectedIndex = -1;
            chkCasetas.Checked = false;
            chkBackhaul.Checked = false;
            txtMontoTransporte.Text = "";
            txtMontoCaseta.Text = "";
            btnCalcular.Enabled = true;
            btnOracle.Enabled = true;
            Session["CTAR_VIAJES"] = null;
            hdfIdProveedorEbs12.Value = "-1";
            hdfIdAlmacenOracle.Value = "-1";
            hdfIdCalculoTarifa.Value = "-1";
            hdfIdTarifa.Value = "-1";
        }

        public void Limpiar()
        {

            txtDescripcion.Text = "";
            txtAlmacen.Text = "";
            txtTransportista.Text = "";
            txtFolioOC.Text = "";
            ddlEstado.SelectedIndex = -1;
            CargarMunicipios(-1);
            CargarCiudades(-1);
           // ddlTipoTarifa.SelectedIndex = -1;
            ddlCodTransporte.SelectedIndex = -1;
            chkCasetas.Checked = false;
            chkBackhaul.Checked = false;
            txtMontoTransporte.Text = "";
            txtMontoCaseta.Text = "";
            btnCalcular.Enabled = true;
            btnOracle.Enabled = true;
            Session["CTAR_VIAJES"] = null;
            hdfIdProveedorEbs12.Value = "-1";
            hdfIdAlmacenOracle.Value = "-1";
            hdfIdCalculoTarifa.Value = "-1";
            hdfIdTarifa.Value = "-1";
        }


	}
}