<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="CalculoTarifasRepresentantes.aspx.cs" Inherits="LogisticaERP.Catalogos.CalculoTarifasRepresentantes" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .table-responsive {
            border: 0px !important;
        }

        .multiselect-right ~ div ul {
            right: inherit;
            left: inherit;
        }

        .box-header.with-border {
            border-bottom: 1px solid #f4f4f4;
            height: inherit;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" EnablePageMethods="true" runat="server" />
    <input id="hPage" type="hidden" value="Calculo de Pago a Representantes" />
    <input type="hidden" runat="server" id="hfEmpresaCloudException" value="" />
    <input type="hidden" runat="server" id="hfIDCalculoTarifaRepresentante" value="" />
    <input type="hidden" runat="server" id="hfTotalCajasEstandar" value="" />
    <input type="hidden" runat="server" id="hfIDOrdenCompra" value="" />
     <input type="hidden" runat="server" id="hfFechaViaje" value="" />
    <!-- Main content -->
    <section id="divCustomContent" class="custom-content" runat="server">
        <div class="bs-component">
            <div class="clearfix">
                <div class="row">
                    <!-- /Primera Tabla -->
                    <div class="col-lg-12" style="margin-top: 0px;">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Información del Viaje</h3>
                                <%--<div class="box-tools pull-right">
                                    <!-- Collapse Button -->
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse" style="padding:1.5px 3px;">
                                    <i class="fa fa-minus"></i>
                                    </button>
                                </div>--%>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-3">
                                        <label for="lblTipoViaje" class="control-label-top">Tipo Viaje: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtTipoViaje" placeholder="Tipo Viaje" autocomplete="off" maxlength="250" value="ENTREGA A CLIENTE" disabled="disabled" />
                                    </div>
                                    <div class="col-lg-2">
                                        <label for="lblNumViaje" class="control-label-top">Núm. Viaje: </label>
                                        <div class="input-group">
                                            <input runat="server" type="text" class="form-control marginoff" id="txtNumViaje" placeholder="Núm. Viaje" autocomplete="off" maxlength="250" disabled="disabled" />
                                            <span class="input-group-btn">
                                                <input type="button" id="btnBuscar" class="btn custom-btn btn-default" value="..." />
                                            </span>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="lblProveedor" class="control-label-top">Proveedor: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtProveedor" placeholder="Proveedor" autocomplete="off" maxlength="250" disabled="disabled" />
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="lblTotalTransporte" class="control-label-top">Total Transporte: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtTotalTransporte" placeholder="Total Transporte" autocomplete="off" maxlength="250" disabled="disabled" />
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="lblOrdenCompraCloud" class="control-label-top">Orden Compra Cloud: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtOrdenCompraCloud" placeholder="Orden Compra Cloud" autocomplete="off" maxlength="250" disabled="disabled" />
                                    </div>
                                    <div class="col-lg-1">
                                        <a id="btnGrabar" class="btn custom-btn btn-link" style="margin-top: 26px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Guardar"><i class="fa fa-floppy-o" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i></a>
                                    </div>
                                    <div class="col-lg-1">
                                        <a id="btnGenerarOC" class="btn custom-btn btn-link" style="margin-top: 26px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Generar OC Cloud"><i class="fa fa-file-text-o" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i></a>
                                    </div>
                                    <div class="col-lg-1">
                                        <a id="btnLimpiar" class="btn custom-btn btn-link" style="margin-top: 26px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Limpiar Forma"><i class="fa fa-eraser" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i></a>
                                    </div>
                                    <div class="col-lg-1">
                                        <a id="btnExportar" class="btn custom-btn btn-link" style="margin-top: 26px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Exportar"><i class="fa fa-file-excel-o" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <!-- /Cuarta Tabla -->
                    <div class="col-lg-12" style="margin-top: 0px;">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Entregas</h3>
                                <%--<div class="box-tools pull-right">
                                    <!-- Collapse Button -->
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse" style="padding:1.5px 3px;">
                                    <i class="fa fa-minus"></i>
                                    </button>
                                </div>--%>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <dx:ASPxGridViewExporter ID="GVExportar" GridViewID="GVEntrega" runat="server" ></dx:ASPxGridViewExporter>
                                            <dx:ASPxGridView
                                                ID="GVEntrega"
                                                ClientInstanceName="GVEntrega"
                                                runat="server"
                                                AutoGenerateColumns="False"
                                                Theme="MetropolisBlue"
                                                Width="100%"
                                                KeyboardSupport="true"
                                                AccessKey="1">
                                                <ClientSideEvents
                                                    Init="function(s, e) {
                                                        OnInit(s, e, '');
                                                        resizeHeightDevControl(window, window, '#GVEntrega', GVEntrega, 100, 0);
                                                    }"
                                                    EndCallback="function(s, e) {}" />
                                                <Columns>
                                                    <dx:GridViewCommandColumn Caption=" " ButtonType="Image" ShowNewButtonInHeader="false" ShowSelectCheckbox="false" SelectAllCheckboxMode="None" ShowClearFilterButton="true" ShowEditButton="false" ShowDeleteButton="false" Width="50" VisibleIndex="0">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <NewButton Image-ToolTip="Nuevo" Image-IconID="actions_new_16x16gray" />
                                                        <EditButton Image-ToolTip="Editar" Image-IconID="edit_edit_16x16gray" />
                                                        <UpdateButton Image-ToolTip="Modificar" Image-IconID="save_save_16x16gray" />
                                                        <CancelButton Image-ToolTip="Cancelar" Image-IconID="actions_close_16x16gray" />
                                                        <DeleteButton Image-ToolTip="Eliminar" Image-IconID="actions_deleteitem_16x16gray" />
                                                        <ClearFilterButton Image-ToolTip="Limpiar Filtro" Image-IconID="actions_clear_16x16gray" />
                                                    </dx:GridViewCommandColumn>
                                                    <dx:GridViewDataSpinEditColumn FieldName="IDEntregaEBS12" Caption="Núm. Entrega" Width="100" VisibleIndex="1">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <dx:GridViewDataDateColumn FieldName="FechaProgramadaEBS12" Caption="Fecha Programada" Width="120" VisibleIndex="2">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataDateColumn>
                                                    <dx:GridViewDataSpinEditColumn FieldName="NumPedidoVentaEBS12" Caption="Núm. Pedido Venta" Width="120" VisibleIndex="3">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <dx:GridViewDataColumn FieldName="NumSucursalEBS12" Caption="Núm. Sucursal" Width="120" VisibleIndex="4">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="CiudadEBS12" Caption="Ciudad" Width="120" VisibleIndex="5">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="EstadoEBS12" Caption="Estado" Width="120" VisibleIndex="6">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="CodigoAlmacenEBS12" Caption="Cód. Almacen" Width="120" VisibleIndex="7">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>
                                                    <%--<dx:GridViewDataColumn FieldName="CodigoSubalmacenEBS12" Caption="Cód. Subalmacen" Width="120" VisibleIndex="8">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>--%>
                                                    <dx:GridViewDataSpinEditColumn FieldName="CajasEBS12" Caption="Cajas Estandar" Width="120" VisibleIndex="9">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        <PropertiesSpinEdit DisplayFormatString="n4"></PropertiesSpinEdit>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <dx:GridViewDataSpinEditColumn FieldName="CostoCajaEBS12" Caption="Costo Caja" Width="120" VisibleIndex="10">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        <PropertiesSpinEdit DisplayFormatString="n4"></PropertiesSpinEdit>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <dx:GridViewDataSpinEditColumn FieldName="CostoTotalEBS12" Caption="Costo Total" Width="120" VisibleIndex="11">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        <PropertiesSpinEdit DisplayFormatString="n4"></PropertiesSpinEdit>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <dx:GridViewDataColumn FieldName="Excepcion" Caption="Excepción" Width="120" VisibleIndex="12">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Left"></CellStyle>
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
        </div>
            <div style="display: none">
            <asp:Button runat="server" ID="btnExportarInterno" UseSubmitBehavior="False" CssClass="btn custom-btn btn-primary btn-exporter" Text="Exportar a Excel" OnClick="btnExportarInterno_Click" />
        </div>
    </section>
    <script type="text/javascript">
        "use strict";

        var attrEvents = function () {
            if ($("#hfEmpresaCloudException").val()) {
                alert($("#hfEmpresaCloudException").val());
                location.href = "../../Inicio.aspx";
            }

            $(".navbar-btn.sidebar-toggle").click();
            GVEntrega.Refresh();

            $("#btnBuscar").click(function myfunction() {
                showLoading(true);
                $.ajax({
                    url: "CalculoTarifasRepresentantes.aspx/EstablecerModalViajes",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    timeout: 600000,
                    success: function () {
                        showLoading(false);
                        showModal("../../Controles/BusquedaGenerica/Busqueda.aspx", "Viajes", "100%", "100%");
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#btnGenerarOC").click(function () {
                if (parseFloat($("#hfIDOrdenCompra").val())) {
                    showMessage(messageType.INFORMATIONBOX, "El viaje ya cuenta con una Orden de Compra en Oracle Cloud.");
                    return;
                } else if (!GVEntrega.pageRowCount) {
                    showMessage(messageType.WARNINGBOX, "Debe al menos seleccionar un viaje con entregas asignadas.");
                    return;
                }
                showLoading(true);
                $.ajax({
                    url: "CalculoTarifasRepresentantes.aspx/CrearOrdenCompraCloud",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ idCalculoTarifaRepresentante: " + (parseFloat($("#hfIDCalculoTarifaRepresentante").val()) || 0) +
                        ", numViaje: '" + $("#txtNumViaje").val() +
                        "', fechaViaje: '" + $("#hfFechaViaje").val() +
                        "', proveedor: '" + $("#txtProveedor").val() +
                        "', totalTransporte: '" + $("#txtTotalTransporte").val() +
                        "', totalCajasEstandar: '" + $("#hfTotalCajasEstandar").val() + "' }",
                    timeout: 600000,
                    success: function (data) {
                        showLoading(false);
                        if (!data.d.Error) {
                            showMessage(messageType.SUCCESSBOX, data.d.Mensaje);
                            $("#btnLimpiar").click();
                        } else {
                            showMessage(messageType.ERRORBOX, data.d.Mensaje);
                            if (data.d.Nuevo)
                                $("#hfIDCalculoTarifaRepresentante").val(data.d.IDCalculo);
                        }
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#btnLimpiar").click(function () {
                showLoading(true);
                $("#txtNumViaje").val("");
                $("#txtProveedor").val("");
                $("#txtTotalTransporte").val("");
                $("#hfIDCalculoTarifaRepresentante").val("");
                $("#hfTotalCajasEstandar").val("");
                $("#hfIDOrdenCompra").val("");
                $("#txtOrdenCompraCloud").val("");
                $("#btnGenerarOC").attr("disabled", false);
                $.ajax({
                    url: "CalculoTarifasRepresentantes.aspx/LimpiarGVEntrega",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    timeout: 600000,
                    success: function () {
                        showLoading(false);
                        GVEntrega.Refresh();
                    },
                    error: function (error) {
                        showLoading(false);
                        var exception = JSON.parse(error.responseText);
                        showMessage(messageType.ERRORBOX, exception.Message);
                    }
                });
            });
        };


        $("#btnGrabar").click(function () {
            //Validar que existan datos de entregas.

            if (!GVEntrega.pageRowCount) {
                showMessage(messageType.INFORMATIONBOX, "No existen entregas relacionadas al Viaje.");
                return;
            } else {

                showLoading(true);

                $.ajax({
                    url: "CalculoTarifasRepresentantes.aspx/GuardarCalculo",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ idCalculoTarifaRepresentante: " + (parseFloat($("#hfIDCalculoTarifaRepresentante").val()) || 0) +
                        ", numViaje: '" + $("#txtNumViaje").val() +
                        "', proveedor: '" + $("#txtProveedor").val() +
                        "', totalTransporte: '" + $("#txtTotalTransporte").val() +
                        "', totalCajasEstandar: '" + $("#hfTotalCajasEstandar").val() + "' }",
                    timeout: 600000,
                    success: function (data) {
                        showLoading(false);
                        if (!data.d.Error) {
                            showMessage(messageType.SUCCESSBOX, data.d.Mensaje);
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
        });

        $("#btnExportar").click(function () {
            if (!GVEntrega.pageRowCount) {
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

        function establecerViaje(viaje) {
            showLoading(true);
            $("#txtNumViaje").val(viaje.FolioViaje);
            $("#txtProveedor").val(viaje.ProveedorOracle);
            $("#hfFechaViaje").val(new Date(viaje.FechaEnvio).yyyymmdd());
            $.ajax({
                url: "CalculoTarifasRepresentantes.aspx/EstablecerGVEntrega",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ idEmpresa: " + viaje.IDEmpresa + ", numViaje: '" + viaje.FolioViaje + "' }",
                timeout: 600000,
                success: function (data) {
                    showLoading(false);
                    $("#txtTotalTransporte").val(data.d.TotalTransporte);
                    $("#hfIDCalculoTarifaRepresentante").val(data.d.IDCalculoTarifaRepresentante);
                    $("#hfTotalCajasEstandar").val(data.d.TotalCajasEstandar);
                    $("#hfIDOrdenCompra").val(data.d.IDOrdenCompra);
                    $("#txtOrdenCompraCloud").val(data.d.FolioOrdenCompra);
                    GVEntrega.Refresh();
                    $("#btnGenerarOC").attr("disabled", data.d.IDOrdenCompra > 0);
                },
                error: function (request, status, error) {
                    showLoading(false);
                    showMessage(messageType.ERRORBOX, request.responseText);
                }
            });
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
                desaparecer: false,
                FuncionCallback: null
            });
        }

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

        Date.prototype.yyyymmdd = function () {
            var mm = this.getMonth() + 1; // getMonth() is zero-based
            var dd = this.getDate();

            return [this.getFullYear(),
                    (mm > 9 ? '' : '0') + mm,
                    (dd > 9 ? '' : '0') + dd
            ].join('/');
        };

        Sys.Application.add_load(Scripts_Load);
        Sys.Application.add_load(attrEvents);
    </script>
</asp:Content>
