<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="BarcoViaje.aspx.cs" Inherits="LogisticaERP.Catalogos.TrazabilidadTinas.BarcoViaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
    $(function () {
        kendo.jQuery.uniqueSort = kendo.jQuery.uniqueSort ? kendo.jQuery.uniqueSort : kendo.jQuery.unique;

        let URLactual = window.location;

        let StringidBarco = "0";
        let StringidViaje = "0"
        let fechaFin = "";
        let fechaInicio = "";
        let barcos = [];
        let primeraCarga = false;

        if (!primeraCarga) {
            fechaInicio = kendo.toString(kendo.parseDate(new Date()), 'yyyy-MM-dd');
            fechaFin = kendo.toString(kendo.parseDate(new Date()), 'yyyy-MM-dd');
            primeraCarga = true;
        }

        if (URLactual.href.length > 70) {
            //Se recibe el URL de donde se extrae los datos.
            fechaInicio = URLactual.search.substr(14, 10);
            fechaFin = URLactual.search.substr(34, 10);
            StringidBarco = URLactual.search.substr(59, 17) == ""? "000":URLactual.search.substr(59, 17);

            $("#fechaFinDP").val(URLactual.search.substr(34, 10));
            $("#fechaInicioDP").val(URLactual.search.substr(14, 10));
            $("#BarcoDDL").val(URLactual.search.substr(59, 17));
        }

        $('#fechaInicioDP').kendoDatePicker({
            value: fechaInicio,
            format: "yyyy-MM-dd",
            culture: "es-MX",
            weekNumber: true,
            change: function () {
                fechaInicioSeleccion = kendo.toString(kendo.parseDate(this.value()), 'yyyy-MM-dd');
                fechaInicio = fechaInicioSeleccion;
                if (StringidBarco != "0") {
                    let barcoViajeGrid = $('#barcoViaje').data("kendoGrid");
                    barcoViajeGrid.dataSource.read();
                } 
            }
        });

        $('#fechaFinDP').kendoDatePicker({
            value: fechaFin,
            format: "yyyy-MM-dd",
            culture: "es-MX",
            weekNumber: true,
            change: function () {
                fechaFinSeleccion = kendo.toString(kendo.parseDate(this.value()), 'yyyy-MM-dd');
                fechaFin = fechaFinSeleccion;
                if (StringidBarco != "0") {
                    let barcoViajeGrid = $('#barcoViaje').data("kendoGrid");
                    barcoViajeGrid.dataSource.read();
                } 
            }
        });

        $('#BarcoDDL').kendoDropDownList({
            enable: true,
            dataValueField: "StringidBarco",
            dataTextField: "barco",
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            //Obtener catalogo Barco
                            url: "BarcoViaje.aspx/CargarDDLBarco",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            success: function (result) {
                                if (result.d != null) {
                                    for (let i = 0; i <= result.d.barcosViajes.length - 1; i++) {
                                        //Se guarda en una variable los datos para su manipulación.
                                        barcos[i] = result.d.barcosViajes[i].barco;
                                    }
                                    for (var j = barcos.length - 1; j >= 0; j--){
                                        //En el ddl se eliminan los barcos repetidos.
                                        if (barcos.indexOf(barcos[j]) !== j) {
                                            let index = j;
                                            result.d.barcosViajes.splice(index, 1);
                                        }
                                    }
                                    options.success(result.d.barcosViajes);
                                }else {
                                    alert("Error al cargar la información");
                                }
                                
                            }
                        });
                    }
                }
            },
            change: dropdownlistBarcoViaje,
            optionLabel:"Seleccionar barco",
        });

        function dropdownlistBarcoViaje() {
            //Actualiza los datos del DDL para barco viaje
            var dropdownlist = $("#BarcoDDL").data("kendoDropDownList");
            StringidBarco = dropdownlist.dataItem().StringidBarco;
            if (StringidBarco == "") {
                StringidBarco = "0"
                let barcoViajeGrid = $('#barcoViaje').data("kendoGrid");
                barcoViajeGrid.dataSource.read();
            }

            if (StringidBarco != "0") {
                let barcoViajeGrid = $('#barcoViaje').data("kendoGrid");
                barcoViajeGrid.dataSource.read();
            } 
        }

        //Botones.
        //Consultar
        function consultarBtn(e) {
            // e.target es el DOM del actual elemento representando del boton
            var tr = $(e.target).closest("tr"); 
            // Toma el data bound del actual row
            var data = this.dataItem(tr);
            StringidViaje = data.StringidViaje;
            //se envia en el url los datos para mantener la consulta en el navegador. 
            window.location.replace("BarcoViajeConsultar.aspx?&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin + "&StringidBarco=" + StringidBarco + "&StringidViaje=" + StringidViaje);
        }
        //Editar
        function editarBtn(e) {
            // e.target es el DOM del actual elemento representando del boton
            var tr = $(e.target).closest("tr"); 
            // Toma el data bound del actual row
            var data = this.dataItem(tr);
            StringidViaje = data.StringidViaje;
            //se envia en el url los datos para mantener la consulta en el navegador. 
            window.location.replace("BarcoViajeEditar.aspx?&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin + "&StringidBarco=" + StringidBarco + "&StringidViaje=" + StringidViaje);
        }

        window.barcoViaje = $("#barcoViaje").kendoGrid({
            columns: [
                {
                    field: "barco",
                    title: "Barco",
                    editor: BarcoDDL,
                    width: 100
      
                },
                {
                    field: "ejercicio",
                    title: "Ejercicio",
                    width: 100
      
                },
                {
                    field: "viaje",
                    title: "Viaje",
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
                        click: consultarBtn,
                        text: "Consultar",
                    },
                    {
                        click: editarBtn,
                        text: "Editar",
                    },
                ],
                    title: "",
                    width: 140
                }
                    
            ],
            dataBound: function (e) {
                // Aplicar una clase css para el row si el timepo restante es menor a la toleancia

                //Encuentras la posicion dentro de la tabla.
                let columnIndex = this.wrapper.find(".k-grid-header [data-field=" + "activo" + "]").index() + 1;
                //Se envia el cuerpo del row
                let rows = e.sender.tbody.children();
                //se recorre el row
                for (let j = 0; j <= rows.length - 1; j++) {
                    let row = $(rows[j]);
                    let dataItem = e.sender.dataItem(row);
                    let activado = dataItem.activo;
                    let descargado = dataItem.descargado;
                    let subviaje = dataItem.subviaje;
                    //Se desactiva el botón de editar
                    if (activado == false || descargado == true) {
                        var cell = row.children().eq(columnIndex);
                        cell.children(':eq(1)').hide();
                    }
                }
            },
            dataSource: {
                sync: function () {
                    this.read();
                },
                pageSize:15,
                schema: {
                    model: {
                        id: "idBarco",
                        fields: {
                            barco: {
                                type:"string",
                            },
                            ejercicio: {
                                type: "string",
                            },
                            viaje: {
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
                        var param = JSON.stringify({
                            idBarco: StringidBarco,
                            fechaInicio: fechaInicio,
                            fechaFin: fechaFin
                        });
                        $.ajax({
                            //Obtener catalogo de barco
                            url: "BarcoViaje.aspx/ObtenerCatalogoBarco",
                            contentType: "application/json;charset=utf-8",
                            type: "POST",
                            data: param,
                            success: function (result) {
                                if (!result.d) {
                                    alert("Ha habido un error al obtener la información");
                                    options.error(result.d == []);
                                } else {
                                    options.success(result.d.barcosViajes);
                                }
                            },
                            error: function (result) {
                                options.error(result);
                                alert("Ha habido un error al cargar la información del recurso de integración");
                            }
                        });
                    },
                    create: function (options) {

                    },
                        
                },
            },
            scrollable: true,
            resizable: true,
            editable: "inline",
            toolbar: [
                {
                    template: '<a class="k-button" href="\\#" onclick="return toolbar_click()"><span class="glyphicon glyphicon-plus"></span></a>'
                }
            ],
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
        
        $("#fechaInicioDP").attr("readonly", true);
        $("#fechaFinDP").attr("readonly", true);

    });

    function toolbar_click() {
        //Se dirige a la pantalla crear con los parametros de busqueda.
        let fechaInicio = $("#fechaInicioDP").val();
        let fechaFin = $("#fechaFinDP").val();
        let StringidBarco = $("#BarcoDDL").val();
        window.location.replace("BarcoViajeCrear.aspx?&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin + "&StringidBarco=" + StringidBarco);
        return false;
    }

    $(function () {
        let count = $("#barcoViaje").offset().top + 30;

        count = $(".k-grid-toolbar").length ? count + $(".k-grid-toolbar").innerHeight() : count;
        count = $(".k-grid-header").length ? count + $(".k-grid-header").innerHeight() : count;
        count = $(".k-grid-pager").length ? count + $(".k-grid-pager").innerHeight() : count;

        $(".k-grid-content").css("max-height", `calc(100vh - ${count}px)`);
    }); 

  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="360000" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
    <input type="hidden" value="Barco - Viaje " id="hPage" />

    <section id="divCustomContent" class="custom-content" runat="server">
                <div class="bs-component">
                    <div class="clearfix">
                        <br />
                        <div class="row">
                            <div class="col-sm-12">
                                <!-- /.box-content -->
                                <div class="box custom-box box-primary">
                                    <!-- /.box-header -->
                                    <div class="box-header with-border">
                                        <h3 class="box-title"></h3>
                                    </div>
                                    <div class="col-sm-12">
                                        <div class="col-sm-3">
                                            <label for="lbltrazabilidadCabezaDP" class="control-label-top">Fecha Inicio: </label>
                                            <input id="fechaInicioDP" class="col-sm color-DP"/>
                                        </div>
                                        <div class="col-sm-3">
                                            <label id="lbltrazabilidadCabezaDP" class="control-label-top">Fecha Fin: </label>
                                            <input id="fechaFinDP" class="col-sm color-DP"/>
                                        </div>
                                        <div class="col-sm-3">
                                            <label for="lblContenedorTermico" class="control-label-top">Seleccionar barco: </label>
                                            <input id="BarcoDDL" type="text"/>
                                        </div>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-6">
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
                                        <h3 class="box-title">Barco/Viaje</h3>
                                    </div>
                                    <!-- /.box-body -->
                                    <div class="box-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div id="barcoViaje"> 
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                 </div>
                             </div>
                          </div>
                       </div>
                  </div>
        </section>
</asp:Content>
