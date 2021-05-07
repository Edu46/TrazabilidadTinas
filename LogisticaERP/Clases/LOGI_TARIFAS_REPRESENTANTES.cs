using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticaERP.LogisticaSOA;
using System.ServiceModel;

namespace LogisticaERP.Clases
{
    public class LOGI_TARIFAS_REPRESENTANTES :ClaseBase
    {
        public TarifaRepresentante Tarifas { get; set; }
        public List<TarifaRepresentante> ListaTarifas { get; set; }
        public List<TarifaConfiguracion> ListaTarifasConf { get; set; }

        public LOGI_TARIFAS_REPRESENTANTES()
        {
            Tarifas = new TarifaRepresentante();
            ListaTarifas = new List<TarifaRepresentante>();
            ListaTarifasConf = new List<TarifaConfiguracion>();
        }

        [Serializable()]
        public class TarifasRepresentanteGrid
        {
            public decimal Id_index { get; set; }
            public decimal Id_tarifa_representante { get; set; }
            public decimal? Id_almacen_origen_ebs12 { get; set; }
            public decimal? Id_estado { get; set; }
            public decimal? Id_municipio { get; set; }
            public decimal? Id_ciudad { get; set; }
            public string Zona { get; set; }
            public decimal Kilometros { get; set; }
            public decimal Kilometros_min { get; set; }
            public decimal Cajas { get; set; }
            public decimal Costo_caja { get; set; }
            public string CodAlmacen { get; set; }
            public string Estado { get; set; }
            public string Ciudad { get; set; }


            public bool Activo { get; set; }
            public bool Borrado { get; set; }
            public bool Modifico { get; set; }

            public TarifasRepresentanteGrid()
            {
                Id_index = -1;
                Id_tarifa_representante = -1;
                Id_estado = (decimal?)null;
                Id_municipio = (decimal?)null;
                Id_almacen_origen_ebs12 = (decimal?)null;
                Zona = null;
                Cajas = 0;
                Costo_caja = 0;
                Activo = false;
                Modifico = false;
                Borrado = false;
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

                    resultado = logistica.GuardarTarifaRepresentante(Tarifas);
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

                    ListaTarifas = logistica.ObtenerTarifaRepresentante(Tarifas.Id_tarifa_representante <= 0 ? (decimal?)null : Tarifas.Id_tarifa_representante
                                                            ,Tarifas.Id_empresa <= 0 ? (decimal?)null : Tarifas.Id_empresa
                                                            ,Tarifas.Id_almacen_origen_ebs12 <= 0 ? (decimal?)null : Tarifas.Id_almacen_origen_ebs12
                                                            ,Tarifas.Id_estado <= 0 ? (decimal?)null : Tarifas.Id_estado
                                                            ,Tarifas.Id_municipio <= 0 ? (decimal?)null : Tarifas.Id_municipio
                                                            ,Tarifas.Activo).ToList();

                    if (ListaTarifas != null && ListaTarifas.Count() > 0)
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

        public List<TarifaRepresentante> GrabarListaTarifasRepresentante(List<TarifaRepresentante> ListaTarifas)
        {
            List<TarifaRepresentante> TarifasRepresentantes = new List<TarifaRepresentante>();

            try
            {
                using (LogisticaWCFAPPServiciosClient soa = new LogisticaWCFAPPServiciosClient())
                {
                    ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
                    TarifasRepresentantes = soa.GuardarListaTarifasRepresentantes(ListaTarifas.ToArray()).ToList();

                }
            }
            catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                throw Faultexc;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return TarifasRepresentantes;
        }

    }
}