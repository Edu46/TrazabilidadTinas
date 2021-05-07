<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="BusquedaViajes.aspx.cs" Inherits="LogisticaERP.Catalogos.BusquedaViajes" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" EnablePageMethods="true" runat="server" />
    <input type="hidden" value="Busqueda Viajes" id="hPage" />
    <input type="hidden" runat="server" id="hfEmpresaCloudException" value="" />
    <section class="custom-content">
        <div class="bs-component">
            <div class="clearfix">
                <div id="accordion" style="margin-bottom: 8px;" class="accordion-style1 panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading" data-toggle="tooltip" data-container="body" data-placement="auto" title="" data-original-title="Filtros de Búsqueda">
                            <h4 class="panel-title">
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">Filtros de Búsqueda</a>
                            </h4>
                        </div>
                        <div class="panel-collapse collapse in" id="collapseOne">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="box custom-box box-primary" style="margin-bottom: 0px;">
                                            <div class="box-body">
                                                <div class="row row-bottom">                                                   
                                                    <div class="col-lg-3">
                                                        <label for="lblViaje" class="control-label-top">Tipo Viaje:</label>
                                                        <asp:DropDownList runat="server" CssClass="multiselect form-control marginoff" data-style="btn-select-default" ID="ddlTipoViaje" placeholder="Tipo Viaje" AppendDataBoundItems="true" validate="true">
                                                            <asp:ListItem Value="0">..:: Seleccionar ::..</asp:ListItem>
                                                            <asp:ListItem Value="1">TRANSFERENCIA</asp:ListItem>
                                                            <asp:ListItem Value="2">ENTREGA A CLIENTE</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <label for="lblAlmacen" class="control-label-top">Tipo Almacen:</label>
                                                        <asp:DropDownList runat="server" CssClass="multiselect form-control marginoff" data-style="btn-select-default" ID="ddlTipoAlmacen" placeholder="Tipo Almacen" AppendDataBoundItems="true" validate="true">
                                                            <asp:ListItem Value="0">..:: Seleccionar ::..</asp:ListItem>
                                                            <asp:ListItem Value="1">CEDIS</asp:ListItem>
                                                            <asp:ListItem Value="2">PORTEADOR</asp:ListItem>
                                                            <asp:ListItem Value="3">REPRESENTANTE</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <label for="lblAlm" class="control-label-top">Almacen:</label>
                                                        <asp:DropDownList runat="server" CssClass="multiselect form-control marginoff" data-style="btn-select-default" ID="ddlAlmacen" placeholder="Origen" AppendDataBoundItems="true" validate="true">
                                                            <asp:ListItem Value="0">..:: Seleccionar ::..</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="col-lg-3">
                                                        <label for="lblProveedor" class="control-label-top">Proveedor:</label>
                                                        <asp:DropDownList runat="server" CssClass="multiselect form-control marginoff" data-style="btn-select-default" ID="ddlProveedor" placeholder="Proveedor" AppendDataBoundItems="true" validate="true">
                                                            <asp:ListItem Value="0">..:: Seleccionar ::..</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                     <div class="col-sm-2">
                                                        <label for="lblFolio" class="control-label-top">Folio Viaje:</label>
                                                        <asp:TextBox ID="txtFolioViaje" runat="server" CssClass="form-control marginoff" MaxLength="50" placeholder="" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <label for="lblFechaInicial" class="control-label-top">Fecha Inicial:</label>
                                                        <dx:ASPxDateEdit ID="deFechaInicio" runat="server" placeholder="Fecha Inicial" autocomplete="off" CssClass="form-control-date" Theme="MetropolisBlue" Date="">
                                                            <ClientSideEvents DateChanged="function() { setDate(); }" />
                                                        </dx:ASPxDateEdit>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <label for="lblFechaFinal" class="control-label-top">Fecha Final:</label>
                                                        <dx:ASPxDateEdit ID="deFechaFin" runat="server" placeholder="Fecha Final" autocomplete="off" CssClass="form-control-date" Theme="MetropolisBlue" Date=""></dx:ASPxDateEdit>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <a id="btnBuscar" class="btn custom-btn btn-link" style="margin-top: 19px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Buscar Viajes"><i class="fa fa-search" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i>Buscar</a>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <a id="btnExportar" class="btn custom-btn btn-link" style="margin-top: 19px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Exportar"><i class="fa fa-file-excel-o" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i>Exportar</a>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <a id="btnLimpiar" class="btn custom-btn btn-link" style="margin-top: 19px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Exportar"><i class="fa fa-eraser" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i>Limpiar</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12" style="margin-top: 0px;">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Viajes</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <dx:ASPxGridView
                                                runat="server"
                                                ID="GridViajes"
                                                ClientInstanceName="GridViajes"
                                                Theme="MetropolisBlue"
                                                Width="100%"
                                                AutoGenerateColumns="False"
                                                KeyboardSupport="True"
                                                AccessKey="1">
                                                <ClientSideEvents
                                                    Init="function(s, e) {
                                            OnInit(s, e, '');
                                            resizeHeightDevControl(window, window, '#GridViajes', GridViajes, 110, 0);
                                            $(window).resize();
                                        }"
                                                    EndCallback="function (s, e) {
                                            if(s.cpException !== undefined) {
                                                showMessage(messageType.ERRORBOX, s.cpException);
                                                s.cpException = undefined;
                                            }
                          
                                        }" />
                                                <Columns>
                                                    <dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowClearFilterButton="true" ShowSelectCheckbox="false" SelectAllCheckboxMode="None" Width="80px" VisibleIndex="0">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <CustomButtons>
                                                            <dx:GridViewCommandColumnCustomButton ID="btnVerReporte" Image-IconID="reports_addgroupheader_16x16" Image-ToolTip="Ver Reporte Orden Trabajo Envio Almacen" Visibility="BrowsableRow" />
                                                        </CustomButtons>
                                                    </dx:GridViewCommandColumn>
                                                    <dx:GridViewDataTextColumn FieldName="FolioViaje" Caption="Folio Viaje" Width="150px" VisibleIndex="1">
                                                        <PropertiesTextEdit ClientInstanceName="FolioViaje" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="TipoViaje" Caption="Tipo Viaje" Width="150px" VisibleIndex="2">
                                                        <PropertiesTextEdit ClientInstanceName="TipoViaje" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Descripcion" Caption="Descripcion" Width="150px" VisibleIndex="3">
                                                        <PropertiesTextEdit ClientInstanceName="Descripcion" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="NombreAlmacenOracle" Caption="Almacen" Width="150px" VisibleIndex="4">
                                                        <PropertiesTextEdit ClientInstanceName="NombreAlmacenOracle" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="ProveedorOracle" Caption="Proveedor" Width="250px" VisibleIndex="5">
                                                        <PropertiesTextEdit ClientInstanceName="ProveedorOracle" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataDateColumn FieldName="FechaEnvio" Caption="Fecha Envio" Width="150px" VisibleIndex="6">
                                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" ClientInstanceName="FechaEnvio" Style-HorizontalAlign="Justify"></PropertiesDateEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataDateColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Folio_OC" Caption="Folio OC" Width="150px" VisibleIndex="7">
                                                        <PropertiesTextEdit ClientInstanceName="Folio_OC" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Entregas" Caption="Entregas" Width="150px" VisibleIndex="8">
                                                        <PropertiesTextEdit ClientInstanceName="Entregas" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Flete" Caption="Flete" Width="150px" VisibleIndex="9">
                                                        <PropertiesTextEdit ClientInstanceName="Flete" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Caseta" Caption="Caseta" Width="150px" VisibleIndex="10">
                                                        <PropertiesTextEdit ClientInstanceName="Caseta" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />                                                    
                                                    </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn FieldName="Chofer" Caption="Chofer" Width="150px" VisibleIndex="11">
                                                        <PropertiesTextEdit ClientInstanceName="Chofer" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn FieldName="PlacaTractor" Caption="Placa Tractor" Width="150px" VisibleIndex="12">
                                                        <PropertiesTextEdit ClientInstanceName="PlacaTractor" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn FieldName="PlacaCaja" Caption="Placa Caja" Width="150px" VisibleIndex="13">
                                                        <PropertiesTextEdit ClientInstanceName="PlacaCaja" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn FieldName="Marca" Caption="Marca" Width="150px" VisibleIndex="14">
                                                        <PropertiesTextEdit ClientInstanceName="Marca" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn FieldName="Modelo" Caption="Modelo" Width="150px" VisibleIndex="15">
                                                        <PropertiesTextEdit ClientInstanceName="Modelo" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn FieldName="NumeroECOCaja" Caption="Número ECON Caja" Width="150px" VisibleIndex="16">
                                                        <PropertiesTextEdit ClientInstanceName="NumeroECOCaja" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn FieldName="NumeroSello" Caption="Número Sello" Width="150px" VisibleIndex="17">
                                                        <PropertiesTextEdit ClientInstanceName="NumeroSello" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn FieldName="NumeroSerieCaja" Caption="Número Serie Caja" Width="150px" VisibleIndex="18">
                                                        <PropertiesTextEdit ClientInstanceName="NumeroSerieCaja" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn FieldName="CodigoTransporte" Caption="Codigo Transporte" Width="150px" VisibleIndex="19">
                                                        <PropertiesTextEdit ClientInstanceName="CodigoTransporte" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="InformacionCompleta" Caption="Informacion Adicional" Width="150px" VisibleIndex="20">
                                                        <PropertiesTextEdit ClientInstanceName="InformacionCompleta" Style-HorizontalAlign="Justify"></PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataTextColumn>

                                                </Columns>
                                                <Settings VerticalScrollableHeight="225" VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" ShowFilterRow="true" ShowFilterRowMenu="true" ShowHeaderFilterButton="true" ShowFooter="false" />
                                                <SettingsBehavior AllowFocusedRow="true" AllowDragDrop="false" />
                                                <SettingsPager AlwaysShowPager="true" PageSize="30" />
                                            </dx:ASPxGridView>
                                            <div style="height: 40px;"></div>
                                            <dx:ASPxGridViewExporter ID="GVExportar" GridViewID="GridViajes" runat="server" OnRenderBrick="exportarGrid_RenderBrick"></dx:ASPxGridViewExporter>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="display: none">
            <asp:Button runat="server" ID="btnExportarInterno" UseSubmitBehavior="False" CssClass="btn custom-btn btn-primary btn-exporter" Text="Exportar a Excel" OnClick="btnExportarInterno_Click" />
        </div>
    </section>
    <script type="text/javascript">
        "use strict";

        $("#btnLimpiar").click(function () {

            $("#ddlTipoViaje").val("0").multiselect('refresh');;
            $("#ddlTipoAlmacen").val("0").multiselect('refresh');;
            $("#ddlAlmacen").val("0").multiselect('refresh');;
            $("#ddlProveedor").val("0").multiselect('refresh');;
            $("#txtFolioViaje").val("");
            deFechaInicio.Clear();
            deFechaFin.Clear();

            showLoading(true);

            $.ajax({
                url: "BusquedaViajes.aspx/Limpiar",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                timeout: 600000,
                success: function (data) {
                    showLoading(false);
                    GridViajes.Refresh();
                },
                error: function (error) {
                    showLoading(false);
                    var exception = JSON.parse(error.responseText);
                    showMessage(messageType.ERRORBOX, exception.Message);
                }
            });
          
        });


        $("#btnBuscar").click(function () {

            var fechaInicio = deFechaInicio.GetText();
            var fechaFin = deFechaFin.GetText();
            var tipoViaje = parseInt($("#ddlTipoViaje").val()) > 0 ? $("#ddlTipoViaje option:selected").text() : null;
            var tipoAlmacen = parseInt($("#ddlTipoAlmacen").val()) > 0 ? $("#ddlTipoAlmacen option:selected").text() : null;
            var almacen = $("#ddlAlmacen").val();
            var proveedor = $("#ddlProveedor").val();
            var folio = $("#txtFolioViaje").val() != '' ? $("#txtFolioViaje").val() : null;

            /*if (!fechaInicio || !fechaFin) {
                showMessage(messageType.WARNINGBOX, "Favor de seleccionar un rango de fechas a buscar.");
                return;
            }*/

            showLoading(true);

            $.ajax({
                url: "BusquedaViajes.aspx/ObtenerViajes",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{" +
                    " folio: '" + folio + "'" +
                    " ,tipoViaje: '" + tipoViaje + "'" +
                    " ,tipoAlmacen: '" + tipoAlmacen + "'" +
                    ", idAlmacen: '" + almacen + "'" +
                    ", idProveedor: '" + proveedor + "'" +
                    ", fechaInicio: '" + fechaInicio + "'" +
                    ", fechaFin: '" + fechaFin + "'" +
                    " }",
                timeout: 600000,
                success: function (data) {
                    showLoading(false);
                    GridViajes.Refresh();
                },
                error: function (error) {
                    showLoading(false);
                    var exception = JSON.parse(error.responseText);
                    showMessage(messageType.ERRORBOX, exception.Message);
                }
            });
        });

        $("#btnExportar").click(function () {
            if (!GridViajes.pageRowCount) {
                showMessage(messageType.INFORMATIONBOX, "No existen registros para exportar a Excel.");
                return;
            } else {
                $('#btnExportarInterno').click();
            }
        });


        var messageType = {
            SUCCESSBOX: SUCCESSBOX,
            WARNINGBOX: WARNINGBOX,
            ERRORBOX: ERRORBOX,
            INFORMATIONBOX: INFORMATIONBOX
        };

        function showLoading(display) {
            if (display) {
                showUpdateProgressLoading();
            } else {
                hideUpdateProgressLoading();
            }
        }


        function showMessage(messageType, sms, delay) {
            delay = delay || 7000;
            MostrarCajaMensajes(messageType, sms, delay);
        }

        Sys.Application.add_load(Scripts_Load);
        //Sys.Application.add_load(attrEvents);
        //Sys.Application.add_load(attrEvents2);
    </script>

</asp:Content>
