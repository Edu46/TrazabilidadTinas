using LogisticaERP.Clases.CalculoTarifaRepresentante.OracleCloud;
using LogisticaERP.LogisticaSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;

namespace LogisticaERP.Clases.CalculoTarifaRepresentante
{
    public class LOGICalculoTarifaRepresentante
    {        
        private readonly string _token;

        public LOGICalculoTarifaRepresentante()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
        }

        public LogisticaSOA.CalculoTarifaRepresentante ObtenerCalculoTarifaRepresentante(decimal idEmpresa, string numViaje, bool esConsultaLigera)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    return proxy.ObtenerCalculoTarifaRepresentante(idEmpresa, numViaje, esConsultaLigera);
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

        public decimal GuardarCalculoTarifaRepresentante(LogisticaSOA.CalculoTarifaRepresentante calculoTarifaRepresentante)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    return proxy.GuardarCalculoTarifaRepresentante(calculoTarifaRepresentante);
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

        public void ActualizarCalculoTarifaRepresentanteOC(decimal idCalculoTarifaRepresentante, decimal idOrdenCompra, string folioOrdenCompra)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    proxy.ActualizarCalculoTarifaRepresentanteOC(idCalculoTarifaRepresentante, idOrdenCompra, folioOrdenCompra);
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