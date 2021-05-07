using DevExpress.Web;
using LogisticaERP.Clases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace LogisticaERP.Controles.BusquedaGenerica
{
    public partial class Busqueda : PaginaBase
    {
        private const string _identityGridViewDataSource = "ERP>Controles>BusquedaGenerica";
        private const string _identityGridViewKeyFieldName = "ERP>Controles>BusquedaGenerica>KeyFieldName";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //extracción de parametros
                var parametro = Session[ParametroGridView._sessionBusquedaGenerica] as ParametroGridView;
                if (parametro == null) return;
                //valida existencia de un keyfieldname en la asignación de campos
                string keyFieldName = parametro.Campos.FirstOrDefault(x => x.IsKeyFieldName).FieldName;
                if (keyFieldName == null)
                {
                    ControladorMensajes.MostrarMensaje(UpdatePanel1, "Error Ausencia de KeyFieldName", ControladorMensajes.TipoMensaje.Error, "Favor de asignar un KeyFieldName.");
                    return;
                }

                hfFunctionCallback.Value = parametro.FunctionCallback;

                GridViewBG.Styles.Header.Font.Bold = true;
                GridViewBG.Styles.Header.HorizontalAlign = parametro.HeaderHorizontalAlign;
                GridViewBG.SettingsPager.PageSize = parametro.PageSize;
                GridViewBG.SettingsPager.NumericButtonCount = parametro.NumericButtonCount;

                if (parametro.ShowSelectCheckbox)
                {
                    panelDeControles.Style.Add("display", "block");
                    mainContent.Attributes.Add("class", "custom-content-popup");
                    var gridViewCommandColumn = new GridViewCommandColumn { ShowSelectCheckbox = true, SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page, Width = Unit.Pixel(50) };
                    GridViewBG.Columns.Add(gridViewCommandColumn);
                }

                IEnumerable<Campo> campos = parametro.Campos;
                foreach (var x in campos.Where(x => (x.IsKeyFieldName && !string.IsNullOrEmpty(x.Caption)) || !x.IsKeyFieldName))
                {
                    if (x.DataType == typeof(decimal))
                    {
                        var gridViewDataSpinEditColumn = new GridViewDataSpinEditColumn() { Caption = x.Caption, FieldName = x.FieldName, Width = Unit.Pixel(x.Width) };
                        gridViewDataSpinEditColumn.CellStyle.HorizontalAlign = x.CellHorizontalAlign;
                        gridViewDataSpinEditColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                        gridViewDataSpinEditColumn.Settings.AllowAutoFilterTextInputTimer = DevExpress.Utils.DefaultBoolean.False;
                        gridViewDataSpinEditColumn.PropertiesSpinEdit.DisplayFormatString = x.DisplayFormatString;
                        GridViewBG.Columns.Add(gridViewDataSpinEditColumn);
                    }
                    else if (x.DataType == typeof(DateTime))
                    {
                        var gridViewDataDateColumn = new GridViewDataDateColumn() { Caption = x.Caption, FieldName = x.FieldName, Width = Unit.Pixel(x.Width) };
                        gridViewDataDateColumn.CellStyle.HorizontalAlign = x.CellHorizontalAlign;
                        gridViewDataDateColumn.PropertiesDateEdit.DisplayFormatString = x.DisplayFormatString;
                        GridViewBG.Columns.Add(gridViewDataDateColumn);
                    }
                    else if (x.DataType == typeof(bool))
                    {
                        var gridViewDataCheckColumn = new GridViewDataCheckColumn() { Caption = x.Caption, FieldName = x.FieldName, Width = Unit.Pixel(x.Width) };
                        gridViewDataCheckColumn.CellStyle.HorizontalAlign = x.CellHorizontalAlign;
                        GridViewBG.Columns.Add(gridViewDataCheckColumn);
                    }
                    else if (x.DataType == typeof(Button))
                    {
                        var gridViewCommandColumn = new GridViewCommandColumn { Caption = x.Caption, Width = Unit.Pixel(x.Width), ButtonType = GridViewCommandButtonType.Image };
                        foreach (var item in GridViewBG.Columns)
                        {
                            if (item.GetType() == typeof(GridViewCommandColumn) && !(item as GridViewCommandColumn).ShowSelectCheckbox)
                            {
                                gridViewCommandColumn = (GridViewCommandColumn)item;
                                break;
                            }
                        }

                        GridViewCommandColumnCustomButton gridViewCommandColumnCustomButton;
                        x.ListaCampoTipoBoton.ForEach(y =>
                        {
                            gridViewCommandColumnCustomButton = new GridViewCommandColumnCustomButton();
                            gridViewCommandColumnCustomButton.Index = y.Index;
                            gridViewCommandColumnCustomButton.ID = y.Id;
                            gridViewCommandColumnCustomButton.Image.IconID = y.IconId;
                            gridViewCommandColumnCustomButton.Image.ToolTip = y.Tooltip;
                            gridViewCommandColumn.CustomButtons.Add(gridViewCommandColumnCustomButton);
                        });

                        GridViewBG.Columns.Add(gridViewCommandColumn);

                        if (!string.IsNullOrEmpty(x.ClientSideEventsCustomButtonClick))
                            GridViewBG.ClientSideEvents.CustomButtonClick = x.ClientSideEventsCustomButtonClick;
                    }
                    else
                    {
                        var gridViewDataTextColumn = new GridViewDataTextColumn() { Caption = x.Caption, FieldName = x.FieldName, Width = Unit.Pixel(x.Width), };
                        gridViewDataTextColumn.CellStyle.HorizontalAlign = x.CellHorizontalAlign;
                        gridViewDataTextColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                        gridViewDataTextColumn.Settings.AllowAutoFilterTextInputTimer = DevExpress.Utils.DefaultBoolean.False;
                        gridViewDataTextColumn.PropertiesTextEdit.DisplayFormatString = x.DisplayFormatString;
                        GridViewBG.Columns.Add(gridViewDataTextColumn);
                    }
                }

                Session[_identityGridViewDataSource] = parametro.DataSource;
                Session[_identityGridViewKeyFieldName] = campos.First(x => x.IsKeyFieldName).FieldName;
                //Session[ParametroGridView._sessionBusquedaGenerica] = null;
            }

            FillGridView();
        }

        protected void GridViewBG_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters))
            {
                switch (e.Parameters)
                {
                    case "OBTENER_FILAS_SELECCIONADAS":
                        var listObject = new List<object>();
                        GridViewBG.GetSelectedFieldValues(GridViewBG.KeyFieldName).ForEach(x => listObject.Add(FindItem(Convert.ToDecimal(x), GridViewBG.KeyFieldName)));

                        GridViewBG.JSProperties["cpRow"] = new
                        {
                            Movimiento = "OBTENER_FILAS_SELECCIONADAS",
                            Datos = listObject
                        };

                        if (listObject.Any())
                            Session[_identityGridViewDataSource] = null;

                        break;
                    default:
                        decimal key = Convert.ToDecimal(e.Parameters);
                        var row = FindItem(key, Session[_identityGridViewKeyFieldName].ToString());
                        GridViewBG.JSProperties["cpRow"] = new
                        {
                            Movimiento = string.Empty,
                            Datos = row
                        };

                        Session[_identityGridViewDataSource] = null;
                        break;
                }
            }
        }

        #endregion

        #region Methods

        public object FindItem(decimal key, string keyFildName)
        {
            decimal value = 0;
            object resultado = new object();
            var dataSource = Session[_identityGridViewDataSource] as IEnumerable;
            foreach (object row in dataSource)
            {
                value = Convert.ToDecimal(row.GetType().GetProperty(keyFildName).GetValue(row));
                if (value == key)
                {
                    resultado = row;
                    break;
                }
            }
            return resultado;
        }

        private void FillGridView()
        {
            GridViewBG.DataSource = Session[_identityGridViewDataSource];
            GridViewBG.KeyFieldName = Session[_identityGridViewKeyFieldName].ToString();
            GridViewBG.DataBind();
        }

        #endregion

    }
}