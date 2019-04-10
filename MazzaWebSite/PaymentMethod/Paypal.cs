using PayPal.Api;
using System.Collections.Generic;

namespace MazzaWebSite.PaymentMethod
{
    public class Paypal
    {
        public static void SendMoney()
        {
            var apiContext = Configuration.GetAPIContext();

            var paymentList = Payment.List(apiContext);
            var payout = new Payout
            {
                sender_batch_header = new PayoutSenderBatchHeader
                {
                    sender_batch_id = "batch_" + System.Guid.NewGuid().ToString().Substring(0, 8),
                    email_subject = "You have a payment"
                    
                },
                items = new List<PayoutItem>
                {
                    new PayoutItem
                    {
                        recipient_type = PayoutRecipientType.EMAIL,
                        amount = new Currency
                        {
                            value = "8.99",
                            currency = "EUR"
                        },
                        receiver = "lmazzaferro6-buyer@gmail.com",
                        note = "Thank you.",
                        sender_item_id = "item_1"
                    }
                }
            };


            var createdPayout = payout.Create(apiContext);
        }
    }
}