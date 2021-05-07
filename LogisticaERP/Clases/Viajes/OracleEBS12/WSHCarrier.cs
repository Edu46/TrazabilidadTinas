using LogisticaERP.LogisticaSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;

namespace LogisticaERP.Clases.Viajes.OracleEBS12
{
    public class WSHCarrier
    {
        private readonly string _token;

        public WSHCarrier()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
        }

        public List<Carrier> ObtenerTransportistas(long? carrierID = null)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    return proxy.GetCarriers(carrierID).ToList();
                }
            }
            catch (FaultException<ExcepcionesServicioDLL> faultException)
            {
                throw new Exception(faultException.Detail.Mensaje, faultException);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}