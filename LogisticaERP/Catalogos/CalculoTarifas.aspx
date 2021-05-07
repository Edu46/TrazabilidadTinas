<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="CalculoTarifas.aspx.cs" Inherits="LogisticaERP.Catalogos.CalculoTarifas" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function CargarScripts() {

            $("#btnBuscar").on('click', function () {

                $("#btnCargarInfo").click();

            });

        }

        function click() {
            console.log("prueba");

        }

        /*
          * SUCCESSBOX;
          * WARNINGBOX;
           * ERRORBOX;
          * INFORMATIONBOX;
   */
        MostrarMensaje = function (smsType, sms, displayTime) {
            displayTime = displayTime || 7000;
            MostrarCajaMensajes(smsType, [{ 'StrongText': sms }], displayTime);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" value="Calculo de Tarifas" id="hPage" />
    <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript">
                Sys.Application.add_load(Scripts_Load);
                Sys.Application.add_load(CargarScripts);
            </script>

            <!-- Estilos para los botónes en el Grid -->
            <!-- Inicio Elementos ocultos -->
            <div style="display: none;">
                <!-- Campos -->
                <asp:HiddenField runat="server" ID="hdfIdTarifa" Value="-1" />
                <asp:HiddenField runat="server" ID="hdfIdCalculoTarifa" Value="-1" />
                <asp:HiddenField runat="server" ID="hdfAlmacenDestino" Value="-1" />
                <asp:HiddenField runat="server" ID="hdfIdProveedorEbs12" Value="-1" />
                <asp:HiddenField runat="server" ID="hdfIdAlmacenOracle" Value="-1" />
                <!-- Campos -->
                <!-- Botones -->
                <!-- Botones -->
                <!-- GridView que renderiza primero para corregir fallos de devexpress -->
                <dx:ASPxGridView ID="gridRenderFix" runat="server" ClientIDMode="Static">
                    <ClientSideEvents Init="function(s, e) { gridRenderFix.Refresh(); }" />
                </dx:ASPxGridView>
                <!-- GridView que renderiza primero para corregir fallos de devexpress -->
            </div>
            <!-- Termina Elementos ocultos -->
            <!-- Panel de botones -->
            <div class="btn-container">
                <asp:Button runat="server" ID="btnLimpiar" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Limpiar" OnClick="btnLimpiar_Click" />
                <asp:Button runat="server" ID="btnOracle" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Generar Orden de Compra" OnClick="btnOracle_Click" />
                <asp:Button runat="server" ID="btnCalcular" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Calcular" OnClick="btnCalcular_Click" />

            </div>
            <!-- Termina Panel de botones -->
            <!-- Principal -->
            <section class="custom-content" style="overflow-x: hidden;">
                <div class="bs-component">
                    <div class="clearfix">
                        <div class="row">
                            <div class="col-sm-12 paddingoff">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary" style="margin-bottom: 8px;">
                                    <!-- /.box-header-->
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Información del Viaje</h3>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                            
                                                <div class="col-sm-2">
                                                    <label for="lblPlaneacion" class="control-label-top">No. Viaje</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtNumViaje" runat="server" CssClass="form-control marginoff" MaxLength="18" placeholder="Número Viaje" autocomplete="off" onkeypress="return validarCaracteres('soloLetrasNumeros',event)"></asp:TextBox>
                                                        <span class="input-group-btn">
                                                            <%--<div class="input-group-append">--%>
                                                            <button class="btn custom-btn btn-default" type="button" id="btnBuscar">
                                                                <i class="fa fa-search feedback"></i>
                                                            </button>
                                                            <%--</div>--%>
                                                        </span>
                                                    </div>
                                                </div>

                                                <div class="col-sm-8">
                                                    <label for="lblDescripcion" class="control-label-top">Descripción</label>
                                                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control marginoff" MaxLength="0" placeholder="" autocomplete="off" Enabled="false"></asp:TextBox>
                                                </div>
                                            
                                                <div class="col-sm-2">
                                                    <label for="lblAlmacen" class="control-label-top">Almacén Origen</label>
                                                    <asp:TextBox ID="txtAlmacen" runat="server" CssClass="form-control marginoff" MaxLength="0" placeholder="" autocomplete="off" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-5">
                                                    <label for="lblTransportista" class="control-label-top">Proveedor</label>
                                                    <asp:TextBox ID="txtTransportista" runat="server" CssClass="form-control marginoff" MaxLength="0" placeholder="" autocomplete="off" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-2">
                                                    <label for="lblFolioOC" class="control-label-top">Folio OC</label>
                                                    <asp:TextBox ID="txtFolioOC" runat="server" CssClass="form-control marginoff" MaxLength="0" placeholder="Folio OC" autocomplete="off" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-9 paddingoff">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary" style="margin-bottom: 8px;">
                                    <!-- /.box-header-->
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Detalles del Flete</h3>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="col-sm-4">
                                                    <label for="lblEstado" class="control-label-top">Estado</label>
                                                    <asp:DropDownList runat="server" CssClass="multiselect form-control marginoff" data-style="btn-select-default" ID="ddlEstado" placeholder="Estado" AppendDataBoundItems="true" validate="true" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">..:: Seleccionar ::..</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-4">
                                                    <label for="lblCiudad" class="control-label-top">Municipio</label>
                                                    <asp:DropDownList runat="server" CssClass="multiselect form-control marginoff" data-style="btn-select-default" ID="ddlMunicipio" placeholder="Municipio" AppendDataBoundItems="true" validate="true" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipio_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">..:: Seleccionar ::..</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-4">
                                                    <label for="lblCiudad" class="control-label-top">Ciudad</label>
                                                    <asp:DropDownList runat="server" CssClass="multiselect form-control marginoff" data-style="btn-select-default" ID="ddlCiudad" placeholder="Ciudad" AppendDataBoundItems="true" validate="true">
                                                        <asp:ListItem Value="0">..:: Seleccionar ::..</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-4">
                                                    <label for="lblTipoTarifa" class="control-label-top">Tipo Tarifa</label>
                                                    <asp:DropDownList runat="server" CssClass="multiselect form-control marginoff" data-style="btn-select-default" ID="ddlTipoTarifa" placeholder="Tipo Tarifa" AppendDataBoundItems="true" validate="true">
                                                        <asp:ListItem Value="0">..:: Seleccionar ::..</asp:ListItem>
                                                        <asp:ListItem Value="1">CLIENTE</asp:ListItem>
                                                        <asp:ListItem Value="2">REPRESENTANTE</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-4">
                                                    <label for="lblCiudad" class="control-label-top">Cod. Transporte</label>
                                                    <asp:DropDownList runat="server" CssClass="multiselect form-control marginoff" data-style="btn-select-default" ID="ddlCodTransporte" placeholder="Cod. Transporte" AppendDataBoundItems="true" validate="true">
                                                        <asp:ListItem Value="0">..:: Seleccionar ::..</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2 ">
                                                    <label for="lblBackhaul" class="control-label-top"></label>
                                                    <dx:ASPxCheckBox runat="server" elemID="checkbox" CheckBoxStyle-Cursor="pointer" ID="chkBackhaul" Checked="False" Theme="Mulberry"></dx:ASPxCheckBox>
                                                    <label for="lblBackhaul">Backhaul</label>
                                                </div>
                                                <div class="col-sm-2 ">
                                                    <label for="lblCasetas" class="control-label-top"></label>
                                                    <dx:ASPxCheckBox runat="server" elemID="checkbox" CheckBoxStyle-Cursor="pointer" ID="chkCasetas" Checked="False" Theme="Mulberry"></dx:ASPxCheckBox>
                                                    <label for="lblCasetas">Casetas</label>
                                                </div>

                                            </div>
                                        </div>


                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-3 ">
                                    <!-- /.box-content -->
                                    <div class="box custom-box box-primary" style="margin-bottom: 8px;">
                                        <!-- /.box-header-->
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Calculo Tarifas</h3>
                                        </div>
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-sm-10">
                                                    <label for="lblTransporte" class="control-label-top">Transporte</label>
                                                    <asp:TextBox ID="txtMontoTransporte" runat="server" CssClass="form-control marginoff" MaxLength="0" placeholder="" autocomplete="off" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-10">
                                                    <label for="lblMontoCasetas" class="control-label-top">Casetas</label>
                                                    <asp:TextBox ID="txtMontoCaseta" runat="server" CssClass="form-control marginoff" MaxLength="0" placeholder="" autocomplete="off" Enabled="false"></asp:TextBox>
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
                    <asp:Button runat="server" ID="btnCargarInfo" UseSubmitBehavior="false" OnClick="btnCargarInfo_Click" />
                </div>
            </section>
            <!-- Termina Principal -->
            <script type="text/javascript">
            </script>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlEstado" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlMunicipio" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnCargarInfo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCalcular" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnOracle" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
