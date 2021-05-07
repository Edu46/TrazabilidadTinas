using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class ManejadorLogsErrores
    {
        public static string CrearCarpeta()
        {
            string carpeta = "~/LogsErrores";

            carpeta = HttpContext.Current.Server.MapPath(carpeta);

            try
            {
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }
                return carpeta;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Falló el proceso: {0}", ex.ToString());
                return null;
            }
        }

        public static void GuardarLog(Exception ex)
        {
            string archivo = Path.Combine(CrearCarpeta(), DateTime.Now.ToString().Replace("/", "-").Substring(0, 10) + ".txt");

            StreamWriter sw = new StreamWriter(archivo, true);
            sw.WriteLine("Fecha y Hora: {0}", DateTime.Now);

            if (ex.InnerException != null)
            {
                sw.WriteLine("Excepción Interna");
                sw.WriteLine("Tipo: " + ex.InnerException.GetType().ToString());
                sw.WriteLine("Mensaje: " + ex.InnerException.Message);
                sw.WriteLine("Origen: " + ex.InnerException.Source);

                if (ex.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Rastreo de pila: " + ex.InnerException.StackTrace);
                }
            }
            sw.WriteLine("Excepción");
            sw.Write("Tipo: " + ex.GetType().ToString());
            sw.WriteLine("Mensaje: " + ex.Message);
            sw.WriteLine("Origen: " + Path.GetFileName(HttpContext.Current.Request.Url.AbsolutePath));

            if (ex.StackTrace != null)
            {
                sw.WriteLine("Rastreo de pila: " + ex.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }

        public static void GuardarLog(string elemento, string error)
        {
            string archivo = Path.Combine(CrearCarpeta(), DateTime.Now.ToString().Replace("/", "-").Substring(0, 10) + ".txt");

            StreamWriter sw = new StreamWriter(archivo, true);
            sw.WriteLine("Fecha y Hora: {0}", DateTime.Now);
            sw.WriteLine("Error en el elemento: " + elemento);
            sw.WriteLine("Origen: " + Path.GetFileName(HttpContext.Current.Request.Url.AbsolutePath));
            sw.WriteLine("Mensaje: " + error);
            sw.WriteLine();
            sw.Close();
        }
    }
}