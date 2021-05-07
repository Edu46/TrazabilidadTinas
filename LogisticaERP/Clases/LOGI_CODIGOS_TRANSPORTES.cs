using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticaERP.LogisticaSOA;
using System.ServiceModel;

namespace LogisticaERP.Clases
{
    public class LOGI_CODIGOS_TRANSPORTES :ClaseBase
    {
        private readonly string _token;
        public CodigoTransporte CodigosTarifas { get; set; }
        public List<CodigoTransporte> ListaCodigosTarifas { get; set; }

        public LOGI_CODIGOS_TRANSPORTES()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;

            CodigosTarifas = new CodigoTransporte();
            ListaCodigosTarifas = new List<CodigoTransporte>();
        }

        public List<CodigoTransporte> ObtenerCodigosTransportes(decimal? idCodigoTransporte = null, decimal? idEmpresa = null, string codigoTransporte = null, string descripcion = null, bool? activo = true)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    return proxy.ObtenerCodigoTransporte(idCodigoTransporte, idEmpresa, codigoTransporte, descripcion, activo).ToList();
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

        public override bool Grabar()
        {
            bool resultado = false;

            try
            {
                using (LogisticaWCFAPPServiciosClient logistica = new LogisticaWCFAPPServiciosClient())
                {
                    ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;

                    resultado = logistica.GuardarCodigoTransporte(CodigosTarifas);
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

                    ListaCodigosTarifas = logistica.ObtenerCodigoTransporte(CodigosTarifas.Id_codigo_transporte <= 0 ? (decimal?)null : CodigosTarifas.Id_codigo_transporte
                                                                            , CodigosTarifas.Id_empresa <= 0 ? (decimal?)null : CodigosTarifas.Id_empresa
                                                                            ,CodigosTarifas.Codigo_transporte
                                                                            ,CodigosTarifas.Descripcion
                                                                            ,CodigosTarifas.Activo).ToList();

                    if (ListaCodigosTarifas != null && ListaCodigosTarifas.Count() > 0)
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

    }
}