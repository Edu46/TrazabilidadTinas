<%@ Page Title="" Language="C#" MasterPageFile="~/Blank.Master" AutoEventWireup="true" CodeBehind="Busqueda.aspx.cs" Inherits="LogisticaERP.Controles.BusquedaGenerica.Busqueda" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Hidden field -->
            <input runat="server" type="hidden" id="hfFunctionCallback" value="" />
            <!-- GridView que renderiza primero para corregir fallos de devexpress -->
            <div style="display: none;">
                <dx:ASPxGridView ID="gridRenderFix" runat="server" ClientSideEvents-Init="function (s, e) { gridRenderFix.Refresh(); }" />
            </div>
            <!-- Panel de controles -->
            <div id="panelDeControles" runat="server" class="btn-container-popup" style="display:none">
                <input runat="server" type="button" id="btnAceptar" style="margin-left: 5px;margin-right: 26px;" class="btn custom-btn btn-primary pull-right" value="Aceptar" />
            </div>
            <!-- Main content -->
            <section id="mainContent" class="custom-content" runat="server">
                <div class="bs-component">
                    <div class="clearfix">
                        <div class="row">
                            <div class="col-sm-12">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary" style="margin-bottom: 8px;">
                                    <!-- /.box-header -->
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="table-responsive">
                                                    <dx:ASPxGridView 
                                                        runat="server" 
                                                        ID="GridViewBG" 
                                                        ClientInstanceName="GridViewBG" 
                                                        CssClass="withPagerInBoxHeader"
                                                        AutoGenerateColumns="False" 
                                                        KeyboardSupport="true" 
                                                        AccessKey="1"
                                                        EnableTheming="True" 
                                                        Theme="MetropolisBlue" 
                                                        Width="100%"
                                                        OnCustomCallback="GridViewBG_CustomCallback">
                                                        <ClientSideEvents 
                                                            Init="function(s, e){
                                                                resizeHeightDevControl(window, window, '#GridViewBG', GridViewBG, 60, 0);
                                                                $(window).resize();
                                                            }"
                                                            EndCallback="function(s, e) { setData(s); }"
                                                            RowDblClick="function(s, e) { getData(s, e); }"/>
                                                        <Settings VerticalScrollableHeight="225" VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" ShowFilterRow="true" ShowFilterRowMenu="true" ShowHeaderFilterButton="true" />
                                                        <SettingsBehavior AllowSelectByRowClick="true" />
                                                        <Styles>
                                                            <Header Wrap="True"></Header>
                                                        </Styles>
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
    </asp:UpdatePanel>
    <script type="text/javascript">
        "use strict";

        var _functionName = "";

        var _messageType = {
            SUCCESSBOX: SUCCESSBOX,
            WARNINGBOX: WARNINGBOX,
            ERRORBOX: ERRORBOX,
            INFORMATIONBOX: INFORMATIONBOX
        };

        var attrEvents = function () {
            $("#btnAceptar").click(obtenerFilasSeleccionadas);
        };

        $(document).ready(function () {
            _functionName = $("#hfFunctionCallback").val();
        });

        function obtenerFilasSeleccionadas() {
            GridViewBG.PerformCallback("OBTENER_FILAS_SELECCIONADAS");
        }

        function getData(s, e) {
            if (!_functionName || e.htmlEvent.target.tagName !== "TD") return;
            var key = s.GetRowKey(s.GetFocusedRowIndex());
            s.PerformCallback(key);
        }

        function setData(s) {
            if (!s.cpRow) return;
            if (s.cpRow.Movimiento === "OBTENER_FILAS_SELECCIONADAS" && (!s.cpRow.Datos || !s.cpRow.Datos.length)) {
                showMessage(_messageType.WARNINGBOX, "Favor de seleccionar uno o mas registros!");
                return;
            }

            parent.window[_functionName](s.cpRow.Datos);
            parent.$(window).CerrarPopup(this);
            s.cpRow = undefined;
        }

        function showMessage(messageType, sms, delay) {
            delay = delay || 7000;
            MostrarCajaMensajes(messageType, sms, delay);
        }

        Sys.Application.add_load(Scripts_Load);
        Sys.Application.add_load(attrEvents);
    </script>
</asp:Content>