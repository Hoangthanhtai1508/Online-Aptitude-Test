using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAptitudeTest.Models
{
    public class CandidateTestViewModel
    {
        public Test test { get; set; }
        public Question question { get; set; }
        public Candidate_Test_Detail candidate_test { get; set; }
        public TestQuestion test_question { get; set; }
    }
}