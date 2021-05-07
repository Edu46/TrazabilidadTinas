using LogisticaERP.LogisticaSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;

namespace LogisticaERP.Clases.RecepcionarASN
{
    public class LOGASN
    {
        private readonly string _token;

        public LOGASN()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
        }

        public List<ASN> ObtenerASNs()
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    return proxy.ObtenerASNs().ToList();
                }
            }
            catch (FaultException<ExcepcionesServicioDLL> faultException)
            {
                throw new Exception(faultException.Detail.ExcDetalle.Mensaje, faultException);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<ASN> ObtenerASNMaestroDetalle(decimal? idASN, string noANS, string noEnvio)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    return proxy.ObtenerASNMaestroDetalle(idASN, noANS, noEnvio).ToList();
                }
            }
            catch (FaultException<ExcepcionesServicioDLL> faultException)
            {
                throw new Exception(faultException.Detail.ExcDetalle.Mensaje, faultException);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void ActualizarASNEstadoDocumento(string noASN, decimal idEstadoDocumento, string noEnvio, string usuario)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    proxy.ActualizarASNEstadoDocumento(noASN, idEstadoDocumento, noEnvio, usuario);
                }
            }
            catch (FaultException<ExcepcionesServicioDLL> faultException)
            {
                throw new Exception(faultException.Detail.ExcDetalle.Mensaje, faultException);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}