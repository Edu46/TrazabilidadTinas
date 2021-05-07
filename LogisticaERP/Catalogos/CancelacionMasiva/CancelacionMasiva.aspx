<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="CancelacionMasiva.aspx.cs" Inherits="LogisticaERP.Catalogos.CancelacionMasiva.CancelacionMasiva" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

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

        Eventos = function () {
            $(document).ready(function () { });

            $('#btnCancelarPedidosVenta').on('click', function () {
                MostrarDialogoCodigosMotivos();
            });

            function MostrarDialogoCodigosMotivos() {
                try {
                    var filas = gridPedidosVenta.GetSelectedRowCount();
                    var idEmpresaEBS12 = document.getElementById('<%= hdIdEmpresaEBS12.ClientID%>').value;;

                    if (idEmpresaEBS12 <= 0) {
                        showMessage(WARNINGBOX, 'Empresa Oracle no especificada.');
                        return;
                    }

                    if (filas <= 0) {
                        showMessage(WARNINGBOX, 'No se han seleccionados pedidos de venta.');
                        return;
                    }

                    $(window).MostrarPopup({
                        contenedor: null,
                        url: 'Popup/BusquedaCodigosMotivos.aspx',
                        alto: '500px',
                        ancho: '500px',
                        tituloPopup: 'Seleccionar Código Motivo',
                        masterPage: '~/Blank.Master',
                        movible: false,
                        desaparecer: false
                    });
                }
                catch (error) {
                    console.error(error);
                    showMessage(WARNINGBOX, 'Ocurrió un problema no esperado, favor de comunicarse al Centro de Atención a Usuarios.');
                }
            }
        }

        function MostrarDatosCodigoMotivo(codigo) {
            try {
                $(window).CerrarPopup(this);
                if (codigo != '') {
                    var hdCodigoMotivo = document.getElementById('hdCodigoMotivo');
                    hdCodigoMotivo.value = codigo;

                    setTimeout(function () {
                        $("#btnCancelar").click();
                    }, 1000);
                }
            }
            catch (error) {
                console.error(error);
                showMessage(WARNINGBOX, 'Ocurrió un problema no esperado, favor de comunicarse al Centro de Atención a Usuarios.');
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript">
                Sys.Application.add_load(Scripts_Load);
                Sys.Application.add_load(Eventos);
            </script>
            <!-- Inicio Elementos ocultos -->
            <asp:HiddenField runat="server" ID="hdCodigoMotivo" Value="" />
            <asp:HiddenField runat="server" ID="hdIdEmpresaEBS12" Value="" />
            <!-- GridView que renderiza primero para corregir fallos de devexpress -->
            <div style="display: none;">
                <!-- Inicia apartado botones ocultos -->
                <asp:Button runat="server" ID="btnCancelar" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Cancelar" OnClick="btnCancelar_Click" />
                <dx:ASPxGridView ID="gridRenderFix" runat="server" ClientIDMode="Static">
                    <ClientSideEvents Init="function(s, e) { gridRenderFix.Refresh(); }" />
                </dx:ASPxGridView>
            </div>
            <!-- Panel de botones -->
            <div class="btn-container" runat="server">
                <asp:Button runat="server" ID="btnLimpiar" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Limpiar" OnClick="btnLimpiar_Click" />
                <asp:Button runat="server" ID="btnBuscar" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Buscar" OnClick="btnBuscar_Click" />
                <button id="btnCancelarPedidosVenta" style="margin-left: 5px;" class="btn custom-btn btn-primary pull-right">Cancelar </button>
            </div>
            <!-- Termina Panel de botones -->
            <!-- Principal -->
            <section class="custom-content" style="overflow-x: hidden;">
                <div class="bs-component">
                    <div class="clearfix">
                        <div class="row">
                            <div class="col-md-12 paddingoff">
                                <div class="box box-primary" style="margin-bottom: 8px;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title custom-box-title">Información de Pedidos de Venta</h3>
                                    </div>
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="col-sm-2">
                                                    <label for="dtpFechaInicio" class="control-label-top">Fecha Inicio:</label>
                                                    <dx:ASPxDateEdit ID="dtpFechaInicio"
                                                        ClientInstanceName="dtpFechaInicio"
                                                        runat="server"
                                                        autocomplete="off"
                                                        CssClass="form-control-date"
                                                        Theme="MetropolisBlue"
                                                        Date=""
                                                        UseMaskBehavior="true"
                                                        EditFormat="DateTime"
                                                        EditFormatString="dd/MM/yyyy"
                                                        DisplayFormatString="dd/MM/yyyy">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                                <div class="col-sm-2">
                                                    <label for="dtpFechaFin" class="control-label-top">Fecha Fin:</label>
                                                    <dx:ASPxDateEdit ID="dtpFechaFin"
                                                        ClientInstanceName="dtpFechaFin"
                                                        runat="server"
                                                        autocomplete="off"
                                                        CssClass="form-control-date"
                                                        Theme="MetropolisBlue"
                                                        UseMaskBehavior="true"
                                                        EditFormat="DateTime"
                                                        EditFormatString="dd/MM/yyyy"
                                                        DisplayFormatString="dd/MM/yyyy">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                                <div class="col-md-2">
                                                    <label for="txtOrdenCompra" class="control-label-top">Orden de Compra:</label>
                                                    <asp:TextBox ID="txtOrdenCompra"
                                                        runat="server"
                                                        CssClass="form-control marginoff"
                                                        placeholder="Orden de Compra"
                                                        aria-autocomplete="off">
                                                    </asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <label for="txtCliente" class="control-label-top">Cliente:</label>
                                                    <asp:TextBox ID="txtCliente"
                                                        runat="server"
                                                        CssClass="form-control marginoff"
                                                        placeholder="Cliente"
                                                        aria-autocomplete="off">
                                                    </asp:TextBox>
                                                </div>
                                                 <div class="col-md-3">
                                                    <label for="cmbAlmacen" class="control-label-top">Almacen:</label>
                                                    <asp:DropDownList ID="cmbAlmacen"
                                                        runat="server"
                                                        CssClass="multiselect form-control marginoff"
                                                        data-style="btn-select-default"
                                                        placeholder="Almacen"
                                                        validate="true"
                                                        AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 paddingoff">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary" style="margin-bottom: 8px;">
                                    <!-- /.box-header-->
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Pedidos de Venta</h3>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="table-responsive">
                                                    <dx:ASPxGridView
                                                        ID="gridPedidosVenta"
                                                        ClientInstanceName="gridPedidosVenta"
                                                        runat="server"
                                                        EnableTheming="true"
                                                        Theme="MetropolisBlue"
                                                        CssClass="withPagerInBoxHeader"
                                                        AutoGenerateColumns="false"
                                                        Width="100%"
                                                        KeyboardSupport="true"
                                                        AccessKey="1"
                                                        OnHtmlRowPrepared="gridPedidosVenta_HtmlRowPrepared">
                                                        <ClientSideEvents
                                                            Init="
															function(s, e)
															{
																OnInit(s, e,'OrdenCompra');
																resizeHeightDevControl(window, window, '#gridPedidosVenta', gridPedidosVenta, 70,0);
																$(window).resize();
															}"
                                                            BeginCallback="function(obj_Sender, e_Parametros) { }"
                                                            EndCallback="function(obj_Sender, e_Parametros) { }" />
                                                        <Columns>
                                                            <dx:GridViewCommandColumn Caption="" ButtonType="Image" ShowSelectCheckbox="true" ShowEditButton="true" SelectAllCheckboxMode="Page" ShowClearFilterButton="true" Width="40px" VisibleIndex="0">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ClearFilterButton Image-ToolTip="Limpiar Filtro" Image-IconID="actions_clear_16x16gray" />
                                                            </dx:GridViewCommandColumn>
                                                            <dx:GridViewDataTextColumn Caption="Orden de Compra" FieldName="OrdenCompra" Name="OrdenCompra" VisibleIndex="1" Width="120px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="No. Pedido Oracle" FieldName="NoPedidoOracle" Name="NoPedidoOracle" VisibleIndex="2" Width="120px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Almacén" FieldName="Almacen" Name="Almacen" VisibleIndex="3" Width="100px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Cliente Facturacion" FieldName="ClienteFacturacion" Name="ClienteFacturacion" VisibleIndex="4" Width="120px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Cliente Entrega" FieldName="ClienteEntrega" Name="ClienteEntrega" VisibleIndex="5" Width="120px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Cliente" FieldName="Cliente" Name="Cliente" VisibleIndex="6" Width="300px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Formato" FieldName="Formato" Name="Formato" VisibleIndex="7" Width="90px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataDateColumn Caption="Fecha OC" FieldName="FechaOC" Name="FechaOC" VisibleIndex="8" Width="90px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" EditFormat="Date" UseMaskBehavior="True" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Center" />
                                                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataDateColumn Caption="Fecha Cancelación" FieldName="FechaCancelacion" Name="FechaCancelacion" VisibleIndex="9" Width="120px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" EditFormat="Date" UseMaskBehavior="True" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Center" />
                                                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataTextColumn Caption="Linea" FieldName="Linea" Name="Linea" VisibleIndex="10" Width="90px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="SKU" FieldName="SKU" Name="SKU" VisibleIndex="11" Width="90px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Descripción" FieldName="Descripcion" Name="Descripcion" VisibleIndex="12" Width="300px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="UDM" FieldName="UDM" Name="UDM" VisibleIndex="13" Width="90px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Cantidad" FieldName="Cantidad" Name="Cantidad" VisibleIndex="14" Width="90px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Precio Lista" FieldName="PrecioLista" Name="PrecioLista" VisibleIndex="15" Width="90px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Precio Unitario" FieldName="PrecioUnitario" Name="PrecioUnitario" VisibleIndex="16" Width="120px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Total Linea" FieldName="TotalLinea" Name="TotalLinea" VisibleIndex="17" Width="100px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Moneda" FieldName="Moneda" Name="Moneda" VisibleIndex="18" Width="90px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Cod. Barras" FieldName="CodigoBarras" Name="CodigoBarras" VisibleIndex="19" Width="120px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Estatus" FieldName="Estatus" Name="Estatus" VisibleIndex="20" Width="130px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataDateColumn Caption="Fecha Creacion" FieldName="FechaCreacion" Name="FechaCreacion" VisibleIndex="21" Width="120px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" EditFormat="Date" UseMaskBehavior="True" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Center" />
                                                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataDateColumn Caption="Fecha Ultima Actualización" FieldName="FechaUltimaActualizacion" Name="FechaUltimaActualizacion" VisibleIndex="22" Width="180px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" EditFormat="Date" UseMaskBehavior="True" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="true" HorizontalAlign="Center" />
                                                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataTextColumn Caption="Resultado" FieldName="Resultado" Name="Resultado" VisibleIndex="23" Width="90px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Mensaje" FieldName="Mensaje" Name="Mensaje" VisibleIndex="24" Width="180px" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings VerticalScrollableHeight="225" VerticalScrollBarMode="Auto" ShowFilterRow="true" />
                                                        <Settings ShowFilterRow="true" />
                                                        <SettingsBehavior AllowFocusedRow="true" />
                                                        <SettingsEditing Mode="Inline" />
                                                        <SettingsPager AlwaysShowPager="true" PageSize="18" NumericButtonCount="5" />
                                                    </dx:ASPxGridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
