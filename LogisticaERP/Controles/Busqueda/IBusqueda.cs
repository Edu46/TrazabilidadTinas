using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRUPOPINSA.Controles.Busqueda
{
    interface IBusqueda
    {

        DataTable Busqueda(List<string> FiltrosPersonalizados, string CamposConsultas, string Filtro, string Token, object TablaBusqueda, ParametrosBusqueda.ModuloBusqueda Modulo);

    }
}
