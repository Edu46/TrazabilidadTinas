using InventariosERP.InventariosSOA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GRUPOPINSA.Controles.Busqueda
{
    public static class SessionHelperBusqueda
    {

        private static T Lee<T>(string variable)
        {
            object valor = HttpContext.Current.Session[variable];
            if (valor != null)
            {
                return (T)valor;
            }
            else
            {
                return default(T);
            }
        }

        private static void Escribir(string variable, object valor)
        {
            HttpContext.Current.Session[variable] = valor;
        }

        public static ParametrosBusqueda Parametros_de_busqueda
        {
            get
            {
                return Lee<ParametrosBusqueda>("Parametros_de_busqueda");
            }
            set
            {
                Escribir("Parametros_de_busqueda", value);
            }
        }

        public static DataTable Resultado_Busqueda
        {
            get
            {
                return Lee<DataTable>("ResultadoBusqueda");
            }
            set
            {
                Escribir("ResultadoBusqueda", value);
            }
        }

        public static List<ArticuloAyuda> Resultado_Busquedas_Articulos
        {
            get
            {
                return Lee<List<ArticuloAyuda>>("ResultadoBusquedasArticulos");
            }
            set
            {
                Escribir("ResultadoBusquedasArticulos", value);
            }
        }

        public static DataTable Resultado_Busqueda_Articulos_Existencia
        {
            get
            {
                return Lee<DataTable>("ResultadoBusquedasArticulosExistencia");
            }
            set
            {
                Escribir("ResultadoBusquedasArticulosExistencia", value);
            }
        }

        public static DataTable Resultado_Busqueda_Empleado
        {
            get
            {
                return Lee<DataTable>("ResultadoBusquedaEmpleado");
            }
            set
            {
                Escribir("ResultadoBusquedaEmpleado", value);
            }
        }

        public static List<ArticuloFabricanteNoParteBusqueda> Resultado_Busqueda_Articulos_Fabricantes
        {
            get
            {
                return Lee<List<ArticuloFabricanteNoParteBusqueda>>("ResultadoBusquedaArticulosFabricantes");
            }
            set
            {
                Escribir("ResultadoBusquedaArticulosFabricantes", value);
            }
        }

    }
}