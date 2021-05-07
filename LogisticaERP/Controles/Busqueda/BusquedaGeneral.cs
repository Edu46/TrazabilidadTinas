using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventarioSOA = InventariosERP.InventariosSOA;
using GrupoPinsaSOA = InventariosERP.GrupoPinsaSOA;
using CompraSOA = InventariosERP.ComprasSOA;
using MantenimientoSOA = InventariosERP.MantenimientoSOA;
using ProyectoSOA = InventariosERP.ProyectosSOA;
using SeguridadERPSOA = InventariosERP.SeguridadERPSOA;
using GRUPOPINSA.Controles.Busqueda;
using System.Data;
using System.Reflection;
using System.Collections;
using InventariosERP.Clases;
using System.ServiceModel;

namespace GRUPOPINSA.Controles.Busqueda
{
    public class BusquedaGeneral : IBusqueda
    {
        private object listaResultados = new object();

        /// <summary>
        /// Metodo principal para la busqueda avanzada
        /// </summary>
        /// <param name="FiltrosPersonalizados">Filtros personalizados para la busqueda avanzada</param>
        /// <param name="CamposConsultas">Campos que se mostraran en la respuesta</param>
        /// <param name="Filtro">Filtro que selecciona que usuario para la seleccion de registros</param>
        /// <param name="Token">Token para el permiso del usuario</param>
        /// <param name="TablaBusqueda">Tabla sobre la cual se realizara la busqueda</param>
        /// <param name="Modulo">Modulo del servicio al cual se aplicara la busqueda</param>
        /// <returns></returns>
        public DataTable Busqueda(List<string> FiltrosPersonalizados, string CamposConsultas, string Filtro, string Token, object TablaBusqueda, ParametrosBusqueda.ModuloBusqueda Modulo)
        {
            DataTable resultado = new DataTable();
            object[] parametros;
            try
            {

                
                if (Modulo == ParametrosBusqueda.ModuloBusqueda.INVENTARIOS)
                {
                    parametros = new object[] { new InventarioSOA.BusquedaAvanzada()
                    { 
                        FiltroPersonalizado = FiltrosPersonalizados.ToArray(), 
                        CamposConsulta= CamposConsultas.Split(new Char[] { ',' }), 
                        TablaBusqueda= (InventarioSOA.eTablasBusqueda)Convert.ToInt32(TablaBusqueda),
                        Filtro= Filtro
                    }};
                    resultado = this.Busqueda<InventarioSOA.ResultadoBusquedaAvanzada, InventarioSOA.InventariosWCFAPPServiciosInventariosServiceClient, InventarioSOA.ExcepcionesServicioDLL>(Token, parametros);
                }
                else if(Modulo == ParametrosBusqueda.ModuloBusqueda.GRUPOPINSA)
                {
                    parametros = new object[] { new GrupoPinsaSOA.BusquedaAvanzada()
                    { 
                        FiltroPersonalizado = FiltrosPersonalizados.ToArray(), 
                        CamposConsulta= CamposConsultas.Split(new Char[] { ',' }), 
                        TablaBusqueda= (GrupoPinsaSOA.eTablasBusqueda)Convert.ToInt32(TablaBusqueda),
                        Filtro= Filtro
                    }};
                    resultado = this.Busqueda<GrupoPinsaSOA.ResultadoBusquedaAvanzada, GrupoPinsaSOA.GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient, GrupoPinsaSOA.ExcepcionesServicioDLL>(Token, parametros);
                }
                else if (Modulo == ParametrosBusqueda.ModuloBusqueda.COMPRAS)
                {
                    parametros = new object[] { new CompraSOA.BusquedaAvanzada()
                    { 
                        FiltroPersonalizado = FiltrosPersonalizados.ToArray(), 
                        CamposConsulta= CamposConsultas.Split(new Char[] { ',' }), 
                        TablaBusqueda= (CompraSOA.eTablasBusqueda)Convert.ToInt32(TablaBusqueda),
                        Filtro= Filtro
                    }};
                    resultado = this.Busqueda<CompraSOA.ResultadoBusquedaAvanzada, CompraSOA.ComprasWCFAPPServiciosComprasServiceClient, CompraSOA.ExcepcionesServicioDLL>(Token, parametros);
                }
                else if (Modulo == ParametrosBusqueda.ModuloBusqueda.MANTENIMIENTO)
                {
                    parametros = new object[] { new MantenimientoSOA.BusquedaAvanzada()
                    { 
                        FiltroPersonalizado = FiltrosPersonalizados.ToArray(), 
                        CamposConsulta= CamposConsultas.Split(new Char[] { ',' }), 
                        TablaBusqueda= (MantenimientoSOA.eTablasBusqueda)Convert.ToInt32(TablaBusqueda),
                        Filtro= Filtro
                    }};
                    resultado = this.Busqueda<MantenimientoSOA.ResultadoBusquedaAvanzada, MantenimientoSOA.MantenimientoServiceClient, MantenimientoSOA.ExcepcionesServicioDLL>(Token, parametros);
                }
                else if (Modulo == ParametrosBusqueda.ModuloBusqueda.PROYECTOS)
                {
                    parametros = new object[] { new ProyectoSOA.BusquedaAvanzada()
                    { 
                        FiltroPersonalizado = FiltrosPersonalizados.ToArray(), 
                        CamposConsulta= CamposConsultas.Split(new Char[] { ',' }), 
                        TablaBusqueda= (ProyectoSOA.eTablasBusqueda)Convert.ToInt32(TablaBusqueda),
                        Filtro= Filtro
                    }};
                    resultado = this.Busqueda<ProyectoSOA.ResultadoBusquedaAvanzada, ProyectoSOA.ProyectosWCFAPPServiciosProyectosServiceClient, ProyectoSOA.ExcepcionesServicioDLL>(Token, parametros);
                }
                else if (Modulo == ParametrosBusqueda.ModuloBusqueda.SEGURIDAD)
                {
                    parametros = new object[] { new SeguridadERPSOA.BusquedaAvanzada()
                    { 
                        FiltroPersonalizado = FiltrosPersonalizados.ToArray(), 
                        CamposConsulta= CamposConsultas.Split(new Char[] { ',' }), 
                        TablaBusqueda= (SeguridadERPSOA.eTablasBusqueda)Convert.ToInt32(TablaBusqueda),
                        Filtro= Filtro
                    }};
                    resultado = this.Busqueda<SeguridadERPSOA.ResultadoBusquedaAvanzada, SeguridadERPSOA.SeguridadModulosERPServiceClient, SeguridadERPSOA.ExcepcionesServicioDLL>(Token, parametros);
                }

                return resultado;
            }            
            catch (FaultException<InventarioSOA.ExcepcionesServicioDLL> FaultexcInventarios)
            {
                throw FaultexcInventarios;
            }
            catch (FaultException<GrupoPinsaSOA.ExcepcionesServicioDLL> FaultexcGpoPinsa)
            {
                throw FaultexcGpoPinsa;
            }
            catch (FaultException<CompraSOA.ExcepcionesServicioDLL> FaultexcCompras)
            {
                throw FaultexcCompras;
            }
            catch (FaultException<MantenimientoSOA.ExcepcionesServicioDLL> FaultexcMantto)
            {
                throw FaultexcMantto;
            }
            catch (FaultException<ProyectoSOA.ExcepcionesServicioDLL> FaultexcProyectos)
            {
                throw FaultexcProyectos;
            }
            catch (FaultException<SeguridadERPSOA.ExcepcionesServicioDLL> FaultexcSeguridad)
            {
                throw FaultexcSeguridad;
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

       /// <summary>
       /// Metodo para la busqueda avanzada
       /// </summary>
       /// <typeparam name="T">Tipo de respuesta para el servicio correspondiente</typeparam>
       /// <typeparam name="U">Servicio SOA al cual va a realizar la peticion de la busqueda avanzada</typeparam>
       /// <param name="Token">Token para el permiso del usuario</param>
       /// <param name="Parametros">Parametros que se envian a la busqueda avanzada</param>
       /// <returns></returns>
        private DataTable Busqueda<T, U, R>(string Token, object[] Parametros) where U : class where R : class
        {
            try
            {

                var res = this.ExecuteBusquedaAvanzada<T, U, R>(Token, Parametros);

                return this.ConvierteResultadoClase<T>(res);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<T> demostracion<T>()
        {
            List<T> listaDemostracion = null;
            return listaDemostracion;
        }

        /// <summary>
        /// Metodo de busqueda avanzada el cual regresa una lista del tipo especificado
        /// </summary>
        /// <typeparam name="T">Tipo de lista que regresara la busqueda avanzada</typeparam>
        /// <param name="FiltrosPersonalizados">Filtros personalizados para la busqueda avanzada</param>
        /// <param name="CamposConsultas">Campos que se mostraran en la respuesta</param>
        /// <param name="Filtro">Filtro que selecciona que usuario para la seleccion de registros</param>
        /// <param name="Token">Token para el permiso del usuario</param>
        /// <param name="TablaBusqueda">Tabla sobre la cual se realizara la busqueda</param>
        /// <param name="Modulo">Modulo del servicio al cual se aplicara la busqueda</param>
        /// <returns></returns>
        public List<T> Busqueda<T>(List<string> FiltrosPersonalizados, string CamposConsultas, string Filtro, string Token, object TablaBusqueda, ParametrosBusqueda.ModuloBusqueda Modulo)
        {
            List<T> resultado = new List<T>();
            object[] Parametros;
            try
            {

                if (Modulo == ParametrosBusqueda.ModuloBusqueda.INVENTARIOS)
                {
                    Parametros = new object[] { new InventarioSOA.BusquedaAvanzada()
                    { 
                        FiltroPersonalizado = FiltrosPersonalizados.ToArray(), 
                        CamposConsulta= CamposConsultas.Split(new Char[] { ',' }), 
                        TablaBusqueda= (InventarioSOA.eTablasBusqueda)Convert.ToInt32(TablaBusqueda),
                        Filtro= Filtro
                    }};
                    resultado = this.BusquedaLista<T, InventarioSOA.ResultadoBusquedaAvanzada, InventarioSOA.InventariosWCFAPPServiciosInventariosServiceClient, InventarioSOA.ExcepcionesServicioDLL>(Token, Parametros);
                }

                
                
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para la busqueda avanzada que regresa una lista del tipo especificado
        /// </summary>
        /// <typeparam name="T">Tipo de lista que regresara la busqueda avanzada</typeparam>
        /// <typeparam name="K">Tipo de respuesta para el servicio especificado</typeparam>
        /// <typeparam name="U">Servicio SOA al cual va a realizar la peticion de la busqueda avanzada</typeparam>
        /// <param name="Token">Token para el permiso del usuario</param>
        /// <param name="Parametros">Parametros que se envian a la busqueda avanzada</param>
        /// <returns></returns>
        private List<T> BusquedaLista<T,K,U, R>(string Token, object[] Parametros) where U : class where R : class
        {
            List<T> resultado = new List<T>();
            try
            {
                
                var res = this.ExecuteBusquedaAvanzada<K, U, R>(Token, Parametros);
                resultado = this.ConvierteResultadoClase<List<T>, K>(res);

                return resultado;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Metodo que ejecuta la busqueda avanzada desde el servicio especificado
        /// </summary>
        /// <typeparam name="T">Tipo de respuesta para el servicio correspondiente</typeparam>
        /// <typeparam name="U">Servicio SOA al cual va a realizar la peticion de la busqueda avanzada</typeparam>
        /// <param name="Token">Token para el permiso del usuario</param>
        /// <param name="Parametros">Parametros que se envian a la busqueda avanzada</param>
        /// <returns></returns>
        private T ExecuteBusquedaAvanzada<T, U, R>(string Token, object[] Parametros) where U : class where R : class
        {
            try
            {
                var soa = Activator.CreateInstance(typeof(U), new object[] { }) as U;
                ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
                MethodInfo methodInfo = typeof(U).GetMethod("BusquedaAvanzada");
                object respuesta = methodInfo.Invoke(soa, Parametros);
                return (T)respuesta;
            }            
            catch (Exception ex)
            {
                throw (System.ServiceModel.FaultException<R>)ex.InnerException;

               // throw ex;
            }
        }

        private DataTable ConvierteResultadoClase<T>(T resultado)
        {
            String datos = String.Empty;
            DataTable tablaResultado = new DataTable();
            try
            {

                string TipoEntidad = string.Empty;
                string Respuesta = string.Empty;

                TipoEntidad = resultado.GetType().InvokeMember("TipoEntidad", BindingFlags.GetProperty, null, resultado, null).ToString();
                Respuesta = resultado.GetType().InvokeMember("Respuesta", BindingFlags.GetProperty, null, resultado, null).ToString();

                Type tipo_respuesta = Type.GetType(System.Reflection.Assembly.GetAssembly(typeof(T)).GetTypes().SingleOrDefault(x => x.FullName == resultado.GetType().Namespace + "." + TipoEntidad).FullName);

                Type genericClass = typeof(convertir<>);
                Type constructedClass = genericClass.MakeGenericType(tipo_respuesta);
                var cctor = constructedClass.GetConstructor(new Type[] { typeof(string) });
                var instance = cctor.Invoke(new object[] { Respuesta });

                tablaResultado = constructedClass.InvokeMember("Respuesta", System.Reflection.BindingFlags.GetProperty, null, instance, null) as DataTable;
                this.listaResultados = constructedClass.InvokeMember("ListaResultado", System.Reflection.BindingFlags.GetProperty, null, instance, null);

                return tablaResultado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private T ConvierteResultadoClase<T, K>(K resultado) 
        {
            try
            {
                string TipoEntidad = string.Empty;
                string Respuesta = string.Empty;

                TipoEntidad = resultado.GetType().InvokeMember("TipoEntidad", BindingFlags.GetProperty, null, resultado, null).ToString();
                Respuesta = resultado.GetType().InvokeMember("Respuesta", BindingFlags.GetProperty, null, resultado, null).ToString();


                var datos = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Respuesta));
                var res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(datos);


                return (T)res;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public object FindItem(decimal Id, string PrimaryKey)
        {
            object resultado = new object();
			decimal d_Valor = 0;
            try
            {
                IEnumerable enume = this.listaResultados as IEnumerable;

                foreach (object l in enume)
                {
					d_Valor = Convert.ToDecimal(l.GetType().GetProperty(PrimaryKey).GetValue(l));
                    if (d_Valor == Id)
                    {
                        resultado = l;
                        break;
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public object FindItem(string Id, string PrimaryKey)
        {
            object resultado = new object();
            try
            {
                IEnumerable enume = this.listaResultados as IEnumerable;

                foreach (object l in enume)
                {
                    if (l.GetType().GetProperty(PrimaryKey).GetValue(l).ToString() == Id)
                    {
                        resultado = l;
                        break;
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }


    public class convertir<T>
    {
        public DataTable Respuesta { get; set; }
        public Object ListaResultado { get; set; }

        public convertir(string respuesta_soa)
        {
            var datos = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(respuesta_soa));
            var d = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(datos);
            this.ListaResultado = d;
            this.Respuesta = d.ToDataTable();
        }

    }

}