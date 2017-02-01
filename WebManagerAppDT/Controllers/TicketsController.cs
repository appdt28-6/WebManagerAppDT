﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebManagerAppDT.Models;

namespace WebManagerAppDT.Controllers
{
    public class TicketsController : Controller
    {
        private AppDTEntities db = new AppDTEntities();

        public ActionResult Tickets()
        {
            if (Session["user_id"] != null)
            {
                var total = db.ps_VentasTotal().ToList();
                ViewData["total"] = total[0].Value;
                return View();
            }
            else
            {
                return Redirect("~/Home/Login");
            }
        }

        public ActionResult TICKETS_PV_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<ps_TicketsTotal_Result> tickets_pv = db.ps_TicketsTotal().ToList().AsQueryable();
            DataSourceResult result = tickets_pv.ToDataSourceResult(request, tICKETS_PV => new {
                Ticket_Id = tICKETS_PV.Ticket_Id,
                Ticket_Date = tICKETS_PV.Ticket_Date,
                Ticket_Subtotal = tICKETS_PV.Ticket_Subtotal,
                Ticket_Factura = tICKETS_PV.Ticket_Factura,
                Sucu_Id = tICKETS_PV.Sucu_Id,
                Ticket_Status = tICKETS_PV.Ticket_Status
            });

            return Json(result);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    
        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
