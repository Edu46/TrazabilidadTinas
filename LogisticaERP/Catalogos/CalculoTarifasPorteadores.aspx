<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="CalculoTarifasPorteadores.aspx.cs" Inherits="LogisticaERP.Catalogos.CalculoTarifasPorteadores" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .table-responsive { border: 0px !important; }

        .multiselect-right ~div ul {
             right: inherit; 
             left: inherit; 
        }

        .box-header.with-border {
            border-bottom: 1px solid #f4f4f4;
            height: inherit;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" EnablePageMethods="true" runat="server" />
    <input id="hPage" type="hidden" value="Calculo de Pago a Porteadores" />
    <input type="hidden" runat="server" id="hfEmpresaCloudException" value="" />
    <input type="hidden" runat="server" id="hfIDCalculoTarifaPorteador" value="" />
    <input type="hidden" runat="server" id="hfFechaViaje" value="" />
    <input type="hidden" runat="server" id="hfIDTarifa" value="" />
    <input type="hidden" runat="server" id="hfIDOrdenCompra" value="" />
    <input type="hidden" runat="server" id="hfIDProveedorOracle" value="" />
    <input type="hidden" runat="server" id="hfIDAlmacenOracle" value="" />
    <!-- Main content -->
    <section id="divCustomContent" class="custom-content" runat="server">
        <div class="bs-component">
            <div class="clearfix">
                <div class="row">
                    <!-- /Primera Tabla -->
                    <div class="col-lg-12" style="margin-top: 0px;">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Información del Viaje</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-2">
                                        <label for="lblTipoViaje" class="control-label-top">Tipo Viaje: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtTipoViaje" placeholder="Tipo Viaje" autocomplete="off" maxlength="250" value="ENTREGA A CLIENTE" disabled="disabled" />
                                    </div>
                                    <div class="col-lg-2">
                                        <label for="lblFechaDesde" class="control-label-top">Viajes Fecha Desde:</label>
                                        <dx:ASPxDateEdit ID="deFechaInicio" runat="server" placeholder="Fecha Desde" autocomplete="off" CssClass="form-control-date" Theme="MetropolisBlue" Date="">
                                            <ClientSideEvents DateChanged="function() { setDate(); }"/>
                                        </dx:ASPxDateEdit>
                                    </div>
                                    <div class="col-lg-2">
                                        <label for="lblFechaHasta" class="control-label-top">Viajes Fecha Hasta:</label>
                                        <dx:ASPxDateEdit ID="deFechaFin" runat="server" placeholder="Fecha Hasta" autocomplete="off" CssClass="form-control-date" Theme="MetropolisBlue" Date=""></dx:ASPxDateEdit>
                                    </div>
                                    <div class="col-lg-2">
                                        <label for="lblNumViaje" class="control-label-top">Núm. Viaje: </label>
                                        <div class="input-group">
                                            <input runat="server" type="text" class="form-control marginoff" id="txtNumViaje" placeholder="Núm. Viaje" autocomplete="off" maxlength="250" disabled="disabled" validate="true" />
                                            <span class="input-group-btn">
                                                <input type="button" id="btnBuscar" class="btn custom-btn btn-default" value="..." />
                                            </span>
                                        </div>
                                    </div>
                                    <div class="col-lg-2">
                                        <label for="lblOrdenCompraCloud" class="control-label-top">OC Cloud: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtOrdenCompraCloud" placeholder="Orden Compra Cloud" autocomplete="off" maxlength="250" disabled="disabled" />
                                    </div>
                                    <div class="col-lg-2">
                                        <label for="lblAlmacenOrigen" class="control-label-top">Almacén Origen: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtAlmacenOrigen" placeholder="Almacén Origen" autocomplete="off" maxlength="250" disabled="disabled" validate="true" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <label for="lblProveedor" class="control-label-top">Proveedor: </label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtProveedor" placeholder="Proveedor" autocomplete="off" maxlength="250" disabled="disabled" validate="true" />
                                    </div>
                                    <div class="col-lg-1">
                                        <a id="btnCalcular" class="btn custom-btn btn-link" style="margin-top: 26px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Calcular"><i class="fa fa-calculator" aria-hidden="true" style="font-size: 20px;margin-right: 6px;"></i></a>
                                    </div>
                                    <div class="col-lg-1">
                                        <a id="btnGenerarOC" class="btn custom-btn btn-link" style="margin-top: 26px; width: 100%; color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Generar OC Cloud"><i class="fa fa-file-text-o" aria-hidden="true" style="font-size: 20px;margin-right: 6px;"></i></a>
                                    </div>
                                    <div class="col-lg-1">
                                        <a id="btnLimpiar" class="btn custom-btn btn-link" style="margin-top: 26px; width: 100%;  color: #103d5f !important;" data-toggle="tooltip" data-placement="top" title="Limpiar Forma"><i class="fa fa-eraser" aria-hidden="true" style="font-size: 20px;margin-right: 6px;"></i></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <!-- /Segunda Tabla -->
                    <div class="col-lg-10" style="margin-top: 0px;">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Detalles del Flete</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-4">
                                        <label for="lblEstado" class="control-label-top">Estado:</label>
                                        <asp:DropDownList runat="server" ID="ddlEstado" placeholder="Estado" ClientIDMode="Static" CssClass="multiselect multiselect-right form-control marginoff" data-style="btn-select-default"  validate="true" />
                                    </div>
                                    <div class="col-lg-4">
                                        <label for="lblMunicipio" class="control-label-top">Municipio:</label>
                                        <asp:DropDownList runat="server" ID="ddlMunicipio" placeholder="Municipio" ClientIDMode="Static" CssClass="multiselect multiselect-right form-control marginoff" data-style="btn-select-default" validate="true" />
                                    </div>
                                    <div class="col-lg-4">
                                        <label for="lblCiudad" class="control-label-top">Ciudad:</label>
                                        <asp:DropDownList runat="server" ID="ddlCiudad" placeholder="Ciudad" ClientIDMode="Static" CssClass="multiselect multiselect-right form-control marginoff" data-style="btn-select-default" validate="true" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6">
                                        <label for="lblCodigoTransporte" class="control-label-top">Cód. Transporte:</label>
                                        <asp:DropDownList runat="server" ID="ddlCodigoTransporte" placeholder="Código Transporte" ClientIDMode="Static" CssClass="multiselect multiselect-right form-control marginoff" data-style="btn-select-default" validate="true" />
                                    </div>
                                    <div class="col-lg-2">
                                        <label for="lblBackhaul" class="control-label-top"></label>
                                        <dx:ASPxCheckBox runat="server" elemID="checkbox" CheckBoxStyle-Cursor="pointer" ID="chkBackhaul" Checked="False" Theme="Mulberry"></dx:ASPxCheckBox>
                                        <label for="lblBackhaul">Backhaul</label>
                                    </div>
                                    <div class="col-lg-2">
                                        <label for="lblCasetas" class="control-label-top"></label>
                                        <dx:ASPxCheckBox runat="server" elemID="checkbox" CheckBoxStyle-Cursor="pointer" ID="chkCasetas" Checked="False" Theme="Mulberry"></dx:ASPxCheckBox>
                                        <label for="lblCasetas">Casetas</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /Tercer Tabla -->
                    <div class="col-lg-2" style="margin-top: 0px;">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Calculo de Tarifa</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <label for="lblTransporte" class="control-label-top">Transporte:</label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtTransporte" placeholder="Transporte" autocomplete="off" maxlength="250" disabled="disabled" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <label for="lblCasetas" class="control-label-top">Casetas:</label>
                                        <input runat="server" type="text" class="form-control marginoff" id="txtCasetas" placeholder="Casetas" autocomplete="off" maxlength="250" disabled="disabled" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script type="text/javascript">
        "use strict";

        var attrEvents = function () {
            if ($("#hfEmpresaCloudException").val()) {
                alert($("#hfEmpresaCloudException").val());
                location.href = "../../Inicio.aspx";
            }

            $(".navbar-btn.sidebar-toggle").click();

            $("#btnBuscar").click(function myfunction() {
                var fechaInicio = deFechaInicio.GetDate();
                var fechaFin = deFechaFin.GetDate();
                if (!fechaInicio || !fechaFin) {
                    showMessage(messageType.WARNINGBOX, "Favor de seleccionar un rango de fechas a buscar.");
                    return;
                }

                $("#btnLimpiar").click();
                showLoading(true);
                $.ajax({
                    url: "CalculoTarifasPorteadores.aspx/EstablecerModalViajes",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ fechaInicio: '" + deFechaInicio.GetText() +
                        "', fechaFin: '" + deFechaFin.GetText() + "' }",
                    timeout: 600000,
                    success: function () {
                        showLoading(false);
                        showModal("../../Controles/BusquedaGenerica/Busqueda.aspx", "Viajes", "100%", "100%");
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#btnCalcular").click(function () {
                if ($("#txtNumViaje").val().trim() === "") {
                    showMessage(messageType.WARNINGBOX, "El campo Núm. Viaje no debe estar vacío.");
                    return;
                } else if ($("#txtAlmacenOrigen").val().trim() === "") {
                    showMessage(messageType.WARNINGBOX, "El campo Almacen Origen no debe estar vacío.");
                    return;
                } else if ($("#txtProveedor").val().trim() === "") {
                    showMessage(messageType.WARNINGBOX, "El campo Proveedor no debe estar vacío.");
                    return;
                }

                if (!validarForma()) return;
                clean_global_vars();
                showLoading(true);
                $.ajax({
                    url: "CalculoTarifasPorteadores.aspx/CalcularTarifa",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ numViaje: '" + $("#txtNumViaje").val() +
                        "', fechaViaje: '" + $("#hfFechaViaje").val() +
                        "', idProveedorEBS12: " + parseFloat($("#hfIDProveedorOracle").val()) +
                        ", idAlmacenEBS12: " + parseFloat($("#hfIDAlmacenOracle").val()) +
                        ", idMunicipio: " + parseFloat($("#ddlMunicipio").val()) +
                        ", idCiudad: " + parseFloat($("#ddlCiudad").val()) +
                        ", idCodigoTransporte: " + parseFloat($("#ddlCodigoTransporte").val()) +
                        ", esBackhaul: " + chkBackhaul.GetChecked() +
                        ", esCaseta: " + chkCasetas.GetChecked() + " }",
                    timeout: 600000,
                    success: function (data) {
                        showLoading(false);
                        if (!data.d.Error) {
                            $("#hfIDCalculoTarifaPorteador").val(data.d.CalculoTarifa.Id_calculo_tarifa);
                            $("#hfIDTarifa").val(data.d.CalculoTarifa.Id_tarifa);
                            $("#hfIDOrdenCompra").val(data.d.CalculoTarifa.Id_orden_compra);
                            $("#txtOrdenCompraCloud").val(data.d.CalculoTarifa.Folio_orden_compra);
                            $("#btnCalcular").attr("disabled", data.d.CalculoTarifa.Id_orden_compra > 0);
                            $("#btnGenerarOC").attr("disabled", data.d.CalculoTarifa.Id_orden_compra > 0);
                            // falta establecer los valores de los fletes y tarifas
                            $("#ddlEstado").val(data.d.CalculoTarifa.Id_estado).multiselect("refresh");
                            obtenerMunicipios(data.d.CalculoTarifa.Id_estado, data.d.CalculoTarifa.Id_municipio, data.d.CalculoTarifa.Id_ciudad);
                            $("#ddlCodigoTransporte").val(data.d.CalculoTarifa.Id_codigo_transporte).multiselect("refresh");
                            chkBackhaul.SetChecked(data.d.CalculoTarifa.Backhaul);
                            chkCasetas.SetChecked(data.d.CalculoTarifa.Caseta);
                            $("#txtTransporte").val("$" + (data.d.CalculoTarifa.Monto_transporte).toFixed(4).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                            $("#txtCasetas").val("$" + (data.d.CalculoTarifa.Monto_caseta).toFixed(4).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                        } else {
                            $("#txtTransporte").val("");
                            $("#txtCasetas").val("");
                            showMessage(messageType.ERRORBOX, data.d.CalculoTarifa.Mensaje);
                        }
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#btnGenerarOC").click(function () {
                if (parseFloat($("#hfIDOrdenCompra").val()))
                {
                    showMessage(messageType.INFORMATIONBOX, "El viaje ya cuenta con una Orden de Compra en Oracle Cloud.");
                    return;
                } else if (!parseFloat($("#hfIDTarifa").val())) {
                    showMessage(messageType.WARNINGBOX, "No se puede generar una Orden de Compra en Oracle Cloud sin antes haber calculado una tarifa a un proteador.");
                    return;
                }

                showLoading(true);
                $.ajax({
                    url: "CalculoTarifasPorteadores.aspx/CrearOrdenCompraCloud",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{ idCalculoTarifaPorteador: " + (parseFloat($("#hfIDCalculoTarifaPorteador").val()) || 0) +
                        ", idTarifa: " + parseFloat($("#hfIDTarifa").val()) +
                        ", numViaje: '" + $("#txtNumViaje").val() +
                        "', idAlmacenEBS12: " + parseFloat($("#hfIDAlmacenOracle").val()) +
                        ", fechaViaje: '" + $("#hfFechaViaje").val() +
                        "', proveedor: '" + $("#txtProveedor").val() +
                        "', esBackhaul: " + chkBackhaul.GetChecked() +
                        ", esCaseta: " + chkCasetas.GetChecked() +
                        ", totalTransporte: '" + $("#txtTransporte").val().replace("$", "").replace(",", "") +
                        "', totalCasetas: '" + ($("#txtCasetas").val().replace("$", "").replace(",", "") || 0) + "' }",
                    timeout: 600000,
                    success: function (data) {
                        showLoading(false);
                        if (!data.d.Error) {
                            showMessage(messageType.SUCCESSBOX, data.d.Mensaje);
                            $("#btnLimpiar").click();
                        } else {
                            showMessage(messageType.ERRORBOX, data.d.Mensaje);
                        }
                    },
                    error: function (request, status, error) {
                        showLoading(false);
                        showMessage(messageType.ERRORBOX, request.responseText);
                    }
                });
            });

            $("#btnLimpiar").click(function () {
                $("#txtNumViaje").val("");
                $("#hfFechaViaje").val("");
                $("#hfIDOrdenCompra").val("");
                $("#txtOrdenCompraCloud").val("");
                $("#hfIDProveedorOracle").val("");
                $("#txtProveedor").val("");
                $("#hfIDAlmacenOracle").val("");
                $("#txtAlmacenOrigen").val("");
                $("#ddlEstado").multiselect("enable");
                $("#ddlEstado").val("0").multiselect('refresh');
                $("#ddlEstado").change();
                $("#ddlCodigoTransporte").multiselect("enable");
                $("#ddlCodigoTransporte").val("0").multiselect('refresh');
                chkBackhaul.SetEnabled(true);
                chkBackhaul.SetChecked(false);
                chkCasetas.SetEnabled(true);
                chkCasetas.SetChecked(false);
                $("#txtTransporte").val("");
                $("#txtCasetas").val("");
                $("#btnCalcular").attr("disabled", false);
                $("#btnGenerarOC").attr("disabled", false);
            });

            $("#ddlEstado").change(function () {
                obtenerMunicipios(this.value);
            });

            $("#ddlMunicipio").change(function () {
                obtenerCiudades(this.value);
            });
        };

        var messageType = {
            SUCCESSBOX: SUCCESSBOX,
            WARNINGBOX: WARNINGBOX,
            ERRORBOX: ERRORBOX,
            INFORMATIONBOX: INFORMATIONBOX
        };

        Date.prototype.yyyymmdd = function () {
            var mm = this.getMonth() + 1; // getMonth() is zero-based
            var dd = this.getDate();

            return [this.getFullYear(),
                    (mm > 9 ? '' : '0') + mm,
                    (dd > 9 ? '' : '0') + dd
            ].join('/');
        };

        function establecerViaje(viaje) {
            showLoading(true);
            $("#txtNumViaje").val(viaje.FolioViaje);
            $("#hfFechaViaje").val(new Date(viaje.FechaEnvio).yyyymmdd());
            $("#hfIDProveedorOracle").val(viaje.IDProveedorOracle);
            $("#txtProveedor").val(viaje.ProveedorOracle);
            $("#hfIDAlmacenOracle").val(viaje.IDAlmacenOracle);
            $("#txtAlmacenOrigen").val(viaje.NombreAlmacenOracle);
            $.ajax({
                url: "CalculoTarifasPorteadores.aspx/EstablecerCalculoTarifaPorteador",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{ numViaje: '" + viaje.FolioViaje + "' }",
                timeout: 600000,
                success: function (data) {
                    showLoading(false);
                    //console.log(data);
                    if (data.d) {
                        $("#hfIDCalculoTarifaPorteador").val(data.d.Id_calculo_tarifa);
                        $("#hfIDTarifa").val(data.d.Id_tarifa);
                        $("#hfIDOrdenCompra").val(data.d.Id_orden_compra);
                        $("#txtOrdenCompraCloud").val(data.d.Folio_orden_compra);
                        $("#btnCalcular").attr("disabled", data.d.Id_orden_compra > 0);
                        $("#btnGenerarOC").attr("disabled", data.d.Id_orden_compra > 0);
                        // falta establecer los valores de los fletes y tarifas
                        $("#ddlEstado").val(data.d.Id_estado).multiselect("refresh");
                        obtenerMunicipios(data.d.Id_estado, data.d.Id_municipio, data.d.Id_ciudad);
                        $("#ddlCodigoTransporte").val(data.d.Id_codigo_transporte).multiselect("refresh");
                        chkBackhaul.SetChecked(data.d.Backhaul);
                        chkCasetas.SetChecked(data.d.Caseta);
                        $("#txtTransporte").val("$" + (data.d.Monto_transporte).toFixed(4).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                        $("#txtCasetas").val("$" + (data.d.Monto_caseta).toFixed(4).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                    }
                },
                error: function (request, status, error) {
                    showLoading(false);
                    showMessage(messageType.ERRORBOX, request.responseText);
                }
            });
        }

        function fillDropDownList(dropDownList, data, valueField, textField) {
            $(dropDownList + " option").remove();
            if (data.length > 0) {
                $(dropDownList).append($("<option></option>").val(0).html("..:: Seleccione ::.."));
                for (var i = 0; i < data.length; i++)
                    $(dropDownList).append($("<option></option>").val(data[i][valueField]).html(data[i][textField]));
            }
            $(dropDownList).multiselect("rebuild");
        }

        function obtenerCiudades(idMunicipio, idCiudad) {
            showLoading(true);
            $.ajax({
                url: "CalculoTarifasPorteadores.aspx/ObtenerCiudades",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                timeout: 600000,
                data: "{ idMunicipio: " + parseFloat(idMunicipio) + " }",
                success: function (data) {
                    fillDropDownList("#ddlCiudad", data.d, "Id_ciudad", "Nombre_ciudad");
                    if (idCiudad)
                        $("#ddlCiudad").val(idCiudad).multiselect("refresh");

                    showLoading(false);
                },
                error: function (error) {
                    showLoading(false);
                    var exception = JSON.parse(error.responseText);
                    showMessage(messageType.ERRORBOX, exception.Message);
                }
            });
        }

        function obtenerMunicipios(idEstado, idMunicipio, idCiudad) {
            showLoading(true);
            $.ajax({
                url: "CalculoTarifasPorteadores.aspx/ObtenerMunicipios",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                timeout: 600000,
                data: "{ idEstado: " + parseFloat(idEstado) + " }",
                success: function (data) {
                    fillDropDownList("#ddlMunicipio", data.d, "Id_municipio", "Nombre_municipio");
                    fillDropDownList("#ddlCiudad", "");
                    if (idMunicipio) {
                        $("#ddlMunicipio").val(idMunicipio).multiselect("refresh");
                        obtenerCiudades(idMunicipio, idCiudad);
                    } else {
                        showLoading(false);
                    }
                },
                error: function (error) {
                    showLoading(false);
                    var exception = JSON.parse(error.responseText);
                    showMessage(messageType.ERRORBOX, exception.Message);
                }
            });
        }

        function setDate() {
            var date = new Date(deFechaInicio.GetDate());
            date.setDate(date.getDate());
            date.setHours(0, 0, 0, 0);
            deFechaFin.SetMinDate(date);
            if (deFechaInicio.GetDate() > deFechaFin.GetDate())
                deFechaFin.SetDate();
        }

        function showModal(url, title, alto, ancho, masterPage) {
            $(window).MostrarPopup({
                contenedor: null,
                url: url,
                alto: alto,
                ancho: ancho,
                tituloPopup: title,
                masterPage: masterPage ? masterPage : "~/Blank.Master",
                movible: false,
                desaparecer: false,
                FuncionCallback: null
            });
        }

        function showLoading(display) {
            if (display) {
                showUpdateProgressLoading();
            } else {
                hideUpdateProgressLoading();
            }
        }

        function showMessage(messageType, sms, delay) {
            delay = delay || 7000;
            MostrarCajaMensajes(messageType, sms, delay);
        }

        Sys.Application.add_load(Scripts_Load);
        Sys.Application.add_load(attrEvents);
    </script>
</asp:Content>