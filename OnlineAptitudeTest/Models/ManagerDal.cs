using OnlineAptitudeTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAptitudeTest.Models
{
    public class ManagerDal
    {
        //===========================Khoa===========================
        User user = new User();
        OnlineAptitudeTestEntities db = new OnlineAptitudeTestEntities();

        public bool CheckPass(int id, string pass)
        {
            string encryptedPass = Common.Encryptor.MD5Hash(pass);
            var check = db.AdminManagers.SingleOrDefault(x => x.AdminManagerID == id && x.Password.Equals(encryptedPass));
            if (check != null)
            {
                return true;
            }
            return false;
        }

        public void ChangePassword(int id, string pass)
        {
            string encryptedPass = Common.Encryptor.MD5Hash(pass);
            var check = db.AdminManagers.SingleOrDefault(x => x.AdminManagerID == id);
            if (check != null)
            {
                check.Password = encryptedPass;
                db.SaveChanges();
            }
        }

        public void UpdateLastLogin()
        {
            var updateLastLogin = (from x in db.AdminManagers where x.AdminManagerID == user.ID select x).Single();
            updateLastLogin.LastLogin = DateTime.Now;
            db.SaveChanges();
        }
        //==========================================================

        //===========================Tai===========================
        public List<AdminManager> GetManagers()
        {
            return db.AdminManagers.Where(x => x.RoleID == 2).ToList();
            //return entity.AdminManagers.ToList();
        }

        public AdminManager GetManager(int id)
        {
            return db.AdminManagers.SingleOrDefault(a => a.AdminManagerID == id);
        }

        public bool CreateADManager(AdminManager a)
        {
            try
            {
                AdminManager check = db.AdminManagers.SingleOrDefault(aa => aa.AdminManagerID == a.AdminManagerID);
                if (check == null)
                {
                    //a.RoleID = 2;
                    if (((DateTime.Now.Year - Convert.ToDateTime(a.Birthday).Year == 18) && (DateTime.Now.Month >= Convert.ToDateTime(a.Birthday).Month) && (DateTime.Now.Day >= Convert.ToDateTime(a.Birthday).Day)) || ((DateTime.Now.Year - Convert.ToDateTime(a.Birthday).Year > 18)))
                    {
                        db.AdminManagers.Add(a);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public bool CheckUserName(string user)
        {
            var check = db.AdminManagers.SingleOrDefault(aa => aa.Username.Equals(user));
            if (check != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckEmail(string email)
        {
            var check = db.AdminManagers.SingleOrDefault(aa => aa.Email.Equals(email));
            if (check != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckPhone(string phone)
        {
            var check = db.AdminManagers.SingleOrDefault(aa => aa.Phone.Equals(phone));
            if (check != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckUserName_Update(AdminManager a)
        {
            var check = db.AdminManagers.SingleOrDefault(aa => aa.Username.Equals(a.Username) && aa.AdminManagerID != a.AdminManagerID);
            if (check != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckEmail_Update(AdminManager a)
        {
            var check = db.AdminManagers.SingleOrDefault(aa => aa.Email.Equals(a.Email) && aa.AdminManagerID != a.AdminManagerID);
            if (check != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckPass_Update(AdminManager a)
        {
            var check = db.AdminManagers.SingleOrDefault(x => x.AdminManagerID == a.AdminManagerID);
            if (check.Password.Equals(a.Password))
            {
                return true;
            }
            return false;
        }

        public bool CheckPhone_Update(AdminManager a)
        {
            var check = db.AdminManagers.SingleOrDefault(aa => aa.Phone.Equals(a.Phone) && aa.AdminManagerID != a.AdminManagerID);
            if (check != null)
            {
                return true;
            }
            return false;
        }

        public bool UpdateADManager(AdminManager a)
        {
            bool temp = false;
            if (DateTime.Now.Year - a.Birthday.Year > 18)
            {
                temp = true;
            }
            else if ((DateTime.Now.Year - a.Birthday.Year == 18) && (DateTime.Now.Month > a.Birthday.Month))
            {
                temp = true;
            }
            else if ((DateTime.Now.Year - a.Birthday.Year == 18) && (DateTime.Now.Month == a.Birthday.Month) && (DateTime.Now.Day >= a.Birthday.Day))
            {
                temp = true;
            }
            else
            {
                temp = false;
            }


            if (temp == true)
            {
                var check = db.AdminManagers.SingleOrDefault(x => x.AdminManagerID == a.AdminManagerID);
                check.Username = a.Username;
                check.Password = a.Password;
                check.RoleID = a.RoleID;
                check.Email = a.Email;
                check.Name = a.Name;
                check.Gender = a.Gender;
                check.Birthday = a.Birthday;
                check.Phone = a.Phone;
                check.LastLogin = a.LastLogin;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool DeleteADManager(int id)
        {
            AdminManager check = db.AdminManagers.SingleOrDefault(c => c.AdminManagerID == id);
            if (check != null)
            {
                try
                {
                    var a = db.AdminManagers.Remove(check);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            return false;
        }

        //==========================================================

        //===========================Huynh==========================

        public List<Candidate> GetCandidates()
        {
            return db.Candidates.ToList();
        }

        public List<Test> GetTest()
        {

            return db.Tests.ToList();
        }

        public int AddCandidate(string _Username, string _Password, int _RoleID, string _Address, string _Email, string _Name, string _Gender, string _birthday, string _Phone, string _EducationDetail, string _WorkExp, int _Testcode)
        {
            var candidate = new Candidate();
            Candidate check1 = db.Candidates.FirstOrDefault(c => c.Email.Trim() == _Email.Trim());
            Candidate check2 = db.Candidates.FirstOrDefault(c => c.Phone.Trim() == _Phone.Trim());
            if (((DateTime.Now.Year - Convert.ToDateTime(_birthday).Year == 18) && (DateTime.Now.Month >= Convert.ToDateTime(_birthday).Month) && (DateTime.Now.Day >= Convert.ToDateTime(_birthday).Day)) || ((DateTime.Now.Year - Convert.ToDateTime(_birthday).Year > 18)))
            {

            }
            else
            {
                return 5;
            }
            if (check1 != null)
            {
                return 3;
            }
            if (check2 != null)
            {
                return 4;
            }

            if (check1 == null && check2 == null)
            {

                candidate.Username = _Username;
                candidate.Password = Common.Encryptor.MD5Hash(_Password); ;
                candidate.RoleID = _RoleID;
                candidate.Email = _Email;
                candidate.Name = _Name;
                candidate.Gender = _Gender;
                candidate.Email = _Email;
                candidate.Address = _Address;
                candidate.Birthday = Convert.ToDateTime(_birthday);
                candidate.Phone = _Phone;
                candidate.TestStatus = 0;
                candidate.Education_Details = _EducationDetail;
                candidate.Work_Experience = _WorkExp;
                if (_Testcode != 0)
                {


                    candidate.TestCode = _Testcode;
                }
                else
                {
                    candidate.TestCode = 0;
                }

                try
                {
                    db.Candidates.Add(candidate);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return 0;
                }
            }
            else
            {
                return 0;
            }
            return 1;
        }
        public bool DeleteCandidate(int id)
        {
            try
            {
                var delete = (from x in db.Candidates where x.CandidateID == id select x).Single();
                db.Candidates.Remove(delete);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public Candidate GetCandidate(int id)
        {
            Candidate cand = new Candidate();
            try
            {
                cand = db.Candidates.SingleOrDefault(x => x.CandidateID == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return cand;
        }
        public int EditCandidate(int CandidateID, string _Address, string _Password, string _Email, string _Name, string _Gender, string _birthday, string _Phone, string _EducationDetail, string _WorkExp)
        {
            if (((DateTime.Now.Year - Convert.ToDateTime(_birthday).Year == 18) && (DateTime.Now.Month >= Convert.ToDateTime(_birthday).Month) && (DateTime.Now.Day >= Convert.ToDateTime(_birthday).Day)) || ((DateTime.Now.Year - Convert.ToDateTime(_birthday).Year > 18)))
            {

            }
            else
            {
                return 5;
            }
            //  Candidate check = db.Candidates.FirstOrDefault(c => c.Username.Trim() == _Username.Trim());
            if (_Email.Trim() != "")
            {
                Candidate check1 = db.Candidates.FirstOrDefault(c => c.Email.Trim() == _Email.Trim());
                if (check1 == null)
                {
                    Candidate check2 = db.Candidates.FirstOrDefault(c => c.Phone.Trim() == _Phone.Trim());
                    if (check2 == null)
                    {
                        try
                        {
                            var update = (from x in db.Candidates where x.CandidateID == CandidateID select x).Single();
                            update.Username = _Email;
                            update.Address = _Address;
                            update.RoleID = 3;
                            update.Email = _Email;
                            update.Gender = _Gender;
                            update.Name = _Name;
                            //  update.Password = _Password;
                            if (_Phone.Trim() != "")
                            {
                                update.Phone = _Phone;
                            }
                            update.Education_Details = _EducationDetail;
                            update.Work_Experience = _WorkExp;
                            if (_Password.Trim() != "")
                                update.Password = Common.Encryptor.MD5Hash(_Password);


                            update.Birthday = Convert.ToDateTime(_birthday);

                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            return 0;
                        }
                    }
                    else
                    {
                        return 4;//bao trung Phone
                    }
                }
                else
                {
                    return 3;//bao trung Email
                }
            }
            else
            {
                Candidate check2 = db.Candidates.FirstOrDefault(c => c.Phone.Trim() == _Phone.Trim());
                if (check2 == null)
                {
                    try
                    {
                        var update = (from x in db.Candidates where x.CandidateID == CandidateID select x).Single();

                        update.Address = _Address;
                        update.RoleID = 3;

                        update.Gender = _Gender;
                        update.Name = _Name;
                        //  update.Password = _Password;
                        if (_Phone.Trim() != "")
                        {
                            update.Phone = _Phone;
                        }
                        update.Education_Details = _EducationDetail;
                        update.Work_Experience = _WorkExp;
                        if (_Password.Trim() != "")
                            update.Password = Common.Encryptor.MD5Hash(_Password);
                        update.Birthday = Convert.ToDateTime(_birthday);

                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return 0;
                    }
                }
                else
                {
                    return 4;//bao trung Phone
                }
            }
            return 1;
        }

        //=======khoa==========
        public bool CreateTest(Test t)
        {
            t.TimeCreate = DateTime.Now;
            t.CreateBy = user.ID;
            if (t.TimeStart < DateTime.Now)
            {
                return false;
            }
            else
            {
                db.Tests.Add(t);
                db.SaveChanges();
                return true;
            }
        }

        public bool EditTest(Test t)
        {
            if (t.TimeStart < DateTime.Now)
            {
                return false;
            }
            else
            {
                var check = db.Tests.Find(t.TestCode);
                if (check != null)
                {
                    check.TimeStart = t.TimeStart;
                    check.Note = t.Note;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //==========================


        //=============================================================Test===============
        public void addQues(int id, int testques)
        {
            var test = new TestQuestion();
            test.TestCode = id;
            test.QuestionID = testques;

            db.TestQuestions.Add(test);
            db.SaveChanges();
        }
        public int AddQFT(int id)
        {
            var test = new TestQuestion();
            test.TestCode = id;
            /*   var rand=new Random();
               var contact = db.Questions.OrderBy(c => rand.Next()).Select(c => c.QuestionID).Take(5);*/

            var p = (from post in db.Questions
                     where post.TypeID == 1
                     select post.QuestionID).ToArray(); // Chọn dữ liệu từ LinQ

            // Tiến hành random và chọn 5 record đầu tiên
            //   dataSet1BindingSource.DataSource = postRD;
            int ChuoiSo = p.Length;
            int[] mArray = new int[ChuoiSo];
            int[] result = new int[5];
            for (int i = 0; i < p.Length; i++)
            {//tạo dãy số
                mArray[i] = i;
            }
            Random r = new Random();
            for (int i = 0; i < result.Length; i++, ChuoiSo--)
            {
                int index = r.Next(ChuoiSo - 1);
                result[i] = mArray[index]; //chọn số ngẫu nhiên
                mArray[index] = mArray[ChuoiSo - 1];//mảng số còn lại.
            }

            addQues(id, p[result[0]]);
            addQues(id, p[result[1]]);
            addQues(id, p[result[2]]);
            addQues(id, p[result[3]]);
            addQues(id, p[result[4]]);

            // ====================================================

            var p_ = (from post in db.Questions
                      where post.TypeID == 2
                      select post.QuestionID).ToArray(); // Chọn dữ liệu từ LinQ

            // Tiến hành random và chọn 5 record đầu tiên
            //   dataSet1BindingSource.DataSource = postRD;
            int ChuoiSo_ = p_.Length;
            int[] mArray_ = new int[ChuoiSo_];
            int[] result_ = new int[5];
            for (int i = 0; i < p_.Length; i++)
            {//tạo dãy số
                mArray_[i] = i;
            }
            Random r_ = new Random();
            for (int i = 0; i < result_.Length; i++, ChuoiSo_--)
            {
                int index = r.Next(ChuoiSo_ - 1);
                result_[i] = mArray_[index]; //chọn số ngẫu nhiên
                mArray_[index] = mArray_[ChuoiSo_ - 1];//mảng số còn lại.
            }

            addQues(id, p_[result_[0]]);
            addQues(id, p_[result_[1]]);
            addQues(id, p_[result_[2]]);
            addQues(id, p_[result_[3]]);
            addQues(id, p_[result_[4]]);


            //=============================================================

            var _p = (from post in db.Questions
                      where post.TypeID == 3
                      select post.QuestionID).ToArray(); // Chọn dữ liệu từ LinQ

            // Tiến hành random và chọn 5 record đầu tiên
            //   dataSet1BindingSource.DataSource = postRD;
            int _ChuoiSo = _p.Length;
            int[] _mArray = new int[_ChuoiSo];
            int[] _result = new int[5];
            for (int i = 0; i < _p.Length; i++)
            {//tạo dãy số
                _mArray[i] = i;
            }
            Random _r = new Random();
            for (int i = 0; i < _result.Length; i++, _ChuoiSo--)
            {
                int index = _r.Next(_ChuoiSo - 1);
                _result[i] = _mArray[index]; //chọn số ngẫu nhiên
                _mArray[index] = _mArray[_ChuoiSo - 1];//mảng số còn lại.
            }

            addQues(id, _p[_result[0]]);
            addQues(id, _p[_result[1]]);
            addQues(id, _p[_result[2]]);
            addQues(id, _p[_result[3]]);
            addQues(id, _p[_result[4]]);

            return 1;
        }

        public List<Candidate> GetCandidateForTest()
        {

            return db.Candidates.Where(x => x.TestCode == 0).ToList();
        }
        public bool addForTest(int idTestCode, int idCandidate)
        {
            var testForQuest = new TestCandidateList();
            testForQuest.TestCode = idTestCode;
            testForQuest.CandidateID = idCandidate;

            db.TestCandidateLists.Add(testForQuest);
            db.SaveChanges();


            var update = (from x in db.Candidates where x.CandidateID == idCandidate select x).Single();
            update.TestCode = idTestCode;
            db.SaveChanges();
            return true;
        }


        public List<Candidate> GetCandidateToDelete(int id)
        {
            return db.Candidates.Where(x => x.TestCode == id).ToList();
        }
        public bool DeleteCFTC(int idCandidate)
        {

            Candidate check = db.Candidates.SingleOrDefault(c => c.CandidateID == idCandidate);
            if (check.TestStatus == 0)
            {
                var delete = (from x in db.TestCandidateLists where x.CandidateID == idCandidate select x).Single();
                db.TestCandidateLists.Remove(delete);
                db.SaveChanges();

                var update = (from x in db.Candidates where x.CandidateID == idCandidate select x).Single();
                update.TestCode = 0;
                db.SaveChanges();

            }
            else
            {
                return false;
            }

            return true;

        }


        //=====================Test View===================
        public Test GetTestView(int id)
        {
            Test cand = new Test();
            try
            {
                cand = db.Tests.SingleOrDefault(x => x.TestCode == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return cand;
        }

        public bool DeleteTestView(int id)
        {

            try
            {
                TestQuestion checkTestQuest = db.TestQuestions.FirstOrDefault(c => c.TestCode == id);
                if (checkTestQuest != null)
                {


                    for (int i = 0; i < 15; i++)
                    {
                        TestQuestion checkTestQuestDEL = db.TestQuestions.FirstOrDefault(c => c.TestCode == id);
                        var deleteTestQuestion = (from x in db.TestQuestions where x.ID == checkTestQuestDEL.ID select x).Single();
                        db.TestQuestions.Remove(deleteTestQuestion);
                        db.SaveChanges();
                    }
                }

                var delete = (from x in db.Tests where x.TestCode == id select x).Single();
                db.Tests.Remove(delete);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
    }
}