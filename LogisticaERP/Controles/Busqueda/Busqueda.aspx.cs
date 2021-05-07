using DevExpress.Web;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data;
using System.Reflection;


namespace GRUPOPINSA.Controles.Busqueda
{
    public partial class Busqueda : System.Web.UI.Page
    {

        private static List<CampoFiltrar> listaCamposFiltro;
        private static List<CampoResultado> listaCamposResultado;
        //private static eTablasBusqueda TablaBusqueda;
        private static Object TablaBusqueda;
        private static String PrimaryKey;
        private static String Token;
        private static List<FiltroPersonalizado> listaFiltroPersonalizado;
        private static ParametrosBusqueda.ModuloBusqueda BuscaModulo;

        private static BusquedaGeneral BusquedaGeneral = new BusquedaGeneral();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ASPxTokenBox.IncrementalFilteringMode = DevExpress.Web.IncrementalFilteringMode.Contains;
            this.ASPxTokenBox.ShowDropDownOnFocus = DevExpress.Web.ShowDropDownOnFocusMode.Always;
            this.ASPxTokenBox.AllowCustomTokens = false;

            if (!IsPostBack)
            {
                //var parametros = SessionHelperBusqueda.Parametros_de_busqueda;
                var parametros = Session["Parametros_de_busqueda"] as ParametrosBusqueda;
                
                if (parametros != null)
                {
                    //tomamos los valores que se envian como parametros
                    
                    listaCamposFiltro = parametros.Filtros;
                    listaCamposResultado = parametros.Resultados;
                    TablaBusqueda = parametros.TablaBusqueda;
                    Token = parametros.Token;
                    listaFiltroPersonalizado = parametros.FiltroPersonalizado;
                    BuscaModulo = parametros.BuscaModulo;
                    this.hfProcedimiento.Value = parametros.functionCallback;
                    this.grdResultados.Settings.ShowFilterRow = parametros.VisualizarAutofiltro;
                    

                    if (BuscaModulo == ParametrosBusqueda.ModuloBusqueda.INVENTARIOS)
                    {
                        this.hfConsulta.Value = ((LogisticaERP.InventariosSOA.eTablasBusqueda)Convert.ToDecimal(TablaBusqueda)).ToString();
                    }
                    else if (BuscaModulo == ParametrosBusqueda.ModuloBusqueda.COMPRAS)
                    {
                        this.hfConsulta.Value = ((LogisticaERP.ComprasSOA.eTablasBusqueda)Convert.ToDecimal(TablaBusqueda)).ToString();
                    }
                    else if (BuscaModulo == ParametrosBusqueda.ModuloBusqueda.MANTENIMIENTO)
                    {
                        this.hfConsulta.Value = ((LogisticaERP.MantenimientoSOA.eTablasBusqueda)Convert.ToDecimal(TablaBusqueda)).ToString();
                    }
                    else if (BuscaModulo == ParametrosBusqueda.ModuloBusqueda.PROYECTOS)
                    {
                        this.hfConsulta.Value = ((LogisticaERP.ProyectosSOA.eTablasBusqueda)Convert.ToDecimal(TablaBusqueda)).ToString();
                    }
                    else if(BuscaModulo == ParametrosBusqueda.ModuloBusqueda.GRUPOPINSA)
                    {
                        this.hfConsulta.Value = ((LogisticaERP.GrupoPinsaSOA.eTablasBusqueda)Convert.ToDecimal(TablaBusqueda)).ToString();
                    }
                    else if (BuscaModulo == ParametrosBusqueda.ModuloBusqueda.SEGURIDAD)
                    {
                        this.hfConsulta.Value = ((LogisticaERP.SeguridadERPSOA.eTablasBusqueda)Convert.ToDecimal(TablaBusqueda)).ToString();
                    }

                    this.ASPxTokenBox.DataSource = listaCamposFiltro;
                    this.ASPxTokenBox.DataBind();

                    //se agregan los tokens marcados por default
                    listaCamposFiltro.ForEach(cf =>
                    {
                        if (cf.Default)
                        {
                            this.ASPxTokenBox.Tokens.Add(cf.Caption);
                        }
                    });

                    //limpiamos la session
                    //if (SessionHelperBusqueda.Resultado_Busqueda != null) { SessionHelperBusqueda.Resultado_Busqueda = null; }
                    if (Session["Resultado_Busqueda"] != null) { Session["Resultado_Busqueda"] = null; }

                    if (parametros.PreCargarDatos)
                    {
                        btnBuscar_Click(null, null);
                    }
                }
            }

            if (Session["Resultado_Busqueda"] != null)
            {
                this.grdResultados.DataSource = Session["Resultado_Busqueda"] as DataTable;
                this.grdResultados.DataBind();
            }

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                var parametros = Session["Parametros_de_busqueda"] as ParametrosBusqueda;
                if (!parametros.PreCargarDatos)
                {
                    if (this.txtFiltro.Text.Trim() == String.Empty) { throw new Exception("Favor de introducir el criterio a filtrar"); }
                }
                else
                {
                    txtFiltro.Text = "%";
                }

                if (this.ASPxTokenBox.Value.ToString().Trim() == String.Empty) { throw new Exception("Favor de seleccionar los campos por los cuales se realizara el filtro"); }

                List<String> FiltrosPersonalizado = new List<string>();
                listaFiltroPersonalizado.ForEach(c =>
                {
                    FiltrosPersonalizado.Add(c.ObtieneCampoFiltro());
                });


                //si no estan creadas las columnas, se crean una sola vez
                if (this.grdResultados.Columns.Count <= 0)
                {
                    listaCamposResultado.ForEach(r =>
                    {
                        if (r.Tipo == typeof(bool))
                        {
                            GridViewDataCheckColumn columncheck = new GridViewDataCheckColumn()
                            {
                                Caption = r.Titulo,
                                FieldName = r.NombreCampo,
                                Visible = r.Visible
                                
                            };

                            columncheck.PropertiesCheckEdit.DisplayFormatString = r.DisplayFormatString;
                            columncheck.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                            columncheck.Settings.AllowAutoFilterTextInputTimer = DevExpress.Utils.DefaultBoolean.False;
                            columncheck.HeaderStyle.Font.Bold = true;
                            columncheck.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            grdResultados.Columns.Add(columncheck);
                        }
                        else if (r.Tipo == typeof(DateTime))
                        {
                            GridViewDataDateColumn columndate = new GridViewDataDateColumn()
                            {
                                Caption = r.Titulo,
                                FieldName = r.NombreCampo,
                                Visible = r.Visible,
                                
                            };

                            columndate.PropertiesDateEdit.DisplayFormatString = r.DisplayFormatString;
                            columndate.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                            columndate.Settings.AllowAutoFilterTextInputTimer = DevExpress.Utils.DefaultBoolean.False;
                            columndate.HeaderStyle.Font.Bold = true;
                            columndate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            grdResultados.Columns.Add(columndate);

                        }
                        else
                        {
                            GridViewDataTextColumn column = new GridViewDataTextColumn()
                            {
                                Caption = r.Titulo,
                                FieldName = r.NombreCampo,
                                Visible = r.Visible
                            };

                            column.PropertiesTextEdit.DisplayFormatString = r.DisplayFormatString;
                            column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                            column.Settings.AllowAutoFilterTextInputTimer = DevExpress.Utils.DefaultBoolean.False;
                            column.HeaderStyle.Font.Bold = true;
                            column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            grdResultados.Columns.Add(column);
                        }

                       

                        if (r.Respuesta)
                        {
                            grdResultados.KeyFieldName = r.NombreCampo;
                            PrimaryKey = r.NombreCampo;
                        }

                    });
                }

                string filtro = string.Empty;
                ASPxTokenBox.Value.ToString().Split(new Char[] { ',' }).ToList().ForEach(x => filtro += txtFiltro.Text + ",");
                filtro = filtro.Substring(0, filtro.Length - 1);

                Session["Resultado_Busqueda"] = BusquedaGeneral.Busqueda(FiltrosPersonalizado, ASPxTokenBox.Value.ToString(), filtro, Token, TablaBusqueda, BuscaModulo);				
                
                this.grdResultados.DataSource = Session["Resultado_Busqueda"] as DataTable; // Datos;
                this.grdResultados.KeyFieldName = PrimaryKey;
                this.grdResultados.DataBind();

                if ( (Session["Resultado_Busqueda"] as DataTable).Rows.Count > 0)
                {
                    this.grdResultados.Focus();
                    this.grdResultados.FocusedRowIndex = 0;
                }
                

                if (Session["Resultado_Busqueda"] != null)
                {
                    if ( (Session["Resultado_Busqueda"] as DataTable).Rows.Count == 0 )
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp_resultado", "MostrarCajaMensajes(WARNINGBOX, [{ 'StrongText': 'El Criterio de busqueda no encontro coincidencias.' }], 7000);", true);
                    }
                }
            }
            catch (FaultException<LogisticaERP.GrupoPinsaSOA.ExcepcionesServicioDLL> Faultexc)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel, this.UpdatePanel.GetType(), "temp_error", "MostrarCajaMensajes(ERRORBOX, [{ \"StrongText\": \"" + Faultexc.Detail.Mensaje + "\" }], 7000);", true);
            }
            catch (FaultException<LogisticaERP.InventariosSOA.ExcepcionesServicioDLL> Faultexc)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel, this.UpdatePanel.GetType(), "temp_error", "MostrarCajaMensajes(ERRORBOX, [{ \"StrongText\": \"" + Faultexc.Detail.Mensaje + "\" }], 7000);", true);
            }
            catch (FaultException<LogisticaERP.ComprasSOA.ExcepcionesServicioDLL> Faultexc)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel, this.UpdatePanel.GetType(), "temp_error", "MostrarCajaMensajes(ERRORBOX, [{ \"StrongText\": \"" + Faultexc.Detail.Mensaje + "\" }], 7000);", true);
            }
            catch (FaultException<LogisticaERP.MantenimientoSOA.ExcepcionesServicioDLL> Faultexc)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel, this.UpdatePanel.GetType(), "temp_error", "MostrarCajaMensajes(ERRORBOX, [{ \"StrongText\": \"" + Faultexc.Detail.Mensaje + "\" }], 7000);", true);
            }
            catch (FaultException<LogisticaERP.ProyectosSOA.ExcepcionesServicioDLL> Faultexc)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel, this.UpdatePanel.GetType(), "temp_error", "MostrarCajaMensajes(ERRORBOX, [{ \"StrongText\": \"" + Faultexc.Detail.Mensaje + "\" }], 7000);", true);
            }
            catch (FaultException<LogisticaERP.SeguridadERPSOA.ExcepcionesServicioDLL> Faultexc)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel, this.UpdatePanel.GetType(), "temp_error", "MostrarCajaMensajes(ERRORBOX, [{ \"StrongText\": \"" + Faultexc.Detail.Mensaje + "\" }], 7000);", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel, this.UpdatePanel.GetType(), "temp_error", "MostrarCajaMensajes(ERRORBOX, [{ \"StrongText\": \"" + ex.Message + "\" }], 7000);", true);
            }
        }

        protected void grdResultados_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            decimal Id = Convert.ToDecimal(e.Parameters);
            object respuesta = BusquedaGeneral.FindItem(Id, PrimaryKey);

            grdResultados.JSProperties["cp_resultado"] = respuesta;
            grdResultados.JSProperties["cp_Id"] = Id;

        }
    }
}