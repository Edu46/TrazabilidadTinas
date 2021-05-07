using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GRUPOPINSA.Controles.Busqueda
{
    /// <summary>
    /// Clase donde se indican los campos por los cuales se puede filtrar
    /// </summary>
    [Serializable]
    public class CampoFiltrar
    {
        /// <summary>
        /// Representa el nombre que aparecera para que lo seleccione el usuario en los tokens
        /// </summary>
        public String Caption { get; set; }
        /// <summary>
        /// Nombre del campo al que representa el caption
        /// </summary>
        public String Campo { get; set; }
        /// <summary>
        /// Indica si se seleccionara por default cuando se muestre la pantalla de busqueda
        /// </summary>
        public Boolean Default { get; set; }

        public CampoFiltrar()
        {
            this.Campo = String.Empty;
            this.Campo = String.Empty;
            this.Default = false;
        }

        /// <summary>
        /// Inicializa la clase donde se indican los campos por los cuales se puede filtrar
        /// </summary>
        /// <param name="Caption">Representa el nombre que aparecera para que lo seleccione el usuario en los tokens</param>
        /// <param name="Campo">Nombre del campo al que representa el caption</param>
        /// <param name="Default">Indica si se seleccionara por default cuando se muestre la pantalla de busqueda</param>
        public CampoFiltrar(String Caption, String Campo, Boolean Default)
        {
            this.Caption = Caption;
            this.Campo = Campo;
            this.Default = Default;
        }
    }
}