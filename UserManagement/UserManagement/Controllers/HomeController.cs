using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        private Database1Entities db = new Database1Entities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult PollsterIndex()
        {
            if (Session["UserId"] != null)
            {
                string userIdfromSession = Session["UserId"].ToString();
                int userId = Int32.Parse(userIdfromSession);
           
                List<Survey> Listsurveys = new List<Survey>();
                Listsurveys = (from s in db.Surveys where s.userId==userId  orderby s.SurveyId descending select s).Take(5).ToList();
                ViewBag.SurveysList = Listsurveys;
                //List<User> recentSurveys = new List<User>();
                //recentSurveys=(from s in db.Users where s.type == "Pollster" orderby s.Id descending select s).Take(5).ToList();
                //ViewBag.recentSurveyyy = recentSurveys;
                List<Feedback> feedbacks = new List<Feedback>();
                feedbacks = (from s in db.Feedbacks orderby s.feedbackId descending select s).Take(5).ToList();
                ViewBag.FeedbackList = feedbacks;
                /////
                var pollsterCount = (from s in db.Users where s.type == "Pollster" select s).Count();
                var surveyCount = (from s in db.Surveys where s.userId == userId select s).Count();
                var responseCount = (from r in db.Responses select r).Count();
                var responderCount = (from rs in db.Responders select rs).Count();
                var feedbackCount = (from f in db.Feedbacks where f.userId == userId select f).Count();
                var onlineCount = (Session["UserId"].ToString()).Count();
                var recentSurvey = (from s in db.Surveys select s).Take(5).ToList();
                //int responderTriggerValue1 = 0;

                //var responderTrigger = (from s in db.Surveys where s.userId == userId select s.SurveyId).ToList();
            
                //foreach (var u1 in responderTrigger)
                //{
                //    var responderinresponse = (from q3 in db.Questions where  q3.surveyid== u1 select q3.questionId).ToList();
                //    foreach (var q3 in responderinresponse)
                //    {
                //        var responderinresponse23 = (from q4 in db.Responses where q4.questionId == q3 select q3).ToList().Count;
                //        if (responderinresponse23 != 0)
                //        {
                //            responderTriggerValue1 = responderTriggerValue1 + responderinresponse23;
                //        }
                       
                //    }
               

                //}
                //ViewBag.recentPollsterTriggerCount = responderCount;
                ViewBag.recentSurvey = recentSurvey;
                ViewBag.surveyCounter = surveyCount;
                ViewBag.responseCount = responseCount;
                ViewBag.responderCount = responderCount;
                ViewBag.feedbackCount = feedbackCount;
                ViewBag.onlineCount = onlineCount;
                ViewBag.pollcount = pollsterCount;
                return View();
            }
            return View();
        }
        public ActionResult AdminIndex()
        {
            return View();
        }

        public ActionResult AdminPage()
        {
            if (Session["UserId"] != null)
            {
                string userIdfromSession = Session["UserId"].ToString();
                int surveyID = Int32.Parse(userIdfromSession);

                List<Survey> Listsurveys = new List<Survey>();
                Listsurveys = (from s in db.Surveys orderby s.SurveyId descending select s).Take(5).ToList();
                ViewBag.SurveysList = Listsurveys;
                //List<User> recentSurveys = new List<User>();
                //recentSurveys=(from s in db.Users where s.type == "Pollster" orderby s.Id descending select s).Take(5).ToList();
                //ViewBag.recentSurveyyy = recentSurveys;
                List<Feedback> feedbacks = new List<Feedback>();
                feedbacks = (from s in db.Feedbacks orderby s.feedbackId descending select s).Take(5).ToList();
                ViewBag.FeedbackList = feedbacks;
                /////
                var pollsterCount= (from s in db.Users where s.type=="Pollster" select s).Count();
                var surveyCount = (from s in db.Surveys select s).Count();
                var responseCount = (from r in db.Responses select r).Count();
                var responderCount = (from rs in db.Responders select rs).Count();
                var feedbackCount = (from f in db.Feedbacks select f).Count();
                var onlineCount = (Session["UserId"].ToString()).Count();
                var recentSurvey = (from s in db.Surveys select s).Take(5).ToList();
                ViewBag.recentSurvey = recentSurvey;
                ViewBag.surveyCounter = surveyCount;
                ViewBag.responseCount = responseCount;
                ViewBag.responderCount = responderCount;
                ViewBag.feedbackCount = feedbackCount;
                ViewBag.onlineCount = onlineCount;
                ViewBag.pollcount = pollsterCount;
                return View();
            }
            return View();
        }
    }
}