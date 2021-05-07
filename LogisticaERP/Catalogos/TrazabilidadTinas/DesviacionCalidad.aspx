<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="DesviacionCalidad.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.DesviacionCalidad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>

       $(function () {
            kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;
            let problemasP = [];
            let update = false;

           function actualizar(idProblema, problema, clave, descripcion, restriccion, calidadERP, claveProblemaOracle, activo) {
               //Se obtienen los datos para actualizar.
                let desviacionCalidadAlertaGrid = $('#desviacionCalidad').data("kendoGrid");
                let param = {
                    idProblema: idProblema,
                    problema: problema,
                    clave: clave,
                    descripcion: descripcion,
                    restriccion: restriccion,
                    calidadERP: calidadERP,
                    claveProblemaOracle: claveProblemaOracle,
                    activo: activo
                }
               let params = JSON.stringify({ problema: param });
               $.ajax({
                    //Actualizar desviacion de calidad
                    url: "DesviacionCalidad.aspx/EditarDesviacionCalidad",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                desviacionCalidadAlertaGrid.dataSource.read();
                                break;
                            case 204:
                                desviacionCalidadAlertaGrid.dataSource.read();
                                alert("Error al identificar el elemento a crear/actualizar");
                                break;
                            case 400:
                                desviacionCalidadAlertaGrid.dataSource.read();
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                desviacionCalidadAlertaGrid.dataSource.read();
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 409:
                                desviacionCalidadAlertaGrid.dataSource.read();
                                alert("La descripción ingresada ya existe");
                                break;
                        }
                    },
                    error: function (result) {
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                        desviacionCalidadAlertaGrid.dataSource.read();
                    }
                });
           }

            window.desviacionCalidad = $("#desviacionCalidad").kendoGrid({
                columns: [
                    {
                        field: "problema",
                        title: "Problema",
                        width: 100,
                    },
                    {
                        field: "clave",
                        title: "Clave",
                        width: 100,
                    },
                    {
                        field: "descripcion",
                        title: "Descripción",
                        width: 100,
                    },
                    {
                        field: "restriccion",
                        title: "Problema con restricción",
                        template: '<center><input type="checkbox" #= restriccion==true ? checked="checked" : "" # disabled="disabled" ></input></center>',
                        width: 100,
                        attributes: {
                            "class": "checkbox-cell"
                        }
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
                            id: "idProblema",
                            fields: {
                                problema: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    },
                                },
                                clave: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    },
                                },
                                descripcion: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    },
                                },
                                restriccion: {
                                    type: "boolean",
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
                                //Obtener desviacion de calidad 
                                url: "DesviacionCalidad.aspx/ObtenerDesviacionCalidad",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                               success: function (result) {
                                    if (result.d != null) {
                                        for (let i = 0; i < result.d.problemas.length; i++) {
                                            //Se guarda en una variable los datos de calidad para su manipulación.
                                            problemasP[i] = result.d.problemas[i];
                                        }
                                        options.success(result.d.problemas);
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
                                problema: options.data.problema,
                                clave: options.data.clave,
                                descripcion: options.data.descripcion,
                                restriccion: options.data.restriccion,
                                activo: options.data.activo
                            }
                            let params = JSON.stringify({ problema: param });
                            $.ajax({
                                //Se crea la desviacion de calidad.
                                url: "DesviacionCalidad.aspx/CrearDesviacionCalidad",
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
                                    alert("Ha habido un error al crear el elemento, por favor intente de nuevo"); 
                                }    
                            });
                        },
                        update: function (options) {
                            update = false;
                            for (let a = 0; a < problemasP.length; a++) {
                                //Se valida que no se encuentre activa la desviacion de calidad para mostrar el confirm.
                                if (options.data.idProblema == problemasP[a].idProblema) {
                                    if (options.data.activo == false && options.data.activo != problemasP[a].activo) {
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
                                    let idProblema = options.data.idProblemaString;
                                    let problema = options.data.problema;
                                    let clave = options.data.clave;
                                    let descripcion = options.data.descripcion;
                                    let restriccion = options.data.restriccion;
                                    let calidadERP = options.data.calidadERP;
                                    let claveProblemaOracle = options.data.claveProblemaOracle;
                                    let activo = options.data.activo;
                                    //se envian los parametros para desactivar el row
                                    actualizar(idProblema, problema, clave, descripcion, restriccion, calidadERP, claveProblemaOracle, activo);
                                }
                            } else {
                                let idProblema = options.data.idProblemaString;
                                let problema = options.data.problema;
                                let clave = options.data.clave;
                                let descripcion = options.data.descripcion;
                                let restriccion = options.data.restriccion;
                                let calidadERP = options.data.calidadERP;
                                let claveProblemaOracle = options.data.claveProblemaOracle;
                                let activo = options.data.activo;
                                //se envian los parametros para actualizar el row
                                actualizar(idProblema, problema, clave, descripcion, restriccion, calidadERP, claveProblemaOracle, activo);
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
            let count = $("#desviacionCalidad").offset().top + 30;

            count = $(".k-grid-toolbar").length ? count + $(".k-grid-toolbar").innerHeight() : count;
            count = $(".k-grid-header").length ? count + $(".k-grid-header").innerHeight() : count;
            count = $(".k-grid-pager").length ? count + $(".k-grid-pager").innerHeight() : count;

            $(".k-grid-content").css("max-height", `calc(100vh - ${count}px)`);
        }); 
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
<input type="hidden" value="Desviación de calidad" id="hPage" />

<section id="Section1" class="custom-content" runat="server">
            <div class="bs-component">
                <div class="clearfix">
                    <div class="row">
                        <div class="col-sm-12">
                            <!-- /.box-content -->
                            <div class="box custom-box box-primary">
                                <!-- /.box-header -->
                                <div class="box-header with-border">
                                    <h3 class="box-title">Desviación de calidad</h3>
                                </div>
                                <!-- /.box-body -->
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div id="desviacionCalidad"></div>
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
