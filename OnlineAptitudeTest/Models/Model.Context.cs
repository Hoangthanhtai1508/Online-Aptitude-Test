﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OnlineAptitudeTestEntities : DbContext
    {
        public OnlineAptitudeTestEntities()
            : base("name=OnlineAptitudeTestEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminManager> AdminManagers { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<Candidate_Test_Detail> Candidate_Test_Detail { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<TestCandidateList> TestCandidateLists { get; set; }
        public virtual DbSet<TestQuestion> TestQuestions { get; set; }
    }
}
