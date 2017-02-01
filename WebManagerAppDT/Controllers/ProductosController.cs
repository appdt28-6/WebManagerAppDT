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
    public class ProductosController : Controller
    {
        private AppDTEntities db = new AppDTEntities();

        public ActionResult Productos()
        {
            if (Session["user_id"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("~/Home/Login");
            }
        }

        public ActionResult PRODUCTOS_PV_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<PRODUCTOS_PV> productos_pv = db.PRODUCTOS_PV;
            DataSourceResult result = productos_pv.ToDataSourceResult(request, pRODUCTOS_PV => new {
                Prod_Id = pRODUCTOS_PV.Prod_Id,
                Prod_Codigo = pRODUCTOS_PV.Prod_Codigo,
                Prod_Name = pRODUCTOS_PV.Prod_Name,
                Prod_Price = pRODUCTOS_PV.Prod_Price,
                Prod_Desc = pRODUCTOS_PV.Prod_Desc
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PRODUCTOS_PV_Create([DataSourceRequest]DataSourceRequest request, PRODUCTOS_PV pRODUCTOS_PV)
        {
            if (ModelState.IsValid)
            {
                var entity = new PRODUCTOS_PV
                {
                    Prod_Codigo = pRODUCTOS_PV.Prod_Codigo,
                    Prod_Name = pRODUCTOS_PV.Prod_Name,
                    Prod_Price = pRODUCTOS_PV.Prod_Price,
                    Prod_Desc = pRODUCTOS_PV.Prod_Desc
                };

                db.PRODUCTOS_PV.Add(entity);
                db.SaveChanges();
                pRODUCTOS_PV.Prod_Id = entity.Prod_Id;
            }

            return Json(new[] { pRODUCTOS_PV }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PRODUCTOS_PV_Update([DataSourceRequest]DataSourceRequest request, PRODUCTOS_PV pRODUCTOS_PV)
        {
            if (ModelState.IsValid)
            {
                var entity = new PRODUCTOS_PV
                {
                    Prod_Id = pRODUCTOS_PV.Prod_Id,
                    Prod_Codigo = pRODUCTOS_PV.Prod_Codigo,
                    Prod_Name = pRODUCTOS_PV.Prod_Name,
                    Prod_Price = pRODUCTOS_PV.Prod_Price,
                    Prod_Desc = pRODUCTOS_PV.Prod_Desc
                };

                db.PRODUCTOS_PV.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { pRODUCTOS_PV }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PRODUCTOS_PV_Destroy([DataSourceRequest]DataSourceRequest request, PRODUCTOS_PV pRODUCTOS_PV)
        {
            if (ModelState.IsValid)
            {
                var entity = new PRODUCTOS_PV
                {
                    Prod_Id = pRODUCTOS_PV.Prod_Id,
                    Prod_Codigo = pRODUCTOS_PV.Prod_Codigo,
                    Prod_Name = pRODUCTOS_PV.Prod_Name,
                    Prod_Price = pRODUCTOS_PV.Prod_Price,
                    Prod_Desc = pRODUCTOS_PV.Prod_Desc
                };

                db.PRODUCTOS_PV.Attach(entity);
                db.PRODUCTOS_PV.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { pRODUCTOS_PV }.ToDataSourceResult(request, ModelState));
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
