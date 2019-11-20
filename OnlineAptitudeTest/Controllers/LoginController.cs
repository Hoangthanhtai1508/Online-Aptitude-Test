using OnlineAptitudeTest.Common;
using OnlineAptitudeTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAptitudeTest.Controllers
{
    public class LoginController : Controller
    {
        User user;
        // GET: Login
        public ActionResult Index()
        {
            if (Session[UserSession.ISLOGIN] != null && (bool)Session[UserSession.ISLOGIN])
            {
                if ((int)Session[UserSession.ROLEID] == 1)
                    return RedirectToAction("Index", "Admin");
                if ((int)Session[UserSession.ROLEID] == 2)
                    return RedirectToAction("Index", "Manager");
                if ((int)Session[UserSession.ROLEID] == 3)
                    return RedirectToAction("Index", "Candidate");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CheckLogin(model))
                {
                    user = new User();
                    if (user.IsAdmin())
                        return RedirectToAction("Index", "Admin");
                    if (user.IsManager())
                        return RedirectToAction("Index", "Manager");
                    if (user.IsCandidate())
                        return RedirectToAction("Index", "Candidate");
                }
                else
                    ViewBag.error = "Username or password is incorrect.";
            }
            else
                ViewBag.error = "There was an error in the process. Please try again.";
            return View();
        }
    }
}