<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="BusquedaCodigosMotivos.aspx.cs" Inherits="LogisticaERP.Catalogos.CancelacionMasiva.Popup.BusquedaCodigosMotivos" %>

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

        function onGridDblClick(codigo) {
            try {
                parent.MostrarDatosCodigoMotivo(codigo);
            }
            catch (error) {
                console.error(error);
                showMessage(WARNINGBOX, 'Ocurrió un problema no esperado, favor de comunicarse a Atención a Usuarios.');
                return false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript">
                Sys.Application.add_load(Scripts_Load);
            </script>
             <!-- Inicio Elementos ocultos -->
            <div style="display: none;">
                <!-- GridView que renderiza primero para corregir fallos de devexpress -->
                <dx:ASPxGridView ID="gridRenderFix" runat="server" ClientIDMode="Static">
                    <ClientSideEvents Init="function(s, e) { gridRenderFix.Refresh(); }" />
                </dx:ASPxGridView>
            </div>
            <!-- Principal -->
            <section class="custom-content" style="overflow-x: hidden;">
                <div class="bs-component">
                    <div class="clearfix">
                        <div class="row">
                            <div class="col-sm-12 paddingoff">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary" style="margin-bottom: 8px;">
                                    <%--Inicia Cabecera--%>
									<div class="panel-heading">
										<h3 class="panel-title css_Titulos_Formas">&nbsp;</h3>
									</div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="table-responsive">
                                                    <dx:ASPxGridView
                                                        ID="gridCodigosMotivos"
                                                        ClientInstanceName="gridCodigosMotivos"
                                                        runat="server"
                                                        EnableTheming="true"
                                                        Theme="MetropolisBlue"
                                                        CssClass="withPagerInBoxHeader"
                                                        AutoGenerateColumns="false"
                                                        Width="100%"
                                                        KeyboardSupport="true"
                                                        AccessKey="1">
                                                        <ClientSideEvents
                                                            Init="
															function(s, e)
															{
																OnInit(s, e,'Codigo');
																resizeHeightDevControl(window, window, '#gridCodigosMotivos', gridCodigosMotivos, 70,0);
																$(window).resize();
															}"
                                                            RowDblClick="function(obj_Sender, e_Parametros) {
                                                                    gridCodigosMotivos.GetRowValues(e_Parametros.visibleIndex, 'Codigo', onGridDblClick);
                                                                }"
                                                             />
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Codigo" FieldName="Codigo" Name="Codigo" VisibleIndex="0" Width="30%" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Descripcion" FieldName="Descripcion" Name="Descripcion" VisibleIndex="1" Width="70%" ReadOnly="true" EditFormSettings-Visible="False">
                                                                <CellStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" ShowFilterRow="true" />
                                                        <Settings ShowFilterRow="true" />
                                                        <SettingsBehavior AllowFocusedRow="True" />
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
    </asp:UpdatePanel>
</asp:Content>
