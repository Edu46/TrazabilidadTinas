using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace LogisticaERP.Clases
{
	public class CFuncionesGenerales
	{
		#region Variables Globales de la Función
		private string s_Mensaje;
		private CComboInformacion st_InformacionCombo;
		#endregion
		#region Constructor
		public CFuncionesGenerales()
		{
			this.s_Mensaje = "";
			this.st_InformacionCombo = new CComboInformacion();
			this.st_InformacionCombo.b_Respuesta = false;
			this.st_InformacionCombo.s_Mensaje = "No se encontraron coincidencias.";
			this.st_InformacionCombo.obj_InformacionCombo = null;
		}
		#endregion
		#region Funciones Retorno
		public string ObtenerMensaje()
		{
			return this.s_Mensaje;
		}
		public CComboInformacion ObtenerInformacionCombo()
		{
			return this.st_InformacionCombo;
		}
		#endregion
		#region Funciones Generales Públicas
		public void LimpiarControles(ControlCollection st_Controles)
		{
			foreach (System.Web.UI.Control Control in st_Controles)
			{
				if (Control is TextBox)
				{
					((TextBox)Control).Text = string.Empty;
				}
				else if (Control is DropDownList)
				{
					((DropDownList)Control).ClearSelection();
				}
				else if (Control is RadioButtonList)
				{
					((RadioButtonList)Control).ClearSelection();
				}
				else if (Control is CheckBoxList)
				{
					((CheckBoxList)Control).ClearSelection();
				}
				else if (Control is RadioButton)
				{
					((RadioButton)Control).Checked = false;
				}
				else if (Control is ASPxCheckBox)
				{
					((ASPxCheckBox)Control).Checked = false;
				}
				else if (Control is HiddenField)
				{
					((HiddenField)Control).Value = string.Empty;
				}
				else if (Control is ASPxDateEdit)
				{
					((ASPxDateEdit)Control).Text = string.Empty;
				}
				else if(Control is ListBox)
				{
					((ListBox)Control).SelectedIndex = -1;
				}
				else if (Control.HasControls())
				{
					//Al entrar en esta parte indica si el Control contiene mas controles. Así ningún control se quedará sin ser limpiado.
					LimpiarControles(Control.Controls);
				}
			}
		}
		public bool ValidarCorreo(string s_Correo)
		{
			return Regex.IsMatch
			(s_Correo,
			@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
			@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
			RegexOptions.IgnoreCase,
			TimeSpan.FromMilliseconds(250));
		}
		public bool ConsultarInformacionCombo(eTipoBusqueda e_TiposCamposObtener, string s_NombreCombo, Object st_Parametros)
		{
			bool b_Respuesta = false;
			decimal? d_IdGenerico = (decimal?)null;
			try
			{
				#region Consulta Informacion Combo
				switch (e_TiposCamposObtener)
				{
					case eTipoBusqueda.CONSULTAR_DIVISIONES:
						{
							#region Divisiones del Usuario
							#endregion
						}
						break;
					case eTipoBusqueda.CONSULTAR_EMPRESAS:
						{
							#region Empresas del Usuario
							#endregion
						}
						break;
					case eTipoBusqueda.CONSULTAR_CATALOGO_ALMACENES:
						{
							#region Catálogo de Almacenes con LeadTime
							Logi_Planeacion_Reabastecimiento_Almacen FuncionesPlaneacion = new Logi_Planeacion_Reabastecimiento_Almacen();
							if (FuncionesPlaneacion.ConsultarCatalogoLeadTime(null, null))
							{
								b_Respuesta = true;
								this.st_InformacionCombo.b_Respuesta = true;
								this.st_InformacionCombo.s_Mensaje = "OK";
								this.st_InformacionCombo.obj_InformacionCombo =
								(
									from
										Elementos in FuncionesPlaneacion.ObtenerCatalogoLeadTime()
									group
										Elementos by new { Elementos.d_Id_Catalogo_Lead_Time_Almacen, Elementos.s_Codigo_Almacen }
										into
											sub_Elementos
										select new { sub_Elementos.Key.d_Id_Catalogo_Lead_Time_Almacen, sub_Elementos.Key.s_Codigo_Almacen }
								).ToList();
							}
							#endregion
						}
						break;
					default:
						{
							this.st_InformacionCombo.b_Respuesta = false;
							this.st_InformacionCombo.s_Mensaje = "Opción equivocada.";
						}
						break;
				}
				#endregion
			}
			catch (Exception ErrorExcepcion)
			{
				this.s_Mensaje = ErrorExcepcion.Message;
				this.s_Mensaje = "Ocurrió un problema al consultar la información de " + s_NombreCombo + ", favor de refrescar la página para cargar correctamente la información.";
			}
			return b_Respuesta;
		}
		public void LlenarCombo(DropDownList ddl_NombreCombo, Object st_InformacionCombo, string s_NombreCombo, string s_NombreIdOpcion, string s_TextoMostrar, string s_Inicial, bool b_AgregarMensaje)
		{
			//Esta función permite introducir información dinámicamente a un combo.
			if (s_Inicial == "")
			{
				s_Inicial = "Seleccione una Opción";
			}
			try
			{
				ddl_NombreCombo.DataSource = st_InformacionCombo;
				ddl_NombreCombo.DataValueField = s_NombreIdOpcion;
				ddl_NombreCombo.DataTextField = s_TextoMostrar;
				ddl_NombreCombo.DataBind();
				if (b_AgregarMensaje)
				{
					ddl_NombreCombo.Items.Insert(0, new ListItem() { Text = "..:: " + s_Inicial + " ::..", Value = "0" });
				}
			}
			catch (Exception ErrorExcepcion)
			{
				s_Inicial = ErrorExcepcion.Message;
				this.s_Mensaje = "Ocurrió un problema al agregar información al Combo " + s_NombreCombo;
			}
		}
		public void LlenarComboGrid(ASPxGridView GridConComboLlenar, Object st_InformacionCombo, string s_NombreColumna, string s_NombreCampoID, string s_NombreCampoTexto, string s_NombreCombo)
		{
			//Esta función permite introducir información dinámicamente a un combo.
			try
			{
				GridViewDataComboBoxColumn grid_NombreColumna = GridConComboLlenar.Columns[s_NombreColumna] as GridViewDataComboBoxColumn;
				grid_NombreColumna.PropertiesComboBox.DataSource = st_InformacionCombo;
				grid_NombreColumna.PropertiesComboBox.TextField = s_NombreCampoTexto;
				grid_NombreColumna.PropertiesComboBox.ValueField = s_NombreCampoID;
				grid_NombreColumna.PropertiesComboBox.ValueType = typeof(Int32);
				GridConComboLlenar.DataBind();
			}
			catch (Exception ErrorExcepcion)
			{
				s_NombreColumna = ErrorExcepcion.Message;
				this.s_Mensaje = "Ocurrió un problema al agregar información al Combo " + s_NombreCombo;
			}
		}
		public string ObtenerFecha(int i_TipoFecha, DateTime dt_Fecha)
		{
			string s_Fecha = string.Empty;
			switch(i_TipoFecha)
			{
				case 1:
					{
						s_Fecha = dt_Fecha.Year.ToString("D04") + "-" + dt_Fecha.Month.ToString("D02") + "-" + dt_Fecha.Day.ToString("D02");
					}
					break;
				default:
					{
						s_Fecha = "2015-01-01";
					}
					break;
			}
			return s_Fecha;
		}
		#endregion
		#region Funciones Generales públicas para Animaciones
		public string ObtenerDivCargarPagina(string s_ClaseColor)
		{
			return "<div class='css_Icono'><div class='" + s_ClaseColor + " css_Girar'><i class='fa fa-circle-o-notch' aria-hidden='true'></i></div></div>";
		}
		public string ObtenerDivCorrecto(string s_ClaseColor)
		{
			return "<div class='css_Icono'><div class='" + s_ClaseColor + "'><i class='fa fa-check-circle' aria-hidden='true'></i></div></div>";
		}
		public string ObtenerDivError(string s_ClaseColor)
		{
			return "<div class='css_Icono'><div class='" + s_ClaseColor + "'><i class='fa fa-exclamation-triangle' aria-hidden='true'></i></div></div>";
		}
		#endregion
		#region Estructuras
		public class CComboInformacion
		{
			[DataMember]
			public bool b_Respuesta { get; set; }
			[DataMember]
			public string s_Mensaje { get; set; }
			[DataMember]
			public Object obj_InformacionCombo { get; set; }
		}
		#endregion
		#region Enumeradores
		public enum eTipoBusqueda : int
		{
			[System.Runtime.Serialization.EnumMemberAttribute(Value = "CONSULTAR DIVISIONES")]
			CONSULTAR_DIVISIONES = 0,
			[System.Runtime.Serialization.EnumMemberAttribute(Value = "CONSULTAR EMPRESAS")]
			CONSULTAR_EMPRESAS = 1,
			[System.Runtime.Serialization.EnumMemberAttribute(Value = "CONSULTAR CATALOGO ALMACENES")]
			CONSULTAR_CATALOGO_ALMACENES = 2
		}
		#endregion
	}
}