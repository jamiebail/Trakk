using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using API.Models;

namespace API.Controllers
{
    public class GameReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GameReports
        public ActionResult Index()
        {
            return Json(db.Results.ToList());
        }

        // GET: GameReports/Details/5
      
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
