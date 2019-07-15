using MazzaWebSite.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
namespace MazzaWebSite.Controllers
{
    public class InvestorController : Controller
    {
        public ActionResult Dashboard()
        {
            int userId = User.Identity.GetUserId<int>();
            using (var dbContext = new MazzaDbContext())
            {
                Boh model = new Boh
                {
                    InvestmentReports = dbContext.Database
                    .SqlQuery<InvestmentReport>(string.Format("CALL spREP_InvestmentReport ({0})", userId))
                    .ToList(),
                    AffiliateLists = dbContext.Database
                    .SqlQuery<AffiliateList>(string.Format("CALL spREP_GetAffiliate ({0})", userId))
                    .ToList()
                };
            return Request.IsAuthenticated && model != null ? View(model) : (ActionResult)RedirectToAction("Login", "Account");
            }
        }
        public ActionResult Deposit()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            using (var dbContext = new MazzaDbContext())
            {
                DepositModel model = new DepositModel
                {
                    PaymentTypes = dbContext.PaymentTypes.Where(a => a.IsActive).OrderByDescending(i => i.Code.Equals("BTC")).ThenBy(p => p.Code).ToList()
                };
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTransaction(DepositModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "You must insert amount > 0 and check one payment method!");
                return RedirectToAction("Deposit", "Investor");
            }
            using (var dbContext = new MazzaDbContext())
            {
                var paymentType = dbContext.PaymentTypes.FirstOrDefault(p => p.Code.Equals(model.Coin));
                if (paymentType.WalletTypeId==1)
                {
                    return RedirectToAction(paymentType.PaymentTypeDesc, "Payment");
                }
            }

            //Get Rates 


            string s_privkey = "89c2E2B75e268825e6Ec2C2E76f1cbA2604cD048f74Ea51cf23fc1a85E57c2b3";
            string s_pubkey = "6431d23e0f03d7a10013bc0776b44c66a749438e65f77b6309995637a971a2b7";
            Encoding encoding = Encoding.UTF8;
            SortedList<string, string> parms = new SortedList<string, string>
            {
                //{ "amount", model.Amount.ToString() },
                //{ "currency1", model.Coin },
                //{ "currency2", model.Coin },
                //{ "buyer_email", "1millionproject.info@gmail.com" }
            };

            parms["version"] = "1";
            parms["key"] = s_pubkey;
            //parms["cmd"] = "create_transaction";
            parms["cmd"] = "rates";

            string post_data = "";
            foreach (KeyValuePair<string, string> parm in parms)
            {
                if (post_data.Length > 0) { post_data += "&"; }
                post_data += parm.Key + "=" + Uri.EscapeDataString(parm.Value);
            }

            byte[] keyBytes = encoding.GetBytes(s_privkey);
            byte[] postBytes = encoding.GetBytes(post_data);
            var hmacsha512 = new System.Security.Cryptography.HMACSHA512(keyBytes);
            string hmac = BitConverter.ToString(hmacsha512.ComputeHash(postBytes)).Replace("-", string.Empty);

            // do the post:
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient cl = new System.Net.WebClient();
            cl.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            cl.Headers.Add("HMAC", hmac);
            cl.Encoding = encoding;

            var ret = new Dictionary<string, dynamic>();
            try
            {
                string resp = cl.UploadString("https://www.coinpayments.net/api.php", post_data);
                var decoder = new System.Web.Script.Serialization.JavaScriptSerializer();
                ret = decoder.Deserialize<Dictionary<string, dynamic>>(resp);

                if (ret["error"] == "ok")
                {
                    var userId = User.Identity.GetUserId<int>();

                    using (var dbContext = new MazzaDbContext())
                    {
                        dbContext.DepositTransactions.Add(new DepositTransaction
                        {
                            UserId = userId,
                            Amount = model.Amount,
                            TransactionId = ret["result"]["txn_id"],
                            Address = ret["result"]["address"],
                            StatusUrl = ret["result"]["status_url"],
                            Timeout = ret["result"]["timeout"],
                            TransactionDate = DateTime.UtcNow
                        });
                        dbContext.SaveChanges();
                    }
                    return Redirect(ret["result"]["status_url"]);
                }
            }
            catch (Exception ex)
            {
                SendEmail.Send("lmazzaferro6@gmail.com", "Errore Deposit", ex.Message);
            }
            return RedirectToAction("Deposit", "Investor");
        }
        public ActionResult Prova()
        {
            return View();
        }
    }
}