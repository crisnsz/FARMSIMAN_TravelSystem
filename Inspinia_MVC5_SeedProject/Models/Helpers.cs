using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ERP_GMEDINA.Models
{
    public class Helpers
    {

        readonly FARSIMANEntities db = new FARSIMANEntities();
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        //Cerrar sesion
        public void FCerrarSesion()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D);
            HttpContext.Current.Response.Expires = -1500;
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            AuthenticationManager.SignOut();
            HttpContext.Current.Session["UserNombreUsuario"] = null;
            HttpContext.Current.Session["UserNombresApellidos"] = null;
            HttpContext.Current.Session["UserLogin"] = null;
            HttpContext.Current.Session["UserLoginIsAdmin"] = null;
            HttpContext.Current.Session["UserLoginSesion"] = null;
            HttpContext.Current.Session["UserLoginRols"] = null;
            HttpContext.Current.Session["UserRol"] = null;
            HttpContext.Current.Session["UserRolEstado"] = null;
            HttpContext.Current.Session["UserState"] = null;
        }

        public void ValidateUser(string sPantalla, out bool UserState, out bool IsAdmin, out int UserPosition, out bool AccessPantalla)
        {
            UserState = Convert.ToBoolean(HttpContext.Current.Session["UserState"]);
            IsAdmin = Convert.ToBoolean(HttpContext.Current.Session["UserLoginIsAdmin"]);
            UserPosition = Convert.ToInt32(HttpContext.Current.Session["UserPosition"]);
            AccessPantalla = GetUserAccessPosition(sPantalla);


        }


        public bool GetUserAccessPosition(string sPantalla)
        {
            bool Retorno = false;
            int UserPositionID = Convert.ToInt32(HttpContext.Current.Session["UserPosition"]);
            if (!Convert.ToBoolean(HttpContext.Current.Session["UserLoginIsAdmin"]))
            {


                var listPositionAccess = (from position in db.tbPositions
                                          join access in db.tbAccesses on position.position_ID equals access.position_ID
                                          join obj in db.tbObjects on access.object_ID equals obj.object_ID
                                          where position.position_ID == UserPositionID
                                          select new
                                          {
                                              PositionName = position.position_Name,
                                              ObjectName = obj.object_Name,
                                              ObjectReference = obj.object_Reference
                                          }).ToList();



                var BuscarList = listPositionAccess.Where(x => x.ObjectReference == sPantalla);
                int Conteo = BuscarList.Count();

                if (Conteo > 0)
                    Retorno = true;
            }
            else
                Retorno = true;

            return Retorno;
        }

        public bool GetUserLogin()
        {
            bool Estado = false;

            var isLogged = HttpContext.Current.Session["UserLogin"];
            if (isLogged == null)
            {
                return Estado;
            }

            int user = (int)HttpContext.Current.Session["UserLogin"];
            if (user != 0)
                Estado = true;

            return Estado;
        }


        public List<tbUser> getUserInformation()
        {
            int user = 0;
            List<tbUser> UsuarioList = new List<tbUser>();

            user = (int)HttpContext.Current.Session["UserLogin"];
            if (user != 0)
            {
                UsuarioList = db.tbUsers.Where(s => s.user_ID == user).ToList();
            }

            return UsuarioList;
        }

        public int GetUser()
        {
            int user = (int)HttpContext.Current.Session["UserLogin"];

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime DatetimeNow()
        {
            DateTime dt = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(-6)).DateTime;
            return dt;
        }

        public byte[] ComputeSHA512Hash(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                return sha512.ComputeHash(inputBytes);
            }
        }
    }
}