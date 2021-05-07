using LogisticaERP.LogisticaSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;

namespace LogisticaERP.Clases.RecepcionarASN
{
    public class LOGASNDetalleDistribucion
    {
        private readonly string _token;

        public LOGASNDetalleDistribucion()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
        }

        public void GuardarASNDetalleDistribucion(string noASN, string noEnvio, List<ASNDetalleDistribucion> asnDetallesDitribuciones)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    proxy.GuardarASNDetalleDistribucion(noASN, noEnvio, asnDetallesDitribuciones.ToArray());
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