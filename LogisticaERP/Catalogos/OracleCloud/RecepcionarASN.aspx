<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="RecepcionarASN.aspx.cs" Inherits="LogisticaERP.Catalogos.OracleCloud.RecepcionarASN" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modal-content label {
            padding-left: 0 !important;
            padding-right: 0 !important;
        }

        .modal-content input {
            height: 26px !important;
        }

        .multiselect-container > li > a > label.radio, .multiselect-container > li > a > label.checkbox {
            margin-left: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" value="Recepcionar ASNs" id="hPage" />
    <div style="display: none">
        <asp:Button runat="server" ID="btnExportarInterno" UseSubmitBehavior="False" CssClass="btn custom-btn btn-primary btn-exporter" Text="Exportar a Excel" OnClick="btnExportarInterno_Click" />
    </div>
    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" EnablePageMethods="true" runat="server" />
    <!-- Hidden field -->
    <input type="hidden" runat="server" id="hfEmpresaCloudException" value="" />
    <input type="hidden" runat="server" id="hfIdEmpresaEBS12" value="" />
    <!-- Main content -->
    <section class="custom-content">
        <div class="bs-component">
            <div class="clearfix">
                <div id="accordion" style="margin-bottom: 8px;" class="accordion-style1 panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading" data-toggle="tooltip" data-container="body" data-placement="auto" title="" data-original-title="Dar click para mostrar/ocultar Filtros de Búsqueda">
                            <h4 class="panel-title">
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                                    <i class="accordion-icon fa fa-angle-down" data-icon-hide="accordion-icon fa fa-angle-down" data-icon-show="accordion-icon fa fa-angle-right"></i>
                                    Filtros de Búsqueda/Acciones
                                </a>
                            </h4>
                        </div>
                        <div class="panel-collapse collapse in" id="collapseOne">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <!-- /.box-content -->
                                        <div class="box custom-box box-primary" style="margin-bottom: 8px;">
                                            <!-- /.box-header -->
                                            <!--<div class="box-header with-border">
                                                <h3 class="box-title">Titutlo</h3>
                                            </div>-->
                                            <!-- /.box-body -->
                                            <div class="box-body">
                                                <div class="row row-bottom">
                                                    <div class="col-lg-3">
                                                        <label for="lblASN" class="control-label-top">ASN: </label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtASN" runat="server" CssClass="form-control marginoff" MaxLength="250" placeholder="ASN" autocomplete="off" Enabled="false" />
                                                            <span class="input-group-btn">
                                                                <input type="button" id="btnBuscarASN" class="btn custom-btn btn-default" value="..." />
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <label for="lblFechaASN" class="control-label-top">Fecha ASN: </label>
                                                        <asp:TextBox ID="txtFechaASN" runat="server" CssClass="form-control marginoff" MaxLength="250" placeholder="Fecha ASN" autocomplete="off" Enabled="false" />
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <label for="lblFactura" class="control-label-top">Factura: </label>
                                                        <asp:TextBox ID="txtFactura" runat="server" CssClass="form-control marginoff" MaxLength="250" placeholder="Factura" autocomplete="off" Enabled="false" />
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <label for="lblFechaFactura" class="control-label-top">Fecha Factura: </label>
                                                        <asp:TextBox ID="txtFechaFactura" runat="server" CssClass="form-control marginoff" MaxLength="250" placeholder="Fecha Factura" autocomplete="off" Enabled="false" />
                                                    </div>
                                                </div>
                                                <div class="row row-bottom">
                                                    <div class="col-lg-3">
                                                        <label for="lblProveedor" class="control-label-top">Proveedor: </label>
                                                        <asp:TextBox ID="txtProveedor" runat="server" CssClass="form-control marginoff" MaxLength="250" placeholder="Proveedor" autocomplete="off" Enabled="false" />
                                                    </div>
                                                    <div class="col-lg-3">
                                                        <label for="lblEstado" class="control-label-top">Estado: </label>
                                                        <asp:TextBox ID="txtEstado" runat="server" CssClass="form-control marginoff" MaxLength="250" placeholder="Estado" autocomplete="off" Enabled="false" />
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <a id="btnExportar" class="btn custom-btn btn-link" style="margin-top: 19px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Exportar Excel"><i class="fa fa-file-excel-o" style="font-size: 20px; margin-right: 6px;"></i>Exportar Excel</a>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <a id="btnLimpiar" class="btn custom-btn btn-link" style="margin-top: 19px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Limpiar Filtros"><i class="fa fa-eraser" style="font-size: 20px; margin-right: 6px;"></i>Limpiar Filtros</a>
                                                    </div>
                                                    <div class="col-lg-2">
                                                        <div class="btn-group" role="group" style="margin-top: 19px; width: 100%;">
                                                            <button type="button" class="btn custom-btn btn-link dropdown-toggle" style="color: #103d5f !important; width: 100%;" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                                Acciones
                                                            <span class="caret" style="margin-left: 6px;"></span>
                                                            </button>
                                                            <ul class="dropdown-menu">
                                                                <li><a href="#" id="btnRecepcionar"><i class="fa fa-dropbox" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i>Recepcionar</a></li>
                                                                <li><a href="#modalEnvioLTEA" data-toggle="modal"><i class="fa fa-cubes" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i>Lotear Transferencia Entre Almacenes</a></li>
                                                                <li><a href="#modalEnvioLEC" data-toggle="modal"><i class="fa fa-cubes" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i>Lotear Entrega A Cliente</a></li>
																<li><a href="#" id="btn_EjecutarConcurrente" data-toggle="modal"><i class="fa fa-file-text-o" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i>Obtener Facturas PINSA</a></li>
                                                            </ul>
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
                </div>
                <div class="row">
                    <!-- /.box-content -->
                    <div class="box custom-box box-primary" style="margin-bottom: 8px;">
                        <!-- /.box-header -->
                        <div class="box-header with-border">
                            <h3 class="box-title">Avisos de Envíos Anticipados</h3>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                        <dx:ASPxGridViewExporter ID="GVExportar" GridViewID="GVASN" runat="server"></dx:ASPxGridViewExporter>
                                        <dx:ASPxGridView
                                            ID="GVASN"
                                            ClientInstanceName="GVASN"
                                            runat="server"
                                            AutoGenerateColumns="False"
                                            Theme="MetropolisBlue"
                                            Width="100%"
                                            KeyboardSupport="true"
                                            AccessKey="1">
                                            <ClientSideEvents
                                                Init="function(s, e) {
                                                    OnInit(s, e, '');
                                                    resizeHeightDevControl(window, window, '#GVASN', GVASN, 100, 0);
                                                    $(window).resize();
                                                }"
                                                BeginCallback="function(s, e) {}"
                                                EndCallback="function(s, e) {}"
                                                RowDblClick="function(s,e) {}" />
                                            <Columns>
                                                <dx:GridViewCommandColumn Caption=" " ButtonType="Image" ShowNewButtonInHeader="false" ShowSelectCheckbox="false" SelectAllCheckboxMode="None" ShowClearFilterButton="true" ShowEditButton="false" Width="50" VisibleIndex="0">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <NewButton Image-ToolTip="Nuevo" Image-IconID="actions_new_16x16gray" />
                                                    <EditButton Image-ToolTip="Editar" Image-IconID="edit_edit_16x16gray" />
                                                    <UpdateButton Image-ToolTip="Modificar" Image-IconID="save_save_16x16gray" />
                                                    <CancelButton Image-ToolTip="Cancelar" Image-IconID="actions_close_16x16gray" />
                                                    <ClearFilterButton Image-ToolTip="Limpiar Filtro" Image-IconID="actions_clear_16x16gray" />
                                                </dx:GridViewCommandColumn>
                                                <dx:GridViewDataSpinEditColumn FieldName="NoLinea" Caption="No.Linea" Width="100" VisibleIndex="1">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="true" />
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                </dx:GridViewDataSpinEditColumn>
                                                <dx:GridViewDataColumn FieldName="CodProducto" Caption="SKU" Width="100" VisibleIndex="2">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="true" />
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="NoProducto" Caption="Descripción" Width="300" VisibleIndex="3">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="true" />
                                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Cantidad" Caption="Cantidad" Width="100" VisibleIndex="4">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="true" />
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataSpinEditColumn FieldName="Lote" Caption="Lote" Width="100" VisibleIndex="5">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="true" />
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                    <PropertiesSpinEdit DisplayFormatString="c2"></PropertiesSpinEdit>
                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                </dx:GridViewDataSpinEditColumn>
                                                <dx:GridViewDataColumn FieldName="NoPallet" Caption="LPN" Width="100" VisibleIndex="6">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Bold="true" />
                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                            <Settings VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" ShowFilterRow="true" ShowFilterRowMenu="true" ShowHeaderFilterButton="true" />
                                            <SettingsBehavior AllowFocusedRow="true" />
                                            <SettingsPager AlwaysShowPager="True" PageSize="12" NumericButtonCount="10" />
                                        </dx:ASPxGridView>
                                        <div style="height: 40px;"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- Modal Envio LTEA -->
    <div class="modal fade" id="modalEnvioLTEA" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="modal-evento" aria-hidden="true" style="overflow-y: auto;">
        <div class="modal-dialog modal-sm" id="modalDialogLTEA" style="width: 100% !important;">
            <div class="modal-content">
                <div class="modal-header">
                    <%--<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>--%>
                    <h4 class="modal-title">Lotear Transferencia Entre Almacenes</h4>
                </div>
                <div class="modal-body" id="modalBodyLTEA">
                    <div class="row">
                        <div class="col-lg-3">
                            <label for="lblAlmacenOrigen" class="control-label-top">Almacen Origen:</label>
                            <asp:DropDownList ID="ddlAlmacenOrigen" runat="server" placeholder="Almacen Origen" CssClass="multiselect form-control marginoff" data-style="btn-select-default" Width="100%" />
                        </div>
                        <div class="col-lg-3">
                            <label for="lblAlmacenDestino" class="control-label-top">Almacen Destino:</label>
                            <asp:DropDownList ID="ddlAlmacenDestino" runat="server" placeholder="Almacen Destino" CssClass="multiselect form-control marginoff" data-style="btn-select-default" Width="100%" />
                        </div>
                        <div class="col-lg-3">
                            <label for="lblEnvio" class="control-label-top">Envio: </label>
                            <asp:TextBox ID="txtEnvioLTEA" runat="server" CssClass="form-control marginoff" MaxLength="250" placeholder="Envio" autocomplete="off" Enabled="false" />
                        </div>
                        <div class="col-lg-3">
                            <a id="btnBuscarEnvioLTEA" class="btn custom-btn btn-link" style="margin-top: 28px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Buscar Envios"><i class="fa fa-search" style="font-size: 20px; margin-right: 6px;"></i>Buscar Envios</a>
                        </div>
                    </div>
                    <div class="row" style="padding: 16px;">
                        <!-- /.box-content -->
                        <div class="box custom-box box-primary">
                            <!-- /.box-header -->
                            <div class="box-header with-border">
                                <h3 class="box-title">Validación ASN Contra Envio</h3>
                            </div>
                            <!-- /.box-body -->
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <dx:ASPxGridView
                                                ID="GVValidacionLTEA"
                                                ClientInstanceName="GVValidacionLTEA"
                                                runat="server"
                                                AutoGenerateColumns="False"
                                                Theme="MetropolisBlue"
                                                Width="100%"
                                                KeyboardSupport="true"
                                                AccessKey="1"
                                                OnHtmlRowPrepared="GVValidacionLTEA_HtmlRowPrepared">
                                                <ClientSideEvents
                                                    Init="function(s, e) {
                                                        OnInit(s, e, '');
                                                        resizeHeightDevControl(window, window, '#GVValidacionLTEA', GVValidacionLTEA, 200, 0);
                                                        $(window).resize();
                                                    }"
                                                    BeginCallback="function(s, e) {}"
                                                    EndCallback="function(s, e) {}"
                                                    RowDblClick="function(s,e) {}" />
                                                <Columns>
                                                    <dx:GridViewCommandColumn Caption=" " ButtonType="Image" ShowNewButtonInHeader="false" ShowSelectCheckbox="false" SelectAllCheckboxMode="None" ShowClearFilterButton="true" ShowEditButton="false" Width="40" VisibleIndex="0">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <NewButton Image-ToolTip="Nuevo" Image-IconID="actions_new_16x16gray" />
                                                        <EditButton Image-ToolTip="Editar" Image-IconID="edit_edit_16x16gray" />
                                                        <UpdateButton Image-ToolTip="Modificar" Image-IconID="save_save_16x16gray" />
                                                        <CancelButton Image-ToolTip="Cancelar" Image-IconID="actions_close_16x16gray" />
                                                        <ClearFilterButton Image-ToolTip="Limpiar Filtro" Image-IconID="actions_clear_16x16gray" />
                                                    </dx:GridViewCommandColumn>
                                                    <dx:GridViewDataSpinEditColumn FieldName="ASNLineNumber" Caption="No. Linea" Width="100" VisibleIndex="1">
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <dx:GridViewBandColumn Caption="ASN">
                                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Center" BackColor="#203764" ForeColor="White" />
                                                        <Columns>
                                                            <dx:GridViewDataColumn FieldName="ASNItemNumber" Caption="SKU" Width="100" VisibleIndex="2">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="ASNItemDescription" Caption="Descripción" Width="300" VisibleIndex="3">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="ASNQuantity" Caption="Cantidad" Width="100" VisibleIndex="4">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                        </Columns>
                                                    </dx:GridViewBandColumn>
                                                    <dx:GridViewBandColumn Caption="ENVIO">
                                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Center" BackColor="#375623" ForeColor="White" />
                                                        <Columns>
                                                            <dx:GridViewDataColumn FieldName="ShipmentItemNumber" Caption="SKU" Width="100" VisibleIndex="5">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="ShipmentItemDescription" Caption="Descripción" Width="300" VisibleIndex="6">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="ShipmentQuantity" Caption="Cantidad" Width="100" VisibleIndex="7">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                        </Columns>
                                                    </dx:GridViewBandColumn>
                                                </Columns>
                                                <Settings VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" ShowFilterRow="true" ShowFilterRowMenu="true" ShowHeaderFilterButton="true" />
                                                <SettingsBehavior AllowFocusedRow="true" />
                                                <SettingsPager AlwaysShowPager="True" PageSize="12" NumericButtonCount="10" />
                                            </dx:ASPxGridView>
                                            <div style="height: 40px;"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnModalLotearLTEA" type="button" class="btn btn-default" onclick="">Lotear</button>
                    <button id="btnModalCancelarLTEA" type="button" class="btn btn-default">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal Envio LCE -->
    <div class="modal fade" id="modalEnvioLEC" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="modal-evento" aria-hidden="true" style="overflow-y: auto;">
        <div class="modal-dialog modal-sm" id="modalDialogLEC" style="width: 100% !important;">
            <div class="modal-content">
                <div class="modal-header">
                    <%--<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>--%>
                    <h4 class="modal-title">Lotear Entrega A Clientes</h4>
                </div>
                <div class="modal-body" id="modalBodyLEC">
                    <div class="row">
                        <div class="col-lg-3">
                            <label for="lblEnvio" class="control-label-top">Envio: </label>
                            <asp:TextBox ID="txtEnvioLEC" runat="server" CssClass="form-control marginoff" MaxLength="250" placeholder="Envio" autocomplete="off" Enabled="false" />
                        </div>
                        <div class="col-lg-3">
                            <a id="btnBuscarEnvioLEC" class="btn custom-btn btn-link" style="margin-top: 28px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Buscar Envios"><i class="fa fa-search" style="font-size: 20px; margin-right: 6px;"></i>Buscar Envios</a>
                        </div>
                    </div>
                    <div class="row" style="padding: 16px;">
                        <!-- /.box-content -->
                        <div class="box custom-box box-primary">
                            <!-- /.box-header -->
                            <div class="box-header with-border">
                                <h3 class="box-title">Validación ASN Contra Envio</h3>
                            </div>
                            <!-- /.box-body -->
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <dx:ASPxGridView
                                                ID="GVValidacionLEC"
                                                ClientInstanceName="GVValidacionLEC"
                                                runat="server"
                                                AutoGenerateColumns="False"
                                                Theme="MetropolisBlue"
                                                Width="100%"
                                                KeyboardSupport="true"
                                                AccessKey="1"
                                                OnHtmlRowPrepared="GVValidacionLEC_HtmlRowPrepared">
                                                <ClientSideEvents
                                                    Init="function(s, e) {
                                                        OnInit(s, e, '');
                                                        resizeHeightDevControl(window, window, '#GVValidacionLEC', GVValidacionLEC, 200, 0);
                                                        $(window).resize();
                                                    }"
                                                    BeginCallback="function(s, e) {}"
                                                    EndCallback="function(s, e) {}"
                                                    RowDblClick="function(s,e) {}" />
                                                <Columns>
                                                    <dx:GridViewCommandColumn Caption=" " ButtonType="Image" ShowNewButtonInHeader="false" ShowSelectCheckbox="false" SelectAllCheckboxMode="None" ShowClearFilterButton="true" ShowEditButton="false" Width="40" VisibleIndex="0">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <NewButton Image-ToolTip="Nuevo" Image-IconID="actions_new_16x16gray" />
                                                        <EditButton Image-ToolTip="Editar" Image-IconID="edit_edit_16x16gray" />
                                                        <UpdateButton Image-ToolTip="Modificar" Image-IconID="save_save_16x16gray" />
                                                        <CancelButton Image-ToolTip="Cancelar" Image-IconID="actions_close_16x16gray" />
                                                        <ClearFilterButton Image-ToolTip="Limpiar Filtro" Image-IconID="actions_clear_16x16gray" />
                                                    </dx:GridViewCommandColumn>
                                                    <dx:GridViewDataSpinEditColumn FieldName="ASNLineNumber" Caption="No. Linea" Width="100" VisibleIndex="1">
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <dx:GridViewBandColumn Caption="ASN">
                                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Center" BackColor="#203764" ForeColor="White" />
                                                        <Columns>
                                                            <dx:GridViewDataColumn FieldName="ASNItemNumber" Caption="SKU" Width="100" VisibleIndex="2">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="ASNItemDescription" Caption="Descripción" Width="300" VisibleIndex="3">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="ASNQuantity" Caption="Cantidad" Width="100" VisibleIndex="4">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                        </Columns>
                                                    </dx:GridViewBandColumn>
                                                    <dx:GridViewBandColumn Caption="ENVIO">
                                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Center" BackColor="#375623" ForeColor="White" />
                                                        <Columns>
                                                            <dx:GridViewDataColumn FieldName="ShipmentItemNumber" Caption="SKU" Width="100" VisibleIndex="5">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="ShipmentItemDescription" Caption="Descripción" Width="300" VisibleIndex="6">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="ShipmentQuantity" Caption="Cantidad" Width="100" VisibleIndex="7">
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Wrap="True" BackColor="#FAFAFA" />
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                            </dx:GridViewDataColumn>
                                                        </Columns>
                                                    </dx:GridViewBandColumn>
                                                </Columns>
                                                <Settings VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" ShowFilterRow="true" ShowFilterRowMenu="true" ShowHeaderFilterButton="true" />
                                                <SettingsBehavior AllowFocusedRow="true" />
                                                <SettingsPager AlwaysShowPager="True" PageSize="12" NumericButtonCount="10" />
                                            </dx:ASPxGridView>
                                            <div style="height: 40px;"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnModalLotearLEC" type="button" class="btn btn-default" onclick="">Lotear</button>
                    <button id="btnModalCancelarLEC" type="button" class="btn btn-default">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal Confirmar Lotear Transferencia entre Almacenes -->
    <div class="modal fade" id="modalConfirmarLTEA" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="modal-evento" aria-hidden="true" style="overflow-y: auto;">
        <div class="modal-dialog modal-sm" style="width: 50%; margin-top: 130px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalGrabarLabel">Guardar</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <p id="modal-evento-mensaje" style="margin: 0; font-family: 'Quantico', sans-serif; font-weight: bold; font-size: 18px;">
                                Existen SKU's en el Envio con cantidad pendiente por lotear,
                                si acepta enviar a lotear SKU's con cantidades incompletas/parciales,
                                se cerraran las lineas en el Envio y ya no podra completarla con otro ASN.
                                ¿Desea continuar?
                            </p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnConfirmarLTEAAceptar" type="button" class="btn btn-default">Aceptar</button>
                    <button id="btnConfirmarLTEACancelar" type="button" class="btn btn-default">Cancelar</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var attrEvents = function attrEvents() {
            if ($("#hfEmpresaCloudException").val()) {
                alert($("#hfEmpresaCloudException").val());
                location.href = "../../Inicio.aspx";
            }

            $(".navbar-btn.sidebar-toggle").click();

            $("#btnBuscarASN").click(function () {
                showLoading(true);
                $.ajax({
                    url: "RecepcionarASN.aspx/EstablecerModalASN",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    timeout: 600000,
                    success: function () {
                        showLoading(false);
                        showModal("../../Controles/BusquedaGenerica/Busqueda.aspx", "ASN's (Avisos de Envíos Anticipados)", "100%", "100%");
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#btnLimpiar").click(function () {
                var asn = { IdASN: 0, NoASN: "", FechaASN: "", NoFactura: "", FechaFactura: "", NombreProveedor: "", EstadoDocumento: "" };
                establecerASN(asn);
            });

            $("#btnExportar").click(function () {
                if (!GVASN.pageRowCount) {
                    showMessage(messageType.INFORMATIONBOX, "No existen registros para exportar a Excel.");
                    return;
                } else {
                    $('#btnExportarInterno').click();
                }
            });

            $("#btnRecepcionar").click(function () {
                if (!GVASN.pageRowCount) {
                    showMessage(messageType.WARNINGBOX, "Debe al menos seleccionar un ASN con lineas.");
                    return;
                } else if ($("#txtEstado").val() !== "INGRESADO") {
                    showMessage(messageType.WARNINGBOX, "Solo puede recepcionar ASN's con estado INGRESADO.");
                    return;
                }

                showLoading(true);
                $.ajax({
                    url: "RecepcionarASN.aspx/ActualizarASNEstadoDocumento",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ noASN: '" + $("#txtASN").val() + "', idEstadoDocumento: 2 }",
                    timeout: 600000,
                    success: function () {
                        showLoading(false);
                        showMessage(messageType.SUCCESSBOX, "Espere notificación de confirmación de recepción del ASN via correo.");
                        $("#btnLimpiar").click();
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#modalEnvioLTEA").on('show.bs.modal', function (e) {
                if (!GVASN.pageRowCount) {
                    showMessage(messageType.WARNINGBOX, "Debe al menos seleccionar un ASN con lineas.");
                    return e.preventDefault();
                } else if ($("#txtEstado").val() !== "RECEPCIONADO Y FACTURADO") {
                    showMessage(messageType.WARNINGBOX, "Solo puede lotear ASN's con estado RECEPCIONADO Y FACTURADO.");
                    return e.preventDefault();
                }
            });

            $("#modalEnvioLTEA").on('shown.bs.modal', function (e) {
                var offset = $("#modalDialogLTEA").offset();
                var mainPlaceContentHeight = $(window).height() - Math.abs(offset.top) - 170;
                $("#modalBodyLTEA").css("height", mainPlaceContentHeight);
                $(window).resize();
            });

            $("#btnBuscarEnvioLTEA").click(function () {
                if ($("#ddlAlmacenOrigen").val() === "0") {
                    showMessage(messageType.WARNINGBOX, "Debe al menos seleccionar un almacen origen.");
                    return;
                } else if ($("#ddlAlmacenDestino").val() === "0") {
                    showMessage(messageType.WARNINGBOX, "Debe al menos seleccionar un almacen destino.");
                    return;
                }

                showLoading(true);
                $.ajax({
                    url: "RecepcionarASN.aspx/EstablecerModalEnvioLTEA",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ almacenOrigen: '" + $("#ddlAlmacenOrigen").val() + "', almacenDestino: '" + $("#ddlAlmacenDestino").val() + "' }",
                    timeout: 600000,
                    success: function () {
                        showLoading(false);
                        showModal("../../Controles/BusquedaGenerica/Busqueda.aspx", "Envios", "100%", "100%");
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#btnModalLotearLTEA").click(function () {
                if (!GVValidacionLTEA.pageRowCount) {
                    showMessage(messageType.WARNINGBOX, "Debe al menos seleccionar y validar un envio.");
                    return;
                }

                showLoading(true);
                $.ajax({
                    url: "RecepcionarASN.aspx/ExistenLineasIncompletasLTEA",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    timeout: 600000,
                    success: function (data) {
                        showLoading(false);
                        if (data.d) {
                            $("#modalConfirmarLTEA").modal("show");
                        } else {
                            lotearTransferenciaEntreAlmacen(true);
                        }
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#btnConfirmarLTEAAceptar").click(function () {
                $("#modalConfirmarLTEA").modal("hide");
                lotearTransferenciaEntreAlmacen(true);
            });

            $("#btnConfirmarLTEACancelar").click(function () {
                $("#modalConfirmarLTEA").modal("hide");
                lotearTransferenciaEntreAlmacen(false);
            });

            $("#btnModalCancelarLTEA").click(function () {
                $("#ddlAlmacenOrigen").val("0").multiselect("refresh");
                $("#ddlAlmacenDestino").val("0").multiselect("refresh");
                $("#txtEnvioLTEA").val("");
                if (GVValidacionLTEA.pageRowCount) {
                    var envio = { SalesOrderNumber: "" };
                    establecerGVValidacionLTEA(envio);
                }
                $("#modalEnvioLTEA").modal("hide");
            });

            $("#modalEnvioLEC").on("show.bs.modal", function (e) {
                if (!GVASN.pageRowCount) {
                    showMessage(messageType.WARNINGBOX, "Debe al menos seleccionar un ASN con lineas.");
                    return e.preventDefault();
                } else if ($("#txtEstado").val() !== "RECEPCIONADO Y FACTURADO") {
                    showMessage(messageType.WARNINGBOX, "Solo puede lotear ASN's con estado RECEPCIONADO Y FACTURADO.");
                    return e.preventDefault();
                }
            });

            $("#modalEnvioLEC").on("shown.bs.modal", function (e) {
                var offset = $("#modalDialogLEC").offset();
                var mainPlaceContentHeight = $(window).height() - Math.abs(offset.top) - 170;
                $("#modalBodyLEC").css("height", mainPlaceContentHeight);
                $(window).resize();
            });

            $("#btnBuscarEnvioLEC").click(function () {
                showLoading(true);
                $.ajax({
                    url: "RecepcionarASN.aspx/EstablecerModalEnvioLEC",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ idEmpresaEBS12: " + parseFloat($("#hfIdEmpresaEBS12").val()) + " }",
                    timeout: 600000,
                    success: function () {
                        showLoading(false);
                        showModal("../../Controles/BusquedaGenerica/Busqueda.aspx", "Envios", "100%", "100%");
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#btnModalLotearLEC").click(function () {
                if (!GVValidacionLEC.pageRowCount) {
                    showMessage(messageType.WARNINGBOX, "Debe al menos seleccionar y validar un envio.");
                    return;
                }

                showLoading(true);
                $.ajax({
                    url: "RecepcionarASN.aspx/LotearEntregaACliente",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ noASN: '" + $("#txtASN").val() + "', noEnvio: '" + $("#txtEnvioLEC").val() + "' }",
                    timeout: 600000,
                    success: function () {
                        showLoading(false);
                        showMessage(messageType.SUCCESSBOX, "Espere notificación de confirmación del Loteo de Entrega a Cliente via correo.");
                        $("#modalEnvioLEC").modal("hide");
                        $("#btnLimpiar").click();
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#btnModalCancelarLEC").click(function () {
                $("#txtEnvioLEC").val("");
                if (GVValidacionLEC.pageRowCount) {
                    var envio = { DeliveryID: 0 };
                    establecerGVValidacionLEC(envio);
                }
                $("#modalEnvioLEC").modal("hide");
            });
			$("#btn_EjecutarConcurrente").click
			(
				function ()
				{
					$(window).MostrarPopup
					(
						{
							contenedor: null,
							url: 'popup/ObtenerFacturaPinsa.aspx',
							alto: '350px',
							ancho: '370px',
							tituloPopup: 'Obtener Facturas PINSA',
							masterPage: '~/Blank.Master&',
							movible: false,
							desaparecer: false
						}
					);
				}
			);
        };

        var messageType = {
            SUCCESSBOX: SUCCESSBOX,
            WARNINGBOX: WARNINGBOX,
            ERRORBOX: ERRORBOX,
            INFORMATIONBOX: INFORMATIONBOX
        };

        function establecerASN(asn) {
            $("#txtASN").val(asn.NoASN);
            $("#txtFechaASN").val(asn.FechaASN);
            $("#txtFactura").val(asn.NoFactura);
            $("#txtFechaFactura").val(asn.FechaFactura);
            $("#txtProveedor").val(asn.NombreProveedor);
            $("#txtEstado").val(asn.EstadoDocumento);
            establecerGVASN(asn.IdASN);
        }

        function establecerGVASN(idASN) {
            showLoading(true);
            $.ajax({
                url: "RecepcionarASN.aspx/EstablecerGVASN",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ idASN: " + idASN + " }",
                timeout: 600000,
                success: function () {
                    showLoading(false);
                    GVASN.Refresh();
                },
                error: function (request, status, error) {
                    showLoading(false);
                    showMessage(messageType.ERRORBOX, request.responseText);
                }
            });
        }

        function establecerGVValidacionLEC(envio) {
            $("#txtEnvioLEC").val(envio.DeliveryID || "");
            showLoading(true);
            $.ajax({
                url: "RecepcionarASN.aspx/EstablecerGVValidacionLEC",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ noEnvio: " + envio.DeliveryID + " }",
                timeout: 600000,
                success: function () {
                    showLoading(false);
                    GVValidacionLEC.Refresh();
                },
                error: function (request, status, error) {
                    showLoading(false);
                    showMessage(messageType.ERRORBOX, request.responseText);
                }
            });
        }

        function establecerGVValidacionLTEA(envio) {
            $("#txtEnvioLTEA").val(envio.SalesOrderNumber);
            showLoading(true);
            $.ajax({
                url: "RecepcionarASN.aspx/EstablecerGVValidacionLTEA",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ noASN: '" + $("#txtASN").val() + "', noEnvio: '" + envio.SalesOrderNumber + "' }",
                timeout: 600000,
                success: function () {
                    showLoading(false);
                    GVValidacionLTEA.Refresh();
                },
                error: function (request, status, error) {
                    showLoading(false);
                    showMessage(messageType.ERRORBOX, request.responseText);
                }
            });
        }

        function formattedDate(date) {
            var d = new Date(date || Date.now()),
                month = "" + (d.getMonth() + 1),
                day = "" + d.getDate(),
                year = d.getFullYear();
            if (month.length < 2) month = "0" + month;
            if (day.length < 2) day = "0" + day;
            return [year, month, day].join("-");
        }

        function lotearTransferenciaEntreAlmacen(esLotear) {
            showLoading(true);
            $.ajax({
                url: "RecepcionarASN.aspx/LotearTransferenciaEntreAlmacen",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ noASN: '" + $("#txtASN").val() + "', noEnvio: '" + $("#txtEnvioLTEA").val() + "', esLoteo: " + esLotear + ", almacenCloud: '" + $("#ddlAlmacenOrigen").val() + "' }",
                timeout: 600000,
                success: function (data) {
                    showLoading(false);
                    if (!data.d.Error) {
                        showMessage(messageType.SUCCESSBOX, data.d.Mensaje);
                        $("#btnModalCancelarLTEA").click();
                        $("#btnLimpiar").click();
                    } else {
                        showMessage(messageType.ERRORBOX, data.d.Mensaje);
                    }
                },
                error: function (request, status, error) {
                    showLoading(false);
                    showMessage(messageType.ERRORBOX, request.responseText);
                }
            });
        }

        function showLoading(display) {
            if (display) {
                showUpdateProgressLoading();
            } else {
                hideUpdateProgressLoading();
            }
        }

        function showGridViewLoading(display) {
            if (display) {
                GVASN.ShowLoadingElements()
            } else {
                GVASN.HideLoadingElements();
            }
        }

        function showMessage(messageType, sms, delay) {
            delay = delay || 7000;
            MostrarCajaMensajes(messageType, sms, delay);
        }

        function showModal(url, title, alto, ancho, masterPage) {
            $(window).MostrarPopup({
                contenedor: null,
                url: url,
                alto: alto,
                ancho: ancho,
                tituloPopup: title,
                masterPage: masterPage ? masterPage : "~/Blank.Master",
                movible: false,
                desaparecer: false
            });
        }
        function CerrarModalObtenerFacturaPinsa()
        {
        	$(window).CerrarPopup(this);
        }

        Sys.Application.add_load(Scripts_Load);
        Sys.Application.add_load(attrEvents);
    </script>
</asp:Content>
