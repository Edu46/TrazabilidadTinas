<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="Zona.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.Zona" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
        $(function () {

            kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;
            let zonasDescargaP = [];
            let update = false;

            function actualizar(idZonaDescarga, descripcion, activo) {
                //Se obtienen los datos para actualizar las zonas.
                let zonasGrid = $('#zonas').data("kendoGrid");
                let param = {
                    idZonaDescarga: idZonaDescarga,
                    descripcion: descripcion,
                    activo: activo
               }
                let params = JSON.stringify({ catalogoZona: param });
                $.ajax({
                    //Actualizar zonas
                    url: "Zona.aspx/EditarCatalogoZona",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                zonasGrid.dataSource.read();
                                break;
                            case 204:
                                zonasGrid.dataSource.read();
                                alert("Error al identificar el elemento a crear/actualizar");
                                break;
                            case 400:
                                zonasGrid.dataSource.read();
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                zonasGrid.dataSource.read();
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 409:
                                zonasGrid.dataSource.read();
                                alert("La descripción ingresada ya existe");
                                break;
                        }
                    },
                    error: function (result) {
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                        zonasGrid.dataSource.read();
                    }
                });
           }

            window.zonas = $("#zonas").kendoGrid({
                columns: [
                    {
                        field: "descripcion",
                        title: "Zonas",
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
                            id: "idZonaDescarga",
                            fields: {
                                descripcion: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido",
                                        },
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
                                //Obtener los catalogos de zonas 
                                url: "Zona.aspx/ObtenerZonas",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d != null) {
                                        for (let i = 0; i < result.d.zonasDescarga.length; i++) {
                                            //Se guarda en una variable los datos para su manipulación.
                                            zonasDescargaP[i] = result.d.zonasDescarga[i];
                                        }
                                        options.success(result.d.zonasDescarga);
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
                                activo: options.data.activo
                            }
                            let params = JSON.stringify({ catalogoZona: param });
                            $.ajax({
                                //Se crea la zona
                                url: "Zona.aspx/CrearZona",
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
                            for (let a = 0; a < zonasDescargaP.length; a++) {
                                if (options.data.idZonaDescarga == zonasDescargaP[a].idZonaDescarga) {
                                    //Se valida que no se encuentre activa la alerta para mostrar el confirm.
                                    if (options.data.activo == false && options.data.activo != zonasDescargaP[a].activo) {
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
                                    let idZonaDescarga = options.data.idZonaDescarga;
                                    let descripcion = options.data.descripcion;
                                    let activo = options.data.activo;
                                    //se envian los parametros para desactivar el row
                                    actualizar(idZonaDescarga, descripcion, activo);
                                }
                            } else {
                                let idZonaDescarga = options.data.idZonaDescarga;
                                let descripcion = options.data.descripcion;
                                let activo = options.data.activo;
                                //se envian los parametros para actualizar el row
                                actualizar(idZonaDescarga, descripcion, activo);
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
            let count = $("#zonas").offset().top + 30;

            count = $(".k-grid-toolbar").length ? count + $(".k-grid-toolbar").innerHeight() : count;
            count = $(".k-grid-header").length ? count + $(".k-grid-header").innerHeight() : count;
            count = $(".k-grid-pager").length ? count + $(".k-grid-pager").innerHeight() : count;

            $(".k-grid-content").css("max-height", `calc(100vh - ${count}px)`);
        }); 
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Zonas" id="hPage" />
<section id="Section1" class="custom-content" runat="server">
            <div class="bs-component">
                <div class="clearfix">
                    <div class="row">
                        <div class="col-sm-12">
                            <!-- /.box-content -->
                            <div class="box custom-box box-primary">
                                <!-- /.box-header -->
                                <div class="box-header with-border">
                                    <h3 class="box-title">Zonas</h3>
                                </div>
                                <!-- /.box-body -->
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div id="zonas"></div>
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
