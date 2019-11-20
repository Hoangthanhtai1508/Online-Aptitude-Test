using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAptitudeTest.Models
{
    public class ReportDal
    {
        OnlineAptitudeTestEntities db = new OnlineAptitudeTestEntities();

        public int CountCanTestFromTo(FormCollection form)
        {
            DateTime fromDate = DateTime.Parse(Convert.ToString(form["TuNgay"]));
            DateTime toDate = DateTime.Parse(Convert.ToString(form["DenNgay"]));

            List<ReportViewModel> list = new List<ReportViewModel>();
            try
            {
                list = (from c in db.Candidates
                        join l in db.TestCandidateLists on c.CandidateID equals l.CandidateID
                        join t in db.Tests on l.TestCode equals t.TestCode
                        where t.TimeStart.Day >= fromDate.Day &&
                        t.TimeStart.Month >= fromDate.Month &&
                        t.TimeStart.Year >= fromDate.Year &&
                        t.TimeStart.Day <= toDate.Day &&
                        t.TimeStart.Month <= toDate.Month &&
                        t.TimeStart.Year <= toDate.Year
                        select new ReportViewModel { candidate = c, candidateList = l, test = t }).ToList();
            }
            catch { }
            return list.Count();
        }

        public int CountCanPassedTestFromTo(FormCollection form)
        {
            DateTime fromDate = DateTime.Parse(Convert.ToString(form["TuNgay"]));
            DateTime toDate = DateTime.Parse(Convert.ToString(form["DenNgay"]));

            List<ReportViewModel> list = new List<ReportViewModel>();
            try
            {
                list = (from c in db.Candidates
                        join l in db.TestCandidateLists on c.CandidateID equals l.CandidateID
                        join t in db.Tests on l.TestCode equals t.TestCode
                        where t.TimeStart.Day >= fromDate.Day &&
                        t.TimeStart.Month >= fromDate.Month &&
                        t.TimeStart.Year >= fromDate.Year &&
                        t.TimeStart.Day <= toDate.Day &&
                        t.TimeStart.Month <= toDate.Month &&
                        t.TimeStart.Year <= toDate.Year &&
                        c.Pass == 1
                        select new ReportViewModel { candidate = c, candidateList = l, test = t }).ToList();
            }
            catch { }
            return list.Count();
        }
        //========================================================================================================
        public int CountCanTest(FormCollection form)
        {
            DateTime fromDate = DateTime.Parse(Convert.ToString(form["NgayChon"]));

            List<ReportViewModel> list = new List<ReportViewModel>();
            try
            {
                list = (from c in db.Candidates
                        join l in db.TestCandidateLists on c.CandidateID equals l.CandidateID
                        join t in db.Tests on l.TestCode equals t.TestCode
                        where t.TimeStart.Day == fromDate.Day &&
                        t.TimeStart.Month == fromDate.Month &&
                        t.TimeStart.Year == fromDate.Year
                        select new ReportViewModel { candidate = c, candidateList = l, test = t }).ToList();
            }
            catch { }
            return list.Count();
        }

        public int CountCanPassedTest(FormCollection form)
        {
            DateTime fromDate = DateTime.Parse(Convert.ToString(form["NgayChon"]));

            List<ReportViewModel> list = new List<ReportViewModel>();
            try
            {
                list = (from c in db.Candidates
                        join l in db.TestCandidateLists on c.CandidateID equals l.CandidateID
                        join t in db.Tests on l.TestCode equals t.TestCode
                        where t.TimeStart.Day == fromDate.Day &&
                        t.TimeStart.Month == fromDate.Month &&
                        t.TimeStart.Year == fromDate.Year &&
                        c.Pass == 1
                        select new ReportViewModel { candidate = c, candidateList = l, test = t }).ToList();
            }
            catch { }
            return list.Count();
        }
        //========================================================================================================
        public int CountCanTestMonth(FormCollection form)
        {
            DateTime fromDate = DateTime.Parse(Convert.ToString(form["ThangChon"]));

            List<ReportViewModel> list = new List<ReportViewModel>();
            try
            {
                list = (from c in db.Candidates
                        join l in db.TestCandidateLists on c.CandidateID equals l.CandidateID
                        join t in db.Tests on l.TestCode equals t.TestCode
                        where t.TimeStart.Month == fromDate.Month &&
                        t.TimeStart.Year == fromDate.Year
                        select new ReportViewModel { candidate = c, candidateList = l, test = t }).ToList();
            }
            catch { }
            return list.Count();
        }

        public int CountCanPassedTestMonth(FormCollection form)
        {
            DateTime fromDate = DateTime.Parse(Convert.ToString(form["ThangChon"]));

            List<ReportViewModel> list = new List<ReportViewModel>();
            try
            {
                list = (from c in db.Candidates
                        join l in db.TestCandidateLists on c.CandidateID equals l.CandidateID
                        join t in db.Tests on l.TestCode equals t.TestCode
                        where t.TimeStart.Month == fromDate.Month &&
                        t.TimeStart.Year == fromDate.Year &&
                        c.Pass == 1
                        select new ReportViewModel { candidate = c, candidateList = l, test = t }).ToList();
            }
            catch { }
            return list.Count();
        }
    }
}
