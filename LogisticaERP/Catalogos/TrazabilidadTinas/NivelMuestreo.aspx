<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="NivelMuestreo.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.NivelMuestreo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
        $(function () {

            kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;
            let nivelesMuestreoP = [];
            let update = false;

            function actualizar(idNivelMuestreo, descripcion, activo) {
                //Se obtienen los datos para actualizar.
                let nivelMuestreoGrid = $('#nivelMuestreo').data("kendoGrid");
                let param = {
                    idNivelMuestreo: idNivelMuestreo,
                    descripcion: descripcion,
                    activo: activo,
                }
                let params = JSON.stringify({ catalogoNivelMuestreo: param });
                $.ajax({
                    //Actualizar nivel de muestreo
                    url: "NivelMuestreo.aspx/EditarNivelMuestreo",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                nivelMuestreoGrid.dataSource.read();
                                break;
                            case 204:
                                nivelMuestreoGrid.dataSource.read();
                                alert("Error al identificar el elemento a crear/actualizar");
                                break;
                            case 400:
                                nivelMuestreoGrid.dataSource.read();
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                nivelMuestreoGrid.dataSource.read();
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 409:
                                nivelMuestreoGrid.dataSource.read();
                                alert("La descripción ingresada ya existe");
                                break;
                        }
                    },
                    error: function (result) {
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                        nivelMuestreoGrid.dataSource.read();
                    }
                });
            }

            window.nivelMuestreo = $("#nivelMuestreo").kendoGrid({
                columns: [
                    {
                        field: "descripcion",
                        title: "Nivel de muestreo",
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
                            id: "idNivelMuestreo",
                            fields: {
                                descripcion: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    },
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
                                //Obtener catalogo del nivel de muestreo
                                url: "NivelMuestreo.aspx/ObtenerNivelMuestreo",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d != null) {
                                        for (let i = 0; i < result.d.nivelesMuestreo.length; i++) {
                                            //Se guarda en una variable los datos para su manipulación.
                                            nivelesMuestreoP[i] = result.d.nivelesMuestreo[i];
                                        }
                                        options.success(result.d.nivelesMuestreo);
                                    } else {
                                        options.success([]);
                                    }
                                }

                            });
                        },
                        create: function (options) {
                            let param = {
                                descripcion: options.data.descripcion,
                                activo: options.data.activo
                            }
                            let params = JSON.stringify({ catalogoNivelMuestreo: param });
                            $.ajax({
                                //Se crea registro de nivel de muestreo
                                url: "NivelMuestreo.aspx/CrearNivelMuestreo",
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
                        },
                        update: function (options) {
                            update = false;
                            for (let a = 0; a < nivelesMuestreoP.length; a++) {
                                if (options.data.idNivelMuestreo == nivelesMuestreoP[a].idNivelMuestreo) {
                                    //Se valida que no se encuentre activo el nivel de muestreo para mostrar el confirm.
                                    if (options.data.activo == false && options.data.activo != nivelesMuestreoP[a].activo) {
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
                                    let idNivelMuestreo = options.data.idNivelMuestreo;
                                    let descripcion = options.data.descripcion;
                                    let activo = options.data.activo;
                                    //se envian los parametros del alerta para desactivar el row
                                    actualizar(idNivelMuestreo, descripcion, activo);
                                }
                            } else {
                                let idNivelMuestreo = options.data.idNivelMuestreo;
                                let descripcion = options.data.descripcion;
                                let activo = options.data.activo;
                                //se envian los parametros del alerta para actualizar el row
                                actualizar(idNivelMuestreo, descripcion, activo);
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
            let count = $("#nivelMuestreo").offset().top + 30;

            count = $(".k-grid-toolbar").length ? count + $(".k-grid-toolbar").innerHeight() : count;
            count = $(".k-grid-header").length ? count + $(".k-grid-header").innerHeight() : count;
            count = $(".k-grid-pager").length ? count + $(".k-grid-pager").innerHeight() : count;

            $(".k-grid-content").css("max-height", `calc(100vh - ${count}px)`);
        }); 
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Nivel de muestreo" id="hPage" />

    <section id="Section1" class="custom-content" runat="server">
            <div class="bs-component">
                <div class="clearfix">
                    <div class="row">
                        <div class="col-sm-12">
                            <!-- /.box-content -->
                            <div class="box custom-box box-primary">
                                <!-- /.box-header -->
                                <div class="box-header with-border">
                                    <h3 class="box-title">Nivel de muestreo</h3>
                                </div>
                                <!-- /.box-body -->
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div id="nivelMuestreo"></div>
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
