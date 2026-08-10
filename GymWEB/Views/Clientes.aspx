<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Clientes.aspx.cs" Inherits="GymWEB.Views.Clientes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta charset="utf-8" />

    <title>Clientes - Gym Control Pro</title>

    <link href="../Content/site.css" rel="stylesheet" />

</head>

<body>

<form id="form1" runat="server">

    <div class="layout">

        <!-- ==============================
             MENÚ LATERAL
        =============================== -->

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


        <!-- ==============================
             CONTENIDO
        =============================== -->

        <div class="content">

            <div class="topbar">

                <div>

                    <h1>Administración de Clientes</h1>

                    <p class="bienvenida">
                        Usuario:
                        <asp:Label
                            ID="lblUsuario"
                            runat="server">
                        </asp:Label>
                    </p>

                </div>

            </div>


            <!-- ==============================
                 FORMULARIO
            =============================== -->

            <asp:Panel
                ID="pnlFormulario"
                runat="server"
                CssClass="form-card">

                <h2>Datos del cliente</h2>

                <div class="form-grid">

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


                    <div class="form-group">

                        <asp:Label
                            ID="lblCedula"
                            runat="server"
                            Text="Cédula">
                        </asp:Label>

                        <asp:TextBox
                            ID="txtCedula"
                            runat="server"
                            CssClass="textbox">
                        </asp:TextBox>

                    </div>


                    <div class="form-group">

                        <asp:Label
                            ID="lblTelefono"
                            runat="server"
                            Text="Teléfono">
                        </asp:Label>

                        <asp:TextBox
                            ID="txtTelefono"
                            runat="server"
                            CssClass="textbox">
                        </asp:TextBox>

                    </div>


                    <div class="form-group">

                        <asp:Label
                            ID="lblEmail"
                            runat="server"
                            Text="Correo electrónico">
                        </asp:Label>

                        <asp:TextBox
                            ID="txtEmail"
                            runat="server"
                            CssClass="textbox"
                            TextMode="Email">
                        </asp:TextBox>

                    </div>


                    <div class="form-group">

                        <asp:Label
                            ID="lblFechaNacimiento"
                            runat="server"
                            Text="Fecha de nacimiento">
                        </asp:Label>

                        <asp:TextBox
                            ID="txtFechaNacimiento"
                            runat="server"
                            CssClass="textbox"
                            TextMode="Date">
                        </asp:TextBox>

                    </div>


                    <div class="form-group">

                        <asp:Label
                            ID="lblSexo"
                            runat="server"
                            Text="Sexo">
                        </asp:Label>

                        <asp:DropDownList
                            ID="ddlSexo"
                            runat="server"
                            CssClass="textbox">

                            <asp:ListItem
                                Text="Seleccione..."
                                Value="">
                            </asp:ListItem>

                            <asp:ListItem
                                Text="Masculino"
                                Value="M">
                            </asp:ListItem>

    <asp:ListItem
        Text="Femenino"
        Value="F">
                            </asp:ListItem>

    </asp:DropDownList>

    </div>

    </div>


    <asp:Label
        ID="lblMensaje"
        runat="server"
        CssClass="mensaje">
                </asp:Label>


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


    <!-- ==============================
                 LISTADO
            =============================== -->

            <asp:Panel
                ID="pnlListado"
                runat="server"
                CssClass="form-card">

                <h2>Clientes registrados</h2>

                <asp:GridView
                    ID="gvClientes"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="grid"
                    EmptyDataText="No hay clientes registrados."
                    DataKeyNames="Id"
                    OnRowCommand="gvClientes_RowCommand">

                    <Columns>

        <asp:BoundField
            DataField="Id"
            HeaderText="ID" />

        <asp:BoundField
            DataField="Nombre"
            HeaderText="Nombre" />

        <asp:BoundField
            DataField="Cedula"
            HeaderText="Cédula" />

        <asp:BoundField
            DataField="Telefono"
            HeaderText="Teléfono" />

        <asp:BoundField
            DataField="Email"
            HeaderText="Correo" />

        <asp:BoundField
            DataField="Sexo"
            HeaderText="Sexo" />

        <asp:CheckBoxField
            DataField="Estado"
            HeaderText="Activo" />

        <asp:ButtonField
            ButtonType="Button"
            CommandName="EditarCliente"
            Text="Editar" />

        <asp:ButtonField
            ButtonType="Button"
            CommandName="EliminarCliente"
            Text="Eliminar"
            HeaderText="Acciones" />

    </Columns>

    </asp:GridView>

    </asp:Panel>

    </div>

    </div>

    </form>

</body>

</html>