<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="AsignarUbicacionFinalTina.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.AsignarUbicacionFinalTina" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(function () {
            var idTransaccionProveedorConsultado = 0;
            var datosBasculaActual = [];
            var pesoCaptadoProveedor = 0;
            kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;

            var ddlBasculas = $('#basculasDDL').kendoDropDownList({
                enable: true,
                dataValueField: "idBascula",
                dataTextField: "descripcion",
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                url: "AsignarUbicacionFinalTina.aspx/ObtenerBasculasActivas",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d) {
                                        if (!result.d.basculas.length) {
                                            alert("No se encontraron básculas");
                                        }
                                        options.success(result.d.basculas);
                                    } else {
                                        options.success([]);
                                        alert("Ha habido un error al consultar las básculas, revise los logs para mas detalles");
                                    }
                                },
                                error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al consultar las básculas, revise los logs para mas detalles");
                                }
                            });
                        }
                    }
                }
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

            $("#confirmarBasculaBtn").kendoButton({
                click: function (e) {
                    if (ddlBasculas.val()) {
                        $('#modalSeleccionBascula').modal('hide');
                        var dropdownlist = $("#basculasDDL").data("kendoDropDownList");
                        $('#basculaSeleccionadaTxt').val(dropdownlist.text());
                        $("#aplicarBtn").prop("disabled", true);
                    } else {
                        alert("Seleccione una báscula de la lista para continuar");
                    }
					e.preventDefault();
				}
            });

            mostrarModalSeleccionBascula();

            $("#actualizarBtn").kendoButton({
                click: function (e) {
                    // Obtener datos de la bascula actual
                    var params = JSON.stringify({
                        idBascula: ddlBasculas.val()
                    });
                    $.ajax({
                        url: "AsignarUbicacionFinalTina.aspx/ObtenerDatosBasculaActual",
                        contentType: "application/json;charset=utf-8",
                        type: "POST",
                        data: params,
                        beforeSend: function () {
                            window.kendo.ui.progress($("#dataContainer1"), true);
                            window.kendo.ui.progress($("#dataContainer2"), true);
                        },
                        success: function (result) {
                            window.kendo.ui.progress($("#dataContainer1"), false);
                            window.kendo.ui.progress($("#dataContainer2"), false);
                            if (result.d) {
                                if (result.d.tinas) {
                                    // validar objeto vacio
                                    if (!result.d.tinas.idEtiquetaTina && !result.d.tinas.idTina && !result.d.tinas.idTanque) {
                                        alert("No se encontraron datos de la báscula actual");
                                    } else {
                                        datosBasculaActual = result.d.tinas;
                                        // Mostrar la información en los controles
                                        mostrarDatosBasculaActual(result.d.tinas);
                                    }
                                } else {
                                    alert("No se encontraron datos de la báscula actual");
                                    limpiarComponentes();
                                }
                            } else {
                                alert("Ha habido un error al consultar los datos actuales de la báscula");
                                limpiarComponentes();
                            }
                        },
                        error: function (result) {
                            limpiarComponentes();
                            window.kendo.ui.progress($("#dataContainer1"), false);
                            window.kendo.ui.progress($("#dataContainer2"), false);
							alert("Ha habido un error al cargar la información del recurso de integración");
						}
                    });
                    e.preventDefault();
                }
            });

            function mostrarDatosBasculaActual(datos) {
                // Concatenar las certificaciones
                var certificaciones = "";
                if (datos.certificaciones.length) {
                    for (var i = 0; i <= datos.certificaciones.length - 1; i++) {
                        if (i == 0) {
                            certificaciones = datos.certificaciones[i].certificacion;
                        }else{
                            certificaciones += (", " + datos.certificaciones[i].certificacion);
                        }
                    }
                } else {
                    certificaciones = "Sin información";
                }
                $("#codigoTinaDD").text(datos.tina ? datos.tina : "Sin información");
                $("#taraDD").text(datos.taraTina || datos.taraTina == 0 ? datos.taraTina : "Sin información");
                $("#rsaDD").text(datos.RSA || datos.RSA == 0 ? datos.RSA : "Sin información");
                $("#barcoDD").text(datos.barco ? datos.barco : "Sin información");
                $("#viajeDD").text(datos.viaje ? datos.viaje : "Sin información");
                $("#ejercicioDD").text(datos.ejercicio ? datos.ejercicio : "Sin información");
                $("#claveDD").text(datos.clave ? datos.clave : "Sin información");
                $("#tallaDD").text(datos.talla ? datos.talla : "Sin información");
                $("#calidadDD").text(datos.nombreCalidad ? datos.nombreCalidad : "Sin información");
                $("#desviacionCalidadDD").text(datos.desviacionCalidad ? datos.desviacionCalidad : "Sin información");
                $("#sensorialDD").text(datos.sensorial ? (datos.sensorial = false ? "NO" : "SI") : "Sin información");
                $("#nivelMuestreoDD").text(datos.nivel ? datos.nivel : "Sin información");
                $("#tinaSensorDD").text(datos.tinaConSensor ? (datos.tinaConSensor = false ? "NO" : "SI") : "Sin información");
                $("#tanqueDD").text(datos.tanque ? datos.tanque : "Sin información");
                $("#certificacionesDD").text(certificaciones);
                $("#temperaturaDD").text(datos.temperatura || datos.temperatura == 0 ? datos.temperatura : "Sin información");
                $("#montacargasDD").text(datos.montacargas ? datos.montacargas : "Sin información");
                $("#taraMontacargasDD").text(datos.taraMontacargas || datos.taraMontacargas == 0 ? datos.taraMontacargas : "Sin información");
                $("#taraTinaDD").text(datos.taraTina || datos.taraTina == 0 ? datos.taraTina : "Sin información");
                // Consultar el numero de transacción del pesado de producto de proveedor
                var pesosRegistrados = {
                    taraMontacargas: datos.taraMontacargas,
                    taraTina: datos.taraTina
                }
                consultarNumeroTransaccionProveedor(pesosRegistrados);
            }

            function consultarNumeroTransaccionProveedor(pesosRegistrados) {
                idTransaccionProveedorConsultado = 0;
                var params = JSON.stringify({
                    // Asignar el idBascula correspondiente para efectuar la consulta
                    idBascula: ddlBasculas.val()
                });
                $.ajax({
                    url: "AsignarUbicacionFinalTina.aspx/ConsultarTransaccionProveedor",
                    contentType: "application/json;charset=utf-8",
                    type: "POST",
                    data: params,
                    beforeSend: function () {
                        window.kendo.ui.progress($("#dataContainer1"), true);
                        window.kendo.ui.progress($("#dataContainer2"), true);
                    },
                    success: function (result) {
                        window.kendo.ui.progress($("#dataContainer1"), false);
                        window.kendo.ui.progress($("#dataContainer2"), false);
                        if (result.d) {
                            if (result.d.transaccion) {
                                idTransaccionProveedorConsultado = result.d.transaccion.idTransaccionBascula;
                                pesoCaptadoProveedor = result.d.transaccion.peso;
                                // Validar los datos de la transacción de proveedor
                                mostrarDetalleTransaccionProveedor(result.d.transaccion, pesosRegistrados);
                            } else {
                                alert("Sin datos de transacción del proveedor")
                            }
                        } else {
                            alert("Ha habido un error al consultar los datos de la transacción del proveedor");
                        }
                    },
                    error: function (result) {
                        window.kendo.ui.progress($("#dataContainer1"), false);
                        window.kendo.ui.progress($("#dataContainer2"), false);
						alert("Ha habido un error al cargar la información del recurso de integración");
					}
                });
            }

            function mostrarDetalleTransaccionProveedor(transaccion, pesos) {
                $("#pesoCaptadoTransaccionDD").text(transaccion.peso || transaccion.peso == 0 ? (transaccion.peso + " Kg.") : "N/A");
                $("#horaTransaccionDD").text(transaccion.fechaTransaccion ? kendo.toString(kendo.parseDate(transaccion.fechaTransaccion), 'hh:mm tt') : "Sin información");
                $("#pesoCaptadoDD").text(transaccion.peso || transaccion.peso == 0 ? transaccion.peso : "Sin informacion");
                // Calcular peso neto
                var pesoNeto = 0;
                pesoNeto = (transaccion.peso ? transaccion.peso : 0) - (pesos.taraMontacargas ? pesos.taraMontacargas : 0) - (pesos.taraTina ? pesos.taraTina : 0);
                $("#pesoNetoDD").text(pesoNeto.toFixed(3)  + " Kg.");
                mostrarModalTransaccion();
            }

            function mostrarModalTransaccion(e) {
				prevenirCerrarModalTransaccion();
				/*$("#modalTransaccion").modal().on('shown.bs.modal', function () {
					// Al abrir modal
				});
				$("#modalTransaccion").on("hidden.bs.modal", function () { 
					// Al cerrar modal
				});*/
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

            function mostrarModalTolerancia(e) {
				prevenirCerrarModalTolerancia();
			};

			function prevenirCerrarModalTolerancia() {
				$('#modalTolerancia').modal({
					backdrop: 'static',
					keyboard: false
				});
            }

            function mostrarModalToleranciaExcedida(e) {
				prevenirCerrarModalToleranciaExcedida();
			};

			function prevenirCerrarModalToleranciaExcedida() {
				$('#modalToleranciaExcedida').modal({
					backdrop: 'static',
					keyboard: false
				});
            }

            function limpiarComponentes() {
                // Limpiar el valor de los componentes
                $("#codigoTinaDD").text("");
                $("#taraDD").text("");
                $("#rsaDD").text("");
                $("#barcoDD").text("");
                $("#viajeDD").text("");
                $("#ejercicioDD").text("");
                $("#claveDD").text("");
                $("#tallaDD").text("");
                $("#calidadDD").text("");
                $("#desviacionCalidadDD").text("");
                $("#sensorialDD").text("");
                $("#nivelMuestreoDD").text("");
                $("#tinaSensorDD").text("");
                $("#tanqueDD").text("");
                $("#certificacionesDD").text("");
                $("#temperaturaDD").text("");
                $("#montacargasDD").text("");
                $("#taraMontacargasDD").text("");
                $("#taraTinaDD").text("");
                $("#pesoCaptadoTransaccionDD").text("");
                $("#horaTransaccionDD").text("");
                $("#pesoCaptadoDD").text("");
                $("#pesoNetoDD").text("");
                // Inhabilitar los componentes
                var dropdownlist = $("#frigorificosDDL").data("kendoDropDownList");
                dropdownlist.dataSource.read();
                dropdownlist = $("#areasDDL").data("kendoDropDownList");
                dropdownlist.value("");
                dropdownlist = $("#columnasDDL").data("kendoDropDownList");
                dropdownlist.enable(false);
                dropdownlist.value("");
                dropdownlist = $("#lineasDDL").data("kendoDropDownList");
                dropdownlist.enable(false);
                dropdownlist.value("");
                dropdownlist = $("#lineasPosExtDDL").data("kendoDropDownList");
                dropdownlist.dataSource.read();
                $("#aplicarBtn").prop("disabled", true);
            }

            $("#confirmarBtn").kendoButton({
                click: function (e) {
                    $('#modalTransaccion').modal('hide');
                    // Consultar el tiempo restante del montacargas
                    var params = JSON.stringify({
                        idBascula: ddlBasculas.val()
                    });
                    $.ajax({
                        url: "AsignarUbicacionFinalTina.aspx/ConsultarTarajeMontacargas",
                        contentType: "application/json;charset=utf-8",
                        type: "POST",
                        data: params,
                        beforeSend: function () {
                            window.kendo.ui.progress($("#dataContainer2"), true);
                        },
                        success: function (result) {
                            window.kendo.ui.progress($("#dataContainer2"), false);
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
                // Evaluar tiempo restante
                if (taraje.tiempoRestante <= 0) {
                    $("#montacargasExcedidoDD").text(taraje.montacargas ? taraje.montacargas : "Sin descripción");
                    // PENDIENTE - Mostrar dato de placas
                    // Actualizar el registro de la transacción del proveedor para descartarlo
                    actualizarRegistroTransaccionProveedor(false);
                    mostrarModalToleranciaExcedida();
                    limpiarComponentes();
                } else if (taraje.tiempoRestante <= taraje.tolerancia && taraje.tiempoRestante > 0) {
                    $("#tiempoRestanteDD").text(taraje.tiempoRestante || taraje.tiempoRestante == 0 ? (Math.round(taraje.tiempoRestante) + " Min.") : "Sin datos");
                    $("#montacargasToleranciaDD").text(taraje.montacargas ? taraje.montacargas : "Sin descripción");
                    // PENDIENTE - Mostrar dato de placas
                    mostrarModalTolerancia();
                    $("#aplicarBtn").prop("disabled", false);
                } else {
                    $("#aplicarBtn").prop("disabled", false);
                } 
            }

            $("#noCoincideBtn").kendoButton({
                click: function (e) {
                    // Solicitar confirmación
                    var respuesta = confirm("Confirmar para continuar");
                    if (respuesta == true) {
                        // Actualizar el registro de la transacción del proveedor para descartarlo
                        actualizarRegistroTransaccionProveedor(false);
                        $('#modalTransaccion').modal('hide');
                        mostrarModalTransaccionNoCoincide();
                        // Quitar valores de peso de los componentes
                        $("#pesoNetoDD").text("0 Kg.");
                        $("#pesoCaptadoDD").text("No hay datos de peso");
                    } 
					e.preventDefault();
				}
            });

            function actualizarRegistroTransaccionProveedor(coincide) {
                var params = JSON.stringify({
                    transaccionPesoDetalle: {
                        idBascula: ddlBasculas.val(),
                        idTransaccionBascula: idTransaccionProveedorConsultado,
                        coincide: coincide
                    }
                });
                $.ajax({
                    url: "AsignarUbicacionFinalTina.aspx/ActualizarTransaccionProveedor",
                    contentType: "application/json;charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                // No se muestra mensaje en caso de ser exitoso
                                break;
                            case 400:
                                alert("Ha habido un error al actualizar el registro del pesaje de la báscula, revise los logs para más detalles");
                                break;
							case 409:
								alert("Ha habido un error al actualizar el registro del pesaje de la báscula, revise los logs para más detalles");
								break;
						    default:
							    alert("Ha habido un error al actualizar el registro del pesaje de la báscula");
							    break;
					    }
                    },
                    error: function (result) {
						alert("Ha habido un error al ejecutar la actualización del registro de pesaje del proveedor en el recurso de integración");
					}
                });
            }

            $("#aceptarBtn").kendoButton({
                click: function (e) {
                    $('#modalTransaccionNoCoincide').modal('hide');
					e.preventDefault();
				}
            });

            $("#aceptarToleranciaExcedidaBtn").kendoButton({
                click: function (e) {
                    $('#modalToleranciaExcedida').modal('hide');
					e.preventDefault();
				}
            });

			/*$("#verBtn").kendoButton({
				click: function (e) {
					mostrarModalTransaccion();
					e.preventDefault();
				}
            });

            $("#ver2Btn").kendoButton({
				click: function (e) {
					mostrarModalTransaccionNoCoincide();
					e.preventDefault();
				}
            });

            $("#ver3Btn").kendoButton({
				click: function (e) {
					mostrarModalTolerancia();
					e.preventDefault();
				}
            });

            $("#ver4Btn").kendoButton({
				click: function (e) {
					mostrarModalToleranciaExcedida();
					e.preventDefault();
				}
            });*/

            var ddlTipoEntrada = $('#tipoEntradaDDL').kendoDropDownList({
                enable: true,
                //autoBind: false,
                dataValueField: "idEntrada",
                dataTextField: "descripcionEntrada",
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                url: "AsignarUbicacionFinalTina.aspx/ObtenerTiposEntrada",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d) {
                                        options.success(result.d.entradas);
                                    } else {
                                        options.success([]);
                                        alert("Ha habido un error al consultar los tipos de entrada, revise los logs para mas detalles");
                                    }
                                    
                                },
                                error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al consultar los tipos de entrada, revise los logs para mas detalles");
                                }
                            });
                        }
                    }
                }
            });

            var ddlFrigorificos = $('#frigorificosDDL').kendoDropDownList({
                enable: true,
                //autoBind: false,
                dataValueField: "idAlmacen",
                dataTextField: "descripcion",
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                url: "AsignarUbicacionFinalTina.aspx/ObtenerFrigorificos",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d.frigorificosAntecamaras.length) {
                                        options.success(result.d.frigorificosAntecamaras);
                                        // Cargar el ddl de areas
                                        var dropdownlist = $("#areasDDL").data("kendoDropDownList");
                                        dropdownlist.enable(true);
                                        dropdownlist.dataSource.read();
                                    } else {
                                        options.success([]);
                                        alert("No se encontraron frigoríficos");
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
                optionLabel: "Seleccionar",
                change: function () {
                    if (this.value()) {
                        var dropdownlist = $("#areasDDL").data("kendoDropDownList");
                        dropdownlist.dataSource.read();
                        // Limpiar la selección
                        dropdownlist.value("");
                    }
				}
            });

            var ddlAreasDetalle = $('#areasDetalleDDL').kendoDropDownList({
                enable: true,
                autoBind: false,
                dataValueField: "area",
                dataTextField: "area",
                dataSource: {
                    transport: {
                        read: function (options) {
                            var params = JSON.stringify({
                                idAlmacen: ddlFrigorificos.val()
                            });
                            $.ajax({
                                url: "AsignarUbicacionFinalTina.aspx/ConsultarAreasFrigorifico",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    if (result.d) {
                                        if (!result.d.ubicaciones.length) {
                                            alert("No se encontraron áreas");
                                        }
                                        options.success(result.d.ubicaciones);
                                    } else {
                                        options.success([]);
                                        alert("Ha habido un error al consultar las áreas, revise los logs para mas detalles");
                                    }
                                },
                                error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al consultar las áreas, revise los logs para mas detalles");
                                }
                            });
                        }
                    }
                },
                optionLabel: "Seleccionar",
                change: function () {
                    if (this.value()) {
                        // Obtener los datos (línea - columna) del área seleccionada
                        consultarDetalleArea(true);
                        // Limpiar los valores del grid
                        var grid = $("#desgloseTinaGrid").data("kendoGrid");
                        grid.dataSource.data([]);
                    }
				}
            });

            function consultarDetalleArea(isFromModal) {
                var params = JSON.stringify({
                    idAlmacen: ddlFrigorificos.val(),
                    area: isFromModal ? ddlAreasDetalle.val() : ddlAreas.val()
                });
                $.ajax({
                    url: "AsignarUbicacionFinalTina.aspx/ConsultarDetalleArea",
                    contentType: "application/json;charset=utf-8",
                    type: "POST",
                    data: params,
                    beforeSend: function () {
                        window.kendo.ui.progress($("#mapaGraficoArea"), true);
                    },
                    success: function (result) {
                        window.kendo.ui.progress($("#mapaGraficoArea"), false);
                        if (result.d) {
                            if (result.d.area) {
                                if (isFromModal) {
                                    obtenerDetalleAreaUbicaciones(result.d.area);
                                } else {
                                    generarValoresDDLColumnaLinea(result.d.area);
                                }
                            } else {
                                alert("No hay datos del área seleccionada")
                            }
                        } else {
                            alert("Ha habido un error al consultar los datos del área seleccionada");
                        }
                    },
                    error: function (result) {
                        window.kendo.ui.progress($("#mapaGraficoArea"), false);
						alert("Ha habido un error al cargar la información del recurso de integración");
                    }
                });
            }

            function obtenerDetalleAreaUbicaciones(detalleArea) {
                // Variables de estructura del mapa
                var lineas = detalleArea.numeroLineas;
                var columnas = detalleArea.numeroColumnas;
                var detalleUbicaciones = [];

                // Consultar detalle de cada ubicación
                var params = JSON.stringify({
                    idAlmacen: ddlFrigorificos.val(),
                    area: ddlAreasDetalle.val()
                });
                $.ajax({
                    url: "AsignarUbicacionFinalTina.aspx/ConsultarDetalleUbicaciones",
                    contentType: "application/json;charset=utf-8",
                    type: "POST",
                    data: params,
                    beforeSend: function () {
                        window.kendo.ui.progress($("#mapaGraficoArea"), true);
                    },
                    success: function (result) {
                        window.kendo.ui.progress($("#mapaGraficoArea"), false);
                        if (result.d) {
                            if (result.d.ubicaciones.length) {  
                                detalleUbicaciones = result.d.ubicaciones;
                                generarMapaAreaUbicaciones(lineas, columnas, detalleUbicaciones);
                            } else {
                                alert("No se encontraron ubicaciones del área seleccionada")
                            }
                        } else {
                            alert("Ha habido un error al consultar las ubicaciones del área seleccionada");
                        }
                    },
                    error: function (result) {
                        window.kendo.ui.progress($("#mapaGraficoArea"), false);
						alert("Ha habido un error al cargar la información del recurso de integración");
                    }
                });
            }

            function generarMapaAreaUbicaciones(lineas, columnas, ubicaciones) {
                var mapaAreaUbicaciones = [];
                var valorSumaPosicionxy = 0;

                // Iterable para el mapa
                for (var i = 0; i <= lineas - 1; i++) {
                    mapaAreaUbicaciones[i] = [];
                    for (var x = 0; x <= columnas - 1; x++) {
                        if (ubicaciones[valorSumaPosicionxy].numeroTinasXUbicacion == 6) {
                            // Posición ocupada
                            mapaAreaUbicaciones[i].push({ estatus: "Ocupada", ubicacion: ubicaciones[valorSumaPosicionxy].ubicacion });
                        } else if (ubicaciones[valorSumaPosicionxy].numeroTinasXUbicacion > 0 && ubicaciones[valorSumaPosicionxy].numeroTinasXUbicacion < 6) {
                            // Posición parcialmente ocupada
                            mapaAreaUbicaciones[i].push({ estatus: "Parcial", ubicacion: ubicaciones[valorSumaPosicionxy].ubicacion });
                        } else if (ubicaciones[valorSumaPosicionxy].numeroTinasXUbicacion <= 0) {
                            // Posición vacía
                            mapaAreaUbicaciones[i].push({ estatus: "Vacia", ubicacion: ubicaciones[valorSumaPosicionxy].ubicacion });
                        } else {
                            // Posición incongruente
                            mapaAreaUbicaciones[i].push({ estatus: "Incongruente", ubicacion: ubicaciones[valorSumaPosicionxy].ubicacion });
                        }
                        valorSumaPosicionxy += 1;
                    }
                }

                // Generar el html de los numerados de línea y columna
                let templateColumnas = "";
                var innerTemplateColumnas = "";
                for (let c = 0; c <= columnas - 1; c++) {
                    innerTemplateColumnas += `<td class="cart-blocked"><div class="card-table">${(c + 1)}</div></td>`;
                }
                templateColumnas = `<tr>${innerTemplateColumnas}</tr>`;
                document.querySelector("#numeradoColumnasGraficoArea").innerHTML = templateColumnas;

                let templateLineas = "";
                var innerTemplateLineas = "";
                for (let l = 0; l <= lineas - 1; l++) {
                    innerTemplateLineas += `<td class="cart-blocked"><div class="card-table">${(l + 1)}</div></td>`;
                }
                templateLineas = `<tr>${innerTemplateLineas}</tr>`;
                document.querySelector("#numeradoLineasGraficoArea").innerHTML = templateLineas;

                // Generar el html del mapa de área
                let template = "";
                for (let i = 0; i < mapaAreaUbicaciones.length; i++) {
                    var innerTemplate = "";
                    for (let e = 0; e < mapaAreaUbicaciones[i].length; e++) {
                        // Establecer la clase correspondiente en cada <td> dependiendo del estatus de la ubicación
                        if (mapaAreaUbicaciones[i][e].estatus == "Ocupada") {
                            innerTemplate += `<td idubicacion="${mapaAreaUbicaciones[i][e].ubicacion}" class="cart-blocked"><div class="card-table"><span class="icon-full"></span></div></td>`;
                        } else if (mapaAreaUbicaciones[i][e].estatus == "Parcial") {
                            innerTemplate += `<td idubicacion="${mapaAreaUbicaciones[i][e].ubicacion}" class="cart-shift1"><div class="card-table"><span class="icon-parcial"></span></div></td>`;
                        } else if (mapaAreaUbicaciones[i][e].estatus == "Vacia") {
                            innerTemplate += `<td idubicacion="${mapaAreaUbicaciones[i][e].ubicacion}" class="cart-shift2"><div class="card-table"><span class="icon-empty"></span></div></td>`;
                        } else if (mapaAreaUbicaciones[i][e].estatus == "Incongruente") {
                            innerTemplate += `<td idubicacion="${mapaAreaUbicaciones[i][e].ubicacion}" class="cart-empty"><div class="card-table"><span class="icon-warning"></span></div></td>`;
                        }
                    }
                    template += `<tr>${innerTemplate}</tr>`;
                }
                document.querySelector("#mapaGraficoArea").innerHTML = template;

                // Bindear evento para consultar el desglose por tina
                $("#mapaGraficoArea").on("click", "td", function (e) {
                    var ubicacion = e.target.parentElement.parentElement.attributes.idubicacion.value ? e.target.parentElement.parentElement.attributes.idubicacion.value : "n/a";
                    consultarDesgloseTina(ubicacion);
                })
            }

            function consultarDesgloseTina(idUbicacion) {
                var grid = $("#desgloseTinaGrid").data("kendoGrid");
                var params = JSON.stringify({
                    idUbicacion: idUbicacion,
                    idAlmacen: ddlFrigorificos.val()
                });
                $.ajax({
                    url: "AsignarUbicacionFinalTina.aspx/ObtenerDesgloseTina",
                    contentType: "application/json;charset=utf-8",
                    type: "POST",
                    data: params,
                    beforeSend: function () {
                        window.kendo.ui.progress($("#desgloseTinaGrid"), true);
                    },
                    success: function (result) {
                        window.kendo.ui.progress($("#desgloseTinaGrid"), false);
                        if (result.d) {
                            if (result.d.detalleUbicacion.length) {
                                // Popular el grid con la información de la tina
                                grid.dataSource.data(result.d.detalleUbicacion);
                            } else {
                                grid.dataSource.data([]);
                            }
                        } else {
                            alert("Ha habido un error al consultar los datos de la tina seleccionada");
                            grid.dataSource.data([]);
                        }
                    },
                    error: function (result) {
                        window.kendo.ui.progress($("#desgloseTinaGrid"), false);
                        alert("Ha habido un error al cargar la información del recurso de integración");
                        grid.dataSource.data([]);
                    }
                });
            }

            $("#desgloseTinaGrid").kendoGrid({
                columns: [
                    {
                        //field: "talla",
                        title: "Producto",
                        width: 140,
                        template: "#= claveVA # #= talla #"
                    },
                    {
                        field: "calidad",
                        title: "Calidad",
                        width: 100
                    },
                    {
                        field: "barco",
                        title: "Barco",
                        width: 100
                    },
                    {
                        field: "ejercicio",
                        title: "Ejercicio",
                        width: 100
                    },
                    {
                        field: "tanque",
                        title: "Tanque",
                        width: 100
                    },
                    {
                        field: "tina",
                        title: "Tina",
                        width: 100
                    },
                    {
                        field: "fecha",
                        title: "Fecha/Hora",
                        width: 150,
                        template: "#= kendo.toString(kendo.parseDate(fecha), 'dd/MM/yyyy HH:mm') #",
                        filterable: false
                    }
                ],
                dataSource: {
                    sync: function () {
                        this.read();
                    },
                    pageSize: 15,
                    schema: {
                        model: {
                            fields: {
                                claveVA: {
                                    type: "string"
                                },
                                talla: {
                                    type: "string"
                                },
                                calidad: {
                                    type: "string"
                                },
                                barco: {
                                    type: "string"
                                },
                                ejercicio: {
                                    type: "string"
                                },
                                tanque: {
                                    type: "string"
                                },
                                tina: {
                                    type: "string"
                                },
                                fecha: {
                                    type: "string"
                                }
                            }
                        }
                    },
                    data: []
                },
                scrollable: true,
                resizable: true,
                editable: false,
                pageable: {
                    messages: {
                        display: "{0}-{1} de {2} elementos",
                        empty: ""
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

            var ddlAreas = $('#areasDDL').kendoDropDownList({
                enable: false,
                autoBind: false,
                dataValueField: "area",
                dataTextField: "area",
                dataSource: {
                    transport: {
                        read: function (options) {
                            var params = JSON.stringify({
                                idAlmacen: ddlFrigorificos.val() ? ddlFrigorificos.val() : 0
                            });
                            $.ajax({
                                url: "AsignarUbicacionFinalTina.aspx/ConsultarAreasFrigorifico",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    if (result.d) {
                                        options.success(result.d.ubicaciones);
                                        // Limpiar la selección 
                                        var dropdownlist = $("#columnasDDL").data("kendoDropDownList");
                                        var dropdownlist2 = $("#lineasDDL").data("kendoDropDownList");
                                        dropdownlist.value("");
                                        dropdownlist2.value("");
                                        if (!result.d.ubicaciones.length) {
                                            // Bloquear los ddl correspondientes
                                            dropdownlist.enable(false);
                                            dropdownlist2.enable(false);
                                        }
                                    } else {
                                        options.success([]);
                                        alert("Ha habido un error al consultar las áreas, revise los logs para mas detalles");
                                    }
                                },
                                error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al consultar las áreas, revise los logs para mas detalles");
                                }
                            });
                        }
                    }
                },
                optionLabel: "Seleccionar",
                change: function () {
                    // Limpiar la selección de los ddl correspondientes
                    var dropdownlist = $("#columnasDDL").data("kendoDropDownList");
                    var dropdownlist2 = $("#lineasDDL").data("kendoDropDownList");
                    dropdownlist.value("");
                    dropdownlist2.value("");
                    if (this.value()) {
                        // Cargar ddl de columnas y líneas
                        consultarDetalleArea(false);
                    } else {
                        dropdownlist.enable(false);
                        dropdownlist2.enable(false);
                    }
				}
            });

            function generarValoresDDLColumnaLinea(detalleArea) {
                var lineas = detalleArea.numeroLineas;
                var columnas = detalleArea.numeroColumnas;
                var listaLineas = [];
                var listaColumnas = [];
                // Generar lista de líneas
                for (var i = 0; i <= lineas - 1; i++) {
                    if (i < 9) {
                        listaLineas.push({
                            idLinea: "L0" + (i + 1),
                            linea: "L0" + (i + 1)
                        });
                    } else {
                        listaLineas.push({
                            idLinea: "L" + (i + 1),
                            linea: "L" + (i + 1)
                        });
                    }
                }
                // Generar lista de columnas
                for (var x = 0; x <= columnas - 1; x++) {
                    if (x < 9) {
                        listaColumnas.push({
                            idColumna: "C0" + (x + 1),
                            columna: "C0" + (x + 1)
                        });
                    } else {
                        listaColumnas.push({
                            idColumna: "C" + (x + 1),
                            columna: "C" + (x + 1)
                        });
                    }
                }
                var dropdownlist = $("#columnasDDL").data("kendoDropDownList");
                dropdownlist.enable(true);
                dropdownlist.setDataSource(listaColumnas);
                var dropdownlist2 = $("#lineasDDL").data("kendoDropDownList");
                dropdownlist2.enable(true);
                dropdownlist2.setDataSource(listaLineas);
            }

            var ddlColumnas = $('#columnasDDL').kendoDropDownList({
                enable: false,
                autoBind: false,
                dataValueField: "idColumna",
                dataTextField: "columna",
                dataSource: {
                    data: []
                },
                optionLabel: "Seleccionar"
            });

            var ddlLineas = $('#lineasDDL').kendoDropDownList({
                enable: false,
                autoBind: false,
                dataValueField: "idLinea",
                dataTextField: "linea",
                dataSource: {
                    data: []
                },
                optionLabel: "Seleccionar"
            });

            var ddlLineasPosExt = $('#lineasPosExtDDL').kendoDropDownList({
                enable: true,
                //autoBind: false,
                dataValueField: "ubicacion",
                dataTextField: "descripcion",
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                url: "AsignarUbicacionFinalTina.aspx/ObtenerUbicacionesExternas",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d) {
                                        options.success(result.d.entradas);
                                    } else {
                                        options.success([]);
                                        alert("Ha habido un error al consultar las ubicaciones exteriores, revise los logs para mas detalles");
                                    }
                                    
                                },
                                error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al consultar las ubicaciones exteriores, revise los logs para mas detalles");
                                }
                            });
                        }
                    }
                }
            });

            $("#verUbicacionesBtn").kendoButton({
                click: function (e) {
                    // Comprobar que el frigorífico seleccionado tenga areas relacionadas
                    if (ddlFrigorificos.val()) {
                        var params = JSON.stringify({
                            idAlmacen: ddlFrigorificos.val()
                        });
                        $.ajax({
                            url: "AsignarUbicacionFinalTina.aspx/ConsultarAreasFrigorifico",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            data: params,
                            success: function (result) {
                                if (result.d) {
                                    if (result.d.ubicaciones.length) {
                                        // Popular lista de áreas
                                        var dropdownlist = $("#areasDetalleDDL").data("kendoDropDownList");
                                        //dropdownlist.dataSource.read();
                                        dropdownlist.setDataSource(result.d.ubicaciones);
                                        mostrarModalUbicaciones();
                                    } else {
                                        alert("No se encontraron áreas correspondientes al frigorífico seleccionado");
                                    }
                                } else {
                                    alert("Ha habido un error al consultar las áreas del frigorífico");
                                }
                            },
                            error: function (result) {
                                alert("Ha habido un error al cargar la información del recurso de integración");
                            }
                        });
                    } else {
                        alert("Es necesario seleccionar un frigorífico para continuar")
                    }
                    e.preventDefault();
                }
            });

            function mostrarModalUbicaciones(e) {
                prevenirCerrarModalUbicaciones();
                $("#modalUbicaciones").on("hidden.bs.modal", function () { 
                    var dropdownlist = $("#areasDetalleDDL").data("kendoDropDownList");
                    dropdownlist.value("");
                    // Reestablecer el gráfico
                    document.querySelector("#numeradoColumnasGraficoArea").innerHTML = "";
                    document.querySelector("#numeradoLineasGraficoArea").innerHTML = "";
                    document.querySelector("#mapaGraficoArea").innerHTML = "";
                    // Limpiar los valores del grid
                    var grid = $("#desgloseTinaGrid").data("kendoGrid");
                    grid.dataSource.data([]);
				});
			};

			function prevenirCerrarModalUbicaciones() {
				$('#modalUbicaciones').modal({
					backdrop: 'static',
					keyboard: false
				});
            }

            $("#aplicarBtn").kendoButton({
                click: function (e) {
                    if (ddlTipoEntrada.val()) {
                        if (ddlFrigorificos.val()) {
                            if (ddlAreas.val()) {
                                if (ddlColumnas.val() && ddlLineas.val()) {
                                    if (ddlLineasPosExt.val()) {
                                        // Verificar la disponibilidad de la tina
                                        var idUbicacion = ddlAreas.val() + ddlLineas.val() + ddlColumnas.val();
                                        var params = JSON.stringify({
                                            idUbicacion: idUbicacion,
                                            idAlmacen: ddlFrigorificos.val()
                                        });
                                        $.ajax({
                                            url: "AsignarUbicacionFinalTina.aspx/ObtenerDesgloseTina",
                                            contentType: "application/json;charset=utf-8",
                                            type: "POST",
                                            data: params,
                                            beforeSend: function () {
                                                window.kendo.ui.progress($("#dataContainer1"), true);
                                                window.kendo.ui.progress($("#dataContainer2"), true);
                                            },
                                            success: function (result) {
                                                window.kendo.ui.progress($("#dataContainer1"), false);
                                                window.kendo.ui.progress($("#dataContainer2"), false);
                                                if (result.d) {
                                                    if (result.d.detalleUbicacion) {
                                                        if (result.d.detalleUbicacion.length < 6) {
                                                            var params = JSON.stringify({
                                                                tina: {
                                                                    idEtiquetaTina: datosBasculaActual.idEtiquetaTina,
                                                                    idTina: datosBasculaActual.idTina,
                                                                    tina: datosBasculaActual.tina,
                                                                    idAlmacen: ddlFrigorificos.val(),
                                                                    idEtiquetaMontacargas: datosBasculaActual.idEtiquetaMontacargas,
                                                                    posicionExterna: ddlLineasPosExt.val(),
                                                                    area: ddlAreas.val(),
                                                                    linea: ddlLineas.val(),
                                                                    columna: ddlColumnas.val(),
                                                                    pesoTotal: pesoCaptadoProveedor,
                                                                    bascula: ddlBasculas.val(),
                                                                    idTipoEntrada: ddlTipoEntrada.val(),
                                                                    idBarco: datosBasculaActual.idBarco,
                                                                    idViaje: datosBasculaActual.idViaje,
                                                                    idTanque: datosBasculaActual.idTanque,
                                                                    idProducto: datosBasculaActual.idProducto,
                                                                    idProblema: datosBasculaActual.idProblema,
                                                                    calidad: datosBasculaActual.claveCalidad,
                                                                    muestreo: datosBasculaActual.muestreo,
                                                                    nivel: datosBasculaActual.nivelMuestreo,
                                                                    idCalidad: datosBasculaActual.calidad,
                                                                    mediaTina: false
                                                                }
                                                            });
                                                            asignarUbicacionTinaSAI(params);
                                                        } else {
                                                            alert("La ubicación seleccionada ya está ocupada con 6 tinas");
                                                        }
                                                    } else {
                                                        alert("No se pudo obtener el nivel de ocupación de la ubicación. Intente de nuevo");
                                                    }
                                                } else {
                                                    alert("Ha habido un error al consultar los datos de la tina seleccionada");
                                                    grid.dataSource.data([]);
                                                }
                                            },
                                            error: function (result) {
                                                window.kendo.ui.progress($("#dataContainer1"), false);
                                                window.kendo.ui.progress($("#dataContainer2"), false);
                                                alert("Ha habido un error al cargar la información del recurso de integración");
                                                grid.dataSource.data([]);
                                            }
                                        });
                                    } else {
                                        alert("Es necesario seleccionar la posición externa para continuar");
                                    }
                                } else {
                                    alert("Es necesario seleccionar columna y línea para continuar");
                                }
                            } else {
                                alert("Es necesario seleccionar un área para continuar");
                            }
                        } else {
                            alert("Es necesario seleccionar un frigorífico para continuar");
                        }
                    } else {
                        alert("Es necesario seleccionar un tipo de entrada");
                    }
                    e.preventDefault();
                }
            });

            function asignarUbicacionTinaSAI(tina) {
                $.ajax({
                    url: "AsignarUbicacionFinalTina.aspx/AsignarUbicacionTinaSAI",
                    contentType: "application/json;charset=utf-8",
                    type: "POST",
                    data: tina,
                    beforeSend: function () {
                        window.kendo.ui.progress($("#dataContainer1"), true);
                        window.kendo.ui.progress($("#dataContainer2"), true);
                    },
                    success: function (result) {
                        window.kendo.ui.progress($("#dataContainer1"), false);
                        window.kendo.ui.progress($("#dataContainer2"), false);
                        switch (result.d) {
                            case 200:
                                asignarUbicacionTinaSIP(tina);
                                // NOTA: Para efectos de rollback en caso de error, se debe considerar también la tabla de proveedor
                                actualizarRegistroTransaccionProveedor(true);
                                break;
                            case 400:
                                alert("Ha habido un error al asignar la ubicación de la tina en SAI, revise los logs para más detalles");
                                break;
							case 409:
								alert("Ha habido un error al asignar la ubicación de la tina en SAI, revise los logs para más detalles");
								break;
						    default:
							    alert("Ha ocurrido un error desconocido");
							    break;
					    }
                    },
                    error: function (result) {
                        window.kendo.ui.progress($("#dataContainer1"), false);
                        window.kendo.ui.progress($("#dataContainer2"), false);
						alert("Ha habido un error al ejecutar la operación en el recurso de integración en SAI");
					}
                });
            }

            function asignarUbicacionTinaSIP(tina) {
                $.ajax({
                    url: "AsignarUbicacionFinalTina.aspx/AsignarUbicacionTinaSIP",
                    contentType: "application/json;charset=utf-8",
                    type: "POST",
                    data: tina,
                    beforeSend: function () {
                        window.kendo.ui.progress($("#dataContainer1"), true);
                        window.kendo.ui.progress($("#dataContainer2"), true);
                    },
                    success: function (result) {
                        window.kendo.ui.progress($("#dataContainer1"), false);
                        window.kendo.ui.progress($("#dataContainer2"), false);
                        switch (result.d) {
                            case 200:
                                alert("Se ha asignado la tina a la ubicación seleccionada");
                                limpiarComponentes();
                                break;
                            case 400:
                                alert("Ha habido un error al asignar la ubicación de la tina en SIP, revise los logs para más detalles");
                                break;
							case 409:
								alert("Ha habido un error al asignar la ubicación de la tina en SIP, revise los logs para más detalles");
								break;
						    default:
							    alert("Ha ocurrido un error desconocido");
							    break;
					    }
                    },
                    error: function (result) {
                        window.kendo.ui.progress($("#dataContainer1"), false);
                        window.kendo.ui.progress($("#dataContainer2"), false);
						alert("Ha habido un error al ejecutar la operación en el recurso de integración en SIP");
					}
                });
            }
        });
    </script>  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Asignar Ubicación Final por Tina" id="hPage" />

    <section id="divCustomContentt" class="custom-content" runat="server">
        <div class="bs-component">
            <div class="clearfix">
                <div class="row">
                    <div class="col-sm-12">
                        <!-- /.box-content -->
                        <div class="box custom-box box-primary admin-nuevo">
                            <!-- /.box-header -->
                            <div class="box-header with-border">
                                <h3 class="box-title"></h3>
                            </div>
                            <div class="col-sm-12">
                                <div class="col-sm-3">
                                    <input id="basculaSeleccionadaTxt" class="k-textbox" type="text" disabled="disabled" />
                                </div>
                                <div class="col-sm-7"> 
                                </div>
                                <div class="col-sm-2">
                                    <button id="actualizarBtn" class="k-primary">Actualizar</button>
                                    <!--<button id="verBtn" class="k-primary">Ver Modal</button>
                                    <button id="ver2Btn" class="k-primary">Ver Modal 2</button>
                                    <button id="ver3Btn" class="k-primary">Ver Modal 3</button>
                                    <button id="ver4Btn" class="k-primary">Ver Modal 4</button> -->
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
                                <h3 class="box-title">Datos de Tina</h3>
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
                                                                Código Tina
                                                            </dt>
                                                            <dd id="codigoTinaDD" class="col-sm">
                                                            </dd>
                                                            <dt class="col-sm">
                                                                Ejercicio
                                                            </dt>
                                                            <dd id="ejercicioDD" class="col-sm">
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Tara (Kg.)
                                                            </dt>
                                                            <dd id="taraDD" class="col-sm">
                                                            </dd>
                                                            <dt class="col-sm">
                                                                Clave
                                                            </dt>
                                                            <dd id="claveDD" class="col-sm">
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                RSA
                                                            </dt>
                                                            <dd id="rsaDD" class="col-sm">
                                                            </dd>
                                                            <dt class="col-sm">
                                                                Talla
                                                            </dt>
                                                            <dd id="tallaDD" class="col-sm">
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Barco
                                                            </dt>
                                                            <dd id="barcoDD" class="col-sm">
                                                            </dd>
                                                            <dt class="col-sm">
                                                                Calidad
                                                            </dt>
                                                            <dd id="calidadDD" class="col-sm">
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Viaje
                                                            </dt>
                                                            <dd id="viajeDD" class="col-sm">
                                                            </dd>
                                                            <dt class="col-sm">
                                                                Desv. de Cal.
                                                            </dt>
                                                            <dd id="desviacionCalidadDD" class="col-sm">
                                                            </dd>
                                                        </div>
                                                    </dl>
                                                    <dl class="lista-descripcion">
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Sensorial
                                                            </dt>
                                                            <dd id="sensorialDD" class="col-sm">
                                                            </dd>
                                                            <dt class="col-sm">
                                                                Temperatura (°C)
                                                            </dt>
                                                            <dd id="temperaturaDD" class="col-sm">
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Nivel de Muestreo
                                                            </dt>
                                                            <dd id="nivelMuestreoDD" class="col-sm">
                                                            </dd>
                                                            <dt class="col-sm">
                                                                Media Tina
                                                            </dt>
                                                            <dd id="mediaTinaDD" class="col-sm">
                                                                <input id="mediaTinaChk" class="k-checkbox-disabled" type="checkbox" disabled="disabled" />
																<span class="checkmark"></span>
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Tina con Sensor
                                                            </dt>
                                                            <dd id="tinaSensorDD" class="col-sm">
                                                            </dd>
                                                            <dt class="col-sm">
                                                                Tipo de Entrada
                                                            </dt>
                                                            <dd id="tipoEntradaDD" class="col-sm">
                                                                <input id="tipoEntradaDDL" />
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Tanque
                                                            </dt>
                                                            <dd id="tanqueDD" class="col-sm">
                                                            </dd>
                                                            <dt class="col-sm">
                                                            </dt>
                                                            <dd id="espacioDD" class="col-sm">
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Certificaciones
                                                            </dt>
                                                            <dd id="certificacionesDD" class="col-sm">
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
                                <h3 class="box-title">Datos de Montacargas</h3>
                            </div>
                            <!-- /.box-body -->
                            <div class="box-body">
                                <div class="row">
                                    <!-- div-12 -->
                                    <div id="infoContainerData2" class="info-container-data">
                                        <div>
                                            <div id="dataContainer2" class="data-list-container ">
                                                <div class="col-sm-12 column-container unique-container">
                                                    <div class="identify-row">
                                                        <span>
                                                            <dt class="col-sm">
                                                                 Peso Neto:
                                                            </dt>
                                                            <%--<h2>0000 Kg.</h2>--%>
                                                            <h2>
                                                                <dd id="pesoNetoDD" class="col-sm">
                                                                </dd>
                                                            </h2>
                                                        </span>
                                                    </div>
                                                    <dl class="lista-descripcion">
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Montacargas
                                                            </dt>
                                                            <dd id="montacargasDD" class="col-sm">
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Tara Montacargas (Kg.)
                                                            </dt>
                                                            <dd id="taraMontacargasDD" class="col-sm">
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Tara Tina (Kg.)
                                                            </dt>
                                                            <dd id="taraTinaDD" class="col-sm">
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Peso Captado (Kg.)
                                                            </dt>
                                                            <dd id="pesoCaptadoDD" class="col-sm">
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
                                <h3 class="box-title">Asignar Ubicación</h3>
                            </div>
                            <!-- /.box-body -->
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="col-sm-2">
                                            <label for="lblFrigorificos" class="control-label-top">Frigorífico: </label>
                                            <input id="frigorificosDDL" />
                                        </div>
                                        <div class="col-sm-2">
                                            <label for="lblVerUbicacionesBtn" class="control-label-top"> </label>
                                            <button id="verUbicacionesBtn" class="k-primary">Ver Ubicaciones</button>
                                        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <div class="col-sm-2">
                                            <label for="lblAreas" class="control-label-top">Área: </label>
                                            <input id="areasDDL" />
                                        </div>
                                        <div class="col-sm-2">
                                            <label for="lblColumnas" class="control-label-top">Columna: </label>
                                            <input id="columnasDDL" />
                                        </div>
                                        <div class="col-sm-2">
                                            <label for="lblLineas" class="control-label-top">Línea: </label>
                                            <input id="lineasDDL" />
                                        </div>
                                        <div class="col-sm-2">
                                            <label for="lblLineaPosExt" class="control-label-top">Línea Posición Externa: </label>
                                            <input id="lineasPosExtDDL" />
                                        </div>
                                        <div class="col-sm-4">
                                            <label for="lblaplicarBtn" class="control-label-top"> </label>
                                            <button id="aplicarBtn" class="k-primary">Aplicar</button>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                            </div>
                            <br />
                        </div>
                    </div>
                </div>
                <br />
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

    <!-- Modal Tolerancia -->
        <div class="modal fade k-custom-modal-tm" id="modalTolerancia" role="dialog">
            <div class="modal-dialog emx-custom-modal">
                <!-- Modal content-->
                <div class="modal-content modal-center">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Actualizar Tara</h4>
                    </div>
                    <div class="modal-body">
                        <div id="modalToleranciaForm">
                            <div class="row">
                                <div id="infoContainerData4" class="info-container-data">
                                    <div id="dataContainer4" class="data-list-container ">
                                        <div class="col-sm-12 column-container unique-container oneContainer-twoColumns">
                                            <dl class="lista-descripcion">
                                                <div style="background-color: transparent;" class="identify-row">
                                                    <span>
                                                        <dt class="col-sm">
                                                                Tiempo Restante:
                                                        </dt>
                                                        <h2>
                                                            <dd id="tiempoRestanteDD" class="col-sm">
                                                                10 Min.
                                                            </dd>
                                                        </h2>
                                        
                                                    </span>
                                                </div>
                                            </dl>
                                            <dl class="lista-descripcion">
                                                <dt class="col-sm">
                                                    Montacargas:
                                                </dt>
                                                <dd id="montacargasToleranciaDD" class="col-sm">
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
                        </div>
                    </div>
                </div>
            </div>
        </div>

    <!-- Modal Tolerancia Excedida -->
        <div class="modal fade k-custom-modal-tm" id="modalToleranciaExcedida" role="dialog">
            <div class="modal-dialog emx-custom-modal">
                <!-- Modal content-->
                <div class="modal-content modal-center">
                    <div class="modal-header">
                        <h4 style="color: red;" class="modal-title">Tiempo de Tolerancia Excedido</h4>
                    </div>
                    <div class="modal-body">
                        <div id="modalToleranciaExcedidaForm">
                            <div class="row">
                                <div class="k-block-custom">
				                    <span class="k-icon k-i-warning"></span>
				                    <p class="k-text-tag">Es necesario actualizar la tara del montacargas para poder continuar con la operación</p>
				                </div>
                                <div id="infoContainerData5" class="info-container-data">
                                    <div id="dataContainer5" class="data-list-container ">
                                        <div class="col-sm-12 column-container unique-container oneContainer-twoColumns">
                                            <dl class="lista-descripcion">
                                                 <dt class="col-sm">
                                                    Montacargas:
                                                </dt>
                                                <dd id="montacargasExcedidoDD" class="col-sm">
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
                             <button id="aceptarToleranciaExcedidaBtn" class="k-button k-primary">Aceptar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    <!-- Modal Ubicaciones -->
        <div class="modal fade k-custom-modal-tm" id="modalUbicaciones" role="dialog">
            <div class="modal-dialog emx-custom-modal">
                <!-- Modal content-->
                <div class="modal-content modal-center">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Ubicaciones</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-sm-4">
                                <label for="lblAreasDetalle" class="control-label-top">Área: </label>
                                <input id="areasDetalleDDL" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary custom-modules-styles">
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                             <div class="col-sm-12">
                                                <div id="listView1" class="custom-map-modules">
                                                    <!-- Mapa gráfico del área -->
                                                    <div class="table-container k-wrapper">
														<div class="k-table-zero"></div>
                                                        <table id="numeradoColumnasGraficoArea" class="map-table-styles k-table-one">
                                                        </table>
                                                        <table id="numeradoLineasGraficoArea" class="map-table-styles k-table-two">
                                                        </table>
                                                        <table id="mapaGraficoArea" class="map-table-styles k-table-three">
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div id="desgloseTinaGrid"></div>
                            </div>
                        </div>
                    </div>
                     <div class="modal-footer">
                         <div class="row">
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
