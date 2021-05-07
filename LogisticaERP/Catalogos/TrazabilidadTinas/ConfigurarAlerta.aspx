<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="ConfigurarAlerta.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.ConfigurarAlerta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(function () {

            kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;
            let desTallas = "";
            let idTallas = "0";
            let indiceTalla = 0;
            let tallaDescripcion = "";
            let alerta = [];
            let update = false;

            function actualizar(idFrecuenciaAlerta, talla, maximaExposicion, rangoAlerta, tolerancia, activo) {
                //Se obtienen los datos para actualizar las alertas.
                let configuarAlertaGrid = $('#configuarAlerta').data("kendoGrid");
                let param = {
                    idFrecuenciaAlerta: idFrecuenciaAlerta,
                    talla: talla,
                    maximaExposicion: maximaExposicion,
                    rangoAlerta: rangoAlerta,
                    tolerancia: tolerancia,
                    activo: activo,
                }
                let params = JSON.stringify({ catalogoAlerta: param });
                $.ajax({
                    //Actualizar datos de Alerta
                    url: "ConfigurarAlerta.aspx/EditarAlerta",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                configuarAlertaGrid.dataSource.read();
                                break;
                            case 204:
                                configuarAlertaGrid.dataSource.read();
                                alert("Error al identificar el elemento a crear/actualizar");
                                break;
                            case 400:
                                configuarAlertaGrid.dataSource.read();
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                configuarAlertaGrid.dataSource.read();
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 409:
                                configuarAlertaGrid.dataSource.read();
                                alert("La descripción ingresada ya existe");
                                break;
                        }
                    },
                    error: function (result) {
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                        configuarAlertaGrid.dataSource.read();
                    }
                });
            }

            function cambioTallaDDL(container, options) {
               $('<input id="cambioTallaDDL" name="cambioTallaDDL" required data-bind="value:value" data-required-msg="Campo requerido"/><span class="k-invalid-msg" data-for="cambioTallaDDL" ></span>')
                .appendTo(container);
                $("#cambioTallaDDL")
                .kendoDropDownList({
                    dataSource: {
                        schema: {
                            model: {
                                id: "idTallas",
                                fields: {
                                    talla: { type: "string" },
                                    clave: { type: "string" },
                                    idTallas: {type: "string"}
                                }
                            }
                        },
                        transport: {
                            read: function (options) {
                                $.ajax({
                                    //Obtener informacion del catalogo de tallas. 
                                    url: "ConfigurarAlerta.aspx/ObtenerTallas",
                                    contentType: "application/json;charset=utf-8",
                                    type: "POST",
                                    success: function (result) {
                                        for (i = 0; i <= result.d.length - 1; i++) {
                                            if (result.d[i].clave == desTallas) {
                                                //Se toma el indice de la talla para mostrar en el DDL
                                                indiceTalla = i;
                                            }
                                        }
                                        options.success(result.d);
                                    },
                                    error: function (result) {
                                        options.error(result);
                                        alert("Ha habido un error al cargar la información");
                                    }
                                });

                            }
                        }
                    },
                    dataTextField: "clave",
                    dataValueField: "idTallas",
                    change: function () {
                        idTallas = this.value();
                    },
                    select: onSelectTalla,
                    dataBound: setIndiceTalla
                });
            }

            function setIndiceTalla(evt) {
                //Se carga el ddl en el indice que le corresponde 
                if (evt.sender.dataSource.data().length > 0) {
                        evt.sender.select(indiceTalla);
                        idTallas = evt.sender.dataSource.data()[indiceTalla].idTallas;
                }
                evt.sender.dataBound = false;
            }

            function onSelectTalla(e) {
                //Obtiene la descripcion de la talla seleccionada  
                let dataItem = e.dataItem;
                tallaDescripcion = dataItem.clave;
            };

            window.configuarAlerta = $("#configuarAlerta").kendoGrid({
                columns: [
                    {
                        field: "talla",
                        title: "Talla",
                        editor: cambioTallaDDL,
                        width: 100
                    },
                    {
                        field: "maximaExposicion",
                        title: "Tiempo de exposición",
                        width: 100,
                    },
                    {
                        field: "tolerancia",
                        title: "Tolerancia",
                        width: 100,
                    },
                    {
                        field: "activo",
                        title: "Estatus",
                        template: '<center><input type="checkbox" #= activo==true ? checked="checked" : "" # disabled="disabled" ></input></center>',
                        width: 100,
                        attributes: {
                            "class": "checkbox-cell"
                        }

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
                            id: "idFrecuenciaAlerta",
                            fields: {
                                talla: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido",
                                        },
                                    },
                                },
                                maximaExposicion: {
                                    type: "number",
                                    validation: {
                                        required: {
                                            message:"Campo requerido",
                                        },
                                        min: 0,
                                    },
                                },
                                tolerancia: {
                                    type: "number",
                                    validation: {
                                        required: {
                                            message: "Campo requerido",
                                        },
                                        min: 0,
                                    },
                                },
                                rangoAlerta: {
                                    type: "number",
                                    validation: {
                                        required: {
                                            message: "Campo requerido",
                                        },
                                        min: 0,
                                    },
                                },
                                tolerancia: {
                                    type: "number",
                                    validation: {
                                        required: {
                                            message: "Campo requerido",
                                        },
                                        min: 0,
                                    },
                                },
                                idTalla: {
                                    type: "string",
                                },
                                activo: {
                                    type: "boolean",
                                },
                            }
                        }
                    },
                    transport: {
                        read: function (options) {
                            $.ajax({
                                //Obtener los datos de Alertas.
                                url: "ConfigurarAlerta.aspx/ObtenerAlertas",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d != null) {
                                        for (let i = 0; i < result.d.alertas.length; i++) {
                                            //Se guardan los datos para su manipulación.
                                            alerta[i] = result.d.alertas[i];
                                        }
                                        options.success(result.d.alertas);
                                    } else {
                                        options.success([]);
                                    }
                                },
                                error: function (result) {
                                    options.error(result);
                                    alert("Ha habido un error al cargar la información del recurso de integración");
                                }
                            });
                        },
                        create: function (options) {
                            //se valida que no se pueda agregar una alerta con tiempo y tolerancia en cero
                            if (options.data.maximaExposicion <= 0 && options.data.tolerancia <= 0) {
                                alert("No es posible guardar una aleta con el tiempo o tolerancia en cero");
                                let configuarAlertaGrid = $("#configuarAlerta").data("kendoGrid");
                                configuarAlertaGrid.dataSource.read();
                            } else {
                                let param = {
                                    talla: tallaDescripcion,
                                    maximaExposicion: options.data.maximaExposicion,
                                    rangoAlerta: options.data.rangoAlerta,
                                    tolerancia: options.data.tolerancia,
                                    activo: options.data.activo
                                }
                                let params = JSON.stringify({ catalogoAlerta: param });
                                $.ajax({
                                    //Se crea la alerta
                                    url: "ConfigurarAlerta.aspx/CrearAlerta",
                                    contentType: "application/json; charset=utf-8",
                                    type: "POST",
                                    data: params,
                                    success: function (result) {
                                        switch (result.d){
                                            case 200:
                                                options.success();
                                                break;
                                            case 204:
                                                options.success();
                                                alert("Error al identificar el elemento a crear");
                                                break;
                                            case 304:
                                                options.error();     
                                                alert("El dato ingresado ya existe");
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
                                    error: function(result){
                                        options.error(result);
                                        alert("Ha habido un error al crear el elemento, por favor intente de nuevo"); 
                                    }    
                                });
                            }
                        },
                        update: function (options) {
                            //se valida que no se pueda agregar una alerta con tiempo y tolerancia en cero
                            if (options.data.maximaExposicion <= 0 && options.data.tolerancia <= 0) {
                                alert("No es posible guardar una aleta con el tiempo o tolerancia en cero");
                                let configuarAlertaGrid = $("#configuarAlerta").data("kendoGrid");
                                configuarAlertaGrid.dataSource.read();
                            } else {
                                update = false;
                                for (let a = 0; a < alerta.length; a++) {
                                    if (options.data.idFrecuenciaAlerta == alerta[a].idFrecuenciaAlerta) {
                                        //Se valida que no se encuentre activa la alerta para mostrar el confirm.
                                        if (options.data.activo == false && options.data.activo != alerta[a].activo) {
                                            update = true;
                                        } else {
                                            update = false;
                                        }
                                    }
                                }
                                if (update == true) {
                                    let res = confirm("¿Estas seguro que deseas desactivar el registro?");
                                    if (!res) {
                                        options.success();
                                    } else {
                                        let idFrecuenciaAlerta = options.data.idFrecuenciaAlerta;
                                        let talla = options.data.talla;
                                        let maximaExposicion = options.data.maximaExposicion;
                                        let rangoAlerta = options.data.rangoAlerta;
                                        let tolerancia = options.data.tolerancia;
                                        let activo = options.data.activo;
                                        //se envian los parametros del alerta para desactivar el row
                                        actualizar(idFrecuenciaAlerta, talla, maximaExposicion, rangoAlerta, tolerancia, activo);
                                    }
                                } else {
                                    let idFrecuenciaAlerta = options.data.idFrecuenciaAlerta;
                                    let talla = options.data.talla;
                                    let maximaExposicion = options.data.maximaExposicion;
                                    let rangoAlerta = options.data.rangoAlerta;;
                                    let tolerancia = options.data.tolerancia;
                                    let activo = options.data.activo;
                                    //se envian los parametros para la actualizacion.
                                    actualizar(idFrecuenciaAlerta, talla, maximaExposicion, rangoAlerta, tolerancia, activo);
                                }
                            }
                        },
                    },
                },
                scrollable: true,
                resizable: true,
                editable: "inline",
                edit: function(e) {
                    if (!e.model.isNew()) {
                        desTallas = e.model.talla;
                    } else {
                        //se setea los valores a cero
                        idTallas = "0";
                        desTallas = "";
                        idTallas = "0";
                        indiceTalla = 0;
                        //Habilitar el botón de agregar
                        $('.k-grid-add').prop("disabled", true);
                        $('.k-grid-add').addClass("k-grid-add-disabled");
                        //Deshabilitar los iconos de filtro
                        $('.k-grid-filter').addClass("display-none ");
                    }
                },
                save: function (e) {
                    //Se habilita el botón de agregar
                    $('.k-grid-add').prop("disabled", false);
                    $('.k-grid-add').removeClass("k-grid-add-disabled");
                    //Se habilitan los iconos de filtro
                    $('.k-grid-filter').removeClass("display-none ");
                },
                cancel: function (e) {
                    //se setea los valores a cero
                    desTallas = "";
                    idTallas = "0";
                    indiceTalla = 0;
                    //Se habilita el botón de agregar
                    $('.k-grid-add').prop("disabled", false);
                    $('.k-grid-add').removeClass("k-grid-add-disabled");
                    //Se habilitan los iconos de filtro
                    $('.k-grid-filter').removeClass("display-none ");
                },
                toolbar: [{ name: "create", text: "" }],
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
                filterMenuInit: function(e) {
                    var numeric = e.container.find("[data-role=numerictextbox]").data("kendoNumericTextBox");
                    if (numeric) {
                        numeric.min(0);
                    }
                },
                navigatable: true,
            });
        });

        $(function () {
            let count = $("#configuarAlerta").offset().top + 30;

            count = $(".k-grid-toolbar").length ? count + $(".k-grid-toolbar").innerHeight() : count;
            count = $(".k-grid-header").length ? count + $(".k-grid-header").innerHeight() : count;
            count = $(".k-grid-pager").length ? count + $(".k-grid-pager").innerHeight() : count;

            $(".k-grid-content").css("max-height", `calc(100vh - ${count}px)`);
        }); 

  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Configurar alertas" id="hPage" />

    <section id="Section1" class="custom-content " runat="server">
            <div class="bs-component ">
                <div class="clearfix ">
                    <div class="row ">
                        <div class="col-sm-12">
                            <!-- /.box-content -->
                            <div class="box custom-box box-primary">
                                <!-- /.box-header -->
                                <div class="box-header with-border">
                                    <h3 class="box-title">Configurar alerta</h3>
                                </div>
                                <!-- /.box-body -->
                                <div class="box-body ">
                                    <div class="row ">
                                        <div class="col-sm-12 ">
                                            <div id="configuarAlerta"></div>
                                        </div>
                                    </div>
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
    </section>
    <style>
        .k-i-warning {
            font-size: 30px;
            margin: 0px;
            width: 60%;
            color: white;
        }
    </style>
</asp:Content>
