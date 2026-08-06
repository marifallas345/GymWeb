<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="GymWEB.Views.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />

    <title>Gym Control Pro</title>

    <link href="../Content/site.css" rel="stylesheet" />
</head>

<body>

    <form id="form1" runat="server">

        <div class="login-container">

            <div class="login-card">

                <h1>GYM CONTROL PRO</h1>

                <p class="subtitulo">
                    Sistema de Administración
                </p>

                <div class="form-group">

                    <asp:Label
                        ID="lblUsuario"
                        runat="server"
                        Text="Usuario">
                    </asp:Label>

                    <asp:TextBox
                        ID="txtUsuario"
                        runat="server"
                        CssClass="textbox">
                    </asp:TextBox>

                </div>

                <div class="form-group">

                    <asp:Label
                        ID="lblPassword"
                        runat="server"
                        Text="Contraseña">
                    </asp:Label>

                    <asp:TextBox
                        ID="txtPassword"
                        runat="server"
                        CssClass="textbox"
                        TextMode="Password">
                    </asp:TextBox>

                </div>

                <asp:Label
                    ID="lblMensaje"
                    runat="server"
                    CssClass="mensaje">
                </asp:Label>

                <asp:Button
                    ID="btnLogin"
                    runat="server"
                    Text="Iniciar Sesión"
                    CssClass="btn"
                    CausesValidation="false"
                    UseSubmitBehavior="false"
                    OnClick="btnLogin_Click" />

            </div>

        </div>

    </form>

</body>
</html>