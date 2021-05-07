using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticaERP.LogisticaSOA;
using System.ServiceModel;

namespace LogisticaERP.Clases
{
    public class LOGI_CALCULOS_TARIFAS :ClaseBase
    {
        private readonly string _token;
        public CalculoTarifa CalculoTarifas { get; set; }
        public List<CalculoTarifa> ListaCalculoTarifas { get; set; }

        public LOGI_CALCULOS_TARIFAS()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
            CalculoTarifas = new CalculoTarifa();
            ListaCalculoTarifas = new List<CalculoTarifa>();
        }

        public override bool Grabar()
        {
            bool resultado = false;

            try
            {
                using (LogisticaWCFAPPServiciosClient logistica = new LogisticaWCFAPPServiciosClient())
                {
                    ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;

                    CalculoTarifas.Id_calculo_tarifa = logistica.GuardarCalculoTarifa(CalculoTarifas);

                    if (CalculoTarifas.Id_calculo_tarifa > 0)
                        resultado = true;
                }

            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faulexcep)
            {
                throw Faulexcep;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultado;
        }

        public override bool Busqueda()
        {
            bool resultado = false;

            try
            {
                using (LogisticaWCFAPPServiciosClient logistica = new LogisticaWCFAPPServiciosClient())
                {
                    ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;

                    ListaCalculoTarifas = logistica.ObtenerCalculoTarifa( CalculoTarifas.Num_viaje, CalculoTarifas.Pedido_venta, CalculoTarifas.Tranferencia).ToList();

                    if (ListaCalculoTarifas != null && ListaCalculoTarifas.Count() > 0)
                        resultado = true;
                }

            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faulexcep)
            {
                throw Faulexcep;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultado;
        }

        public decimal GrabarCalculoTarifaPorteador(CalculoTarifa calculoTarifa)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    return proxy.GuardarCalculoTarifa(calculoTarifa);
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

        public List<CalculoTarifa> ObtenerCalculosTarifasPorteadores(string numViaje, bool esPedidoVenta, bool esTransferencia)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    return proxy.ObtenerCalculoTarifa(numViaje, esPedidoVenta, esTransferencia).ToList();
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