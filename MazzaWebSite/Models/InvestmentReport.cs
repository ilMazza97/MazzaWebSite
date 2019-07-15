using System;
using System.Collections.Generic;

namespace MazzaWebSite.Models
{
    public class Boh
    {
        public List<InvestmentReport> InvestmentReports { get; set; }
        public List<AffiliateList> AffiliateLists { get; set; }
    }
    public class InvestmentReport : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal PackageTotal { get; set; }
        public string PackageName { get; set; }
        public string PackagePercentage { get; set; }
        public decimal TotalDeposit { get; set; }
        public string DepositPercentage { get; set; }
        public decimal TotalWithdrawal { get; set; }
        public decimal? Profit { get; set; }
        public decimal? AccumulateCommission { get; set; }
        public decimal? DestinateCommission { get; set; }
        public string Commission { get; set; }
        public DateTime NextPayout { get; set; }
        public int AffiliateCount { get; set; }
        public List<string> Affiliate { get; set; }
    }
    public class AffiliateList
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int IsActive { get; set; }
        public DateTime ActiveDate { get; set; }
    }
}
