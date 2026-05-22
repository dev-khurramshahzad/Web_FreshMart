using FreshMart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Diagnostics;

namespace FreshMart.Controllers
{
    public class HomeController : Controller
    {
        public static List<CartItem> CartData = new List<CartItem>();
        public static Customer? LoggedInCustomer = null;
        private readonly IConfiguration config;


        private readonly DbFreshMartContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DbFreshMartContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
        public IActionResult Items(int? id, int? brand)
        {
            // Displaying Brands and Categories List on Side
            ViewBag.Cats = _context.Categories.ToList();


            //Filter Category
            var items = _context.Items.Include(x => x.CategoryF).ToList();

            ViewBag.TotalItems = items;

            if (id != null)
            {
                return View(items.Where(x => x.CategoryFid == id));
            }


            return View(items);
        }
        public IActionResult ItemDetails(int id)
        {
            var item = _context.Items.Include(x => x.CategoryF).FirstOrDefault(x => x.ItemId == id);
            return View(item);
        }
        public IActionResult Cart()
        {
            return View(CartData);
        }
        public IActionResult AddToCart(int id)
        {
            var item = _context.Items.Include(x => x.CategoryF).FirstOrDefault(x => x.ItemId == id);
            if (checkForExistingItem(id) == -1)
            {
                CartData.Add(new CartItem { item = item, quantity = 1 });
            }
            else
            {
                CartData[checkForExistingItem(id)].quantity++;
            }


            return RedirectToAction("Cart");
        }
        public IActionResult Remove(int id)
        {

            CartData.RemoveAt(checkForExistingItem(id));

            return RedirectToAction("Cart");
        }
        public IActionResult QtyPlus(int id)
        {
            CartData[checkForExistingItem(id)].quantity++;
            return RedirectToAction("Cart");
        }
        public IActionResult QtyMinus(int id)
        {
            if (CartData[checkForExistingItem(id)].quantity > 1)
            {
                CartData[checkForExistingItem(id)].quantity--;
            }
            else
            {
                TempData["Title"] = "Info";
                TempData["Message"] = "Quantity cannot be less than 1";
                TempData["Icon"] = "info";
            }

            return RedirectToAction("Cart");
        }
        int checkForExistingItem(int id)
        {
            int FoundIndex = -1;
            for (int i = 0; i < CartData.Count; i++)
            {
                if (id == CartData[i].item?.ItemId)
                {
                    FoundIndex = i;
                }
            }
            return FoundIndex;
        }
        public IActionResult CheckoutMethod(string paymentMethod,decimal amount)
        {
            // Check cart empty
            if (LoggedInCustomer == null)
            {
                TempData["Title"] = "Info";
                TempData["Message"] = "Please Login to Checkout";
                TempData["Icon"] = "info";

                return Redirect("Login?returnUrl=" + Request.Path);
            }


            if (paymentMethod == "cod")
                return Redirect("/Home/Checkout");

            else
                return Redirect("/Payment/Checkout?amount=" + amount);

        }

        public IActionResult Checkout()
        {
           
            // Check cart empty
            if (LoggedInCustomer == null)
            {
                TempData["Title"] = "Info";
                TempData["Message"] = "Please Login to Checkout";
                TempData["Icon"] = "info";

                return Redirect("Login?returnUrl=" + Request.Path);
            }


            // Check cart empty
            if (CartData.Count == 0)
            {
                TempData["Title"] = "Info";
                TempData["Message"] = "Cart is Empty Please add some items to checkout";
                TempData["Icon"] = "info";

                return Redirect("Cart");
            }

            // Calculate total
            decimal total = 0;

            foreach (var cartItem in CartData)
            {
                total += (decimal)(cartItem.item.Sprice * cartItem.quantity);
            }

            // Create Order
            Order order = new Order()
            {
                CustomerFid = LoggedInCustomer.CustomerId,
                OrderDate = DateTime.Now,
                TotalAmount = total
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Save Order Details
            foreach (var cartItem in CartData)
            {
                OrderDetail detail = new OrderDetail()
                {
                    OrderFid = order.OrderId,
                    ItemFid = cartItem.item?.ItemId,
                    Quantity = cartItem.quantity,
                    UnitPrice = cartItem.item?.Sprice
                };

                _context.OrderDetails.Add(detail);

                // Stock Update
                var product = _context.Items.Find(cartItem.item?.ItemId);

                if (product != null)
                {
                    product.AvailableStock -= cartItem.quantity;
                }
            }

            _context.SaveChanges();

            // Clear Cart
            CartData.Clear();

            return Redirect("Invoice?id=" + order.OrderId);
        }
        public async Task<IActionResult> Invoice(int id)
        {
            var order = await _context.Orders.Include(c => c.CustomerF).FirstOrDefaultAsync(x => x.OrderId == id);

            var orderDetails = _context.OrderDetails
                .Where(x => x.OrderFid == id)
                .ToList();

            foreach (var item in orderDetails)
            {
                item.ItemF = _context.Items.Find(item.ItemFid);
            }

            ViewBag.OrderDetails = orderDetails;

            return View(order);
        }
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // =========================================
        // REGISTER
        // =========================================

        [HttpPost]
        public IActionResult Register(string txtName, string txtEmail, string txtPass, string txtCPass, string txtPhone, string txtAddress, string txtCity, string txtState, string txtZip)
        {
            // PASSWORD CHECK

            if (txtPass != txtCPass)
            {
                TempData["Title"] = "Error";
                TempData["Message"] = "Passwords do not match";
                TempData["Icon"] = "error";

                return RedirectToAction("Login");
            }

            // EMAIL EXISTS

            var checkEmail = _context.Customers
                .FirstOrDefault(x => x.Email == txtEmail);

            if (checkEmail != null)
            {
                TempData["Title"] = "Error";
                TempData["Message"] = "Email already exists";
                TempData["Icon"] = "error";

                return RedirectToAction("Login");
            }

            // SAVE CUSTOMER

            Customer customer = new Customer()
            {
                FullName = txtName,
                Email = txtEmail,
                Password = txtPass,
                Phone = txtPhone,
                Address = txtAddress,
                City = txtCity,
                State = txtState,
                ZipCode = txtZip
            };

            _context.Customers.Add(customer);

            _context.SaveChanges();

            TempData["Title"] = "Success";
            TempData["Message"] = "Account Created Successfully";
            TempData["Icon"] = "success";

            return RedirectToAction("Login");
        }

        // =========================================
        // LOGIN CHECK
        // =========================================

        [HttpPost]
        public IActionResult LoginCheck(string txtEmail, string txtPass, string role, string returnUrl)
        {
            // ADMIN LOGIN

            if (role == "Admin")
            {
                Admin? admin = _context.Admins.FirstOrDefault(x => x.Email == txtEmail && x.Passwor == txtPass);
                if (admin != null)
                {
                    HttpContext.Session.SetString("Admin", admin?.Username);
                    HttpContext.Session.SetInt32("AdminID", admin.AdminId);

                    TempData["Title"] = "Welcome";
                    TempData["Message"] = "Admin Login Successful";
                    TempData["Icon"] = "success";

                    return RedirectToAction("AdminIndex", "Home");
                }

                TempData["Title"] = "Error";
                TempData["Message"] = "Invalid Admin Credentials";
                TempData["Icon"] = "error";

                return RedirectToAction("Login");
            }

            // CUSTOMER LOGIN

            var customer = _context.Customers.FirstOrDefault(x => x.Email == txtEmail && x.Password == txtPass);

            if (customer != null)
            {
                LoggedInCustomer = customer;

                TempData["Title"] = "Success";
                TempData["Message"] = "Login Successful";
                TempData["Icon"] = "success";

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return Redirect("/");
            }

            TempData["Title"] = "Error";
            TempData["Message"] = "Invalid Email or Password";
            TempData["Icon"] = "error";

            return RedirectToAction("Login");
        }

        // =========================================
        // SEND OTP EMAIL
        // =========================================

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
            {
                TempData["Title"] = "Error";
                TempData["Message"] = "Email not found";
                TempData["Icon"] = "error";

                return RedirectToAction("Login");
            }

            // GENERATE OTP

            Random random = new Random();

            int otp = random.Next(1000, 9999);

            // SAVE IN SESSION

            HttpContext.Session.SetString("ResetEmail", email);

            HttpContext.Session.SetInt32("OTP", otp);

            // SEND EMAIL

            MimeMessage message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(config["EmailSettings:Email"]));

            message.To.Add(MailboxAddress.Parse(email));

            message.Subject = "FreshMart Password Reset OTP";

            message.Body = new TextPart("html")
            {
                Text = $@"
        <div style='font-family: Arial, sans-serif;
                    padding:20px;
                    background:#f4f4f4;'>

            <div style='max-width:500px;
                        margin:auto;
                        background:white;
                        padding:30px;
                        border-radius:10px;
                        box-shadow:0 0 10px rgba(0,0,0,0.1);'>

                <h2 style='color:#16a34a;
                           text-align:center;'>
                    FreshMart Password Reset
                </h2>

                <p style='font-size:16px;
                          color:#333;'>

                    Hello,

                </p>

                <p style='font-size:15px;
                          color:#555;
                          line-height:25px;'>

                    We received a request to reset your FreshMart account password.
                    Please use the following One-Time Password (OTP)
                    to continue the password reset process.

                </p>

                <div style='text-align:center;
                            margin:30px 0;'>

                    <span style='display:inline-block;
                                 background:#16a34a;
                                 color:white;
                                 padding:15px 40px;
                                 font-size:32px;
                                 font-weight:bold;
                                 letter-spacing:5px;
                                 border-radius:8px;'>

                        {otp}

                    </span>

                </div>

                <p style='font-size:14px;
                          color:#777;
                          line-height:22px;'>

                    This OTP is valid for a limited time.
                    Do not share this code with anyone for security reasons.

                </p>

                <hr>

                <p style='text-align:center;
                          color:#999;
                          font-size:13px;'>

                    © FreshMart Authentication System

                </p>

            </div>

        </div>"
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect(
                    config["EmailSettings:Host"],
                    Convert.ToInt32(
                        config["EmailSettings:Port"]),
                    MailKit.Security.SecureSocketOptions.StartTls);

                smtp.Authenticate(
                    config["EmailSettings:Email"],
                    config["EmailSettings:Password"]);

                smtp.Send(message);

                smtp.Disconnect(true);
            }

            TempData["Title"] = "Success";
            TempData["Message"] = "OTP sent to your email";
            TempData["Icon"] = "success";

            return RedirectToAction("Login");
        }

        // =========================================
        // VERIFY OTP
        // =========================================

        [HttpPost]
        public IActionResult VerifyOTP(int otp)
        {
            int? savedOtp = HttpContext.Session.GetInt32("OTP");

            if (savedOtp == otp)
            {
                TempData["Title"] = "Success";
                TempData["Message"] = "OTP Verified";
                TempData["Icon"] = "success";

                return RedirectToAction("Login");
            }

            TempData["Title"] = "Error";
            TempData["Message"] = "Invalid OTP";
            TempData["Icon"] = "error";

            return RedirectToAction("Login");
        }

        // =========================================
        // RESET PASSWORD
        // =========================================

        [HttpPost]
        public IActionResult ResetPassword(string password, string confirmPassword)
        {
            // PASSWORD CHECK

            if (password != confirmPassword)
            {
                TempData["Title"] = "Error";
                TempData["Message"] = "Passwords do not match";
                TempData["Icon"] = "error";

                return RedirectToAction("Login");
            }

            string? email = HttpContext.Session.GetString("ResetEmail");

            var customer = _context.Customers.FirstOrDefault(x => x.Email == email);

            if (customer != null)
            {
                customer.Password = password;

                _context.SaveChanges();

                HttpContext.Session.Remove("OTP");
                HttpContext.Session.Remove("ResetEmail");

                TempData["Title"] = "Success";
                TempData["Message"] = "Password Changed Successfully";
                TempData["Icon"] = "success";
            }
            else
            {
                TempData["Title"] = "OTP Error";
                TempData["Message"] = "Please Enter a correct OTP Only Then It can be reset";
                TempData["Icon"] = "error";
            }

            // CLEAR SESSION

            HttpContext.Session.Remove("OTP");
            HttpContext.Session.Remove("ResetEmail");



            return RedirectToAction("Login");
        }

        // =========================================
        // LOGOUT
        // =========================================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["Title"] = "Success";
            TempData["Message"] = "Logout Successful";
            TempData["Icon"] = "success";

            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
