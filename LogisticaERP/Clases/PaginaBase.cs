using LogisticaERP.GrupoPinsaSOA;
using LogisticaERP.SeguridadERPSOA;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;

namespace LogisticaERP.Clases
{
    public class PaginaBase : System.Web.UI.Page
    {
        public static string Id_Modulo = System.Configuration.ConfigurationManager.AppSettings["Id_Modulo"];
        public static string NombreCacheEnsamblados = System.Configuration.ConfigurationManager.AppSettings["NombreCacheEnsamblados"];

        private string _token = string.Empty;
        private bool _esUsuarioGuardar = false;
        private bool _esUsuarioConsultar = false;
        private bool _esUsuarioAutorizar = false;
        private bool _esUsuarioBorrar = false;
        private bool _esUsuarioRevisar = false;
        private UsuarioSesion _usuarioSesion = null;

        public string Token { get { return _token; } private set { _token = value; } }
        public bool EsUsuarioGuardar { get { return _esUsuarioGuardar; } private set { _esUsuarioGuardar = value; } }
        public bool EsUsuarioConsultar { get { return _esUsuarioConsultar; } private set { _esUsuarioConsultar = value; } }
        public bool EsUsuarioAutorizar { get { return _esUsuarioAutorizar; } private set { _esUsuarioAutorizar = value; } }
        public bool EsUsuarioBorrar { get { return _esUsuarioBorrar; } private set { _esUsuarioBorrar = value; } }
        public bool EsUsuarioRevisar { get { return _esUsuarioRevisar; } private set { _esUsuarioRevisar = value; } }
        public UsuarioSesion UsuarioSesion { get { return _usuarioSesion; } private set { _usuarioSesion = value; } }

        /// <summary>
        /// Loads the ViewState from the persistence medium
        /// </summary>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            try
            {
                string compression = ConfigurationManager.AppSettings["viewStateCompression"];
                if (string.IsNullOrEmpty(compression)) compression = "true";
                if (bool.Parse(compression))
                {
                    string viewState = Request.Form["__VSTATE"];
                    if (viewState.EndsWith(",")) viewState = viewState.Substring(0, viewState.Length - 1);
                    byte[] bytes = Convert.FromBase64String(viewState);
                    bytes = Compressor.Decompress(bytes);

                    viewState = Convert.ToBase64String(bytes);
                    if (string.IsNullOrEmpty(viewState)) return null;
                    LosFormatter formatter = new LosFormatter();
                    return formatter.Deserialize(viewState);
                }
                else
                    return base.LoadPageStateFromPersistenceMedium();
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Saves the ViewState from the persistence medium
        /// </summary>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            StringWriter writer = new StringWriter();
            try
            {
                string compression = ConfigurationManager.AppSettings["viewStateCompression"];
                if (string.IsNullOrEmpty(compression)) compression = "true";
                if (bool.Parse(compression))
                {
                    LosFormatter formatter = new LosFormatter();
                    formatter.Serialize(writer, state);
                    string vState = writer.ToString();
                    byte[] bytes = Convert.FromBase64String(vState);
                    bytes = Compressor.Compress(bytes);
                    vState = Convert.ToBase64String(bytes);

                    System.Web.UI.ScriptManager sm = System.Web.UI.ScriptManager.GetCurrent(this);
                    if (sm != null && sm.IsInAsyncPostBack)
                        System.Web.UI.ScriptManager.RegisterHiddenField(this, "__VSTATE", vState);
                    else
                        Page.ClientScript.RegisterHiddenField("__VSTATE", vState);
                }
                else
                    base.SavePageStateToPersistenceMedium(state);
            }
            catch (Exception) { throw; }
            finally
            {
                if (writer != null) { writer.Dispose(); writer = null; }
            }
        }

        public PaginaBase()
        {
            base.Load += new EventHandler(PaginaBase_Load);
            base.PreInit += new EventHandler(PaginaBase_Preinit);
        }

        private void PaginaBase_Preinit(object sender, EventArgs e)
        {
            try
            {
                _token = (HttpContext.Current.Session["Token"] ?? string.Empty).ToString();
            }
            catch (Exception)
            {
                _token = string.Empty;
            }

            var masterPage = Page.Request.QueryString["masterPage"];
            if (!string.IsNullOrEmpty(masterPage))
            {
                Page.MasterPageFile = masterPage.ToString();
            }

            ValidarAcceso();
        }

        private void PaginaBase_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
            //    ValidarAcceso();
            //}
        }

        private void ValidarAcceso()
        {
            if (Session["UsuarioSesionSerializado"] != null)
            {
                //string pagina = this.Page.Request.Url.AbsolutePath.ToUpperInvariant().Split('/').Last();
                string pagina = this.Page.Request.Url.AbsolutePath.Substring(this.Page.Request.Url.AbsolutePath.IndexOf('/') == 0 ? 1 : 0).ToUpperInvariant();

                UsuarioSesion sesion = _usuarioSesion = Newtonsoft.Json.JsonConvert.DeserializeObject<UsuarioSesion>(System.Web.HttpContext.Current.Session["UsuarioSesionSerializado"].ToString());

                if (sesion.Usuario.Es_admin) //  Si el usuario es administrador tiene permiso a todo
                {
                    _esUsuarioAutorizar = _esUsuarioBorrar = _esUsuarioConsultar = _esUsuarioGuardar = _esUsuarioRevisar = sesion.Usuario.Es_admin;
                    return;
                }

                List<UsuarioFuncionalidad> listaUsuarioModuloFuncionalidad = sesion.Usuario.ListaUsuarioFuncionalidad.Where(x => x.Id_modulo == decimal.Parse(Id_Modulo)).ToList();
                //List<UsuarioFuncionalidad> listaUsuarioModuloFuncionalidad = sesion.Usuario.ListaUsuarioFuncionalidad.ToList();

                //UsuarioFuncionalidad usuarioFuncionalidad = listaUsuarioModuloFuncionalidad.Where(x => !string.IsNullOrEmpty(x.Uri) && pagina == x.Uri.ToUpperInvariant().Split('/').Last()).FirstOrDefault();
                //UsuarioFuncionalidad usuarioFuncionalidad = listaUsuarioModuloFuncionalidad.Where(x => !string.IsNullOrEmpty(x.Uri) && pagina == x.Uri.Substring(x.Uri.IndexOf('/') == 0 ? 1 : 0).ToUpperInvariant()).FirstOrDefault(); //PROBLEMAS CON FUNCIONALIDADES CON PARAMETRO QUERYSTRING
                UsuarioFuncionalidad usuarioFuncionalidad = listaUsuarioModuloFuncionalidad.Where(x => !string.IsNullOrEmpty(x.Uri) && x.Uri.Substring(x.Uri.IndexOf('/') == 0 ? 1 : 0).ToUpperInvariant().Contains(pagina)).FirstOrDefault();

                if (usuarioFuncionalidad == null)
                {
                    var masterPage = Page.Request.QueryString["masterPage"];

                    Response.Redirect(new StringBuilder("~/AccesoRestringido.aspx").Append(!string.IsNullOrEmpty(masterPage) ? "?masterPage=" + masterPage : string.Empty).ToString(), false);
                }
                else
                {
                    _esUsuarioAutorizar = usuarioFuncionalidad.Es_autorizar ?? false;
                    _esUsuarioBorrar = usuarioFuncionalidad.Es_borrar ?? false;
                    _esUsuarioConsultar = usuarioFuncionalidad.Es_consultar ?? false;
                    _esUsuarioGuardar = usuarioFuncionalidad.Es_guardar ?? false;
                    _esUsuarioRevisar = usuarioFuncionalidad.Es_revisar ?? false;

                    ValidarControles(sesion.Usuario, pagina);
                }
            }
        }

        private void ValidarControles(Usuario usuario, string pagina)
        {
            try
            {
                //var controles = usuario.ListaUsuarioFuncionalidadControlJerarquico.Where(x => !string.IsNullOrEmpty(x.Entity.Uri) && pagina == x.Entity.Uri.ToUpperInvariant().Split('/').Last());
                var controles = usuario.ListaUsuarioFuncionalidadControlJerarquico.Where(x => !string.IsNullOrEmpty(x.Entity.Uri) && pagina == x.Entity.Uri.Substring(x.Entity.Uri.IndexOf('/') == 0 ? 1 : 0).ToUpperInvariant());

                controles.ToList().ForEach(e =>
                {
                    ControlesUsuarioFuncionalidadRecursivo(this.Page.Master, e);
                });
            }
            catch (Exception exc)
            {
                string e = exc.Message;
            }
        }

        private void ControlesUsuarioFuncionalidadRecursivo(dynamic controlRaiz, JerarquiaDeUsuarioFuncionalidadControl jerarquiaDeUsuarioFuncionalidadControl)
        {
            AplicarPrivilegios(controlRaiz, jerarquiaDeUsuarioFuncionalidadControl);

            jerarquiaDeUsuarioFuncionalidadControl.ChildNodes.ToList().ForEach(e =>
            {
                dynamic control = ConvertirControl<Control>(e.Entity.Tipo_control_funcionalidad);

                if (control == null)
                {
                    AplicarPrivilegiosColumnaGrid(controlRaiz, jerarquiaDeUsuarioFuncionalidadControl.Entity.Identificador_control_funcionalidad, e);
                }

                ControlesUsuarioFuncionalidadRecursivo(controlRaiz, e);
            });
        }

        private void AplicarPrivilegiosColumnaGrid(dynamic controlRaiz, string identificadorControlPadre, JerarquiaDeUsuarioFuncionalidadControl elemento)
        {
            dynamic grid = FindControlRecursive(controlRaiz, identificadorControlPadre);

            try
            {
                grid.Columns[elemento.Entity.Identificador_control_funcionalidad].Visible = elemento.Entity.Es_visible ?? true;
            }
            catch (Exception) { }
        }

        private void AplicarPrivilegios(dynamic controlPadre, JerarquiaDeUsuarioFuncionalidadControl jerarquiaDeUsuarioFuncionalidadControl)
        {
            dynamic ctrl = FindControlRecursive(controlPadre, jerarquiaDeUsuarioFuncionalidadControl.Entity.Identificador_control_funcionalidad);

            if (ctrl != null)
            {
                try
                {
                    ctrl.ReadOnly = jerarquiaDeUsuarioFuncionalidadControl.Entity.Es_sololectura ?? false;
                }
                catch (Exception)
                {
                    try
                    {
                        if (true == (jerarquiaDeUsuarioFuncionalidadControl.Entity.Es_sololectura ?? false))
                        {
                            ctrl.Attributes.Add("readonly", "readonly");
                        }
                    }
                    catch (Exception) { }
                }

                try
                {
                    ctrl.Enabled = !jerarquiaDeUsuarioFuncionalidadControl.Entity.Es_deshabilitado ?? true;
                }
                catch (Exception)
                {
                    try
                    {
                        ctrl.Disabled = jerarquiaDeUsuarioFuncionalidadControl.Entity.Es_deshabilitado ?? true;
                    }
                    catch (Exception) { }
                }

                try
                {
                    ctrl.Visible = jerarquiaDeUsuarioFuncionalidadControl.Entity.Es_visible ?? true;
                }
                catch (Exception) { }
            }
        }

        private dynamic FindControlRecursive(dynamic root, string id)
        {
            try
            {
                if (root.ID == id)
                {
                    return root;
                }

                foreach (Control c in root.Controls)
                {
                    Control t = FindControlRecursive(c, id);
                    if (t != null)
                    {
                        return t;
                    }
                }
            }
            catch (Exception) { }

            return null;
        }

        private static Control ConvertirControl<T>(string type)
            where T : Control
        {
            try
            {
                Assembly asm = Assembly.GetAssembly(FindType(type));
                Type tp = asm.GetType(type, true, true);
                dynamic ctrl = asm.CreateInstance(type, true) as T;

                return ctrl;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Type FindType(string qualifiedTypeName)
        {
            Type t = Type.GetType(qualifiedTypeName);

            if (t != null)
            {
                return t;
            }
            else
            {
                var listaEnsamblados = HttpContext.Current.Cache[NombreCacheEnsamblados] as System.Reflection.Assembly[];
                foreach (Assembly asm in listaEnsamblados)
                {
                    t = asm.GetType(qualifiedTypeName);
                    if (t != null)
                    {
                        return t;
                    }
                }

                return null;
            }
        }
    }
}