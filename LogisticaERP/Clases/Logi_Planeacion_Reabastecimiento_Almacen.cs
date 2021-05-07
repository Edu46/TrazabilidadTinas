using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Threading;
using System.ServiceModel;
using LogisticaERP.LogisticaSOA;
using System.Collections.Generic;


namespace LogisticaERP.Clases
{
	public class Logi_Planeacion_Reabastecimiento_Almacen : ClaseBase
	{
		#region Variables
		List<C_Planeacion_Reabastecimiento_Almacen> st_Planeacion_Reabastecimiento_Almacen;
		List<CPlaneacion_Reabastecimiento_Almacen_Navi> st_Planeacion_Reabastecimiento_Almacen_Navi;
		List<C_Catalogo_Lead_Time> st_Catalogo_Lead_Time;
		#endregion
		#region Constructor
		public Logi_Planeacion_Reabastecimiento_Almacen()
		{
			this.st_Planeacion_Reabastecimiento_Almacen = new List<C_Planeacion_Reabastecimiento_Almacen>();
			this.st_Planeacion_Reabastecimiento_Almacen_Navi = new List<CPlaneacion_Reabastecimiento_Almacen_Navi>();
			this.st_Catalogo_Lead_Time = new List<C_Catalogo_Lead_Time>();
		}
		#endregion
		#region No Sirven para Nada
		public override bool Grabar()
		{
			throw new NotImplementedException();
		}
		public override bool Busqueda()
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Obtener Informacion
		public List<C_Planeacion_Reabastecimiento_Almacen> ObtenerPlaneacionReabastecimientoAlmacenes()
		{
			return this.st_Planeacion_Reabastecimiento_Almacen;
		}
		public List<CPlaneacion_Reabastecimiento_Almacen_Navi> ObtenerPlaneacionReabastecimientoAlmacenesNavi()
		{
			return this.st_Planeacion_Reabastecimiento_Almacen_Navi;
		}
		public List<C_Catalogo_Lead_Time> ObtenerCatalogoLeadTime()
		{
			return this.st_Catalogo_Lead_Time;
		}
		#endregion
		#region Consultas
		public bool ConsultarPlaneacionReabastecimientoAlmacenesLigero(decimal? d_Id_Plneacion_Reabastecimientos, decimal? d_Folio)
		{
			try
			{
				using (LogisticaWCFAPPServiciosClient ws_Cliente = new LogisticaWCFAPPServiciosClient())
				{
					ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
					ws_Cliente.InnerChannel.OperationTimeout = new TimeSpan(0, 2, 0);
					this.st_Planeacion_Reabastecimiento_Almacen = ws_Cliente.ConsultarMovimientosPlaneacionLigero(d_Id_Plneacion_Reabastecimientos, d_Folio).ToList();
					if (this.st_Planeacion_Reabastecimiento_Almacen != null && this.st_Planeacion_Reabastecimiento_Almacen.Count > 0)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> faultException)
			{
				throw new Exception(faultException.Detail.ExcDetalle.Mensaje, faultException);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
		public bool ConsultarPlaneacionReabastecimientoAlmacenes(decimal? d_Id_Plneacion_Reabastecimientos, decimal? d_Folio)
		{
			try
			{
				using (LogisticaWCFAPPServiciosClient ws_Cliente = new LogisticaWCFAPPServiciosClient())
				{
					ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
					ws_Cliente.InnerChannel.OperationTimeout = new TimeSpan(0, 2, 0);
					this.st_Planeacion_Reabastecimiento_Almacen = ws_Cliente.ConsultarMovimientosPlaneacion(d_Id_Plneacion_Reabastecimientos, d_Folio).ToList();
					if (this.st_Planeacion_Reabastecimiento_Almacen != null && this.st_Planeacion_Reabastecimiento_Almacen.Count > 0)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> faultException)
			{
				throw new Exception(faultException.Detail.ExcDetalle.Mensaje, faultException);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
		public bool ConsultarPlaneacionReabastecimientoAlmacenesNavi(string s_Fecha_Inicio, string s_Fecha_Fin, string s_Almacen, string s_Mes_Uno, string s_Mes_Dos, string s_Mes_Tres, int i_Dias_Inv_Autorizado, string s_Producto, string s_Anio, string s_Id_Empresa, string s_DiasInventarioAutorizado, string s_Usuario, bool b_EsEBS12)
		{
			try
			{
				using (LogisticaWCFAPPServiciosClient ws_Cliente = new LogisticaWCFAPPServiciosClient())
				{
					//GrabarLogPlanes("========================================================");
					//GrabarLogPlanes("Comienza proceso para obtener información de los planes.");
					ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
					//GrabarLogPlanes("Se asigna tiempo de espera");
					ws_Cliente.InnerChannel.OperationTimeout = new TimeSpan(0, 25, 0);
					//GrabarLogPlanes("Se realiza la consulta");
					this.st_Planeacion_Reabastecimiento_Almacen_Navi = ws_Cliente.ConsultarInformacionNavition(s_Fecha_Inicio, s_Fecha_Fin, s_Almacen, s_Mes_Uno, s_Mes_Dos, s_Mes_Tres, i_Dias_Inv_Autorizado, s_Producto, s_Anio, s_Id_Empresa, s_DiasInventarioAutorizado, s_Usuario, b_EsEBS12).ToList();
					//GrabarLogPlanes("Se obtiene la respuesta y se procede a validar");
					if (this.st_Planeacion_Reabastecimiento_Almacen_Navi != null && this.st_Planeacion_Reabastecimiento_Almacen_Navi.Count > 0)
					{
						//GrabarLogPlanes("Respuesta correcta.");
						return true;
					}
					else
					{
						//GrabarLogPlanes("Respuesta incorrecta.");
						return false;
					}
				}
			}
			catch (ThreadAbortException ErrorHilo)
			{
				//GrabarLogPlanes("==============>.");
				//GrabarLogPlanes("Thread - caught ThreadAbortException - resetting.");
				//GrabarLogPlanes("Exception message: [" + ErrorHilo.Message + "]");
				//GrabarLogPlanes("==============>.");
				//GrabarLogPlanes("Exception message: [" + ErrorHilo.ToString() + "]");
				//Thread.ResetAbort();
				throw ErrorHilo;
			}
			catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> faultException)
			{
				//GrabarLogPlanes("==============>.");
				//GrabarLogPlanes("Error por defecto Libreria [" + faultException.ToString() + "].");
				//GrabarLogPlanes("==============>.");
				//GrabarLogPlanes("Error por defecto Libreria [" + faultException.Message + "].");
				//GrabarLogPlanes("Error por defecto Libreria [" + faultException.Detail.ExcDetalle.Mensaje + "].");
				throw new Exception(faultException.Detail.ExcDetalle.Mensaje, faultException);
			}
			catch (Exception exception)
			{
				//GrabarLogPlanes("==============>.");
				//GrabarLogPlanes("Error por defecto Libreria [" + exception.ToString() + "].");
				//GrabarLogPlanes("==============>.");
				//GrabarLogPlanes("Entró por el default.");
				//GrabarLogPlanes("Error por defecto [" + exception.Message + "].");
				throw exception;
			}
		}
		public bool ConsultarCatalogoLeadTime(string s_Codigo_Almacen, string s_Descripcion_Almacen)
		{
			try
			{
				using (LogisticaWCFAPPServiciosClient ws_Cliente = new LogisticaWCFAPPServiciosClient())
				{
					ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
					ws_Cliente.InnerChannel.OperationTimeout = new TimeSpan(0, 2, 0);
					this.st_Catalogo_Lead_Time = ws_Cliente.ConsultarCatalogoLeadTime(s_Codigo_Almacen, s_Descripcion_Almacen).ToList();
					if (this.st_Catalogo_Lead_Time != null && this.st_Catalogo_Lead_Time.Count > 0)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> faultException)
			{
				throw new Exception(faultException.Detail.ExcDetalle.Mensaje, faultException);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
		#endregion
		#region Funcionalidades
		public void GrabarLogPlanes(string s_TextoGrabar)
		{
			try
			{
				string s_Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
				string s_RutaCsv = s_Path.Replace("file:\\", "") + "\\LogPlanes\\";
				string s_Archivo = s_RutaCsv + "PlanesReabastecimiento.Log";
				string s_FechaSesion = (string.Format("[{0}-{1}]", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString())).Replace("/", "");
				if (!Directory.Exists(s_RutaCsv))
				{
					DirectoryInfo di_Directorio = Directory.CreateDirectory(s_RutaCsv);
				}
				using (StreamWriter swArchivo = File.AppendText(s_Archivo))
				{
					swArchivo.WriteLine(s_FechaSesion + "=>" + s_TextoGrabar);
					swArchivo.Flush();
					swArchivo.Close();
				}
			}
			catch (Exception errorExcepcion)
			{
				throw errorExcepcion;
			}
		}
		#endregion
		#region Grabar
		public CRespuestaGeneral GrabarPlaneacionReabastecimientoAlmacen(C_Planeacion_Reabastecimiento_Almacen st_PlanesReabastecimiento)
		{
			try
			{
				ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
				using (LogisticaWCFAPPServiciosClient ws_Cliente = new LogisticaWCFAPPServiciosClient())
				{
					ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
					ws_Cliente.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
					return ws_Cliente.GrabarMovimientosPlanesReabastecimiento(st_PlanesReabastecimiento);
				}
			}
			catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> faultException)
			{
				throw new Exception(faultException.Detail.ExcDetalle.Mensaje, faultException);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
		public CRespuestaGeneral ActualizarEstatusPlanReabastecimiento(decimal d_Id_Planeacion_Reabastecimiento, decimal d_Id_Estatus_Planeacion)
		{
			try
			{
				ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
				using (LogisticaWCFAPPServiciosClient ws_Cliente = new LogisticaWCFAPPServiciosClient())
				{
					ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
					ws_Cliente.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
					return ws_Cliente.ActualizarEstatusPlanReabastecimiento(d_Id_Planeacion_Reabastecimiento, d_Id_Estatus_Planeacion);
				}
			}
			catch (FaultException<LogisticaSOA.ExcepcionesServicioDLL> faultException)
			{
				throw new Exception(faultException.Detail.ExcDetalle.Mensaje, faultException);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
		#endregion
	}
}