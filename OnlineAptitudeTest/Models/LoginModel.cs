using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAptitudeTest.Models
{
    public class LoginModel
    {
        OnlineAptitudeTestEntities db = new OnlineAptitudeTestEntities();

        public string Username { get; set; }
        public string Password { get; set; }


        public bool CheckLogin(LoginModel model)
        {
            model.Password = Common.Encryptor.MD5Hash(model.Password);
            try
            {
                if (Convert.ToBoolean(db.AdminManagers.First(x => x.Username == model.Username && x.Password == model.Password).AdminManagerID))
                {
                    SetAdminSession(db.AdminManagers.First(x => x.Username == model.Username && x.Password == model.Password).AdminManagerID);
                    return true;
                }
            }
            catch (Exception) { }
            try
            {
                if (Convert.ToBoolean(db.AdminManagers.First(x => x.Username == model.Username && x.Password == model.Password).AdminManagerID))
                {
                    SetManagerSession(db.AdminManagers.First(x => x.Username == model.Username && x.Password == model.Password).AdminManagerID);
                    return true;
                }
            }
            catch (Exception) { }
            try
            {
                if (Convert.ToBoolean(db.Candidates.First(x => x.Username == model.Username && x.Password == model.Password).CandidateID))
                {
                    SetCandidateSession(db.Candidates.First(x => x.Username == model.Username && x.Password == model.Password).CandidateID);
                    return true;
                }
            }
            catch (Exception) { }
            return false;
        }

        public void SetAdminSession(int userID)
        {
            AdminManager user = db.AdminManagers.SingleOrDefault(x => x.AdminManagerID == userID);
            HttpContext.Current.Session.Add(Common.UserSession.ISLOGIN, true);
            HttpContext.Current.Session.Add(Common.UserSession.ID, user.AdminManagerID);
            HttpContext.Current.Session.Add(Common.UserSession.ROLEID, user.RoleID);
            HttpContext.Current.Session.Add(Common.UserSession.USERNAME, user.Username);
            HttpContext.Current.Session.Add(Common.UserSession.EMAIL, user.Email);
            HttpContext.Current.Session.Add(Common.UserSession.NAME, user.Name);
        }
        public void SetManagerSession(int userID)
        {
            AdminManager user = db.AdminManagers.SingleOrDefault(x => x.AdminManagerID == userID);
            HttpContext.Current.Session.Add(Common.UserSession.ISLOGIN, true);
            HttpContext.Current.Session.Add(Common.UserSession.ID, user.AdminManagerID);
            HttpContext.Current.Session.Add(Common.UserSession.ROLEID, user.RoleID);
            HttpContext.Current.Session.Add(Common.UserSession.USERNAME, user.Username);
            HttpContext.Current.Session.Add(Common.UserSession.EMAIL, user.Email);
            HttpContext.Current.Session.Add(Common.UserSession.NAME, user.Name);
        }
        public void SetCandidateSession(int userID)
        {
            Candidate user = db.Candidates.SingleOrDefault(x => x.CandidateID == userID);
            HttpContext.Current.Session.Add(Common.UserSession.ISLOGIN, true);
            HttpContext.Current.Session.Add(Common.UserSession.ID, user.CandidateID);
            HttpContext.Current.Session.Add(Common.UserSession.ROLEID, user.RoleID);
            HttpContext.Current.Session.Add(Common.UserSession.USERNAME, user.Username);
            HttpContext.Current.Session.Add(Common.UserSession.EMAIL, user.Email);
            HttpContext.Current.Session.Add(Common.UserSession.NAME, user.Name);
            HttpContext.Current.Session.Add(Common.UserSession.TESTCODE, user.TestCode);
            HttpContext.Current.Session.Add(Common.UserSession.TESTSTATUS, user.TestStatus);
        }
    }
}