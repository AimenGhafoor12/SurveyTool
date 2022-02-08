using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Models
{
    public class DisplaySurveyQuestions
    {
        public Nullable<int> surveyid { get; set; }
        public int questionId { get; set; }
        public String questionType { get; set; }
        public String answerOption { get; set; }
        public int optionId { get; set; }
        public int anw_questionId { get; set; }
        public String question_Title { get; set; }
    }
}