<%@ Page Language="C#" AutoEventWireup="true" Async="true"CodeBehind="Usuarios.aspx.cs" Inherits="GymWEB.Views.Usuarios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta charset="utf-8" />

    <title>Usuarios - Gym Control Pro</title>

    <link href="../Content/site.css" rel="stylesheet" />

</head>

<body>

<form id="form1" runat="server">

    <div class="layout">

        <!-- MENÚ LATERAL -->

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

                <div>

                    <h1>
                        Administración de Usuarios
                    </h1>

                    <p class="bienvenida">

                        Usuario:

                        <asp:Label
                            ID="lblUsuario"
                            runat="server">
                        </asp:Label>

                    </p>

                </div>

            </div>


            <!-- FORMULARIO -->

            <asp:Panel
                ID="pnlFormulario"
                runat="server"
                CssClass="form-card">

                <h2>
                    Datos del usuario
                </h2>

                <div class="form-grid">

                    <!-- NOMBRE -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblNombre"
                            runat="server"
                            Text="Nombre">
                        </asp:Label>

                        <asp:TextBox
                            ID="txtNombre"
                            runat="server"
                            CssClass="textbox">
                        </asp:TextBox>

                    </div>


                    <!-- USUARIO -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblUsuarioLogin"
                            runat="server"
                            Text="Usuario">
                        </asp:Label>

                        <asp:TextBox
                            ID="txtUsuarioLogin"
                            runat="server"
                            CssClass="textbox">
                        </asp:TextBox>

                    </div>


                    <!-- CONTRASEÑA -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblContrasena"
                            runat="server"
                            Text="Contraseña">
                        </asp:Label>

                        <asp:TextBox
                            ID="txtContrasena"
                            runat="server"
                            CssClass="textbox"
                            TextMode="Password">
                        </asp:TextBox>

                    </div>


                    <!-- ROL -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblRol"
                            runat="server"
                            Text="Rol">
                        </asp:Label>

                        <asp:DropDownList
                            ID="ddlRol"
                            runat="server"
                            CssClass="textbox">

                            <asp:ListItem
                                Text="Administrador"
                                Value="Administrador" />

                            <asp:ListItem
                                Text="Empleado"
                                Value="Empleado" />

                        </asp:DropDownList>

                    </div>


                    <!-- ESTADO -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblEstado"
                            runat="server"
                            Text="Estado">
                        </asp:Label>

                        <asp:CheckBox
                            ID="chkEstado"
                            runat="server"
                            Text=" Usuario activo"
                            Checked="true" />

                    </div>

                </div>


                <!-- MENSAJE -->

                <asp:Label
                    ID="lblMensaje"
                    runat="server"
                    CssClass="mensaje">
                </asp:Label>


                <!-- BOTONES -->

                <div class="form-buttons">

                    <asp:Button
                        ID="btnGuardar"
                        runat="server"
                        Text="Guardar"
                        CssClass="btn"
                        OnClick="btnGuardar_Click" />

                    <asp:Button
                        ID="btnActualizar"
                        runat="server"
                        Text="Actualizar"
                        CssClass="btn"
                        Visible="false"
                        OnClick="btnActualizar_Click" />

                    <asp:Button
                        ID="btnLimpiar"
                        runat="server"
                        Text="Limpiar"
                        CssClass="btn-secondary"
                        OnClick="btnLimpiar_Click" />

                </div>

            </asp:Panel>


            <!-- LISTADO -->

            <asp:Panel
                ID="pnlListado"
                runat="server"
                CssClass="form-card">

                <h2>
                    Usuarios registrados
                </h2>

                <asp:GridView
                    ID="gvUsuarios"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="grid"
                    EmptyDataText="No hay usuarios registrados."
                    DataKeyNames="Id"
                    OnRowCommand="gvUsuarios_RowCommand">

                    <Columns>

                        <asp:BoundField
                            DataField="Id"
                            HeaderText="ID" />

                        <asp:BoundField
                            DataField="Nombre"
                            HeaderText="Nombre" />

                        <asp:BoundField
                            DataField="UsuarioLogin"
                            HeaderText="Usuario" />

                        <asp:BoundField
                            DataField="Rol"
                            HeaderText="Rol" />

                        <asp:CheckBoxField
                            DataField="Estado"
                            HeaderText="Activo" />

                        <asp:ButtonField
                            ButtonType="Button"
                            CommandName="EditarUsuario"
                            Text="Editar"
                            HeaderText="Acciones" />

                        <asp:ButtonField
                            ButtonType="Button"
                            CommandName="EliminarUsuario"
                            Text="Eliminar" />

                    </Columns>

                </asp:GridView>

            </asp:Panel>

        </div>

    </div>

</form>

</body>

</html>