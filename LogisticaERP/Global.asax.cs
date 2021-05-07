using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace LogisticaERP
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Eliminar todos los archivos temporales
            Elimina_Archivos_Temporales(System.IO.Path.GetTempPath(), "*LOG_temp_*.*");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //Eliminar todos los archivos temporales
            Elimina_Archivos_Temporales(System.IO.Path.GetTempPath(), "*LOG_temp_*.*");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //Eliminar todos los archivos temporales de la sesión
            Elimina_Archivos_Temporales(System.IO.Path.GetTempPath(), Session.SessionID + "_*.*");
        }

        protected void Elimina_Archivos_Temporales(string ubicacion, string criterio)
        {
            string[] archivoList = Directory.GetFiles(ubicacion, criterio);

            try
            {
                foreach (string f in archivoList)
                {
                    System.IO.File.Delete(f);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}