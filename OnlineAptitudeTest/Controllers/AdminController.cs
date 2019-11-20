using OnlineAptitudeTest.Common;
using OnlineAptitudeTest.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAptitudeTest.Controllers
{
    public class AdminController : Controller
    {
        User user = new User();
        AdminDal adminDal = new AdminDal();
        ManagerDal managerDal = new ManagerDal();
        CandidateDal candidateDal = new CandidateDal();
        // GET: Admin
        public ActionResult Index()
        {
            if (!user.IsAdmin())
                return View("Error");
            adminDal.UpdateLastLogin();
            ViewBag.CountMan = managerDal.GetManagers().Count();
            ViewBag.CountCan = candidateDal.GetPassedCandidate(null).Count;
            return View();
        }

        public ActionResult Logout()
        {
            if (!user.IsAdmin())
                return View("Error");
            user.Reset();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult ChangePassword()
        {
            if (!user.IsAdmin())
                return View("Error");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection f)
        {
            if (!user.IsAdmin())
                return View("Error");
            string curPass = f["CurrentPassword"];
            string newPass = f["NewPassword"];
            string confirmPass = f["ConfirmPassword"];
            if (!newPass.Equals(confirmPass))
            {
                ViewBag.error = "New password and confirm password is not the same.\nPlease try again.";
            }
            else if (!adminDal.CheckPass(user.ID, curPass))
            {
                ViewBag.error = "Current password is not correct.\nPlease try again.";
            }
            else
            {
                adminDal.ChangePassword(user.ID, newPass);
                ViewBag.success = "New password updated.";
            }
            return View();
        }


        public ActionResult PassedCandidateList(int? page, string txtSearch)
        {
            if (!user.IsAdmin())
                return View("Error");
            if (page == null) page = 1;
            var cur = candidateDal.GetPassedCandidate(txtSearch);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(cur.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult PassedCandidateDetails(int id)
        {
            if (!user.IsAdmin())
                return View("Error");
            CandidateScoreViewModel can = candidateDal.PassedCandidateDetails(id);
            return View(can);
        }

        public ActionResult ManagerList(int? page, string txtSearch)
        {
            if (!user.IsAdmin())
                return View("Error");
            else
            {
                if (page == null) page = 1;
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                var cur = managerDal.GetManagers();
                if (!String.IsNullOrEmpty(txtSearch))
                {
                    cur = managerDal.GetManagers().Where(x => x.Name.ToUpper().Contains(txtSearch.ToUpper())).ToList();
                }

                return View(cur.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Details(int id)
        {
            if (!user.IsAdmin())
                return View("Error");
            return View(managerDal.GetManager(id));
        }

        public ActionResult CreateManager()
        {
            if (!user.IsAdmin())
                return View("Error");
            return View();
        }

        [HttpPost]
        public ActionResult CreateManager(AdminManager a)
        {
            if (!user.IsAdmin())
                return View("Error");
            a.RoleID = 2;
            a.LastLogin = DateTime.Now;
            string encryptedPass = Common.Encryptor.MD5Hash(a.Password);
            a.Password = encryptedPass;
            if (ModelState.IsValid)
            {
                if (managerDal.CheckUserName(a.Username))
                {
                    ViewBag.error = "The Username already exists!!!";
                }
                else if (managerDal.CheckEmail(a.Email))
                {
                    ViewBag.error = "The email already exists!!!";
                }
                else if (managerDal.CheckPhone(a.Phone))
                {
                    ViewBag.error = "The phone number already exists!!!";
                }
                else
                {
                    bool check = managerDal.CreateADManager(a);
                    if (!check)
                    {
                        TempData["UserName"] = false;
                        TempData["statusCC"] = "Manager age must be 18 or older";
                    }
                    else
                    {
                        TempData["stt"] = true;
                        TempData["stt"] = "More success---";
                    }
                    return RedirectToAction("ManagerList", "Admin");
                }
            }
            return View(a);
        }

        public ActionResult EditManager(int id)
        {
            if (!user.IsAdmin())
                return View("Error");
            return View(managerDal.GetManager(id));
        }
        [HttpPost]
        public ActionResult EditManager(AdminManager a)
        {
            if (!user.IsAdmin())
                return View("Error");
            if (managerDal.CheckPass_Update(a) == false)
            {
                string encryptedPass = Common.Encryptor.MD5Hash(a.Password);
                a.Password = encryptedPass;
            }
            if (managerDal.CheckUserName_Update(a))
            {
                ViewBag.error = "The Username already exists!!!";
            }
            else if (managerDal.CheckEmail_Update(a))
            {
                ViewBag.error = "The email already exists!!!";
            }
            else if (managerDal.CheckPhone_Update(a))
            {
                ViewBag.error = "The phone number already exists!!!";
            }
            else
            {
                bool check = managerDal.UpdateADManager(a);
                if (!check)
                {
                    TempData["UserName"] = false;
                    TempData["statusCC"] = "Manager age must be 18 or older";
                }
                else
                {
                    TempData["stt"] = true;
                    TempData["stt"] = "Correction successful";
                    return RedirectToAction("ManagerList", "Admin");
                }
                return View(a);
            }
            return View(a);

        }

        public ActionResult DeleteManager(int id)
        {
            if (!user.IsAdmin())
                return View("Error");
            if (managerDal.DeleteADManager(id))
            {
                TempData["stt"] = true;
                TempData["stt"] = "Delete successful";
            }
            else
            {
                TempData["stt"] = false;
                TempData["stt"] = "Delete failed";
            }
            return RedirectToAction("ManagerList");
        }
    }
}