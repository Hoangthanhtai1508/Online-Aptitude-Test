using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineAptitudeTest.Models
{
    [MetadataTypeAttribute(typeof(AdminManagerMetadata))]
    public partial class AdminManager
    {
        internal sealed class AdminManagerMetadata
        {




            public int AdminManagerID { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public int RoleID { get; set; }


            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Email { get; set; }
            public string Name { get; set; }
            public string Gender { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            public System.DateTime Birthday { get; set; }
            public string Phone { get; set; }
            public Nullable<System.DateTime> LastLogin { get; set; }
        }
    }
}