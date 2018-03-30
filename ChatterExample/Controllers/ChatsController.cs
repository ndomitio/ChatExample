﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChatterExample;
using Newtonsoft.Json;

namespace ChatterExample.Controllers
{
    public class ChatsController : Controller
    {
        private ChatExEntities db = new ChatExEntities();

        // GET: Chats
        public ActionResult Index()
        {
            var chats = db.Chats.Include(c => c.AspNetUser);
            return View(chats.ToList());
        }




        public JsonResult TestJson()
        {
            //string jsonTest = "{ \"firstName\": \"Bob\",\"lastName\": \"Sauce\", \"children\": [{\"firstName\": \"Barbie\", \"age\": 19 },{\"firstName\": \"Ron\", \"age\": null }] }";

            //return Json(jsonTest, JsonRequestBehavior.AllowGet);

            //SQL Statment HERE (mine is not shown since this is part of //your project grade).

            //Now, the LINQ equivalent is declared as a variable (below).
            var chats = from Chats in db.Chats
                        select new
                        {
                            Chats.Message,
                            Chats.AspNetUser.UserName
                        };

            //Next we serialize our data in the model to JSON(Newtonsoft //library at work). 
            //FYI: the query isn’t run until you call the ToList() method.

            var output = JsonConvert.SerializeObject(chats.ToList());

            //Finally, we return Json to the view
            return Json(output, JsonRequestBehavior.AllowGet);


        }


        // GET: Chats/Details/5
        public ActionResult Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Chat chat = db.Chats.Find(id);
                if (chat == null)
                {
                    return HttpNotFound();
                }
                return View(chat);
            }

            // GET: Chats/Create
            public ActionResult Create()
            {
                ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
                return View();
            }

        // POST: Chats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChatID,UserID,TimeStamp,Message")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Chats.Add(chat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", chat.UserID);
            return View(chat);
        }

        // GET: Chats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", chat.UserID);
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChatID,UserID,TimeStamp,Message")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", chat.UserID);
            return View(chat);
        }

        // GET: Chats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chat chat = db.Chats.Find(id);
            db.Chats.Remove(chat);
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
