using Header = ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader;
using LogisticaERP.LogisticaSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace LogisticaERP.Clases
{
    public class LOGI_TARIFAS : ClaseBase
    {
        private readonly string _token;
        public Tarifa Tarifas { get; set; }
        public List<Tarifa> ListaTarifas { get; set; }
        public List<TarifaConfiguracion> ListaTarifasConf { get; set; }

        public LOGI_TARIFAS()
        {
            _token = (string)HttpContext.Current.Session["Token"];
            Header.AutenticacionHeaderInfo.Token = _token;
            Tarifas = new Tarifa();
            ListaTarifas = new List<Tarifa>();
            ListaTarifasConf = new List<TarifaConfiguracion>();
        }

        public override bool Grabar()
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    return proxy.GuardarTarifa(Tarifas);
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

        public override bool Busqueda()
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    ListaTarifas = proxy.ObtenerTarifa(
                        Tarifas.Id_tarifa <= 0 ? (decimal?)null : Tarifas.Id_tarifa
                        ,Tarifas.Id_empresa <= 0 ? (decimal?)null : Tarifas.Id_empresa
                        ,Tarifas.Id_estado <= 0 ? (decimal?)null : Tarifas.Id_estado
                        ,Tarifas.Id_municipio <= 0 ? (decimal?)null : Tarifas.Id_municipio
                        ,Tarifas.Id_ciudad <= 0 ? (decimal?)null : Tarifas.Id_ciudad
                        ,Tarifas.Id_tipo_tarifa <= 0 ? (decimal?)null : Tarifas.Id_tipo_tarifa
                        ,Tarifas.Id_codigo_transporte <= 0 ? (decimal?)null : Tarifas.Id_codigo_transporte
                        ,Tarifas.Nombre_proveedor_ebs12
                        ,Tarifas.Nombre_almacen_ebs12
                        ,Tarifas.Id_almacen_origen_ebs12 <= 0 ? (decimal?)null : Tarifas.Id_almacen_origen_ebs12
                        ,Tarifas.Id_proveedor_ebs12 <= 0 ? (decimal?)null : Tarifas.Id_proveedor_ebs12
                        ,Tarifas.Backhaul
                        ,Tarifas.Activo
                    ).ToList();

                    return ListaTarifas != null && ListaTarifas.Count() > 0;
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

        public bool GrabarListaTarifas(List<Tarifa> tarifas)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    return proxy.GuardarListaTarifas(tarifas.ToArray());
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

        public bool BusquedaConfiguracion(decimal? idTarifa, decimal? idEmpresa, string centroCosto)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    ListaTarifasConf = proxy.ObtenerTarifaConfiguracion(idTarifa, idEmpresa, centroCosto).ToList();
                    return ListaTarifasConf != null && ListaTarifasConf.Count() > 0;
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

        public TarifaConfiguracion ObtenerTarifaConfiguracion(decimal? idTarifa, decimal? idEmpresa, string centroCosto)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    return proxy.ObtenerTarifaConfiguracion(idTarifa, idEmpresa, centroCosto).FirstOrDefault();
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

        public List<Tarifa> ObtenerTarifa(decimal? idTarifa, decimal? idEmpresa, decimal? idEstado, decimal? idMunicipio, decimal? idCiudad,
            decimal? idTipoTarifa, decimal? idCodigoTransporte, string proveedorEBS12, string almacenEBS12, decimal? idAlmacenOrigenEBS12,
            decimal? idProveedorEBS12, bool backhaul, bool activo)
        {
            try
            {
                using (var proxy = new LogisticaWCFAPPServiciosClient())
                {
                    return proxy.ObtenerTarifa(idTarifa, idEmpresa, idEstado, idMunicipio, idCiudad, idTipoTarifa, idCodigoTransporte,
                        proveedorEBS12, almacenEBS12, idAlmacenOrigenEBS12, idProveedorEBS12, backhaul, activo).ToList();
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

        #region Classes

        [Serializable()]
        public class TarifasGrid
        {
            public TarifasGrid()
            {
                Id_index = -1;
                Id_tarifa = -1;
                Id_estado = (decimal?)null;
                Id_municipio = (decimal?)null;
                Id_ciudad = (decimal?)null;
                Id_codigo_transporte = (decimal?)null;
                Modifico = false;
                Borrado = false;
                Backhaul = false;
                Activo = true;
            }

            public decimal Id_index { get; set; }
            public decimal Id_tarifa { get; set; }
            public decimal? Id_estado { get; set; }
            public decimal? Id_municipio { get; set; }
            public decimal? Id_ciudad { get; set; }
            public decimal Id_tipo_tarifa { get; set; }
            public decimal? Id_codigo_transporte { get; set; }
            public decimal Id_proveedor_ebs12 { get; set; }
            public decimal Id_almacen_origen_ebs12 { get; set; }
            public decimal Monto_tarifa { get; set; }
            public decimal Monto_caseta { get; set; }
            public string Proveedor { get; set; }
            public string Almacen { get; set; }
            public DateTime Fecha_inicio { get; set; }
            public DateTime Fecha_fin { get; set; }
            public bool Activo { get; set; }
            public bool Borrado { get; set; }
            public bool Modifico { get; set; }
            public bool Backhaul { get; set; }
        }

        #endregion
    }
}