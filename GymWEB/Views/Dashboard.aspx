<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="GymWEB.Views.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta charset="utf-8"/>

    <title>Dashboard</title>

    <link href="../Content/site.css" rel="stylesheet"/>

</head>

<body>

<form id="form1" runat="server">

<div class="layout">

    <!-- MENU -->

    <aside class="sidebar">

        <h2>GYM</h2>

        <asp:HyperLink runat="server"
            NavigateUrl="Dashboard.aspx"
            CssClass="menu-item"
            Text="🏠 Dashboard" />

        <asp:HyperLink runat="server"
            NavigateUrl="Clientes.aspx"
            CssClass="menu-item"
            Text="👥 Clientes" />

        <asp:HyperLink runat="server"
            NavigateUrl="Membresias.aspx"
            CssClass="menu-item"
            Text="📋 Membresías" />

        <asp:HyperLink runat="server"
            NavigateUrl="Inscripciones.aspx"
            CssClass="menu-item"
            Text="📝 Inscripciones" />

        <asp:HyperLink runat="server"
            NavigateUrl="Usuarios.aspx"
            CssClass="menu-item"
            Text="👤 Usuarios" />

    </aside>

    <!-- CONTENIDO -->

    <main class="content">

        <div class="topbar">

            <h1>Dashboard</h1>

            <asp:Button
                ID="btnSalir"
                runat="server"
                CssClass="btn-danger"
                Text="Cerrar Sesión"
                OnClick="btnSalir_Click"/>

        </div>

        <h2 class="bienvenida">

            Bienvenido,
            <asp:Label
                ID="lblUsuario"
                runat="server"/>

        </h2>

        <div class="cards">

            <div class="card">

                <h3>Clientes</h3>

                <asp:Label
                    ID="lblClientes"
                    runat="server"
                    Text="0"
                    CssClass="numero"/>

            </div>

            <div class="card">

                <h3>Membresías</h3>

                <asp:Label
                    ID="lblMembresias"
                    runat="server"
                    Text="0"
                    CssClass="numero"/>

            </div>

            <div class="card">

                <h3>Inscripciones</h3>

                <asp:Label
                    ID="lblInscripciones"
                    runat="server"
                    Text="0"
                    CssClass="numero"/>

            </div>

            <div class="card">

                <h3>Usuarios</h3>

                <asp:Label
                    ID="lblUsuarios"
                    runat="server"
                    Text="0"
                    CssClass="numero"/>

            </div>

        </div>

    </main>

</div>

</form>

</body>

</html>