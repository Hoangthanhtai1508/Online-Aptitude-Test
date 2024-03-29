//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineAptitudeTest.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            this.Candidate_Test_Detail = new HashSet<Candidate_Test_Detail>();
            this.TestQuestions = new HashSet<TestQuestion>();
        }
    
        public int QuestionID { get; set; }
        public int TypeID { get; set; }
        public string QuestionDetails { get; set; }
        public string Answer_a { get; set; }
        public string Answer_b { get; set; }
        public string Answer_c { get; set; }
        public string Answer_d { get; set; }
        public string CorrectAnswer { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Candidate_Test_Detail> Candidate_Test_Detail { get; set; }
        public virtual QuestionType QuestionType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestQuestion> TestQuestions { get; set; }
    }
}
