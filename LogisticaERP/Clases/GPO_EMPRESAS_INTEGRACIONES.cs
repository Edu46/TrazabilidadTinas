using LogisticaERP.GrupoPinsaSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;

namespace LogisticaERP.Clases
{
    public class GPO_EMPRESAS_INTEGRACIONES
    {
        private readonly string _token;

        public GPO_EMPRESAS_INTEGRACIONES()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
        }

        public List<EmpresaIntegracion> ObtenerEmpresasIntegraciones(decimal? idEmpresaIntegracion, decimal? idEmpresaSIP, decimal? idEmpresaEBS, decimal? idEmpresaEBS12,
            decimal? idEmpresaCloud, string claveCloud, string claveEBS)
        {
            List<EmpresaIntegracion> empresas = new List<EmpresaIntegracion>();

            try
            {
                using (var grupopinsa = new GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient())
                {
                    ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = _token;
                    empresas = grupopinsa.ObtenerEmpresasIntegraciones(idEmpresaIntegracion, idEmpresaSIP, idEmpresaEBS, idEmpresaEBS12, idEmpresaCloud, claveCloud, claveEBS).ToList();
                }

                return empresas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}