using FreshMart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshMart.Controllers
{
    public class AdminController : Controller
    {
        // Database connection
        private readonly DbFreshMartContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(DbFreshMartContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // =========================================
        // ADMIN INDEX / DASHBOARD
        // =========================================
        public IActionResult Index()
        {
            // Check if admin is logged in
            string? admin = HttpContext.Session.GetString("Admin");
            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("Login", "Home");
            }

            // Get all orders from database
            List<Order> allOrders = _context.Orders.ToList();
            ViewBag.TotalOrders = allOrders.Count;

            // Calculate total revenue
            decimal totalRevenue = 0;
            foreach (var order in allOrders)
            {
                if (order.TotalAmount.HasValue)
                {
                    totalRevenue += order.TotalAmount.Value;
                }
            }
            ViewBag.TotalRevenue = totalRevenue;

            // Get total customers
            List<Customer> allCustomers = _context.Customers.ToList();
            ViewBag.TotalCustomers = allCustomers.Count;

            // Get total products
            List<Item> allProducts = _context.Items.ToList();
            ViewBag.TotalProducts = allProducts.Count;

            // Count low stock products (less than 10 items)
            int lowStockCount = 0;
            foreach (var product in allProducts)
            {
                if (product.AvailableStock < 10)
                {
                    lowStockCount++;
                }
            }
            ViewBag.LowStockProducts = lowStockCount;

            return View();
        }

        // =========================================
        // STOCK REPORT
        // =========================================
        public IActionResult StockReport(string searchTerm = "")
        {
            // Check if admin is logged in
            string? admin = HttpContext.Session.GetString("Admin");
            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("Login", "Home");
            }

            // Get all items from database
            List<Item> allItems = _context.Items.Include(i => i.CategoryF).ToList();

            // Create a list to store stock report data
            List<StockReportData> stockReportList = new List<StockReportData>();

            // Loop through each item and create report data
            foreach (var item in allItems)
            {
                // If search term is provided, filter items
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    // Check if item name or category contains search term
                    if (!item.Name.Contains(searchTerm) && 
                        (item.CategoryF == null || !item.CategoryF.CategoryName.Contains(searchTerm)))
                    {
                        continue; // Skip this item if it doesn't match search
                    }
                }

                // Create stock status
                string stockStatus = "In Stock";
                if (item.AvailableStock < 1)
                {
                    stockStatus = "Out of Stock";
                }
                else if (item.AvailableStock < 10)
                {
                    stockStatus = "Low Stock";
                }

                // Calculate total stock value
                decimal totalValue = 0;
                if (item.AvailableStock.HasValue && item.Pprice.HasValue)
                {
                    totalValue = (decimal)item.AvailableStock.Value * item.Pprice.Value;
                }

                // Add report data to list
                StockReportData reportData = new StockReportData()
                {
                    ItemId = item.ItemId,
                    ItemName = item.Name,
                    CategoryName = item.CategoryF?.CategoryName ?? "N/A",
                    SellingPrice = item.Sprice ?? 0,
                    PurchasePrice = item.Pprice ?? 0,
                    StockUnit = item.StockUnit ?? "N/A",
                    AvailableStock = (int)(item.AvailableStock ?? 0),
                    Status = item.Status ?? "Active",
                    ExpiryDate = item.ExpDate.HasValue ? new DateTime(item.ExpDate.Value.Year, item.ExpDate.Value.Month, item.ExpDate.Value.Day) : null,
                    StockStatus = stockStatus,
                    TotalValue = totalValue
                };

                stockReportList.Add(reportData);
            }

            // Calculate summary statistics
            int totalProducts = stockReportList.Count;
            decimal totalStockValue = 0;
            int lowStockCount = 0;
            int outOfStockCount = 0;

            foreach (var report in stockReportList)
            {
                totalStockValue += report.TotalValue;
                if (report.StockStatus == "Low Stock")
                {
                    lowStockCount++;
                }
                else if (report.StockStatus == "Out of Stock")
                {
                    outOfStockCount++;
                }
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalStockValue = totalStockValue;
            ViewBag.LowStockCount = lowStockCount;
            ViewBag.OutOfStockCount = outOfStockCount;

            return View(stockReportList);
        }

        // =========================================
        // ORDER REPORT
        // =========================================
        public IActionResult OrderReport(string searchTerm = "", string filterBy = "all")
        {
            // Check if admin is logged in
            string? admin = HttpContext.Session.GetString("Admin");
            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("Login", "Home");
            }

            // Get all orders from database with customer information
            List<Order> allOrders = _context.Orders
                .Include(o => o.CustomerF)
                .ToList();

            // Create a list to store order report data
            List<OrderReportData> orderReportList = new List<OrderReportData>();

            // Loop through each order
            foreach (var order in allOrders)
            {
                // Apply date filter
                bool includeOrder = false;
                DateTime today = DateTime.Today;

                if (filterBy == "today")
                {
                    // Only show today's orders
                    if (order.OrderDate.HasValue && order.OrderDate.Value.Date == today)
                    {
                        includeOrder = true;
                    }
                }
                else if (filterBy == "week")
                {
                    // Only show orders from last 7 days
                    if (order.OrderDate.HasValue && order.OrderDate.Value >= today.AddDays(-7))
                    {
                        includeOrder = true;
                    }
                }
                else if (filterBy == "month")
                {
                    // Only show orders from current month
                    if (order.OrderDate.HasValue && 
                        order.OrderDate.Value.Month == today.Month && 
                        order.OrderDate.Value.Year == today.Year)
                    {
                        includeOrder = true;
                    }
                }
                else
                {
                    // Show all orders
                    includeOrder = true;
                }

                if (!includeOrder)
                {
                    continue; // Skip this order if it doesn't match filter
                }

                // Apply search filter
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    bool matchesSearch = false;

                    // Check if order ID matches
                    if (order.OrderId.ToString().Contains(searchTerm))
                    {
                        matchesSearch = true;
                    }

                    // Check if customer name matches
                    if (order.CustomerF != null && order.CustomerF.FullName.Contains(searchTerm))
                    {
                        matchesSearch = true;
                    }

                    // Check if customer email matches
                    if (order.CustomerF != null && order.CustomerF.Email.Contains(searchTerm))
                    {
                        matchesSearch = true;
                    }

                    if (!matchesSearch)
                    {
                        continue; // Skip this order if it doesn't match search
                    }
                }

                // Count items in this order
                List<OrderDetail> orderDetails = _context.OrderDetails
                    .Where(od => od.OrderFid == order.OrderId)
                    .ToList();

                // Create order report data
                OrderReportData reportData = new OrderReportData()
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate ?? DateTime.Now,
                    CustomerName = order.CustomerF?.FullName ?? "N/A",
                    CustomerEmail = order.CustomerF?.Email ?? "N/A",
                    CustomerPhone = order.CustomerF?.Phone ?? "N/A",
                    TotalAmount = order.TotalAmount ?? 0,
                    ItemCount = orderDetails.Count
                };

                orderReportList.Add(reportData);
            }

            // Calculate summary statistics
            int totalOrders = orderReportList.Count;
            decimal totalRevenue = 0;
            decimal averageOrderValue = 0;

            foreach (var report in orderReportList)
            {
                totalRevenue += report.TotalAmount;
            }

            if (totalOrders > 0)
            {
                averageOrderValue = totalRevenue / totalOrders;
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.FilterBy = filterBy;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.AverageOrderValue = averageOrderValue;

            return View(orderReportList);
        }

        // =========================================
        // ORDER DETAILS
        // =========================================
        public IActionResult OrderDetails(int id)
        {
            // Check if admin is logged in
            string? admin = HttpContext.Session.GetString("Admin");
            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("Login", "Home");
            }

            // Find the order by ID
            Order? order = _context.Orders
                .Include(o => o.CustomerF)
                .FirstOrDefault(o => o.OrderId == id);

            // If order not found, show error page
            if (order == null)
            {
                return NotFound();
            }

            // Get all details for this order
            List<OrderDetail> orderDetails = _context.OrderDetails
                .Where(od => od.OrderFid == id)
                .ToList();

            // Load item information for each order detail
            foreach (var detail in orderDetails)
            {
                detail.ItemF = _context.Items.Find(detail.ItemFid);
            }

            ViewBag.OrderDetails = orderDetails;

            return View(order);
        }

        // =========================================
        // EXPORT STOCK REPORT (CSV)
        // =========================================
        public IActionResult ExportStockReport()
        {
            // Check if admin is logged in
            string? admin = HttpContext.Session.GetString("Admin");
            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("Login", "Home");
            }

            // Get all items
            List<Item> allItems = _context.Items.Include(i => i.CategoryF).ToList();

            // Create CSV header
            string csv = "Item ID,Item Name,Category,Selling Price,Purchase Price,Stock Unit,Available Stock,Status,Expiry Date,Stock Value\n";

            // Add each item to CSV
            foreach (var item in allItems)
            {
                decimal stockValue = 0;
                if (item.AvailableStock.HasValue && item.Pprice.HasValue)
                {
                    stockValue = (decimal)item.AvailableStock.Value * item.Pprice.Value;
                }

                csv += $"{item.ItemId},\"{item.Name}\",\"{item.CategoryF?.CategoryName}\",{item.Sprice},{item.Pprice},\"{item.StockUnit}\",{item.AvailableStock},\"{item.Status}\",{item.ExpDate},{stockValue}\n";
            }

            // Convert CSV string to bytes
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(csv);

            // Return file for download
            string fileName = "StockReport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".csv";
            return File(buffer, "text/csv", fileName);
        }

        // =========================================
        // EXPORT ORDER REPORT (CSV)
        // =========================================
        public IActionResult ExportOrderReport()
        {
            // Check if admin is logged in
            string? admin = HttpContext.Session.GetString("Admin");
            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("Login", "Home");
            }

            // Get all orders with customer information
            List<Order> allOrders = _context.Orders
                .Include(o => o.CustomerF)
                .ToList();

            // Create CSV header
            string csv = "Order ID,Order Date,Customer Name,Customer Email,Customer Phone,Total Amount,Item Count\n";

            // Add each order to CSV
            foreach (var order in allOrders)
            {
                // Count items in this order
                int itemCount = _context.OrderDetails.Where(od => od.OrderFid == order.OrderId).Count();

                csv += $"{order.OrderId},\"{order.OrderDate:yyyy-MM-dd}\",\"{order.CustomerF?.FullName}\",\"{order.CustomerF?.Email}\",\"{order.CustomerF?.Phone}\",{order.TotalAmount},{itemCount}\n";
            }

            // Convert CSV string to bytes
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(csv);

            // Return file for download
            string fileName = "OrderReport_" + DateTime.Now.ToString("yyyy_MM_dd") + ".csv";
            return File(buffer, "text/csv", fileName);
        }
    }

    // =========================================
    // HELPER CLASSES FOR REPORTS
    // =========================================

    public class StockReportData
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public string StockUnit { get; set; } = "";
        public int AvailableStock { get; set; }
        public string Status { get; set; } = "";
        public DateTime? ExpiryDate { get; set; }
        public string StockStatus { get; set; } = "";
        public decimal TotalValue { get; set; }
    }

    public class OrderReportData
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }
}
