using System;
using System.Web;
using System.Linq;
using System.Web.UI;
using LogisticaERP.Clases;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using LogisticaERP.GrupoPinsaSOA;
using System.Runtime.Serialization;
using LogisticaERP.LogisticaSOA;
using DevExpress.Web;

namespace LogisticaERP.Catalogos
{
	public partial class PlaneacionReabastecimientoAlmacen : PaginaBase
	{
		#region Variables Globales
		private Empresa st_InformacionEmpresaSession = new Empresa();
		private CFuncionesGenerales FuncionesGenerales = new CFuncionesGenerales();
		private string s_GridPlaneacionReabastecimiento = "st_Info_GridPlaneacionReabastecimiento";
		private string s_InfoComboAlmacen = "st_Info_Combo_Almacen";
		private int i_COL_MES_1 = 7;
		private int i_COL_MES_2 = 8;
		private int i_COL_MES_3 = 9;
		#endregion
		#region Cargar Página
		protected void Page_Load(object obj_Sender, EventArgs e_ParametrosEvento)
		{
			InicializarEmpresa();
			if (!IsPostBack)
			{
				InicializarPantalla();
			}
			RefrescarGrids();
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
			////////////////////////////////////////////////////////////////////////////////////////////////////
			//Se inicializan variables de Sesión
			Session[s_GridPlaneacionReabastecimiento] = null;
			////////////////////////////////////////////////////////////////////////////////////////////////////
			//Se inicializan elementos de la pantalla
			txt_Dias_Inv_Autorizado.Text = "15"; //Se especifican 15 días de inventario por default a petición de Victor Rendón
			dt_Fecha_Inicio.Text = DateTime.Today.ToShortDateString();
			txt_Estatus.Text = "Nuevo";
			co_d_Id_Planeacion_Reabastecimiento.Value = "";
			co_Id_Estatus.Value = "1";
			txt_Anio.Text = DateTime.Today.Year.ToString();
			GridPlaneacionReabastecimiento.Columns[i_COL_MES_1].Caption = "Mes 1";
			GridPlaneacionReabastecimiento.Columns[i_COL_MES_2].Caption = "Mes 2";
			GridPlaneacionReabastecimiento.Columns[i_COL_MES_3].Caption = "Mes 3";
			////////////////////////////////////////////////////////////////////////////////////////////////////
			//Se activan los controles para versión inicial de la pantalla
			ActivarDesactivarBotones();
			////////////////////////////////////////////////////////////////////////////////////////////////////
			//Inicializacion de los Combos
			InicializarComboAlmacenes();
			////////////////////////////////////////////////////////////////////////////////////////////////////
			//Se refresca Grid
			RefrescarGrids();
		}
		private void InicializarComboAlmacenes()
		{
			Logi_Planeacion_Reabastecimiento_Almacen FuncionesPlanes = new Logi_Planeacion_Reabastecimiento_Almacen();
			if(FuncionesPlanes.ConsultarCatalogoLeadTime(null,null))
			{
				FuncionesGenerales.LlenarCombo(ddl_Almacen, FuncionesPlanes.ObtenerCatalogoLeadTime(), "Lista de Almacenes", "d_Id_Catalogo_Lead_Time_Almacen", "s_Codigo_Almacen", "", true);
				Session[s_InfoComboAlmacen] = FuncionesPlanes.ObtenerCatalogoLeadTime();
			}
		}
		#endregion
		#region Funcionalidad
		private void MostrarMensaje(ControladorMensajes.TipoMensaje enum_TipoMensaje, dynamic dy_Mensaje, int? i_TiempoMostrar = 7000)
		{
			//Control generico de mensajes.
			ControladorMensajes.MostrarMensaje(this.UpdatePanel1, string.Format("PLANEACION_REABASTECIMIENTO{0}{1}", enum_TipoMensaje, new Random().Next(0, 99)), enum_TipoMensaje, dy_Mensaje, i_TiempoMostrar);
		}
		private bool ValidarElementosPantalla()
		{
			try
			{
				if(dt_Fecha_Inicio.Text == "")
				{
					MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Favor de especificar una fecha antes de continuar.");
					return false;
				}
				if(lst_Meses.GetSelectedIndices().Count() != 3)
				{
					MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Favor de especificar 3 meses antes de continuar.");
					return false;
				}
				if (txt_Dias_Inv_Autorizado.Text != "" && Convert.ToDecimal(txt_Dias_Inv_Autorizado.Text) < 1)
				{
					MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Favor de especificar los días de inventario autorizado mayor a cero.");
					return false;
				}
				if (txt_Anio.Text != "" && Convert.ToDecimal(txt_Anio.Text) < 2017)
				{
					MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Favor de especificar un año correcto, antes de continuar.");
					return false;
				}
			}
			catch (Exception ErrorExcepcion)
			{
				MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Ocurrió un problema al validar la información de la pantalla [" + ErrorExcepcion.Message + "]");
			}
			return true;
		}
		private void ConsultarInformacion()
		{
			try
			{
				List<CC_Planeacion_Reabastecimiento_Almacen_Detalle> st_InformacionGrid = new List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>();
				if (co_d_Id_Planeacion_Reabastecimiento.Value == "")
				{
					//Para este caso se busca la información que esta en la empresa
					if(ValidarElementosPantalla())
					{
						st_InformacionGrid = ConsultarInformacionEmpresa();
					}
				}
				else
				{
					//Para este caso se busca la información que ya se ha grabado anteriormente
					st_InformacionGrid = ConsultarInformacionGrabada();
				}
				ActivarDesactivarBotones();
				Session[s_GridPlaneacionReabastecimiento] = st_InformacionGrid.Where(Elemento => Elemento.s_Descripcion_Almacen != null ).ToList();
				RefrescarGrids();
			}
			catch (Exception ErrorExcepcion)
			{
				MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Ocurrió un problema al Cargar la información de los tipos de conciliaciones [" + ErrorExcepcion.Message + "]");
			}
		}
		private List<CC_Planeacion_Reabastecimiento_Almacen_Detalle> ConsultarInformacionEmpresa()
		{
			List<CC_Planeacion_Reabastecimiento_Almacen_Detalle> st_InformacionGrid = new List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>();
			try
			{
				List<CPlaneacion_Reabastecimiento_Almacen_Navi> st_InformacionConsulta = new List<CPlaneacion_Reabastecimiento_Almacen_Navi>();
				string s_Fecha_Inicio = FuncionesGenerales.ObtenerFecha(1, Convert.ToDateTime(dt_Fecha_Inicio.Text));
				string s_Fecha_Fin = s_Fecha_Inicio;
				string s_Almacen = Convert.ToDecimal(ddl_Almacen.SelectedItem.Value) == 0 ? "null" : ddl_Almacen.SelectedItem.ToString();
				string s_Mes_Uno = string.Empty;
				string s_Mes_Dos = string.Empty;
				string s_Mes_Tres = string.Empty;
				string s_Dias_Inv_Autorizado = txt_Dias_Inv_Autorizado.Text;
				string s_Producto = txt_Producto.Text == "" ? "null": txt_Producto.Text;
				string s_Anio = txt_Anio.Text == "" ? "0" : txt_Anio.Text;
				string s_Id_Empresa = "1000";
				string s_Usuario = UsuarioSesion.Usuario.Clave_usuario;
				bool b_EsEBS12 = chk_Ebs12.Checked;
				foreach(var st_Elemento in lst_Meses.GetSelectedIndices())
				{
					if (s_Mes_Uno == "")
					{
						s_Mes_Uno = (st_Elemento + 1).ToString();
						GridPlaneacionReabastecimiento.Columns[i_COL_MES_1].Caption = ObtenerNombreMes((st_Elemento + 1));
					}
					else
					{
						if (s_Mes_Dos == "")
						{
							s_Mes_Dos = (st_Elemento + 1).ToString();
							GridPlaneacionReabastecimiento.Columns[i_COL_MES_2].Caption = ObtenerNombreMes((st_Elemento + 1));
						}
						else
						{
							if (s_Mes_Tres == "")
							{
								s_Mes_Tres = (st_Elemento + 1).ToString();
								GridPlaneacionReabastecimiento.Columns[i_COL_MES_3].Caption = ObtenerNombreMes((st_Elemento + 1));
								break;
							}
						}
					}
				}
				Logi_Planeacion_Reabastecimiento_Almacen FuncionesPlanes = new Logi_Planeacion_Reabastecimiento_Almacen();
				if (FuncionesPlanes.ConsultarPlaneacionReabastecimientoAlmacenesNavi(s_Fecha_Inicio, s_Fecha_Fin, s_Almacen, s_Mes_Uno, s_Mes_Dos, s_Mes_Tres, Convert.ToInt32(s_Dias_Inv_Autorizado), s_Producto, s_Anio, s_Id_Empresa, s_Dias_Inv_Autorizado, s_Usuario, b_EsEBS12))
				{
					foreach (CPlaneacion_Reabastecimiento_Almacen_Navi st_Elemento in FuncionesPlanes.ObtenerPlaneacionReabastecimientoAlmacenesNavi())
					{
						int i_Ventas_Promedio = (st_Elemento.d_Mes_1.Value + st_Elemento.d_Mes_2.Value + st_Elemento.d_Mes_3.Value) / 3;
						int d_SaldoFinalCalculo = (int)(st_Elemento.d_Inventario_Actual + st_Elemento.d_Inventario_Transito - st_Elemento.d_Envios);
						int d_VentasPromedioDiarias = (int)(i_Ventas_Promedio / ObtenerDiasDelMes(Convert.ToDateTime(dt_Fecha_Inicio.Text).Year, Convert.ToDateTime(dt_Fecha_Inicio.Text).Month));
						int i_Dias_InventarioCalculo = d_VentasPromedioDiarias > 0 ? (d_SaldoFinalCalculo / d_VentasPromedioDiarias) : 0;
						int i_Dias_InventarioIdealCalculo = (Convert.ToInt32(s_Dias_Inv_Autorizado)) + st_Elemento.i_Lead_Time.Value;
						int d_Sugerido = (i_Dias_InventarioIdealCalculo - i_Dias_InventarioCalculo) > 0 ? (i_Dias_InventarioIdealCalculo - i_Dias_InventarioCalculo) * d_VentasPromedioDiarias : (i_Dias_InventarioIdealCalculo - i_Dias_InventarioCalculo);
						int i_Ventas_Dia = Convert.ToInt32(Math.Ceiling((Convert.ToDecimal(i_Ventas_Promedio) / Convert.ToDecimal(30))));
						int i_Dias_Auto = Convert.ToInt32(txt_Dias_Inv_Autorizado.Text);
						int i_Dias_Cal = i_Dias_InventarioCalculo < 0 ? 0 : i_Dias_InventarioCalculo;
						int i_Enviar = i_Dias_InventarioCalculo >= Convert.ToDecimal(txt_Dias_Inv_Autorizado.Text) ? 0 : (i_Ventas_Dia * (i_Dias_Auto - i_Dias_Cal));
						int i_Por_Surtir = Convert.ToInt32((d_SaldoFinalCalculo - Convert.ToDecimal(st_Elemento.s_Rec_Ships)) >= 0 ? 0 : (d_SaldoFinalCalculo - Convert.ToDecimal(st_Elemento.s_Rec_Ships)));
						int i_Por_Surtir_Aplicar = i_Por_Surtir < 0 ? (i_Por_Surtir * -1) + i_Enviar : i_Por_Surtir;
						//asignación de la información.
						st_InformacionGrid.Add
						(
							new CC_Planeacion_Reabastecimiento_Almacen_Detalle
							{
								d_Id_Planeacion_Reabastecimiento_Detalle = st_Elemento.d_Id_Planeacion_Reabastecimiento_Navi,
								d_Id_Planeacion_Reabastecimiento = 0,
								d_Id_Planeacion_Reabastecimiento_Navi = st_Elemento.d_Id_Planeacion_Reabastecimiento_Navi,
								d_Id_Empresa = st_Elemento.d_Id_Empresa,
								s_Codigo_Almacen = st_Elemento.s_Codigo_Almacen,
								s_Descripcion_Almacen = st_Elemento.s_Descripcion_Almacen,
								s_SKU = st_Elemento.s_Sku,
								s_Descripcion_SKU = st_Elemento.s_Descripcion_Sku,
								d_Inventario_Actual = st_Elemento.d_Inventario_Actual.Value,
								d_Inventario_Transito = st_Elemento.d_Inventario_Transito.Value,
								d_Ventas_Promedio = i_Ventas_Promedio,
								d_Ventas_Mes = st_Elemento.d_Ventas_Mes.Value,
								d_Envios = st_Elemento.d_Envios.Value,
								d_Saldo_Final = d_SaldoFinalCalculo - Convert.ToDecimal(st_Elemento.s_Rec_Ships),
								i_Dias_Inventario = i_Dias_InventarioCalculo,
								i_Dias_Inventario_Ideal = i_Dias_InventarioIdealCalculo,
								i_Lead_Time = st_Elemento.i_Lead_Time.Value,
								d_Sugerido = d_Sugerido,
								d_Real = st_Elemento.d_Real.Value,
								d_Mes_1 = st_Elemento.d_Mes_1.Value,
								d_Mes_2 = st_Elemento.d_Mes_2.Value,
								d_Mes_3 = st_Elemento.d_Mes_3.Value,
								d_Ventas_Diarias_Promedio = d_VentasPromedioDiarias,
								s_Unidad_Medida = "Cajas",
								s_Mensaje_Mostrar = st_Elemento.s_Mensaje_Mostrar,
								s_Rec_Ships = Convert.ToDecimal(st_Elemento.s_Rec_Ships).ToString("n0"),
								s_Inventario_Pinsa = Convert.ToDecimal(st_Elemento.s_Inventario_Pinsa).ToString("n0"),
								//s_Por_Surtir = st_Elemento.s_Por_Surtir,
								//s_Por_Surtir = (d_SaldoFinalCalculo - Convert.ToDecimal(st_Elemento.s_Rec_Ships)) >= 0 ? "0" : (d_SaldoFinalCalculo - Convert.ToDecimal(st_Elemento.s_Rec_Ships)).ToString("N0"),
								s_Por_Surtir = i_Por_Surtir_Aplicar.ToString("n0"),
								//s_A_Enviar = st_Elemento.s_A_Enviar,
								//s_A_Enviar = i_Dias_InventarioCalculo >= Convert.ToDecimal(txt_Dias_Inv_Autorizado.Text) ? "0" : ((i_Ventas_Promedio / 30) * (Convert.ToDecimal(txt_Dias_Inv_Autorizado.Text) - i_Dias_InventarioCalculo)).ToString("N0"),
								//s_A_Enviar = i_Dias_InventarioCalculo >= Convert.ToDecimal(txt_Dias_Inv_Autorizado.Text) ? "0" : (i_Ventas_Dia * (i_Dias_Auto - i_Dias_Cal)).ToString("N0"),
								s_A_Enviar = i_Enviar.ToString("n0"),
							}
						);
					}
				}
			}
			catch (Exception ErrorExcepcion)
			{
				MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Ocurrió un problema al Cargar la información de Reabastecimiento [" + ErrorExcepcion.Message + "]");
			}
			return st_InformacionGrid;
		}
		private List<CC_Planeacion_Reabastecimiento_Almacen_Detalle> ConsultarInformacionGrabada()
		{
			List<CC_Planeacion_Reabastecimiento_Almacen_Detalle> st_InformacionGrid = new List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>();
			try
			{
				C_Planeacion_Reabastecimiento_Almacen st_InformacionPlan = new C_Planeacion_Reabastecimiento_Almacen();
				Logi_Planeacion_Reabastecimiento_Almacen FuncionesPlanes = new Logi_Planeacion_Reabastecimiento_Almacen();
				decimal d_Id_Plneacion_Reabastecimientos = Convert.ToDecimal(co_d_Id_Planeacion_Reabastecimiento.Value);
				if (FuncionesPlanes.ConsultarPlaneacionReabastecimientoAlmacenes(d_Id_Plneacion_Reabastecimientos, (decimal?)null))
				{
					List<C_Catalogo_Lead_Time> st_Info_Combo_Almacen = Session[s_InfoComboAlmacen] == null ? new List<C_Catalogo_Lead_Time>() : Session[s_InfoComboAlmacen] as List<C_Catalogo_Lead_Time>;
					//Se agrega la información al encabezado
					st_InformacionPlan = FuncionesPlanes.ObtenerPlaneacionReabastecimientoAlmacenes().FirstOrDefault();
					co_d_Id_Planeacion_Reabastecimiento.Value = st_InformacionPlan.d_Id_Planeacion_Reabastecimiento.ToString();
					co_Id_Estatus.Value = st_InformacionPlan.d_Id_Estatus_Planeacion.ToString();
					txt_Folio_Planeacion.Text = st_InformacionPlan.d_Folio_Planeacion.ToString();
					if (st_Info_Combo_Almacen.Where(Elemento => Elemento.s_Codigo_Almacen == st_InformacionPlan.s_Almacen).FirstOrDefault() != null)
					{
						ddl_Almacen.SelectedValue = st_Info_Combo_Almacen.Where(Elemento => Elemento.s_Codigo_Almacen == st_InformacionPlan.s_Almacen).FirstOrDefault().d_Id_Catalogo_Lead_Time_Almacen.ToString();
					}
					dt_Fecha_Inicio.Text = st_InformacionPlan.dt_Fecha_Planeacion.ToShortDateString();
					txt_Estatus.Text = st_InformacionPlan.s_Descripcion_Estatus;
					txt_Dias_Inv_Autorizado.Text = st_InformacionPlan.d_Dias_Inventario_Autorizado.ToString();
					txt_Anio.Text = st_InformacionPlan.d_Anio.ToString();
					txt_Producto.Text = st_InformacionPlan.s_Producto;
					//Se seleccionan los meses que se habían grabado
					lst_Meses.Items.FindByValue((st_InformacionPlan.d_Mes_1).ToString()).Selected = true;
					lst_Meses.Items.FindByValue((st_InformacionPlan.d_Mes_2).ToString()).Selected = true;
					lst_Meses.Items.FindByValue((st_InformacionPlan.d_Mes_3).ToString()).Selected = true;
					//Nombre de las columnas del Grid
					GridPlaneacionReabastecimiento.Columns[i_COL_MES_1].Caption = ObtenerNombreMes(st_InformacionPlan.d_Mes_1);
					GridPlaneacionReabastecimiento.Columns[i_COL_MES_2].Caption = ObtenerNombreMes(st_InformacionPlan.d_Mes_2);
					GridPlaneacionReabastecimiento.Columns[i_COL_MES_3].Caption = ObtenerNombreMes(st_InformacionPlan.d_Mes_3);
					//Se agrega la información al Grid
					foreach (C_Planeacion_Reabastecimiento_Almacen_Detalle st_Elemento in st_InformacionPlan.st_Planeacion_Reabastecimiento_Almacen_Detalle.ToList())
					{
						st_InformacionGrid.Add
						(
							new CC_Planeacion_Reabastecimiento_Almacen_Detalle
							{
								d_Id_Planeacion_Reabastecimiento_Detalle = (st_Elemento.d_Id_Planeacion_Reabastecimiento_Detalle == 0 ? st_Elemento.d_Id_Planeacion_Reabastecimiento_Navi : st_Elemento.d_Id_Planeacion_Reabastecimiento_Detalle),
								d_Id_Planeacion_Reabastecimiento = st_Elemento.d_Id_Planeacion_Reabastecimiento,
								d_Id_Planeacion_Reabastecimiento_Navi = st_Elemento.d_Id_Planeacion_Reabastecimiento_Navi,
								d_Id_Empresa = st_Elemento.d_Id_Empresa,
								s_Codigo_Almacen = st_Elemento.s_Codigo_Almacen,
								s_Descripcion_Almacen = st_Elemento.s_Descripcion_Almacen,
								s_SKU = st_Elemento.s_SKU,
								s_Descripcion_SKU = st_Elemento.s_Descripcion_SKU,
								d_Inventario_Actual = st_Elemento.d_Inventario_Actual,
								d_Inventario_Transito = st_Elemento.d_Inventario_Transito,
								d_Ventas_Promedio = st_Elemento.d_Ventas_Promedio,
								d_Ventas_Mes = st_Elemento.d_Ventas_Mes,
								d_Envios = st_Elemento.d_Envios,
								d_Saldo_Final = st_Elemento.d_Saldo_Final,
								i_Dias_Inventario = st_Elemento.i_Dias_Inventario,
								i_Dias_Inventario_Ideal = (st_InformacionPlan.d_Dias_Inventario_Autorizado + st_Elemento.i_Lead_Time),
								i_Lead_Time = st_Elemento.i_Lead_Time,
								d_Sugerido = st_Elemento.d_Sugerido,
								d_Real = st_Elemento.d_Real,
								d_Mes_1 = st_Elemento.d_Mes_1,
								d_Mes_2 = st_Elemento.d_Mes_2,
								d_Mes_3 = st_Elemento.d_Mes_3,
								d_Ventas_Diarias_Promedio = st_Elemento.d_Ventas_Diarias_Promedio,
								s_Unidad_Medida = "Cajas",
								s_Mensaje_Mostrar = st_Elemento.s_Mensaje_Mostrar,
								s_Rec_Ships = Convert.ToDecimal(st_Elemento.s_Rec_Ships).ToString("n0"),
								s_Inventario_Pinsa = Convert.ToDecimal(st_Elemento.s_Inventario_Pinsa).ToString("n0"),
								s_Por_Surtir = Convert.ToDecimal(st_Elemento.s_Por_Surtir).ToString("n0"),
								s_A_Enviar = Convert.ToDecimal(st_Elemento.s_A_Enviar).ToString("n0"),
							}
						);
					}
				}
				else
				{
					MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Ocurrió un problema al Cargar la información de Reabastecimiento, no se encontró información correcta para el Folio [" + txt_Folio_Planeacion.Text + "]");
				}
			}
			catch (Exception ErrorExcepcion)
			{
				MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Ocurrió un problema al Cargar la información de Reabastecimiento [" + ErrorExcepcion.Message + "]");
			}
			return st_InformacionGrid;
		}
		private void ActivarDesactivarBotones()
		{
			switch(co_Id_Estatus.Value)
			{
				case "1": //NUEVO
					{
						ddl_Almacen.Enabled = true;
						dt_Fecha_Inicio.Enabled = true;
						lst_Meses.Enabled = true;
						txt_Dias_Inv_Autorizado.Enabled = true;
						txt_Anio.Enabled = true;
						txt_Producto.Enabled = true;
						btn_Cargar_Grid.Enabled = true;
						btn_Grabar_Informacion.Enabled = true;
						btn_Registrar_Informacion.Enabled = false;
						btn_Exportar_Excel.Enabled = false;
					}
					break;
				case "2": //GRABADO
					{
						ddl_Almacen.Enabled = false;
						dt_Fecha_Inicio.Enabled = false;
						lst_Meses.Enabled = false;
						txt_Dias_Inv_Autorizado.Enabled = false;
						txt_Anio.Enabled = false;
						txt_Producto.Enabled = false;
						btn_Cargar_Grid.Enabled = false;
						btn_Grabar_Informacion.Enabled = true;
						btn_Registrar_Informacion.Enabled = true;
						btn_Exportar_Excel.Enabled = false;
					}
					break;
				case "3": //REGISTRADO
					{
						ddl_Almacen.Enabled = false;
						dt_Fecha_Inicio.Enabled = false;
						lst_Meses.Enabled = false;
						txt_Dias_Inv_Autorizado.Enabled = false;
						txt_Anio.Enabled = false;
						txt_Producto.Enabled = false;
						btn_Cargar_Grid.Enabled = false;
						btn_Grabar_Informacion.Enabled = false;
						btn_Registrar_Informacion.Enabled = false;
						btn_Exportar_Excel.Enabled = true;
					}
					break;
				default:
					break;
			}
		}
		private bool GrabarInformacion()
		{
			bool b_Respuesta = false;
			try
			{
				//Se declaran variables
				CRespuestaGeneral st_Respuesta = new CRespuestaGeneral();
				List<CC_Planeacion_Reabastecimiento_Almacen_Detalle> st_InformacionGrid = Session[s_GridPlaneacionReabastecimiento] == null ? new List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>() : Session[s_GridPlaneacionReabastecimiento] as List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>;
				List<C_Planeacion_Reabastecimiento_Almacen_Detalle> st_DetalleGrabar = new List<C_Planeacion_Reabastecimiento_Almacen_Detalle>();
				if (st_InformacionGrid.Count > 0)
				{
					//Información que se va a grabar
					C_Planeacion_Reabastecimiento_Almacen st_Informacion_Grabar = new C_Planeacion_Reabastecimiento_Almacen();
					st_Informacion_Grabar.d_Id_Planeacion_Reabastecimiento = Convert.ToDecimal((co_d_Id_Planeacion_Reabastecimiento.Value == "" ? "0" : co_d_Id_Planeacion_Reabastecimiento.Value));
					st_Informacion_Grabar.d_Id_Estatus_Planeacion = Convert.ToDecimal(co_Id_Estatus.Value);
					st_Informacion_Grabar.d_Id_Empresa = 1000;
					st_Informacion_Grabar.s_Descripcion_Estatus = txt_Estatus.Text;
					st_Informacion_Grabar.s_Descripcion_Planeacion = "";
					st_Informacion_Grabar.d_Folio_Planeacion = Convert.ToDecimal((txt_Folio_Planeacion.Text == "" ? "0" : txt_Folio_Planeacion.Text));
					st_Informacion_Grabar.s_Almacen = ddl_Almacen.SelectedItem.Value == "0" ? "" : ddl_Almacen.SelectedItem.ToString();
					st_Informacion_Grabar.dt_Fecha_Planeacion = Convert.ToDateTime(dt_Fecha_Inicio.Text);
					st_Informacion_Grabar.d_Dias_Inventario_Autorizado = Convert.ToInt32(txt_Dias_Inv_Autorizado.Text);
					st_Informacion_Grabar.d_Anio = Convert.ToInt32(txt_Anio.Text);
					st_Informacion_Grabar.s_Producto = txt_Producto.Text;
					foreach (var st_Elemento in lst_Meses.GetSelectedIndices())
					{
						if (st_Informacion_Grabar.d_Mes_1 == 0)
						{
							st_Informacion_Grabar.d_Mes_1 = st_Elemento + 1;
						}
						else
						{
							if (st_Informacion_Grabar.d_Mes_2 == 0)
							{
								st_Informacion_Grabar.d_Mes_2 = st_Elemento + 1;
							}
							else
							{
								if (st_Informacion_Grabar.d_Mes_3 == 0)
								{
									st_Informacion_Grabar.d_Mes_3 = st_Elemento + 1;
									break;
								}
							}
						}
					}
					foreach(CC_Planeacion_Reabastecimiento_Almacen_Detalle st_Elemento in st_InformacionGrid)
					{
						st_DetalleGrabar.Add
						(
							new C_Planeacion_Reabastecimiento_Almacen_Detalle
							{
								d_Id_Planeacion_Reabastecimiento_Detalle = (st_Elemento.d_Id_Planeacion_Reabastecimiento_Detalle == 0 ? st_Elemento.d_Id_Planeacion_Reabastecimiento_Navi : st_Elemento.d_Id_Planeacion_Reabastecimiento_Detalle),
								d_Id_Planeacion_Reabastecimiento = st_Elemento.d_Id_Planeacion_Reabastecimiento,
								d_Id_Planeacion_Reabastecimiento_Navi = st_Elemento.d_Id_Planeacion_Reabastecimiento_Navi,
								d_Id_Empresa = st_Elemento.d_Id_Empresa,
								s_Codigo_Almacen = st_Elemento.s_Codigo_Almacen,
								s_Descripcion_Almacen = st_Elemento.s_Descripcion_Almacen,
								s_SKU = st_Elemento.s_SKU,
								s_Descripcion_SKU = st_Elemento.s_Descripcion_SKU,
								d_Inventario_Actual = st_Elemento.d_Inventario_Actual,
								d_Inventario_Transito = st_Elemento.d_Inventario_Transito,
								d_Ventas_Promedio = st_Elemento.d_Ventas_Promedio,
								d_Ventas_Mes = st_Elemento.d_Ventas_Mes,
								d_Envios = st_Elemento.d_Envios,
								d_Saldo_Final = st_Elemento.d_Saldo_Final,
								i_Dias_Inventario = st_Elemento.i_Dias_Inventario,
								i_Lead_Time = st_Elemento.i_Lead_Time,
								d_Sugerido = st_Elemento.d_Sugerido,
								d_Real = st_Elemento.d_Real,
								d_Mes_1 = st_Elemento.d_Mes_1,
								d_Mes_2 = st_Elemento.d_Mes_2,
								d_Mes_3 = st_Elemento.d_Mes_3,
								d_Ventas_Diarias_Promedio = st_Elemento.d_Ventas_Diarias_Promedio,
								s_Unidad_Medida = "Cajas",
								s_Mensaje_Mostrar = st_Elemento.s_Mensaje_Mostrar,
								s_Rec_Ships = st_Elemento.s_Rec_Ships,
								s_Inventario_Pinsa = st_Elemento.s_Inventario_Pinsa,
								s_Por_Surtir = st_Elemento.s_Por_Surtir,
								s_A_Enviar = st_Elemento.s_A_Enviar,
							}
						);
					}
					st_Informacion_Grabar.st_Planeacion_Reabastecimiento_Almacen_Detalle = st_DetalleGrabar.ToArray();
					//Se crea la instancia para grabar la planeación
					Logi_Planeacion_Reabastecimiento_Almacen FuncionesPlanes = new Logi_Planeacion_Reabastecimiento_Almacen();
					st_Respuesta = FuncionesPlanes.GrabarPlaneacionReabastecimientoAlmacen(st_Informacion_Grabar);
					if (st_Respuesta.i_Respuesta > 0)
					{
						b_Respuesta = true;
						MostrarMensaje(ControladorMensajes.TipoMensaje.Exito, "Se ha grabado correctamente la información.");
						InicializarPantalla();
					}
					else
					{
						MostrarMensaje(ControladorMensajes.TipoMensaje.Error, "Ocurrió un problema al Grabar la información de la planeación [" + st_Respuesta.s_Mensaje + "]");
					}
				}
				else
				{
					MostrarMensaje(ControladorMensajes.TipoMensaje.Error, "Favor de consultar información antes de grabar.");
				}
			}
			catch (Exception ErrorExcepcion)
			{
				MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Ocurrió un problema al Grabar la información de los tipos de conciliaciones [" + ErrorExcepcion.Message + "]");
			}
			return b_Respuesta;
		}
		private void ExportarInformacion()
		{
			try
			{
				List<CC_Planeacion_Reabastecimiento_Almacen_Detalle> st_InformacionGrid = Session[s_GridPlaneacionReabastecimiento] == null ? new List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>() : Session[s_GridPlaneacionReabastecimiento] as List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>;
				if (st_InformacionGrid != null && st_InformacionGrid.Count > 0)
				{
					string s_NombreArchivo = "Plan_Reabastecimiento_" + txt_Folio_Planeacion.Text + "_" + DateTime.Now.Date.ToString();
					GridPlaneacionReabastecimiento_Exportar.WriteXlsxToResponse(s_NombreArchivo, true);
				}
			}
			catch (Exception ErrorExecion)
			{
				ControladorMensajes.MostrarMensaje(UpdatePanel1, "ExportarGridExcel", ControladorMensajes.TipoMensaje.Advertencia, ErrorExecion.Message, 7000);
			}
		}
		private string ObtenerNombreMes(int i_Mes)
		{
			switch(i_Mes)
			{
				case 1: { return "ENE"; }
				case 2: { return "FEB"; }
				case 3: { return "MAR"; }
				case 4: { return "ABR"; }
				case 5: { return "MAY"; }
				case 6: { return "JUN"; }
				case 7: { return "JUL"; }
				case 8: { return "AGO"; }
				case 9: { return "SEP"; }
				case 10: { return "OCT"; }
				case 11: { return "NOV"; }
				case 12: { return "DIC"; }
				default: { return "S/N"; }
			}
		}
		private void RegistrarInformacion()
		{
			try
			{
				decimal d_Id_Planeacion_Reabastecimiento = Convert.ToDecimal((co_d_Id_Planeacion_Reabastecimiento.Value == "" ? "0" : co_d_Id_Planeacion_Reabastecimiento.Value));
				if( GrabarInformacion() )
				{
					CRespuestaGeneral st_Respuesta = new CRespuestaGeneral();
					Logi_Planeacion_Reabastecimiento_Almacen FuncionesPlanes = new Logi_Planeacion_Reabastecimiento_Almacen();
					st_Respuesta = FuncionesPlanes.ActualizarEstatusPlanReabastecimiento(d_Id_Planeacion_Reabastecimiento, (decimal)3);
					if (st_Respuesta.i_Respuesta > 0)
					{
						MostrarMensaje(ControladorMensajes.TipoMensaje.Exito, "Se ha registrado correctamente la información.");
						InicializarPantalla();
					}
					else
					{
						MostrarMensaje(ControladorMensajes.TipoMensaje.Error, "Ocurrió un problema al registrar la información de la planeación [" + st_Respuesta.s_Mensaje + "]");
					}
				}
			}
			catch (Exception ErrorExcepcion)
			{
				MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, "Ocurrió un problema al registrar la información de los tipos de conciliaciones [" + ErrorExcepcion.Message + "]");
			}
		}
		private int ObtenerDiasDelMes(int i_Anio, int i_Mes)
		{
			int i_Dia = 0;
			switch (i_Mes)
			{
				case 0:
					{
						i_Dia = 31;
					}
					break;
				case 2:
					{
						if (((i_Anio % 4) == 0) && (((i_Anio % 100) != 0) || ((i_Anio % 400) == 0)))
						{
							i_Dia = 29;
						}
						else
						{
							i_Dia = 28;
						}
					}
					break;
				case 1:
				case 3:
				case 5:
				case 7:
				case 8:
				case 10:
				case 12:
					{
						i_Dia = 31;
					}
					break;
				case 4:
				case 6:
				case 9:
				case 11:
					{
						i_Dia = 30;
					}
					break;
			}
			return i_Dia;
		}
		#endregion
		#region Funcionalidad Grid
		private void RefrescarGrids()
		{
			List<CC_Planeacion_Reabastecimiento_Almacen_Detalle> st_InformacionGrid = Session[s_GridPlaneacionReabastecimiento] == null ? new List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>() : Session[s_GridPlaneacionReabastecimiento] as List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>;
			GridPlaneacionReabastecimiento.DataSource = st_InformacionGrid;
			GridPlaneacionReabastecimiento.KeyFieldName = "d_Id_Planeacion_Reabastecimiento_Detalle";
			GridPlaneacionReabastecimiento.DataBind();
		}
		protected void GridPlaneacionReabastecimiento_CellEditorInitialize(object obj_Sender, ASPxGridViewEditorEventArgs e_ParametrosEvento)
		{
			if (e_ParametrosEvento.Column.FieldName == "d_Real")
			{
				if (Convert.ToDecimal(co_Id_Estatus.Value) > 2)
				{
					e_ParametrosEvento.Editor.ReadOnly = true;
				}
				else
				{
					e_ParametrosEvento.Editor.ReadOnly = false;
				}
			}
			else
			{
				e_ParametrosEvento.Editor.ReadOnly = true;
			}
		}
		protected void GridPlaneacionReabastecimiento_HtmlDataCellPrepared(object obj_Sender, ASPxGridViewTableDataCellEventArgs e_ParametrosEvento)
		{
			e_ParametrosEvento.Cell.Attributes.Add("onclick", String.Format("onCellClick({0}, '{1}', GridPlaneacionReabastecimiento)", e_ParametrosEvento.VisibleIndex, e_ParametrosEvento.DataColumn.FieldName));
		}
		protected void GridPlaneacionReabastecimiento_RowUpdating(object obj_Sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e_ParametrosEvento)
		{
			try
			{
				List<CC_Planeacion_Reabastecimiento_Almacen_Detalle> st_InformacionGrid =
					Session[s_GridPlaneacionReabastecimiento] == null ?
						new List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>() :
						Session[s_GridPlaneacionReabastecimiento] as List<CC_Planeacion_Reabastecimiento_Almacen_Detalle>;
				if (Convert.ToDecimal(co_Id_Estatus.Value) < 3)
				{
					decimal d_Id_Planeacion_Reabastecimiento_Detalle = e_ParametrosEvento.Keys["d_Id_Planeacion_Reabastecimiento_Detalle"] == null ? 0 : Convert.ToDecimal(e_ParametrosEvento.Keys["d_Id_Planeacion_Reabastecimiento_Detalle"]);
					CC_Planeacion_Reabastecimiento_Almacen_Detalle st_ElementoModificar = st_InformacionGrid.Where(Elemento => Elemento.d_Id_Planeacion_Reabastecimiento_Detalle == d_Id_Planeacion_Reabastecimiento_Detalle).FirstOrDefault();
					st_ElementoModificar.d_Real = Convert.ToDecimal(e_ParametrosEvento.NewValues["d_Real"]);
					Session[s_GridPlaneacionReabastecimiento] = st_InformacionGrid;
				}
				RefrescarGrids();
				e_ParametrosEvento.Cancel = true;
				GridPlaneacionReabastecimiento.CancelEdit();
			}
			catch (Exception ErrorExcepcion)
			{
				MostrarMensaje(ControladorMensajes.TipoMensaje.Advertencia, ErrorExcepcion.Message);
			}
		}
		#endregion
		#region Botones
		protected void btn_Cargar_Grid_Click(object obj_Sender, EventArgs e_ParametrosEvento)
		{
			ConsultarInformacion();
		}
		protected void btn_Exportar_Excel_Click(object obj_Sender, EventArgs e_ParametrosEvento)
		{
			ExportarInformacion();
		}
		protected void btn_Limpiar_Click(object obj_Sender, EventArgs e_ParametrosEvento)
		{
			InicializarPantalla();
		}
		protected void btn_Grabar_Informacion_Click(object obj_Sender, EventArgs e_ParametrosEvento)
		{
			GrabarInformacion();
		}
		protected void btn_Registrar_Informacion_Click(object obj_Sender, EventArgs e_ParametrosEvento)
		{
			RegistrarInformacion();
		}
		#endregion
		#region Eventos
		#endregion
		#region Web Métodos
		#endregion
		#region Clases
		[Serializable]
		public class CCombo_Elementos
		{
			[DataMember]
			public int d_Id_Elemento { get; set; }
			[DataMember]
			public string s_Descripcion { get; set; }
		}
		[Serializable]
		public class CC_Planeacion_Reabastecimiento_Almacen_Detalle
		{
			[DataMember]
			public decimal d_Id_Planeacion_Reabastecimiento_Detalle { get; set; }
			[DataMember]
			public decimal d_Id_Planeacion_Reabastecimiento_Navi { get; set; }
			[DataMember]
			public decimal d_Id_Planeacion_Reabastecimiento { get; set; }
			[DataMember]
			public decimal d_Id_Empresa { get; set; }
			[DataMember]
			public string s_Codigo_Almacen { get; set; }
			[DataMember]
			public string s_Descripcion_Almacen { get; set; }
			[DataMember]
			public string s_SKU { get; set; }
			[DataMember]
			public string s_Descripcion_SKU { get; set; }
			[DataMember]
			public decimal d_Inventario_Actual { get; set; }
			[DataMember]
			public decimal d_Inventario_Transito { get; set; }
			[DataMember]
			public decimal d_Ventas_Promedio { get; set; }
			[DataMember]
			public decimal d_Ventas_Mes { get; set; }
			[DataMember]
			public decimal d_Envios { get; set; }
			[DataMember]
			public decimal d_Saldo_Final { get; set; }
			[DataMember]
			public int i_Dias_Inventario { get; set; }
			[DataMember]
			public int i_Lead_Time { get; set; }
			[DataMember]
			public int i_Dias_Inventario_Ideal { get; set; }
			[DataMember]
			public decimal d_Sugerido { get; set; }
			[DataMember]
			public decimal d_Real { get; set; }
			[DataMember]
			public int d_Mes_1 { get; set; }
			[DataMember]
			public int d_Mes_2 { get; set; }
			[DataMember]
			public int d_Mes_3 { get; set; }
			[DataMember]
			public decimal d_Ventas_Diarias_Promedio { get; set; }
			[DataMember]
			public string s_Unidad_Medida { get; set; }
			[DataMember]
			public string s_Mensaje_Mostrar { get; set; }
			[DataMember]
			public string s_Rec_Ships { get; set; }
			[DataMember]
			public string s_Inventario_Pinsa { get; set; }
			[DataMember]
			public string s_Por_Surtir { get; set; }
			[DataMember]
			public string s_A_Enviar { get; set; }
			//Constructor
			public CC_Planeacion_Reabastecimiento_Almacen_Detalle()
			{
				this.d_Id_Planeacion_Reabastecimiento_Detalle = 0;
				this.d_Id_Planeacion_Reabastecimiento_Navi = 0;
				this.d_Id_Planeacion_Reabastecimiento = 0;
				this.d_Id_Empresa = 0;
				this.s_Codigo_Almacen = string.Empty;
				this.s_Descripcion_Almacen = string.Empty;
				this.s_SKU = string.Empty;
				this.s_Descripcion_SKU = string.Empty;
				this.d_Inventario_Actual = 0;
				this.d_Inventario_Transito = 0;
				this.d_Ventas_Promedio = 0;
				this.d_Ventas_Mes = 0;
				this.d_Envios = 0;
				this.d_Saldo_Final = 0;
				this.i_Dias_Inventario = 0;
				this.i_Lead_Time = 0;
				this.i_Dias_Inventario_Ideal = 0;
				this.d_Sugerido = 0;
				this.d_Real = 0;
				this.d_Mes_1 = 0;
				this.d_Mes_2 = 0;
				this.d_Mes_3 = 0;
				this.d_Ventas_Diarias_Promedio = 0;
				this.s_Unidad_Medida = string.Empty;
				this.s_Mensaje_Mostrar = string.Empty;
				this.s_Rec_Ships = string.Empty;
				this.s_Inventario_Pinsa = string.Empty;
				this.s_Por_Surtir = string.Empty;
				this.s_A_Enviar = string.Empty;
			}
		}
		#endregion
	}
}