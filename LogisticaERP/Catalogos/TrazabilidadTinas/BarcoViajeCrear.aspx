<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="BarcoViajeCrear.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.BarcoViajeCrear" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>

    $(function () {
        kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;

        let fechaHoy = kendo.toString(kendo.parseDate(new Date()), 'yyyy-MM-dd');
        let URLactual = window.location;

        let idProductoS = "";
        let indiceProducto = 0;
        let desProducto = "";

        let viaje = "";
        let idTanques = "0";
        let indiceTanques = 0;

        let valores = [];
        let listaCertificaciones = "";
        let nombreBarco = [];
        let datosBarcos = [];
        let nombreSteamer = [];
        let codigoViajePesca = [];
        let datosSteamer = [];
        let certificaciones = [];
        let idEmpresaObtenido = "0";
        let idViajeObtenido = "000"; //Se retorna al ser creado el Barco Viaje
        let idTanqueSeleccionado = "";
        let statusDolphinSave = false;

        //Crear Barco Viaje.
        let idBarcoSeleccionado = "000";
        let ejercicio = "";
        let idSteamerSeleccionado = "0";
        let fehcaDescarga = kendo.toString(kendo.parseDate(new Date()), 'yyyy-MM-dd');
        let fehcaSalida = kendo.toString(kendo.parseDate(new Date()), 'yyyy-MM-dd');
        let fehcaArribo = kendo.toString(kendo.parseDate(new Date()), 'yyyy-MM-dd');
        let rsa = "";
        let viajePesca = 0;
        let idFolio = 0;
        let idEmbarcacion = 0;

        desactivarInput();

        let StringidBarco = "000";
        let fechaInicio = "";

        if (URLactual.href.length > 70) {
            //Se cargan los datos del barco viaje seleccionado
            fechaInicio = URLactual.search.substr(14, 10);
            fechaFin = URLactual.search.substr(34, 10);
            StringidBarco = URLactual.search.substr(59, 17);
        }

        function desactivarInput() {
            //Se desactivan los campos de la pantalla.
            $('#lblSeleccionarBarcoDD').prop("disabled", true);
            $('#lblViajePescaDD').prop("disabled", true);
            $('#lblSeleccionarSteamerDD').prop("disabled", true);
            $('#viajeDDL').prop("disabled", true);
        }

        $("#btnAgregarBarco").kendoButton({
            click: function (e) {
                e.preventDefault();
                mostrarModalBarcos();
                obtenerDatosBarcos();
                $('#lblViajePescaDD').val("Selecciona viaje pesca");
                let popUpBarcosGrid = $("#popupBarcosGrid").data("kendoGrid");
                popUpBarcosGrid.dataSource.read();
            }
        });

        $("#btnAgregarSteamer").kendoButton({
            click: function (e) {
                e.preventDefault();
                //Valida que se haya seleccionado un barco 
                if (idBarcoSeleccionado === "000") {
                    alert("Selecciona el barco para poder agregar el Steamer");
                } else {
                    mostrarModalSteamer();
                    obtenerDatosSteamer();
                    let popUpSteamerGrid = $("#popupSteamerGrid").data("kendoGrid");
                    popUpSteamerGrid.dataSource.read();
                }
            }
        });

        $("#btnAgregarViajePesca").kendoButton({
            click: function (e) {
                e.preventDefault();
                //Valida que se haya seleccionado un barco
                if (idBarcoSeleccionado === "000") {
                    alert("Selecciona el barco para poder agregar el Steamer");
                } else {
                    mostrarModalViajePesca();
                    let popUpViajePescaGrid = $("#popupViajePescaGrid").data("kendoGrid");
                    popUpViajePescaGrid.dataSource.read();
                }
            }
        });

        function desactivarAgregar() {
            //Al agregar un barco se deshabilitan los campos. 
            $('#btnAgregarBarco').prop("disabled", true);
            $('#lblEjercicioDD').prop("disabled", true);
            $('#folioIdentificadorDD').prop("disabled", true);
            $('#btnAgregarViajePesca').prop("disabled", true);
            $('#btnAgregarSteamer').prop("disabled", true);
            $('#viajeDDL').prop("disabled", true);
            $('#lblRSA').prop("disabled", true);
            $('#agregarBarcoViaje').prop("disabled", true);
            let fechaArriboDP = $("#fechaArriboDP").data("kendoDatePicker");
            let fechaSalidaDP = $("#fechaSalidaDP").data("kendoDatePicker");
            let fechaDescargaDP = $("#fechaDescargaDP").data("kendoDatePicker");
            let folioIdentificadorDDL = $("#folioIdentificadorDDL").data("kendoDropDownList");
            fechaArriboDP.enable(false);
            fechaSalidaDP.enable(false);
            fechaDescargaDP.enable(false);
            folioIdentificadorDDL.enable(false);
        };

        $('#agregarBarcoViaje').on('click', function (e) {
            e.preventDefault();
            ejercicio = $("#lblEjercicioDD").val();
            viaje = $("#viajeDDL").val();
            rsa = $("#lblRSA").val();
            //Se revisan que los campos esten completos antes de ser creado. 
            if (idBarcoSeleccionado ===  "000") {
                alert("Selecciona un Barco ");
            } else if (viaje === "No cuenta con viaje") {
                alert("No cuenta con un viaje asignado")
            } else if (ejercicio === "") {
                alert("Agregue el ejercicio ")
            } else if (rsa === "") {
                alert("Agregue el RSA")
            } else {
                        var param = {
                            idBarco: idBarcoSeleccionado,
                            viaje: $('#viajeDDL').val(),
                            ejercicio: ejercicio,
                            idSteamer: idSteamerSeleccionado,
                            fechaDescarga: fehcaDescarga,
                            fechaSalida: fehcaSalida,
                            fechaArribo: fehcaArribo,
                            RSA: rsa,
                            descargado: false,
                            descargaTinas: false,
                            subviaje: false,
                            viajePesca: viajePesca,
                            idFolio: idFolio,
                            idEmbarcacion: idEmbarcacion,
                        }
                        let params = JSON.stringify({ viajeBarco: param });
                        $.ajax({
                            //Se crea viaje para el barco
                            url: "BarcoViajeCrear.aspx/CrearBarcoViaje",
                            contentType: "application/json; charset=utf-8",
                            type: "POST",
                            data: params,
                            success: function (result) {
                                switch (parseInt(result.d.codigo)) {
                                    case 200:
                                        alert("El Barco viaje ha sido creado");
                                        desactivarAgregar();
                                        //Se extrae el viaje obtenido
                                        idViajeObtenido = result.d.idViaje;
                                        if (idViajeObtenido != "000") {
                                            $('.k-grid-add').prop("disabled", false);
                                            $('.k-grid-add').removeClass("k-grid-add-disabled");
                                            $('#agregarBarcoViaje').prop("disabled", true);
                                        }
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
                                        alert("Ha ocurrido un problema al ejecutar el flujo 'postBarcosViajes'. Póngase en contacto con el departamento de CAU.");
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

        function obteneViaje() {
            let params = JSON.stringify({
                idBarco: idBarcoSeleccionado,
                ejercicio: $('#lblEjercicioDD').val()
            });
            $.ajax({
                //Obtener id viaje de BD.
                url: "BarcoViajeCrear.aspx/ObtenerViaje",
                contentType: "application/json;charset=utf-8",
                type: "POST",
                data: params,
                success: function (result) {
                    if (!result.d) {
                        alert("Ha habido un error al obtener la información");
                    } else if (result.d.sigViaje.numeroViaje != null) {
                        $('#viajeDDL').val(result.d.sigViaje.numeroViaje)
                    } else {
                        $('#viajeDDL').val("No cuenta con viaje")
                    }
                },
                error: function (result) {
                    options.error(result);
                    alert("Ha habido un error al cargar la información del recurso de integración");
                }
            });
        }

        $("#folioIdentificadorDDL").kendoDropDownList({
            dataTextField: "folioMP",
            dataValueField: "idFolio",
            optionLabel: "Selecciona folio identificador",
            dataSource: {
                transport: {
                    read: function (options) {
                        let params = JSON.stringify({
                            idEmpresa : idEmpresaObtenido
                        })
                        $.ajax({
                            //Obtener folio identificador
                            url: "BarcoViajeCrear.aspx/ObtenerFolioIdentificador",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            data: params,
                            success: function (result) {
                                if (!result.d) {
                                    alert("Ha habido un error al obtener la información");
                                }
                                options.success(result.d.folios);
                            },
                            error: function (result) {
                                options.error(result);
                                alert("Ha habido un error al cargar la información del recurso de integración");
                            }
                        });
                    }
                }
            },
            change: dropdownlistFI,
            messages: {
              noData: "No hay datos para mostrar"
            },
            open: onOpenFolio,
        });

        function dropdownlistFI() {
            var dropdownlist = $("#folioIdentificadorDDL").data("kendoDropDownList");
            //Se obtiene el id Folio 
            idFolio = dropdownlist.dataItem().idFolio;
        }

        function onOpenFolio() {
            //Se valida que primero se seleccione el barco
            if (idBarcoSeleccionado === "000") {
                alert("Selecciona el barco para obtener la lista de folio identificador");
            }
        };

        //kENDO DATE PICKER
        $("#fechaDescargaDP").kendoDatePicker({
            value: fechaHoy,
            format: "yyyy-MM-dd",
            culture: "es-MX",
            weekNumber: true,
            change: function () {
                fehcaDescarga = kendo.toString(kendo.parseDate(this.value()), 'yyyy-MM-dd');
            }
        });

        $("#fechaSalidaDP").kendoDatePicker({
            value: fechaHoy,
            format: "yyyy-MM-dd",
            culture: "es-MX",
            weekNumber: true,
            change: function () {
                fehcaSalida = kendo.toString(kendo.parseDate(this.value()), 'yyyy-MM-dd');
            }
        });

        $("#fechaArriboDP").kendoDatePicker({
            value: fechaHoy,
            format: "yyyy-MM-dd",
            culture: "es-MX",
            weekNumber: true,
            change: function () {
                fehcaArribo = kendo.toString(kendo.parseDate(this.value()), 'yyyy-MM-dd');
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
                        //Si la certificaciones es igual a 1
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
                                url: "BarcoViajeCrear.aspx/ObtenerCertificaciones",
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
            //se cargan los valores del multiselect al editar
            let multiselect = $("#cambioCertificacionMS").data("kendoMultiSelect");
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
                                idBarco: idBarcoSeleccionado,
                            });
                            $.ajax({
                                //Obtener los tanques para el DDL
                                url: "BarcoViajeCrear.aspx/ObtenerTanques",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    if (!result.d) {
                                        alert("Ha habido un error al obtener la información");
                                    } else {
                                        for (i = 0; i <= result.d.tanques.length - 1; i++) {
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
                idBarco: idBarcoSeleccionado,
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
                url: "BarcoViajeCrear.aspx/CrearTanqueCertificacion",
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
                idBarco: idBarcoSeleccionado,
                idTanque: idTanqueSeleccionado,
                activo: true,
                idCertificacion: certificaciones
                }
                let params = JSON.stringify({ tanqueCertificacion: param });
                $.ajax({
                    //Se editan los tanques certificaciones
                    url: "BarcoViajeCrear.aspx/EditarTanqueCertificacion",
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
                    create: function (options) {/* ... */},
                    update: function (options) { /* ... */ },
                    destroy: function (options) {
                        let params = JSON.stringify({
                            idBarco: idBarcoSeleccionado,
                            idViaje: idViajeObtenido,
                            idTanque: options.data.idTanque,
                        });
                        $.ajax({
                            //Se eliminan los tanques certificaciones.
                            url: "BarcoViajeCrear.aspx/EliminarTanquesCertificaciones",
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
                    idTanques = e.model.idTanque;
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
                                url: "BarcoViajeCrear.aspx/ObtenerProducto",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (!result.d) {
                                        alert("Ha habido un error al obtener la información");
                                    } else {
                                        for (i = 0; i <= result.d.catalogoProductos.length - 1; i++) {
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
                            url: "BarcoViajeCrear.aspx/ObtenerViajesProductos",
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
                                url: "BarcoViajeCrear.aspx/CrearProducto",
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
                                //Editar producto y cantidad de barco viaje
                                url: "BarcoViajeCrear.aspx/EditarProducto",
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
                            url: "BarcoViajeCrear.aspx/EliminarProductos",
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

        //BARCOS

        function obtenerDatosBarcos() {
            $.ajax({
                //Obtener el catalogo de barcos
                url: "BarcoViajeCrear.aspx/ObtenerBarcos",
                contentType: "application/json;charset=utf-8",
                type: "POST",
                success: function (result) {
                    if (!result.d) {
                        alert("Ha habido un error al obtener la información");
                    } else {
                        for (var i = 0; i < result.d.catalogoBarcos.length; i++) {
                            //Se guardan estos parametros para ser utilizados en los autocomplete 
                            nombreBarco[i] = result.d.catalogoBarcos[i].barco;
                            datosBarcos[i] = result.d.catalogoBarcos[i];
                        }
                    }

                    
                },
                error: function (result) {
                    result.error(result);
                    alert("Ha habido un error al cargar la información");
                },
            });
        }

        $("#barcosAC").kendoAutoComplete({
            filter: "contains",
            placeholder: "Ingrese nombre del barco",
            dataSource: {
                //Son obtenidos de el caltalogo de barcos.
                data: nombreBarco
            },
            change: function (e) {
                for (var x = 0; x < datosBarcos.length; x++) {
                    if (datosBarcos[x].barco == this.value()) {
                        idBarcoSeleccionado = datosBarcos[x].StringIdBarco;
                        idEmpresaObtenido = datosBarcos[x].StringidEmpresa;
                        let popUpBarcosGrid = $("#popupBarcosGrid").data("kendoGrid");
                        popUpBarcosGrid.dataSource.read();
                    }
                }
            }
        });

        function agregarBarcoBtn(e) {
            //Popup Agregar barco. 
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            $("#lblSeleccionarBarcoDD").val(dataItem.barco);
            let dropdownlistFolio = $("#folioIdentificadorDDL").data("kendoDropDownList");
            dropdownlistFolio.dataSource.read();
            $('#viajeDDL').val("");
            $('#lblEjercicioDD').val("");
            alert("Dato seleccionado");
        }

        $("#popupBarcosGrid").kendoGrid({
            columns: [
                {
                    field: "barco",
                    title: "Barco",
                    editable: () => { return false },
                    width: 110,
                },
                {
                    field: "clave",
                    title: "Clave",
                    editable: () => { return false },
                    width: 110,
                },
                {
                    field: "nombreComercial",
                    title: "Nombre Comercial",
                    editable: () => { return false },
                    width: 100,
                },
                {
                    command: [
                        {
                            click: agregarBarcoBtn,
                            text: "Agregar",
                        },
                    ],
                    title: "",
                    width: 140
                }
            ],
            autoBind: false,
            dataSource: {
                sync: function () {
                    this.read();
                },
                schema: {
                    model: {
                        id: "StringIdBarco",
                        fields: {
                            StringIdBarco: {
                                type: "string"
                            },
                            barco: {
                                type: "string"
                            },
                            clave: {
                                type: "string"
                            },
                            nombreComercial: {
                                type: "string",
                            }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        let params = JSON.stringify({
                            idBarco: idBarcoSeleccionado,
                        });
                        $.ajax({
                            //Obtener detalle del barco seleccionado. 
                            url: "BarcoViajeCrear.aspx/ObtenerDetalleBarcos",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            data: params,
                            success: function (result) {
                                if (!result.d) {
                                    alert("Ha habido un error al obtener la información");
                                } else {
                                    options.success(result.d.catalogoBarcos);
                                }

                                
                            },
                            error: function (result) {
                                result.error(result);
                                alert("Ha habido un error al cargar la información");
                            },
                        });
                    }
                },
                pageSize: 24,
            },
            resizable: true,
            toolbar: [],
            editable: true,
            batch: true,
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

        //Steamer

        function obtenerDatosSteamer() {
            $.ajax({
                //Se obtienen los steamer del catalogo. 
                url: "BarcoViajeCrear.aspx/ObtenerSteamer",
                contentType: "application/json;charset=utf-8",
                type: "POST",
                success: function (result) {
                    if (!result.d) {
                        alert("Ha habido un error al obtener la información");
                    } else {
                        for (var i = 0; i < result.d.steamers.length; i++) {
                            //Se guardan estos parametros para ser utilizados en los autocomplete
                            nombreSteamer[i] = result.d.steamers[i].steamer;
                            datosSteamer[i] = result.d.steamers[i];
                        }
                    }

                    
                },
                error: function (result) {
                    result.error(result);
                    alert("Ha habido un error al cargar la información");
                },
            });
        }

        $("#steamerAC").kendoAutoComplete({
            filter: "contains",
            placeholder: "Ingrese nombre del Steamer",
            dataSource: {
                //Se obtienen del catalogo de la llamada al catalogo del steamer.
                data: nombreSteamer
            },
            change: function (e) {
                for (var x = 0; x < datosSteamer.length; x++) {
                    if (datosSteamer[x].steamer == this.value()) {
                        idSteamerSeleccionado = datosSteamer[x].StringidSteamer
                        let popupSteamerGrid = $("#popupSteamerGrid").data("kendoGrid");
                        popupSteamerGrid.dataSource.read();
                    }
                }
            }
        });

        function agregarSteamerBtn(e) {
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            $("#lblSeleccionarSteamerDD").val(dataItem.steamer);
            alert("Dato seleccionado");
            e.preventDefault();
        }

        $("#popupSteamerGrid").kendoGrid({
            columns: [
                {
                    field: "steamer",
                    title: "Steamer",
                    editable: () => { return false },
                    width: 110,
                },
                {
                    field: "claveSteamer",
                    title: "Clave",
                    editable: () => { return false },
                    width: 110,
                },
                {
                    field: "nombreComercial",
                    title: "Nombre Comercial",
                    editable: () => { return false },
                    width: 100,
                },
                {
                    command: [
                        {
                            click: agregarSteamerBtn,
                            text: "Agregar",
                        },
                    ],
                    title: "",
                    width: 140
                }
            ],
            autoBind: false,
            dataSource: {
                sync: function () {
                    this.read();
                },
                schema: {
                    model: {
                        id: "StringidSteamer",
                        fields: {
                            StringidSteamer: {
                                type: "string"
                            },
                            steamer: {
                                type: "string"
                            },
                            claveSteamer: {
                                type: "string"
                            },
                            nombreComercial: {
                                type: "string",
                            }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        let params = JSON.stringify({
                            idSteamer: idSteamerSeleccionado,
                        });
                        $.ajax({
                            //Se obtienen el detalle del steamer seleccionado. 
                            url: "BarcoViajeCrear.aspx/ObtenerDetalleSteamer",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            data: params,
                            success: function (result) {
                                if (!result.d) {
                                    alert("Ha habido un error al obtener la información");
                                } else {
                                    options.success(result.d.steamers);
                                }

                            },
                            error: function (result) {
                                result.error(result);
                                alert("Ha habido un error al cargar la información");
                            },
                        });
                    }
                },
                pageSize: 24,
            },
            resizable: true,
            toolbar: [],
            editable: true,
            batch: true,
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

        $("#viajePescaAC").kendoAutoComplete({
            filter: "contains",
            placeholder: "Ingrese numero del Viaje pesca",
            dataSource: {
                data: codigoViajePesca
            },
            change: function (e) {
                idEmbacacionSeleccionada = this.value();
                let popUpViajePescaGrid = $("#popupViajePescaGrid").data("kendoGrid");
                popUpViajePescaGrid.dataSource.read();
            }
        });

        //Viaje Pesca
        function agregarViajePescaBtn(e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            $("#lblViajePescaDD").val(dataItem.viaje);
            viajePesca = dataItem.viaje;
            idEmbarcacion = dataItem.idEmbarcacion;
            alert("Dato seleccionado");
        }

        $("#popupViajePescaGrid").kendoGrid({
            columns: [
                {
                    field: "barco",
                    title: "Barco",
                    editable: () => { return false },
                    width: 110,
                },
                {
                    field: "viaje",
                    title: "Viaje",
                    editable: () => { return false },
                    width: 110,
                },
                {
                    field: "anio",
                    title: "Año",
                    editable: () => { return false },
                    width: 110,
                },
                {
                    field: "fechaIni",
                    title: "Fecha Inicio",
                    editable: () => { return false },
                    width: 100,
                },
                {
                    field: "fechaFin",
                    title: "Fecha Fin",
                    editable: () => { return false },
                    width: 100,
                },
                {
                    field: "pais",
                    title: "País",
                    editable: () => { return false },
                    width: 100,
                },
                {
                    command: [
                        {
                            click: agregarViajePescaBtn,
                            text: "Agregar",
                        },
                    ],
                    title: "",
                    width: 140
                }
            ],
            autoBind: false,
            dataSource: {
                sync: function () {
                    this.read();
                },
                schema: {
                    model: {
                        id: "StringidBarco",
                        fields: {
                            barco: {
                                type: "string"
                            },
                            viaje: {
                                type: "number"
                            },
                            anio: {
                                type: "number"
                            },
                            fechaIni: {
                                type: "string",
                            },
                            fechaFin: {
                                type: "string",
                            },
                            pais: {
                                type: "string",
                            }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        let params = JSON.stringify({
                            idBarco: idBarcoSeleccionado,
                        });
                        $.ajax({
                            //Obtener datos de viaje pesca
                            url: "BarcoViajeCrear.aspx/ObtenerViajePesca",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            data: params,
                            success: function (result) {
                                if (!result.d) {
                                    alert("Ha habido un error al obtener la información");
                                } else {
                                    options.success(result.d.viajesPesca);
                                }
                    
                            },
                            error: function (result) {
                            result.error(result);
                            alert("Ha habido un error al cargar la información");
                            },
                        });
                    }
                },
                pageSize: 24,
            },
            resizable: true,
            toolbar: [],
            editable: true,
            batch: true,
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

        //Se limita a 13 digitos
        $('form').bind("keypress", function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                return false;
            }
        });

        //Se limitan los digitos y no es posible ingresar negativos 
        $('#lblEjercicioDD').on('input', function () {
            let value = $(this).val();
            if ((value !== '') && (value.indexOf('.') === -1)) {
                $(this).val(Math.max(Math.min(value, 9999), 0));
                if (value.length == 4) {
                    obteneViaje();
                }
            }
        });

        //se limita a 18 digitos 
        $('#lblRSA').on('input', function () {
            let value = $(this).val();
            if ((value !== '') && (value.indexOf('.') === -1)) {
                $(this).val(Math.max(Math.min(value, 999999999999999999), 0));
            }
        });

        
        $("#btnSalir").click(function () {
            //Cuando se oprime el boton salir se retorna a barco viaje consulta con las variables. 
            window.location.replace("BarcoViaje.aspx?&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin + "&StringidBarco=" + StringidBarco);
        });

    });

    function mostrarModalBarcos(e) {  
        prevenirCerrarModalBarcos();  
        $("#myModalBarcos").modal();
    };

    function prevenirCerrarModalBarcos() {
        $('#myModalBarcos').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    function mostrarModalSteamer(e) {  
        prevenirCerrarModalSteamer();  
        $("#myModalSteamer").modal();
    };

    function prevenirCerrarModalSteamer() {
        $('#myModalSteamer').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    function mostrarModalViajePesca(e) {  
        prevenirCerrarModalViajePesca();  
        $("#myModalViajePesca").modal();
    };

    function prevenirCerrarModalViajePesca() {
        $('#myModalViajePesca').modal({
            backdrop: 'static',
            keyboard: false
        });
    };

    </script>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Crear barco - viaje" id="hPage" />

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
                                                        <div class="col-sm-12 column-container unique-container">
                                                            <dl class="lista-descripcion" style = "width:auto">
                                                                <div class="each-row">
                                                                    <dt class="col-sm" style = "width:25%";">
                                                                        Barco
                                                                    </dt>
                                                                    <dd id="barcoDD" class="col-sm">
                                                                            <input id="lblSeleccionarBarcoDD" type="text" class="form-control " placeholder="Selecciona el Barco""/>
                                                                    </dd>
                                                                    <dd style="width: 20%;">
                                                                            <button id="btnAgregarBarco" type="button" class="style-button k-primary k-button"><span class="glyphicon glyphicon-plus"></span></button>
                                                                    </dd>
                                                                    <dt class="col-sm">
                                                                        Fecha Descarga
                                                                    </dt>
                                                                    <dd id="fechaDescargaDD" class="col-sm k-dropdown-custome" >
                                                                        <input id="fechaDescargaDP"/>
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm" style="width: 25%;">
                                                                        Ejercicio
                                                                    </dt>
                                                                    <dd id="ejercicioDD" class="col-sm">
                                                                        <input id="lblEjercicioDD" type="number" min="0" max="9999"  class="form-control"/>
                                                                    </dd>
                                                                    <dd style="width: 20%;"></dd>
                                                                    <dt class="col-sm">
                                                                        Fecha Salida
                                                                    </dt>
                                                                    <dd id="fechaSalidaDD" class="col-sm k-dropdown-custome">
                                                                        <input id="fechaSalidaDP"/>
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm" style="width: 25%;">
                                                                        Viaje
                                                                    </dt>
                                                                    <dd id="viajeDD" class="col-sm k-dropdown-custome">
                                                                        <input id="viajeDDL" type="text" class="form-control " placeholder="Selecciona el Barco y Ejercicio"/>
                                                                    </dd>
                                                                    <dd style="width: 20%;"></dd>
                                                                    <dt class="col-sm">
                                                                        Fecha Arribo
                                                                    </dt>
                                                                    <dd id="fechaArriboDD" class="col-sm k-dropdown-custome">
                                                                        <input id="fechaArriboDP"/>
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm" style="width: 13%;";>
                                                                        Steamer
                                                                    </dt>
                                                                    <dd id="steamerDD" class="col-sm" style="width: 25.7%;">
                                                                        <input id="lblSeleccionarSteamerDD" type="text" class="form-control " placeholder="Selecciona el Steamer"/>
                                                                    </dd>
                                                                    <dd style="width: 10%;">
                                                                        <button id="btnAgregarSteamer" type="button" class="style-button k-primary k-button"><span class="glyphicon glyphicon-plus"></span></button>
                                                                    </dd>
                                                                    <dt class="col-sm" style="width: 25.5%;">
                                                                        Viaje de pesca
                                                                    </dt>
                                                                    <dd id="viajePescaDD" class="col-sm" style="width: 22.2%;">
                                                                        <input id="lblViajePescaDD" type="text" class="form-control "  placeholder="Selecciona viaje pesca"/>
                                                                    </dd>
                                                                    <dd style="width: 3%;">
                                                                        <button id="btnAgregarViajePesca" type="button" class="style-button k-primary k-button" ><span class="glyphicon glyphicon-plus"></span></button>
                                                                    </dd>
                                                                </div>
                                                                <div class="each-row">
                                                                    <dt class="col-sm" style="width: 25%;">
                                                                        Folio Id.
                                                                    </dt>
                                                                    <dd id="folioIdentificadorDD" class="col-sm k-dropdown-custome">
                                                                        <input id="folioIdentificadorDDL" />
                                                                    </dd>
                                                                    <dd style="width: 20%;"></dd>
                                                                    <dt class="col-sm">
                                                                        RSA
                                                                    </dt>
                                                                    <dd id="rsaDD" class="col-sm">
                                                                        <input id="lblRSA" type="number" type="number" min="0" max="999999999999999999" class="form-control " placeholder="Ingresa RSA"/>
                                                                    </dd>
                                                                </div>
                                                                <div class="four-button-contain">
                                                                    <button id="agregarBarcoViaje" class="k-primary k-button pushAgregar">Agregar barco - viaje</button>
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
                                        <h3 class="box-title">Productos</h3>
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
            </div>
        <!-- Modal Barcos-->
        <!--<div class="modal fade k-custom-modal-especialidades" id="myModalBarcos" role="dialog">-->
        <div class="modal fade" id="myModalBarcos" role="dialog">
            <div class="modal-dialog emx-custom-modal">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Barcos</h4>
                    </div>
                    <div id="headercontrols">
                        <div class="col-sm-3">
                            <label for="lblSeleccionado" class="control-label-top">BARCO: </label>
                            <input id="barcosAC"/>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div id="popupBarcosGrid"></div>
                            <div class="col-sm-6 four-button-contain">
                            </div>
                        </div>
                    </div>
                     <!--<div class="modal-footer">
                    </div>-->
                </div>
            </div>
        </div>
        <!-- Modal Steamer-->
        <!--<div class="modal fade k-custom-modal-especialidades" id="myModalBarcos" role="dialog">-->
        <div class="modal fade" id="myModalSteamer" role="dialog">
            <div class="modal-dialog emx-custom-modal">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Steamer</h4>
                    </div>
                    <div id="headercontrols">
                        <div class="col-sm-3">
                            <label for="lblCarritoSeleccionado" class="control-label-top">Steamer: </label>
                            <input id="steamerAC"/>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div id="popupSteamerGrid"></div>
                            <div class="col-sm-6 four-button-contain">
                            </div>
                        </div>
                    </div>
                     <!--<div class="modal-footer">
                    </div>-->
                </div>
            </div>
        </div>
        <!-- Modal Viaje Pesca -->
        <!--<div class="modal fade k-custom-modal-especialidades" id="myModalBarcos" role="dialog">-->
        <div class="modal fade" id="myModalViajePesca" role="dialog">
            <div class="modal-dialog emx-custom-modal">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Viaje de pesca</h4>
                    </div>
                    <div id="headercontrols">
                        <!--<div class="col-sm-3">
                            <label for="lblViajePesca" class="control-label-top">Baco viaje: </label>
                            <input id="barcosViajePescaAC"/>
                        </div>-->
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div id="popupViajePescaGrid"></div>
                            <div class="col-sm-6 four-button-contain">
                            </div>
                        </div>
                    </div>
                     <!--<div class="modal-footer">
                    </div>-->
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

            .color-DP {
                width: 10em;
            }

           .style-button{
                width: 22px;
                height: 22px;
           }
        </style>
    </section>
</asp:Content>
