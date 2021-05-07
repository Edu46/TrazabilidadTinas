<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="BarcoViajeConsultar.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.BarcoViajeConsultar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
        $(function () {
            kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;
            let StringidBarco = "";
            let idViajeObtenido = ""

            let fechaInicio = "";
            let fechaFin = "";

            barcoViaje();
            desactivar();
            function desactivar() {
                //EL consultar desactiva los campos 
                $('#lblSeleccionarBarcoDD').prop("disabled", true);
                $('#lblEjercicioDD').prop("disabled", true);
                $('#lblViajeDD').prop("disabled", true);
                $('#lblSteamer').prop("disabled", true)
                $('#lblFolioIndentificador').prop("disabled", true);
                $('#lblViajePesca').prop("disabled", true);

                $('#lblRSA').prop("disabled", true);
                $("#lblFechaDescargaDP").attr("readonly", true);
                $("#lblFechaSalidaDP").attr("readonly", true);
                $("#lblFechaArriboDP").attr("readonly", true);

                $("#lblEstatus").prop("disabled", true);
                $("#lblDescargado").prop("disabled", true);
                $("#lblDescargaTinas").prop("disabled", true);
                $("#lblSubViaje").prop("disabled", true);
            }

            function barcoViaje() {
                //La funcion carga los datos del barco viaje seleccionado
                //Obteniendo los datos por el URL. 
                var URLactual = window.location;
                StringidBarco = URLactual.search.substr(59, 17);
                idViajeObtenido = URLactual.search.substr(91);
                
                fechaInicio = URLactual.search.substr(14, 10);
                fechaFin = URLactual.search.substr(34, 10);

                let params = JSON.stringify({
                    fechaInicio: URLactual.search.substr(14, 10),
                    fechaFin: URLactual.search.substr(34, 10),
                    idViaje : URLactual.search.substr(91)
                });
                $.ajax({
                    //Cargamos los datos del Barco viaje.
                    url: "BarcoViajeConsultar.aspx/ObtenerViaje",
                    contentType: "application/json;charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        if (result.d.barcosViajes != "") {
                            //Se llenan los campos con el viaje obtenido.
                            //Primera Columna
                            $("#lblSeleccionarBarcoDD").val(result.d.barcosViajes[0].barco);
                            $("#lblEjercicioDD").val(result.d.barcosViajes[0].ejercicio);
                            $("#lblViajeDD").val(result.d.barcosViajes[0].viaje);
                            $("#lblSteamer").val(result.d.barcosViajes[0].steamer);
                            $("#lblFolioIndentificador").val(result.d.barcosViajes[0].folioIdentificador);
                            //Segunda Columna
                            $("#lblRSA").val(result.d.barcosViajes[0].RSA);
                            $("#lblFechaDescargaDP").val(result.d.barcosViajes[0].fechaDescarga.substr(0, 10));
                            $("#lblFechaSalidaDP").val(result.d.barcosViajes[0].fechaSalida.substr(0, 10));
                            $("#lblFechaArriboDP").val(result.d.barcosViajes[0].fechaArribo.substr(0, 10));
                            if (result.d.barcosViajes[0].viajePesca === 0) {
                                $("#lblViajePesca").val(result.d.barcosViajes[0].viajePesca)
                            } else {
                                $("#lblViajePesca").val((result.d.barcosViajes[0].barco + result.d.barcosViajes[0].viaje + result.d.barcosViajes[0].ejercicio).replace(" ",""));
                            }
                            //Tercera Columna 
                            $("#lblEstatus").prop('checked', result.d.barcosViajes[0].activo);
                            $("#lblDescargado").prop('checked', result.d.barcosViajes[0].descargado);
                            $("#lblDescargaTinas").prop('checked', result.d.barcosViajes[0].descargaTinas);
                            $("#lblSubViaje").prop('checked', result.d.barcosViajes[0].subviaje);
                        } else {
                            //Primera Columna
                            $("#lblSeleccionarBarcoDD").val("No hay información para mostrar");
                            $("#lblEjercicioDD").val("No hay información para mostrar");
                            $("#lblViajeDD").val("No hay información para mostrar");
                            $("#lblSteamer").val("No hay información para mostrar");
                            $("#lblFolioIndentificador").val("No hay información para mostrar");
                            //Segunda Columna
                            $("#lblViajePesca").val("No hay información para mostrar");
                            $("#lblRSA").val("No hay información para mostrar");
                            $("#lblFechaDescargaDP").val("----/--/--");
                            $("#lblFechaSalidaDP").val("----/--/--");
                            $("#lblFechaArriboDP").val("----/--/--");
                            
                        }
                    },
                    error: function (result) {
                        options.error(result);
                        alert("Ha habido un error al cargar la información del recurso de integración");
                    }
                });
            }

            window.tanquesGrid = $("#tanquesGrid").kendoGrid({
                columns: [
                    {
                        field: "descripcionTanque",
                        title: "Tanque",
                        width: 100
      
                    },
                    {
                        field: "listaCertificaciones",
                        title: "Certificaciones",
                        width: 100
      
                    },
                    
                ],
                dataSource: {
                    sync: function () {
                        this.read();
                    },
                    pageSize:15,
                    schema: {
                        model: {
                            id: "idTanqueCertificacion",
                            fields: {
                                descripcionTanque: {
                                    type:"string",
                                },
                                listaCertificaciones: {
                                    type:"string",
                                },
                            }
                        }
                    },
                    transport: {
                        read: function (options) {
                            let params = JSON.stringify({
                                idViaje: idViajeObtenido
                            });
                            $.ajax({
                                //Obtene la relacion de tanques certificaciones.
                                url: "BarcoViajeCrear.aspx/ObtenerTanquesCertificaciones",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    if (!result.d) {
                                        alert("Ha habido un error al obtener la información");
                                    }
                                    options.success(result.d.tanquesCertificaciones);
                                },
                                error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al cargar la información del recurso de integración");
                                }
                           });
                        },
                    },
                },
                scrollable: true,
                resizable: true,
                pageable: {
                    messages: {
                        display: "{0}-{1} de {2} elementos",
                        empty: "No hay información para mostrar"
                    }
                },
                sortable: true,
                noRecords: true,
                messages: {
                    noRecords: "No hay información para mostrar"
                },
                filterable: FILTER_CONFIG,
                navigatable: true
            });

            window.productoGrid = $("#productoGrid").kendoGrid({
                columns: [
                    {
                        field: "descripcionProducto",
                        title: "Producto",
                        width: 100
      
                    },
                    {
                        field: "cantidad",
                        title: "Cantidad",
                        width: 100
      
                    },
                    
                ],
                dataSource: {
                    sync: function () {
                        this.read();
                    },
                    pageSize:15,
                    schema: {
                        model:{
                            id: "idViajeProducto",
                            fields: {
                                descripcionProducto: {
                                    type:"string",
                                },
                                cantidad: {
                                    type:"number",
                                },
                            }
                        }
                    },
                    transport: {
                        read: function (options) {
                            let params = JSON.stringify({
                                idViaje: idViajeObtenido
                            });
                            $.ajax({
                                //Obtener catalogo de viajes productos.
                                url: "BarcoViajeConsultar.aspx/ObtenerViajesProductos",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                data: params,
                               success: function (result) {
                                   if (result.d.viajesProductos != "") {
                                        options.success(result.d.viajesProductos);
                                   } else {
                                       options.success(result.d.viajesProductos == []);
                                   }
                               },
                               error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al cargar la información del recurso de integración");
                               }
                           });
                        },
                    },
                },
                scrollable: true,
                resizable: true,
                pageable: {
                    messages: {
                        display: "{0}-{1} de {2} elementos",
                        empty: "No hay información para mostrar"
                    }
                },
                sortable: true,
                noRecords: true,
                messages: {
                    noRecords: "No hay información para mostrar"
                },
                filterable: FILTER_CONFIG,
                navigatable: true
            });

            $("#btnSalir").click(function () {
                //El boton de salir retorna los parametros para cagar la consulta en la pantalla de consulta.
                window.location.replace("BarcoViaje.aspx?&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin + "&StringidBarco=" + StringidBarco);
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Detalle Barco - Viaje" id="hPage" />

    <section id="divCustomContentt" class="custom-content" runat="server">
                <div class="bs-component">
                    <div class="clearfix">
                        <div class="row">
                            <div class="col-sm-12">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary">
                                    <!-- /.box-header -->
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Datos barco - viaje</h3>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <!-- div-12 -->
                                            <div id="infoContainerData1" class="info-container-data">
                                                <div>
                                                    <div id="dataContainer1" class="data-list-container">
                                                        <div class="col-sm-12 column-container unique-container oneContainer-twoColumns">
                                                            <dl class="lista-descripcion">
                                                                <div class="each-row">
                                                                    <dt class="col-sm">
                                                                        Barco
                                                                    </dt>
                                                                    <dd id="barcoDD" class="col-sm">
                                                                        <input id="lblSeleccionarBarcoDD" type="text" class="form-control text-row"  aria-describedby="basic-addon2"/>
                                                                    </dd>
                                                                    <dt class="col-sm">
                                                                        Fecha Descarga
                                                                    </dt>
                                                                    <dd id="fechaDescargaDD" class="col-sm" >
                                                                        <input id="lblFechaDescargaDP" type="text" class="form-control text-row"  aria-describedby="basic-addon2"/>
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm">
                                                                        Ejercicio
                                                                    </dt>
                                                                    <dd id="ejercicioDD" class="col-sm">
                                                                        <input id="lblEjercicioDD" type="text" class="form-control text-row"/>
                                                                    </dd>
                                                                    <dt class="col-sm">
                                                                        Fecha Salida
                                                                    </dt>
                                                                    <dd id="fechaSalidaDD" class="col-sm">
                                                                        <input id="lblFechaSalidaDP" type="text" class="form-control text-row"  aria-describedby="basic-addon2"/>
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm">
                                                                        Viaje
                                                                    </dt>
                                                                    <dd id="viajeDD" class="col-sm">
                                                                        <input id="lblViajeDD" type="text" class="form-control text-row"/>
                                                                    </dd>
                                                                    <dt class="col-sm">
                                                                        Fecha Arribo
                                                                    </dt>
                                                                    <dd id="fechaArriboDD" class="col-sm">
                                                                        <input id="lblFechaArriboDP" type="text" class="form-control text-row"/>
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm">
                                                                        Steamer
                                                                    </dt>
                                                                    <dd id="steamerDD" class="col-sm">
                                                                        <input id="lblSteamer" type="text" class="form-control text-row"/>
                                                                    </dd>
                                                                    <dt class="col-sm">
                                                                        Viaje de pesca
                                                                    </dt>
                                                                    <dd id="viajePescaDD" class="col-sm">
                                                                        <input id="lblViajePesca" type="text" class="form-control text-row"/>
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm">
                                                                        Folio Identificador
                                                                    </dt>
                                                                    <dd id="folioIdentificadorDD" class="col-sm">
                                                                        <input id="lblFolioIndentificador" type="text" class="form-control text-row"/>
                                                                    </dd>
                                                                    <dt class="col-sm">
                                                                        RSA
                                                                    </dt>
                                                                    <dd id="rsaDD" class="col-sm">
                                                                        <input id="lblRSA" type="text" class="form-control text-row"/>
                                                                    </dd>
                                                                </div>
                                                            </dl>
                                                            <dl class="lista-descripcion">
                                                                <div class="each-row">
                                                                    <dt class="col-sm">
                                                                        Estatus
                                                                    </dt>
                                                                    <dd id="estatusDD" class="col-sm">
                                                                        <input id="lblEstatus" type="checkbox" class="k-checkbox"/>
                                                                        <label for="lblEstatus" class="k-checkbox-label"></label>
                                                                    </dd>
                                                                    <dt class="col-sm">
                                                                        <!--titulo-->
                                                                    </dt>
                                                                    <dd id="t1DD" class="col-sm">
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm">
                                                                        Descargado
                                                                    </dt>
                                                                    <dd id="descargadoDD" class="col-sm">
                                                                        <input id="lblDescargado" type="checkbox" class="k-checkbox" />
                                                                        <label for="lblDescargado" class="k-checkbox-label"></label>
                                                                    </dd>
                                                                    <dt class="col-sm">
                                                                        <!--titulo-->
                                                                    </dt>
                                                                    <dd id="t2DD" class="col-sm">
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm">
                                                                        Descarga con tinas
                                                                    </dt>
                                                                    <dd id="tinaSensorDD" class="col-sm">
                                                                        <input id="lblDescargaTinas" type="checkbox" class="k-checkbox" />
                                                                        <label for="lblDescargaTinas" class="k-checkbox-label"></label>
                                                                    </dd>
                                                                    <dt class="col-sm">
                                                                        <!--titulo-->
                                                                    </dt>
                                                                    <dd id="t3DD" class="col-sm">
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm">
                                                                        Sub-Viaje
                                                                    </dt>
                                                                    <dd id="subviajeDD" class="col-sm">
                                                                        <input id="lblSubViaje" type="checkbox" class="k-checkbox" />
                                                                        <label for="lblSubViaje" class="k-checkbox-label"></label>
                                                                    </dd>
                                                                    <dt class="col-sm">
                                                                        <!--titulo-->
                                                                    </dt>
                                                                    <dd id="t4DD" class="col-sm">
                                                                    </dd>
                                                                </div>
                                                            </dl>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                           <div class="col-sm-12">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary">
                                    <!-- /.box-header -->
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Tanques</h3>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div id="tanquesGrid"> 
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                 </div>
                             </div>
                          </div>
                        <br />
                        <div class="row">
                           <div class="col-sm-12">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary">
                                    <!-- /.box-header -->
                                    <div class="box-header with-border">
                                        <h3 class="box-title">Producto</h3>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div id="productoGrid"> 
                                            </div>
                                                <div class="col-sm-12">
                                                    <div class="four-button-contain">
                                                        <a id="btnSalir" class="k-primary k-button">Volver</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                 </div>
                             </div>
                          </div>
                </div>
            </div>
        <style>
            /*Checked checkbox*/
            .k-checkbox:checked + .k-checkbox-label:before {
            background:  white;
            color: #3c8dbc;
            }

        </style>
    </section>
</asp:Content>
