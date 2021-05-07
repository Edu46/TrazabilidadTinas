using System;
using System.Web;
using System.Linq;
using LogisticaERP.GrupoPinsaSOA;
using System.Collections.Generic;
using LogisticaERP.SeguridadERPSOA;

namespace LogisticaERP.Clases
{
    public class SessionHelper
    {
        public static HttpContext httpcontext = null;

        private static T Lee<T>(string variable)
        {

            object valor = null; // httpcontext != null ? httpcontext.Session[variable] : HttpContext.Current.Session[variable];

            if (httpcontext != null)
            {
                if (httpcontext.Session != null)
                {
                    valor = httpcontext.Session[variable];
                }
                else
                {
                    valor = HttpContext.Current.Session[variable];
                }
            }
            else
            {
                if (HttpContext.Current != null)
                {
                    valor = HttpContext.Current.Session[variable];
                }
                else
                {
                    valor = null;
                }
            }

            if (valor != null)
            {
                return (T)valor;
            }
            else
            {
                return default(T);
            }
        }

        public static Empresa Empresa
        {
            get
            {
                Empresa empresa = Lee<Empresa>("LOG_Empresa");

                if (empresa == null)
                {
                    UsuarioSesion sesion = Newtonsoft.Json.JsonConvert.DeserializeObject<UsuarioSesion>(System.Web.HttpContext.Current.Session["UsuarioSesionSerializado"].ToString());

                    var q = from ge in GrupoNegocioERP.ListaGrupoNegocioERPEmpresa select ge;

                    if (!sesion.Usuario.Es_admin) //  Si el usuario es administrador tiene permiso a todo
                    {
                        if (sesion.Usuario.ListaUsuarioEmpresa != null)
                        {
                            empresa = new GPO_EMPRESAS().ObtenerEmpresa(q.Join(sesion.Usuario.ListaUsuarioEmpresa, ge => ge.Id_empresa, ue => ue.Id_empresa, (ge, ue) => ue.Id_empresa).First());
                        }
                    }
                    else
                    {
                        empresa = new GPO_EMPRESAS().ObtenerEmpresa(q.Join(new GPO_EMPRESAS().ObtieneListaEmpresas(), ge => ge.Id_empresa, em => em.Id_empresa, (ge, em) => em.Id_empresa).First());
                    }
                }

                return empresa;
            }
            set
            {
                Escribir("LOG_Empresa", value);
            }
        }

        public static GrupoNegocioERP GrupoNegocioERP
        {
            get
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<GrupoNegocioERP>(System.Web.HttpContext.Current.Session["GrupoNegocioSerializado"].ToString()) ?? new GrupoNegocioERP();
            }
        }

        private static void Escribir(string variable, object valor)
        {
            if (httpcontext != null)
            {
                httpcontext.Session[variable] = valor;
            }
            else if (HttpContext.Current != null)
            {
                HttpContext.Current.Session[variable] = valor;
            }
        }
    }
}