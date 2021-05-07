<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="AntecamaraBasculaFrigorifico.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.AntecamaraBasculaFrigorifico" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
        let descripcionAntecamara = "";
        let basculaDescripcion = "";

        $(function () {
            kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;
            let idBasculaNumber = 0;
            let idBascula = 0;
            let indiceBascula = 0;

            let idAntecamara = 0;
            let indiceAntecamara = 0;

            let idFrigorificocoNumber = 0;
            let idFrigorifico = 0;
            let indiceFrigorifico = 0;
            
            //Update
            let alertaB = [];
            let alertaF = [];
            let update = false;

            //Actualizar Antecamara Bascula
            function actualizarAtecamaraBascula(idAntecamaraBascula, idAntecamara, idBascula, activo) {
                //Se obtienen los datos para actualizar la Antecamara Basucula.
                let antecamaraBasculaGrid = $('#antecamaraBascula').data("kendoGrid");
                let param = {
                    idAntecamaraBascula: idAntecamaraBascula,
                    idAntecamara: idAntecamara,
                    idBascula: idBascula,
                    activo: activo,
                }
                let params = JSON.stringify({ antecamarasBascula: param });
                $.ajax({
                    //Actualizar antecamara bascula. 
                    url: "AntecamaraBasculaFrigorifico.aspx/EditarAntecamaraBascula",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                antecamaraBasculaGrid.dataSource.read();
                                break;
                            case 204:
                                antecamaraBasculaGrid.dataSource.read();
                                alert("Error al identificar el elemento a crear/actualizar");
                                break;
                            case 304:
                                antecamaraBasculaGrid.dataSource.read();
                                alert("La descripción ingresada ya existe");
                                break;
                            case 400:
                                antecamaraBasculaGrid.dataSource.read();
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                antecamaraBasculaGrid.dataSource.read();
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;

                        }
                    },
                    error: function (result) {
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                        antecamaraBasculaGrid.dataSource.read();
                    }
                });
            }

            //Actualizar antecamara frigorifico
            function actualizarAtecamaraFrigorifico(idAntecamaraFrigorifico, idAntecamara, idFrigorifico, activo) {
                //Se obtienen los datos para actualizar la Antecamara frigorifico.
                let antecamaraFrigorificoGrid = $('#antecamaraFrigorifico').data("kendoGrid");
                let param = {
                    idAntecamaraFrigorifico: idAntecamaraFrigorifico,
                    idAntecamara: idAntecamara,
                    idFrigorifico: idFrigorifico,
                    activo: activo,
                }
                let params = JSON.stringify({ antecamaraFrigorifico: param });
                $.ajax({
                    //Actualizar antecamara frigorifico 
                    url: "AntecamaraBasculaFrigorifico.aspx/EditarAntecamaraFrigorifico",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    data: params,
                    success: function (result) {
                        switch (result.d) {
                            case 200:
                                antecamaraFrigorificoGrid.dataSource.read();
                                break;
                            case 204:
                                antecamaraFrigorificoGrid.dataSource.read();
                                alert("Error al identificar el elemento a crear/actualizar");
                                break;
                            case 400:
                                antecamaraFrigorificoGrid.dataSource.read();
                                alert("El flujo de integración ha identificado un problema al tratar de crear/actualizar el registro");
                                break;
                            case 405:
                                antecamaraFrigorificoGrid.dataSource.read();
                                alert("La sesión ha expirado, la operación no puede ser completada");
                                break;
                            case 409:
                                result.options.error();
                                alert("La descripción ingresada ya existe");
                                break;
                        }
                    },
                    error: function (result) {
                        alert("Ha habido un error al crear/actualizar el elemento, por favor intente de nuevo");
                        antecamaraBasculaGrid.dataSource.read();
                    }
                });
                
            }

            function AntecamaraDDL(container, options) {

                $('<input id="AntecamaraDDL" name="AntecamaraDDL" required data-bind="value:value" data-required-msg="Campo requerido"/><span class="k-invalid-msg" data-for="AntecamaraDDL" ></span>')
                .appendTo(container);
                $("#AntecamaraDDL")
                .kendoDropDownList({
                    dataSource: {
                        schema: {
                            model: {
                                id: "idAntecamara",
                                fields: {
                                    nombreAntecamara: { type: "string" },
                                    activo: { type: "boolean" },
                                }
                            }
                        },
                        transport: {
                            read: function (options) {
                                $.ajax({
                                    //Obtener catalogo Antecamara para el DDL
                                    url: "AntecamaraBasculaFrigorifico.aspx/ObtenerAntecamara",
                                    contentType: "application/json;charset=utf-8",
                                    type: "POST",
                                    success: function (result) {
                                        for (i = 0; i <= result.d.antecamaras.length - 1; i++) {
                                            if (result.d.antecamaras[i].idAntecamara == idAntecamara) {
                                                //Se obtiene el indice del DDL  
                                                indiceAntecamara = i;
                                            }

                                        }
                                        options.success(result.d.antecamaras);
                                    },
                                    error: function (result) {
                                        options.error(result);
                                        alert("Ha habido un error al cargar la información");
                                    }
                                });

                            }
                        }
                    },
                    dataTextField: "nombreAntecamara",
                    dataValueField: "idAntecamara",
                    select: onSelectAntecamara,
                    dataBound: setIndiceAntecamara,
                });
            }

            function setIndiceAntecamara(evt) {
                //Se carga el ddl en el indice que le corresponde 
                if (evt.sender.dataSource.data().length > 0) {
                    evt.sender.select(indiceAntecamara);
                    idAntecamara = evt.sender.dataSource.data()[indiceAntecamara].idAntecamara
                }
                evt.sender.dataBound = false;
            }

            function onSelectAntecamara(e) {
                //Obtiene la descripcion de la antecamara seleccionada 
                let dataItem = e.dataItem;
                idAntecamara = dataItem.idAntecamara;
                descripcionAntecamara = dataItem.descripcionAntecamara;
            };

            function basculasDDL(container, options) {

                $('<input id="basculasDDL" name="basculasDDL" required data-bind="value:value" data-required-msg="Campo requerido"/><span class="k-invalid-msg" data-for="basculasDDL" ></span>')
                .appendTo(container);
                $("#basculasDDL")
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
                                    //Obtener catalogo Basculas.
                                    url: "AntecamaraBasculaFrigorifico.aspx/ObtenerBasculasActivas",
                                    contentType: "application/json;charset=utf-8",
                                     type: "POST",
                                     success: function (result) {
                                        for (i = 0; i <= result.d.basculas.length - 1; i++) {
                                            if (result.d.basculas[i].idBascula === idBascula) {
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
                    idBascula = evt.sender.dataSource.data()[indiceBascula].idBascula
                }
                evt.sender.dataBound = false;
            }

            function onSelectBascula(e) {
                //Obtiene la descripcion de la bascula seleccionada
                let dataItem = e.dataItem;
                idBascula = dataItem.idBascula;
                idBasculaNumber = dataItem.idBascula;
            };

            function frigorificoDDL(container, options) {

                 $('<input id="frigorificoDDL" name="frigorificoDDL" required data-bind="value:value" data-required-msg="Campo requerido"/><span class="k-invalid-msg" data-for="frigorificoDDL" ></span>')
                .appendTo(container);
                $("#frigorificoDDL")
                .kendoDropDownList({
                    dataSource: {
                        schema: {
                            model: {
                                id: "idFrigorifico",
                                fields: {
                                    descripcionFrigorifico: { type: "string" },
                                    activo: { type: "boolean" },
                                }
                            }
                        },
                        transport: {
                            read: function (options) {
                                let params = JSON.stringify({
                                    activo: true
                                });
                                $.ajax({
                                    //Obtener catalogo figorifico.
                                    url: "AntecamaraBasculaFrigorifico.aspx/ObtenerFrigorifico",
                                    contentType: "application/json;charset=utf-8",
                                    type: "POST",
                                    data: params,
                                    success: function (result) {
                                        for (i = 0; i <= result.d.frigorificos.length - 1; i++) {
                                            if (result.d.frigorificos[i].idFrigorifico == idFrigorifico) {
                                                 //Se obtiene el indice del DDL 
                                                indiceFrigorifico = i;
                                            }

                                        }
                                        options.success(result.d.frigorificos);
                                    },
                                    error: function (result) {
                                        options.error(result);
                                        alert("Ha habido un error al cargar la información");
                                    }
                                });
                            }
                        }
                    },
                    dataTextField: "descripcionFrigorifico",
                    dataValueField: "idFrigorifico",
                    select: onSelectFrigorifico,
                    dataBound: setIndiceFrigorifico,
                });
            }

            function setIndiceFrigorifico(evt) {
                //Se carga el ddl en el indice que le corresponde
                if (evt.sender.dataSource.data().length > 0) {
                    evt.sender.select(indiceFrigorifico);
                    idFrigorifico = evt.sender.dataSource.data()[indiceFrigorifico].idFrigorifico
                }
                evt.sender.dataBound = false;
            }

            function onSelectFrigorifico(e) {
                //Obtiene la descripcion de la bascula seleccionada
                let dataItem = e.dataItem;
                idFrigorifico = dataItem.idFrigorifico;
                idFrigorificocoNumber = dataItem.idFrigorifico;
            };

            window.calidad = $("#antecamaraBascula").kendoGrid({
                columns: [
                    {
                        field: "descripcionAntecamara",
                        title: "Antecámara",
                        editor: AntecamaraDDL,
                        width: 100,
                    },
                    {
                        field: "descripcionBascula",
                        title: "Báscula",
                        editor: basculasDDL,
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
                            id: "idAntecamaraBascula",
                            fields: {
                                descripcionAntecamara: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    }
                                },
                                descripcionBascula: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    }
                                },
                                idBascula: {
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
                                //Obtener antecamara bascula 
                                url: "AntecamaraBasculaFrigorifico.aspx/ObtenerAntecamaraBascula",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d != null) {
                                        for (let i = 0; i < result.d.antecamarasBasculas.length; i++) {
                                            //Se guarda en una variable los datos para su manipulación.
                                            alertaB[i] = result.d.antecamarasBasculas[i];
                                        }
                                        options.success(result.d.antecamarasBasculas);
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
                                idAntecamara: idAntecamara,
                                idBascula: idBascula,
                                activo: options.data.activo,
                            }
                            let params = JSON.stringify({ antecamarasBascula: param });
                            $.ajax({
                                //Se crea la relacion Antecamara bascula.
                                url: "AntecamaraBasculaFrigorifico.aspx/CrearAntecamaraBascula",
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    switch (result.d) {
                                        case 200:
                                            idAntecamara = 0;
                                            idBascula = 0;
                                            options.success(result.d);
                                            break;
                                        case 304:
                                            idAntecamara = 0;
                                            idBascula = 0;
                                            alert("EL REGISTRO YA EXISTE, NO ES POSIBLE AGREGARLO DE NUEVO");
                                            let antecamaraBasculaGrid = $('#antecamaraBascula').data("kendoGrid");
                                            antecamaraBasculaGrid.dataSource.read();
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
                            for (let a = 0; a < alertaB.length; a++) {
                                if (options.data.idAntecamaraBascula == alertaB[a].idAntecamaraBascula) {
                                    //Se valida que no se encuentre activa la alerta para mostrar el confirm.
                                    if (options.data.activo == false && options.data.activo != alertaB[a].activo) {
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
                                    let idAntecamaraBascula = options.data.idAntecamaraBascula;
                                    let idAntecamara = options.data.idAntecamara;
                                    let idBascula = options.data.idBascula;
                                    let activo = options.data.activo;
                                    //se envian los parametros para desactivar el row
                                    actualizarAtecamaraBascula(idAntecamaraBascula, idAntecamara, idBascula, activo);
                                }
                            } else {
                                let idAntecamaraBascula = options.data.idAntecamaraBascula;
                                let idAntecamara = options.data.idAntecamara;
                                let idBascula = idBasculaNumber;
                                let activo = options.data.activo;
                                //se envian los parametros para la actualizacion.
                                actualizarAtecamaraBascula(idAntecamaraBascula, idAntecamara, idBascula, activo);
                            }
                        },
                    },
                },
                scrollable: true,
                resizable: true,
                editable: "inline",
                edit: function(e) {
                    if (!e.model.isNew()) {
                        idBascula = e.model.idBascula;
                        idAntecamara = e.model.idAntecamara;
                    } else {
                        idBascula = 0;
                        indiceBascula = 0;
                        idAntecamara = 0;
                        indiceAntecamara = 0;
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
                    idBascula = 0;
                    indiceBascula = 0;
                    idAntecamara = 0;
                    indiceAntecamara = 0;
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
                navigatable: true
            });

            window.antecamaraFrigorifico = $("#antecamaraFrigorifico").kendoGrid({
                columns: [
                    {
                        field: "descripcionAntecamara",
                        title: "Antecámara",
                        editor: AntecamaraDDL,
                        width: 100,
                    },
                    {
                        field: "descripcionFrigorifico",
                        title: "Frigorífico",
                        editor: frigorificoDDL,
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
                            id: "idAntecamaraFrigorifico",
                            fields: {
                                descripcionAntecamara: {
                                    type: "string",
                                    validation: {
                                        required: {
                                            message: "Campo requerido"
                                        }
                                    }
                                },
                                descripcionFrigorifico: {
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
                                //Obtener datos de relacion Antecamara frigorifico
                                url: "AntecamaraBasculaFrigorifico.aspx/ObtenerAntecamaraFrigorifico",
                                contentType: "application/json;charset=utf-8",
                                type: "POST",
                                success: function (result) {
                                    if (result.d != null) {
                                        for (let i = 0; i < result.d.antecamarasFrigorificos.length; i++) {
                                            //Se guarda en una variable los datos para su manipulación.
                                            alertaF[i] = result.d.antecamarasFrigorificos[i];
                                        }
                                        options.success(result.d.antecamarasFrigorificos);
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
                                idAntecamara: idAntecamara,
                                idFrigorifico: idFrigorifico,
                                activo: options.data.activo,
                            }
                            let params = JSON.stringify({ antecamaraFrigorifico: param });
                            $.ajax({
                                //se crea la relacion antecamar frigorifico.
                                url: "AntecamaraBasculaFrigorifico.aspx/CrearAntecamarFrigorifico",
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                                data: params,
                                success: function (result) {
                                    switch (result.d) {
                                        case 200:
                                            idAntecamara = 0;
                                            idFrigorifico = "";
                                            options.success(result.d);
                                            break;
                                        case 304:
                                            alert("EL REGISTRO YA EXISTE, NO ES POSIBLE AGREGARLO DE NUEVO");
                                            let antecamaraFrigorificoGrid = $('#antecamaraFrigorifico').data("kendoGrid");
                                            antecamaraFrigorificoGrid.dataSource.read();
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
                            for (let a = 0; a < alertaF.length; a++) {
                                if (options.data.idAntecamaraFrigorifico == alertaF[a].idAntecamaraFrigorifico) {
                                    //Se valida que no se encuentre activa la alerta para mostrar el confirm.
                                    if (options.data.activo == false && options.data.activo != alertaF[a].activo) {
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
                                    let idAntecamaraFrigorifico = options.data.idAntecamaraFrigorifico;
                                    let idAntecamara = options.data.idAntecamara;
                                    let idFrigorifico = options.data.StridFrigorifico;
                                    let activo = options.data.activo;
                                    //se envian los parametros para desactivar el row
                                    actualizarAtecamaraFrigorifico(idAntecamaraFrigorifico, idAntecamara, idFrigorifico, activo);
                                }
                            } else {
                                let idAntecamaraFrigorifico = options.data.idAntecamaraFrigorifico;
                                let idAntecamara = options.data.idAntecamara;
                                let idFrigorifico = idFrigorificocoNumber;
                                let activo = options.data.activo;
                                //se envian los parametros para la actualizacion.
                                actualizarAtecamaraFrigorifico(idAntecamaraFrigorifico, idAntecamara, idFrigorifico, activo);
                            }
                        },
                    },
                },
                scrollable: true,
                resizable: true,
                editable: "inline",
                edit: function(e) {
                    if (!e.model.isNew()) {
                        idAntecamara = e.model.idAntecamara;
                        idFrigorifico = e.model.idFrigorifico;
                    } else {
                        idAntecamara = 0;
                        indiceAntecamara = 0;
                        idFrigorifico = 0;
                        indiceFrigorifico = 0;
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
                    idAntecamara = 0;
                    indiceAntecamara = 0;
                    idFrigorifico = 0;
                    indiceFrigorifico = 0;
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
                navigatable: true

            });
        });

        $(function () {
            let count = $("#antecamaraBascula").offset().top + 30;

            count = $(".k-grid-toolbar").length ? count + $(".k-grid-toolbar").innerHeight() : count;
            count = $(".k-grid-header").length ? count + $(".k-grid-header").innerHeight() : count;
            count = $(".k-grid-pager").length ? count + $(".k-grid-pager").innerHeight() : count;

            $(".k-grid-content").css("max-height", `calc(100vh - ${count}px)`);
        }); 

        $(function () {
            let count = $("#antecamaraFrigorifico").offset().top + 30;

            count = $(".k-grid-toolbar").length ? count + $(".k-grid-toolbar").innerHeight() : count;
            count = $(".k-grid-header").length ? count + $(".k-grid-header").innerHeight() : count;
            count = $(".k-grid-pager").length ? count + $(".k-grid-pager").innerHeight() : count;

            $(".k-grid-content").css("max-height", `calc(100vh - ${count}px)`);
        }); 

  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Relación Antecámara-Báscula-Frigorífico" id="hPage" />
<section id="Section1" class="custom-content" runat="server">
        <div class="bs-component">
            <div class="clearfix">
                <div class="row">
                    <div class="col-sm-12">
                        <!-- /.box-content -->
                        <div class="box custom-box box-primary">
                            <!-- /.box-header -->
                            <div class="box-header with-border">
                                <h3 class="box-title">Relación antecámara-báscula</h3>
                            </div>
                            <!-- /.box-body -->
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div id="antecamaraBascula"></div>
                                    </div>
                                </div>
                            </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    <br />
    <div class="bs-component">
            <div class="clearfix">
                <div class="row">
                    <div class="col-sm-12">
                        <!-- /.box-content -->
                        <div class="box custom-box box-primary">
                            <!-- /.box-header -->
                            <div class="box-header with-border">
                                <h3 class="box-title">Relación antecámara-frigorífico</h3>
                            </div>
                            <!-- /.box-body -->
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div id="antecamaraFrigorifico"></div>
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
