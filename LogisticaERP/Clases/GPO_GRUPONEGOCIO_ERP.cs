using LogisticaERP.GrupoPinsaSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace LogisticaERP.Clases
{
    public class GPO_GRUPONEGOCIO_ERP : ClaseBase
    {
        public List<GrupoNegocioERP> ObtenerGruposNegocioERP(decimal? id_grupo_negocio_erp, string nombre, bool? activo)
        {
            List<GrupoNegocioERP> listaGrupoNegocioERP = new List<GrupoNegocioERP>();

            try
            {
                using (GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient servicio = new GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient())
                {
                    ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;

                    listaGrupoNegocioERP = servicio.ObtenerGruposNegocioERP(id_grupo_negocio_erp, nombre, activo).ToList();
                }
            }
            catch (FaultException<GrupoPinsaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                throw new Exception(Faultexc.Detail.Mensaje, Faultexc);
            }
            catch (Exception exc)
            {
                throw exc;
            }

            return listaGrupoNegocioERP;
        }

        public override bool Grabar()
        {
            throw new NotImplementedException();
        }

        public override bool Busqueda()
        {
            throw new NotImplementedException();
        }
    }
}