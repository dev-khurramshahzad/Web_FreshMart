# Admin Module - Quick Reference Guide

## Navigation

| Feature | URL | Purpose |
|---------|-----|---------|
| Admin Dashboard | `/Admin/Index` | View key metrics and summary |
| Stock Report | `/Admin/StockReport` | Track inventory levels |
| Order Report | `/Admin/OrderReport` | Analyze sales data |
| Order Details | `/Admin/OrderDetails/{id}` | View items in specific order |
| Export Stock | `/Admin/ExportStockReport` | Download stock data as CSV |
| Export Orders | `/Admin/ExportOrderReport` | Download order data as CSV |

---

## Stock Report Features

### Summary Cards
- **Total Products**: Number of products in inventory
- **Total Stock Value**: Calculated as (Available Stock × Purchase Price)
- **Low Stock Count**: Items with less than 10 units
- **Out of Stock Count**: Items with 0 units

### Stock Status Badges
| Status | Color | Meaning |
|--------|-------|---------|
| In Stock | 🟢 Green | 10 or more units available |
| Low Stock | 🟡 Yellow | Less than 10 units |
| Out of Stock | 🔴 Red | 0 units available |

### Search Functionality
- Search by product name
- Search by category name
- Case-sensitive search

---

## Order Report Features

### Summary Cards
- **Total Orders**: Total number of orders
- **Total Revenue**: Sum of all order amounts
- **Average Order Value**: Total Revenue ÷ Total Orders
- **Order Count**: Number of orders in current view

### Date Filters
| Filter | Description |
|--------|-------------|
| All Orders | Show all orders ever created |
| Today | Show only today's orders |
| Last 7 Days | Show orders from past week |
| This Month | Show orders from current month |

### Search Functionality
- Search by customer name
- Search by customer email
- Search by order ID

---

## Code Structure

### AdminController Methods

```
AdminController
├── Index()                    // Dashboard
├── StockReport()              // View stock data
├── OrderReport()              // View order data
├── OrderDetails(int id)       // View specific order details
├── ExportStockReport()        // Download CSV
└── ExportOrderReport()        // Download CSV
```

### Helper Classes

```
StockReportData
├── ItemId
├── ItemName
├── CategoryName
├── SellingPrice
├── PurchasePrice
├── StockUnit
├── AvailableStock
├── Status
├── ExpiryDate
├── StockStatus
└── TotalValue

OrderReportData
├── OrderId
├── OrderDate
├── CustomerName
├── CustomerEmail
├── CustomerPhone
├── TotalAmount
└── ItemCount
```

---

## Common Code Patterns

### 1. **Check Admin Login**
```csharp
string? admin = HttpContext.Session.GetString("Admin");
if (string.IsNullOrEmpty(admin))
{
    return RedirectToAction("Login", "Home");
}
```

### 2. **Loop Through Collection**
```csharp
foreach (var item in allItems)
{
    // Process item
}
```

### 3. **Filter with Condition**
```csharp
if (searchTerm.Contains(item.Name))
{
    // Include item
}
```

### 4. **Calculate Summary**
```csharp
decimal total = 0;
foreach (var item in items)
{
    total += item.Amount;
}
```

### 5. **Create CSV Export**
```csharp
string csv = "Header1,Header2\n";
foreach (var item in items)
{
    csv += $"{item.Value1},{item.Value2}\n";
}
byte[] buffer = System.Text.Encoding.UTF8.GetBytes(csv);
return File(buffer, "text/csv", fileName);
```

---

## View Elements

### Stock Report View
- Summary Cards (4 cards)
- Search Form
- Responsive Table (11 columns)
- Export Button
- Empty State Message

### Order Report View
- Summary Cards (4 cards)
- Search Form
- Date Filter Dropdown
- Responsive Table (8 columns)
- View Details Button
- Export Button
- Empty State Message

---

## Data Display Formats

### Currency
- Format: `Rs. {value:F2}`
- Example: `Rs. 1500.50`

### Date
- Stock Report: `dd/MM/yyyy`
- Order Report: `dd MMM yyyy` with time `hh:mm tt`
- Example: `15/12/2024` and `15 Dec 2024 02:30 PM`

### Numbers
- Stock quantity: Integer (no decimals)
- Prices: 2 decimal places

---

## Bootstrap Classes Used

| Class | Purpose |
|-------|---------|
| `card` | Container element |
| `badge` | Status indicators |
| `btn` | Buttons |
| `table-hover` | Interactive table rows |
| `table-responsive` | Mobile-friendly tables |
| `form-control` | Input fields |
| `text-success` | Green text |
| `text-danger` | Red text |
| `text-warning` | Yellow text |
| `text-primary` | Blue text |

---

## Error Handling

### 404 Not Found
- When order ID doesn't exist in OrderDetails
- Returns 404 error page

### Empty Results
- Shows "No products found" message
- Shows "No orders found" message

### Null Values
- Uses `??` operator for defaults
- Uses `?.` for safe navigation

---

## Performance Considerations

1. **Database Queries**
   - `.Include()` loads related data in one query
   - `.ToList()` executed at appropriate times

2. **Filtering**
   - Search done in-memory after loading
   - Date filters applied before displaying

3. **CSV Export**
   - Loads all data at once
   - Suitable for reasonable data sizes
   - Consider pagination for very large datasets

---

## Testing Checklist

- [ ] Stock Report displays all products
- [ ] Search filter works for products
- [ ] Stock status badges show correct colors
- [ ] Total stock value calculates correctly
- [ ] Order Report displays all orders
- [ ] Date filters work correctly
- [ ] Order search works by name/email/ID
- [ ] View Details button opens correct order
- [ ] CSV exports contain correct data
- [ ] Admin login validation works
- [ ] Empty state messages appear when needed
- [ ] Page layout is responsive on mobile

---

## Tips for Students

1. **Read the Comments** - Code has clear comments explaining logic
2. **Understand Data Flow** - Database → Controller → Helper Class → View
3. **Follow the Pattern** - Use same pattern for similar features
4. **Test Thoroughly** - Try different search/filter combinations
5. **Study the Views** - Learn Bootstrap and Razor syntax
6. **Modify Carefully** - Make small changes and test each one
7. **Use Browser DevTools** - Inspect HTML and debug issues
8. **Keep Code Simple** - Beginner-friendly means readable and maintainable

---

## Quick Troubleshooting

| Problem | Solution |
|---------|----------|
| Admin not logged in | Check Session variables, ensure admin login works |
| No data showing | Check if database has data, verify queries |
| CSV export empty | Verify CSV headers and data loop |
| Search not working | Check if search string matches field names |
| Date filter not working | Verify date logic and filter conditions |
| View Details button broken | Check if OrderDetails action exists |
| Page layout broken | Check Bootstrap and CSS links in layout |

---

## References

- **Bootstrap Documentation**: https://getbootstrap.com/docs/
- **ASP.NET Core Razor Pages**: https://learn.microsoft.com/en-us/aspnet/core/
- **Entity Framework Core**: https://learn.microsoft.com/en-us/ef/core/
- **C# Basics**: https://learn.microsoft.com/en-us/dotnet/csharp/

Good luck with your FYP! Feel free to reach out if you have questions. 📚
