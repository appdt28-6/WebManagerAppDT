using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebManagerAppDT.Models;

namespace WebManagerAppDT.Controllers
{
    public class HomeController : Controller
    {
        private AppDTEntities db = new AppDTEntities();
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model, string returnUrl)
        {

            var role_id = IsValidUser(model.UserName, model.Password);
            var user_id = model.UserName;

            if (ModelState.IsValid && Session["role_id"] != null)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, true);
                //return RedirectToLocal(returnUrl);
                return RedirectToAction(actionName: "Tickets", controllerName: "Tickets", routeValues: new { role_id, user_id });
                //IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else
            {
                // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
                ModelState.AddModelError(key: "", errorMessage: "El nombre de usuario o la contraseña especificados son incorrectos o la conexión no se pudo establecer.\n Verifique conexón/Usario y password");
            }

            //if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))            

            return View(model);
        }
        protected string IsValidUser(string username, string password)
        {
            var role_id = "";

            try
            {

                //Creamos la conexion con la cadena especificada en el Context
                using (db)
                {
                    string pass = GetMD5(password);
                    //Recuperamos los datos del SP
                    var user = db.USUARIOS_PV.Where(u => u.User_Login == username && u.User_Password == pass);
                    //Recorremos el resultado para validar la informacion
                    foreach (var result in user)
                    {
                        if (result.User_nombre != "")
                        {
                            Session["user_id"] = username;
                            //Session["Usua_id"] = result.Usua_Id;
                            Session["role_id"] = result.User_nombre;
                            //Session["comp_identifier"] = result.Usua_Id.ToString();
                            role_id = result.User_nombre;
                        }
                    }
                }

            }
            catch (Exception q)
            {
                role_id = null;

            }
            return role_id;
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
