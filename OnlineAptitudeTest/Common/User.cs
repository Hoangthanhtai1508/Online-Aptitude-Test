using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAptitudeTest.Common
{
    public class User
    {
        public bool ISLOGIN { get; set; } = false;
        public int ID { get; set; }
        public int ROLEID { get; set; }
        public string USERNAME { get; set; }
        public string EMAIL { get; set; }
        public string NAME { get; set; }
        public int TESTCODE { get; set; } = 0;
        public int TESTSTATUS { get; set; } = 0;
        public int PASS { get; set; } = 0;

        public User()
        {
            try
            {
                ISLOGIN = (bool)HttpContext.Current.Session[UserSession.ISLOGIN];
                ID = (int)HttpContext.Current.Session[UserSession.ID];
                ROLEID = (int)HttpContext.Current.Session[UserSession.ROLEID];
                USERNAME = (string)HttpContext.Current.Session[UserSession.USERNAME];
                EMAIL = (string)HttpContext.Current.Session[UserSession.EMAIL];
                NAME = (string)HttpContext.Current.Session[UserSession.NAME];
                TESTCODE = (int)HttpContext.Current.Session[UserSession.TESTCODE];
                TESTSTATUS = (int)HttpContext.Current.Session[UserSession.TESTSTATUS];
                PASS = (int)HttpContext.Current.Session[UserSession.PASS];
            }
            catch (Exception) { }
        }

        public bool IsLogin()
        {
            try
            {
                if (ISLOGIN)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void Reset()
        {
            HttpContext.Current.Session.Clear();
        }

        public bool IsAdmin()
        {
            try
            {
                if (ISLOGIN && ROLEID == 1)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsManager()
        {
            try
            {
                if (ISLOGIN && ROLEID == 2)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsCandidate()
        {
            try
            {
                if (ISLOGIN && ROLEID == 3)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}