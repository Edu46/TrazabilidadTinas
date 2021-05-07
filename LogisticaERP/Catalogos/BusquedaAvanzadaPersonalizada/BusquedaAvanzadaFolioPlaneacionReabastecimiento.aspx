<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="BusquedaAvanzadaFolioPlaneacionReabastecimiento.aspx.cs" Inherits="LogisticaERP.Catalogos.BusquedaAvanzadaPersonalizada.BusquedaAvanzadaFolioPlaneacionReabastecimiento" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		function loadScriptsPage()
		{
		}
		function OnGridFoliosDobleClick(s_Valores)
		{
			var d_Folio_Planeacion = s_Valores[0];
			var d_Id_Planeacion_Reabastecimiento = s_Valores[1];
			parent.CerrarBusquedaFolioPlaneacionReabastecimiento(d_Id_Planeacion_Reabastecimiento,d_Folio_Planeacion);
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<input type="hidden" value="Busqueda avanzada Folio Embarque" id="hPage" />
	<asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" EnablePageMethods="true" runat="server">
	</asp:ScriptManager>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<script type="text/javascript">
				Sys.Application.add_load(Scripts_Load);
				Sys.Application.add_load(loadScriptsPage);
			</script>
			<!-- Grid de devexpress que sirve para renderizar los demas grids y corregir fallos de devexpress -->
			<div style="display: none;">
				<dx:ASPxGridView ID="gridRenderFix" runat="server" ClientIDMode="Static">
					<ClientSideEvents Init="function(s, e) { gridRenderFix.Refresh(); }" />
				</dx:ASPxGridView>
			</div>
			<!-- Main content -->
			<section id="divCustomContent" class="custom-content" runat="server">
				<div class="bs-component">
					<div class="clearfix">
						<div class="row">
							<div class="col-sm-12 paddingoff">
								<div class="box custom-box box-primary" style="margin-bottom: 8px;">
									<!-- /.box-header -->
									<div class="box-header with-border">
										<h3 class="box-title">Buscar Folio</h3>
									</div>
									<!-- /.box-body -->
									<div class="box-body">
										<div class="row">
											<div class="col-sm-12">
												<div class="table-responsive">
													<dx:ASPxGridView
														ID="GridFolios"
														ClientInstanceName="GridFolios"
														runat="server"
														AutoGenerateColumns="False"
														EnableTheming="True"
														Theme="MetropolisBlue"
														CssClass="withPagerInBoxHeader"
														Width="100%"
													>
														<ClientSideEvents
															Init="
															function(obj_Sender, e_Parametros)
															{
																resizeHeightDevControl(window, window, '#GridFolios', GridFolios, 45, 0);
																$(window).resize();
															}"
															RowDblClick="
															function(obj_Sender, e_Parametros)
															{
																GridFolios.GetRowValues(e_Parametros.visibleIndex, 'd_Folio_Planeacion;d_Id_Planeacion_Reabastecimiento', OnGridFoliosDobleClick);
															}"
														/>
														<Columns>
															<dx:GridViewDataTextColumn Caption="Folio" FieldName="d_Folio_Planeacion" Name="d_Folio_Planeacion" VisibleIndex="1">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<CellStyle HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" ></Settings>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Almacén" FieldName="s_Almacen" Name="s_Almacen" VisibleIndex="2">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<CellStyle HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" ></Settings>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Fecha" FieldName="dt_Fecha_Planeacion" Name="dt_Fecha_Planeacion" VisibleIndex="3">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<CellStyle HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" ></Settings>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Estatus" FieldName="s_Descripcion_Estatus" Name="s_Descripcion_Estatus" VisibleIndex="4">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<CellStyle HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" ></Settings>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="d_Id_Planeacion_Reabastecimiento" FieldName="d_Id_Planeacion_Reabastecimiento" Name="d_Id_Planeacion_Reabastecimiento" VisibleIndex="5" Visible="false"></dx:GridViewDataTextColumn>
														</Columns>
														<Settings VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" ShowFilterRow="True" />
														<SettingsBehavior AllowFocusedRow="true" />
														<SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
														<SettingsEditing Mode="Inline" />
														<SettingsPager AlwaysShowPager="True" PageSize="15" NumericButtonCount="5" />
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
