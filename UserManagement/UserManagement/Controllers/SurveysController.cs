using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class SurveysController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: Surveys
        public ActionResult Index()
        {
            if (Session["UserId"] != null && Session["UserType"].ToString() == "Pollster")
            {
                string userIdfromSession = Session["UserId"].ToString();
                int surveyID = Int32.Parse(userIdfromSession);
                var surveys = from s in db.Surveys where s.userId == surveyID select s;
                return View(surveys.ToList());
            }
            return RedirectToAction("Login", "Users");
        }

        // GET: Surveys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // GET: Surveys/Create
        public ActionResult Create()
        {
            if (Session["UserId"] != null && Session["UserType"].ToString() == "Pollster")
            {
                ViewBag.userId = new SelectList(db.Users, "Id", "username");

                return View();
            }
            return RedirectToAction("Login", "Users");
        }

        // POST: Surveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SurveyId,surveyName,Description,startData,endDate")] Survey survey)
        {
            if (Session["UserId"] != null && Session["UserType"].ToString() == "Pollster")
            {
                if (ModelState.IsValid)
                {
                    survey.userId = Int32.Parse(Session["UserId"].ToString());
                    db.Surveys.Add(survey);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.userId = new SelectList(db.Users, "Id", "username", survey.userId);
                return View(survey);
            }
            return RedirectToAction("Login", "Users");
        }

        // GET: Surveys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(db.Users, "Id", "username", survey.userId);
            return View(survey);
        }

        // POST: Surveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SurveyId,surveyName,Description,userId,startData,endDate")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(survey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userId = new SelectList(db.Users, "Id", "username", survey.userId);
            return View(survey);
        }

        // GET: Surveys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // POST: Surveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Survey survey = db.Surveys.Find(id);
            db.Surveys.Remove(survey);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Surveys/DesignSurvey/5
        public ActionResult DesignSurvey(int? id)
        {

            if (Session["UserId"] != null && Session["UserType"].ToString() == "Pollster") 
            {
                //var objDisplaySurveyQuestions;
                var id_param = id;
                Survey surveryInst = new Survey();
                surveryInst.SurveyId = Int32.Parse(id_param.ToString());
                // Question q2;
                surveryInst.Questions = (from q in db.Questions.Where(quest => quest.surveyid == id_param) select q).ToList();
                if (surveryInst.Questions != null)
                {
                    foreach (Question q in surveryInst.Questions)
                    {
                        q.AnswerOptions = (from a in db.AnswerOptions.Where(ans => ans.questionId == q.questionId) select a).ToList();

                    }
                }
                ViewBag.surveyInstance = surveryInst;


                ViewBag.results = surveryInst;

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Survey survey = db.Surveys.Find(id);
                if (survey == null)
                {
                    return HttpNotFound();
                }
                ViewBag.surveyName = survey.surveyName;
                ViewBag.surveyid = id;
                return View(surveryInst);
            }
            return RedirectToAction("Login", "Users");
        }

        public ActionResult GetAllSurveys()
        {

            return View();

        }
        [HttpPost]
        public JsonResult GetSurvy(int? id)
        {
            var id_param = id;
            Survey surveryInst = new Survey();
            surveryInst.SurveyId = Int32.Parse(id_param.ToString());
            surveryInst.Questions = (from q in db.Questions.Where(quest => quest.surveyid == id_param) select q).ToList();

            if (surveryInst.Questions != null)
            {
                foreach (Question q in surveryInst.Questions)
                {
                    q.AnswerOptions = (from a in db.AnswerOptions.Where(ans => ans.questionId == q.questionId) select a).ToList();
                }
            }
            Survey survey = db.Surveys.Find(id);

            ViewBag.surveyName = survey.surveyName;
            ViewBag.surveyid = id;
            return Json(surveryInst);

        }
        public ActionResult GetSurveyView(int? id)
        {
            if (id != null)
            {
                var id_param = id;
                Survey surveryInst = new Survey();
                surveryInst.SurveyId = Int32.Parse(id_param.ToString());
                // Question q2;
                surveryInst.Questions = (from q in db.Questions.Where(quest => quest.surveyid == id_param) select q).ToList();

                if (surveryInst.Questions != null)
                {
                    foreach (Question q in surveryInst.Questions)
                    {
                        q.AnswerOptions = (from a in db.AnswerOptions.Where(ans => ans.questionId == q.questionId) select a).ToList();
                        //ViewBag.surveyInstance = surveryInst;

                        //ViewBag.results = surveryInst;
                        //return View(surveryInst);
                    }
                }
                ViewBag.surveyInstance = surveryInst;


                ViewBag.results = surveryInst;

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Survey survey = db.Surveys.Find(id);
                if (survey == null)
                {
                    return HttpNotFound();
                }
                ViewBag.surveyName = survey.surveyName;
                ViewBag.surveyid = id;
                return View(surveryInst);
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }

        }


        [EncryptedActionParameter]
        [HttpPost] 
        public JsonResult SubmitResponse(String surveyId, String questionId, String optId)
        {

            try
            {
                Question question = new Question();

                int quesId = Int32.Parse(questionId);
                var option_Id = (from ans in db.AnswerOptions where ans.questionId == quesId && ans.optionString==optId select ans.optionId).FirstOrDefault();

                Responder responderObj = new Responder();
                responderObj.responder1 = DateTime.Now.Date.ToString();
                db.Responders.Add(responderObj);
                db.SaveChanges();


                int? responid = (
                                   from res in db.Responders
                                   orderby res.Id descending
                                   select res.Id
                               ).Take(1).SingleOrDefault();
                //    var resId = (from res in db.Responders select res.Id).Last().ToString();
                int responId = Int32.Parse(responid.ToString());
                Response response = new Response();
                response.optionId = option_Id;
                response.questionId = quesId;
                response.optionId = option_Id;
                response.responderId = responId;
                db.Responses.Add(response);
                db.SaveChanges();
               
               return Json("null");
               
            }


            catch (DbEntityValidationException e)
            {
                string ex = e.InnerException.ToString();
                throw e;
            }

        }
        [HttpPost]
        public JsonResult SubmitResponseforCheckbox(String surveyId, String questionId, String[] optId)
        {

            try
            {
                Question question = new Question();

                int quesId = Int32.Parse(questionId);
                Responder responderObj = new Responder();
                responderObj.responder1 = DateTime.Now.Date.ToString();
                db.Responders.Add(responderObj);
                db.SaveChanges();


                int? responid = (
                                   from res in db.Responders
                                   orderby res.Id descending
                                   select res.Id
                               ).Take(1).SingleOrDefault();
                //    var resId = (from res in db.Responders select res.Id).Last().ToString();
                int responId = Int32.Parse(responid.ToString());
                Response response = new Response();
                
                foreach (var value in optId)
                {
                    var option_Id = (from ans in db.AnswerOptions where ans.questionId == quesId && ans.optionString == value select ans.optionId).FirstOrDefault();
                    response.optionId = option_Id;
                    response.questionId = quesId;
                    response.optionId = option_Id;
                    response.responderId = responId;
                    db.Responses.Add(response);
                    db.SaveChanges();
                }

            
                
                return Json("null");
            }


            catch (DbEntityValidationException e)
            {
                string ex = e.InnerException.ToString();
                throw e;
            }

        }
        //public ActionResult generateReport()
        //{
        //    return View();
        //}
        //[HttpPost]

        //[HttpPost]
        public ActionResult generateReport(int id)
        {
           
                AnalysisReport ar = new AnalysisReport();
                Survey surveryInst = new Survey();
                List<AnalysisReport> ls = new List<AnalysisReport>();
                List<AnalysisReport> ls_new = new List<AnalysisReport>();
                ls_new.Add(new AnalysisReport() { questionId = -1 });
                List<AnalysisReport> ls_result_set = new List<AnalysisReport>();
                // Question q2;
                surveryInst.Questions = (from q in db.Questions.Where(quest => quest.surveyid == id) select q).ToList();
                if (surveryInst.Questions != null)
                {
                    foreach (Question q in surveryInst.Questions)
                    {
                        q.AnswerOptions = (from a in db.AnswerOptions.Where(ans => ((ans.questionId == q.questionId) && (ans.optionString != "12345678") && (ans.optionString != "default"))) select a).ToList();
                        int len = q.AnswerOptions.Count;
                        ResponseCounter rc = new ResponseCounter();

                        if (q.AnswerOptions != null)
                        {

                            int[] quesarray = new int[len];
                            int total_answers_given = 0;

                            total_answers_given = (from r in db.Responses.Where(ans => ans.questionId == q.questionId) select r).Count();

                            var resp = (from r in db.Responses.Where(ans => ans.questionId == q.questionId) select r).ToList();

                            string answer_title = string.Empty;

                            ls = (from r in db.Responses
                                  join answer in db.AnswerOptions on r.optionId equals answer.optionId
                                  where r.questionId == q.questionId
                                  group r by r.optionId into g
                                  select new AnalysisReport()
                                  {
                                      optionId = g.Key,
                                      answerOption = (from ao in db.AnswerOptions
                                                      where ao.optionId == g.Key
                                                      select ao.optionString
                                                      ).FirstOrDefault(),
                                      optionIdCount = g.Count(),
                                      questionId = q.questionId,
                                      question_Title = q.question_Title,
                                      responses = total_answers_given,
                                      ResponsePercentage = ((decimal)(g.Count()) / (total_answers_given)) * 100
                                  }).ToList();

                            ls_result_set.AddRange(ls);
                            ls_result_set.AddRange(ls_new); // for knowing that now previous question is finished
                        }
                    }
                }

                ViewBag.results = ls_result_set;

                if (id.ToString() == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Survey survey = db.Surveys.Find(id);
                if (survey == null)
                {
                    return HttpNotFound();
                }
                ViewBag.surveyName = survey.surveyName;
                ViewBag.surveyid = id;

                return View(surveryInst);
            
            
        }




















        public ActionResult ShareLink()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ShareLink(string username, string mylink)
        {
            if (Session["UserId"] != null) 
            {
                //ViewBag.value1 = "aimenghafoor81@gmail.com";
                if (ModelState.IsValid)
                {

                    string userIdfromSession = Session["UserId"].ToString();
                    int userID = Int32.Parse(userIdfromSession);
                    var surveys = from s in db.Surveys where s.userId == userID select s;
                    var value = (from s in db.Surveys where s.userId == userID && s.surveyName == mylink select s.SurveyId).FirstOrDefault();
                    try
                    {                                //localhost:56456
                        string mysurveyLink = "https://localhost:56456/Surveys/getSurveyView/" + value;
                        WebMail.Send(username, "Link", mysurveyLink, username, null, null, true, null, null, null, null, null, username);
                        ViewBag.successMsg = "Your survey has been shared successfully";
                        return RedirectToAction("PollsterIndex", "Home");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.erorMsg = "Your survey has not been shared successfully";
                        return RedirectToAction("PollsterIndex", "Home");
                    }

                }
                else
                {
                    ViewBag.erorMsg = "Your survey has not been shared successfully";
                }

            }
            return RedirectToAction("Login", "Users");
        }


        public ActionResult CreateQuestion(string question_Title, string surveyid, string questionType,
            string[] optionString)
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    Question question = new Question();
                    question.question_Title = question_Title;

                    question.surveyid = int.Parse(surveyid);
                    if (questionType == "Select")
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    question.questionType = questionType;
                    //question.questionId = "4";
                    if (ModelState.IsValid)
                    {
                        db.Questions.Add(question);
                        db.SaveChanges();

                        ViewBag.questionId = question.questionId;

                        AnswerOption answer = new AnswerOption();
                        answer.questionId = ViewBag.questionId;
                        for (int i = 0; i < optionString.Length; i++)
                        {
                            if (optionString[i] != "")
                            {
                                answer.optionString = optionString[i];
                                db.AnswerOptions.Add(answer);
                                db.SaveChanges();
                            }
                        }

                        //return View(question);


                        return RedirectToAction("DesignSurvey", new { id = surveyid });
                    }

                    //ViewBag.questionId = question.questionId;
                    return View(question);

                }
                catch (DbEntityValidationException e)
                {
                    string ex = e.InnerException.ToString();
                    throw e;
                }

            }
            return RedirectToAction("Login", "Users");

        }
        [HttpPost]
        public ActionResult searchPollsterbyname(string searchitembyname)
        {
            if (searchitembyname.Contains("") || searchitembyname.Contains("@@") || searchitembyname.Contains("!!"))
            {
                searchitembyname = "";
                ViewBag["searchmsg"] = "Did not match any documents.  Make sure that all words are spelled correctly.";
                return View();
            }
            var pollstername = (from u in db.Users where u.username == searchitembyname select u);
            var pollsternamevalue = pollstername.ToList();
            if (pollsternamevalue.Count == 0)
            {
                ViewBag["searchnamemsg"] = "Record does not exist";
                return View();
            }

            return View(pollstername.ToList());

        }



    }
    public class ResponseCounter : Response
    {
        public ResponseCounter()
        { }

        static private int countResponse { get; set; }

    }
}