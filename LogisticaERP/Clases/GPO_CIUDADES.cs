using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticaERP.GrupoPinsaSOA;
using System.ServiceModel;

namespace LogisticaERP.Clases
{
    public class GPO_CIUDADES
    {
        private readonly string _token;

        public GPO_CIUDADES()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
        }

        public List<Ciudad> ObtenerCiudades(decimal? idCiudad = null, string clave = null, string nombre = null, decimal? idMunicipio = null, bool? activo = true)
        {
            var ciudades = new List<Ciudad>();

            try
            {
                using (var proxy = new GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient())
                {
                    ciudades = proxy.ObtieneCiudad(idCiudad, clave, nombre, idMunicipio, activo).ToList();
                }

                return ciudades;
            }
            catch (FaultException<GrupoPinsaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                throw new Exception(Faultexc.Detail.Mensaje, Faultexc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}