<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="PlaneacionReabastecimientoAlmacen.aspx.cs" Inherits="LogisticaERP.Catalogos.PlaneacionReabastecimientoAlmacen" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		Eventos =
		function ()
		{
			$(document).ready
			(
				function ()
				{
					//Selección múltiple.
					$('[id*=lst_Meses]').multiselect({includeSelectAllOption: true});
				}
			);
		}
		function CargarScripts()
		{
			MostrarIconosGrid('GridPlaneacionReabastecimiento');
			$("#btn_Buscar_Folio_Planeacion").on
			(
				'click',
				function ()
				{
					$(window).MostrarPopup
					(
						{
							contenedor: null,
							url: 'BusquedaAvanzadaPersonalizada/BusquedaAvanzadaFolioPlaneacionReabastecimiento.aspx',
							alto: '650px',
							ancho: '1000px',
							tituloPopup: 'Buscar Folio Planeación Reabastecimiento Almacén',
							masterPage: '~/Blank.Master&',
							movible: false,
							desaparecer: false
						}
					);
				}
			);
		}
		function CerrarBusquedaFolioPlaneacionReabastecimiento(d_Id_Planeacion_Reabastecimiento, d_Folio)
		{
			$(window).CerrarPopup(this);
			var v_co_d_Id_Planeacion_Reabastecimiento = document.getElementById('co_d_Id_Planeacion_Reabastecimiento');
			var v_txt_Folio_Planeacion = document.getElementById('txt_Folio_Planeacion');
			v_co_d_Id_Planeacion_Reabastecimiento.value = d_Id_Planeacion_Reabastecimiento;
			v_txt_Folio_Planeacion.value = d_Folio;
			$("#btn_Cargar_Grid").click();
		}
		function MostrarIconosGrid(s_NombreGrid)
		{
			var v_GridModificar = document.getElementById(s_NombreGrid + '_DXMainTable');
			for (var i_Renglon = 1, tbl_Renglon; tbl_Renglon = v_GridModificar.rows[i_Renglon]; i_Renglon++)
			{
				var s_Clase = '';
				var tbl_Columna = '';
				var i_ColumnaEstado = 17;
				tbl_Columna = tbl_Renglon.cells[i_ColumnaEstado];
				if (tbl_Columna != undefined)
				{
					s_Clase = tbl_Columna.innerHTML;
				}
				if (s_Clase != '')
				{
					s_Clase = IconoMostrar(s_Clase);
				}
				else
				{
					s_Clase = '';
				}
				var i_Boton = i_Renglon - 1;
				var v_Grid_DXCBtn = document.getElementById(s_NombreGrid + '_DXCBtn' + i_Boton);
				if (v_Grid_DXCBtn != undefined)
				{
					v_Grid_DXCBtn.innerHTML = '<div id="' + s_NombreGrid + '_DXCBtn' + i_Boton + 'Img" class="css_ImagenBoton">' + s_Clase + '</div>';
					$('#' + s_NombreGrid + '_DXCBtn' + i_Boton + "Img").parent().parent().css("background", "white");
				}
			}
		}
		function IconoMostrar(s_Estado)
		{
			return '<i class="css_ColorCaja fa fa-archive" aria-hidden="true"></i>';
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<input type="hidden" value="Reporte Plan Reabastecimiento Almacenes" id="hPage" />
	<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout ="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
	<asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
		<ContentTemplate>
			<script type="text/javascript">
				Sys.Application.add_load(Scripts_Load);
				Sys.Application.add_load(Eventos);
				Sys.Application.add_load(CargarScripts);
			</script>
			<!-- Estilos para los botónes en el Grid -->
			<style>
				.DropDownListPinsa .btn
				{
					padding: 2px 12px;
					border-radius: 1px;
				}
				.dropdown-menu
				{
					right: 6px !important;
				}
				.fa
				{
					display: inline-block !important;
					font: normal normal normal 10px/3 FontAwesome !important;
					font-size: inherit !important;
					text-rendering: auto !important;
					-webkit-font-smoothing: antialiased !important;
					-moz-osx-font-smoothing: grayscale !important;
				}
				.css_ImagenBoton			{ font-size: 20px !important; margin-left: 0px; height: 20px; width: 20px; }
				.css_ColorCaja				{ color: #a05726 !important; }
				.dxgvDataRow_MetropolisBlue	{ white-space: normal !important; }
				#GridPlaneacionReabastecimiento_DXMainTable .dxgvFocusedRow_MetropolisBlue { background: white; color: black; }
				.navbar-nav > li > a { padding-top: 2px; padding-bottom: 2px; }
			</style>
			<!-- Estilos para los botónes en el Grid -->
			<!-- Inicio Elementos ocultos -->
			<div style="display: none;">
				<!-- Campos -->
				<input type="hidden" runat="server" id="co_d_Id_Planeacion_Reabastecimiento" />
				<input type="hidden" runat="server" id="co_Id_Estatus" />
				<!-- Campos -->
				<!-- Botones -->
				<!-- Botones -->
				<!-- GridView que renderiza primero para corregir fallos de devexpress -->
				<dx:ASPxGridView ID="gridRenderFix" runat="server" ClientIDMode="Static">
					<ClientSideEvents Init="function(obj_Sender, e_ParametrosEvento) { gridRenderFix.Refresh(); }" />
				</dx:ASPxGridView>
				<!-- GridView que renderiza primero para corregir fallos de devexpress -->
			</div>
			<!-- Termina Elementos ocultos -->
			<!-- Panel de botones -->
			<div class="btn-container">
				<asp:Button runat="server" ID="btn_Exportar_Excel" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Exportar Excel" OnClick="btn_Exportar_Excel_Click" />
				<asp:Button runat="server" ID="btn_Registrar_Informacion" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Registrar Información" OnClick="btn_Registrar_Informacion_Click"/>
				<asp:Button runat="server" ID="btn_Grabar_Informacion" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Grabar Información" OnClick="btn_Grabar_Informacion_Click"/>
				<asp:Button runat="server" ID="btn_Limpiar" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Limpiar Pantalla" OnClick="btn_Limpiar_Click"/>
				<asp:Button runat="server" ID="btn_Cargar_Grid" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Cargar Información" OnClick="btn_Cargar_Grid_Click"/>
			</div>
			<!-- Termina Panel de botones -->
			<!-- Principal -->
			<section class="custom-content" style="overflow-x: hidden;">
				<div class="bs-component">
					<div class="clearfix">
						<div class="row">
							<div class="col-sm-12 paddingoff">
								<!-- /.box-content -->
								<div class="box custom-box box-primary" style="margin-bottom: 8px; margin-top: 10px;">
									<!-- /.box-header -->
									<div class="box-header with-border">
										<h3 class="box-title">Filtros Búsqueda</h3>
									</div>
									<!-- /.box-body -->
									<div class="box-body">
										<div class="row">
											<div class="col-sm-12">
												<div class="col-sm-2">
													<label for="lblPlaneacion" class="control-label-top">Folio Planeación</label>
													<div class="input-group">
														<asp:TextBox ID="txt_Folio_Planeacion" Enabled="false" runat="server" CssClass="form-control marginoff" MaxLength="18" placeholder="Folio Planeación" autocomplete="off"></asp:TextBox>
														<span class="input-group-btn">
															<asp:Button ID="btn_Buscar_Folio_Planeacion" CssClass="btn custom-btn btn-default" runat="server" Text="..."/>
														</span>
													</div>
												</div>
												<div class="col-sm-2">
													<label for="lblAlmacen" class="control-label-top">Nombre Almacén</label>
													<asp:DropDownList runat="server" CssClass="form-control marginoff" data-style="btn-select-default" ID="ddl_Almacen" placeholder="Almacén" ></asp:DropDownList>
												</div>
												<div class="col-sm-2">
													<label for="lblFechaInicio" class="control-label-top">Fecha Planeación:</label>
													<dx:ASPxDateEdit ID="dt_Fecha_Inicio" runat="server" placeholder="Fecha Planeación"  autocomplete="off" CssClass="form-control-date" Theme="MetropolisBlue" Date="" AutoPostBack="false">
													</dx:ASPxDateEdit>
												</div>
												<div class="DropDownListPinsa col-sm-1">
													<label for="lblMeses" class="control-label-top">Meses Venta</label>
													<asp:ListBox ID="lst_Meses" runat="server" SelectionMode="Multiple" >
														<asp:ListItem Text="ENE" Value="1" />
														<asp:ListItem Text="FEB" Value="2" />
														<asp:ListItem Text="MAR" Value="3" />
														<asp:ListItem Text="ABR" Value="4" />
														<asp:ListItem Text="MAY" Value="5" />
														<asp:ListItem Text="JUN" Value="6" />
														<asp:ListItem Text="JUL" Value="7" />
														<asp:ListItem Text="AGO" Value="8" />
														<asp:ListItem Text="SEP" Value="9" />
														<asp:ListItem Text="OCT" Value="10" />
														<asp:ListItem Text="NOV" Value="11" />
														<asp:ListItem Text="DIC" Value="12" />
													</asp:ListBox>
												</div>
												<div class="col-sm-1">
													<label for="lblEstatus" class="control-label-top">Estatus</label>
													<asp:TextBox ID="txt_Estatus" runat="server" CssClass="form-control marginoff" MaxLength="0" placeholder="" autocomplete="off" Enabled="false"></asp:TextBox>
												</div>
												<div class="col-sm-1">
													<label for="lblDias" class="control-label-top">Días Auto.</label>
													<asp:TextBox ID="txt_Dias_Inv_Autorizado" runat="server" CssClass="form-control marginoff" MaxLength="3" onkeypress="return validarCaracteres('soloNumeros',event)" placeholder="Días Auto." autocomplete="off" ></asp:TextBox>
												</div>
												<div class="col-sm-1">
													<label for="lblAnio" class="control-label-top">Año</label>
													<asp:TextBox ID="txt_Anio" runat="server" CssClass="form-control marginoff" MaxLength="4" onkeypress="return validarCaracteres('soloNumeros',event)" placeholder="Años" autocomplete="off" ></asp:TextBox>
												</div>
												<div class="col-sm-2">
													<label for="lblProducto" class="control-label-top">Producto SKU</label>
													<asp:TextBox ID="txt_Producto" runat="server" CssClass="form-control marginoff" MaxLength="10" onkeypress="return validarCaracteres('soloNumeros',event)" placeholder="Producto" autocomplete="off" ></asp:TextBox>
													<%--
													Se quita por que aún no tienen esta parte.
													<div class="input-group">
														<asp:TextBox ID="txt_Producto" runat="server" CssClass="form-control marginoff" MaxLength="10" onkeypress="return validarCaracteres('soloNumeros',event)" placeholder="Producto" autocomplete="off" ></asp:TextBox>
														<span class="input-group-btn">
															<asp:Button ID="btn_Buscar_Codigo_SKU" CssClass="btn custom-btn btn-default" runat="server" Text="..."/>
														</span>
													</div>
													--%>
												</div>
											</div>
										</div>
										<div class="row">
											<div class="col-sm-12">
												<div class="col-sm-2">
													<label for="lblInventarioEBS12" class="control-label-top"></label>
													<dx:ASPxCheckBox runat="server" ID="chk_Ebs12" elemID="checkbox" CheckBoxStyle-Cursor="pointer" Theme="Mulberry"></dx:ASPxCheckBox>
													<label for="lblPalacioHierro" class="control-label">Inventario EBS12</label>
												</div>
											</div>
										</div>
									</div>
									<!-- Termina/.box-body -->
								</div>
							</div>
						</div>
						<div class="row">
							<div class="col-sm-12 paddingoff">
								<!-- /.box-content -->
								<div class="box custom-box box-primary" style="margin-bottom: 8px;">
									<!-- /.box-header-->
									<div class="box-header with-border">
										<h3 class="box-title">Detalle Planeación</h3>
									</div>
									<!-- /.box-body -->
									<div class="box-body">
										<div class="row">
											<div class="col-sm-12">
												<div class="table-responsive">
													<dx:ASPxGridView
														ID="GridPlaneacionReabastecimiento"
														ClientInstanceName="GridPlaneacionReabastecimiento"
														runat="server"
														EnableTheming="true"
														Theme="MetropolisBlue"
														CssClass="withPagerInBoxHeader"
														AutoGenerateColumns="false"
														Width="100%"
														KeyboardSupport="true"
														AccessKey="1"
														OnCellEditorInitialize = "GridPlaneacionReabastecimiento_CellEditorInitialize"
														OnHtmlDataCellPrepared = "GridPlaneacionReabastecimiento_HtmlDataCellPrepared"
														OnRowUpdating = "GridPlaneacionReabastecimiento_RowUpdating"
													>
														<ClientSideEvents
															Init="
															function(obj_Sender, e_ParametrosEvento)
															{
																OnInit(obj_Sender, e_ParametrosEvento,'d_Real');
																resizeHeightDevControl(window, window, '#GridPlaneacionReabastecimiento', GridPlaneacionReabastecimiento, 70,0);
																$(window).resize();
															}"
															RowDblClick="
															function(obj_Sender, e_ParametrosEvento)
															{
															}"
															CustomButtonClick="
															function(obj_Sender, e_ParametrosEvento)
															{
																switch(e_ParametrosEvento.buttonID)
																{
																	case 'btn_VisualizarContenido':
																		break;
																}
															}"
															EndCallback="
															function(obj_Sender, e_ParametrosEvento)
															{
																MostrarIconosGrid('GridPlaneacionReabastecimiento');
															}"
														/>
														<Columns>
															<dx:GridViewCommandColumn ShowSelectCheckbox="false" ShowEditButton="false" ShowNewButtonInHeader="false" ShowUpdateButton="false" ShowCancelButton="false" VisibleIndex="0" Width="90px" Caption="Detalles" ButtonType="Image">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<CustomButtons>
																	<dx:GridViewCommandColumnCustomButton ID="btn_VisualizarContenido" Visibility="BrowsableRow">
																		<Image ToolTip="Ver Detalle" IconID=""></Image>
																	</dx:GridViewCommandColumnCustomButton>
																</CustomButtons>
															</dx:GridViewCommandColumn>
															<dx:GridViewDataTextColumn Caption="Almacén" FieldName="s_Codigo_Almacen" Name="s_Codigo_Almacen" VisibleIndex="1" Width="90px" ReadOnly="true" EditFormSettings-Visible="False">
																<CellStyle HorizontalAlign="Center" />
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Descripción Almacén" FieldName="s_Descripcion_Almacen" Name="s_Descripcion_Almacen" VisibleIndex="2" Width="150px" ReadOnly="true" EditFormSettings-Visible="False">
																<CellStyle HorizontalAlign="Center" />
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="SKU" FieldName="s_SKU" Name="s_SKU" VisibleIndex="3" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<CellStyle HorizontalAlign="Center" />
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Descripción SKU" FieldName="s_Descripcion_SKU" Name="s_Descripcion_SKU" VisibleIndex="4" Width="450px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Inv. Actual" FieldName="d_Inventario_Actual" Name="d_Inventario_Actual" VisibleIndex="5" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Inv. Tránsito" FieldName="d_Inventario_Transito" Name="d_Inventario_Transito" VisibleIndex="6" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Mes 1" FieldName="d_Mes_1" Name="d_Mes_1" VisibleIndex="7" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Mes 2" FieldName="d_Mes_2" Name="d_Mes_2" VisibleIndex="8" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Mes 3" FieldName="d_Mes_3" Name="d_Mes_3" VisibleIndex="9" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Ventas Prom." FieldName="d_Ventas_Promedio" Name="d_Ventas_Promedio" VisibleIndex="10" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Ventas Mes" FieldName="d_Ventas_Mes" Name="d_Ventas_Mes" VisibleIndex="11" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Envíos" FieldName="d_Envios" Name="d_Envios" VisibleIndex="12" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Rec Ships" FieldName="s_Rec_Ships" Name="s_Rec_Ships" VisibleIndex="13" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<CellStyle HorizontalAlign="Right"></CellStyle>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Saldo Final" FieldName="d_Saldo_Final" Name="d_Saldo_Final" VisibleIndex="14" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Días Inv" FieldName="i_Dias_Inventario" Name="i_Dias_Inventario" VisibleIndex="15" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Días Inv Ideal" FieldName="i_Dias_Inventario_Ideal" Name="i_Dias_Inventario_Ideal" VisibleIndex="16" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Inv. PINSA" FieldName="s_Inventario_Pinsa" Name="s_Inventario_Pinsa" VisibleIndex="17" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<CellStyle HorizontalAlign="Right"></CellStyle>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Por Surtir" FieldName="s_Por_Surtir" Name="s_Por_Surtir" VisibleIndex="18" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<CellStyle HorizontalAlign="Right"></CellStyle>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="A Enviar" FieldName="s_A_Enviar" Name="s_A_Enviar" VisibleIndex="19" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<CellStyle HorizontalAlign="Right"></CellStyle>
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Lead Time" FieldName="i_Lead_Time" Name="i_Lead_Time" VisibleIndex="20" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Sugerido" FieldName="d_Sugerido" Name="d_Sugerido" VisibleIndex="21" Width="95px" ReadOnly="true" EditFormSettings-Visible="False">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="Real" FieldName="d_Real" Name="d_Real" VisibleIndex="22" Width="95px"	>
																<HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
																<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
																<PropertiesTextEdit DisplayFormatString="n0">
																	<ValidationSettings EnableCustomValidation="true">
																		<RequiredField IsRequired="true" ErrorText="Es necesario indicar una cantidad mayor a 0."></RequiredField>
																	</ValidationSettings>
																	<ClientSideEvents
																		KeyPress="
																		function(obj_Sender, e_ParametrosEvento)
																		{
																			return validarCaracteres('soloNumeros', event);
																		}"
																	/>
																</PropertiesTextEdit>
															</dx:GridViewDataTextColumn>
															<dx:GridViewDataTextColumn Caption="d_Id_Planeacion_Reabastecimiento_Detalle" FieldName="d_Id_Planeacion_Reabastecimiento_Detalle" Name="d_Id_Planeacion_Reabastecimiento_Detalle" VisibleIndex="18" Width="0" Visible="false"></dx:GridViewDataTextColumn>
														</Columns>
														<Settings VerticalScrollableHeight="225" VerticalScrollBarMode="Auto" ShowFilterRow="true" />
														<SettingsBehavior AllowFocusedRow="True" AllowDragDrop="false" />
														<SettingsDataSecurity AllowDelete="true" />
														<SettingsEditing Mode="Batch" />
														<SettingsPager AlwaysShowPager="true" PageSize="500"/>
													</dx:ASPxGridView>
													<dx:ASPxGridViewExporter ID="GridPlaneacionReabastecimiento_Exportar" GridViewID="GridPlaneacionReabastecimiento" runat="server"></dx:ASPxGridViewExporter>
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
			<!-- Termina Principal -->
			<script type="text/javascript">
			</script>
		</ContentTemplate>
		<Triggers >
			<asp:AsyncPostBackTrigger ControlID="btn_Cargar_Grid" EventName="Click" />
			<asp:AsyncPostBackTrigger ControlID="btn_Grabar_Informacion" EventName="Click" />
			<asp:AsyncPostBackTrigger ControlID="btn_Registrar_Informacion" EventName="Click" />
			<asp:PostBackTrigger ControlID="btn_Exportar_Excel" />
		</Triggers>
	</asp:UpdatePanel>
</asp:Content>
