using System;
using System.Web;
using System.Linq;
using System.Web.UI;
using LogisticaERP.Clases;
using System.Web.UI.WebControls;
using LogisticaERP.LogisticaSOA;
using System.Collections.Generic;
using LogisticaERP.GrupoPinsaSOA;
using System.Runtime.Serialization;

namespace LogisticaERP.Catalogos.OracleCloud.popup
{
	public partial class ObtenerFacturaPinsa : PaginaBase
	{
		#region Variables Globales
		private Empresa st_InformacionEmpresaSession = new Empresa();
		private CFuncionesGenerales FuncionesGenerales = new CFuncionesGenerales();
		#endregion
		#region Cargar Página
		protected void Page_Load(object obj_Sender, EventArgs e_Parametros)
		{
			InicializarEmpresa();
			if (!IsPostBack)
			{
				InicializarPantalla();
			}
		}
		#endregion
		#region Inicializacion
		private void InicializarEmpresa()
		{
			try
			{
				////////////////////////////////////////////////////////////////////////////////////////////////////
				//Se obtiene la información del usuario que esta entrando en la sesión.
				if (UsuarioSesion != null && UsuarioSesion.Usuario != null)
				{
					////////////////////////////////////////////////////////////////////////////////////////////////////
					//Se obtiene la información de las empresas
					if (Session["LOG_Empresa"] != null)
					{
						st_InformacionEmpresaSession = Session["LOG_Empresa"] as Empresa;
						System.Web.HttpContext.Current.Session["EmpresaID"] = st_InformacionEmpresaSession.Id_empresa;
					}
					else
					{
						List<Empresa> st_ListaEmpresas = new GPO_EMPRESAS().ObtieneListaEmpresasGrupoNegocio();
						st_InformacionEmpresaSession = st_ListaEmpresas.FirstOrDefault();
						System.Web.HttpContext.Current.Session["LOG_Empresa"] = st_InformacionEmpresaSession;
						System.Web.HttpContext.Current.Session["EmpresaID"] = st_InformacionEmpresaSession.Id_empresa;
						MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "No se encontró una empresa seleccionada, se selecciona la empresa " + st_InformacionEmpresaSession.Nombre_comercial + " por el momento.");
					}
				}
			}
			catch (Exception ErrorExcepcion)
			{
				MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Ocurrió un problema al Inicializar la empresa [" + ErrorExcepcion.Message + "]");
			}
		}
		private void InicializarPantalla()
		{
			////////////////////////////////////////////////////////////////////////////////////////////////////
			//Se incializan controles
			FuncionesGenerales.LimpiarControles(this.Controls);
			
		}
		#endregion
		#region Funcionalidad
		private void MostrarMensaje(ControladorMensajes.TipoMensaje enum_TipoMensaje, dynamic dy_Mensaje, int? i_TiempoMostrar = 7000)
		{
			ControladorMensajes.MostrarMensaje(this.UpdatePanel1, string.Format("OBTENER_FACTURAS_PINSA_{0}{1}", enum_TipoMensaje, new Random().Next(0, 99)), enum_TipoMensaje, dy_Mensaje, i_TiempoMostrar);
		}
		private void EjecutarConcurrente()
		{
			try
			{
				string s_Serie = txt_Serie.Text;
				string s_Id_Factura = txt_Factura.Text;
				if(string.IsNullOrEmpty(s_Serie))
				{
					MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "El parámetro Serie es requerido, favor de proporcionar una Serie.");
					return;
				}
				if (!string.IsNullOrEmpty(s_Id_Factura))
				{
					Asn_Ejecutar_Concurrente FuncionesEjecutarConcurrente = new Asn_Ejecutar_Concurrente();
					decimal d_Respuesta_Concurrente = FuncionesEjecutarConcurrente.EjecutarConcurrente_GPIN_EJECUTAR_ASN_DINAM(s_Id_Factura, s_Serie);
					txt_Factura.Text = "";
					MostrarMensaje(ControladorMensajes.TipoMensaje.Exito, "Se ha ejecutado proceso correctamente en Oracle.");
				}
				else
				{
					MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Favor de seleccionar una factura antes de continuar.");
				}
			}
			catch (Exception ErrorExcepcion)
			{
				MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Ocurrió un al ejecutar proceso [" + ErrorExcepcion.Message + "]");
			}
		}
		#endregion
		#region Botones
		protected void btn_EjecutarConcurrente_Click(object obj_Sender, EventArgs e__Parametros)
		{
			EjecutarConcurrente();
		}
		#endregion
	}
}