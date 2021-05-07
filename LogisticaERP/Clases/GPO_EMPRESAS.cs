using LogisticaERP.GrupoPinsaSOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;


namespace LogisticaERP.Clases
{
    public class GPO_EMPRESAS : ClaseBase
    {
        private List<Empresa> listaEmpresas;

        public GPO_EMPRESAS()
        {

        }

        public GPO_EMPRESAS(HttpContext state)
            : base(state)
        {

        }

        public List<Empresa> ObtieneListaEmpresas()
        {
            listaEmpresas = new List<Empresa>();

            try
            {
                if (!string.IsNullOrEmpty(Token))
                {
                    using (GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient grupopinsa = new GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient())
                    {
                        ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
                        this.listaEmpresas = grupopinsa.ObtenerEmpresa(null, null, null, null, null, null, false, true).OrderBy(e => e.Nombre_comercial).ToList();
                    }
                }

                return this.listaEmpresas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Empresa ObtenerEmpresa(decimal id_empresa)
        {
            Empresa empresa = null;

            try
            {
                if (!string.IsNullOrEmpty(Token))
                {
                    using (GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient grupopinsa = new GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient())
                    {
                        ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
                        empresa = grupopinsa.ObtenerEmpresa(id_empresa, null, null, null, null, null, false, true).ToList().FirstOrDefault();
                    }
                }

                return empresa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Empresa> ObtieneListaEmpresasGrupoNegocio()
        {
            try
            {
                if (!string.IsNullOrEmpty(Token))
                {
                    using (GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient grupopinsa = new GrupoPinsaWCFAPPServiciosGrupoPinsaServiceClient())
                    {
                        ExtensionesServiciosWCF.Extensiones.ClienteAutenticacionHeader.AutenticacionHeaderInfo.Token = Token;
                        this.listaEmpresas = grupopinsa.ObtenerEmpresa(null, null, null, null, null, null, false, true).OrderBy(e => e.Nombre_comercial).ToList();

                        this.listaEmpresas = (from lgn in Newtonsoft.Json.JsonConvert.DeserializeObject<GrupoNegocioERP>(contextoSesion.Session["GrupoNegocioSerializado"].ToString()).ListaGrupoNegocioERPEmpresa
                                              join lue in this.listaEmpresas on lgn.Id_empresa equals lue.Id_empresa
                                              orderby lue.Nombre_comercial
                                              select lue).ToList();
                    }
                }

                return this.listaEmpresas;
            }
            catch (FaultException<GrupoPinsaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                throw Faultexc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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