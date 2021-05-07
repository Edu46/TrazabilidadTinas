<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="Bascula.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.Bascula" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(function () {

            kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;
            let bascula = [];
            let update = false;
            
            function actualizar(idBasculaP, descripcionP, activoP) {
                //Se obtienen los datos para actualizar la bascula
                let basculasGrid = $('#basculas').data("kendoGrid");
                let param = {
                    idBascula: idBasculaP,
                    descripcion: descripcionP,
                    activo: activoP,
                }
                let params = JSON.stringify({ catalogoBascula: param });
                $.ajax({
                    //Actualizar informacion bascula.
                    url: "Bascula.aspx/EditarBascula",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                basculasGrid.dataSource.read();
                                break;
                            case 204:
                                basculasGrid.dataSource.read();
                                alert("Error al identificar el elemento a crear/actualizar");
                                break;
                            case 400:
                                basculasGrid.dataSource.read();
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                basculasGrid.dataSource.read();
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 409:
                                basculasGrid.dataSource.read();
                                alert("La descripción ingresada ya existe");
                                break;
                        }
                    },
                    error: function (result) {
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                    }
                });
            }

            window.basculas = $("#basculas").kendoGrid({
                columns: [
                    {
                        field: "descripcion",
                        title: "Báscula",
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
                            id: "idBascula",
                            fields: {
                                idBascula: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    },
                                },
                                idTipo: {
                                    type: "number",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    }
                                },
                                descripcion: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    }
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
                                //Obtener informacion de bascula
                                url: "Bascula.aspx/ObtenerBasculas",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d != null) {
                                        for (let i = 0; i < result.d.basculas.length; i++) {
                                            //Se guarda en una variable los datos de las basculas para su manipulación.
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
                        },
                        create: function (options) {
                            let param = {
                                descripcion: options.data.descripcion,
                                activo: options.data.activo,
                            }
                            let params = JSON.stringify({ catalogoBascula: param });
                            $.ajax({
                                //Crear bascula.
                                url: "Bascula.aspx/CrearBascula",
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    switch (result.d) {
                                        case 200:
                                            options.success(result.d.basculas);
                                            break;
                                        case 304:
                                            alert("YA EXISTE LA BÁSCULA QUE DESEAS AGREGAR");
                                            let basculasGrid = $('#basculas').data("kendoGrid");
                                            basculasGrid.dataSource.read();
                                            break;
                                        case 400:
                                            alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                            break;
                                    }
                                },
                                error: function (options) {
                                    options.error(options);
                                    alert("Ha habido un error al crear el elemento, por favor intente de nuevo");
                                }
                            });
                        },
                        update: function (options) {
                            update = false;
                            for (let a = 0; a < bascula.length; a++) {
                                //Se valida que no se encuentre activa la bascula para mostrar el confirm.
                                if (options.data.idBascula == bascula[a].idBascula) {
                                    if (options.data.activo == false && options.data.activo != bascula[a].activo) {
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
                                    let idBasculaP = options.data.idBascula;
                                    let descripcionP = options.data.descripcion;
                                    let activoP = options.data.activo;
                                    //se envian los parametros para actualizar
                                    actualizar(idBasculaP, descripcionP, activoP);
                                }
                            } else {
                                let idBasculaP = options.data.idBascula;
                                let descripcionP = options.data.descripcion;
                                let activoP = options.data.activo;
                                //se envian los parametros para la actualizacion.
                                actualizar(idBasculaP, descripcionP, activoP);
                            }
                            
                        }
                    },
                },
                scrollable: true,
                resizable: true,
                editable: "inline",
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
                navigatable: true
            });

        });

        $(function () {
            let count = $("#basculas").offset().top + 30;

            count = $(".k-grid-toolbar").length ? count + $(".k-grid-toolbar").innerHeight() : count;
            count = $(".k-grid-header").length ? count + $(".k-grid-header").innerHeight() : count;
            count = $(".k-grid-pager").length ? count + $(".k-grid-pager").innerHeight() : count;

            $(".k-grid-content").css("max-height", `calc(100vh - ${count}px)`);
        }); 

  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Básculas" id="hPage" />

    <section id="Section1" class="custom-content" runat="server">
            <div class="bs-component">
                <div class="clearfix">
                    <div class="row">
                        <div class="col-sm-12">
                            <!-- /.box-content -->
                            <div class="box custom-box box-primary">
                                <!-- /.box-header -->
                                <div class="box-header with-border">
                                    <h3 class="box-title">Básculas</h3>
                                </div>
                                <!-- /.box-body -->
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div id="basculas"></div>
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
