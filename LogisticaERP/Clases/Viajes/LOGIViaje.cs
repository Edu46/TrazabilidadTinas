using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticaERP.LogisticaSOA;
using System.ServiceModel;
using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;

namespace LogisticaERP.Clases.Viajes
{
    public class LOGIViaje
    {
        private readonly string _token;

        public LOGIViaje()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
        }

        public List<Viaje> ObtenerViajes(decimal? idViaje, string tipoViaje, decimal? noViaje, string folioViaje,
            decimal? idEmpresa, string descripcion, string tipoAlmacenOracle, bool activo, DateTime? fechaInicio,
            DateTime? fechaFin, bool esConsultaLigera)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    return proxy.ObtenerViaje(idViaje, tipoViaje, noViaje, folioViaje, idEmpresa, descripcion, 
                        tipoAlmacenOracle, activo, fechaInicio, fechaFin, esConsultaLigera).ToList();
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

        public decimal Grabar(Viaje viaje)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    return proxy.GuardarViaje(viaje);
                }
            }
            catch (FaultException<ExcepcionesServicioDLL> faultException)
            {
                throw new Exception(faultException.Detail.Mensaje, faultException);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Eliminar(decimal idViaje)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    return proxy.EliminarViaje(idViaje);
                }
            }
            catch (FaultException<ExcepcionesServicioDLL> faultException)
            {
                throw new Exception(faultException.Detail.Mensaje, faultException);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<Viaje> ObtenerViajeBusqueda(decimal? idViaje, string tipoViaje, string folioViaje, decimal? idEmpresa,
        string descripcion, string tipoAlmacenOracle, decimal? idAlmacen, decimal? idProveedor, bool activo, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    proxy.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    return proxy.ObtenerViajeBusqueda(idViaje, tipoViaje, folioViaje, idEmpresa, descripcion, tipoAlmacenOracle, idAlmacen, idProveedor, activo, fechaInicio, fechaFin).ToList();
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