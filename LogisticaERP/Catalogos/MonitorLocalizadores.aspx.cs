using LogisticaERP.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogisticaERP.Catalogos
{
    public partial class MonitorLocalizadores : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var localizadores = new LOG_MONITOR_LOCALIZADOR().ObtenerMonitorLocalizador();
            var cadenaHTML = new System.Text.StringBuilder();

            foreach (var localizador in localizadores)
            {
                cadenaHTML.Append(string.Format("<div class=\"card\"><div class=\"card-header\"><h1>{0}</h1></div><h1 style=\"font-weight:500 !important;\">{1}</h1></div>", localizador.Localizador, localizador.Espacios_disponibles));
            }

            mainbox.InnerHtml = cadenaHTML.ToString();
        }
    }
}