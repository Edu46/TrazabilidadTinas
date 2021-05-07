<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="BasculaMuelle.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.BasculaMuelle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script>
        $(function () {
            kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;
            let descripcionBascula = "";
            let descripcionMuelle = "";

            let desObtenidaMuelle = "";
            let desObtenidaBascula = "";

            let basculaMuelleP = [];
            let update = false;

            let idBascula = 0;
            let indiceBascula = 0;

            let idMuelle = 0;
            let indiceMuelle = 0;

            function actualizar(idBasculaMuelle, descripcionBascula, descripcionMuelle, activo) {
                //Se obtienen los datos para actualizar la bascula
                let basculaMuelleGrid = $('#basculaMuelle').data("kendoGrid");
                let param = {
                    idBasculaMuelle: idBasculaMuelle,
                    descripcionBascula: descripcionBascula,
                    descripcionMuelle: descripcionMuelle,
                    activo: activo,
                }
                let params = JSON.stringify({ catalogoBasculaMuelle: param });
                $.ajax({
                    //Actualizar informacion bascula muelle.
                    url: "BasculaMuelle.aspx/EditarBasculaMuelle",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                basculaMuelleGrid.dataSource.read();
                                break;
                            case 204:
                                basculaMuelleGrid.dataSource.read();
                                alert("Error al identificar el elemento a crear/actualizar");
                                break;
                            case 400:
                                basculaMuelleGrid.dataSource.read();
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                basculaMuelleGrid.dataSource.read();
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 309:
                                basculaMuelleGrid.dataSource.read();
                                alert("La descripción ingresada ya existe");
                                break;
                        }
                    },
                    error: function (result) {
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                        basculaMuelleGrid.dataSource.read();
                    }
                });
            }

            function cambioBasculaDDL(container, options) {
                 $('<input id="cambioBasculaDDL" name="cambioBasculaDDL" required data-bind="value:value" data-required-msg="Campo requerido"/><span class="k-invalid-msg" data-for="cambioBasculaDDL" ></span>')
                .appendTo(container);
                $("#cambioBasculaDDL")
                .kendoDropDownList({
                    dataSource: {
                        schema: {
                            model: {
                                id: "idBascula",
                                fields: {
                                    descripcion: { type: "string" },
                                    activo: { type: "boolean" },
                                }
                            }
                        },
                        transport: {
                            read: function (options) {
                                $.ajax({
                                    //obtener catalogo de basculas.
                                    url: "BasculaMuelle.aspx/ObtenerBasculas",
                                    contentType: "application/json;charset=utf-8",
                                     type: "POST",
                                     success: function (result) {
                                        for (i = 0; i <= result.d.basculas.length - 1; i++) {
                                            if (result.d.basculas[i].idBascula == idBascula) {
                                                indiceBascula = i;
                                            }
                                        }
                                        options.success(result.d.basculas);
                                    },
                                    error: function (result) {
                                        options.error(result);
                                        alert("Ha habido un error al cargar la información del recurso de integración");
                                    }
                                });

                            }
                        }
                    },
                    dataTextField: "descripcion",
                    dataValueField: "idBascula",
                    change: function () {
                        idBascula = this.value();
                    },
                    select: onSelectBascula,
                    dataBound: setIndiceBascula,
                });
            }

            function setIndiceBascula(evt) {
                //Se carga el ddl en el indice que le corresponde
                if (evt.sender.dataSource.data().length > 0) {
                    evt.sender.select(indiceBascula);
                    idBascula = evt.sender.dataSource.data()[indiceBascula].idBascula;
                    descripcionBascula = evt.sender.dataSource.data()[indiceBascula].descripcion;
                    desObtenidaBascula = evt.sender.dataSource.data()[indiceBascula].descripcion;
                }
                evt.sender.dataBound = false;
            }

            function onSelectBascula(e) {
                //Obtiene la descripcion de la talla seleccionada
                let dataItem = e.dataItem;
                descripcionBascula = dataItem.descripcion;
                desObtenidaBascula = dataItem.descripcion;
            };

            function cambioMuelleDDL(container, options) {

                 $('<input id="cambioMuelleDDL" name="cambioMuelleDDL" required data-bind="value:value" data-required-msg="Campo requerido"/><span class="k-invalid-msg" data-for="cambioMuelleDDL" ></span>')
                .appendTo(container);
                $("#cambioMuelleDDL")
                .kendoDropDownList({
                    dataSource: {
                        schema: {
                            model: {
                                id: "idMuelle",
                                fields: {
                                    descripcion: { type: "string" },
                                    activo: { type: "boolean" },
                                }
                            }
                        },
                        transport: {
                            read: function (options) {
                                $.ajax({
                                    //Obtener catalogo de muelle
                                    url: "BasculaMuelle.aspx/ObtenerMuelle",
                                    contentType: "application/json;charset=utf-8",
                                    type: "POST",
                                    success: function (result) {
                                        for (i = 0; i <= result.d.muelles.length - 1; i++) {
                                            if (result.d.muelles[i].idMuelle == idMuelle) {
                                                //Se toma el indice del muelle para mostrar en el DDL
                                                indiceMuelle = i;
                                            }

                                        }
                                        options.success(result.d.muelles);
                                    },
                                    error: function (result) {
                                        options.error(result);
                                        alert("Ha habido un error al cargar la información del recurso de integración");
                                    }
                                });

                            }
                        }
                    },
                    dataTextField: "descripcion",
                    dataValueField: "idMuelle",
                    change: function () {
                        idMuelle = this.value();
                    },
                    select: onSelectMuelle,
                    dataBound: setIndiceMuelle

                });
            }

            function setIndiceMuelle(evt) {
                //Se carga el ddl en el indice que le corresponde 
                if (evt.sender.dataSource.data().length > 0) {
                    evt.sender.select(indiceMuelle);
                    idMuelle = evt.sender.dataSource.data()[indiceMuelle].idMuelle;
                    descripcionMuelle = evt.sender.dataSource.data()[indiceMuelle].descripcion;
                    desObtenidaMuelle = evt.sender.dataSource.data()[indiceMuelle].descripcion;
                }
                evt.sender.dataBound = false;
            }

            function onSelectMuelle(e) {
                //Obtiene la descripcion de la talla seleccionada
                let dataItem = e.dataItem;
                descripcionMuelle = dataItem.descripcion;
                desObtenidaMuelle = dataItem.descripcion;
            };

            window.basculaMuelle = $("#basculaMuelle").kendoGrid({
                columns: [
                    {
                        field: "descripcionBascula",
                        title: "Báscula",
                        editor: cambioBasculaDDL,
                        width: 100
      
                    },
                    {
                        field: "descripcionMuelle",
                        title: "Muelle",
                        editor: cambioMuelleDDL,
                        width: 100
      
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
                    pageSize:15,
                    schema: {
                        model: {
                            id: "idBasculaMuelle",
                            fields: {
                                descripcionBascula: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    }
                                },
                                descripcionMuelle: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    }
                                },
                                idBasculaMuelle: {
                                    type: "number",
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
                                url: "BasculaMuelle.aspx/ObtenerBasculaMuelle",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d != null) {
                                        for (let i = 0; i < result.d.basculasMuelles.length; i++) {
                                            //Se guarda en una variable los datos de las basculas para su manipulación.
                                            basculaMuelleP[i] = result.d.basculasMuelles[i];
                                        }
                                        options.success(result.d.basculasMuelles);
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
                            let basculaMuelleGrid = $('#basculaMuelle').data("kendoGrid");
                            let param = {
                                descripcionBascula: desObtenidaBascula,
                                descripcionMuelle: desObtenidaMuelle,
                                activo: options.data.activo,
                            }
                            let params = JSON.stringify({ catalogoBasculaMuelle: param });
                            $.ajax({
                                //se crea la relacion de bascula muelle
                                url: "BasculaMuelle.aspx/CrearBasculaMuelle",
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    switch (result.d) {
                                        case 200:
                                            options.success(result.d);
                                            break;
                                        case 304:
                                            alert("Ya existe esta relación báscula muelle");
                                            basculaMuelleGrid.dataSource.read();
                                            break;
                                        case 400:
                                            alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                            basculaMuelleGrid.dataSource.read();
                                            break;
                                    }
                                },
                                error: function (options) {
                                    alert("Ha habido un error al crear el elemento, por favor intente de nuevo");
                                    basculaMuelleGrid.dataSource.read();
                                }
                            });
                        },
                        update: function (options) {
                            update = false;
                            for (let a = 0; a < basculaMuelleP.length; a++) {
                                if (options.data.idBasculaMuelle == basculaMuelleP[a].idBasculaMuelle) {
                                    //Se valida que no se encuentre activa la bascula muelle para mostrar el confirm.
                                    if (options.data.activo == false && options.data.activo != basculaMuelleP[a].activo) {
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
                                    let idBasculaMuelle = options.data.idBasculaMuelle;
                                    let descripcionBascula = desObtenidaBascula;
                                    let descripcionMuelle = desObtenidaMuelle;
                                    let activo = options.data.activo;
                                    //se envian los parametros para desactivar el row
                                    actualizar(idBasculaMuelle, descripcionBascula, descripcionMuelle, activo);
                                }
                            } else {
                                let idBasculaMuelle = options.data.idBasculaMuelle;
                                let descripcionBascula = desObtenidaBascula;
                                let descripcionMuelle = desObtenidaMuelle;
                                let activo = options.data.activo;
                                //se envian los parametros para la actualizacion.
                                actualizar(idBasculaMuelle, descripcionBascula, descripcionMuelle, activo);
                            }
                        }
                    },
                },
                scrollable: true,
                resizable: true,
                editable: "inline",
                toolbar: [{ name: "create", text: "" }],
                edit: function(e) {
                    if (!e.model.isNew()) {
                        idBascula = e.model.idBascula;
                        idMuelle = e.model.idMuelle;
                    } else {
                        //se setea los valores
                        idBascula = 0;
                        idMuelle = 0;
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
                    //se setea los valores
                    idBascula = 0;
                    indiceBascula = 0;
                    idMuelle = 0;
                    indiceMuelle = 0;
                    //Se habilita el botón de agregar
                    $('.k-grid-add').prop("disabled", false);
                    $('.k-grid-add').removeClass("k-grid-add-disabled");
                    //Se habilitan los iconos de filtro
                    $('.k-grid-filter').removeClass("display-none ");
                },
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

        });

        $(function () {
            let count = $("#basculaMuelle").offset().top + 30;

            count = $(".k-grid-toolbar").length ? count + $(".k-grid-toolbar").innerHeight() : count;
            count = $(".k-grid-header").length ? count + $(".k-grid-header").innerHeight() : count;
            count = $(".k-grid-pager").length ? count + $(".k-grid-pager").innerHeight() : count;

            $(".k-grid-content").css("max-height", `calc(100vh - ${count}px)`);
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Relación báscula - muelle" id="hPage" />

    <section id="Section1" class="custom-content" runat="server">
            <div class="bs-component">
                <div class="clearfix">
                    <div class="row">
                        <div class="col-sm-12">
                            <!-- /.box-content -->
                            <div class="box custom-box box-primary">
                                <!-- /.box-header -->
                                <div class="box-header with-border">
                                    <h3 class="box-title">Relación báscula - muelle</h3>
                                </div>
                                <!-- /.box-body -->
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div id="basculaMuelle"></div>
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
