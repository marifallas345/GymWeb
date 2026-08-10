<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Dashboard.aspx.cs" Inherits="GymWEB.Views.Dashboard" %>

<!DOCTYPE html>

<html>
<head runat="server">

    <meta charset="utf-8" />

    <title>Dashboard - Gym Control Pro</title>

    <link href="../Content/site.css" rel="stylesheet" />

</head>

<body>

    <form id="form1" runat="server">

        <div class="layout">

            <!-- SIDEBAR -->

            <asp:Panel
                ID="pnlSidebar"
                runat="server"
                CssClass="sidebar">

                <h2 class="logo">
                    GYM CONTROL PRO
                </h2>

                <asp:Button
                    ID="btnDashboard"
                    runat="server"
                    Text="Dashboard"
                    CssClass="menu-button"
                    OnClick="btnDashboard_Click" />

                <asp:Button
                    ID="btnClientes"
                    runat="server"
                    Text="Clientes"
                    CssClass="menu-button"
                    OnClick="btnClientes_Click" />

                <asp:Button
                    ID="btnMembresias"
                    runat="server"
                    Text="Membresías"
                    CssClass="menu-button"
                    OnClick="btnMembresias_Click" />

                <asp:Button
                    ID="btnInscripciones"
                    runat="server"
                    Text="Inscripciones"
                    CssClass="menu-button"
                    OnClick="btnInscripciones_Click" />

                <asp:Button
                    ID="btnUsuarios"
                    runat="server"
                    Text="Usuarios"
                    CssClass="menu-button"
                    OnClick="btnUsuarios_Click" />

                <div class="menu-bottom">

                    <asp:Button
                        ID="btnCerrarSesion"
                        runat="server"
                        Text="Cerrar sesión"
                        CssClass="btn-danger"
                        OnClick="btnCerrarSesion_Click" />

                </div>

            </asp:Panel>


            <!-- CONTENIDO -->

            <div class="content">

                <div class="topbar">

                    <h1>
                        Dashboard
                    </h1>

                    <p class="bienvenida">

                        Bienvenido,

                        <strong>

                            <asp:Label
                                ID="lblUsuario"
                                runat="server">
                            </asp:Label>

                        </strong>

                    </p>

                </div>


                <!-- TARJETAS -->

                <div class="cards">

                    <asp:Panel
                        ID="pnlClientes"
                        runat="server"
                        CssClass="card">

                        <h3>
                            Clientes
                        </h3>

                        <asp:Label
                            ID="lblClientes"
                            runat="server"
                            Text="0"
                            CssClass="numero">
                        </asp:Label>

                    </asp:Panel>


                    <asp:Panel
                        ID="pnlMembresias"
                        runat="server"
                        CssClass="card">

                        <h3>
                            Membresías
                        </h3>

                        <asp:Label
                            ID="lblMembresias"
                            runat="server"
                            Text="0"
                            CssClass="numero">
                        </asp:Label>

                    </asp:Panel>


                    <asp:Panel
                        ID="pnlInscripciones"
                        runat="server"
                        CssClass="card">

                        <h3>
                            Inscripciones
                        </h3>

                        <asp:Label
                            ID="lblInscripciones"
                            runat="server"
                            Text="0"
                            CssClass="numero">
                        </asp:Label>

                    </asp:Panel>


                    <asp:Panel
                        ID="pnlUsuarios"
                        runat="server"
                        CssClass="card">

                        <h3>
                            Usuarios
                        </h3>

                        <asp:Label
                            ID="lblUsuarios"
                            runat="server"
                            Text="0"
                            CssClass="numero">
                        </asp:Label>

                    </asp:Panel>

                </div>


                <!-- RESUMEN -->

                <div class="form-card">

                    <h2>
                        Resumen del sistema
                    </h2>

                    <p class="bienvenida">

                        Desde este panel puedes administrar los clientes,
                        membresías, inscripciones y usuarios de Gym Control Pro.

                    </p>

                </div>

            </div>

        </div>

    </form>

</body>
</html>