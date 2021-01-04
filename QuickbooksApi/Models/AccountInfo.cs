﻿using Newtonsoft.Json;

namespace QuickbooksApi.Models
{
    public class AccountInfo : BaseModel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("AccountType")]
        public string AccountType { get; set; }
        [JsonProperty("Classification")]
        public string Classification { get; set; }
        [JsonProperty("CurrentBalance")]
        public double CurrentBalance { get; set; }
        [JsonProperty("FullyQualifiedName")]
        public string FullyQualifiedName { get; set; }
        [JsonProperty("AccountSubType")]
        public string AccountSubType { get; set; }
        [JsonProperty("CurrentBalanceWithSubAccounts")]
        public double CurrentBalanceWithSubAccounts { get; set; }
        [JsonProperty("SubAccount")]
        public bool SubAccount { get; set; }
    }

    public class AccountApiModel : BaseApiModel
    {
        [JsonProperty("Account")]
        public AccountInfo Account { get; set; }
    }
}