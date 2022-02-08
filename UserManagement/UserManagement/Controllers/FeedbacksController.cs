using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class FeedbacksController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: Feedbacks

        public ActionResult Index()
        {
            var feedbacks = db.Feedbacks.Include(f => f.Responder).Include(f => f.User);
            return View(feedbacks.ToList());
        }

        // GET: Feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Feedbacks/Create
        //public ActionResult Create()
        //{
        //    ViewBag.responsderId = new SelectList(db.Responders, "Id", "responder1");
        //    ViewBag.userId = new SelectList(db.Users, "Id", "username");
        //    return View();
        //}

        // POST: Feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "feedbackId,feedbackString,rating,userId,responsderId")] Feedback feedback)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string idfromSession = Session["UserId"].ToString();
        //        int userrid = Int32.Parse(idfromSession);
        //        feedback.userId = userrid;
        //        feedback.responsderId = null;

        //        db.Feedbacks.Add(feedback);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.responsderId = new SelectList(db.Responders, "Id", "responder1", feedback.responsderId);
        //    ViewBag.userId = new SelectList(db.Users, "Id", "username", feedback.userId);
        //    return View(feedback);
        //}

        // GET: Feedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.responsderId = new SelectList(db.Responders, "Id", "responder1", feedback.responsderId);
            ViewBag.userId = new SelectList(db.Users, "Id", "username", feedback.userId);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "feedbackId,feedbackString,rating,userId,responsderId")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.responsderId = new SelectList(db.Responders, "Id", "responder1", feedback.responsderId);
            ViewBag.userId = new SelectList(db.Users, "Id", "username", feedback.userId);
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
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

        public ActionResult GetAllFeedback()
        {
            var feedbackList = (from f in db.Feedbacks select f);
            return View(feedbackList.ToList());
        }

        public ActionResult CreateFeedback()
        {
            if (Session["UserId"] != null && Session["UserType"].ToString() == "Pollster")
            {
                ViewBag.responsderId = new SelectList(db.Responders, "Id", "responder1");
                ViewBag.userId = new SelectList(db.Users, "Id", "username");
                return View();
            }
            return RedirectToAction("Login", "Users");

        }
        [HttpPost]
        public ActionResult CreateFeedback( String feedbackString, int rating)
        {
            if (Session["UserId"] != null && Session["UserType"].ToString() == "Pollster")
            {
                if (ModelState.IsValid)
                {
                    Feedback feedback = new Feedback();

                    string idfromSession = Session["UserId"].ToString();
                    int userrid = Int32.Parse(idfromSession);
                    feedback.userId = userrid;
                    feedback.responsderId = null;
                    feedback.rating = rating;
                    feedback.feedbackString = feedbackString;
                    db.Feedbacks.Add(feedback);
                    db.SaveChanges();
                    return RedirectToAction("PollsterIndex", "Home");

                }
                else
                {
                    ModelState.Clear();
                    ViewBag.Message = "Feedback is successfully saved";
                    return RedirectToAction("Login", "Users");
                }


            }
            return View();

        }

    }
}

