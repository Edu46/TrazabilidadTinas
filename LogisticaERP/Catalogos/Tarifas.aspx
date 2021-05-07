<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="Tarifas.aspx.cs" Inherits="LogisticaERP.Catalogos.Tarifas" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function CargarScripts() {
            barraGrid();

        }

        function barraGrid() {
            $("#gridTarifas_DXStatus").css("height", "35px");
        }

        var indexeliminar = null;
        var columnid = null;
        var columnidfinal = null;
        var valoractual = null;
        var columname = null;
        var indexupdate = null;
        var valorNuevo =null;
        var municipios = null;
        var visibleIndexEdit;
        var lastIdEstado;
        var lastIdMunicipio;

        var ciudades = null;
        var lastIdMunicipioCd;
        var lastIdCiudad;

        $(document).ready(function () {

            PageMethods.ObtenerMunicipios(SucessMunicipios);
            PageMethods.ObtenerCiudades(SucessCiudades);

            $("#btnAceptarBorrar").on("click", function (s, e) {

                gridTarifasCliente.DeleteRow(indexeliminar);
                $('#myModalBorrar').modal('hide');
                indexeliminar = null;

            });


            $("#btnCancelarAct").on("click", function (s, e) {

                gridTarifasCliente.batchEditApi.ResetChanges(indexupdate, columnidfinal);
                $('#myModalActualizar').modal('hide');
                indexupdate = null;
                columnidfinal = null;

            });

            $("#btnAceptarAct").on("click", function (s, e) {
                $('#myModalActualizar').modal('hide');

            });

        });

        function SucessMunicipios(response) {
            municipios = response.datos;
        }

        function SucessCiudades(response) {
            ciudades = response.datos;
        }

        function IntializeGlobalVariables(grid) {
            lastIdEstado = grid.cplastIdProyectoTarea;
            visibleIndexEdit = -1;
            lastIdMunicipio = -1;
            lastIdMunicipio = -1;
            lastIdCiudad = -1;
        }

       /* function OnBatchEditEndEditing(s, e) {

            visibleIndexEdit = -1;
        }*/


        function SeleccionarEstado(s, e) {

            MunicipioCliente.ClearItems();
            MunicipioCliente.AddItem("..:: SELECCIONAR ::..", 0);
            MunicipioCliente.SetSelectedIndex(-1);
            gridTarifasCliente.batchEditApi.SetCellValue(visibleIndexEdit, "Id_municipio", 0);

            var idEstado = s.GetSelectedItem().value;
            lastIdEstado = idEstado;
            lastIdMunicipio = -1;

            CargarMunicipios(municipios, idEstado);
        }

        function CargarMunicipios(municipios, idEstado) {

            MunicipioCliente.ClearItems();
            MunicipioCliente.AddItem("..:: SELECCIONAR ::..", 0);

            var MunicipiosFiltrados = [];

            for (let i = 0; i < municipios.length; i++) {
                var idEstadoValor = municipios[i].Item3;
                if (idEstadoValor == idEstado) {
                    MunicipiosFiltrados.push(municipios[i])
                }
            }

            for (var i in MunicipiosFiltrados) {
                MunicipioCliente.AddItem(MunicipiosFiltrados[i].Item1, MunicipiosFiltrados[i].Item2);
            }


            if (lastIdMunicipio == -1)
                MunicipioCliente.SetSelectedIndex(0);
            else if (lastIdMunicipio > -1) {
                MunicipioCliente.SetSelectedItem(MunicipioCliente.FindItemByValue(lastIdMunicipio));
                lastIdMunicipio = -1;
            }
        }

        function OnBatchEditStartEditing(s, e) {
            visibleIndexEdit = e.visibleIndex;
            var valorEstado = gridTarifasCliente.batchEditApi.GetCellValue(visibleIndexEdit, "Id_estado");
            var valorMunicipio = gridTarifasCliente.batchEditApi.GetCellValue(visibleIndexEdit, "Id_municipio");
            var valorCiudad = gridTarifasCliente.batchEditApi.GetCellValue(visibleIndexEdit, "Id_ciudad");
            lastIdMunicipio = valorMunicipio;
            lastIdCiudad = valorCiudad;

            if (lastIdEstado == valorEstado){
                CargarMunicipios(municipios, valorEstado);
                MunicipioCliente.SetSelectedItem(MunicipioCliente.FindItemByValue(lastIdMunicipio));
            }else {

                if (valorEstado == null) {
                    MunicipioCliente.ClearItems();
                    MunicipioCliente.AddItem("..:: SELECCIONAR ::..", 0);
                    MunicipioCliente.SetSelectedIndex(-1);
                    return;
                }

                lastIdEstado = valorEstado;
                CargarMunicipios(municipios, valorEstado);
            }


            /* Ciudades*/

            if (lastIdMunicipioCd == valorMunicipio) {
                CargarCiudades(ciudades, valorMunicipio);
                CiudadCliente.SetSelectedItem(CiudadCliente.FindItemByValue(lastIdCiudad));
            }
            else {

                if (valorMunicipio == null) {
                    CiudadCliente.ClearItems();
                    CiudadCliente.AddItem("..:: SELECCIONAR ::..", 0);
                    CiudadCliente.SetSelectedIndex(-1);

                } else {
                    lastIdMunicipioCd = valorMunicipio;
                    CargarCiudades(ciudades, valorMunicipio);
                }

            }
        }

        /*Cargar las ciudades relacionadas con el municipio */


        function SeleccionarMunicipio(s, e) {

            CiudadCliente.ClearItems();
            CiudadCliente.AddItem("..:: SELECCIONAR ::..", 0);
            CiudadCliente.SetSelectedIndex(-1);
            gridTarifasCliente.batchEditApi.SetCellValue(visibleIndexEdit, "Id_ciudad", 0);

            var idMunicipio = s.GetSelectedItem().value;
            lastIdMunicipioCd = idMunicipio;
            lastIdCiudad = -1;

            CargarCiudades(ciudades, idMunicipio);
        }

        function CargarCiudades(ciudades, idMunicipio) {

            CiudadCliente.ClearItems();
            CiudadCliente.AddItem("..:: SELECCIONAR ::..", 0);

            var CiudadesFiltrados = [];

            for (let i = 0; i < ciudades.length; i++) {
                var idMunicipioValor = ciudades[i].Item3;
                if (idMunicipioValor == idMunicipio) {
                    CiudadesFiltrados.push(ciudades[i])
                }
            }

            for (var i in CiudadesFiltrados) {
                CiudadCliente.AddItem(CiudadesFiltrados[i].Item1, CiudadesFiltrados[i].Item2);
            }


            if (lastIdCiudad == -1)
                CiudadCliente.SetSelectedIndex(0);
            else if (lastIdCiudad > -1) {
                CiudadCliente.SetSelectedItem(CiudadCliente.FindItemByValue(lastIdCiudad));
                lastIdCiudad = -1;
            }
        }


        function validarValor(s, e, numeroDecimales, nombreColumna) {
            if (parseFloat(s.GetValue() != null && s.GetValue()).toFixed(numeroDecimales) > 0) {
                e.isValid = true;
            }
            else {
                e.isValid = false;
                e.errorText = 'Es necesario selecciona un ' + nombreColumna + ' .';
            }
        }

        function validarValorCero(s, e, numeroDecimales, nombreColumna) {
            if (parseFloat(s.GetValue() != null && s.GetValue()).toFixed(numeroDecimales) >= 0) {
                e.isValid = true;
            }
            else {
                e.isValid = false;
                e.errorText = 'Es necesario selecciona un ' + nombreColumna + ' .';
            }
        }





    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<div class="modal fade" id="myModalBorrar" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="myModalBorrar" aria-hidden="true" style="overflow-y: auto;">
		<div class="modal-dialog modal-sm">
			<div class="modal-content">
				<div class="modal-header">
					<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
					<h4 class="modal-title" id="myModalBorrarLabel">Eliminar registro</h4>
				</div>
				<div class="modal-body">
					<div class="row">
						<div class="col-md-12">
							<p style="margin: 0; font-family: 'Quantico', sans-serif; font-weight: bold; font-size: 18px;">
								¿Esta seguro(a) que desea eliminar este registro?
							</p>
						</div>
					</div>
				</div>
				<div class="modal-footer">
					<button id="btnAceptarBorrar" type="button" class="btn btn-default">Aceptar</button>
					<button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
				</div>
			</div>
		</div>
	</div>

	<div class="modal fade" id="myModalActualizar" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="myModalBorrar" aria-hidden="true" style="overflow-y: auto;">
		<div class="modal-dialog modal-sm">
			<div class="modal-content">
				<div class="modal-header">
					<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
					<h4 class="modal-title" id="myModalActualizarLabel">Eliminar registro</h4>
				</div>
				<div class="modal-body">
					<div class="row">
						<div class="col-md-12">
							<p style="margin: 0; font-family: 'Quantico', sans-serif; font-weight: bold; font-size: 18px;">
								¿Esta seguro(a) que desea actualizar este registro?
							</p>
						</div>
					</div>
				</div>
				<div class="modal-footer">
					<button id="btnAceptarAct" type="button" class="btn btn-default">Aceptar</button>
                    <button id="btnCancelarAct" type="button" class="btn btn-default">Cancelar</button>
				</div>
			</div>
		</div>
	</div>


    <input type="hidden" value="Catalogo de Tarifas" id="hPage" />
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
                <asp:HiddenField runat="server" ID="hdfIdEmpresa" Value="-1" />
                <asp:HiddenField runat="server" ID="hdfValorColumna" Value="-1" />
                <asp:HiddenField runat="server" ID="hdfValorNuevo" Value="-1" />
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
                <asp:Button runat="server" ID="btnExportarExcel" UseSubmitBehavior="False" Style="margin-left: 5px;" CssClass="btn custom-btn btn-primary pull-right" Text="Exportar Excel" OnClick="btnExportarExcel_Click" />

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
                                        <h3 class="box-title">Captura de Tarifas</h3>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="table-responsive">
                                                    <dx:ASPxGridView
                                                        ID="gridTarifas"
                                                        ClientInstanceName="gridTarifasCliente"
                                                        runat="server"
                                                        EnableTheming="true"
                                                        Theme="MetropolisBlue"
                                                        CssClass="withPagerInBoxHeader"
                                                        AutoGenerateColumns="false"
                                                        Width="100%"
                                                        KeyboardSupport="true"
                                                        AccessKey="1"
                                                        OnCellEditorInitialize="gridTarifas_CellEditorInitialize"
                                                        OnBatchUpdate="gridTarifas_BatchUpdate"
                                                        OnRowInserting="gridTarifas_RowInserting"
                                                        OnRowUpdating="gridTarifas_RowUpdating"
                                                        OnRowDeleting="gridTarifas_RowDeleting"
                                                        OnRowValidating="gridTarifas_RowValidating"
                                                         OnInitNewRow="gridTarifas_InitNewRow"
                                                        >
                                                        <ClientSideEvents
                                                            Init="
															function(s, e)
															{
																OnInit(s, e,'Estado');
																resizeHeightDevControl(window, window, '#gridTarifas', gridTarifasCliente, 70,0);
																$(window).resize();
                                                                IntializeGlobalVariables(s);
															}"
                                                            EndCallback="
                                                            function(s, e) 
                                                            {                                                                                          
                                                                if (s.cpExito != undefined && s.cpExito != null) {
                                                                    MostrarCajaMensajes(SUCCESSBOX, [{ 'StrongText': 'Se ha guardado con exito la información.' }], 8000);
                                                                    s.cpExito = null;
                                                                }                                                                                      

                                                                if (s.cpMensajeAdvertencia != undefined && s.cpMensajeAdvertencia != null) {
                                                                    MostrarCajaMensajes(WARNINGBOX, [{ 'StrongText': s.cpMensajeAdvertencia }], 8000);
                                                                    s.cpMensajeAdvertencia = null;
                                                                }

                                                                if (s.cpMensajeError != undefined && s.cpMensajeError != null) {
                                                                    MostrarCajaMensajes(ERRORBOX, [{ 'StrongText': s.cpMensajeError }], 8000);
                                                                    s.cpMensajeError = null;
                                                                }                   

                                                                barraGrid();

                                                            }"  
                                                            BatchEditEndEditing="
                                                            function(s,e)
                                                            {
                                                               visibleIndexEdit = -1;
                                                               valorNuevo = e.rowValues[columnid].value;
                                                             
                                                                if (valorNuevo != valoractual && e.visibleIndex >= 0){
                                                                    $('#myModalActualizar').modal('show');	
                                                                     columnidfinal = columnid;
                                                                     indexupdate = e.visibleIndex;
                                                                }
                                                            }"
                                                            BatchEditStartEditing="
                                                            function(s, e) 
                                                            {
                                                                columname = e.focusedColumn.fieldName;
	                                                            columnid = e.focusedColumn.index;
                                                                valoractual = s.batchEditApi.GetCellValue(e.visibleIndex, e.focusedColumn.fieldName,false);

                                                                if ( e.focusedColumn.fieldName == 'Backhaul'){
                                                                    console.log('Backhaul');
                                                                    console.log(valoractual);
                                                                }

                                                                if ( e.focusedColumn.fieldName == 'Fecha_inicio'){
                                                                    console.log('Fecha_inicio');
                                                                    console.log(valoractual);
                                                                }

                                                                if (valoractual == 0) valoractual=null;
                                                                OnBatchEditStartEditing(s, e);
                                                                
                                                                
                                                                
                                                            }"

                                                            CustomButtonClick="function(s,e){
                                                                switch(e.buttonID) {
																	case 'btnEliminar':
                                                                        indexeliminar = e.visibleIndex;
																		$('#myModalBorrar').modal('show');																		
																	break;
																	
																}
                                                            }"
                                                            />
                                                        <Columns>
                                                            <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowCancelButton="true" ButtonType="Image" Width="50" VisibleIndex="0" Caption=" " ShowClearFilterButton="True">
                                                                <NewButton Image-ToolTip="Nuevo" Image-IconID="actions_new_16x16gray"></NewButton>
                                                                <EditButton Image-ToolTip="Editar" Image-IconID="edit_edit_16x16gray"></EditButton>
                                                                <UpdateButton Image-ToolTip="Modificar" Image-IconID="save_save_16x16gray"></UpdateButton>
                                                                <DeleteButton Image-ToolTip="Eliminar" Image-IconID="actions_deleteitem_16x16gray"></DeleteButton>
                                                                <CancelButton Image-ToolTip="Cancelar" Image-IconID="actions_close_16x16gray"></CancelButton>
                                                                <ClearFilterButton Image-ToolTip="Limpiar filtro" Image-IconID="actions_clear_16x16gray"></ClearFilterButton>
                                                                <CustomButtons>
                                                                    <dx:GridViewCommandColumnCustomButton Text="Eliminar" ID="btnEliminar">
                                                                        <Image ToolTip="Eliminar" IconID="actions_deleteitem_16x16gray" ></Image>
                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                </CustomButtons>
                                                            </dx:GridViewCommandColumn>
                                                            <dx:GridViewDataComboBoxColumn Caption="Estado" VisibleIndex="1" FieldName="Id_estado" Name="Id_estado" Width="150px">
                                                                <PropertiesComboBox IncrementalFilteringMode="StartsWith" TextField="Nombre" ValueField="Id_estado" ClientInstanceName="EstadoCliente">
                                                                    <ClientSideEvents 
                                                                        SelectedIndexChanged="function(s, e) {
																				    		SeleccionarEstado(s,e);
																				    	    }"
                                                                        Validation="function(s, e) {
                                                                            validarValor(s, e, 0, &#39;Estado&#39;);
                                                                         }"
                                                                        ></ClientSideEvents>
                                                                    <ValidationSettings>
                                                                        <RequiredField ErrorText="Es necesario seleccionar un Estado."></RequiredField>
                                                                    </ValidationSettings>
                                                                </PropertiesComboBox>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" FilterMode="DisplayText"></Settings>
                                                            </dx:GridViewDataComboBoxColumn>
                                                            <dx:GridViewDataComboBoxColumn Caption="Municipio" VisibleIndex="2" FieldName="Id_municipio" Name="Id_municipio" Width="200px">
                                                                <PropertiesComboBox IncrementalFilteringMode="StartsWith" TextField="Nombre_municipio" ValueField="Id_municipio" ClientInstanceName="MunicipioCliente">
                                                                    <ClientSideEvents 
                                                                        Validation="function(s, e) {
                                                                            validarValor(s, e, 0, &#39;Municipio&#39;);
                                                                            }"
                                                                         SelectedIndexChanged="function(s, e) {
																				    		SeleccionarMunicipio(s,e);
																			}"
                                                                        ></ClientSideEvents>
                                                                </PropertiesComboBox>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" FilterMode="DisplayText"></Settings>
                                                                <PropertiesComboBox ValueField="Id_municipio" TextField="Nombre_municipio" IncrementalFilteringMode="StartsWith" ClientInstanceName="MunicipioCliente">
                                                                </PropertiesComboBox>
                                                            </dx:GridViewDataComboBoxColumn>
                                                            <dx:GridViewDataComboBoxColumn Caption="Ciudad" VisibleIndex="3" FieldName="Id_ciudad" Name="Id_ciudad" Width="180px">
                                                                <PropertiesComboBox IncrementalFilteringMode="StartsWith" TextField="Nombre_ciudad" ValueField="Id_ciudad" ClientInstanceName="CiudadCliente">
                                                                    <ClientSideEvents Validation="function(s, e) {
                                                                        validarValor(s, e, 0, &#39;Ciudad&#39;);
                                                                    }"></ClientSideEvents>
                                                                </PropertiesComboBox>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" FilterMode="DisplayText"></Settings>
                                                            </dx:GridViewDataComboBoxColumn>
                                                            <dx:GridViewDataComboBoxColumn Caption="Tipo Tarifa" VisibleIndex="4" FieldName="Id_tipo_tarifa" Name="Id_tipo_tarifa" Width="130px">
                                                                <PropertiesComboBox>
                                                                    <ClientSideEvents Validation="function(s, e) {
                                                                        validarValor(s, e, 0, &#39;Tipo Tarifa&#39;);

                                                                        }"></ClientSideEvents>
                                                                    <Items>
                                                                        <dx:ListEditItem Text="CLIENTE" Value="1"></dx:ListEditItem>
                                                                        <dx:ListEditItem Text="REPRESENTANTE" Value="2"></dx:ListEditItem>
                                                                    </Items>
                                                                </PropertiesComboBox>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataComboBoxColumn>
                                                            <dx:GridViewDataComboBoxColumn Caption="Cod. Transporte" VisibleIndex="5" FieldName="Id_codigo_transporte" Name="Id_codigo_transporte" Width="150px">
                                                                <PropertiesComboBox>
                                                                   <ClientSideEvents Validation="function(s, e) {
                                                                        validarValor(s, e, 0, &#39;Cod. Transporte&#39;);
                                                                        }"></ClientSideEvents>
                                                                </PropertiesComboBox>

                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataComboBoxColumn>
                                                            <dx:GridViewDataComboBoxColumn Caption="Proveedor" VisibleIndex="6" FieldName="Id_proveedor_ebs12" Name="Id_proveedor_ebs12" Width="250px">
                                                                <PropertiesComboBox>
                                                                   <ClientSideEvents Validation="function(s, e) {
                                                                        validarValor(s, e, 0, &#39;Proveedor&#39;);
                                                                        }"></ClientSideEvents>
                                                                </PropertiesComboBox>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataComboBoxColumn>
                                                            <dx:GridViewDataSpinEditColumn Caption="Tarifa" VisibleIndex="7" FieldName="Monto_tarifa" Name="Monto_tarifa" Width="100px">
                                                                <PropertiesSpinEdit DisplayFormatString="C4">
                                                                    <ClientSideEvents Validation="function(s, e) {
                                                                        validarValor(s, e, 3, &#39;Monto de Tarifa&#39;);
                                                                        }"></ClientSideEvents>
                                                                </PropertiesSpinEdit>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" FilterMode="DisplayText"></Settings>
                                                            </dx:GridViewDataSpinEditColumn>
                                                            <dx:GridViewDataSpinEditColumn Caption="Tarifa Caseta" VisibleIndex="8" FieldName="Monto_caseta" Name="Monto_caseta" Width="100px">
                                                                <PropertiesSpinEdit DisplayFormatString="C4">
                                                                    <ClientSideEvents Validation="function(s, e) {
                                                                        validarValorCero(s, e, 3, &#39;Monto de Caseta&#39;);
                                                                        }"></ClientSideEvents>
                                                                </PropertiesSpinEdit>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" FilterMode="DisplayText"></Settings>
                                                            </dx:GridViewDataSpinEditColumn>
                                                            <dx:GridViewDataDateColumn Caption="Fecha Inicio" VisibleIndex="9" FieldName="Fecha_inicio" Name="Fecha_inicio" Width="100px">
                                                                <PropertiesDateEdit DisplayFormatString="d/MM/yyyy">
                                                                    <ValidationSettings>
                                                                        <RequiredField IsRequired="True" ErrorText="Es necesario seleccionar un Fecha de Inicio."></RequiredField>
                                                                    </ValidationSettings>
                                                                </PropertiesDateEdit>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataDateColumn Caption="Fecha Fin" VisibleIndex="10" FieldName="Fecha_fin" Name="Fecha_fin" Width="100px">
                                                                <PropertiesDateEdit DisplayFormatString="d/MM/yyyy">
                                                                    <ValidationSettings>
                                                                        <RequiredField IsRequired="True" ErrorText="Es necesario seleccionar una Fecha Fin."></RequiredField>
                                                                    </ValidationSettings>
                                                                </PropertiesDateEdit>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataComboBoxColumn Caption="Almacen Origen" VisibleIndex="11" FieldName="Id_almacen_origen_ebs12" Name="Id_almacen_origen_ebs12" Width="150px">
                                                                <PropertiesComboBox>
                                                                   <ClientSideEvents Validation="function(s, e) {
                                                                        validarValor(s, e, 0, &#39;Almacen de Origen&#39;);
                                                                        }"></ClientSideEvents>
                                                                </PropertiesComboBox>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False"></Settings>
                                                            </dx:GridViewDataComboBoxColumn>
                                                            <dx:GridViewDataCheckColumn FieldName="Backhaul" Name="Backhaul" Caption="Backhaul" VisibleIndex="12" Width="70px">
                                                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains"></Settings>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                            </dx:GridViewDataCheckColumn>
                                                           <dx:GridViewDataCheckColumn FieldName="Activo" Name="Activo" Caption="Activo" VisibleIndex="13" Width="50px">
                                                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains"></Settings>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                                            </dx:GridViewDataCheckColumn>
                                                        </Columns>
                                                        <Templates>
                                                            <StatusBar>
                                                                <div>
                                                                    <input type="button" id="btnCancelarGrid" runat="server" style="float: right; margin-left: 5px; margin-right: 35px;" class="btn custom-btn btn-primary" value="Cancelar" onclick="gridTarifasCliente.CancelEdit();" />
                                                                    <input type="button" id="btnGrabarGrid" runat="server" style="float: right; margin-left: 5px;" class="btn custom-btn btn-primary" value="Grabar" onclick="gridTarifasCliente.UpdateEdit();" />
                                                                </div>
                                                            </StatusBar>
                                                        </Templates>
                                                        <Settings VerticalScrollableHeight="225" VerticalScrollBarMode="Auto" ShowFilterRow="true" />
                                                        <SettingsPager AlwaysShowPager="true" PageSize="18" NumericButtonCount="5" />
                                                        <SettingsBehavior AllowFocusedRow="True" />
                                                        <SettingsEditing Mode="Batch" />
                                                        
                                                        <Styles>
                                                            <BatchEditModifiedCell BackColor="#75B486 " Font-Bold="true">
                                                            </BatchEditModifiedCell>
                                                        </Styles>
                                                    </dx:ASPxGridView>
                                                    <dx:ASPxGridViewExporter ID="gridTarifasExportar" GridViewID="gridTarifas" runat="server"></dx:ASPxGridViewExporter>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
