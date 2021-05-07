using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Script.Services;
using LogisticaERP.GrupoPinsaSOA;
using LogisticaERP.SeguridadERPSOA;
using LogisticaERP.Clases;

namespace LogisticaERP
{
    public partial class Inicio : PaginaBase
    {
        [WebMethod(enableSession: true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static dynamic UsuarioEmpresas()
        {
            List<Empresa> listaUsuarioEmpresas = null;

            listaUsuarioEmpresas = (List<Empresa>)System.Web.HttpContext.Current.Session["LOG_Empresas"];

            UsuarioSesion sesion = Newtonsoft.Json.JsonConvert.DeserializeObject<UsuarioSesion>(System.Web.HttpContext.Current.Session["UsuarioSesionSerializado"].ToString());

            if (!sesion.Usuario.Es_admin) //  Si el usuario es administrador tiene permiso a todo
            {
                if (listaUsuarioEmpresas != null && sesion.Usuario.ListaUsuarioEmpresa != null)
                {
                    var usuarioEmp = from l in listaUsuarioEmpresas
                                     join ue in sesion.Usuario.ListaUsuarioEmpresa on l.Id_empresa equals ue.Id_empresa
                                     select l;

                    listaUsuarioEmpresas = usuarioEmp.ToList();
                }
            }

            //Cargar solo las empresas que estan relacionadas con el grupo de negocio
            listaUsuarioEmpresas = (from lgn in Newtonsoft.Json.JsonConvert.DeserializeObject<GrupoNegocioERP>(System.Web.HttpContext.Current.Session["GrupoNegocioSerializado"].ToString()).ListaGrupoNegocioERPEmpresa
                                    join lue in listaUsuarioEmpresas on lgn.Id_empresa equals lue.Id_empresa
                                    orderby lue.Nombre_comercial
                                    select lue).ToList();

            //var emp = System.Web.HttpContext.Current.Session["LOG_Empresa"] as Empresa;
            //if (emp == null) { emp = new Empresa() { Id_empresa = 0 }; }

            Empresa emp = new Empresa() { Id_empresa = 0 };
            var IDEmpresa = System.Web.HttpContext.Current.Session["EmpresaID"];

            if (IDEmpresa != null)
            {
                emp = listaUsuarioEmpresas.Where(e => e.Id_empresa == Convert.ToInt32(IDEmpresa)).FirstOrDefault();
            }

            return new
            {
                listaEmpresas = listaUsuarioEmpresas,
                empresa = emp,
                resultado = true,
                mensaje = "Ok",
            };
        }

        [WebMethod(enableSession: true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static dynamic AsignarIDEmpresa(int IDEmpresa)
        {
            var em = System.Web.HttpContext.Current.Session["LOG_Empresas"] as List<Empresa>;
            System.Web.HttpContext.Current.Session["LOG_Empresa"] = em.Where(e => e.Id_empresa == IDEmpresa).FirstOrDefault() as Empresa;
            return new
            {
                empresa = System.Web.HttpContext.Current.Session["LOG_Empresa"],
                resultado = true,
                mensaje = "Ok",
            };
        }

        [WebMethod(enableSession: true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static dynamic ValidarSessionIDEmpresa()
        {
            var em = System.Web.HttpContext.Current.Session["LOG_Empresa"] as Empresa;
            var empresaID = 0;
            if (System.Web.HttpContext.Current.Session["EmpresaID"] != null)
            {
                empresaID = Convert.ToInt32(System.Web.HttpContext.Current.Session["EmpresaID"]);
            }
            return new
            {
                empresaID = em.Id_empresa,
                empresaIDOld = empresaID,
                resultado = true,
                mensaje = "Ok",
            };
        }

        [WebMethod(enableSession: true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static dynamic AsingnarOldIDEmpresa(int IDEmpresa)
        {
            System.Web.HttpContext.Current.Session["EmpresaID"] = IDEmpresa;
            return new
            {
                empresaID = System.Web.HttpContext.Current.Session["EmpresaID"],
                resultado = true,
                mensaje = "Ok",
            };
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}