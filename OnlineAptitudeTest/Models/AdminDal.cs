using OnlineAptitudeTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAptitudeTest.Models
{
    public class AdminDal
    {
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
            if(check != null)
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
    }
}