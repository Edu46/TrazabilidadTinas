using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.TrazabilidadTinas
{
    public class FolioIdentificador
    {
        public int idFolio { get; set; }
        public string folioMP { get; set; }
        public string razonSocial { get; set; }
    }

    public class ListaFolioIdentificador
    {
        public List<FolioIdentificador> folios { get; set; }
    }
}