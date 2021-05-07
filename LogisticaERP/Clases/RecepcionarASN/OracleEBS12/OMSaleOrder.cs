using LogisticaERP.LogisticaSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;

namespace LogisticaERP.Clases.RecepcionarASN.OracleEBS12
{
    public class OMSaleOrder
    {
        private readonly string _token;

        public OMSaleOrder()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
        }

        public List<SaleOrderDelivery> ObtenerPedidosVentasEnvios(decimal? idUnidadNegocio)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    return proxy.GetSalesOrdersDeliveries(idUnidadNegocio).ToList();
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