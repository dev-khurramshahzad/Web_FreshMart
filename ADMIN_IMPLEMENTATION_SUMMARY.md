# Admin Controller Implementation - Beginner Friendly

## Overview
A simplified Admin Controller has been created for the FreshMart project with professional views for Stock Report and Order Report. The code is structured for easy understanding by students working on their final year project (FYP).

---

## Files Created/Modified

### 1. **AdminController.cs** (NEW)
Location: `FreshMart/Controllers/AdminController.cs`

#### Key Features:
- **Simple, well-commented code** - Easy for beginners to understand
- **No complex LINQ queries** - Uses basic foreach loops instead
- **Clear variable names** - Self-documenting code
- **Separated concerns** - Each method has a specific responsibility

#### Methods Implemented:

##### `Index()` - Admin Dashboard
- Displays key metrics (Total Orders, Revenue, Customers, Products, Low Stock)
- Checks if admin is logged in using Session
- Calculates statistics using simple loops

##### `StockReport()` - Inventory Management
**Parameters:**
- `searchTerm` (optional): Filter products by name or category

**Functionality:**
- Retrieves all items from database
- Filters items based on search term
- Calculates stock status (In Stock, Low Stock, Out of Stock)
- Creates stock report data with detailed information
- Displays summary statistics

**Data Displayed:**
- Item ID, Name, Category
- Selling and Purchase Prices
- Available Stock
- Stock Status with color-coded badges
- Total Stock Value
- Expiry Date

##### `OrderReport()` - Sales Analytics
**Parameters:**
- `searchTerm` (optional): Search by customer name, email, or order ID
- `filterBy` (optional): Filter by date range (all, today, week, month)

**Functionality:**
- Retrieves all orders with customer information
- Applies date filters
- Applies search filters
- Counts items per order
- Calculates statistics (Total Orders, Revenue, Average Order Value)

**Data Displayed:**
- Order ID
- Order Date and Time
- Customer Information (Name, Email, Phone)
- Item Count
- Total Amount

##### `OrderDetails()` - Order Details View
- Shows detailed information for a specific order
- Displays all items in the order with prices
- Links to the OrderDetails view

##### `ExportStockReport()` - CSV Export
- Exports all stock information to CSV file
- Filename format: `StockReport_YYYY_MM_DD.csv`
- Useful for analysis in Excel

##### `ExportOrderReport()` - CSV Export
- Exports all order information to CSV file
- Filename format: `OrderReport_YYYY_MM_DD.csv`

---

## Helper Classes

### `StockReportData`
Used to pass stock information to the view.

**Properties:**
```csharp
public int ItemId { get; set; }
public string ItemName { get; set; }
public string CategoryName { get; set; }
public decimal SellingPrice { get; set; }
public decimal PurchasePrice { get; set; }
public string StockUnit { get; set; }
public int AvailableStock { get; set; }
public string Status { get; set; }
public DateTime? ExpiryDate { get; set; }
public string StockStatus { get; set; }
public decimal TotalValue { get; set; }
```

### `OrderReportData`
Used to pass order information to the view.

**Properties:**
```csharp
public int OrderId { get; set; }
public DateTime OrderDate { get; set; }
public string CustomerName { get; set; }
public string CustomerEmail { get; set; }
public string CustomerPhone { get; set; }
public decimal TotalAmount { get; set; }
public int ItemCount { get; set; }
```

---

## Views Updated

### 1. **StockReport.cshtml**
Location: `FreshMart/Views/Admin/StockReport.cshtml`

**Features:**
- Summary cards showing:
  - Total Products
  - Total Stock Value
  - Low Stock Items Count
  - Out of Stock Items Count
- Search functionality to filter products
- Responsive table with all stock details
- Color-coded stock status badges:
  - 🟢 Green: In Stock
  - 🟡 Yellow: Low Stock
  - 🔴 Red: Out of Stock
- Export to CSV button
- Empty state message when no products found

**Key Elements:**
```html
<!-- Search Form -->
<!-- Summary Cards -->
<!-- Responsive Table with Stock Details -->
<!-- Export Button -->
```

### 2. **OrderReport.cshtml**
Location: `FreshMart/Views/Admin/OrderReport.cshtml`

**Features:**
- Summary cards showing:
  - Total Orders
  - Total Revenue
  - Average Order Value
  - Order Count
- Search by customer name, email, or order ID
- Filter orders by date range (All, Today, Last 7 Days, This Month)
- Responsive table with order details
- View Details button for each order
- Export to CSV button
- Empty state message when no orders found

**Key Elements:**
```html
<!-- Search Form -->
<!-- Date Filter Dropdown -->
<!-- Summary Cards -->
<!-- Responsive Table with Order Details -->
<!-- View Details Button -->
<!-- Export Button -->
```

---

## Code Learning Points for Students

### 1. **Session Management**
```csharp
// Check if admin is logged in
string? admin = HttpContext.Session.GetString("Admin");
if (string.IsNullOrEmpty(admin))
{
    return RedirectToAction("Login", "Home");
}
```

### 2. **Simple Data Filtering**
```csharp
// Instead of complex LINQ, we use simple loops
foreach (var item in allItems)
{
    if (!string.IsNullOrEmpty(searchTerm))
    {
        if (!item.Name.Contains(searchTerm) && 
            (item.CategoryF == null || !item.CategoryF.CategoryName.Contains(searchTerm)))
        {
            continue; // Skip if doesn't match
        }
    }
    // Process item
}
```

### 3. **Creating Report Data**
```csharp
// Build report objects from database entities
StockReportData reportData = new StockReportData()
{
    ItemId = item.ItemId,
    ItemName = item.Name,
    AvailableStock = (int)(item.AvailableStock ?? 0),
    TotalValue = (decimal)item.AvailableStock.Value * item.Pprice.Value
};
```

### 4. **Summary Statistics**
```csharp
// Calculate totals using loops
decimal totalStockValue = 0;
foreach (var report in stockReportList)
{
    totalStockValue += report.TotalValue;
}
```

### 5. **CSV Export**
```csharp
// Create CSV string
string csv = "Column1,Column2,Column3\n";
foreach (var item in items)
{
    csv += $"{item.Id},\"{item.Name}\",{item.Price}\n";
}
// Convert to bytes and return file
byte[] buffer = System.Text.Encoding.UTF8.GetBytes(csv);
return File(buffer, "text/csv", fileName);
```

---

## How to Use

### Access Stock Report
```
URL: /Admin/StockReport
Search: Filter products by name or category
Export: Download CSV file for analysis
```

### Access Order Report
```
URL: /Admin/OrderReport
Search: Find specific orders by customer or order ID
Filter: View orders by date range
Export: Download CSV file
View Details: Click to see items in an order
```

### Dashboard
```
URL: /Admin/Index
Shows key metrics at a glance
```

---

## Important Notes

1. **Admin Login Required** - All admin methods check for active admin session
2. **Data Conversion** - AvailableStock is stored as `double` in database but converted to `int` for display
3. **Date Handling** - ExpDate is stored as `DateOnly` but converted to `DateTime` for display
4. **Null Safety** - Uses null coalescing operators (`??`) to provide default values
5. **Professional UI** - Bootstrap classes ensure responsive design

---

## Database Entities Used

- **Items** - Product information with stock levels
- **Orders** - Customer orders
- **OrderDetails** - Items within each order
- **Customers** - Customer information
- **Categories** - Product categories

---

## Testing Recommendations

1. Test Stock Report with multiple products
2. Test search functionality
3. Test order filtering by different date ranges
4. Test CSV export
5. Test with empty data sets
6. Test with missing customer or category information

---

## Future Enhancements (Optional)

1. Add charts/graphs to reports
2. Add filtering by category in stock report
3. Add order status tracking
4. Add customer spending analysis
5. Add product comparison reports
6. Add date range selector for custom reports

---

## Support for FYP Students

This implementation follows best practices for:
- ✅ Code readability and maintainability
- ✅ Separation of concerns
- ✅ Error handling and validation
- ✅ Professional UI/UX
- ✅ Performance optimization (using ToList() appropriately)
- ✅ Security (Session validation)

Good luck with your FYP! 🚀
