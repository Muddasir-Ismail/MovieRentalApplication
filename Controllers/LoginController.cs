using MovieRentalApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MovieRentalApplication.Controllers
{
    public class LoginController : Controller
    {
        MyFirstApplicationMVCEntities db = new MyFirstApplicationMVCEntities();
        HttpCookie cookie = new HttpCookie("User");

        // GET: Login
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["User"];
            if (cookie != null)
            {
                ViewBag.username = cookie["username"].ToString();
                string  EncryptedPassword = cookie["password"].ToString();
                byte[] b = Convert.FromBase64String(EncryptedPassword);
                string decryptPassword = ASCIIEncoding.ASCII.GetString(b);
                ViewBag.password = cookie["password"].ToString();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(login s)
        {
            if (ModelState.IsValid == true)
            {
                if (s.remember_me == true)
                {
                    cookie["Username"] = s.username;
                    byte[] b = ASCIIEncoding.ASCII.GetBytes(s.password);
                    string EncryptedPassword = Convert.ToBase64String(b);
                    cookie["password"] = EncryptedPassword;
                    cookie.Expires = DateTime.Now.AddDays(2);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Response.Cookies.Add(cookie);
                }

                var credientials = db.logins.Where(model => model.username == s.username && model.password == s.password).FirstOrDefault();
                if (credientials != null)
                {
                    // Session["UserId"] = s.id.ToString();
                    Session["Username"] = s.username.ToString();
                    TempData["LoginMessage"] = "<script>alert('Login successfully!')</script>";
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    TempData["LoginMessage"] = "<script>alert('Login failed!')</script>";
                }
            }
            return View();
        }
    }
}