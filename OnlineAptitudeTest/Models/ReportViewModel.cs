using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAptitudeTest.Models
{
    public class ReportViewModel
    {
        public Candidate candidate { get; set; }
        public Test test { get; set; }
        public TestCandidateList candidateList { get; set; }
    }
}