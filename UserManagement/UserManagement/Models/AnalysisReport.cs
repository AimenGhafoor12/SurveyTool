using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Models
{
    public class AnalysisReport
    {
        public Nullable<int> surveyid { get; set; }
        public int questionId { get; set; }
        public String question_Title { get; set; }
        public int optionId { get; set; }
        public String answerOption { get; set; }

        public Decimal ResponsePercentage { get; set; }

        public int optionIdCount { get; set; }

        public int responses { get; set; }

    }
}