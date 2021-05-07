<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="LogisticaERP.Inicio" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <input type="hidden" value="Inicio" id="hPage" />
    <asp:HiddenField runat="server" ID="hResultado" Value="" />
    <script type="text/javascript">
        Sys.Application.add_load(Scripts_Load);
    </script>
    <!-- Main content -->
    <section class="custom-content">
        <div class="bs-component">
            <div class="clearfix">
                <div class="row">
                    <h1 style="margin-left: 20px; margin-right: 20px;">Bienvenid@ a Modulo Logistica.
                    </h1>
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->
</asp:Content>
