using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FARSIMAN.Models;
using Microsoft.Owin.Security;


namespace FARSIMAN.Controllers
{
    public class LoginController : Controller
    {
        readonly FARSIMANEntities db = new FARSIMANEntities();

        readonly Helpers Help = new Helpers();

        // GET: Login
        public ActionResult Index()
        {
            Help.FCerrarSesion();
            return View();
        }

        [HttpPost]
        public ActionResult Index(tbUser tbUser, string txtPassword)
        {
            try
            {
                var UserData = db.UDP_Sec_Login(tbUser.user_Username, txtPassword).FirstOrDefault();

                //Paso 1: Validar si el usuario existe.
                if (UserData is null)
                {
                    ModelState.AddModelError("user_Username", "Usuario o Password incorrecto");
                    return View(tbUser);
                }

                if (!UserData.user_IsActive)
                {
                    //Si el usuario no es activo que muestre mensaje y retorne al login una vez mas.
                    ModelState.AddModelError("user_Username", "Usuario inactivo, contacte al Administrador");
                    return View(tbUser);
                }

                Session["UserNombreUsuario"] = UserData.user_Username;
                Session["UserNombresApellidos"] = db.tbEmployees.Find(UserData.employee_ID).employee_Name;
                Session["UserLogin"] = UserData.user_ID;
                Session["UserLoginIsAdmin"] = UserData.user_IsAdmin;
                Session["UserState"] = UserData.user_IsActive;


                //Si el usuario no es admin, recuperar la información del rol y sus accesos
                if (!UserData.user_IsAdmin)
                {

                    var UserPosition = (from user in db.tbUsers
                                        where user.user_ID == UserData.user_ID
                                        join employee in db.tbEmployees
                                          on user.employee_ID equals employee.employee_ID
                                        join position in db.tbPositions
                                          on employee.position_ID equals position.position_ID
                                        select new
                                        {
                                            user.user_Username,
                                            user.user_Password,
                                            employee.employee_Name,
                                            position.position_ID,
                                            position.position_Name
                                        }).FirstOrDefault();



                    if (UserPosition is null)
                    {
                        //Si el usuario no es activo que muestre mensaje y retorne al login una vez mas.
                        ModelState.AddModelError("user_Username", "No se pudo obtener al posicion del usuario, contacte con el administrador");
                        return View(tbUser);
                    }

                    Session["UserPosition"] = UserPosition.position_ID;
                }

                return RedirectToAction("Index", "Employee");
            }
            catch (Exception)
            {
                return View(tbUser);
            }
        }




        public ActionResult CerrarSesion()
        {
            Help.FCerrarSesion();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult SinAcceso()
        {
            //Validar Inicio de Sesión
            if (Help.GetUserLogin())
                return View();
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult NotFound()
        {
            //Validar Inicio de Sesión
            if (Help.GetUserLogin())
                return View();
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SinRol()
        {
            //Validar Inicio de Sesión
            if (Help.GetUserLogin())
                return View();
            else
                return RedirectToAction("Index", "Login");
        }
    }
}