using LogisticaERP.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP.Catalogos.CancelacionMasiva.Popup
{
    public partial class BusquedaCodigosMotivos : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ObtenerPedidosVenta();
            }
            CargarGridCodigosMotivos();
        }

        #region Metodos

        private void ObtenerPedidosVenta()
        {
            EBS12_CODIGOS_MOTIVOS codigoMotivo = new EBS12_CODIGOS_MOTIVOS();
            List<EBS12_CODIGOS_MOTIVOS.CodigoMotivo> listaCodigosMotivos = new List<EBS12_CODIGOS_MOTIVOS.CodigoMotivo>();
            List<EBS12_CODIGOS_MOTIVOS.CodigoMotivoGrid> listaCodigosMotivosGrid = new List<EBS12_CODIGOS_MOTIVOS.CodigoMotivoGrid>();
            decimal index = 0;

            try
            {
                listaCodigosMotivos = codigoMotivo.ObtenerCodigosMotivos();
                if (listaCodigosMotivos.Count > 0)
                {
                    listaCodigosMotivosGrid = (from cm in listaCodigosMotivos
                                               select new EBS12_CODIGOS_MOTIVOS.CodigoMotivoGrid
                                               {
                                                   IdIndex = index++,
                                                   Codigo = cm.codigo,
                                                   Descripcion = cm.descripcion
                                               }).ToList();
                }
            }
            catch (JsonException ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, ex.Message);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ControladorMensajes.TipoMensaje.Error, ex.Message);
            }

            Session["ListaCodigosMotivos"] = listaCodigosMotivosGrid;
        }

        private void CargarGridCodigosMotivos()
        {
            List<EBS12_CODIGOS_MOTIVOS.CodigoMotivoGrid> listaCodigosMotivos = Session["ListaCodigosMotivos"] as List<EBS12_CODIGOS_MOTIVOS.CodigoMotivoGrid>;
            gridCodigosMotivos.DataSource = listaCodigosMotivos;
            gridCodigosMotivos.KeyFieldName = "IdIndex";
            gridCodigosMotivos.DataBind();
        }
       
        private void MostrarMensaje(ControladorMensajes.TipoMensaje tipo, dynamic sms, int? tiempoMostrar = 7000)
        {
            ControladorMensajes.MostrarMensaje(UpdatePanel1, string.Format("CAPRECMP_{0}{1}", tipo, new Random().Next(0, 99)), tipo, sms, tiempoMostrar);
        }

        #endregion
    }
}
