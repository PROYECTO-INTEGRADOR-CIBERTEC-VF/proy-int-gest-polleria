using System;
using System.Web.Mvc;
using gest_polleria_front.Models;

namespace gest_polleria_front.Controllers
{
    public abstract class AppController : Controller
    {
        protected string CurrentUserName
        {
            get { return Session["AuthUser"] as string ?? string.Empty; }
        }

        protected string CurrentUserRole
        {
            get { return Session["AuthRole"] as string ?? string.Empty; }
        }

        protected int? CurrentUserId
        {
            get
            {
                var raw = Session["AuthUserId"];
                if (raw == null)
                {
                    return null;
                }

                int id;
                return int.TryParse(raw.ToString(), out id) ? (int?)id : null;
            }
        }

        protected string CurrentUserDisplayName
        {
            get { return Session["AuthNombre"] as string ?? CurrentUserName; }
        }

        protected bool IsAuthenticated
        {
            get { return !string.IsNullOrWhiteSpace(CurrentUserName); }
        }

        protected bool IsAdmin
        {
            get { return string.Equals(CurrentUserRole, "Administrador", StringComparison.OrdinalIgnoreCase); }
        }

        protected bool IsMesero
        {
            get { return string.Equals(CurrentUserRole, "MESERO", StringComparison.OrdinalIgnoreCase); }
        }

        protected void SignIn(LoginApiResponse response, string fallbackUser)
        {
            var userName = string.IsNullOrWhiteSpace(response.UserName) ? fallbackUser : response.UserName;
            var role = string.IsNullOrWhiteSpace(response.Rol) ? "Usuario" : response.Rol;
            var displayName = string.IsNullOrWhiteSpace(response.NombreCompleto) ? userName : response.NombreCompleto;

            Session["AuthUser"] = userName;
            Session["AuthRole"] = role;
            Session["AuthNombre"] = displayName;
            Session["AuthUserId"] = response.IdUsuario;
        }

        protected void SignOutUser()
        {
            Session.Clear();
            Session.Abandon();
        }

        protected void FlashSuccess(string message)
        {
            SetFlash("success", message);
        }

        protected void FlashError(string message)
        {
            SetFlash("error", message);
        }

        private void SetFlash(string type, string message)
        {
            TempData["FlashType"] = type;
            TempData["FlashMessage"] = message;
        }
    }
}
