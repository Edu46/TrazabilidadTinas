<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="Calidad.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.Calidad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(function () {
            kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;
            let calidadesP = [];
            let update = false;

            function actualizar(idCalidad, nombre, clave, activo) {
                //Se obtienen los datos para actualizar la calidad
                let calidadGrid = $('#calidad').data("kendoGrid");
                let param = {
                    idCalidad: idCalidad,
                    nombre: nombre,
                    clave: clave,
                    activo: activo,
                }
                let params = JSON.stringify({ catalogoCalidad: param });
                $.ajax({
                    //Actualizar infromacion de calidad 
                    url: "Calidad.aspx/EditarCalidad",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                calidadGrid.dataSource.read();
                                break;
                            case 204:
                                calidadGrid.dataSource.read();
                                alert("Error al identificar el elemento a crear/actualizar");
                                break;
                            case 400:
                                calidadGrid.dataSource.read();
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                calidadGrid.dataSource.read();
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 409:
                                calidadGrid.dataSource.read();
                                alert("La descripción ingresada ya existe");
                                break;
                        }
                    },
                    error: function (result) {
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                        calidadGrid.dataSource.read();
                    }
                });
            }

            window.calidad = $("#calidad").kendoGrid({
                columns: [
                    {
                        field: "nombre",
                        title: "Nombre calidad",
                        width: 100,
                    },
                    {
                        field: "clave",
                        title: "Clave Calidad",
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
                    pageSize:15,
                    schema: {
                        model: {
                            id: "idCalidad",
                            fields: {
                                nombre: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    }
                                },
                                clave: {
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
                                //Obtenemos datos de calidad 
                                url: "Calidad.aspx/ObtenerCalidad",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d != null) {
                                        for (let i = 0; i < result.d.calidades.length; i++) {
                                            //Se guarda en una variable los datos de las basculas para su manipulación.
                                            calidadesP[i] = result.d.calidades[i];
                                        }
                                        options.success(result.d.calidades);
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
                                nombre: options.data.nombre,
                                clave: options.data.clave,
                                activo: options.data.activo,
                            }
                            let params = JSON.stringify({ catalogoCalidad: param });
                            $.ajax({
                                //Crea registro de calidad. 
                                url: "Calidad.aspx/CrearCalidad",
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    switch (result.d) {
                                        case 200:
                                            options.success(result.d);
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
                            for (let a = 0; a < calidadesP.length; a++) {
                                if (options.data.idCalidad == calidadesP[a].idCalidad) {
                                    //Se valida que no se encuentre activa la bascula muelle para mostrar el confirm.
                                    if (options.data.activo == false && options.data.activo != calidadesP[a].activo) {
                                        update = true;
                                    } else {
                                        update = false;
                                    }
                                }
                            }
                            if (update == true) {
                                let res = confirm("¿Estás seguro que deseas desactivar el registro?");
                                if (!res) {
                                    options.success();
                                } else {
                                    let idCalidad = options.data.idCalidad;
                                    let nombre = options.data.nombre;
                                    let clave = options.data.clave;
                                    let activo = options.data.activo;
                                    //se envian los parametros para desactivar el row
                                    actualizar(idCalidad, nombre, clave, activo);
                                }
                            } else {
                                let idCalidad = options.data.idCalidad;
                                let nombre = options.data.nombre;
                                let clave = options.data.clave;
                                let activo = options.data.activo;
                                //se envian los parametros para la actualizacion.
                                actualizar(idCalidad, nombre, clave, activo);
                            }
                        },
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
            let count = $("#calidad").offset().top + 30;

            count = $(".k-grid-toolbar").length ? count + $(".k-grid-toolbar").innerHeight() : count;
            count = $(".k-grid-header").length ? count + $(".k-grid-header").innerHeight() : count;
            count = $(".k-grid-pager").length ? count + $(".k-grid-pager").innerHeight() : count;

            $(".k-grid-content").css("max-height", `calc(100vh - ${count}px)`);
        }); 
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Calidad" id="hPage" />
    
    <section id="Section1" class="custom-content" runat="server">
            <div class="bs-component">
                <div class="clearfix">
                    <div class="row">
                        <div class="col-sm-12">
                            <!-- /.box-content -->
                            <div class="box custom-box box-primary">
                                <!-- /.box-header -->
                                <div class="box-header with-border">
                                    <h3 class="box-title">Calidad</h3>
                                </div>
                                <!-- /.box-body -->
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div id="calidad"></div>
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
