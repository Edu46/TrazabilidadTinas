using LogisticaERP.Clases;
using LogisticaERP.GrupoPinsaSOA;
using LogisticaERP.SeguridadERPSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP
{
    public partial class MainPage : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var menuDesplegado = Convert.ToBoolean(Session["MenuDesplegado"]);
            var strRawUrl = this.Request.RawUrl;
            if (menuDesplegado)
            {
                if (strRawUrl.Contains("Inicio"))
                {
                    liMonitorLocalizador.Attributes.Add("class", "treeview active");
                    Session["MenuDesplegado"] = false;
                }
            }
            try
            {
                //Esta seccion obtiene la informacion del usuario y simula que esta logeado al sistema
                if (Session["Token"] == null || Request.Cookies["GRUPOPINSA.ERP.ASPXFORMSAUTH"] == null || Request.Cookies["ASP.NET_SessionId"] == null || Request.Cookies["Session_grupopinsa.erp"] == null)
                {
                    IniciarSesion("usuarioerp", "pinsa123");
                }

                if (!IsPostBack)
                {

                    //if (Cache["Empresas"] == null || (Cache["Empresas"] as List<Empresa>).Count <= 0)
                    if (Session["LOG_Empresas"] == null)
                    {
                        var t6 = Task.Factory.StartNew(state =>
                        {
                            //Cache["Empresas"] = new GPO_EMPRESAS((HttpContext)state).ObtieneListaEmpresas();
                            ((HttpContext)state).Session["LOG_Empresas"] = new GPO_EMPRESAS((HttpContext)state).ObtieneListaEmpresasGrupoNegocio();
                        }, HttpContext.Current);
                        Task.WaitAll(new Task[] { t6 });
                    }
                    //Session["LOG_Empresas"] = Cache["Empresas"];
                    Session["MenuOculto"] = false;
                }
            }
            catch (FaultException<GrupoPinsaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "MostrarCajaMensajes(ERRORBOX, [{ 'StrongText': '" + Faultexc.Detail.ExcDetalle.Mensaje + "' }], 7000);", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "MostrarCajaMensajes(ERRORBOX, [{ 'StrongText': '" + ex.Message + "' }], 7000);", true);
            }
            if (Convert.ToBoolean(Session["MenuOculto"]))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "expandButtonMenu", "$('.btn-container').addClass('deactivate');", true);
            }
        }

        protected void cbxMenuOculto_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            Session["MenuOculto"] = Convert.ToBoolean(e.Parameter);
        }

        public void IniciarSesion(string usuario, string contrasenia)
        {
            SERP_USUARIOSESION login = new SERP_USUARIOSESION();
            string direccion = string.Empty;

            try
            {
                UsuarioSesion sesion = login.ObtenerSesionUsuario(usuario, contrasenia);

                if (sesion != null && sesion.Usuario != null)
                {
                    if (!sesion.Usuario.Activo)
                    {
                        throw new Exception("El usuario se encuentra inactivo.");
                    }

                    Session["Token"] = sesion.Token;
                    GrupoNegocioERP grupoNegocioERP = new GPO_GRUPONEGOCIO_ERP().ObtenerGruposNegocioERP(10, null, true).FirstOrDefault(); //6 = Pesca Azteca, simula que selecciona el grupo de negocio

                    Session["UsuarioSesionSerializado"] = Newtonsoft.Json.JsonConvert.SerializeObject(sesion);
                    Session["UsuarioSignalR"] = new System.Text.StringBuilder("{").Append(string.Format("\"Id_usuario\": {0}, \"Clave_Usuario\": \"{1}\"", string.Format("{0:0}", sesion.Usuario.Id_usuario), sesion.Usuario.Clave_usuario)).Append("}").ToString();
                    Session["GrupoNegocioSerializado"] = Newtonsoft.Json.JsonConvert.SerializeObject(grupoNegocioERP);
                    Session["GrupoNegocioCliente"] = new System.Text.StringBuilder("{").Append(string.Format("\"Nombre\": \"{0}\", \"Descripcion\": \"{1}\"", grupoNegocioERP.Nombre, grupoNegocioERP.Descripcion)).Append("}").ToString();

                    FormsAuthentication.SetAuthCookie(sesion.Usuario.Clave_usuario.ToUpper(), true);
                }
                else
                {
                    throw new Exception("Usuario no válido, no pudo iniciarse sesión.");
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}