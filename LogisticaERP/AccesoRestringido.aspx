<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="AccesoRestringido.aspx.cs" Inherits="LogisticaERP.AccesoRestringido" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="sm" runat="server">        
    </asp:ScriptManager> 
    <div style="padding:50px 0px 0px 50px;">
        <h1>Acceso restringido, no tienes los privilegios necesarios para ingresar a esta secci&oacute;n.</h1>
    </div>
</asp:Content>
