<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="Viajes.aspx.cs" Inherits="LogisticaERP.Catalogos.Viajes" %>

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
    <input id="hPage" type="hidden" value="Viajes" />
    <!--viaje de transferencia o entrega cliente
        En CalculoTarifasRepresentantes solo se pueden consultar viajes de tipo entrega a cliente
        2) Cuando un viaje sea de tipo entrega a cliente, se habilita el grid de entregas para asignación
        3) cuando un viaje sea de tipo transferencia se deshabilita el grid de entregas y el calculo no aplica para la interfaz
           de calculo de tarifas de representantes-->
    <input type="hidden" runat="server" id="hfEmpresaCloudException" value="" />
    <input type="hidden" runat="server" id="hfFecha" value="" />
    <input type="hidden" runat="server" id="hfAlmacenes" value="" />
    <input type="hidden" runat="server" id="hfIDViaje" value="" />
    <input type="hidden" runat="server" id="hfNumViaje" value="" />
    <input type="hidden" runat="server" id="hfCommand" value="" />
    <!-- Main content -->
    <section id="divCustomContent" class="custom-content" runat="server">
        <div class="bs-component">
            <div class="clearfix">
                <div class="row">
                    <!-- /Primera Tabla -->
                    <div class="col-lg-12" style="margin-top: 0px;">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Datos Generales</h3>
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
                                        <label for="lblFolio" class="control-label-top">Folio: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtFolio" placeholder="Folio" autocomplete="off" maxlength="18" disabled="disabled" />
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="lblTipoViaje" class="control-label-top">Tipo Viaje: </label>
                                        <asp:DropDownList runat="server" ID="ddlTipoViaje" ClientIDMode="Static" CssClass="multiselect multiselect-right form-control marginoff" data-style="btn-select-default" validate="true" placeholder="Tipo Viaje">
                                            <asp:ListItem Value="0">..:: Seleccione ::..</asp:ListItem>
                                            <asp:ListItem Value="TRANSFERENCIA">TRANSFERENCIA</asp:ListItem>
                                            <asp:ListItem Value="ENTREGA A CLIENTE">ENTREGA A CLIENTE</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-lg-4">
                                        <label for="lblNombre" class="control-label-top">Nombre: </label>
                                        <div class="input-group">
                                            <input runat="server" type="text" class="form-control marginoff" id="txtNombre" placeholder="Nombre" autocomplete="off" maxlength="250" validate="true" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                            <span class="input-group-btn">
                                                <input type="button" id="btnBuscar" class="btn custom-btn btn-default" value="..." />
                                            </span>
                                        </div>
                                    </div>
                                    <div class="col-lg-2">
                                        <label for="lblFecha" class="control-label-top">Fecha:</label>
                                        <dx:ASPxDateEdit runat="server" ID="deFecha" CssClass="form-control-date" Theme="MetropolisBlue" placeholder="Fecha" autocomplete="off" validate="true"></dx:ASPxDateEdit>
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="lblAlmacen" class="control-label-top">Almacen: </label>
                                        <asp:DropDownList runat="server" ID="ddlAlmacen" ClientIDMode="Static" CssClass="multiselect multiselect-right form-control marginoff" data-style="btn-select-default" validate="true" placeholder="Almacen" />
                                    </div>
                                    <div class="col-lg-1">
                                        <a id="btnGrabar" class="btn custom-btn btn-link" style="margin-top: 26px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Grabar"><i class="fa fa-floppy-o" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i></a>
                                    </div>
                                    <div class="col-lg-1">
                                        <a id="btnLimpiar" class="btn custom-btn btn-link" style="margin-top: 26px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Limpiar Forma"><i class="fa fa-eraser" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i></a>
                                    </div>
                                    <div class="col-lg-1">
                                        <a id="btnBorrar" class="btn custom-btn btn-link" style="margin-top: 26px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Eliminar"><i class="fa fa-trash" aria-hidden="true" style="font-size: 20px; margin-right: 6px;"></i></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <!-- /Segunda Tabla -->
                    <div class="col-lg-6" style="margin-top: 0px;">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Transporte</h3>
                                <div class="box-tools pull-right">
                                    <!-- Collapse Button -->
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse" style="padding: 1.5px 3px;">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-6">
                                        <label for="lblTransportista" class="control-label-top">Transportista: </label>
                                        <asp:DropDownList runat="server" ID="ddlTransportista" ClientIDMode="Static" CssClass="multiselect multiselect-right form-control marginoff" data-style="btn-select-default" placeholder="Transportista" />
                                    </div>
                                    <div class="col-lg-6">
                                        <label for="lblChofer" class="control-label-top">Chofer: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtChofer" placeholder="Chofer" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <label for="lblMarca" class="control-label-top">Marcar:</label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtMarca" placeholder="Marca" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                    <div class="col-lg-6">
                                        <label for="lblModelo" class="control-label-top">Modelo: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtModelo" placeholder="Modelo" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <label for="lblPlacaTractor" class="control-label-top">Placa Tractor: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtPlacaTractor" placeholder="Placa Tractor" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                    <div class="col-lg-6">
                                        <label for="lblPlacaCaja" class="control-label-top">Caja: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtCaja" placeholder="Caja" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <label for="lblNumeroSerieCaja" class="control-label-top">Núm. Serie Caja:</label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtNumeroSerieCaja" placeholder="Núm. Serie Caja" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                    <div class="col-lg-6">
                                        <label for="lblNumeroECONCaja" class="control-label-top">Núm. ECON Caja: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtNumeroECONCaja" placeholder="Núm. ECON Caja" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /Tercera Tabla -->
                    <div class="col-lg-6" style="margin-top: 0px;">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Documentación</h3>
                                <div class="box-tools pull-right">
                                    <!-- Collapse Button -->
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse" style="padding: 1.5px 3px;">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-6">
                                        <label for="lblNumeroRuta" class="control-label-top">Núm. Ruta: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtNumeroRuta" placeholder="Núm. Ruta" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                    <div class="col-lg-6">
                                        <label for="lblNumeroConvoy" class="control-label-top">Núm. Convoy: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtNumeroConvoy" placeholder="Núm. Convoy" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <label for="lblNumeroSello" class="control-label-top">Núm. Sello:</label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtNumeroSello" placeholder="Núm. Sello" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                    <div class="col-lg-6">
                                        <label for="lblNumeroCartePorte" class="control-label-top">Núm. Carta Porte: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtNumeroCartaPorte" placeholder="Núm. Carta Porte" autocomplete="off" maxlength="250" onkeypress="validarCaracteres('soloLetras_Numeros',event)" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <label for="lblEstado" class="control-label-top">Estado Viaje: </label>
                                        <asp:DropDownList runat="server" ID="ddlEstadoViaje" ClientIDMode="Static" CssClass="multiselect multiselect-right form-control marginoff" data-style="btn-select-default" validate="true" placeholder="Estado Viaje">
                                            <asp:ListItem Value="0">..:: Seleccione ::..</asp:ListItem>
                                            <asp:ListItem Value="1">INGRESADO</asp:ListItem>
                                             <asp:ListItem Value="2">OC GENERADA</asp:ListItem>
                                            <asp:ListItem Value="3">FINALIZADO</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-lg-6">
                                        <label for="lblInformacionCompleta" class="control-label-top"></label>
                                        <dx:ASPxCheckBox ID="chkInformacionCompleta" elemID="checkbox" runat="server" Checked="false" Theme="Mulberry" CheckBoxStyle-Cursor="pointer">
                                            <ClientSideEvents CheckedChanged="function(s, e) {
	                                                   ValidarCamposViaje(e,s);                                                   
                                                    }"></ClientSideEvents>
                                        </dx:ASPxCheckBox>
                                        <label for="lblInformacionCompleta">Información Completa</label>
                                    </div>
                                    <div class="col-lg-6" style="display: none">
                                        <label for="lblTodoRegistrado" class="control-label-top"></label>
                                        <dx:ASPxCheckBox ID="chkTodoRegistrado" elemID="checkbox" runat="server" Theme="Mulberry" CheckBoxStyle-Cursor="pointer" CheckState="Unchecked">
                                            <ClientSideEvents CheckedChanged="function(s, e) {
	                                                   ValidarCamposViaje(e,s);                                                   
                                                    }"></ClientSideEvents>
                                        </dx:ASPxCheckBox>
                                        <label for="lblTodoRegistrado">Todo Registrado</label>
                                    </div>


                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="DivAsignacionDeEntregas" style="display: none;">
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
                                            <dx:ASPxGridView
                                                ID="GVEntrega"
                                                ClientInstanceName="GVEntrega"
                                                runat="server"
                                                AutoGenerateColumns="False"
                                                Theme="MetropolisBlue"
                                                Width="100%"
                                                KeyboardSupport="true"
                                                AccessKey="1"
                                                OnRowDeleting="GVEntrega_RowDeleting"
                                                OnCustomButtonInitialize="GVEntrega_CustomButtonInitialize"
                                                OnCommandButtonInitialize="GVEntrega_CommandButtonInitialize">
                                                <ClientSideEvents
                                                    Init="function(s, e) {
                                                        OnInit(s, e, '');
                                                        //resizeHeightDevControl(window, window, '#GVEntrega', GVEntrega, -300, 0);
                                                        GVEntrega.SetHeight(400);
                                                    }"
                                                    BeginCallback="function(s, e) {
                                                        $('#hfCommand').val(e.command); 
                                                        console.log(e.command);
                                                    }"
                                                    EndCallback="function(s, e) {
                                                        if(s.cpException) {
                                                            showMessage(messageType.ERRORBOX, s.cpException);
                                                            s.cpException = undefined;
                                                        }

                                                        if ($('#hfCommand').val() === 'DELETEROW')
                                                            GVEntrega.Refresh();
                                                    }" />
                                                <Columns>
                                                    <dx:GridViewCommandColumn Caption=" " ButtonType="Image" ShowNewButtonInHeader="false" ShowSelectCheckbox="false" SelectAllCheckboxMode="None" ShowClearFilterButton="true" ShowEditButton="false" ShowDeleteButton="true" Width="50" VisibleIndex="0">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <NewButton Image-ToolTip="Nuevo" Image-IconID="actions_new_16x16gray" />
                                                        <EditButton Image-ToolTip="Editar" Image-IconID="edit_edit_16x16gray" />
                                                        <UpdateButton Image-ToolTip="Modificar" Image-IconID="save_save_16x16gray" />
                                                        <CancelButton Image-ToolTip="Cancelar" Image-IconID="actions_close_16x16gray" />
                                                        <DeleteButton Image-ToolTip="Eliminar" Image-IconID="actions_deleteitem_16x16gray" />
                                                        <ClearFilterButton Image-ToolTip="Limpiar Filtro" Image-IconID="actions_clear_16x16gray" />
                                                    </dx:GridViewCommandColumn>
                                                    <dx:GridViewDataSpinEditColumn FieldName="IDEntregaEBS12" Caption="Núm. Entrega" Width="100" VisibleIndex="1">
                                                        <HeaderCaptionTemplate>
                                                            <dx:ASPxLabel ID="btnBusquedaEntrega" runat="server" Text="Núm. Entrega" Font-Bold="true" />
                                                            <dx:ASPxImage ID="imgBusquedaEntrega" runat="server" EmptyImage-IconID="find_find_16x16gray">
                                                                <ClientSideEvents Click="function (s, e) { establecerModalEntregas(); }" />
                                                            </dx:ASPxImage>
                                                        </HeaderCaptionTemplate>
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
                                                    <dx:GridViewDataColumn FieldName="TransportistaEBS12" Caption="Transportista" Width="120" VisibleIndex="4">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="NumSucursalEBS12" Caption="Núm. Sucursal" Width="120" VisibleIndex="5">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="CiudadEBS12" Caption="Ciudad" Width="120" VisibleIndex="6">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="EstadoEBS12" Caption="Estado" Width="120" VisibleIndex="7">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="CodigoAlmacenEBS12" Caption="Cód. Almacen" Width="120" VisibleIndex="8">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="CodigoSubalmacenEBS12" Caption="Cód. Subalmacen" Width="120" VisibleIndex="9">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataSpinEditColumn FieldName="TotalCajasEBS12" Caption="Cajas" Width="120" VisibleIndex="10">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        <PropertiesSpinEdit DisplayFormatString="n4"></PropertiesSpinEdit>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <dx:GridViewDataSpinEditColumn FieldName="TotalCajasEstandarEBS12" Caption="Cajas Estandar" Width="120" VisibleIndex="11">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <HeaderStyle Font-Bold="true" />
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                        <PropertiesSpinEdit DisplayFormatString="n4"></PropertiesSpinEdit>
                                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                    </dx:GridViewDataSpinEditColumn>
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
        <!-- Modal Window Confirmation-->
        <div class="modal fade" id="modalEliminar" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="myModalConfirmationLabel" aria-hidden="true" style="overflow-y: auto;">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="myModalConfirmationLabel">Eliminar Viaje</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <p style="margin: 0; font-family: 'Quantico', sans-serif; font-weight: bold; font-size: 18px;">
                                    ¿Está seguro que desea eliminar el registro?
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <p style="margin: 0; font-family: 'Quantico', sans-serif; font-weight: bold; font-size: 16px; padding-top: 8px; float: left; color: white;">
                        </p>
                        <button id="btnAceptarBorrar" type="button" class="btn btn-default" data-dismiss="modal">Si</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                    </div>
                </div>
            </div>
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
            deFecha.SetDate(new Date($("#hfFecha").val()));
            GVEntrega.Refresh();

            $("[data-widget='collapse']").click(function() {
                //Find the box parent........
                var box = $(this).parents(".box").first();
                //Find the body and the footer
                var bf = box.find(".box-body, .box-footer");
                if (!$(this).children().hasClass("fa-plus")) {
                    $(this).children(".fa-minus").removeClass("fa-minus").addClass("fa-plus");
                    bf.slideUp();
                } else {
                    //Convert plus into minus
                    $(this).children(".fa-plus").removeClass("fa-plus").addClass("fa-minus");
                    bf.slideDown();
                }
            });

            $("#btnBuscar").click(function myfunction() {
                showLoading(true);
                $.ajax({
                    url: "Viajes.aspx/EstablecerModalViajes",
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

            $("#ddlAlmacen, #ddlTipoViaje").change(function () {
                showLoading(true);
                var idAlmcen = parseFloat($("#ddlAlmacen").val());
                mostrarGVEntregas(idAlmcen);
                $.ajax({
                    url: "Viajes.aspx/ActualizarEntregas",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ idAlmacen: " + idAlmcen + " }",
                    timeout: 600000,
                    success: function () {
                        showLoading(false);
                        GVEntrega.Refresh();
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });


            $("#ddlEstadoViaje").change(function () {
                
                if (parseInt($("#ddlEstadoViaje").val()) == 3){
                    $("#btnGrabar").removeClass("disabled");
                }
            });

            $("#btnGrabar").click(function () {
                if (validarForma()) {
                    clean_global_vars();

                    if (parseInt($("#ddlEstadoViaje").val()) == 2) {
                        showMessage(messageType.INFORMATIONBOX, "No se permite seleccionar Estado de Viaje OC GENERADA, favor de verificar.");
                        return;
                    }

                    showLoading(true);


                    var almacenes = JSON.parse($("#hfAlmacenes").val());
                    var almacen = almacenes.filter(a => a.IDAlmacen === parseFloat($("#ddlAlmacen").val()));

                    var viaje = new Object();
                    viaje.IDViaje = parseFloat($("#hfIDViaje").val()) || 0;
                    viaje.NoViaje = parseFloat($("#hfNumViaje").val()) || 0;
                    viaje.FolioViaje = $("#txtFolio").val();
                    viaje.TipoViaje = $("#ddlTipoViaje option:selected").text();
                    viaje.Descripcion = $("#txtNombre").val();
                    viaje.IDEmpresa = idEmpresaSeleccionada;
                    viaje.IDAlmacenOracle = parseFloat($("#ddlAlmacen").val());
                    viaje.IDProveedorOracle = parseFloat($("#ddlTransportista").val());
                    viaje.NombreAlmacenOracle = $("#ddlAlmacen option:selected").text();
                    viaje.TipoAlmacenOracle = almacen[0].TipoAlmacen;
                    viaje.ProveedorOracle = parseFloat($("#ddlTransportista").val()) ? $("#ddlTransportista option:selected").text() : "";
                    viaje.FechaEnvio = deFecha.GetValueString();
                    viaje.Chofer = $("#txtChofer").val();
                    viaje.Marca = $("#txtMarca").val();
                    viaje.Modelo = $("#txtModelo").val();
                    viaje.PlacaTractor = $("#txtPlacaTractor").val();
                    viaje.PlacaCaja = $("#txtCaja").val();
                    viaje.NumeroSerieCaja = $("#txtNumeroSerieCaja").val();
                    viaje.NumeroECOCaja = $("#txtNumeroECONCaja").val();
                    viaje.NumeroRuta = $("#txtNumeroRuta").val();
                    viaje.NumeroConvoy = $("#txtNumeroConvoy").val();
                    viaje.NumeroSello = $("#txtNumeroSello").val();
                    viaje.NumeroCartaPorte = $("#txtNumeroCartaPorte").val();
                    viaje.InformacionCompleta = chkInformacionCompleta.GetChecked();
                    viaje.TodoRegistrado = false; //chkTodoRegistrado.GetChecked();
                    viaje.IDEstado = parseFloat($("#ddlEstadoViaje").val());
                    viaje.Activo = true;
                    viaje.Borrado = false;
                    viaje.ViajeEntregas = null;

                    $.ajax({
                        url: "Viajes.aspx/Grabar",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{ viaje: " + JSON.stringify(viaje) + " }",
                        timeout: 600000,
                        success: function (data) {
                            showLoading(false);
                            if(data.d.Error) {
                                showMessage(messageType.WARNINGBOX, data.d.Mensaje);
                            } else {
                                showMessage(messageType.SUCCESSBOX, data.d.Mensaje);
                                establecerViaje(data.d.datos,true);
                                //$("#btnLimpiar").click();
                            }
                        },
                        error: function (error) {
                            showLoading(false);
                            var exception = JSON.parse(error.responseText);
                            showMessage(messageType.ERRORBOX, exception.Message);
                        }
                    });
                }
            });

            $("#btnLimpiar").click(function () {
                showLoading(true);
                //General
                $("#hfIDViaje").val("");
                $("#hfNumViaje").val("");
                $("#txtFolio").val("");
                $("#ddlTipoViaje").val("0").multiselect("refresh");
                $("#txtNombre").val("");
                deFecha.SetDate(new Date($("#hfFecha").val()));
                $("#ddlAlmacen").val("0").multiselect("refresh");
                mostrarGVEntregas(parseFloat(this.value));
                //Transporte
                $("#ddlTransportista").val("0").multiselect("refresh");
                $("#txtChofer").val("");
                $("#txtMarca").val("");
                $("#txtModelo").val("");
                $("#txtPlacaTractor").val("");
                $("#txtCaja").val("");
                $("#txtNumeroSerieCaja").val("");
                $("#txtNumeroECONCaja").val("");
                //Documentación
                $("#txtNumeroRuta").val("");
                $("#txtNumeroConvoy").val("");
                $("#txtNumeroSello").val("");
                $("#txtNumeroCartaPorte").val("");
                chkInformacionCompleta.SetChecked();
                chkTodoRegistrado.SetChecked();
                $("#ddlEstadoViaje").val("1").multiselect("refresh");
                $("#btnGrabar").removeClass("disabled");
                $("#btnBorrar").removeClass("disabled");
                $.ajax({
                    url: "Viajes.aspx/EstablecerGVEntrega",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ idViaje: -1, entregas: null, esArray: false }",
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


            $("#btnBorrar").click(function () {

                if (parseFloat($("#hfIDViaje").val()))
                {
                    $('#modalEliminar').modal('show');
                } else {
                    showMessage(messageType.INFORMATIONBOX, "Es necesario seleccionar un viaje.");
                    return;
                   
                }
               

                
            });


            $("#btnAceptarBorrar").click(function () {
                showLoading(true);
                $('#myModalConfirmation').modal('hide');
                var idViaje = parseFloat($("#hfIDViaje").val());
                $.ajax({
                    url: "Viajes.aspx/EliminarViaje",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ idViaje: " + idViaje + " }",
                    timeout: 600000,
                    success: function (data) {
                        showLoading(false);

                        if(data.d.Error) {
                            showMessage(messageType.WARNINGBOX, data.d.Mensaje);
                        } else {
                            showMessage(messageType.SUCCESSBOX, data.d.Mensaje);
                            $("#btnLimpiar").click();
                        }
                    },
                    error: function (error) {
                        showLoading(false);
                        var exception = JSON.parse(error.responseText);
                        showMessage(messageType.ERRORBOX, exception.Message);
                    }
                });
            });
        };

        var messageType = {
            SUCCESSBOX: SUCCESSBOX,
            WARNINGBOX: WARNINGBOX,
            ERRORBOX: ERRORBOX,
            INFORMATIONBOX: INFORMATIONBOX
        };

        function establecerGVEntrega(entregas) {
            showLoading(true);
            $.ajax({
                url: "Viajes.aspx/EstablecerGVEntrega",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ idViaje: 0, entregas: '" + JSON.stringify(entregas) + "', esArray: " + Array.isArray(entregas) + " }",
                timeout: 600000,
                success: function () {
                    showLoading(false);
                    GVEntrega.Refresh();
                },
                error: function (request, status, error) {
                    showLoading(false);
                    showMessage(messageType.ERRORBOX, request.responseText);
                }
            });
        }

        function establecerModalEntregas() {
            showLoading(true);
            $.ajax({
                url: "Viajes.aspx/EstablecerModalEntregas",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ idAlmacen: " + parseFloat($("#ddlAlmacen").val()) + " }",
                timeout: 600000,
                success: function () {
                    showLoading(false);
                    showModal('../../Controles/BusquedaGenerica/Busqueda.aspx', 'Entregas', '90%', '90%');
                },
                error: function (request, status, error) {
                    showLoading(false);
                    showMessage(messageType.ERRORBOX, request.responseText);
                }
            });
        }

        function ValidarCamposViaje(e,s)
        {

            if (s.GetChecked()){
                if ( $("#txtMarca").val() == null || $("#txtMarca").val() == '' ||
                     $("#txtModelo").val() == null ||  $("#txtModelo").val() == '' ||
                     $("#txtPlacaTractor").val() == null || $("#txtPlacaTractor").val() == '' ||
                     $("#txtCaja").val() == null || $("#txtCaja").val() == '' ||
                     $("#txtNumeroSerieCaja").val() == null || $("#txtNumeroSerieCaja").val() == '' ||
                     $("#txtNumeroECONCaja").val() == null || $("#txtNumeroECONCaja").val() == '' ||
                     $("#txtNumeroRuta").val() == null ||  $("#txtNumeroRuta").val() == '' ||
                     $("#txtNumeroConvoy").val() == null || $("#txtNumeroConvoy").val() == '' ||
                     $("#txtNumeroSello").val() == null || $("#txtNumeroSello").val() == '' ||
                     $("#txtNumeroCartaPorte").val() == null || $("#txtNumeroCartaPorte").val() == '' ||
                     parseFloat($("#ddlTransportista").val() <= 0) )
                {
                    showMessage(messageType.INFORMATIONBOX, 'Es necesario registrar toda la informacion del viaje. ');
                    s.SetChecked(false); 

                } 
            }
        }



        function establecerViaje(viaje, fechaFormato) {
            showLoading(true);
            //General
            $("#hfIDViaje").val(viaje.IDViaje);
            $("#hfNumViaje").val(viaje.NoViaje);
            $("#txtFolio").val(viaje.FolioViaje);
            $("#ddlTipoViaje").val(viaje.TipoViaje).multiselect("refresh");
            $("#txtNombre").val(viaje.Descripcion);
            fechaFormato = (typeof fechaFormato == "undefined") ? false : fechaFormato;

            if (fechaFormato){
                var fechaNet = new Date(parseInt(viaje.FechaEnvio.substr(6)));
                deFecha.SetDate(fechaNet);
            }else
                deFecha.SetDate(viaje.FechaEnvio);

            $("#ddlAlmacen").val(viaje.IDAlmacenOracle).multiselect("refresh");
            mostrarGVEntregas(viaje.IDAlmacenOracle);
            //Transporte
            $("#ddlTransportista").val(viaje.IDProveedorOracle).multiselect("refresh");
            $("#txtChofer").val(viaje.Chofer);
            $("#txtMarca").val(viaje.Marca);
            $("#txtModelo").val(viaje.Modelo);
            $("#txtPlacaTractor").val(viaje.PlacaTractor);
            $("#txtCaja").val(viaje.PlacaCaja);
            $("#txtNumeroSerieCaja").val(viaje.NumeroSerieCaja);
            $("#txtNumeroECONCaja").val(viaje.NumeroECOCaja);
            //Documentación
            $("#txtNumeroRuta").val(viaje.NumeroRuta);
            $("#txtNumeroConvoy").val(viaje.NumeroConvoy);
            $("#txtNumeroSello").val(viaje.NumeroSello);
            $("#txtNumeroCartaPorte").val(viaje.NumeroCartaPorte);
            $("#ddlEstadoViaje").val(viaje.IDEstado).multiselect("refresh");            
            chkInformacionCompleta.SetChecked(viaje.InformacionCompleta);
            chkTodoRegistrado.SetChecked(viaje.TodoRegistrado);
            $.ajax({
                url: "Viajes.aspx/EstablecerGVEntrega",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ idViaje: " + viaje.IDViaje + ", entregas: null, esArray: false }",
                timeout: 600000,
                success: function () {
                    showLoading(false);
                    GVEntrega.Refresh();
                },
                error: function (request, status, error) {
                    showLoading(false);
                    showMessage(messageType.ERRORBOX, request.responseText);
                }
            });

            if (parseInt(viaje.IDEstado) != 1){
                $("#btnGrabar").addClass("disabled");
                $("#btnBorrar").addClass("disabled");
            }else{
                $("#btnGrabar").removeClass("disabled");
                $("#btnBorrar").removeClass("disabled");
            }

        }

        function mostrarGVEntregas(idAlmacen) {
            //var almacenes = JSON.parse($("#hfAlmacenes").val());
            //var almacen = almacenes.filter(a => a.IDAlmacen === idAlmacen);
            if($("#ddlTipoViaje option:selected").text() === "ENTREGA A CLIENTE"/* &&
                almacen.length && 
                (almacen[0].TipoAlmacen === "REPRESENTANTE" || almacen[0].TipoAlmacen === "PORTEADOR")*/) {
                $("#DivAsignacionDeEntregas").css("display", "");
            } else {
                $("#DivAsignacionDeEntregas").css("display", "none");
            }
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

        Sys.Application.add_load(Scripts_Load);
        Sys.Application.add_load(attrEvents);
    </script>
</asp:Content>
