using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAptitudeTest.Models
{
    public class QuestionData
    {
        OnlineAptitudeTestEntities context = new OnlineAptitudeTestEntities();
        public List<Question> GetQuestions()
        {
            return context.Questions.ToList();
        }


        public Question GetQuestion(int id)
        {
            return context.Questions.Find(id);
        }
        public bool Create(int id_subject, int unit, string content, string answer_a, string answer_b, string answer_c, string answer_d, string correct_answer)
        {
            var question = new Question();
            question.QuestionID = id_subject;
            question.TypeID = unit;
            question.QuestionDetails = content;
            question.Answer_a = answer_a;
            question.Answer_b = answer_b;
            question.Answer_c = answer_c;
            question.Answer_d = answer_d;
            question.CorrectAnswer = correct_answer;
            question.TimeStamp = DateTime.Now;
            try
            {
                context.Questions.Add(question);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public bool Update(int QuestionID, int unit, string content, string answer_a, string answer_b, string answer_c, string answer_d, string correct_answer)
        {
            var question = (from Question in context.Questions where Question.QuestionID == QuestionID select Question).Single();
            question.QuestionID = QuestionID;
            question.TypeID = unit;
            question.QuestionDetails = content;
            question.Answer_a = answer_a;
            question.Answer_b = answer_b;
            question.Answer_c = answer_c;
            question.Answer_d = answer_d;
            question.CorrectAnswer = correct_answer;
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public bool DeleteQuestion(int id)
        {
            try
            {
                var delete = (from x in context.Questions where x.QuestionID == id select x).Single();
                context.Questions.Remove(delete);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public List<QuestionType> ListAll()
        {
            return context.QuestionTypes.ToList();
        }
    }
}