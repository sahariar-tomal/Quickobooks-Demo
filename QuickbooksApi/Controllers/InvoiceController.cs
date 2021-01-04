﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class InvoiceController : BaseController
    {
        private IInvoiceRepository _repository;

        public InvoiceController(
            IInvoiceRepository repository, IApiDataProvider provider,
            IJsonToModelBuilder builder): base(provider, builder)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateInvoice()
        {
            return View();
        }

        public ActionResult DeleteInvoice()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteInvoice(string id)
        {
            Logger.WriteDebug("Deleting invoice.");
            InvoiceInfo invoice = new InvoiceInfo()
            {
                Id = id,
                SyncToken = "0"
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(invoice), Encoding.UTF8, "application/json");
            await HandleDeleteRequest(requestBody, EntityType.Invoice.ToString().ToLower());
            _repository.DeleteInvoiceInfo(id);

            return RedirectToAction("Index");
        }

        public ActionResult DownloadInvoice()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DownloadInvoice(string id)
        {
            var invoiceId = WebUtility.UrlEncode(id);
            var qboBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
            var realmId = Session["realmId"].ToString();
            string uri = string.Format("{0}/v3/company/{1}/invoice/{2}/pdf?minorversion=55", qboBaseUrl, realmId, invoiceId);
            var principal = User as ClaimsPrincipal;
            var token = principal.FindFirst("access_token").Value;
            await _provider.GetPDF(uri, token);

            return RedirectToAction("Index");
        }
    }
}