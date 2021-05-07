using DevExpress.Web;
using LogisticaERP.Clases;
using LogisticaERP.Clases.Viajes;
using LogisticaERP.Clases.Viajes.OracleEBS12;
using LogisticaERP.Controles.BusquedaGenerica;
using LogisticaERP.LogisticaSOA;
using LogisticaERP.SeguridadERPSOA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP.Catalogos
{
    public partial class Viajes : PaginaBase
    {
        private static readonly string _keyGVEntregas = "LogisticaERP>Catalogos>Viajes>GVEntregas";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // se limpia la sesión del gridview de entregas
                HttpContext.Current.Session[_keyGVEntregas] = new List<ViajeEntrega>();
                ddlEstadoViaje.SelectedValue = "1";

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
                    if (empresaIntegracion == null ||
                        !empresaIntegracion.Id_empresa_eb12.HasValue ||
                        empresaIntegracion.Id_empresa_eb12 <= 0 ||
                        !empresaIntegracion.Id_empresa_cloud.HasValue ||
                        empresaIntegracion.Id_empresa_cloud <= 0)
                        hfEmpresaCloudException.Value = "La Empresa seleccionada no cuenta con un mapeo en Empresas Integraciones, favor de comunicarlo al departamento de TI-Aplicaciones!";
                    else
                    {
                        hfFecha.Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        // carga dropdownlist almacenes
                        var almacenes = new INVOrganizationUnit().ObtenerAlmacenesParaTarifasDeViajes(Convert.ToInt64(empresaIntegracion.Id_empresa_eb12), 0);
                        if (almacenes.Any())
                        {
                            hfAlmacenes.Value = Newtonsoft.Json.JsonConvert.SerializeObject(almacenes.Select(x => new { x.IDAlmacen, x.TipoAlmacen }).ToList());
                            LlenarDropDownList(ddlAlmacen, almacenes, "IDAlmacen", "NombreAlmacen");
                        }
                        else
                        {
                            MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No se encontraron almacenes asignados a la empresa seleccionada.");
                        }
                        // carga dropdownlist transportistas
                        var transportistas = new WSHCarrier().ObtenerTransportistas();
                        if (transportistas.Any())
                            LlenarDropDownList(ddlTransportista, transportistas, "CarrierID", "CarrierName");
                        else
                            MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, "No se encontraron transportistas.");
                    }
                }
            }

            LLenarGVEntregas();
        }

        protected void GVEntrega_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                var entregaViaje = (HttpContext.Current.Session[_keyGVEntregas] as List<ViajeEntrega>).Find(x => x.IDEntregaEBS12 == (decimal)e.Keys["IDEntregaEBS12"]);
                if (entregaViaje.IDViaje > 0)
                {
                    entregaViaje.Borrado = true;
                }
                else
                {
                    (HttpContext.Current.Session[_keyGVEntregas] as List<ViajeEntrega>).Remove(entregaViaje);
                }
                e.Cancel = true;
            }
            catch (Exception exception)
            {
                GVEntrega.JSProperties["cpException"] = exception.Message;
            }
        }

        #endregion

        #region Events

        [WebMethod(EnableSession = true)]
        public static void ActualizarEntregas(long idAlmacen)
        {
            try
            {
                var viajeEntregas = HttpContext.Current.Session[_keyGVEntregas] as List<ViajeEntrega>;
                if (viajeEntregas.Any())
                {
                    // remueve todas las entregas que aun no han sido grabadas en base de datos
                    viajeEntregas.RemoveAll(x => x.IDViajeEntrega == 0);
                    // borrado logico de todas las entregas que no correspondan al almacen seleccionado
                    // y habilitamos todas las entregas que corresondan al almacen seleccionado
                    viajeEntregas.ForEach(x => x.Borrado = !(x.IDAlmacenEBS12 == idAlmacen));
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static void EstablecerGVEntrega(decimal idViaje, string entregas, bool esArray)
        {
            try
            {
                // Limpiar el GridView de Entregas
                if (idViaje == -1)
                {
                    HttpContext.Current.Session[_keyGVEntregas] = new List<ViajeEntrega>();
                }
                // Establece las entregas de un viaje consultado y previamente guardado
                else if (idViaje > 0)
                {
                    var viajeEntregas = new LOGIViaje().ObtenerViajes(idViaje, null, null, null, null, null, null, true, null, null, false).First().ViajeEntregas;
                    HttpContext.Current.Session[_keyGVEntregas] = viajeEntregas != null ? viajeEntregas.ToList() : new List<ViajeEntrega>();
                }
                // establece la asignación de entregas en base al almacen seleccionado
                else
                {
                    var viajeEntrega = new List<ViajeEntrega>();

                    if (esArray)
                        viajeEntrega.AddRange(JsonConvert.DeserializeObject<List<ViajeEntrega>>(entregas));
                    else
                        viajeEntrega.Add(JsonConvert.DeserializeObject<ViajeEntrega>(entregas));

                    viajeEntrega.ForEach(x =>
                    {
                        if (!(HttpContext.Current.Session[_keyGVEntregas] as List<ViajeEntrega>).Exists(y => y.IDEntregaEBS12 == x.IDEntregaEBS12))
                            (HttpContext.Current.Session[_keyGVEntregas] as List<ViajeEntrega>).Add(x);
                    });
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static void EstablecerModalEntregas(long idAlmacen)
        {
            try
            {
                var parametro = new ParametroGridView();
                //campos a visualizar
                parametro.Campos.Add(new Campo("Núm. Entrega", "IDEntregaEBS12", true, 200, typeof(decimal), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Fecha Programada", "FechaProgramadaEBS12", 200, typeof(DateTime), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Núm. Pedido Venta", "NumPedidoVentaEBS12", 200, typeof(decimal), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Transportista", "TransportistaEBS12", 200, typeof(string), HorizontalAlign.Left));
                parametro.Campos.Add(new Campo("Núm. Sucursal", "NumSucursalEBS12", 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Ciudad", "CiudadEBS12", 200, typeof(string), HorizontalAlign.Left));
                parametro.Campos.Add(new Campo("Estado", "EstadoEBS12", 200, typeof(string), HorizontalAlign.Left));
                parametro.Campos.Add(new Campo("Cód. Almacen", "CodigoAlmacenEBS12", 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Cód. Subalmacen", "CodigoSubalmacenEBS12", 200, typeof(string), HorizontalAlign.Center));
                parametro.Campos.Add(new Campo("Cajas", "TotalCajasEBS12", 200, typeof(decimal), HorizontalAlign.Right, "n4"));
                parametro.Campos.Add(new Campo("Cajas Estandar", "TotalCajasEstandarEBS12", 200, typeof(decimal), HorizontalAlign.Right, "n4"));
                //fuente de datos
                parametro.DataSource = new OMSaleOrder().ObtenerPedidosVentasViajesA2Meses(idAlmacen, null, true)
                    .Select(x => new ViajeEntrega
                    {
                        IDEntregaEBS12 = x.IDEntrega,
                        FechaProgramadaEBS12 = x.FechaProgramada,
                        NumPedidoVentaEBS12 = x.NumPedidoVenta,
                        TransportistaEBS12 = x.Transportista,
                        NumSucursalEBS12 = x.NumSucursal,
                        CiudadEBS12 = x.Ciudad,
                        EstadoEBS12 = x.Estado,
                        IDAlmacenEBS12 = x.IDAlmacen,
                        CodigoAlmacenEBS12 = x.CodigoAlmacen,
                        CodigoSubalmacenEBS12 = x.CodigoSubalmacen,
                        TotalCajasEBS12 = x.TotalCajas,
                        TotalCajasEstandarEBS12 = x.TotalCajasEstandar
                    })
                    .ToList();
                //configuraciones
                parametro.FunctionCallback = "establecerGVEntrega";
                parametro.PageSize = 20;
                parametro.NumericButtonCount = 10;
                parametro.ShowSelectCheckbox = true;
                //guarda configuración de parametros
                parametro.EstablecerParametros();
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
                parametro.Campos.Add(new Campo("Inf. Completa", "InformacionCompleta", 70, typeof(bool), HorizontalAlign.Center));
                //fuente de datos
                parametro.DataSource = new LOGIViaje().ObtenerViajes(null, null, null, null, Convert.ToDecimal(HttpContext.Current.Session["EmpresaID"]), null, null, true, null, null, true);
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
        public static object Grabar(Viaje viaje)
        {
            try
            {
                var usuarioSesionSerializado = HttpContext.Current.Session["UsuarioSesionSerializado"];
                if (usuarioSesionSerializado != null)
                {
                    var sesion = JsonConvert.DeserializeObject<UsuarioSesion>(usuarioSesionSerializado.ToString());
                    var listaUsuarioModuloFuncionalidad = sesion.Usuario.ListaUsuarioFuncionalidad.Where(x => x.Id_modulo == decimal.Parse(Id_Modulo)).ToList();
                    string pagina = HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.IndexOf('/') == 0 ? 1 : 0).ToUpperInvariant();
                    var usuarioFuncionalidad = listaUsuarioModuloFuncionalidad.Where(x => 
                        !string.IsNullOrEmpty(x.Uri)
                        && x.Uri
                            .Substring(x.Uri.IndexOf('/') == 0 ? 1 : 0)
                            .ToUpperInvariant()
                            .Contains(pagina))
                            .FirstOrDefault();

                    // Si el usuario es administrador tiene permiso a todo
                    if (sesion.Usuario.Es_admin || (usuarioFuncionalidad != null && usuarioFuncionalidad.Es_guardar.HasValue && usuarioFuncionalidad.Es_guardar.Value))
                    {
                        viaje.ViajeEntregas = (HttpContext.Current.Session[_keyGVEntregas] as List<ViajeEntrega>).ToArray();

                        LOGIViaje viajes = new LOGIViaje();

                        //Validar si el check es informacion completa , validar todos los campos-

                        if (viaje.InformacionCompleta.HasValue && viaje.InformacionCompleta.Value)
                        {
                            if (viaje.IDProveedorOracle <= 0 || string.IsNullOrEmpty(viaje.Marca) || string.IsNullOrEmpty(viaje.Modelo) || string.IsNullOrEmpty(viaje.PlacaTractor) || string.IsNullOrEmpty(viaje.PlacaCaja) || string.IsNullOrEmpty(viaje.NumeroSerieCaja) 
                                || string.IsNullOrEmpty(viaje.NumeroECOCaja) || string.IsNullOrEmpty(viaje.NumeroRuta) || string.IsNullOrEmpty(viaje.NumeroConvoy) || string.IsNullOrEmpty(viaje.NumeroSello) || string.IsNullOrEmpty(viaje.NumeroCartaPorte) )

                                return new { Error = true, Mensaje = "Es necesario capturar toda la información del Número de Viaje para poder continuar. " };
                        }

                        viaje.IDViaje = viajes.Grabar(viaje);

                        Viaje v = new LOGIViaje().ObtenerViajes(viaje.IDViaje, null, null, null, null, null, null, true, null, null, true).FirstOrDefault();

                        CLOUD_LISTA_VALORES lista = new CLOUD_LISTA_VALORES();
                        lista.ListaValores.tipoLista = "XXGPIN_INV_TRIPS_TARIFAS";
                        lista.ListaValores.nombreArchivo = "ListasGenericas_Viajes_" + v.IDViaje + ".txt";

                        lista.ListaValores.items.Add(new CLOUD_LISTA_VALORES.Lookups.ItemsLookups()
                        {
                            codigo = v.FolioViaje,
                            descripcion = v.Descripcion,
                            significado = v.FolioViaje,
                            etiqueta = v.NombreAlmacenOracle,
                            activo = "Y"
                        });

                        string JsonMaestro = "";
                        var Json = lista;

                        JsonMaestro = Newtonsoft.Json.JsonConvert.SerializeObject(Json);

                        lista.GenerarArchivotxt(JsonMaestro);
                        
                        return new { Error = false, Mensaje = "Viaje de transferencia guardado con exito.", datos = v };
                    }
                    else
                    {
                        return new { Error = true, Mensaje = "El usuario no tiene privilegios para guardar viajes de transferencia." };
                    }
                }

                return new { Error = true, Mensaje = "Error en la sesión de usuario, favor de autenticarse de nuevo." };
            }
            catch (Exception exception)
            {                
                throw exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public static object EliminarViaje(decimal idViaje)
        {
            try
            {
                var usuarioSesionSerializado = HttpContext.Current.Session["UsuarioSesionSerializado"];
                if (usuarioSesionSerializado != null)
                {
                    var sesion = JsonConvert.DeserializeObject<UsuarioSesion>(usuarioSesionSerializado.ToString());
                    var listaUsuarioModuloFuncionalidad = sesion.Usuario.ListaUsuarioFuncionalidad.Where(x => x.Id_modulo == decimal.Parse(Id_Modulo)).ToList();
                    string pagina = HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.IndexOf('/') == 0 ? 1 : 0).ToUpperInvariant();
                    var usuarioFuncionalidad = listaUsuarioModuloFuncionalidad.Where(x =>
                        !string.IsNullOrEmpty(x.Uri)
                        && x.Uri
                            .Substring(x.Uri.IndexOf('/') == 0 ? 1 : 0)
                            .ToUpperInvariant()
                            .Contains(pagina))
                            .FirstOrDefault();

                    // Si el usuario es administrador tiene permiso a todo
                    if (sesion.Usuario.Es_admin || (usuarioFuncionalidad != null && usuarioFuncionalidad.Es_borrar.HasValue && usuarioFuncionalidad.Es_borrar.Value))
                    {

                        LOGIViaje viajes = new LOGIViaje();
                        viajes.Eliminar(idViaje);
                        return new { Error = false, Mensaje = "Viaje de transferencia eliminado con exito." };
                    }
                    else
                    {
                        return new { Error = true, Mensaje = "El usuario no tiene privilegios para guardar viajes de transferencia." };
                    }
                }

                return new { Error = true, Mensaje = "Error en la sesión de usuario, favor de autenticarse de nuevo." };
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void LlenarDropDownList(DropDownList dropDownList, object list, string valueField, string textField)
        {
            try
            {
                dropDownList.DataSource = list;
                dropDownList.DataValueField = valueField;
                dropDownList.DataTextField = textField;
                dropDownList.DataBind();
                dropDownList.Items.Insert(0, new ListItem() { Text = "..:: Seleccione ::..", Value = "0" });
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }
        }

        private void LLenarGVEntregas()
        {
            GVEntrega.DataSource = (HttpContext.Current.Session[_keyGVEntregas] as List<ViajeEntrega>)
                .Where(x => !x.Borrado)
                .ToList();
            GVEntrega.KeyFieldName = "IDEntregaEBS12";
            GVEntrega.DataBind();
        }

        private void MostrarMensaje(ControladorMensajes.TipoMensaje tipo, dynamic sms, int? tiempoMostrar = 7000)
        {
            ControladorMensajes.MostrarMensaje(this, string.Format("TAR_{0}{1}", tipo, new Random().Next(0, 99)), tipo, sms, tiempoMostrar);
        }

        #endregion

        protected void GVEntrega_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCustomButtonEventArgs e)
        {
            /*
            if (e.VisibleIndex <= -1 || e.IsEditingRow || e.CellType == GridViewTableCommandCellType.Filter) return;

            if (e != null && (e.ButtonID == "btnEliminar"))
            {
                decimal idEstadoViaje = Convert.ToDecimal(ddlEstadoViaje.SelectedValue);

                if (idEstadoViaje != 1)
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.True;
                }
            }
             */

             
        }

        protected void GVEntrega_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

             decimal idEstadoViaje = Convert.ToDecimal(ddlEstadoViaje.SelectedValue);

             if (e.ButtonType == ColumnCommandButtonType.Delete && idEstadoViaje != 1)
            {
                e.Visible = false;
            }


            /*

            switch (e.ButtonType)
            {
                case ColumnCommandButtonType.Edit:
                    e.Visible = GVEntrega((ASPxGridView)sender, e.VisibleIndex);
                    break;
                case ColumnCommandButtonType.Delete:
                    e.Visible = DeleteButtonVisibleCriteria((ASPxGridView)sender, e.VisibleIndex);
                    break;
            }
             * */

        }

    }
}