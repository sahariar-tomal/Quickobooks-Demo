﻿using Newtonsoft.Json;
using QuickbooksApi.Helper;
using QuickbooksApi.Interfaces;
using QuickbooksApi.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuickbooksApi.Controllers
{
    public class AccountController : BaseController
    {
        private IAccountRepository _repository;

        public AccountController(
            IApiDataProvider provider, IAccountRepository repository, 
            IJsonToModelBuilder builder) : base(provider, builder)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccount(AccountInfo model)
        {
            Logger.WriteDebug("Creating new account.");
            AccountInfo acct = new AccountInfo()
            {
                Name = model.Name,
                AccountType = model.AccountType,
                Classification = model.Classification,
                CurrentBalance = model.CurrentBalance
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(acct), Encoding.UTF8, "application/json");
            var acctInfo = await HandlePostRequest(requestBody, EntityType.Account.ToString().ToLower());
            AccountInfo accountInfo = _builder.GetAccountModel(acctInfo);
            _repository.SaveAccountInfo(accountInfo);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> AccountDetails()
        {
            Logger.WriteDebug("Showing account details.");
            var acctInfo = await HandleGetRequest("93", EntityType.Account.ToString().ToLower());
            AccountInfo accountInfo = _builder.GetAccountModel(acctInfo);
            _repository.SaveAccountInfo(accountInfo);
            return View(accountInfo);
        }

        //To do
        public async Task<ActionResult> UpdateAccount()
        {
            var account = new 
            {
                FullyQualifiedName = "Shuvo New Credit",
                domain = "QBO",
                SubAccount = false,
                Description = "Description added during update.",
                Classification = "Asset",
                AccountSubType = "AccountsReceivable",
                CurrentBalanceWithSubAccounts = 1091.23,
                sparse = false,
                MetaData = new
                {
                    CreateTime = "2014-09-12T10:12:02-07:00",
                    LastUpdatedTime = "2015-06-30T15:09:07-07:00"
                },
                AccountType = "Accounts Receivable",
                CurrentBalance = 1091.23,
                Active = true,
                SyncToken = "0",
                Id = "93",
                Name = "Shuvo New Credit"
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
            var accountInfo = await HandlePostRequest(requestBody, EntityType.Account.ToString().ToLower());
            return RedirectToAction("Index");
        }
    }
}