using LogisticaERP.LogisticaSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using TokenWCF = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;

namespace LogisticaERP.Clases
{
    public class LOG_MONITOR_LOCALIZADOR : ClaseBase
    {
        public override bool Grabar()
        {
            throw new NotImplementedException();
        }

        public override bool Busqueda()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MonitorLocalizador> ObtenerMonitorLocalizador()
        {
            try
            {



                using (var logistica = new LogisticaWCFAPPServiciosClient())
                {
                    TokenWCF.AutenticacionHeaderInfo.Token = Token;
                    logistica.InnerChannel.OperationTimeout = new TimeSpan(0, 5, 0);
                    return logistica.ObtenerMonitorLocalizador();
                }
            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> faultException)
            {
                throw new Exception(faultException.Detail.ExcDetalle.Mensaje);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}