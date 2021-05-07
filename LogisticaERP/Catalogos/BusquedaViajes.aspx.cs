using LogisticaERP.Clases;
using LogisticaERP.Clases.Viajes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP.Catalogos
{
    public partial class BusquedaViajes : PaginaBase
    {
        private const string _Viajes = "BViajes";

        [WebMethod(EnableSession = true)]
        public static void Limpiar()
        {
            try
            {

                HttpContext.Current.Session[_Viajes] = null;

            }
            catch (Exception exception)
            {
                throw exception;

            }
        }

        [WebMethod(EnableSession = true)]
        public static void ObtenerViajes(string folio, string tipoViaje, string tipoAlmacen, decimal? idAlmacen, decimal? idProveedor, string fechaInicio, string fechaFin)
        {
            try
            {

                DateTime? fi = fechaInicio != "" ? Convert.ToDateTime(fechaInicio) : (DateTime?)null;
                DateTime? ff = fechaFin != "" ? Convert.ToDateTime(fechaFin) : (DateTime?)null;
                folio = folio != "null" ? folio : null;
                tipoAlmacen = tipoAlmacen != "null" ? tipoAlmacen : null;
                tipoViaje =  tipoViaje != "null" ? tipoViaje : null;
                idAlmacen = idAlmacen <= 0 ? (decimal?)null : idAlmacen;
                idProveedor = idProveedor <= 0 ? (decimal?)null : idProveedor;

               // string fi = Convert.ToDateTime(fechaInicio).ToString("yyyy/MM/dd");
                //string ff = Convert.ToDateTime(fechaFin).ToString("yyyy/MM/dd");

                HttpContext.Current.Session[_Viajes] = new LOGIViaje().ObtenerViajeBusqueda((decimal?)null, tipoViaje, folio, Convert.ToDecimal(HttpContext.Current.Session["EmpresaID"]), null, tipoAlmacen, idAlmacen, idProveedor, true, fi, ff);

            }
            catch (Exception exception)
            {
                throw exception;

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session[_Viajes] = null;
                CargaInicial();
            }
            else
            {
                CargarGrid();
            }
        }

        public void CargaInicial()
        {
            try
            {
                EBS12_ALMACENES almanceOrigen = new EBS12_ALMACENES();
                if (almanceOrigen.ObtenerAlmacenes(Convert.ToDecimal(Session["EmpresaID"]), 0)){
                
                    ddlAlmacen.DataSource = almanceOrigen.Almacenes.items.ToList();
                    ddlAlmacen.DataValueField = "idAlmacen";
                    ddlAlmacen.DataTextField = "codAlmacen";
                    ddlAlmacen.DataBind();
                }
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, almanceOrigen.Almacenes.mensaje);


                EBS12_TRANSPORTISTAS transportista = new EBS12_TRANSPORTISTAS();
                if (transportista.ObtenerTransportistas()){

                    var proveedor = transportista.Transportistas.items.ToList();
                    ddlProveedor.DataSource = proveedor;
                    ddlProveedor.DataValueField = "idTransportista";
                    ddlProveedor.DataTextField = "nombreTransportista";
                    ddlProveedor.DataBind();
                }
                else
                    MostrarMensaje(ControladorMensajes.TipoMensaje.Informativo, transportista.Transportistas.mensaje);


            }
            catch (Exception exception)
            {
                throw exception;

            }
        }

        public void CargarGrid()
        {
            if (Session[_Viajes] != null)
            {
                GridViajes.DataSource = HttpContext.Current.Session[_Viajes];
                GridViajes.KeyFieldName = "IDViaje";
                GridViajes.DataBind();
            }
            else
            {
                /*GridASN.DataSource = new List<>
                GridASN.KeyFieldName = "Id_index";
                GridASN.DataBind();*/
            }
        }



        private void MostrarMensaje(ControladorMensajes.TipoMensaje tipo, dynamic sms, int? tiempoMostrar = 7000)
        {
            ControladorMensajes.MostrarMensaje(ScriptManager1, string.Format("BUSV_{0}{1}", tipo, new Random().Next(0, 99)), tipo, sms, tiempoMostrar);
        }

        protected void exportarGrid_RenderBrick(object sender, DevExpress.Web.ASPxGridViewExportRenderingEventArgs e)
        {

        }

        protected void btnExportarInterno_Click(object sender, EventArgs e)
        {
            GVExportar.WriteXlsToResponse("VIAJES" + "_" + DateTime.Now, true);
        }

    }
}