using System;
using System.IO;
using System.Web;
using System.Linq;
using System.ServiceModel;
using LogisticaERP.LogisticaSOA;
using System.Collections.Generic;

namespace LogisticaERP.Clases
{
	public class Asn_Ejecutar_Concurrente : ClaseBase
	{
		#region Variables
		#endregion
		#region Constructor
		public Asn_Ejecutar_Concurrente()
		{
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
		#region Ejecuciones
		public decimal EjecutarConcurrente_GPIN_EJECUTAR_ASN_DINAM(string s_Factura, string s_Serie)
		{
			try
			{
				decimal d_Retorno_Concurrente = 0;
				using (LogisticaWCFAPPServiciosClient ws_Cliente = new LogisticaWCFAPPServiciosClient())
				{
					ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
					ws_Cliente.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
					d_Retorno_Concurrente = ws_Cliente.EjecutarConcurrente_GPIN_EJECUTAR_ASN_DINAM(s_Factura, s_Serie);
				}
				return d_Retorno_Concurrente;
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