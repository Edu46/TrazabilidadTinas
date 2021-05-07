<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Busqueda.aspx.cs" Inherits="GRUPOPINSA.Controles.Busqueda.Busqueda" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <meta content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' name='viewport' />
    <script type="text/javascript">
        var cdnPath = '<%=ConfigurationManager.AppSettings["cdnPath"] %>';
    </script>
    <!-- bootstrap 3.0.2 -->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <!-- font Awesome -->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <!-- Ionicons -->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/ionicons.min.css" rel="stylesheet" type="text/css" />
    <!-- AdminLTE style -->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/AdminLTE.css" rel="stylesheet" type="text/css" />
    <!-- Site style -->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/Site.css" rel="stylesheet" type="text/css" />
    <!-- Animation-Site style -->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/animation-site.css" rel="stylesheet" type="text/css" />
    <!-- Jquery UI style-->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/jquery-ui-1.11.4.css" rel="stylesheet" type="text/css" />
    <!-- Bootstrap Multiselect Style-->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <!-- Bootstrap Lightbox Style-->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/bootstrap-lightbox.css" rel="stylesheet" type="text/css" />
    <!-- Animate Style-->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/animate.css" rel="stylesheet" type="text/css" />
    <!-- Tooltipster Lightbox Style-->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/tooltipster.css" rel="stylesheet" type="text/css" />
    <!-- Otros Styles -->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/Popupwindow.css" rel="stylesheet" />
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/Mensajes.css" rel="stylesheet" />
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/messages.css" rel="stylesheet" />
    <!-- uploadify plugin -->
    <link href="<%=ConfigurationManager.AppSettings["cdnPath"] %>/css/uploadify/uploadifive.css" rel="Stylesheet" type="text/css" />

    <!-- JS Files -->
    <!-- Jquery -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/jquery-2.1.3.js"></script>
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/jquery-ui-1.11.4.js"></script>
    <!-- Slimscroll -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/plugins/slimscroll/jquery.slimscroll.js" type="text/javascript"></script>
    <!-- Bootstrap -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/bootstrap.min.js" type="text/javascript"></script>
    <!-- Bootstrap Multiselect JS Style-->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/bootstrap-multiselect.js" type="text/javascript"></script>
    <!-- Bootstrap Lightbox JS Style-->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/bootstrap-lightbox.js" type="text/javascript"></script>
    <!-- Lattering plugin -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/jquery.lettering.js" type="text/javascript"></script>
    <!-- Tooltipster plugin -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/jquery.tooltipster.js" type="text/javascript"></script>
    <!-- Textillate plugin -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/jquery.textillate.js" type="text/javascript"></script>
    <!-- bPopup plugin -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/jquery.bpopup.min.js" type="text/javascript"></script>
    <!-- Jquery Hotkeys plugin -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/plugins/jqueryhotkeys/jquery.hotkeys.js" type="text/javascript"></script>
    <!-- Maskjs plugin -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/maskjs.js" type="text/javascript"></script>
    <!-- uploadify plugin -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/plugins/uploadify/jquery.uploadifive.js" type="text/javascript"></script>
    <!-- Otros JS -->
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/animation-site.js"></script>
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/Mensajes.js"></script>
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/messages.js"></script>
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/moment.min.js"></script>
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/Popupwindow.js"></script>
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/MainPageMaster.js"></script>
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/respond.js"></script>
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/Utilerias.js"></script>
    <script src="<%=ConfigurationManager.AppSettings["cdnPath"] %>/js/modernizr.custom.63321.js" type="text/javascript"></script>

    <script type="text/javascript">
        var gridPerformingCallback = false;

        function OnGridDoubleClick(index) {
            var param1 = grdResultados.GetRowKey(index);

            grdResultados.PerformCallback(param1);


            //////var param2 = { Tabla: $("#hfConsulta").val(), Valor: param1 };
            //////var procedimiento = $("#hfProcedimiento").val();

            ////////si se definio una funcion callback
            //////if (procedimiento != "") {
            //////    parent.window[procedimiento](param2);
            //////}
            //////else {  //si no se definio una funcion callback se va por la predeterminada
            //////    parent.CerrarVentana(param2);
            //////}

            //parent.CerrarVentana(param2);
            //var procedimiento = 'parent.' + $("#hfProcedimiento").val() + '(' + param2 + ')';
            //eval(procedimiento);
        }

        function seleccionar_item() {
            var param1 = grdResultados.GetRowKey(grdResultados.GetFocusedRowIndex());

            grdResultados.PerformCallback(param1);
        }

        function FinalizaBusqueda(s, e) {

            var param1 = s.cp_Id;
            var param2 = { Tabla: $("#hfConsulta").val(), Valor: param1, Resultado: s.cp_resultado };
            var procedimiento = $("#hfProcedimiento").val();

            if (procedimiento != "") {
                parent.window[procedimiento](param2);
            }
            else {  //si no se definio una funcion callback se va por la predeterminada
                parent.CerrarVentana(param2);
            }
            setTimeout(function () {
                parent.setLastFocus();
            }, 1000);
        }

        /* Event listener para las busquedas avanzadas en los grids */
        function OnInitGridTools(s, e) {
            ASPxClientUtils.AttachEventToElement(s.GetMainElement(), "click", function (evt) {
                //console.dir(evt);
                if ($('#' + evt.target.id).parents('.dxgvFilterRow_MetropolisBlue').length > 0)
                    return;

                SelectText(evt.srcElement);
            });
        }
    </script>
</head>
<script type="text/javascript">
    var baseUrl = "<%= ResolveUrl("~/") %>";
</script>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hfConsulta" Value="" />
        <asp:HiddenField runat="server" ID="hfProcedimiento" Value="" />
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <script type="text/javascript">
                        Sys.Application.add_load(Scripts_Load);
                    </script>
                    <!-- Grid de devexpress que sirve para renderizar los demas grids y corregir fallos de devexpress -->
                    <div style="display: none;">
                        <dx:ASPxGridView ID="gridRenderFix" runat="server">
                            <ClientSideEvents Init="function(s, e) {
                                                                console.log('Fix render de los grids de devexpress');
	                                                            gridRenderFix.Refresh();
                                                            }" />
                        </dx:ASPxGridView>
                    </div>
                    <!-- Main content -->
                    <section class="custom-content">
                        <div class="bs-component">
                            <div class="clearfix">
                                <div class="row row-bottom">
                                    <div class="col-xs-4" style="padding-left: 0px;">
                                        <div id="token">
                                            <dx:ASPxTokenBox ID="ASPxTokenBox" runat="server" AllowCustomTokens="False" Style="width: 100%;" AutoResizeWithContainer="True" Theme="MetropolisBlue" AllowMouseWheel="True" TextField="Caption" Tokens="" ValueField="Campo">
                                            </dx:ASPxTokenBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-8" style="padding-right: 0px; padding-left: 0px;">
                                        <div id="filtro" class="input-group">
                                            <asp:TextBox runat="server" CssClass="form-control marginoff" ID="txtFiltro" placeholder="Introduzca los datos a buscar"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="btnBuscar" CssClass="btn custom-btn btn-default" Style="border-bottom-right-radius: 4px; border-top-right-radius: 4px;" Text="<span class='fa fa-search'> </span>" OnClick="btnBuscar_Click" />
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">

                                    <div class="col-sm-12 paddingoff">
                                        <!-- /.box-content -->
                                        <div class="box custom-box box-primary" style="margin-bottom: 8px;">
                                            <!-- /.box-header -->
                                            <div class="box-header with-border">
                                                <h3 class="box-title">Busqueda Avanzada</h3>
                                            </div>
                                            <!-- /.box-body -->
                                            <div class="box-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="table-responsive">
                                                            <dx:ASPxGridView 
                                                                ID="grdResultados" 
                                                                ClientInstanceName="grdResultados" 
                                                                runat="server" 
                                                                AutoGenerateColumns="False" 
                                                                EnableRowsCache="False" 
                                                                KeyboardSupport="true" 
                                                                AccessKey="1"
                                                                EnableTheming="True" 
                                                                Theme="MetropolisBlue" 
                                                                CssClass="withPagerInBoxHeader"
                                                                Width="100%" 
                                                                Settings-ShowFooter="False" 
                                                                Settings-VerticalScrollableHeight="220" 
                                                                Settings-VerticalScrollBarMode="Auto"
                                                                SettingsPager-PageSize="10" 
                                                                SettingsPager-NumericButtonCount="5" 
                                                                SettingsPager-AlwaysShowPager="True" 
                                                                OnCustomCallback="grdResultados_CustomCallback">
                                                                <%--<SettingsBehavior AllowSelectByRowClick="false" AllowFocusedRow="true" />--%>
                                                                <ClientSideEvents
                                                                    Init="function(s,e){
                                                        OnInitGridTools(s,e);
                                                        resizeHeightDevControl(window, window, '#grdResultados', grdResultados, 60, 0);
                                                        $(window).resize();
                                                    }"
                                                                    RowDblClick="function(s, e) {
                                                                    OnGridDoubleClick(e.visibleIndex);
                                                                }"
                                                                    EndCallback="function(s, e) {
                                                                    if(s.cp_Id!=null){
                                                                        FinalizaBusqueda(s, e);
                                                                    }
                                                    
                                                                }" />
                                                                <SettingsBehavior AllowFocusedRow="true" />
                                                                <Settings ShowFilterRow="True" ShowFooter="false" VerticalScrollableHeight="230" VerticalScrollBarMode="Auto" />
                                                                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                                                <SettingsPager AlwaysShowPager="True" PageSize="12" NumericButtonCount="5">
                                                                </SettingsPager>
                                                                <Styles>
                                                                    <AlternatingRow Enabled="True" />
                                                                </Styles>
                                                            </dx:ASPxGridView>

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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
    <div id="UpdateProgress" class="overlay" style="display: block; opacity: 1;" role="status" aria-hidden="true">
    </div>
</body>
</html>
