using Newtonsoft.Json;
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
    public class ResponsesController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: Responses

        public ActionResult Index()
        {
            var responses = db.Responses.Include(r => r.Responder);

            Response responseInst = new Response();
            



            return View(responses.ToList());
            
            
            
        
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        // GET: Responses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response response = db.Responses.Find(id);
            if (response == null)
            {
                return HttpNotFound();
            }
            return View(response);
        }

        // GET: Responses/Create
        public ActionResult Create()
        {
            ViewBag.responderId = new SelectList(db.Responders, "Id", "responder1");
            return View();
        }

        // POST: Responses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "questionId,optionId,responseId,responderId")] Response response)
        {
            if (ModelState.IsValid)
            {
                db.Responses.Add(response);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.responderId = new SelectList(db.Responders, "Id", "responder1", response.responderId);
            return View(response);
        }

        // GET: Responses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response response = db.Responses.Find(id);
            if (response == null)
            {
                return HttpNotFound();
            }
            ViewBag.responderId = new SelectList(db.Responders, "Id", "responder1", response.responderId);
            return View(response);
        }

        // POST: Responses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "questionId,optionId,responseId,responderId")] Response response)
        {
            if (ModelState.IsValid)
            {
                db.Entry(response).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.responderId = new SelectList(db.Responders, "Id", "responder1", response.responderId);
            return View(response);
        }

        public ActionResult GetReports(int? id)
        {
            Response res = new Response();
            
            //var id_parse = id;
            //res.responseId= Int32.Parse(id_parse.ToString());
            //foreach (Response q in db.Questions)
            //{
            //    //res.Responder = (from r in db.Responses.Where(ans => ans.questionId = res.optionId) select r);
            //    //res.Responder = (from r in db.Responses Where ans => ans.questionId = res.optionId) select r);

            //    res.Responder = (from data in db.Responses
            //                     where data.questionId = 143
            //                        select data).FirstOrDefault();

            //    //answer.Question = (from r in db.Responses.Where(ans => ans.optionId = answer.optionId) select r).ToList();
            //}
            return View();
        }


        // GET: Responses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response response = db.Responses.Find(id);
            if (response == null)
            {
                return HttpNotFound();
            }
            return View(response);
        }

        // POST: Responses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Response response = db.Responses.Find(id);
            db.Responses.Remove(response);
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
