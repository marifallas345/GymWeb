<%@ Page Language="C#" AutoEventWireup="true"Async="true"CodeBehind="Inscripciones.aspx.cs"Inherits="GymWEB.Views.Inscripciones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta charset="utf-8" />

    <title>Inscripciones - Gym Control Pro</title>

    <link href="../Content/site.css" rel="stylesheet" />

</head>

<body>

<form id="form1" runat="server">

    <div class="layout">

        <!-- =========================================
             MENÚ
        ========================================== -->

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


        <!-- =========================================
             CONTENIDO
        ========================================== -->

        <div class="content">

            <div class="topbar">

                <div>

                    <h1>
                        Administración de Inscripciones
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


            <!-- =========================================
                 FORMULARIO
            ========================================== -->

            <asp:Panel
                ID="pnlFormulario"
                runat="server"
                CssClass="form-card">

                <h2>
                    Nueva inscripción
                </h2>

                <div class="form-grid">

                    <!-- CLIENTE -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblCliente"
                            runat="server"
                            Text="Cliente">
                        </asp:Label>

                        <asp:DropDownList
                            ID="ddlCliente"
                            runat="server"
                            CssClass="textbox">
                        </asp:DropDownList>

                    </div>


                    <!-- MEMBRESÍA -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblMembresia"
                            runat="server"
                            Text="Membresía">
                        </asp:Label>

                        <asp:DropDownList
                            ID="ddlMembresia"
                            runat="server"
                            CssClass="textbox">
                        </asp:DropDownList>

                    </div>


                    <!-- FECHA INICIO -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblFechaInicio"
                            runat="server"
                            Text="Fecha de inicio">
                        </asp:Label>

                        <asp:TextBox
                            ID="txtFechaInicio"
                            runat="server"
                            CssClass="textbox"
                            TextMode="Date">
                        </asp:TextBox>

                    </div>


                    <!-- FECHA VENCIMIENTO -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblFechaVencimiento"
                            runat="server"
                            Text="Fecha de vencimiento">
                        </asp:Label>

                        <asp:TextBox
                            ID="txtFechaVencimiento"
                            runat="server"
                            CssClass="textbox"
                            TextMode="Date">
                        </asp:TextBox>

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


            <!-- =========================================
                 LISTADO
            ========================================== -->

            <asp:Panel
                ID="pnlListado"
                runat="server"
                CssClass="form-card">

                <h2>Inscripciones registradas
                </h2>

                <asp:GridView
                    ID="gvInscripciones"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="grid"
                    EmptyDataText="No hay inscripciones registradas."
                    DataKeyNames="Id"
                    OnRowCommand="gvInscripciones_RowCommand">

                    <Columns>

                        <asp:BoundField
                            DataField="Id"
                            HeaderText="ID" />

                        <asp:BoundField
                            DataField="ClienteId"
                            HeaderText="Cliente" />

                        <asp:BoundField
                            DataField="MembresiaId"
                            HeaderText="Membresía" />

                        <asp:BoundField
                            DataField="FechaInicio"
                            HeaderText="Fecha inicio"
                            DataFormatString="{0:dd/MM/yyyy}" />

                        <asp:BoundField
                            DataField="FechaVencimiento"
                            HeaderText="Fecha vencimiento"
                            DataFormatString="{0:dd/MM/yyyy}" />

                        <asp:CheckBoxField
                            DataField="Estado"
                            HeaderText="Activo" />

                        <asp:ButtonField
                            ButtonType="Button"
                            CommandName="EditarInscripcion"
                            Text="Editar"
                            HeaderText="Acciones" />

                        <asp:ButtonField
                            ButtonType="Button"
                            CommandName="EliminarInscripcion"
                            Text="Eliminar" />

                    </Columns>

                </asp:GridView>

            </asp:Panel>

        </div>

    </div>

</form>

</body>

</html>