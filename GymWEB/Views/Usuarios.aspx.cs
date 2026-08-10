using GymWEB.Helpers;
using GymWEB.Models;
using GymWEB.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GymWEB.Views
{
    public partial class Usuarios : BasePage
    {
        private readonly UsuarioService _usuarioService =
            new UsuarioService();

        // =========================================================
        // PAGE LOAD
        // =========================================================

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUsuario.Text = UsuarioActual;

                Page.RegisterAsyncTask(
                    new PageAsyncTask(
                        CargarUsuariosAsync));
            }
        }

        // =========================================================
        // LISTAR USUARIOS
        // =========================================================

        private async Task CargarUsuariosAsync()
        {
            try
            {
                List<Usuario> usuarios =
                    await _usuarioService
                        .ObtenerTodosAsync();

                gvUsuarios.DataSource =
                    usuarios;

                gvUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "No fue posible cargar los usuarios: "
                    + ex.Message;
            }
        }

        // =========================================================
        // GUARDAR
        // =========================================================

        protected void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            Page.RegisterAsyncTask(
                new PageAsyncTask(
                    GuardarUsuarioAsync));
        }

        private async Task GuardarUsuarioAsync()
        {
            try
            {
                if (!ValidarFormulario(false))
                    return;

                Usuario usuario =
                    new Usuario
                    {
                        Nombre =
                            txtNombre.Text.Trim(),

                        UsuarioLogin =
                            txtUsuarioLogin.Text.Trim(),

                        Contrasena =
                            txtContrasena.Text,

                        Rol =
                            ddlRol.SelectedValue,

                        Estado =
                            chkEstado.Checked
                    };

                bool resultado =
                    await _usuarioService
                        .AgregarAsync(usuario);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Usuario registrado correctamente.";

                    LimpiarFormulario();

                    await CargarUsuariosAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible registrar el usuario.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al registrar el usuario: "
                    + ex.Message;
            }
        }

        // =========================================================
        // EDITAR / ELIMINAR
        // =========================================================

        protected void gvUsuarios_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            int indiceFila;

            if (!int.TryParse(
                e.CommandArgument?.ToString(),
                out indiceFila))
            {
                lblMensaje.Text =
                    "No fue posible identificar el usuario.";

                return;
            }

            if (indiceFila < 0 ||
                indiceFila >= gvUsuarios.Rows.Count)
            {
                lblMensaje.Text =
                    "La fila seleccionada no es válida.";

                return;
            }

            int id =
                Convert.ToInt32(
                    gvUsuarios
                        .DataKeys[indiceFila]
                        .Value);

            if (e.CommandName == "EditarUsuario")
            {
                Page.RegisterAsyncTask(
                    new PageAsyncTask(
                        () => CargarUsuarioParaEditarAsync(id)));
            }
            else if (e.CommandName == "EliminarUsuario")
            {
                Page.RegisterAsyncTask(
                    new PageAsyncTask(
                        () => EliminarUsuarioAsync(id)));
            }
        }

        // =========================================================
        // CARGAR USUARIO PARA EDITAR
        // =========================================================

        private async Task CargarUsuarioParaEditarAsync(int id)
        {
            try
            {
                Usuario usuario =
                    await _usuarioService.ObtenerPorIdAsync(id);

                if (usuario == null)
                {
                    lblMensaje.Text =
                        "No se encontró el usuario.";

                    return;
                }

                txtNombre.Text =
                    usuario.Nombre;

                txtUsuarioLogin.Text =
                    usuario.UsuarioLogin;

                ddlRol.SelectedValue =
                    usuario.Rol;

                chkEstado.Checked =
                    usuario.Estado;

                txtContrasena.Text = "";

                ViewState["UsuarioId"] =
                    usuario.Id;

                btnGuardar.Visible = false;
                btnActualizar.Visible = true;

                lblMensaje.Text = "";
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "No fue posible cargar el usuario: "
                    + ex.Message;
            }
        }
        // =========================================================
        // ACTUALIZAR
        // =========================================================

        protected void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            Page.RegisterAsyncTask(
                new PageAsyncTask(
                    ActualizarUsuarioAsync));
        }

        private async Task ActualizarUsuarioAsync()
        {
            try
            {
                if (!ValidarFormulario(true))
                    return;

                if (ViewState["UsuarioId"] == null)
                {
                    lblMensaje.Text =
                        "No se seleccionó ningún usuario.";

                    return;
                }

                int id =
                    Convert.ToInt32(
                        ViewState["UsuarioId"]);

                Usuario usuario =
                    await _usuarioService
                        .ObtenerPorIdAsync(id);

                if (usuario == null)
                {
                    lblMensaje.Text =
                        "No se encontró el usuario.";

                    return;
                }

                usuario.Nombre =
                    txtNombre.Text.Trim();

                usuario.UsuarioLogin =
                    txtUsuarioLogin.Text.Trim();

                usuario.Rol =
                    ddlRol.SelectedValue;

                usuario.Estado =
                    chkEstado.Checked;

                /*
                 * Solo enviamos una nueva contraseña
                 * si el administrador escribió una.
                 *
                 * Si queda vacía, conservamos la actual.
                 */
                if (!string.IsNullOrWhiteSpace(
                    txtContrasena.Text))
                {
                    usuario.Contrasena =
                        txtContrasena.Text;
                }

                bool resultado =
                    await _usuarioService
                        .ActualizarAsync(usuario);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Usuario actualizado correctamente.";

                    LimpiarFormulario();

                    SalirModoEdicion();

                    await CargarUsuariosAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible actualizar el usuario.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al actualizar el usuario: "
                    + ex.Message;
            }
        }

        // =========================================================
        // ELIMINAR
        // =========================================================

        private async Task EliminarUsuarioAsync(int id)
        {
            try
            {
                bool resultado =
                    await _usuarioService
                        .EliminarAsync(id);

                if (resultado)
                {
                    lblMensaje.Text =
                        "Usuario eliminado correctamente.";

                    if (ViewState["UsuarioId"] != null &&
                        Convert.ToInt32(
                            ViewState["UsuarioId"]) == id)
                    {
                        LimpiarFormulario();

                        SalirModoEdicion();
                    }

                    await CargarUsuariosAsync();
                }
                else
                {
                    lblMensaje.Text =
                        "No fue posible eliminar el usuario.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "Error al eliminar el usuario: "
                    + ex.Message;
            }
        }

        // =========================================================
        // VALIDACIÓN
        // =========================================================

        private bool ValidarFormulario(
            bool modoEdicion)
        {
            if (string.IsNullOrWhiteSpace(
                txtNombre.Text))
            {
                lblMensaje.Text =
                    "Ingrese el nombre del usuario.";

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                txtUsuarioLogin.Text))
            {
                lblMensaje.Text =
                    "Ingrese el nombre de usuario.";

                return false;
            }

            /*
             * Al crear es obligatoria.
             *
             * Al editar puede quedar vacía porque
             * significa que no se desea cambiar.
             */
            if (!modoEdicion &&
                string.IsNullOrWhiteSpace(
                    txtContrasena.Text))
            {
                lblMensaje.Text =
                    "Ingrese una contraseña.";

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                ddlRol.SelectedValue))
            {
                lblMensaje.Text =
                    "Seleccione un rol.";

                return false;
            }

            return true;
        }

        // =========================================================
        // LIMPIAR
        // =========================================================

        protected void btnLimpiar_Click(
            object sender,
            EventArgs e)
        {
            LimpiarFormulario();

            SalirModoEdicion();

            lblMensaje.Text = "";
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = "";

            txtUsuarioLogin.Text = "";

            txtContrasena.Text = "";

            if (ddlRol.Items.Count > 0)
            {
                ddlRol.SelectedIndex = 0;
            }

            chkEstado.Checked = true;
        }

        // =========================================================
        // SALIR DEL MODO EDICIÓN
        // =========================================================

        private void SalirModoEdicion()
        {
            ViewState["UsuarioId"] = null;

            btnGuardar.Visible =
                true;

            btnActualizar.Visible =
                false;
        }

        // =========================================================
        // NAVEGACIÓN
        // =========================================================

        protected void btnDashboard_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "~/Views/Dashboard.aspx");
        }

        protected void btnClientes_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "~/Views/Clientes.aspx");
        }

        protected void btnMembresias_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "~/Views/Membresias.aspx");
        }

        protected void btnInscripciones_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "~/Views/Inscripciones.aspx");
        }

        protected void btnUsuarios_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "~/Views/Usuarios.aspx");
        }

        // =========================================================
        // CERRAR SESIÓN
        // =========================================================

        protected void btnCerrarSesion_Click(
            object sender,
            EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect(
                "~/Views/Login.aspx",
                false);

            Context.ApplicationInstance
                .CompleteRequest();
        }
    }
}