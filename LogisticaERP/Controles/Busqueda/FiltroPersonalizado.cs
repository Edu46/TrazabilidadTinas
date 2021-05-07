using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using InventariosERP.Clases;

namespace GRUPOPINSA.Controles.Busqueda
{
    [Serializable]
    public class FiltroPersonalizado
    {
        public enum eOperador
        {
            [Description("==")]
            Igual,
            [Description("!=")]
            Diferente,
            [Description(">")]
            Mayor,
            [Description("<")]
            Menor,
            [Description(">=")]
            MayorIgual,
            [Description("<=")]
            MenorIgual
        }
        public string Campo { get; set; }
        public string Filtro { get; set; }
        public eOperador Operador { get; set; }
        public FiltroPersonalizado() : this(string.Empty, string.Empty) { }
        public FiltroPersonalizado(string campo, string filtro) : this(campo, filtro, eOperador.Igual) { }
        public FiltroPersonalizado(string campo, string filtro, eOperador operador)
        {
            this.Campo = campo;
            this.Filtro = filtro;
            this.Operador = operador;
        }
        public string ObtieneCampoFiltro()
        {
            return String.Format("{0}|{1}|{2}", this.Campo, this.Filtro, this.Operador.Descripcion());
        }
    }
}