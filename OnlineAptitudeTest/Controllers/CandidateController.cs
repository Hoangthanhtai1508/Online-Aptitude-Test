using OnlineAptitudeTest.Common;
using OnlineAptitudeTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAptitudeTest.Controllers
{
    public class CandidateController : Controller
    {
        User user = new User();
        CandidateDal canDal = new CandidateDal();
        List<CandidateTestViewModel> general = null;
        List<CandidateTestViewModel> math = null;
        List<CandidateTestViewModel> comp = null;
        CandidateScoreViewModel result = null;
        // GET: Candidate
        public ActionResult Index()
        {
            if (!user.IsCandidate())
                return View("Error");
            canDal.UpdateLastLogin();
            return View();
        }

        public ActionResult Logout()
        {
            if (!user.IsCandidate())
                return View("Error");
            user.Reset();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult ChangePassword()
        {
            if (!user.IsCandidate())
                return View("Error");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection f)
        {
            if (!user.IsCandidate())
                return View("Error");
            string curPass = f["CurrentPassword"];
            string newPass = f["NewPassword"];
            string confirmPass = f["ConfirmPassword"];
            if (!newPass.Equals(confirmPass))
            {
                ViewBag.error = "New password and confirm password is not the same.\nPlease try again.";
            }
            else if (!canDal.CheckPass(user.ID, curPass))
            {
                ViewBag.error = "Current password is not correct.\nPlease try again.";
            }
            else
            {
                canDal.ChangePassword(user.ID, newPass);
                ViewBag.success = "New password updated.";
            }
            return View();
        }

        public ActionResult BeginTest()
        {
            if (!user.IsCandidate())
                return View("Error");
            if (user.TESTCODE == 0)
                return View("Error");
            if (canDal.CheckValidTime())
            {
                if (user.TESTSTATUS == 0)
                {
                    canDal.UpdateCandidateStatus(1);
                }
                if (canDal.CheckCanDetails() == false)
                {
                    canDal.AddQuestionToCanDetails();
                }
                if (canDal.CheckCanScore() == false)
                {
                    canDal.AddCanScore();
                }
                return View();
            }
            else
                return View("Error");

        }
        public ActionResult GeneralKnowledge()
        {
            if (!user.IsCandidate())
                return View("Error");
            if (user.TESTCODE == 0)
                return View("Error");
            if (user.TESTSTATUS == 1)
            {
                general = canDal.GetListGeneralQuest(user.TESTCODE);
                ViewBag.min = 5;
                ViewBag.sec = 0;
                return View(general);
            }
            else
                return View("Error");
        }


        [HttpPost]
        public void UpdateCandidateTest(FormCollection form)
        {
            int id_quest = Convert.ToInt32(form["id"]);
            string answer = form["answer"];
            answer = answer.Trim();
            canDal.UpdateCandidateTest(id_quest, answer);
        }

        public ActionResult SubmitPart1()
        {
            if (!user.IsCandidate())
                return View("Error");
            canDal.UpdateCandidateStatus(2);
            general = canDal.GetListGeneralQuest(user.TESTCODE);
            int count_correct = 0;
            foreach (var item in general)
            {
                if (item.candidate_test.CandidateAnswer != null && item.candidate_test.CandidateAnswer.Trim().Equals(item.question.CorrectAnswer.Trim()))
                    count_correct++;
            }
            canDal.UpdateScore(1, count_correct);
            return RedirectToAction("BeginTest");
        }

        public ActionResult SubmitPart2()
        {
            if (!user.IsCandidate())
                return View("Error");
            canDal.UpdateCandidateStatus(3);

            math = canDal.GetListMathQuest(user.TESTCODE);
            int count_correct = 0;
            foreach (var item in math)
            {
                if (item.candidate_test.CandidateAnswer != null && item.candidate_test.CandidateAnswer.Trim().Equals(item.question.CorrectAnswer.Trim()))
                    count_correct++;
            }
            canDal.UpdateScore(2, count_correct);

            return RedirectToAction("BeginTest");
        }

        public ActionResult SubmitPart3()
        {
            if (!user.IsCandidate())
                return View("Error");
            canDal.UpdateCandidateStatus(4);

            comp = canDal.GetListCompQuest(user.TESTCODE);
            int count_correct = 0;
            foreach (var item in comp)
            {
                if (item.candidate_test.CandidateAnswer != null && item.candidate_test.CandidateAnswer.Trim().Equals(item.question.CorrectAnswer.Trim()))
                    count_correct++;
            }
            canDal.UpdateScore(3, count_correct);
            canDal.UpdatePassStatus();
            return RedirectToAction("Result");
        }

        public ActionResult Mathematics()
        {
            if (!user.IsCandidate())
                return View("Error");
            if (user.TESTCODE == 0)
                return View("Error");

            if (user.TESTSTATUS == 2)
            {
                math = canDal.GetListMathQuest(user.TESTCODE);
                ViewBag.min = 5;
                ViewBag.sec = 0;
                return View(math);
            }
            else
                return View("Error");
        }

        public ActionResult ComputerTechnology()
        {
            if (!user.IsCandidate())
                return View("Error");
            if (user.TESTCODE == 0)
                return View("Error");

            if (user.TESTSTATUS == 3)
            {
                comp = canDal.GetListCompQuest(user.TESTCODE);
                ViewBag.min = 5;
                ViewBag.sec = 0;
                return View(comp);
            }
            else
                return View("Error");
        }

        public ActionResult Result()
        {
            if (!user.IsCandidate())
                return View("Error");
            result = canDal.GetResult();
            return View(result);
        }
    }
}