using OnlineAptitudeTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAptitudeTest.Models
{
    public class CandidateDal
    {
        OnlineAptitudeTestEntities db = new OnlineAptitudeTestEntities();

        User user = new User();

        public bool CheckPass(int id, string pass)
        {
            string encryptedPass = Common.Encryptor.MD5Hash(pass);
            var check = db.Candidates.SingleOrDefault(x => x.CandidateID == id && x.Password.Equals(encryptedPass));
            if (check != null)
            {
                return true;
            }
            return false;
        }

        public void ChangePassword(int id, string pass)
        {
            string encryptedPass = Common.Encryptor.MD5Hash(pass);
            var check = db.Candidates.SingleOrDefault(x => x.CandidateID == id);
            if (check != null)
            {
                check.Password = encryptedPass;
                db.SaveChanges();
            }
        }

        public void UpdateLastLogin()
        {
            var updateLastLogin = (from x in db.Candidates where x.CandidateID == user.ID select x).Single();
            updateLastLogin.LastLogin = DateTime.Now;
            db.SaveChanges();
        }

        public bool CheckValidTime()
        {
            var test = db.Tests.SingleOrDefault(x => x.TestCode == user.TESTCODE);
            var timeStart = test.TimeStart;
            var timeEnd = timeStart.AddHours(2);
            if (timeStart <= DateTime.Now && DateTime.Now <= timeEnd)
                return true;
            return false;
        }

        public bool CheckCanDetails()
        {
            var check = db.Candidate_Test_Detail.Count(x => x.CandidateID == user.ID);
            if (check > 0)
                return true;
            return false;
        }

        public bool CheckCanScore()
        {
            var check = db.Scores.Count(x => x.CandidateID == user.ID);
            if (check > 0)
                return true;
            return false;
        }

        public void AddCanScore()
        {
            Score score = new Score();
            score.CandidateID = user.ID;
            score.Part1Score = 0;
            score.Part2Score = 0;
            score.Part3Score = 0;
            db.Scores.Add(score);
            db.SaveChanges();
        }

        public void UpdateScore(int part, int correct)
        {
            var can = db.Scores.Single(x => x.CandidateID == user.ID);
            double score = 0;
            score = ((double)correct / (double)5) * 100;

            if (part == 1)
            {
                can.Part1Score = score;
            }
            else if (part == 2)
            {
                can.Part2Score = score;
            }
            else
            {
                can.Part3Score = score;
            }
            db.SaveChanges();
        }

        public void UpdatePassStatus()
        {
            var can = db.Candidates.Single(x => x.CandidateID == user.ID);
            var result = db.Scores.Single(x => x.CandidateID == user.ID);
            double avg =(double)(result.Part1Score + result.Part2Score + result.Part3Score) / 3;
            can.FinalScore = avg;
            if(result.Part1Score >= 40 && result.Part2Score >=40 && result.Part3Score >= 40)
            {
                can.Pass = 1;
            }
            else
            {
                can.Pass = 0;
            }
            db.SaveChanges();
        }

        public void AddQuestionToCanDetails()
        {
            List<Question> quesList = new List<Question>();
            quesList = (from x in db.Questions
                        join y in db.TestQuestions on x.QuestionID equals y.QuestionID
                        join t in db.Tests on y.TestCode equals t.TestCode
                        where t.TestCode == user.TESTCODE
                        select x).ToList();
            foreach (var item in quesList)
            {
                var ques = new Candidate_Test_Detail() { CandidateID = user.ID, QuestionID = item.QuestionID };
                db.Candidate_Test_Detail.Add(ques);
            }
            db.SaveChanges();
        }


        public List<CandidateTestViewModel> GetListGeneralQuest(int test_code)
        {
            List<CandidateTestViewModel> list = new List<CandidateTestViewModel>();
            try
            {
                list = (from x in db.Candidate_Test_Detail
                        join t in db.Questions on x.QuestionID equals t.QuestionID
                        join q in db.TestQuestions on t.QuestionID equals q.QuestionID
                        join y in db.Tests on q.TestCode equals y.TestCode
                        where y.TestCode == test_code &&
                        x.CandidateID == user.ID &&
                        t.TypeID == 1
                        select new CandidateTestViewModel { candidate_test = x, question = t, test_question = q, test = y }).OrderBy(x => x.candidate_test.ID).ToList();
            }
            catch (Exception) { }
            return list;
        }


        public void UpdateCandidateTest(int id_question, string answer)
        {
            var update = (from x in db.Candidate_Test_Detail where x.CandidateID == user.ID && x.QuestionID == id_question select x).Single();
            update.CandidateAnswer = answer;
            db.SaveChanges();
        }

        public void UpdateCandidateStatus(int part)
        {
            var update = (from x in db.Candidates where x.CandidateID == user.ID select x).Single();
            update.TestStatus = part;
            HttpContext.Current.Session[UserSession.TESTSTATUS] = part;
            db.SaveChanges();
        }


        public List<CandidateTestViewModel> GetListMathQuest(int test_code)
        {
            List<CandidateTestViewModel> list = new List<CandidateTestViewModel>();
            try
            {
                list = (from x in db.Candidate_Test_Detail
                        join t in db.Questions on x.QuestionID equals t.QuestionID
                        join q in db.TestQuestions on t.QuestionID equals q.QuestionID
                        join y in db.Tests on q.TestCode equals y.TestCode
                        where y.TestCode == test_code &&
                        x.CandidateID == user.ID &&
                        t.TypeID == 2
                        select new CandidateTestViewModel { candidate_test = x, question = t, test_question = q, test = y }).OrderBy(x => x.candidate_test.ID).ToList();
            }
            catch (Exception) { }
            return list;
        }

        public List<CandidateTestViewModel> GetListCompQuest(int test_code)
        {
            List<CandidateTestViewModel> list = new List<CandidateTestViewModel>();
            try
            {
                list = (from x in db.Candidate_Test_Detail
                        join t in db.Questions on x.QuestionID equals t.QuestionID
                        join q in db.TestQuestions on t.QuestionID equals q.QuestionID
                        join y in db.Tests on q.TestCode equals y.TestCode
                        where y.TestCode == test_code &&
                        x.CandidateID == user.ID &&
                        t.TypeID == 3
                        select new CandidateTestViewModel { candidate_test = x, question = t, test_question = q, test = y }).OrderBy(x => x.candidate_test.ID).ToList();
            }
            catch (Exception) { }
            return list;
        }


        public CandidateScoreViewModel GetResult()
        {
            CandidateScoreViewModel result = new CandidateScoreViewModel();
            try
            {
                result = (from x in db.Candidates
                          join t in db.Scores on x.CandidateID equals t.CandidateID
                          where x.CandidateID == user.ID
                          select new CandidateScoreViewModel { candidate = x,score = t }).Single();
            }
            catch (Exception) { }
            return result;
        }

        public List<CandidateScoreViewModel> GetPassedCandidate(string txtSearch)
        {
            List<CandidateScoreViewModel> list = new List<CandidateScoreViewModel>();
            if (!String.IsNullOrEmpty(txtSearch))
            {
                try
                {
                    list = (from x in db.Candidates
                            join t in db.Scores on x.CandidateID equals t.CandidateID
                            where x.Pass == 1 &&
                            x.Name.ToUpper().Contains(txtSearch.ToUpper())
                            select new CandidateScoreViewModel { candidate = x, score = t }).ToList();
                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    list = (from x in db.Candidates
                            join t in db.Scores on x.CandidateID equals t.CandidateID
                            where x.Pass == 1
                            select new CandidateScoreViewModel { candidate = x, score = t }).ToList();
                }
                catch (Exception) { }
            }

            return list;
        }

        public CandidateScoreViewModel PassedCandidateDetails(int id)
        {
            CandidateScoreViewModel result = new CandidateScoreViewModel();
            try
            {
                result = (from x in db.Candidates
                          join t in db.Scores on x.CandidateID equals t.CandidateID
                          where x.Pass == 1 && x.CandidateID == id
                          select new CandidateScoreViewModel { candidate = x, score = t }).Single();
            }
            catch (Exception) { }
            return result;
        }
    }
}