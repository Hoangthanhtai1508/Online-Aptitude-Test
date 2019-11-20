using OnlineAptitudeTest.Common;
using OnlineAptitudeTest.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OnlineAptitudeTest.Controllers
{
    public class ManagerController : Controller
    {
        User user = new User();
        ManagerDal managerDal = new ManagerDal();
        OnlineAptitudeTestEntities db = new OnlineAptitudeTestEntities();
        ReportDal report = new ReportDal();
        static int id_testcode;
        // GET: Manager
        public ActionResult Index()
        {
            if (!user.IsManager())
                return View("Error");
            managerDal.UpdateLastLogin();
            ViewBag.CountCan = managerDal.GetCandidates().Count();
            ViewBag.CountGen = qdata.GetQuestions().Where(b => b.TypeID.Equals(1)).ToList().Count();
            ViewBag.CountMath = qdata.GetQuestions().Where(b => b.TypeID.Equals(2)).Count();
            ViewBag.CountCom = qdata.GetQuestions().Where(b => b.TypeID.Equals(3)).Count();
            return View();
        }

        public ActionResult Logout()
        {
            if (!user.IsManager())
                return View("Error");
            user.Reset();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult ChangePassword()
        {
            if (!user.IsManager())
                return View("Error");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection f)
        {
            if (!user.IsManager())
                return View("Error");
            string curPass = f["CurrentPassword"];
            string newPass = f["NewPassword"];
            string confirmPass = f["ConfirmPassword"];
            if (!newPass.Equals(confirmPass))
            {
                ViewBag.error = "New password and confirm password is not the same.\nPlease try again.";
            }
            else if (!managerDal.CheckPass(user.ID, curPass))
            {
                ViewBag.error = "Current password is not correct.\nPlease try again.";
            }
            else
            {
                managerDal.ChangePassword(user.ID, newPass);
                ViewBag.success = "New password updated.";
            }
            return View();
        }


        #region Candidate CRUD
        public ActionResult Candidate(int? page, string txtSearch)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            else
            {
                if (page == null) page = 1;
                var cur = managerDal.GetCandidates();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                if (!String.IsNullOrEmpty(txtSearch))
                {
                    cur = managerDal.GetCandidates().Where(x => x.Name.ToUpper().Contains(txtSearch.ToUpper())).ToList();
                }
                return View(cur.ToPagedList(pageNumber, pageSize));
            }
        }

        [HttpGet]
        public ActionResult CreateCandidate()
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            return View();
        }
        [HttpPost]
        public ActionResult CreateCandidate(FormCollection form)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            // Model.UpdateLastSeen("Thêm Sinh Viên", Url.Action("AddStudent"));
            string Username = form["Email"];
            string Password = form["Phone"];

            string Address = form["Address"];
            string Email = form["Email"];
            string Name = form["Name"];
            string Gender = form["Gender"];
            string Birthday = form["Birthday"];
            string Phone = form["Phone"];
            string Education_Details = form["Education_Details"];
            string Work_Experience = form["Work_Experience"];


            int add = managerDal.AddCandidate(Username, Password, 3, Address, Email, Name, Gender, Birthday, Phone, Education_Details, Work_Experience, 0);
            if (add == 1)
            {
                TempData["CandidateID"] = true;
                TempData["statusCC"] = "Create success";
            }
            if (add == 5)
            {
                TempData["CandidateID"] = true;
                TempData["statusCC"] = "Under 18 years old or an invalid date of birth.";
            }

            if (add == 3)
            {
                TempData["CandidateID"] = false;
                TempData["statusCC"] = "This email already exists !!";
            }
            if (add == 4)
            {
                TempData["CandidateID"] = false;
                TempData["statusCC"] = "This phone already exists !!";
            }
            if (add == 0)
            {
                TempData["CandidateID"] = false;
                TempData["statusCC"] = "Create failed";
            }
            return RedirectToAction("CreateCandidate");
        }

        public ActionResult EditCandidate(string id)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            int id_cand = Convert.ToInt32(id);
            try
            {
                Candidate cand = managerDal.GetCandidate(id_cand);
                return View(cand);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult EditCandidate(FormCollection form)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            int id_student = Convert.ToInt32(form["CandidateID"]);
            string Password = form["Password"];

            string Email = form["Email"];
            string Name = form["Name"];
            string Gender = form["Gender"];
            string Birthday = form["Birthday"];
            string Phone = form["Phone"];
            string Address = form["Address"];
            string Education_Details = form["Education_Details"];
            string Work_Experience = form["Work_Experience"];

            int edit = managerDal.EditCandidate(id_student, Address, Password, Email, Name, Gender, Birthday, Phone, Education_Details, Work_Experience);
            if (edit == 1)
            {
                TempData["CandidateID"] = true;
                TempData["statusEdit"] = "The edit was successful";
            }
            if (edit == 5)
            {
                TempData["CandidateID"] = true;
                TempData["statusEdit"] = "Under 18 years old or an invalid date of birth";
            }

            if (edit == 4)
            {
                TempData["CandidateID"] = false;
                TempData["statusEdit"] = "This phone already exists !!";
            }

            if (edit == 3)
            {
                TempData["CandidateID"] = false;
                TempData["statusEdit"] = "This email already exists !!";
            }
            return RedirectToAction("EditCandidate/" + id_student);
        }

        public ActionResult DetailCandidate(string id)
        {
            if (!user.IsManager())
                return View("Error");
            int id_cand = Convert.ToInt32(id);
            Candidate cand = managerDal.GetCandidate(id_cand);
            return View(cand);
        }


        public ActionResult DeleteCandidate(string id)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            int id_cand = Convert.ToInt32(id);
            TestCandidateList check = db.TestCandidateLists.FirstOrDefault(c => c.CandidateID == id_cand);
            if (check != null)
            {
                TempData["CandidateID"] = false;
                TempData["statusCAN"] = "This candidate has taken the test";
                return RedirectToAction("Candidate");
            }
            bool del = managerDal.DeleteCandidate(id_cand);

            if (del)
            {
                TempData["CandidateID"] = true;
                TempData["statusCAN"] = "Delete successful";
            }
            else
            {

                TempData["CandidateID"] = false;
                TempData["statusCAN"] = "Delete failed";
            }
            return RedirectToAction("Candidate");
        }
        #endregion

        //=========================================================Test===============================

        #region Test View
        public ActionResult TestView(int? page)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            if (page == null) page = 1;
            var cur = managerDal.GetTest();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(cur.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CreateTest()
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateTest(FormCollection form)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            DateTime date = Convert.ToDateTime(form["date"]);
            TimeSpan time = TimeSpan.Parse(form["time"]);
            Test test = new Test();
            test.TimeStart = date + time;
            test.Note = form["note"];

            bool check = managerDal.CreateTest(test);
            if (check)
            {
                TempData["check"] = true;
                TempData["statusCTC"] = "Create test successfull!!!!";
            }
            else
            {
                TempData["check"] = false;
                TempData["statusCTC"] = "Create test falsed!!!!--The time is not valid";
            }

            return RedirectToAction("CreateTest");
        }

        public ActionResult EditTest(int id)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            Test test = managerDal.GetTestView(id);
            ViewBag.date = test.TimeStart.ToString("yyyy-MM-dd");
            ViewBag.time = test.TimeStart.TimeOfDay;

            return View(test);
        }

        [HttpPost]
        public ActionResult EditTest(FormCollection form, Test test)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            DateTime date = Convert.ToDateTime(form["date"]);
            TimeSpan time = TimeSpan.Parse(form["time"]);
            //Test test = new Test();
            //test.TestCode = Convert.ToInt32(form["TestCode"]);
            test.TimeStart = date + time;
            test.Note = form["note"];

            bool check = managerDal.EditTest(test);
            if (check)
            {
                TempData["check"] = true;
                TempData["statusCTC"] = "Edit test successfull!!!!";
            }
            else
            {
                TempData["check"] = false;
                TempData["statusCTC"] = "Edit test falsed!!!!--The time is not valid";
            }

            return RedirectToAction("EditTest/" + test.TestCode);
        }

        public ActionResult DeleteTestView(string id)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            int id_Testcode = Convert.ToInt32(id);
            TestCandidateList checkTestCodeInTest = db.TestCandidateLists.FirstOrDefault(c => c.TestCode == id_Testcode);
            if (checkTestCodeInTest != null)
            {
                TempData["CandidateID"] = false;
                TempData["statusTV"] = "This test has a candidate";
                return RedirectToAction("TestView");
            }

            bool del = managerDal.DeleteTestView(id_Testcode);
            if (del)
            {
                TempData["CandidateID"] = true;
                TempData["statusTV"] = "Delete successfull!";
            }
            else
            {

                TempData["CandidateID"] = false;
                TempData["statusTV"] = "Delete failed!!";
            }
            return RedirectToAction("TestView");
        }
        #endregion

        #region Test code
        public ActionResult TestCode(int? page)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            if (page == null) page = 1;
            var cur = managerDal.GetTest();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(cur.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddQuestion(int id)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            TestQuestion check = db.TestQuestions.FirstOrDefault(c => c.TestCode == id);
            if (check == null)
            {
                //   Model.UpdateLastSeen("Xóa Sinh Viên", Url.Action("DeleteStudent"));
                int id_student = id;
                int del = managerDal.AddQFT(id_student);
                TempData["CandidateID"] = true;
                TempData["statusAQ"] = "Add questions successfull!!!!";

                return RedirectToAction("TestCode");
            }

            else
            {
                TempData["CandidateID"] = false;
                TempData["statusAQ"] = "This test already had questions";
                return RedirectToAction("TestCode");
            }

        }


        static int ktraAdd = 0;
        public ActionResult AddCandidateForTest(int id)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            TestQuestion check = db.TestQuestions.FirstOrDefault(c => c.TestCode == id);
            if (check == null)
            {
                TempData["CandidateID"] = false;
                TempData["statusAQ"] = "This test has no questions yet.";
                return RedirectToAction("TestCode");
            }
            id_testcode = id;
            if (managerDal.GetCandidateForTest().Count() == 0 && ktraAdd == 0)
            {
                TempData["CandidateID"] = true;
                TempData["statusACTC"] = "There are no contestants to add!!!!!!";

            }
            ktraAdd = 0;
            return View(managerDal.GetCandidateForTest());

        }

        [HttpPost]
        public ActionResult AddCandidateForTest(int[] dsADD)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            foreach (int id in dsADD)
            {
                bool del = managerDal.addForTest(id_testcode, id);
                if (del)
                {
                    TempData["CandidateID"] = true;
                    TempData["statusACTC"] = "Add candidate successfull";
                }
                else
                {
                    TempData["CandidateID"] = false;
                    TempData["statusACTC"] = "Add candidate failed!!";
                }

            }
            ktraAdd = 1;
            return RedirectToAction("AddCandidateForTest");
        }
        static int ktraDelete = 0;
        public ActionResult DeleteCandidateFTC(int id)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            if (managerDal.GetCandidateToDelete(id).Count() == 0 && ktraDelete == 0)
            {
                TempData["CandidateID"] = true;
                TempData["statusCFTC"] = "There are no candidates to delete!!!!!!";
            }
            ktraDelete = 0;
            return View(managerDal.GetCandidateToDelete(id));
        }
        [HttpPost]
        public ActionResult DeleteCandidateFTC(int[] dsxoa)
        {
            if (!user.IsManager())
            {
                return View("Error");
            }
            foreach (int id in dsxoa)
            {
                bool del = managerDal.DeleteCFTC(id);
                if (del)
                {
                    TempData["CandidateID"] = true;
                    TempData["statusCFTC"] = "Successfully remove the candidate from the test";
                }
                else
                {
                    TempData["CandidateID"] = false;
                    TempData["statusCFTC"] = "Remove failed";
                }

            }
            ktraDelete = 1;
            return RedirectToAction("DeleteCandidateFTC");
        }

        #endregion


        //================================Test=====================
        QuestionData qdata = new QuestionData();
        public ActionResult GeneralKnowledge(int? page, string txtSearch)
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                if (page == null) page = 1;
                var cur = qdata.GetQuestions().Where(b => b.TypeID.Equals(1)).OrderBy(x => x.QuestionID);
                int pageSize = 10;
                int pageNumber = (page ?? 1);

                if (!String.IsNullOrEmpty(txtSearch))
                {
                    cur = qdata.GetQuestions().Where(b => b.TypeID.Equals(1) && b.QuestionDetails.ToUpper().Contains(txtSearch.ToUpper())).OrderBy(x => x.QuestionID);
                }

                return View(cur.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Mathematics(int? page, string txtSearch)
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                if (page == null) page = 1;
                var cur = qdata.GetQuestions().Where(b => b.TypeID.Equals(2)).OrderBy(x => x.QuestionID);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                if (!String.IsNullOrEmpty(txtSearch))
                {
                    cur = qdata.GetQuestions().Where(b => b.TypeID.Equals(2) && b.QuestionDetails.ToUpper().Contains(txtSearch.ToUpper())).OrderBy(x => x.QuestionID);
                }
                return View(cur.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult ComputerTechnology(int? page, string txtSearch)
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                if (page == null) page = 1;
                var cur = qdata.GetQuestions().Where(b => b.TypeID.Equals(3)).OrderBy(x => x.QuestionID);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                if (!String.IsNullOrEmpty(txtSearch))
                {
                    cur = qdata.GetQuestions().Where(b => b.TypeID.Equals(3) && b.QuestionDetails.ToUpper().Contains(txtSearch.ToUpper())).OrderBy(x => x.QuestionID);
                }
                return View(cur.ToPagedList(pageNumber, pageSize));
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(FormCollection form, HttpPostedFileBase File)
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                int id_subject = Convert.ToInt32(form["id_subject"]);
                int unit = Convert.ToInt32(form["unit"]);
                string content = form["content"];
                string[] answer = new string[] {
                form["answer_a"],
                form["answer_b"],
                form["answer_c"],
                form["answer_d"]
            };
                string answer_a = answer[0];
                string answer_b = answer[1];
                string answer_c = answer[2];
                string answer_d = answer[3];
                string correct_answer = form["correct_answer"];


                bool add = qdata.Create(id_subject, unit, content, answer_a, answer_b, answer_c, answer_d, correct_answer);
                if (add)
                {
                    TempData["status_id"] = true;
                    TempData["status"] = "Add complete !";
                }
                else
                {
                    TempData["status_id"] = false;
                    TempData["status"] = "Error, try again !";
                }
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                int QuestionID = id;
                try
                {
                    Question ques = qdata.GetQuestion(QuestionID);
                    //   Model.UpdateLastSeen("Sửa Sinh Viên " + student.name, Url.Action("EditStudent/" + id));
                    //     ViewBag.ListSpecialities = Model.GetSpecialities();
                    //     ViewBag.ListClass = Model.GetClasses();
                    return View(ques);
                }
                catch (Exception)
                {
                    return View("Error");
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(FormCollection form, HttpPostedFileBase File)
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                int QuestionID = Convert.ToInt32(form["QuestionID"]);
                int unit = Convert.ToInt32(form["unit"]);

                string content = form["Question1"];
                string[] answer = new string[] {
                form["answer_a"],
                form["answer_b"],
                form["answer_c"],
                form["answer_d"]
            };
                string answer_a = answer[0];
                string answer_b = answer[1];
                string answer_c = answer[2];
                string answer_d = answer[3];
                string correct_answer = form["correct_answer"];
                bool edit = qdata.Update(QuestionID, unit, content, answer_a, answer_b, answer_c, answer_d, correct_answer);
                if (edit)
                {
                    TempData["status_id"] = true;
                    TempData["status"] = "Fix complete !";
                }
                else
                {
                    TempData["status_id"] = false;
                    TempData["status"] = "Error, try again !";
                }
                return RedirectToAction("Edit/" + QuestionID);
            }
        }
        public ActionResult Detail(int id)
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                var cur = qdata.GetQuestions().Where(b => b.QuestionID.Equals(id));
                return View(cur);
            }
        }
        //[HttpPost]

        public ActionResult DeleteGeneralKnowledge(int id)
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                int id_question = Convert.ToInt32(id);
                bool del = qdata.DeleteQuestion(id_question);
                if (del)
                {
                    TempData["status_id"] = true;
                    TempData["status"] = "Delete complete !";
                }
                else
                {
                    TempData["status_id"] = false;
                    TempData["status"] = "Error, this question have already in test !!!";
                }
                return RedirectToAction("GeneralKnowledge");
            }
        }
        public ActionResult DeleteMathematics(int id)
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                int id_question = Convert.ToInt32(id);
                bool del = qdata.DeleteQuestion(id_question);
                if (del)
                {
                    TempData["status_id"] = true;
                    TempData["status"] = "Delete complete !";
                }
                else
                {
                    TempData["status_id"] = false;
                    TempData["status"] = "Error, this question have already in test !!!";
                }
                return RedirectToAction("Mathematics");
            }
        }
        public ActionResult DeleteComputerTechnology(int id)
        {
            if (!user.IsManager())
                return View("Error");
            else
            {
                int id_question = Convert.ToInt32(id);
                bool del = qdata.DeleteQuestion(id_question);
                if (del)
                {
                    TempData["status_id"] = true;
                    TempData["status"] = "Delete complete !";
                }
                else
                {
                    TempData["status_id"] = false;
                    TempData["status"] = "Error, this question have already in test !!!";
                }
                return RedirectToAction("ComputerTechnology");
            }
        }


        public void SetViewBag2()
        {
            int[] select1 = { 1, 2, 3, 4 };
            var dao = new QuestionData();
            ViewBag.CorrectAnswer = new SelectList(select1);
        }




        public ActionResult Report()
        {
            return View();
        }

        //============ 
        public ActionResult ShowReport(FormCollection form)
        {
            TempData["sttinfo"] = null;
            if (ViewBag.Total = report.CountCanTestFromTo(form) != 0)
            {
                ViewBag.Total = report.CountCanTestFromTo(form);
                ViewBag.Passed = report.CountCanPassedTestFromTo(form);
                ViewBag.fromDate = DateTime.Parse(Convert.ToString(form["TuNgay"])).ToString("MM/dd/yyyy");
                ViewBag.toDate = DateTime.Parse(Convert.ToString(form["DenNgay"])).ToString("MM/dd/yyyy");
                return View();
            }
            else
            {
                TempData["sttinfo"] = "123";
                TempData["info"] = "Dont have more information from your time choice!!!";
                return RedirectToAction("Report");

            }
        }

        public ActionResult ShowReport2(FormCollection form)
        {
            TempData["sttinfo"] = null;
            if (ViewBag.Total = report.CountCanTest(form) != 0)
            {
                ViewBag.Total = report.CountCanTest(form);
                ViewBag.Passed = report.CountCanPassedTest(form);
                ViewBag.NgayChon = DateTime.Parse(Convert.ToString(form["NgayChon"])).ToString("MM/dd/yyyy");
                return View();
            }
            else
            {
                TempData["sttinfo"] = "123";
                TempData["info"] = "Dont have more information from your day choice!!!";
                return RedirectToAction("Report");
            }
        }

        public ActionResult ShowReport3(FormCollection form)
        {
            TempData["sttinfo"] = null;
            if (ViewBag.Total = report.CountCanTestMonth(form) != 0)
            {
                ViewBag.Total = report.CountCanTestMonth(form);
                ViewBag.Passed = report.CountCanPassedTestMonth(form);
                ViewBag.Month = DateTime.Parse(Convert.ToString(form["ThangChon"])).ToString("MMMM yyyy");
                return View();
            }
            else
            {
                TempData["sttinfo"] = "123";
                TempData["info"] = "Dont have more information from your month choice!!!";
                return RedirectToAction("Report");
            }
        }

    }
}