using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.ServiceModel;
using LogisticaERP.Clases;
using System.Web.UI.WebControls;
using LogisticaERP.LogisticaSOA;
using System.Collections.Generic;
using LogisticaERP.GrupoPinsaSOA;
using System.Runtime.Serialization;
using LogisticaERP.SeguridadERPSOA;

namespace LogisticaERP.Catalogos.BusquedaAvanzadaPersonalizada
{
	public partial class BusquedaAvanzadaFolioPlaneacionReabastecimiento : PaginaBase
	{
		#region Variables Globales
		private string s_Grid_Folios = "st_Info_Grid_Folios";
		private Empresa st_InformacionEmpresaSession = new Empresa();
		#endregion
		#region Cargar Página
		protected void Page_Load(object obj_Sender, EventArgs e_Parametros)
		{
			InicializarEmpresa();
			if (!IsPostBack)
			{
				InicializarPantalla();
			}
			RefrescarGrid();
		}
		#endregion
		#region Inicializacion
		private void InicializarEmpresa()
		{
			if (Session["OPF_Empresa"] != null)
			{
				st_InformacionEmpresaSession = Session["OPF_Empresa"] as Empresa;
			}
			else
			{
				st_InformacionEmpresaSession = new Empresa();
				if (System.Web.HttpContext.Current.Session["EmpresaID"] != null)
				{
					st_InformacionEmpresaSession.Id_empresa = Convert.ToDecimal(System.Web.HttpContext.Current.Session["EmpresaID"]);
				}
				else
				{
					//st_InformacionEmpresaSession = SessionHelper.Empresa;
				}
			}
		}
		private void InicializarPantalla()
		{
			////////////////////////////////////////////////////////////////////////////////////////////////////
			//Se inicializan variables de Sesión
			Session[s_Grid_Folios] = null;
			////////////////////////////////////////////////////////////////////////////////////////////////////
			//Se inicializa Grid
			InicializarGrid();
		}
		#endregion
		#region Funcionalidad
		private void MostrarMensaje(ControladorMensajes.TipoMensaje enum_TipoMensaje, dynamic dy_Mensaje, int? i_TiempoMostrar = 7000)
		{
			ControladorMensajes.MostrarMensaje(this.UpdatePanel1, string.Format("CONCILIACIONES_DE_EGRESOS{0}{1}", enum_TipoMensaje, new Random().Next(0, 99)), enum_TipoMensaje, dy_Mensaje, i_TiempoMostrar);
		}
		private void InicializarGrid()
		{
			try
			{
				List<C_Planeacion_Reabastecimiento_Almacen> st_PlaneacionesCreadas = new List<C_Planeacion_Reabastecimiento_Almacen>();
				Logi_Planeacion_Reabastecimiento_Almacen FuncionesPlaneaciones = new Logi_Planeacion_Reabastecimiento_Almacen();
				if (FuncionesPlaneaciones.ConsultarPlaneacionReabastecimientoAlmacenesLigero((decimal?)null, (decimal?)null))
				{
					st_PlaneacionesCreadas = FuncionesPlaneaciones.ObtenerPlaneacionReabastecimientoAlmacenes();
					if (st_PlaneacionesCreadas.Count < 1)
					{
						MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Actualmente no se encuentra ninguna planeación creada.");
					}
				}
				Session[s_Grid_Folios] = st_PlaneacionesCreadas;
			}
			catch (Exception ErrorExcepcion)
			{
				MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Ocurrió un problema al consultar los Folios, favor de comunicarse a TI Aplicaciones.[" + ErrorExcepcion.Message + "]");
			}
			RefrescarGrid();
		}
		private void RefrescarGrid()
		{
			List<C_Planeacion_Reabastecimiento_Almacen> st_PlaneacionesCreadas = Session[s_Grid_Folios] == null ? new List<C_Planeacion_Reabastecimiento_Almacen>() : Session[s_Grid_Folios] as List<C_Planeacion_Reabastecimiento_Almacen>;
			GridFolios.DataSource = st_PlaneacionesCreadas;
			GridFolios.KeyFieldName = "d_Folio_Planeacion";
			GridFolios.DataBind();
		}
		#endregion
	}
}