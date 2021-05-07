using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticaERP.GrupoPinsaSOA;
using System.ServiceModel;
using System.Globalization;

namespace LogisticaERP.Clases
{
    public class GPO_MUNICIPIOS
    {
        private readonly string _token;

        public GPO_MUNICIPIOS()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
        }

        public List<Municipio> ObtenerMunicipios(decimal? idMunicipio = null, string clave = null, string nombre = null, decimal? idEstado = null, bool? activo = true)
        {
            var municipios = new List<Municipio>();

            try
            {
                using (var proxy = new GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient())
                {
                    municipios = proxy.ObtenerMunicipio(idMunicipio, clave, nombre, idEstado, activo).ToList();
                }

                return municipios;
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