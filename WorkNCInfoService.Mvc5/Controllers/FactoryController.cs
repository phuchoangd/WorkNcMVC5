﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WorkNCInfoService.Mvc5.Models.WorkModels;

namespace WorkNCInfoService.Mvc5.Controllers
{
    public class FactoryController : Controller
    {
        private const int pageSize = 10;
        WorkNCDbContext db = new WorkNCDbContext();
        // GET: Factory
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, bool isDeleted=false, int? page=1)
        {
            int pageNumber = (page ?? 1);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.NoSort = sortOrder == "No" ? "no_asc" : "no";
            if(searchString !=null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var factory = from s in db.WorkNC_Factory select s;

            //sort machine
            switch (sortOrder)
            {
                case "name_asc":
                    factory = factory.OrderBy(n => n.Name);
                    break;
                case "no_asc":
                    factory = factory.OrderBy(n => n.No);
                    break;
                default:
                    factory = factory.OrderBy(n => n.CompanyId);
                    break;
            }
            //search by Machine Name & isDeleted
            if (!String.IsNullOrEmpty(searchString))
            {
                if(isDeleted == false)
                {
                    return View(factory.Where(n => n.Name.Contains(searchString) && n.isDeleted.Equals(false)).ToPagedList(pageNumber, pageSize));
                }
                if(isDeleted == true)
                {
                    return View(factory.Where(n => n.Name.Contains(searchString)).ToPagedList(pageNumber, pageSize));
                }
            }
 
            
            return View(factory.Where(n =>n.isDeleted.Equals(isDeleted)).ToPagedList(pageNumber, pageSize));
        }

        // GET: Factory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Factory/Create
        public ActionResult Create()
        {
            return View();

        }

        // POST: Factory/Create
        [HttpPost]
        public ActionResult Create(WorkNC_Factory factory)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    factory.CreateDate = DateTime.Now;
                    //factory.CreateAccount = User.Identity.Name;
                    db.Entry(factory).State = EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(factory);              
            }
            catch
            {
                return View();
            }
        }

        // GET: Factory/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            WorkNC_Factory factory = db.WorkNC_Factory.Find(id);
            if (factory == null)
                return HttpNotFound();
            return View(factory);
        }

        // POST: Factory/Edit/5
        [HttpPost]
        public ActionResult Edit(WorkNC_Factory factory)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    factory.ModifiedAccount = User.Identity.Name;
                    factory.ModifiedDate = DateTime.Now;

                    db.Entry(factory).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(factory);
            }
            catch
            {
                return View();
            }
        }

        // GET: Factory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            WorkNC_Factory factory = db.WorkNC_Factory.Find(id);
            if (factory == null)
                return HttpNotFound();
            return View(factory);
        }

        // POST: Factory/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id, WorkNC_Factory factory)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    db.Entry(factory).State = EntityState.Deleted;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(factory);
            }
            catch
            {
                return View();
            }
        }
        public PartialViewResult Search()
        {
            return PartialView("_SearchFactory");
        }
        
    }
}
