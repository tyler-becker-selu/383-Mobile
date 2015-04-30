﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeClient.Models;
using RestSharp;
using RestSharp.Deserializers;
using System.Net;

namespace EmployeeClient.Controllers
{
    public class SaleController : Controller
    {
        private RestClient client = new RestClient("http://localhost:12932/");

        #region Algorthms

        private void APIHeaders(RestRequest request)
        {
            if (Session["ApiKey"] != null && Session["UserId"] != null)
            {
                request.AddHeader("xcmps383authenticationkey", Session["ApiKey"].ToString());
                request.AddHeader("xcmps383authenticationid", Session["UserId"].ToString());
            }
        }

        private int GetID(string p)
        {
            string[] x = p.Split('/');
            return Convert.ToInt32(x[x.Length - 1]);
        }

        private List<Sale> GetSales()
        {
            var request = new RestRequest("api/Sales", Method.GET);
            var saleList = new List<Sale>();

            APIHeaders(request);

            var APIresponse = client.Execute(request);

            if (APIresponse.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                saleList = deserial.Deserialize<List<Sale>>(APIresponse);

                foreach (Sale item in saleList)
                {
                    item.ID = GetID(item.URL);
                }
            }

            return saleList;
        }


        #endregion

        // GET: Sale
        public ActionResult Index()
        {
            var sales = GetSales();
            return View(sales);
        }


        // GET: Sale/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sale/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sale/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sale/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sale/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
