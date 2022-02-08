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
    /// <summary>
    /// ////my latest code
    /// </summary>
    public class AnswerOptionsController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: AnswerOptions
        public ActionResult Index()
        {
            var answerOptions = db.AnswerOptions.Include(a => a.Question);
            return View(answerOptions.ToList());
        }

        // GET: AnswerOptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnswerOption answerOption = db.AnswerOptions.Find(id);
            if (answerOption == null)
            {
                return HttpNotFound();
            }
            return View(answerOption);
        }

        // GET: AnswerOptions/Create
        public ActionResult Create()
        {
            ViewBag.questionId = new SelectList(db.Questions, "questionId", "question_Title");
            return View();
        }

        // POST: AnswerOptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "optionId,optionString,questionId")] AnswerOption answerOption)
        {
            if (ModelState.IsValid)
            {
                db.AnswerOptions.Add(answerOption);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.questionId = new SelectList(db.Questions, "questionId", "question_Title", answerOption.questionId);
            return View(answerOption);
        }

        // GET: AnswerOptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnswerOption answerOption = db.AnswerOptions.Find(id);
            if (answerOption == null)
            {
                return HttpNotFound();
            }
            ViewBag.questionId = new SelectList(db.Questions, "questionId", "question_Title", answerOption.questionId);
            return View(answerOption);
        }

        // POST: AnswerOptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "optionId,optionString,questionId")] AnswerOption answerOption)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answerOption).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.questionId = new SelectList(db.Questions, "questionId", "question_Title", answerOption.questionId);
            return View(answerOption);
        }

        // GET: AnswerOptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnswerOption answerOption = db.AnswerOptions.Find(id);
            if (answerOption == null)
            {
                return HttpNotFound();
            }
            return View(answerOption);
        }

        // POST: AnswerOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AnswerOption answerOption = db.AnswerOptions.Find(id);
            db.AnswerOptions.Remove(answerOption);
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
    }
}
