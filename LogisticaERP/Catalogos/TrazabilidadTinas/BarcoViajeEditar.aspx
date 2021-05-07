<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="BarcoViajeEditar.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.BarcoViajeEditar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
    $(function () {
        kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;
        let fechaHoy = kendo.toString(kendo.parseDate(new Date()), 'yyyy-MM-dd');
        let URLactual = window.location;

        let idTanques = "0";
        let indiceTanques = 0;

        let idProductoS = "";
        let indiceProducto = 0;
        let desProducto = "";

        let valores = [];
        let listaCertificaciones = "";
        let datosViaje = [];
        let listaIdTanques = []
        let estatusViaje = false;
        let estatusTrue = true;
        let descargadoEstatus = false;

        let certificaciones = [];
        let idViajeObtenido = "000"; 
        let idBarcoObtenido = "000";
        let StringidBarco = "000";
        let idTanqueSeleccionado = "";
        let statusDolphinSave = false;

        //Editar Barco Viaje.
        let fechaSeleccionDescarga = kendo.toString(kendo.parseDate(new Date()), 'yyyy-MM-dd');
        let fechaSeleccionSalida = kendo.toString(kendo.parseDate(new Date()), 'yyyy-MM-dd');
        let fechaSeleccionArribo = kendo.toString(kendo.parseDate(new Date()), 'yyyy-MM-dd');

        let fechaInicio = "";
        let fechaFin = "";

        primeraCarga();
        desactivar();
        barco_Viaje();

        if (URLactual.href.length > 70) {
           //Se cargan los datos del barco viaje seleccionado
           fechaInicio = URLactual.search.substr(14, 10);
           fechaFin = URLactual.search.substr(34, 10);
           idBarcoObtenido = URLactual.search.substr(59, 17);
           StringidBarco = URLactual.search.substr(59, 17);
        }

        function primeraCarga() {
            fechaInicio = URLactual.search.substr(14, 10);
            fechaFin = URLactual.search.substr(34, 10);
        }

        function desactivar() {
            //Se desactivan los campos.
            $('#lblSeleccionarBarcoDD').prop("disabled", true);
            $('#lblEjercicioDD').prop("disabled", true);
            $('#lblViajeDD').prop("disabled", true);
            $('#lblSteamer').prop("disabled", true);

            //$('#lblRSA').prop("disabled", true);
            $('#lblFolioIndentificador').prop("disabled", true);
            $('#lblViajePesca').prop("disabled", true);
        }

        function desactivarEstatus() {
            //Si el campo activo es false, se desactivan estos campos. 
            $('#lblEstatus').prop("disabled", true);
            $('#lblRSA').prop("disabled", true);
            let datepickerFD = $("#lblFechaDescargaDP").data("kendoDatePicker");
            datepickerFD.enable(false);
            let datepickerFS = $("#lblFechaSalidaDP").data("kendoDatePicker");
            datepickerFS.enable(false);
            let datepickerFA = $("#lblFechaArriboDP").data("kendoDatePicker");
            datepickerFA.enable(false);
            $("#lblEstatus").prop("disabled", true);
            $("#lblDescargado").prop("disabled", true);
            $("#lblDescargaTinas").prop("disabled", true);
            $("#lblSubViaje").prop("disabled", true);
            $(".k-grid-add", "#tanquesGrid").hide();
            $(".k-grid-add", "#productoGrid").hide();
            $('#actualizarBarcoViaje').prop("disabled", true);
        }
             
        function barco_Viaje() {
            $(document).ready(function() {
                // Spin para cargar el indicador en la pagina
                kendo.ui.progress($(".chart-loading"), false);
            });
            //Se obtienen los parametros para la consulta.
            idBarcoObtenido = URLactual.search.substr(59, 17);
            idViajeObtenido = URLactual.search.substr(91);
            let params = JSON.stringify({
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                idViaje : URLactual.search.substr(91)
            });
            $.ajax({
                //Obtener datos de viajes.
                url: "BarcoViajeEditar.aspx/ObtenerViajes",
                contentType: "application/json;charset=utf-8",
                type: "POST",
                data: params,
                success: function (result) {
                    if (!result.d ) {
                        result.d == [];
                    } else {
                        if (result.d.barcosViajes != "") {
                            //Se llenan los campos con el result de la consulta.
                            datosViaje = result.d.barcosViajes;
                            fechaSeleccionArribo = result.d.barcosViajes[0].fechaArribo.substr(0, 10);
                            fechaSeleccionDescarga = result.d.barcosViajes[0].fechaDescarga.substr(0, 10);
                            fechaSeleccionSalida = result.d.barcosViajes[0].fechaSalida.substr(0, 10);
                            estatusViaje = result.d.barcosViajes[0].activo;
                            descargado = result.d.barcosViajes[0].descargado;
                            descargadoEstatus = result.d.barcosViajes[0].descargado;

                            //Primera Columna
                            $("#lblSeleccionarBarcoDD").val(result.d.barcosViajes[0].barco);
                            $("#lblEjercicioDD").val(result.d.barcosViajes[0].ejercicio);
                            $("#lblViajeDD").val(result.d.barcosViajes[0].viaje)
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
                            if (result.d.barcosViajes[0].descargado == true) {
                                // si campo descargado es true, se desactiva el lbl actualizar y lbl descargado.
                                desactivarEstatus();
                                /*$('#actualizarBarcoViaje').prop("disabled", true);
                                $("#lblDescargado").prop("disabled", true);*/
                            }
                            $("#lblDescargaTinas").prop('checked', result.d.barcosViajes[0].descargaTinas);
                            $("#lblSubViaje").prop('checked', result.d.barcosViajes[0].subviaje);
                            if (result.d.barcosViajes[0].subviaje === true) {
                                // si subviaje viene en true, se desactiva el lbl subviaje.
                                $("#lblSubViaje").prop("disabled", true);
                            }
                            if (result.d.barcosViajes[0].activo === false) {
                                desactivarEstatus();
                            }
                        } else {
                            //Primera Columna
                            $("#lblSeleccionarBarcoDD").val("No hay información para mostrar");
                            $("#lblEjercicioDD").val("No hay información para mostrar");
                            $("#lblViajeDDL").val("No hay información para mostrar");
                            $("#lblSteamer").val("No hay información para mostrar");
                            $("#lblFolioIndentificador").val("No hay información para mostrar");
                            //Segunda Columna
                            $("#lblViajePesca").val("No hay información para mostrar");
                            $("#lblRSA").val("No hay información para mostrar");
                            $("#lblFechaDescargaDP").val("----/--/--");
                            $("#lblFechaSalidaDP").val("----/--/--");
                            $("#lblFechaArriboDP").val("----/--/--");
                            //BTN actualizar
                            $('#actualizarBarcoViaje').prop("disabled", true);
                            
                        }
                    }
                },
                error: function (result) {
                    options.error(result);
                    alert("Ha habido un error al cargar la información del recurso de integración");
                }
            });
        }

        $('#actualizarBarcoViaje').on('click', function(e) {
            fechaFin = fechaSeleccionArribo;
            e.preventDefault();
            //Si al cargar la pantalla el campo subviaje viene en false pero es cambiado a true.
            if (datosViaje[0].subviaje == false && $('#lblSubViaje').prop('checked') == true) {
                //Params para el subviaje 
                let param = {
                    idBarco: datosViaje[0].StringidBarco,
                    viaje: datosViaje[0].viaje,
                    ejercicio: datosViaje[0].ejercicio,
                    idSteamer: datosViaje[0].idSteamer,
                    fechaDescarga: fechaHoy,
                    fechaSalida: fechaHoy,
                    fechaArribo: fechaHoy,
                    rsa: datosViaje[0].RSA,
                    descargado: $('#lblDescargado').prop('checked'),
                    descargaTinas: $('#lblDescargaTinas').prop('checked'),
                    subviaje: $('#lblSubViaje').prop('checked'),
                    viajePesca: datosViaje[0].viajePesca,
                    idFolio: datosViaje[0].idFolio,
                    idEmbarcacion: datosViaje[0].idEmbarcacion,
                }
                let params = JSON.stringify({ datosViaje: param });
                $.ajax({
                    //Crear un subviaje del baco viaje actual. 
                    url: "BarcoViajeEditar.aspx/CrearSubviaje",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (parseInt(result.d.codigo)) {
                            case 200:
                                alert("Sub viaje creado.");
                                //Al ser creado el subviaje se redirije a la pantalla de consulta barco viaje con los parametros.
                                window.location.replace("BarcoViaje.aspx?&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin + "&StringidBarco=" + StringidBarco);
                                break;
                            case 204:
                                alert("Error al obtener los datos");
                                break;
                            case 400:
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 409:
                                alert("El barco viaje ingresado ya existe");
                                break;
                        }
                    },
                    error: function (options) {
                        options.error(options);
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                    }
                }); 
            } else {
                //Parametros para editar el barco viaje.
                let param = {
                    idViaje: datosViaje[0].StringidViaje,
                    idBarco: datosViaje[0].StringidBarco,
                    viaje: datosViaje[0].viaje,
                    ejercicio: datosViaje[0].ejercicio,
                    steamer: datosViaje[0].idSteamer,
                    fechaDescarga: fechaSeleccionDescarga,
                    fechaSalida: fechaSeleccionSalida,
                    fechaArribo: fechaSeleccionArribo,
                    RSA: $("#lblRSA").val(),
                    descargado: $('#lblDescargado').prop('checked'),
                    descargaTinas: $('#lblDescargaTinas').prop('checked'),
                    subviaje: $('#lblSubViaje').prop('checked'),
                    viajePesca: datosViaje[0].viajePesca,
                    folioIdentificador: datosViaje[0].idFolio,
                    activo: datosViaje[0].activo,
                    borrado: false,
                }
                let params = JSON.stringify({ viajeBarco: param });
                $(document).ready(function() {
                    // Spin de carga en la pagina
                    kendo.ui.progress($(".chart-loading"), true);
                });
                $.ajax({
                    //Actualizar Barco viaje
                    url: "BarcoViajeEditar.aspx/EditarBarcoViaje",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (parseInt(result.d)) {
                            case 200:
                                alert("Datos actualizados");
                                barco_Viaje();
                                datosSubviaje = [];
                                break;
                            case 204:
                                alert("Error al obtener los datos");
                                break;
                            case 400:
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 409:
                                alert("El barco viaje ingresada ya existe");
                                break;
                        }
                    },
                    error: function (options) {
                        options.error(options);
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                    }
                });
            }
        });

        $("#lblSubViaje").click(function () {
            //Sí descargado es true no puede ser modificado el subviaje
            if (descargadoEstatus === true) {
                alert("Este barco ya se encuentra descargado no puede ser modificado el estatus");
                $("#lblSubViaje").prop('checked', estatusTrue);
            } else {

            }
        });


        $("#lblEstatus").click(function () { 
            if (datosViaje[0].descargado === true) {
                //Se revisa si ya se encuentra inactivo
                alert("No es posible realizar esta acción");
                $("#lblEstatus").prop('checked', estatusViaje);
            } else {
                let  respuesta = confirm("¿Estás seguro que deseas desactivar el barco viaje?");
                if (true === respuesta) {
                    //Se realiza el procedimiento para desactivar el Barco viaje
                    $(document).ready(function() {
                    // Spin indicador de carga
                    kendo.ui.progress($(".chart-loading"), true);
                    });
                    actualizarEstatus();
                } else {
                    $("#lblEstatus").prop('checked', estatusViaje);
                }
            }                
        });

        function actualizarEstatus() {
            let barcoViaje = {
                idBarco: idBarcoObtenido,
                idViaje: idViajeObtenido
            };
            let params = JSON.stringify({ estatusBarcoViaje: barcoViaje });
            $.ajax({
                //Actualizar estatus de barco viaje
                url: "BarcoViajeEditar.aspx/EditarEstatusBarcoViaje",
                contentType: "application/json;charset=utf-8",
                type: "POST",
                data: params,
                success: function (result) {
                    if (!result.d) {
                        alert("Ha habido un error al obtener la información");
                    } else {
                        alert("Estatus Actualizado");
                        //Recarga los datos del barco viaje.
                        return barco_Viaje();
                    }
                },
                error: function (result) {
                    options.error(result);
                    alert("Ha habido un error al cargar la información del recurso de integración");
                }
            });
        }

        //kENDO DATE PICKER
        $("#lblFechaDescargaDP").kendoDatePicker({
            value: fechaHoy,
            format: "yyyy-MM-dd",
            culture: "es-MX",
            weekNumber: true,
            change: function () {
                fechaSeleccionDescarga = kendo.toString(kendo.parseDate(this.value()), 'yyyy-MM-dd');
            }
        });

        $("#lblFechaSalidaDP").kendoDatePicker({
            value: fechaHoy,
            format: "yyyy-MM-dd",
            culture: "es-MX",
            weekNumber: true,
            change: function () {
                fechaSeleccionSalida = kendo.toString(kendo.parseDate(this.value()), 'yyyy-MM-dd');
            }
        });

        $("#lblFechaArriboDP").kendoDatePicker({
            value: fechaHoy,
            format: "yyyy-MM-dd",
            culture: "es-MX",
            weekNumber: true,
            change: function () {
                fechaSeleccionArribo = kendo.toString(kendo.parseDate(this.value()), 'yyyy-MM-dd');
            }
        });

        function cambioCertificacionMS(container, options) {

            $('<input id="cambioCertificacionMS" name="cambioCertificacionMS"/>')
            .appendTo(container)
            $("#cambioCertificacionMS").kendoMultiSelect({
                placeholder: "Selecciona la certificación...",
                dataTextField: "certificacion",
                dataValueField: "idCertificacion",
                change: function (e) {
                    certificaciones = [];
                    let entrada = true;
                    let previous = this._savedOld;
                    let current = this.value();
                    let diff = [];
                    if (previous) {
                        diff = $(previous).not(current).get();
                    }
                    for (let i = 0; i < current.length; i++) {
                        certificaciones.push(current[i]);
                    }
                    for (let i = 0; i < current.length; i++) {
                        //Si la certificaciones igual a 1
                        if (current[i] === 1) {
                            entrada = false;
                            statusDolphinSave = true;
                        } else if (entrada == true) {
                            statusDolphinSave = false;
                        }
                    }
                    this._savedOld = current.slice(0);
                },
                dataSource: {
                    schema: {
                        model: {
                            id: "idCertificacion",
                            fields: {
                                certificacion: {
                                    type: "string",
                                    validation: { required: false },
                                },
                            }
                        }
                    },
                    transport: {
                        read: function (options) {
                            $.ajax({
                                //Se obtienen las certificaciones para el multiselect.
                                url: "BarcoViajeEditar.aspx/ObtenerCertificaciones",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (!result.d) {
                                        alert("Ha habido un error al obtener la información");
                                    } 
                                    options.success(result.d.certificaciones);
                                },
                                error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al cargar la información del recurso de integración");
                                }
                            });
                        }
                    }
                },
                dataBound: setvalCertificacion,
            });
        }

        function setvalCertificacion(evt) {
            let multiselect = $("#cambioCertificacionMS").data("kendoMultiSelect");
            //Se cargan las certificaciones al oprimir editar.
            multiselect.value(valores);
        }

        function cambioTanqueDDL(container, options) {

            $('<input id="cambioTanqueDDL" name="cambioTanqueDDL" required data-bind="value:value" data-required-msg="Campo requerido"/><span class="k-invalid-msg" data-for="cambioTanqueDDL" ></span>')
            .appendTo(container)
            $("#cambioTanqueDDL").kendoDropDownList({
                dataSource: {
                    schema: {
                        model: {
                            id: "StingidTanque",
                            fields: {
                                tanque: { type: "string" },
                                StingidTanque: { type: "string" },
                            }
                        }
                    },
                    transport: {
                        read: function (options) {
                            let params = JSON.stringify({
                                idBarco: idBarcoObtenido,
                            });
                            $.ajax({
                                //Obtener los tanuqes.
                                url: "BarcoViajeEditar.aspx/ObtenerTanques",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    if (!result.d) {
                                        alert("Ha habido un error al obtener la información");
                                    } else {
                                        for (i = 0; i <= result.d.tanques.length - 1; i++) {
                                            listaIdTanques[i] = result.d.tanques[i].StingidTanque;
                                            if (result.d.tanques[i].StingidTanque == idTanques) {
                                                //Se guarda el indice para ser mostrado en el DDL al oprimir editar. 
                                                indiceTanques = i;
                                            }
                                        }
                                        options.success(result.d.tanques);
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
                dataTextField: "tanque",
                dataValueField: "StingidTanque",
                change: function () {
                    idTanque = this.value();
                },
                select: onSelectTanque,
                dataBound: setIndiceTanque
            });
        }

        function setIndiceTanque(evt) {
            //Se carga el ddl en el indice que le corresponde 
            if (evt.sender.dataSource.data().length > 0) {
                evt.sender.select(indiceTanques);
                StingidTanque = evt.sender.dataSource.data()[indiceTanques].StingidTanque;
            }
            idTanque = StingidTanque;
            idTanqueSeleccionado = StingidTanque;
            evt.sender.dataBound = false;
        }

        function onSelectTanque(e) {
            //Obtiene la descripcion de la talla seleccionada  
            let dataItem = e.dataItem;
            idTanqueSeleccionado = dataItem.StingidTanque;
        };

        function guardarTanqueBtn(e) {
            let tanquesGrid = $("#tanquesGrid").data("kendoGrid");
            let param = {
                idViaje: idViajeObtenido,
                idBarco: idBarcoObtenido,
                idTanque: idTanqueSeleccionado,
                dolphinSave: statusDolphinSave, 
                bloqueo: false,
                dondeEstoy: 1,
                objFlot: false,
                activo: true,
                idCertificacion: certificaciones
            }
            let params = JSON.stringify({ tanqueCertificacion: param });
            $.ajax({
                //Crear tanque con certificacion
                url: "BarcoViajeEditar.aspx/CrearTanqueCertificacion",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                data: params,
                success: function (result) {
                    switch (result.d) {
                        case 200:
                            valores = [];
                            certificaciones = [];
                            tanquesGrid.dataSource.read();
                            break;
                        case 204:
                            tanquesGrid.dataSource.read();
                            alert("Error al identificar el elemento a crear");
                            break;
                        case 304:
                            valores = [];
                            certificaciones = [];
                            tanquesGrid.dataSource.read();
                            alert("No es posible guardar este registro");
                            break;
                        case 400:   
                            tanquesGrid.dataSource.read();
                            alert("El flujo de integración ha identificado un problema al tratar de crear el registro");
                            break;
                            case 405:
                            tanquesGrid.dataSource.read();
                            alert("La sesión ha expirado, la operación no puede ser completada");
                            break;
                    }
                },
                error: function(result){
                    alert("Ha habido un error al crear el elemento, por favor intente de nuevo");
                    tanquesGrid.dataSource.read();
                }    
            });
        };

        function editarTanque(e) {
            let tanquesGrid = $("#tanquesGrid").data("kendoGrid");
            //Se verifica que no se guarden tanques sin certificaciones. 
            if (certificaciones.length == 0) {
                tanquesGrid.dataSource.read();
                alert("No es posible guardar tanques sin certificaciones");
            } else {
                let param = {
                    idViaje: idViajeObtenido,
                    idBarco: idBarcoObtenido,
                    idTanque: idTanqueSeleccionado,
                    activo: true,
                    idCertificacion: certificaciones
                }
                let params = JSON.stringify({ tanqueCertificacion: param });
                $.ajax({
                    //Se editan los tanques certificaciones
                    url: "BarcoViajeEditar.aspx/EditarTanqueCertificacion",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                valores = [];
                                certificaciones = [];
                                tanquesGrid.dataSource.read();
                                break;
                            case 204:
                                valores = [];
                                certificaciones = [];
                                tanquesGrid.dataSource.read();
                                alert("Error al identificar el elemento a crear");
                                break;
                                case 304:
                                tanquesGrid.dataSource.read();
                                alert("No fue posible guardar este registro");
                                break;
                            case 400:   
                                tanquesGrid.dataSource.read();
                                alert("El flujo de integración ha identificado un problema al tratar de crear el registro");
                                break;
                                case 405:
                                tanquesGrid.dataSource.read();
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                        }
                    },
                    error: function (result) {
                        alert("Ha habido un error al crear el elemento, por favor intente de nuevo");
                        tanquesGrid.dataSource.read();
                    }    
                });
            }
        };
       

        window.tanquesGrid = $("#tanquesGrid").kendoGrid({
            columns: [
                {
                    field: "descripcionTanque",
                    title: "Tanques",
                    editor: cambioTanqueDDL,
                    width: 100

                },
                {
                    field: "listaCertificaciones",
                    title: "Certificación",
                    editor: cambioCertificacionMS,
                    width: 100

                },
                {
                    command: [
                        {
                            name: "edit",
                            text: {
                                edit: "Editar",
                                cancel: "Cancelar",
                                update: "Guardar",
                            },
                        },
                        {
                            name: "destroy",
                            text: "Eliminar",
                        },
                    ],
                    title: "",
                    width: 140
                }
            ],
            dataSource: {
                sync: function () {
                    this.read();
                },
                pageSize: 15,
                schema: {
                    model: {
                        id: "idTanqueCertificacion",
                        fields: {
                            descripcionTanque: {
                                type: "string",
                                required: {
                                    message: "Campo requerido",
                                }
                            },
                            listaCertificaciones: {
                                type: "string",
                            },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        let params = JSON.stringify({
                            idViaje: idViajeObtenido,
                        });
                        $.ajax({
                            //Se obtienen los tanques certificaciones. 
                            url: "BarcoViajeEditar.aspx/ObtenerTanquesCertificaciones",
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
                    create: function (options) {/* ... */},
                    update: function (options) { /* ... */ },
                    destroy: function (options) {
                        let params = JSON.stringify({
                            idBarco: idBarcoObtenido,
                            idViaje: idViajeObtenido,
                            idTanque: options.data.idTanque,
                        });
                        $.ajax({
                            //Se eliminan los tanques certificaciones. 
                            url: "BarcoViajeEditar.aspx/EliminarTanquesCertificaciones",
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
            editable: {
                confirmation: "¿Estás seguro que deseas eliminar este elemento?",
                mode: "inline"
            },
            edit: function (e) {
                if (!e.model.isNew()) {
                    //Si es un registro existente en el grid inactiva el DDL de tanques
                    $("#cambioTanqueDDL").data("kendoDropDownList").enable(false);
                    idTanques = e.model.idTanque;//Se muestra la descripcion del tanque 
                    listaCertificaciones = e.model.listaIdCertificaciones;
                    valores = Array.from(listaCertificaciones);
                } else {
                    idTanques = "0";
                    indiceTanque = 0;
                    $("#cambioTanqueDDL").data("kendoDropDownList").enable(true);
                }

                if (idViajeObtenido === "000") {
                    let tanquesGrid = $("#tanquesGrid").data("kendoGrid");
                    tanquesGrid.dataSource.read();
                    alert("Es necesario agregar primero el barco-viaje");
                    //Se deshabilita el botón de agregar
                    $('.k-grid-add').prop("disabled", true);
                    $('.k-grid-add').addClass("k-grid-add-disabled");
                } else {
                    //Se deshabilita el botón de agregar
                    $('.k-grid-add').prop("disabled", false);
                    $('.k-grid-add').removeClass("k-grid-add-disabled");
                }
                    
            },
            save: function (e) {
                if (!e.model.isNew()) {
                    editarTanque();
                } else {
                    guardarTanqueBtn();
                }
            },
            cancel: function (e) {
                idTanques = "0";
                indiceTanque = 0;
                valores = [];
                certificaciones = [];
                //Se habilita el botón de agregar
                $("#cambioTanqueDDL").data("kendoDropDownList").enable(true);
                $('.k-grid-add').prop("disabled", false);
                $('.k-grid-add').removeClass("k-grid-add-disabled");
            },
            toolbar: [{ name: "create", text: "" }],
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

        function productoDDL(container, options) {

            $('<input id="productoDDL" name="productoDDL" required data-bind="value:value" data-required-msg="Campo requerido"/><span class="k-invalid-msg" data-for="productoDDL" ></span>')
            .appendTo(container)
            $("#productoDDL").kendoDropDownList({
                dataTextField: "producto",
                dataValueField: "StringidProducto",
                dataSource: {
                    schema: {
                        model: {
                            id: "StringidProducto",
                            fields: {
                                talla: { type: "string" },
                                clave: { type: "string" },
                                StringidProducto: {type: "string"}
                            }
                        }
                    },
                    transport: {
                        read: function (options) {
                            $.ajax({
                                //Obtener productos
                                url: "BarcoViajeEditar.aspx/ObtenerProducto",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (!result.d) {
                                        alert("Ha habido un error al obtener la información");
                                    } else {
                                        for (i = 0; i <= result.d.catalogoProductos.length - 1; i++) {
                                            listaIdProdicto = result.d.catalogoProductos[i].idProducto;
                                            if (result.d.catalogoProductos[i].producto == desProducto) {
                                                //Se toma el indice del producto para su edicion. 
                                                indiceProducto = i;
                                            }
                                        }
                                        options.success(result.d.catalogoProductos);
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
                messages: {
                    noData: "No hay datos para mostrar"
                },
                change: function () {
                    StringidProducto = this.value();
                },
                dataBound: setIndiceProducto,
                select: onSelectProducto,
            });
        }

        function setIndiceProducto(evt) {
            //Se carga el ddl en el indice que le corresponde 
            idProductoS = "";
            if (evt.sender.dataSource.data().length > 0) {
                evt.sender.select(indiceProducto);
                StringidProducto = evt.sender.dataSource.data()[indiceProducto].StringidProducto;
                idProductoS = StringidProducto;
            }
            evt.sender.dataBound = false;
        }

        function onSelectProducto(e) {
            //Obtiene la descripcion de la seleccionada 
            idProductoS = "";
            let dataItem = e.dataItem;
            idProductoS = dataItem.StringidProducto;
        };


        window.productoGrid = $("#productoGrid").kendoGrid({
            columns: [
                {
                    field: "descripcionProducto",
                    title: "Producto",
                    editor: productoDDL,
                    width: 100

                },
                {
                    field: "cantidad",
                    title: "Cantidad",
                    width: 100,
                },
                {
                    command: [
                        {
                            name: "edit",
                            text: {
                                edit: "Editar",
                                update: "Guardar",
                                cancel: "Cancelar"
                            },

                        },
                        {
                            name: "destroy",
                            text: "Eliminar",
                        },

                    ],
                    title: "",
                    width: 140
                }

            ],
            dataSource: {
                sync: function () {
                    this.read();
                },
                pageSize:15,
                schema: {
                    model: {
                        id: "idViajeProducto",
                        fields: {
                            descripcionProducto: {
                                type: "string"
                            },
                            cantidad: {
                                type: "number",
                                validation: {
                                    required: {
                                        message:"Campo requerido",
                                    },
                                    min: 0,
                                },
                            },
                            idViajeProducto: {
                                type: "number"
                            },
                            StringidProducto: {
                                type: "string"
                            }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        let params = JSON.stringify({
                            idViaje: idViajeObtenido,
                        });
                        $.ajax({
                            //Se obtiene los productos del viaje 
                            url: "BarcoViajeEditar.aspx/ObtenerViajesProductos",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            data: params,
                            success: function (result) {
                                if (!result.d) {
                                    alert("Ha habido un error al obtener la información");
                                } else {
                                    options.success(result.d.viajesProductos);
                                }
                            },
                            error: function (result) {
                                result.error(result);
                                alert("Ha habido un error al cargar la información");
                            },
                        });
                    },
                    create: function (options) {
                        //Validacion para no guardar contenedores sin productos
                        if (options.data.cantidad <= 0) {
                            alert("No es posible guardar un contenedor sin poducto");
                            let productoGrid = $("#productoGrid").data("kendoGrid");
                            productoGrid.dataSource.read();
                        } else {
                            let param = {
                                idViaje: idViajeObtenido,
                                idProducto: idProductoS,
                                ejercicio: $('#lblEjercicioDD').val(),
                                cantidad: options.data.cantidad,
                                activo: true
                            }
                            let params = JSON.stringify({ producto: param });
                            $.ajax({
                                //Crear producto para el barco viaje
                                url: "BarcoViajeEditar.aspx/CrearProducto",
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    switch (result.d) {
                                        case 200:
                                            options.success();
                                            break;
                                        case 204:
                                            options.success();
                                            alert("Error al identificar el elemento a crear");
                                            break;
                                        case 304:
                                            options.success();
                                            alert("Error al modificar el registro");
                                            break;
                                        case 400:
                                            options.success();
                                            alert("El flujo de integración ha identificado un problema al tratar de crear el registro");
                                            break;
                                        case 405:
                                            options.success();
                                            alert("La sesión ha expirado, la operación no puede ser completada");
                                            break;
                                        case 409:
                                            options.error();
                                            alert("La Especialidad ingresada ya existe");
                                            break;
                                    }
                                },
                                error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al crear el elemento, por favor intente de nuevo");
                                }
                            });
                        }
                    },
                    update: function (options) {
                        //Validacion para no guardar contenedores sin productos
                        if (options.data.cantidad <= 0) {
                            alert("No es posible guardar un contenedor sin poducto");
                            let productoGrid = $("#productoGrid").data("kendoGrid");
                            productoGrid.dataSource.read();
                        } else {
                            let param = {
                                idViajeProducto: options.data.idViajeProducto,
                                idViaje: idViajeObtenido,
                                idProducto: idProductoS,
                                ejercicio: $('#lblEjercicioDD').val(),
                                cantidad: options.data.cantidad,
                                activo: true,
                                borrado: false,
                            }
                            let params = JSON.stringify({ producto: param });
                            $.ajax({
                                //Editar producto y cantidad de baco viaje
                                url: "BarcoViajeEditar.aspx/EditarProducto",
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    switch (result.d) {
                                        case 200:
                                            options.success();
                                            break;
                                        case 204:
                                            options.success();
                                            alert("Error al identificar el elemento a crear");
                                            break;
                                        case 400:
                                            options.success();
                                            alert("El flujo de integración ha identificado un problema al tratar de crear el registro");
                                            break;
                                        case 405:
                                            options.success();
                                            alert("La sesión ha expirado, la operación no puede ser completada");
                                            break;
                                        case 409:
                                            options.error();
                                            alert("La Especialidad ingresada ya existe");
                                            break;
                                    }
                                },
                                error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al crear el elemento, por favor intente de nuevo");
                                }
                            });
                        }
                    },
                    destroy: function (options) {
                        let params = JSON.stringify({
                            idViajeProducto: options.data.idViajeProducto,
                            idViaje: idViajeObtenido,
                            idProducto: options.data.idProducto,
                        });
                        $.ajax({
                            //Eliminar producto del barco viaje
                            url: "BarcoViajeEditar.aspx/EliminarProductos",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            data: params,
                            success: function (result) {
                                if (!result.d) {
                                    alert("Ha habido un error al obtener la información");
                                }
                                options.success(result.d.viajesProductos);
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
            editable: {
                confirmation: "¿Estás seguro que deseas eliminar este elemento?",
                mode: "inline"
            },
            edit: function (e) {
                //Se verifica que cuando se edite un row este sea nuevo o no.
                if (!e.model.isNew()) {
                    $("#productoDDL").data("kendoDropDownList").enable(false);
                    desProducto = e.model.descripcionProducto;
                } else {
                    indiceProducto = 0;
                    idProducto = "";
                    desProducto = "";
                    $("#productoDDL").data("kendoDropDownList").enable(true);
                }

                if (idViajeObtenido === "000") {
                    let productoGrid = $("#productoGrid").data("kendoGrid");
                    productoGrid.dataSource.read();
                    alert("Es necesario agregar primero el barco-viaje");
                    //Se deshabilita el botón de agregar
                    $('.k-grid-add').prop("disabled", true);
                    $('.k-grid-add').addClass("k-grid-add-disabled");
                } else {
                    //Se deshabilita el botón de agregar
                    $('.k-grid-add').prop("disabled", false);
                    $('.k-grid-add').removeClass("k-grid-add-disabled");
                }
            },
            cancel: function (e) {
                indiceProducto = 0;
                idProducto = "";
                desProducto = "";
                $("#productoDDL").data("kendoDropDownList").enable(true);
                //Se habilita el botón de agregar
                $('.k-grid-add').prop("disabled", false);
                $('.k-grid-add').removeClass("k-grid-add-disabled");
            },
            toolbar: [{ name: "create", text: "" }],
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

        $('#lblRSA').on('input', function () {
            let value = $(this).val();
            if ((value !== '') && (value.indexOf('.') === -1)) {
                $(this).val(Math.max(Math.min(value, 9999999999999999999), 0));
            }
        });

        $("#btnSalir").click(function () {
            //Cuando se oprime el boton salir se retorna a barco viaje consulta con las variables. 
            window.location.replace("BarcoViaje.aspx?&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin + "&StringidBarco=" + StringidBarco);
        });

    });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Editar barco - viaje" id="hPage" />

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
                                                            <dd id="fechaDescargaDD" class="col-sm k-dropdown-custome" >
                                                                <input id="lblFechaDescargaDP" class="col-sm color-DP"/>
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
                                                            <dd id="fechaSalidaDD" class="col-sm k-dropdown-custome">
                                                                <input id="lblFechaSalidaDP" class="col-sm color-DP"/>
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
                                                            <dd id="fechaArriboDD" class="col-sm k-dropdown-custome">
                                                                <input id="lblFechaArriboDP" class="col-sm color-DP"/>
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Steamer
                                                            </dt>
                                                            <dd id="steamerDD" class="col-sm column-line">
                                                                <input id="lblSteamer" type="text" class="form-control text-row"  aria-describedby="basic-addon2"/>
                                                            </dd>
                                                            <dt class="col-sm">
                                                                Viaje de pesca
                                                            </dt>
                                                            <dd id="viajePescaDD" class="col-sm">
                                                                <input id="lblViajePesca" type="text" class="form-control text-row"  aria-describedby="basic-addon2"/>
                                                            </dd>
                                                        </div>
                                                        <div class="each-row">
                                                            <dt class="col-sm">
                                                                Folio Id.
                                                            </dt>
                                                            <dd id="folioIdentificadorDD" class="col-sm column-line">
                                                                <input id="lblFolioIndentificador" type="text" class="form-control text-row"  aria-describedby="basic-addon2"/>
                                                            </dd>
                                                            <dt class="col-sm">
                                                                RSA
                                                            </dt>
                                                            <dd id="rsaDD" class="col-sm">
                                                                <input id="lblRSA" type="number" type="number" min="0" class="form-control " placeholder="Ingresa RSA"/>
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
                                                        <div class="four-button-contain" style="background-color:white">
                                                        <button id="actualizarBarcoViaje" class="k-primary k-button pushAgregar">Actualizar barco - viaje</button>
                                                    </div>
                                                    </dl>
                                                    
                                                </div>
                                            </div>
                                            <div class="chart-loading"></div>
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
                                                <a id="btnSalir" class="k-primary k-button">Salir</a>
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
            .k-i-warning {
                font-size: 30px;
                margin: 0px;
                width: 60%;
                color: white;
            }

            .k-checkbox:checked + .k-checkbox-label:before {
                background:  white;
                color: #3c8dbc;
            }

            .color-DP {
                width: 10em;
            }

            .column-line {
                width: 52% !important;
            }

            @media (max-width: 1280px) {
              .column-line {
                    width: 44% !important;
                    margin-left: -23px;
              }
            }
        </style>
    </section>
</asp:Content>
