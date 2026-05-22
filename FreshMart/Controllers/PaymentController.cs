using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace FreshMart.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout(decimal amount)
        {
            var domain = "https://localhost:7151/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + "Payment/Success",
                CancelUrl = domain + "Payment/Cancel",
                PaymentMethodTypes = new List<string>
            {
                "card"
            },
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "pkr",
                        UnitAmount = (long?)(amount*100), // $50.00
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Fresh Mart Checkout"
                        }
                    }
                }
            }
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }

        public IActionResult Success()
        {
            return Redirect("/Home/Checkout");
        }

        public IActionResult Cancel()
        {
            return Redirect("/Home/Cart");
        }
    }
}
