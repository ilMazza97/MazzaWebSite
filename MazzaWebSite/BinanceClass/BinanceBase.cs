using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;

namespace MazzaWebSite.BinanceClass
{
    public class BinanceBase
    {
        public static void BinanceMain()
        {
            //https://github.com/JKorf/Binance.Net 

            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials("p5X0rxCZaLKbzPJNAGOo2vQ6JusPbKU85dElpK6M81P4HneGm37F0dGNEOMsMaE6", "Flr5VPJiDTcElglnuvNz1JW6pmjBZKcxWUGUdcLhGS1L9CuBx43RuyXlFJN4V1xd"),
                LogVerbosity = LogVerbosity.Debug
            });
            BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
            {
                ApiCredentials = new ApiCredentials("p5X0rxCZaLKbzPJNAGOo2vQ6JusPbKU85dElpK6M81P4HneGm37F0dGNEOMsMaE6", "Flr5VPJiDTcElglnuvNz1JW6pmjBZKcxWUGUdcLhGS1L9CuBx43RuyXlFJN4V1xd"),
                LogVerbosity = LogVerbosity.Debug
            });

            using (var client = new BinanceClient())
            {
                var ping = client.Ping();
                var exchangeInfo = client.GetExchangeInfo();
                var serverTime = client.GetServerTime();
                //var orderBook = client.GetOrderBook("BNBBTC", 10);
                //var aggTrades = client.GetAggregatedTrades("BNBBTC", startTime: DateTime.UtcNow.AddMinutes(-2), endTime: DateTime.UtcNow, limit: 10);
                //var klines = client.GetKlines("BNBBTC", KlineInterval.OneHour, startTime: DateTime.UtcNow.AddHours(-10), endTime: DateTime.UtcNow, limit: 10);
                //var price = client.GetPrice("BNBBTC");
                //var prices24h = client.Get24HPrice("BNBBTC");
                //var allPrices = client.GetAllPrices();
                //var allBookPrices = client.GetAllBookPrices();
                //var historicalTrades = client.GetHistoricalTrades("BNBBTC");

                // Private
                //avar openOrders = client.GetOpenOrders("BNBBTC");
                //var allOrders = client.GetAllOrders("BNBBTC");
                //var testOrderResult = client.PlaceTestOrder("BNBBTC", OrderSide.Buy, OrderType.Limit, 1, price: 1, timeInForce: TimeInForce.GoodTillCancel);
                //var queryOrder = client.QueryOrder("BNBBTC", allOrders.Data[0].OrderId);
                //var orderResult = client.PlaceOrder("BNBBTC", OrderSide.Sell, OrderType.Limit, 10, price: 0.0002m, timeInForce: TimeInForce.GoodTillCancel);
                //var cancelResult = client.CancelOrder("BNBBTC", orderResult.Data.OrderId);
                //var accountInfo = client.GetAccountInfo();
                //var myTrades = client.GetMyTrades("BNBBTC");

                // Withdrawal/deposit
                var withdrawalHistory = client.GetWithdrawHistory();
                var depositHistory = client.GetDepositHistory();
                var withdraw = client.Withdraw("ASSET", "ADDRESS", 0);
            }
        }
    }
}