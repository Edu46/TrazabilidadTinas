<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="ObtenerFacturaPinsa.aspx.cs" Inherits="LogisticaERP.Catalogos.OracleCloud.popup.ObtenerFacturaPinsa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<%--Inicia Referencias a contenido externo--%>
	<link href="../../../css/EstilosGenerales.css" rel="stylesheet" type="text/css" />
	<%--Termina Referencias a contenido externo--%>
	<%--Inicia apartado de script para cargar en la pantalla--%>
	<script type="text/javascript">
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Función para agregar los eventos que tendrá la página.
		function Eventos()
		{
			$("#div_btn_EjecutarConcurrente").on
			(
				'click',
				function ()
				{
					$("#btn_EjecutarConcurrente").click();
				}
			);
			//$("#div_btn_CerrarPopup").on
			//(
			//	'click',
			//	function ()
			//	{
			//		CerrarVentana();
			//	}
			//);
		}
	</script>
	<%--Termina apartado de script para cargar en la pantalla--%>
	<%--Inicia pantalla para cargar en la pantalla--%>
	<style>
		/*-----------------------------------------------------------------------------------------------------------------------------------------------
		Controles generales
		-----------------------------------------------------------------------------------------------------------------------------------------------*/
		.css_Pie_Modal { text-align: center; }
		.modal-dialog_PopUpEmpresa { width: 415px !important; }
		.btn-select-default { font-size: 19px; height: 45px !important; }
		body
		{
		}
		label
		{
			font-weight: normal !important;
		}
		.panel-heading
		{
			background-color: #3c8dbc !important;
		}
		.css_Margen_Nueva_Forma
		{
			margin-top: 10px !important;
		}
		.css_Titulos_Formas
		{
			font-size: 17px !important;
			font-weight: 300 !important;
		}
		.css_Controles_Generales
		{
			font-size: 17px !important;
			line-height: 30px !important;
		}
		.css_Controles_Generales_Etiquetas
		{
			font-size: 16px !important;
			font-weight: bold !important;
			line-height: 30px !important;
		}
		.css_Controles_Generales_Etiquetas_Centradas
		{
			font-size: 17px !important;
			font-weight: bold !important;
			line-height: 30px !important;
			text-align: center;
		}
		.css_MargenElementos_Grupos
		{
			display: inline-block;
		}
		.css_Controles_Input
		{
			height: 30px !important;
		}
		.css_Controles_Input_PopUp
		{
			font-size: 19px;
			height: 45px !important;
		}
		.css_Controles_Input_Time
		{
			display: inline-block !important;
			font-size: 20px !important;
			height: 30px !important;
			text-align: center;
			width: 47% !important;
		}
		.dxpLite_MetropolisBlue
		{
			color: #f9f9f9 !important;
		}
		/*-----------------------------------------------------------------------------------------------------------------------------------------------
		Botones
		-----------------------------------------------------------------------------------------------------------------------------------------------*/
		.css_Div_Boton
		{
			background-color: #3c8dbc;
			border: 1px solid #3c8dbc;
			border-radius: 4px;
			box-shadow: 0px 3px 10px rgba(0,0,0,.35);
			color: #FFFFFF;
			cursor: pointer;
			display: inline;
			font-size:16px;
			margin: 5px;
			padding: 5px 33px;
			text-decoration: none;
			-webkit-user-select: none;
		}
		.css_Div_Boton_Der
		{
			float: right;
		}
		.css_Div_Boton_Izq
		{
			float: left;
		}
		.css_Div_Boton_Redondo
		{
			border-radius: 15px !important;
		}
		.css_Div_Boton:hover
		{
			box-shadow: 0px 7px 10px rgba(0,0,0,.35);
			opacity: 0.7;
		}
		.css_Div_Boton:active
		{
			box-shadow: inset 0px 7px 10px rgba(0,0,0,.35);
			opacity: 0.9;
		}
		.css_Div_Boton_Buscar
		{
			font-size: 19px !important;
			margin: 0px !important;
			padding: 0px 7px 0px 7px !important;
		}
		.css_Div_Boton_Con_Icono
		{
			font-size: 19px !important;
			margin: 0px 0px 0px 7px !important;
			padding: 1px 7px 3px 7px !important;
		}
		.css_Div_Boton_Con_Icono_PopUp
		{
			font-size: 21px !important;
			margin: 0px 0px 0px 7px !important;
			padding: 1px 3px 1px 3px !important;
		}
		.css_Div_Boton_Con_Icono_Sin_Margen
		{
			font-size: 19px !important;
			margin: 0px 0px 0px -3px !important;
			padding: 1px 7px 3px 7px !important;
		}
		.css_Div_Boton_Spin_Mas
		{
			font-size: 19px !important;
			margin: 1px !important;
			padding: 0px 7px 0px 7px !important;
		}
		.css_Div_Boton_Spin_Menos
		{
			font-size: 24px !important;
			margin: 1px !important;
			padding: 0px 9px 0px 9px !important;
			line-height: 27px !important;
		}
		.css_Div_Boton_Grupos_Pequenios
		{
			width: 19% !important;
		}
	</style>
	<%--Termina pantalla para cargar en la pantalla--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<input type="hidden" value="Obtener Facturas PINSA" id="hPage" />
	<asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" EnablePageMethods="true" runat="server">
	</asp:ScriptManager>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<script type="text/javascript">
				Sys.Application.add_load(Scripts_Load);
				Sys.Application.add_load(Eventos);
			</script>
			<!-- Grid de devexpress que sirve para renderizar los demas grids y corregir fallos de devexpress -->
			<div style="display: none;">
				<!-- Inicia apartado campos ocultos -->
				<input type="hidden" runat="server" id="co_s_Error_Pantalla" />
				<!-- Termina apartado campos ocultos -->
				<!-- Inicia apartado botones ocultos -->
				<asp:Button runat="server" ID="btn_EjecutarConcurrente" UseSubmitBehavior="False" Text="EjecutarConcurrente" OnClick="btn_EjecutarConcurrente_Click"/>
				<!-- Termina apartado botones ocultos -->
			</div>
			<!-- Main content -->
			<section id="divCustomContent" class="custom-content" runat="server">
				<div class="bs-component">
					<div class="clearfix">
						<div class="row">
							<div class="col-sm-12 paddingoff">
								<div class="panel panel-primary">
									<%--Inicia Cabecera--%>
									<div class="panel-heading">
										<h3 class="panel-title css_Titulos_Formas">Introduce el Folio</h3>
									</div>
									<%--Termina Cabecera--%>
									<%--Inicia Cuerpo--%>
									<div class="panel-body">
										<div class="row">
											<div class="col-sm-12">
												<div class="row">
													<div class="col-sm-4">
														<label for="lblSerie" class="control-label-top css_Controles_Generales_Etiquetas">Serie:</label>
														<asp:TextBox ID="txt_Serie" runat="server" CssClass="form-control marginoff css_Controles_Generales css_Controles_Input" Enabled="true" placeholder="Serie"></asp:TextBox>
														<label for="lblSerie" class="control-label-top css_Controles_Generales_Etiquetas">&nbsp;</label>
													</div>
													<div class="col-sm-4">
														<label for="lblNumeroFactura" class="control-label-top css_Controles_Generales_Etiquetas">Factura:</label>
														<div class="input-group css_MargenElementos">
															<asp:TextBox ID="txt_Factura" runat="server" CssClass="form-control marginoff css_Controles_Generales css_Controles_Input" Enabled="true" placeholder="Factura"></asp:TextBox>
															<span runat="server" id="span_Contenedor_Boton_Buscar" class="input-group-btn">
																<div id="div_btn_EjecutarConcurrente" class="css_Div_Boton css_Div_Boton_Con_Icono">&nbsp;<i class="fa fa-cogs"></i>&nbsp;</div>
															</span>
														</div>
														<label for="lblNumeroFactura" class="control-label-top css_Controles_Generales_Etiquetas">&nbsp;</label>
													</div>
												</div>
											</div>
										</div>
									</div>
									<%--Termina Cuerpo--%>
								</div>
							</div>
						</div>
					</div>
				</div>
			</section>
		</ContentTemplate>
		<Triggers >
			<asp:AsyncPostBackTrigger ControlID="btn_EjecutarConcurrente" EventName="Click" />
		</Triggers>
	</asp:UpdatePanel>
</asp:Content>
