using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases
{
    public abstract class ClaseBase
    {
        private string _token = string.Empty;
        public string Token { get { return _token; } private set { _token = value; } }

        public HttpContext contextoSesion { get; set; }

        public ClaseBase()
        {
            try
            {
                contextoSesion = HttpContext.Current;
                _token = contextoSesion.Session["Token"].ToString();
            }
            catch (Exception)
            {
                _token = string.Empty;
            }
        }

        public ClaseBase(HttpContext state)
        {
            try
            {
                contextoSesion = state;
                _token = state.Session["Token"].ToString();
            }
            catch (Exception)
            {
                _token = string.Empty;
            }
        }

        abstract public bool Grabar();
        abstract public bool Busqueda();
    }
}