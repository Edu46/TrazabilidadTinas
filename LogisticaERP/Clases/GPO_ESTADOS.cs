using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticaERP.GrupoPinsaSOA;
using System.ServiceModel;

namespace LogisticaERP.Clases
{
    public class GPO_ESTADOS
    {
        private readonly string _token;

        public GPO_ESTADOS()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
        }

        public List<Estado> ObtenerEstados(decimal? idEstado = null, string clave = null, string nombre = null, decimal? idPais = null, bool? activo = true)
        {
            var estados = new List<Estado>();
            try
            {
                using (var proxy = new GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient())
                {
                    estados = proxy.ObtieneEstado(idEstado, clave, nombre, idPais, activo).ToList();
                }

                return estados;
            }
            catch (FaultException<ExcepcionesServicioDLL> fautlException)
            {
                throw new Exception(fautlException.Detail.ExcDetalle.Mensaje, fautlException);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}