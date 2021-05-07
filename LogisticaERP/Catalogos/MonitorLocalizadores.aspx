<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="MonitorLocalizadores.aspx.cs" Inherits="LogisticaERP.Catalogos.MonitorLocalizadores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .card{
            width: 75px;
            background-color: #fff;
            border: 1px solid rgba(0,0,0,.125);
            border-radius: .25rem;
            box-shadow: 1px 1px 3px #888;
            min-height: 80px;
            margin: 5px;
            text-align: center;
        }
        .card-header:first-child {
            border-radius: calc(.25rem - 1px) calc(.25rem - 1px) 0 0;
        }
        .card-header {
            margin-bottom: 0;
            background-color: #f7f7f9;
            border-bottom: 1px solid rgba(0,0,0,.125);
        }
        h1{
            font-weight:bold;
            margin:5px;
            font-size:30px;
        }
        #mainbox{
            box-sizing: border-box;
            justify-content: center;
            display: flex;
            flex-wrap: wrap;
            overflow:auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" EnablePageMethods="true" runat="server" />
    <!--Panel de controles -->
    <div class="btn-container">
        <input runat="server" type="submit" id="btnRefresh" style="margin-left: 5px;" class="btn custom-btn btn-primary pull-right" value="Actualizar" />
    </div>
    <section class="custom-content" style="padding:42px 0 0 0;">
        <div runat="server" id="mainbox"></div>
    </section>
    <script type="text/javascript">
        setInterval(function () { $("#btnRefresh").click(); }, 600000);
    </script>
</asp:Content>