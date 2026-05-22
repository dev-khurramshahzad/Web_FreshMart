# How to Use Admin Features - Step by Step Guide

## Getting Started

### Prerequisites
- Admin account must be created in the database
- Admin must be logged in to access admin features
- Database must contain items, orders, and customers

---

## 1. Admin Dashboard

### Accessing Dashboard
1. Log in as an admin
2. Navigate to `/Admin/Index`
3. Or click "Admin Dashboard" link if available

### What You'll See
- **Total Orders** - Count of all orders in system
- **Total Revenue** - Sum of all order amounts
- **Total Customers** - Count of all registered customers
- **Total Products** - Count of all items in inventory
- **Low Stock Products** - Count of items with less than 10 units

### Purpose
Quick overview of business metrics without detailed data

---

## 2. Stock Report

### Accessing Stock Report
1. From admin dashboard, click "Stock Report" or navigate to `/Admin/StockReport`
2. Page loads with all products

### How to Use

#### View All Products
```
The page displays a table with:
- Item ID
- Product Name
- Category
- Selling Price (customer sees this)
- Purchase Price (what was paid)
- Unit Type (kg, pieces, liters, etc.)
- Available Stock (current quantity)
- Stock Status (In Stock / Low Stock / Out of Stock)
- Expiry Date (when product expires)
- Stock Value (Total = Available Stock × Purchase Price)
```

#### Search for Products
```
1. Type product name or category name in search box
2. Click "Search" button
3. Table updates to show only matching products

Examples:
- Search "Rice" → Shows all rice products
- Search "Vegetables" → Shows all vegetable items
```

#### Understand Stock Status
```
🟢 GREEN "In Stock"        = 10 or more units
🟡 YELLOW "Low Stock"      = Less than 10 units (⚠️ Needs restocking soon)
🔴 RED "Out of Stock"      = 0 units (Need to order immediately)
```

#### Export Report
```
1. Click "Export CSV" button
2. File downloaded as "StockReport_YYYY_MM_DD.csv"
3. Open in Excel for analysis
   - Filter by category
   - Sort by stock value
   - Create charts
```

### Common Tasks

**Task: Find products that need restocking**
```
1. Look for items with "Low Stock" badge (yellow)
2. These items have less than 10 units
3. Click "Export CSV" to send to purchase team
```

**Task: Check total inventory value**
```
1. See "Total Stock Value" card at top
2. This is total value of all items = Σ (Stock × Purchase Price)
```

**Task: Find expired products**
```
1. Look at "Expiry Date" column
2. Note any dates in the past
3. Contact inventory team to remove
```

---

## 3. Order Report

### Accessing Order Report
1. From admin dashboard, click "Order Report" or navigate to `/Admin/OrderReport`
2. Page loads with all orders

### How to Use

#### View All Orders
```
The page displays a table with:
- Order ID (unique identifier)
- Order Date & Time
- Customer Name
- Customer Email
- Customer Phone
- Number of Items in order
- Total Amount (revenue from order)
- View Details button
```

#### Filter Orders by Date
```
1. Click on "Date Filter" dropdown
2. Select one option:
   - "All Orders" → Show all orders ever made
   - "Today" → Show only today's orders
   - "Last 7 Days" → Show orders from past week
   - "This Month" → Show orders from current month
3. Table updates automatically
```

#### Search for Specific Orders
```
1. Type in search box to find by:
   - Customer Name: "Ahmed Khan"
   - Customer Email: "ahmed@email.com"
   - Order ID: "15"
2. Click "Search" button
3. Table shows only matching orders

Note: You can combine search + filter
Example: Search "Ahmed" AND filter "Last 7 Days" 
         → Shows Ahmed's orders from last week only
```

#### View Order Details
```
1. Find the order in the table
2. Click "View Details" button
3. Page shows:
   - Customer information
   - All items in order
   - Quantity of each item
   - Unit price of each item
   - Total for that order
```

#### Understand Summary Cards
```
- Total Orders: Count of all orders in current view
- Total Revenue: Sum of all order amounts in current view
- Average Order Value: Total Revenue ÷ Total Orders
  Example: If 10 orders = Rs. 50,000
           Average = Rs. 50,000 ÷ 10 = Rs. 5,000
```

#### Export Report
```
1. Click "Export CSV" button
2. File downloaded as "OrderReport_YYYY_MM_DD.csv"
3. Open in Excel for detailed analysis
```

### Common Tasks

**Task: Find today's sales**
```
1. Select "Today" from date filter
2. See all orders placed today
3. Sum of "Total Amount" = today's revenue
```

**Task: Find high-value orders**
```
1. View all orders (All Orders filter)
2. Look for highest "Total Amount" values
3. Click "View Details" to see what was ordered
```

**Task: Track customer purchases**
```
1. Use search to find customer name
2. See all orders from that customer
3. Calculate how much they've spent
```

**Task: Analyze weekly sales**
```
1. Select "Last 7 Days" filter
2. "Total Revenue" card shows weekly sales
3. "Average Order Value" shows typical order size
4. Export and create charts in Excel
```

---

## 4. Order Details View

### Accessing Order Details
1. Go to Order Report
2. Find an order
3. Click "View Details" button

### What You'll See
```
- Order ID (e.g., "ORD-42")
- Order Date and Time
- Customer Name
- Customer Email
- Customer Phone

Items Table:
- Item Name
- Quantity Ordered
- Unit Price
- Total for that Item (Qty × Price)

Order Total:
- Sum of all items = Total Amount
```

### What to Do
```
1. Review what customer ordered
2. Check if inventory was properly updated
3. Verify prices are correct
4. If issue found, note order ID for investigation
```

---

## 5. CSV Export & Analysis

### Stock Report CSV Contents
```
Item ID, Item Name, Category, Selling Price, Purchase Price, 
Stock Unit, Available Stock, Status, Expiry Date, Stock Value
```

**How to use in Excel:**
1. Open CSV file in Excel
2. Sort by "Available Stock" to find low stock
3. Filter by "Status" to find inactive items
4. Create chart from "Stock Value" for inventory investment
5. Group by "Category" to analyze by department

### Order Report CSV Contents
```
Order ID, Order Date, Customer Name, Customer Email, 
Customer Phone, Total Amount, Item Count
```

**How to use in Excel:**
1. Open CSV file in Excel
2. Sort by "Total Amount" to find big orders
3. Sort by "Order Date" to see trends
4. Use "Item Count" to analyze average order size
5. Create PIVOT TABLE for sales by date

---

## Common Scenarios

### Scenario 1: Customer Support
**Question: "What did customer Ahmed Khan order?"**

Solution:
1. Go to Order Report
2. Search for "Ahmed Khan"
3. Click "View Details" for any order
4. See exactly what they ordered and when

### Scenario 2: Stock Management
**Task: Find all products that expire in 2025**

Solution:
1. Go to Stock Report
2. Look at "Expiry Date" column
3. Note products expiring soon
4. Contact manager to use or discard

### Scenario 3: Sales Analysis
**Question: "How much did we sell this month?"**

Solution:
1. Go to Order Report
2. Select "This Month" filter
3. See "Total Revenue" card
4. Click "Export CSV" for detailed analysis

### Scenario 4: Inventory Restock
**Task: Which products need urgent restock?**

Solution:
1. Go to Stock Report
2. Look for items with 🔴 "Out of Stock" (RED)
3. Also check 🟡 "Low Stock" (YELLOW) items
4. Export CSV and send to purchase team

### Scenario 5: Customer Spending
**Question: "Who is our best customer?"**

Solution:
1. Go to Order Report
2. Select "All Orders"
3. Click "Export CSV"
4. Open in Excel
5. Use PIVOT TABLE or SUMIF to calculate total spent by customer

---

## Tips & Tricks

### 1. Keyboard Shortcuts
- `Ctrl+F` to search within page
- `Ctrl+P` to print report
- `Ctrl+C` to copy table data

### 2. Mobile Friendly
- All tables scroll horizontally on small screens
- Touch-friendly buttons
- Works on phones and tablets

### 3. Browser Tools
- Right-click "Inspect" to see HTML
- Use Developer Tools (F12) to debug
- Check Network tab to see API calls

### 4. Data Accuracy
- Data updates in real-time from database
- Refresh page to see latest data
- Check timestamps for when data was last updated

### 5. Performance
- Stock Report loads fast with < 1000 products
- Order Report loads fast with < 10000 orders
- If slow, check database for missing indexes

---

## Troubleshooting

### Problem: "No data showing"
**Possible Causes:**
1. Database is empty
2. Wrong filters applied
3. Search term doesn't match anything

**Solution:**
1. Check if data exists in database
2. Remove all filters and search
3. Try different search terms

### Problem: "Page won't load"
**Possible Causes:**
1. Not logged in as admin
2. Admin session expired
3. Database connection issue

**Solution:**
1. Log out and log in again
2. Check Session settings
3. Verify database connection

### Problem: "CSV file is corrupted"
**Possible Causes:**
1. File download interrupted
2. Excel not opening properly
3. Encoding issue

**Solution:**
1. Download again
2. Try opening with Google Sheets
3. Check file encoding (should be UTF-8)

### Problem: "Search is slow"
**Possible Causes:**
1. Large amount of data
2. Slow database
3. Network issue

**Solution:**
1. Be more specific with search
2. Use filters to narrow down
3. Check internet connection

---

## Best Practices

✅ **DO:**
- Export reports weekly for backup
- Use filters to find specific data
- Share CSV files with team members
- Review reports regularly
- Document important findings

❌ **DON'T:**
- Delete orders or stock data manually
- Trust single report without verification
- Forget to export important data
- Leave admin session open unattended
- Share sensitive customer data

---

## Need More Help?

For questions about:
1. **How a feature works** - Check this guide
2. **Code questions** - Read AdminController.cs comments
3. **Database issues** - Check DbFreshMartContext.cs
4. **UI/Layout issues** - Check StockReport.cshtml or OrderReport.cshtml

Good luck! You've got this! 🎉
