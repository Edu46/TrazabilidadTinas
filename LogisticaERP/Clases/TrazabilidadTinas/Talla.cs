using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class Talla
    {
        public long idProducto { get; set; }
        public string clave { get; set; }
        public int claveRapida { get; set; }
        public string producto { get; set; }
        public string claveERP { get; set; }
        public string identificadorProd { get; set; }
        public string idTallas { get; set; }
        public string talla { get; set; }
        public string idVariedad { get; set; }
        public string variedad { get; set; }
        public string claveVariedad { get; set; }
    }
}