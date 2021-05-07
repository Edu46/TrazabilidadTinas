using LogisticaERP.SeguridadERPSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace LogisticaERP.Clases
{
    public class SERP_USUARIOSESION
    {
        public UsuarioSesion ObtenerSesionUsuario(string usuario, string contrasenia)
        {
            UsuarioSesion sesion;

            try
            {
                using (var seguridadModulos = new SeguridadModulosERPServiceClient())
                {
                    string token = CodificarBase64(string.Format("{0}|{1}|{2}", usuario, contrasenia, usuario));

                    ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = token;
                    sesion = seguridadModulos.ObtenerUsuarioSesion(usuario, null, (short?)eTipoModulo.FORMAS_WEB);

                    if (sesion != null && sesion.Usuario != null)
                    {
                        sesion.Usuario.Credenciales = token;
                    }
                }

                return sesion;
            }
            catch (FaultException<SeguridadERPSOA.ExcepcionesServicioDLL> Faultexc)
            {
                throw new Exception(Faultexc.Detail.ExcDetalle.Mensaje, Faultexc);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public string CodificarBase64(string cadenaEntrada)
        {
            try
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(cadenaEntrada);
                return System.Convert.ToBase64String(bytes);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public string DecodificarBase64(string cadenaEntrada)
        {
            try
            {
                var bytes = System.Convert.FromBase64String(cadenaEntrada);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        private List<Usuario> ObtenerListaUsuarios(decimal? id_usuario, string clave_usuario, string nombreCompleto, decimal? id_empleado, decimal? id_rol, decimal?[] id_empresas)
        {
            List<Usuario> listaUsuarios;

            try
            {
                using (var seguridadModulos = new SeguridadModulosERPServiceClient())
                {
                    ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = HttpContext.Current.Session["Token"].ToString();

                    listaUsuarios = seguridadModulos.ObtenerUsuarios(id_usuario, clave_usuario, nombreCompleto, id_empleado, id_rol, id_empresas).ToList();
                }

                return listaUsuarios;

            }
            catch (FaultException<SeguridadERPSOA.ExcepcionesServicioDLL> Faultexc)
            {
                throw Faultexc;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<Usuario> ObtenerUsuarios(decimal? id_usuario, string clave_usuario, string nombreCompleto, decimal? id_empleado, decimal? id_rol, params decimal?[] id_empresas)
        {
            return ObtenerListaUsuarios(id_usuario, clave_usuario, nombreCompleto, id_empleado, id_rol, id_empresas);
        }

        public List<Usuario> ObtenerUsuariosPorEmpresa(params decimal?[] id_empresas)
        {
            return ObtenerListaUsuarios(null, null, null, null, null, id_empresas);
        }
    }
}