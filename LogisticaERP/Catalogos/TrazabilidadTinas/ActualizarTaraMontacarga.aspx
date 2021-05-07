<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="ActualizarTaraMontacarga.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.ActualizarTaraMontacarga" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
    $(function () {
        kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;

        let idMon = 0;
        let idEtiquetaMon = 0;
        let valCoincide = false;
        let idMontacarga = 1;
        let bascula = [];
        let tiempo = [];
        let montacargaTara = []; 
        let taraObtenida = 0;
        let toleranciaSeleccionada = 0;
        let tiempoSeleccionado = 0;
        let idBascula = 0909;
        let idTransaccionProveedorConsultado = 0;

        let ddlBasculas = $('#basculasDDL').kendoDropDownList({
            enable: true,
            dataValueField: "idBascula",
            dataTextField: "descripcion",
            dataSource: {
                transport: {
                    read: function (options) {
                        let param = JSON.stringify({
                            activo: true
                        });
                        $.ajax({
                            //Cargar DDL de las Basculas ara mostrar en el primer modal al entrar a la pantalla. 
                            url: "ActualizarTaraMontacarga.aspx/ObtenerBasculas",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            data:param,
                            success: function (result) {
                                if (result.d != null) {
                                    for (let i = 0; i < result.d.basculas.length; i++) {
                                        bascula[i] = result.d.basculas[i];
                                    }
                                    options.success(result.d.basculas);
                                } else {
                                    options.success([]);
                                }
                            },
                            error: function (result) {
                                options.error(result);
                                alert("Ha habido un error al cargar la información del recurso de integración");
                            }
                        });
                    }
                }
            },
        });

        function mostrarModalSeleccionBascula(e) {
			prevenirCerrarModalSeleccionBascula();
		};

		function prevenirCerrarModalSeleccionBascula() {
			$('#modalSeleccionBascula').modal({
				backdrop: 'static',
				keyboard: false
			});
        }

        function registrarTaraBtn(e) {
            e.preventDefault();
            let dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            idEtiquetaMon = dataItem.idEtiquetaMontacargas;
            idMon = dataItem.idMontacargas
            consultarNumeroTransaccionProveedor();
        }

        //Al ingresar a la pantalla se muestra. 
        mostrarModalSeleccionBascula();

        $("#confirmarBasculaBtn").kendoButton({
            //Boton confirmar para la bascula seleccionada.
            click: function (e) {
                if (ddlBasculas.val()) {
                    idBascula = ddlBasculas.val();
                    let taraMontacargaGrid = $('#taraMontacarga').data("kendoGrid");
                    taraMontacargaGrid.dataSource.read();
                    $('#modalSeleccionBascula').modal('hide');
                } else {
                    alert("Seleccione una báscula de la lista para continuar");
                }
				e.preventDefault();
			}
        });
        

        function consultarNumeroTransaccionProveedor() {
            idTransaccionProveedorConsultado = 0;
            let params = JSON.stringify({
                // Asignar el idBascula correspondiente para efectuar la consulta
                idBascula: ddlBasculas.val()
            });
            $.ajax({
                //Se consulta del proveedor el numero de transaccion tomada del servicio.
                url: "ActualizarTaraMontacarga.aspx/ConsultarTransaccionProveedor",
                contentType: "application/json;charset=utf-8",
                type: "POST",
                data: params,
                success: function (result) {
                    if (result.d) {
                        if (result.d.transaccion) {
                            idTransaccionProveedorConsultado = result.d.transaccion.idTransaccionBascula;
                            taraObtenida = result.d.transaccion.peso;
                            // Validar los datos de la transacción de proveedor
                            mostrarDetalleTransaccionProveedor(result.d.transaccion);
                        } else {
                            alert("Sin datos de transacción del proveedor")
                        }
                    } else {
                        alert("Ha habido un error al consultar los datos de la transacción del proveedor");
                    }
                },
                error: function (result) {
					alert("Ha habido un error al cargar la información del recurso de integración");
				}
            });
        }

        function mostrarDetalleTransaccionProveedor(transaccion) {
            $("#pesoCaptadoTransaccionDD").text(transaccion.peso || transaccion.peso == 0 ? (transaccion.peso + " Kg.") : "N/A");
            $("#horaTransaccionDD").text(transaccion.fechaTransaccion ? kendo.toString(kendo.parseDate(transaccion.fechaTransaccion), 'hh:mm tt') : "Sin información");
            $("#pesoCaptadoDD").text(transaccion.peso || transaccion.peso == 0 ? transaccion.peso : "Sin informacion");
            mostrarModalTransaccion();
        }

        function mostrarModalTransaccion(e) {
			prevenirCerrarModalTransaccion();
		};

		function prevenirCerrarModalTransaccion() {
			$('#modalTransaccion').modal({
				backdrop: 'static',
				keyboard: false
			});
        }

        function mostrarModalTransaccionNoCoincide(e) {
			prevenirCerrarModalTransaccionNoCoincide();
		};

        function prevenirCerrarModalTransaccionNoCoincide() {
			$('#modalTransaccionNoCoincide').modal({
				backdrop: 'static',
				keyboard: false
			});
        }

        $("#confirmarBtn").kendoButton({
            click: function (e) {
                $('#modalTransaccion').modal('hide');
                // Consultar el tiempo restante del montacargas
                let params = JSON.stringify({
                    idBascula: ddlBasculas.val()
                });
                $.ajax({
                    //Se consulta el peso captado del montacargas. 
                    url: "ActualizarTaraMontacarga.aspx/ConsultarTarajeMontacargas",
                    contentType: "application/json;charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        if (result.d) {
                            if (result.d.taraje) {
                                evaluarDatosTaraje(result.d.taraje);
                            } else {
                                alert("Sin datos del taraje del montacargas")
                            }
                        } else {
                            alert("Ha habido un error al consultar los datos del taraje del montacargas");
                        }
                    },
                    error: function (result) {
                        window.kendo.ui.progress($("#dataContainer2"), false);
						alert("Ha habido un error al cargar la información del recurso de integración");
					}
                });
				e.preventDefault();
			}
        });

        function evaluarDatosTaraje(taraje) {
            $("#montacargasExcedidoDD").text(taraje.montacargas ? taraje.montacargas : "Sin descripción");
            // Actualizar el registro de la transacción del proveedor para descartarlo
            valCoincide = true;
            actualizarTaraMontacarga();
            actualizarRegistroTransaccionProveedor();
        }

        $("#noCoincideBtn").kendoButton({
            click: function (e) {
                e.preventDefault();
                // Solicitar confirmación
                let respuesta = confirm("Confirmar para continuar");
                if (respuesta == true) {
                    // Actualizar el registro de la transacción del proveedor para descartarlo
                    valCoincide = false;
                    actualizarRegistroTransaccionProveedor();
                    $('#modalTransaccion').modal('hide');
                    mostrarModalTransaccionNoCoincide();
                    // Quitar valores de peso de los componentes
                    $("#pesoNetoDD").text("0 Kg.");
                    $("#pesoCaptadoDD").text("No hay datos de peso");
                } 
				e.preventDefault();
			}
        });

        $("#aceptarBtn").kendoButton({
            click: function (e) {
                $('#modalTransaccionNoCoincide').modal('hide');
				e.preventDefault();
			}
        });

        function actualizarTaraMontacarga() {
            let taraMontacargaGrid = $('#taraMontacarga').data("kendoGrid");
            let datosTaraMontacarga = {
                idEtiquetaMontacargas: idEtiquetaMon,
                idMontacargas: idMon,
                tara: taraObtenida
            }
            let params = JSON.stringify({ actualizarTara: datosTaraMontacarga });
            $.ajax({
                url: "ActualizarTaraMontacarga.aspx/ActualizarTara",
                contentType: "application/json;charset=utf-8",
                type: "POST",
                data: params,
                success: function (result) {
                    switch (result.d) {
                        case 200:
                            taraMontacargaGrid.dataSource.read();
                            break;
                        case 304:
                            taraMontacargaGrid.dataSource.read();
                            alert("ALERTA NO ACTUALIZADA");
							break;
						default:
                            alert("Ha habido un error al intentar actualizar el registro de transacción del proveedor");
                            taraMontacargaGrid.dataSource.read();
							break;
					}
                },
                error: function (result) {
					alert("Ha habido un error al ejecutar la operación en el recurso de integración");
				}
            });
        }

        function actualizarRegistroTransaccionProveedor() {
            let param = {
                idBascula: ddlBasculas.val(),
                idTransaccionBascula: idTransaccionProveedorConsultado,
                coincide : valCoincide
            };
            params = JSON.stringify({ transaccionPesoDetalle: param });
            $.ajax({
                //Si coincide es true si no coincide es false.
                //false mata el registro del proveedor.
                //true guarda el registro captado del proveedor en la bd y actualiza el ultimo registro del proveedor. 
                url: "ActualizarTaraMontacarga.aspx/ActualizarTransaccionProveedor",
                contentType: "application/json;charset=utf-8",
                type: "POST",
                data: params,
                success: function (result) {
                    switch (result.d) {
                        case 200:
                            //
							break;
						default:
							alert("Ha habido un error al intentar actualizar el registro de transacción del proveedor");
							break;
					}
                },
                error: function (result) {
					alert("Ha habido un error al ejecutar la operación en el recurso de integración");
				}
            });
        }

        tiempo = [
            { id: 1, text: "45 min", tiempo: 45},
            { id: 2, text: "60 min", tiempo: 60},
            { id: 3, text: "1.5 h", tiempo: 90},
            { id: 4, text: "2.5 h", tiempo: 150},
            { id: 5, text: "3 h", tiempo: 180},
            { id: 6, text: "3.5 h", tiempo: 210},
            { id: 7, text: "4 h", tiempo: 240},
            { id: 8, text: "4.5 h", tiempo: 270},
            { id: 9, text: "5 h", tiempo: 300},
        ];

        let tolerancia = [
            { id: 1, text: "15 min", tolerancia: 15 },
            { id: 2, text: "20 min", tolerancia: 20},
            { id: 3, text: "25 min", tolerancia: 25},
            { id: 4, text: "30 min", tolerancia: 30},
        ];

        $('#tiempoDD').kendoDropDownList({
            enable: true,
            dataValueField: "id",
            dataTextField: "text",
            dataSource: {
                data: tiempo,
            },
            optionLabel: "Selecciona el tiempo",
            change: dropdownlistTiempo,
            messages: {
              noData: "No hay datos para mostrar"
            },
        });

        function dropdownlistTiempo() {
            let dropdownlist = $("#tiempoDD").data("kendoDropDownList");
            if (dropdownlist.dataItem().tiempo != null) {
                tiempoSeleccionado = dropdownlist.dataItem().tiempo;
            } else {
                tiempoSeleccionado = 0;
            }
            
        }

        $('#toleranciaDD').kendoDropDownList({
            enable: true,
            dataValueField: "id",
            dataTextField: "text",
            dataSource: {
                data: tolerancia,
            },
            optionLabel: "Selecciona la tolerancia",
            change: dropdownlistTolerancia,
            messages: {
              noData: "No hay datos para mostrar"
            },
        });

        function dropdownlistTolerancia() {
            let dropdownlist = $("#toleranciaDD").data("kendoDropDownList");
            if (dropdownlist.dataItem().tolerancia != null) {
                toleranciaSeleccionada = dropdownlist.dataItem().tolerancia;
            } else {
                toleranciaSeleccionada = 0;
            }
            
        }

        $('#MontacargaDD').kendoDropDownList({
            enable: true,
            dataValueField: "idTipoMontacargas",
            dataTextField: "descripcion",
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            //Se consume del catalogo el tiempo de montacarga.
                            url: "ActualizarTaraMontacarga.aspx/CargarDDLTipoMontacarga",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            success: function (result) {
                                if (result.d != null) {
                                        options.success(result.d.tiposMontacargas);
                                }else {
                                    alert("Error al cargar la información");
                                }
                                
                            }
                        });
                    }
                }
            },
            change: function (e) {
                //Cambia el tiempo dependiente del timpo de montacarga. 
                idMontacarga = this.value();
                let taraMontacargaGrid = $('#taraMontacarga').data("kendoGrid");
                taraMontacargaGrid.dataSource.read();
                if (idMontacarga == 1 || idMontacarga == 3) {
                    let dataSource = new kendo.data.DataSource({
                        data: [
                            { id: 1, text: "45 min", tiempo: 45},
                            { id: 2, text: "60 min", tiempo: 60},
                            { id: 3, text: "1.5 h", tiempo: 90},
                            { id: 4, text: "2.5 h", tiempo: 150},
                            { id: 5, text: "3 h", tiempo: 180},
                            { id: 6, text: "3.5 h", tiempo: 210},
                            { id: 7, text: "4 h", tiempo: 240},
                            { id: 8, text: "4.5 h", tiempo: 270},
                            { id: 9, text: "5 h", tiempo: 300},
                        ]
                    });
                    let dropdownlist = $("#tiempoDD").data("kendoDropDownList");
                    dropdownlist.setDataSource(dataSource);
                } else if (idMontacarga == 2) {
                    let dataSource = new kendo.data.DataSource({
                      data:[{ id: 1, text: "4.5 h", tiempo: 270 },{ id: 2, text: "5 h", tiempo: 300 }],
                    });
                    let dropdownlist = $("#tiempoDD").data("kendoDropDownList");
                    dropdownlist.setDataSource(dataSource);
                }
            },
            //optionLabel:"Selecciona el Montacarga",
        });

        function enviarAlertaBtn(e) {
            let dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            let datosMontacarga = {
                idEtiquetaMontacargas: dataItem.idEtiquetaMontacargas,
            }
            let params = JSON.stringify({ taraMontacargaDetalle: datosMontacarga });
            $.ajax({
                //Se envia la alerta al montacarga tomado del grid.
                url: "ActualizarTaraMontacarga.aspx/EnviarAlertaMontacarga",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                data: params,
                success: function (result) {
                    switch (result.d) {
                        case 200:
                            alert("Se ha enviado la alerta");
                            let taraMontacargaGrid = $('#taraMontacarga').data("kendoGrid");
                            taraMontacargaGrid.dataSource.read();
                            break;
                        case 204:
                            alert("Error al identificar el elemento a crear/actualizar");
                            break;
                        case 400:
                            alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                            break;
                        case 405:
                            alert("La sesión ha expirado, la operación no puede ser completada");
                            break;
                        case 409:
                            alert("La descripción ingresada ya existe");
                            break;
                    }
                },
                error: function (result) {
                    options.error(result);
                    alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                }
            });
        }

        $('#aplicarBtn').on('click', function (e) {
            //Cambiar tiempo de las alertas
            if (tiempoSeleccionado == 0 || toleranciaSeleccionada == 0) {
                alert("El tiempo o la tolerancia no han sido seleccionados");
            } else {
                let datosTiempo = {
                    idTipoMontacargas: idMontacarga,
                    tiempoMaximo: tiempoSeleccionado,
                    tolerancia: toleranciaSeleccionada
                }
                let params = JSON.stringify({ taraMontacargaDetalle: datosTiempo });
                $.ajax({
                    //Se cambia el tiempo y tolerancia seleccionado. 
                    url: "ActualizarTaramontacarga.aspx/ActualizarTiempo",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                alert("Se han actualizado los parámetros con éxito");
                                let taraMontacargaGrid = $('#taraMontacarga').data("kendoGrid");
                                taraMontacargaGrid.dataSource.read();
                                break;
                            case 204:
                                alert("Error al identificar el elemento a crear/actualizar");
                                break;
                            case 400:
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 409:
                                alert("La descripción ingresada ya existe");
                                break;
                        }
                    },
                    error: function (result) {
                        options.error(result);
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                    }
                });
            }
        });

        window.taraMontacarga = $("#taraMontacarga").kendoGrid({
            columns: [
                {
                    field: "descripcionTipoMontacargas",
                    title: "Tipo",
                    width: 100
      
                },
                {
                    field: "montacargasDescripcion",
                    title: "Montacargas",
                    width: 100
      
                },
                {
                    field: "tara",
                    title: "Tara (Kg)",
                    format: "{0:n2}",
                    width: 100
      
                },
                {
                    field: "tiempo",
                    title: "Tiempo Restante (Min)",
                    format: "{0:n0}",
                    width: 100,
                },
                {
                command: [
                    {
                        click: enviarAlertaBtn,
                        text: "Enviar alerta",
                    },
                    {
                        click: registrarTaraBtn,
                        text: "Rregistrar tara",
                    },
                ],
                    title: "",
                    width: 140
                }
                    
            ],
            dataBound: function (e) {
                // Aplicar una clase css para el row si el timepo restante es menor a la toleancia

                let columnIndexViaje = this.wrapper.find(".k-grid-header [data-field=" + "tiempo" + "]").index();
                let columnIndex = this.wrapper.find(".k-grid-header [data-field=" + "tiempo" + "]").index()+ 1;

                let rows = e.sender.tbody.children();

                for (let j = 0; j <= rows.length - 1; j++) {
                    let row = $(rows[j]);
                    let dataItem = e.sender.dataItem(row);
                    let enBascula = dataItem.enBascula;
                    let enAlerta = dataItem.enAlerta;
                    if (dataItem.tiempo <= dataItem.tolerancia) {
                        var cell = row.children().eq(columnIndexViaje);
                        cell.css("color", "red");
                    }

                    //Si alerta se envía en 1 enAlerta, se oculata el botón de tarar montacargas, sí el campo enBascula viene en 1
                    if (enBascula == 0 && enAlerta == 0) {//Se muesta botón de alerta
                        var cell = row.children().eq(columnIndex);
                        cell.children(':eq(0)').show();
                        cell.children(':eq(1)').hide();
                    } else if (enBascula == 1 && enAlerta == 1) { //Se muestra botón de Tarar pero no el de Alerta
                        var cell = row.children().eq(columnIndex);
                        cell.children(':eq(0)').hide();
                        cell.children(':eq(1)').show();
                    } else if (enBascula == 1 && enAlerta == 0) {//Se muestra botón de Tarar y tambien el de Alerta
                        var cell = row.children().eq(columnIndex);
                        cell.children(':eq(0)').show();
                        cell.children(':eq(1)').show();
                    } else if (enBascula == 0 && enAlerta == 1) {//No se muestra nada 
                        var cell = row.children().eq(columnIndex);
                        cell.children(':eq(0)').hide();
                        cell.children(':eq(1)').hide();
                    }
                }
            },
            dataSource: {
                sync: function () {
                    this.read();
                },
                schema: {
                    model: {
                        id: "idTaraMontacarga",
                        fields: {
                            "descripcionTipoMontacargas": {
                                type: "string",
                            },
                            "montacargasDescripcion": {
                                type: "string",
                            },
                            "tara": {
                                type: "number",
                            },
                            "tiempo": {
                                type: "number",
                            },
                            "idEtiquetaMontacargas": {
                                type: "number"
                            },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        let ddlTiempo = $("#tiempoDD").data("kendoDropDownList");
                        let ddlTolerancia = $("#toleranciaDD").data("kendoDropDownList");
                        let params = JSON.stringify({
                            idTipoMontacargas: idMontacarga,
                            idBascula: idBascula
                        });
                        $.ajax({
                            //Se consulta la tara del montacargas y bascula seleccionado. 
                            url: "ActualizarTaraMontacarga.aspx/ObtenerTaraMontacarga",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            data: params,
                            success: function (result) {
                                if (result.d != null) {
                                    for (let m = 0; m <= result.d.montacargasAlertas.length; m++) {
                                        //Se guarda en una variable los datos de las basculas para su manipulación.
                                        montacargaTara[m] = result.d.montacargasAlertas[m];
                                    }
                                    //Al obtener la tara del montacarga se obtiene el tiempo y la tolerancia para mostrarlo en el respectivo DDL.
                                    if (result.d.montacargasAlertas.length > 0) {
                                        ddlTiempo.setOptions({optionLabel: String(result.d.montacargasAlertas[0].tiempoMaximo)});
                                        ddlTolerancia.setOptions({optionLabel: String(result.d.montacargasAlertas[0].tolerancia)});
                                        for (let m = 0; m <= result.d.montacargasAlertas.length-1; m++) {
                                            result.d.montacargasAlertas[m].tiempo = result.d.montacargasAlertas[m].enAlerta == 0 ? result.d.montacargasAlertas[m].tiempoRestante : result.d.montacargasAlertas[m].tiempoRestanteCalculado;
                                        }   
                                    } else {
                                        ddlTiempo.setOptions({ optionLabel: "Selecciona el tiempo" });
                                        ddlTolerancia.setOptions({ optionLabel: "Selecciona el tolerancia" });
                                    }
                                    options.success(result.d.montacargasAlertas);
                                } else {
                                    options.success(result.d == []);
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
            editable: "inline",
            pageable: {
                messages: {
                    display: "{2} elementos",
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

    });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Actualizar tara de un montacargas" id="hPage" />
    <section id="divCustomContent" class="custom-content" runat="server">
                <div class="bs-component">
                    <div class="clearfix">
                        <div class="row">
                            <div class="col-sm-12">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary">
                                    <!-- /.box-header -->
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="col-sm-12">
                                        <div class="col-sm-3">
                                            <label for="lblSeleccionarMontacargasDDL" class="control-label-top">Seleccionar tipo </label>
                                            <input id="MontacargaDD"/>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="lblSeleccionarTiempoDDL" class="control-label-top">Seleccionar tiempo</label>
                                            <input id="tiempoDD"/>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="lblSeleccionarToleranciaDDL" class="control-label-top">Seleccionar tolerancia </label>
                                            <input id="toleranciaDD"/>
                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-6">
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
                                        <h3 class="box-title">Actualizar tara de un montacargas</h3>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div id="taraMontacarga"> 
                                                </div>
                                                <div class="col-sm-12">
                                                <div class="four-button-contain">
                                                    <a id="aplicarBtn" class="k-primary k-button">Aplicar</a>
                                                    <!--<a id="cancelarBtn" class="k-primary k-button">Cancelar</a>--->
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
        </section>

        <!-- Modal Selección de Báscula -->
        <div class="modal fade k-custom-modal-tm" id="modalSeleccionBascula" role="dialog">
            <div class="modal-dialog emx-custom-modal">
                <!-- Modal content-->
                <div class="modal-content modal-center">
                    <div class="modal-header">
                        <h4 class="modal-title">Seleccionar báscula</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-sm-6">
                                <input id="basculasDDL" />
                            </div>
                        </div>
                    </div>
                     <div class="modal-footer">
                         <div class="row">
                            <button id="confirmarBasculaBtn" class="k-button k-primary">Confirmar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Modal Transacción Proveedor -->
        <div class="modal fade k-custom-modal-tm" id="modalTransaccion" role="dialog">
            <div class="modal-dialog emx-custom-modal">
                <!-- Modal content-->
                <div class="modal-content modal-center">
                    <div class="modal-header">
                        <h4 class="modal-title">Confirmar datos de la transacción</h4>
                    </div>
                    <div class="modal-body">
                        <div id="modalForm">
                            <div class="row">
                                <div id="infoContainerData3" class="info-container-data">
                                    <div id="dataContainer3" class="data-list-container ">
                                        <div class="col-sm-12 column-container unique-container oneContainer-twoColumns">
                                            <dl class="lista-descripcion">
                                                <div style="background-color: transparent;" class="identify-row">
                                                    <span>
                                                        <dt class="col-sm">
                                                                Peso Captado:
                                                        </dt>
                                                        <h2>
                                                            <dd id="pesoCaptadoTransaccionDD" class="col-sm">
                                                                1000 Kg.
                                                            </dd>
                                                        </h2>
                                                    </span>
                                                </div>
                                            </dl>
                                            <dl class="lista-descripcion">
                                                <dt class="col-sm">
                                                    Hora de Registro:
                                                </dt>
                                                <dd id="horaTransaccionDD" class="col-sm">
                                                    12:29 PM
                                                </dd>
                                            </dl>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                     <div class="modal-footer">
                         <div class="row">
                            <button id="confirmarBtn" class="k-button k-primary">Confirmar</button>
                            <button id="noCoincideBtn" class="k-button k-primary">No Coincide</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    <!-- Modal Transacción Proveedor No Coincide -->
        <div class="modal fade k-custom-modal-tm" id="modalTransaccionNoCoincide" role="dialog">
            <div class="modal-dialog emx-custom-modal">
                <!-- Modal content-->
                <div class="modal-content modal-center k-warning">
                    <div class="modal-header">
                        <h4 style="color: red;" class="modal-title">Importante</h4>
                    </div>
                    <div class="modal-body">
                        <span class="k-icon k-i-warning"></span>
                        <div class="row">
                            Es necesario reportar este caso al departamento de sistemas debido a la incongruencia con el número de la transacción del proveedor
                        </div>
                    </div>
                     <div class="modal-footer">
                         <div class="row">
                            <button id="aceptarBtn" class="k-button k-primary">Aceptar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    <style>
        .k-grid tbody .k-button {
            min-width: 115px;
        }
    </style>
</asp:Content>
