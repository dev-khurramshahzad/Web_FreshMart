USE [dbFreshMart]
GO
/****** Object:  Table [dbo].[Admins]    Script Date: 5/2/2026 4:51:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admins](
	[AdminID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NULL,
	[Passwor] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[Status] [nvarchar](50) NULL,
	[Role] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 5/2/2026 4:51:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NULL,
	[CategoryDetails] [nvarchar](max) NULL,
	[CategoryImage] [nvarchar](max) NULL,
	[CategoryStatus] [nchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 5/2/2026 4:51:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NULL,
	[Email] [nvarchar](255) NULL,
	[Password] [nvarchar](max) NULL,
	[Phone] [nvarchar](20) NULL,
	[Address] [nvarchar](255) NULL,
	[City] [nvarchar](100) NULL,
	[State] [nvarchar](100) NULL,
	[ZipCode] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedbacks]    Script Date: 5/2/2026 4:51:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedbacks](
	[FeedbackID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerFID] [int] NULL,
	[ItemFID] [int] NULL,
	[FeedbackDate] [datetime] NULL,
	[Rating] [int] NULL,
	[Comments] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[FeedbackID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 5/2/2026 4:51:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[CategoryFID] [int] NULL,
	[SPrice] [decimal](10, 2) NULL,
	[PPrice] [decimal](10, 2) NULL,
	[StockUnit] [nvarchar](50) NULL,
	[AvailableStock] [float] NULL,
	[Status] [nvarchar](50) NULL,
	[ExpDate] [date] NULL,
	[Rating] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Image] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 5/2/2026 4:51:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderFID] [int] NULL,
	[ItemFID] [int] NULL,
	[Quantity] [int] NULL,
	[UnitPrice] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 5/2/2026 4:51:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerFID] [int] NULL,
	[OrderDate] [datetime] NULL,
	[TotalAmount] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Admins] ON 
GO
INSERT [dbo].[Admins] ([AdminID], [Username], [Passwor], [Email], [Status], [Role]) VALUES (1, N'Fatima', N'fat@123', N'fatimamajid@gmail.com', N'Active', N'Manager')
GO
INSERT [dbo].[Admins] ([AdminID], [Username], [Passwor], [Email], [Status], [Role]) VALUES (2, N'Noor', N'noor@123', N'noorzahid@gmail.com', N'Active', N'Manager')
GO
SET IDENTITY_INSERT [dbo].[Admins] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CategoryDetails], [CategoryImage], [CategoryStatus]) VALUES (2, N'Vegetables', N'Organic and Farm Vegetables', N'veg.jpg', N'InStock   ')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CategoryDetails], [CategoryImage], [CategoryStatus]) VALUES (3, N'Fruits', N'Fresh Seasonal Fruits', N'fruits.jpg', N'InStock   ')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CategoryDetails], [CategoryImage], [CategoryStatus]) VALUES (4, N'554', N'435', N'1.jpg', N'active    ')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CategoryDetails], [CategoryImage], [CategoryStatus]) VALUES (5, N'vegetables', N'345', N'1.jpg', N'InStock   ')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CategoryDetails], [CategoryImage], [CategoryStatus]) VALUES (6, N'apple', N'435', N'3.PNG', N'InStock   ')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 
GO
INSERT [dbo].[Customers] ([CustomerID], [FullName], [Email], [Password], [Phone], [Address], [City], [State], [ZipCode]) VALUES (1, N'Hassan', N'hassan@gmail.com', N'#123', N'0300-5678901', N'home', N'Gujranwala', N'Active', N'52000')
GO
INSERT [dbo].[Customers] ([CustomerID], [FullName], [Email], [Password], [Phone], [Address], [City], [State], [ZipCode]) VALUES (2, N'Ayesha Noor', N'ayeshanoor@gmail.com', N'#456', N'0300-5678901', N'home', N'Gujranwala', N'Active', N'52000')
GO
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[Feedbacks] ON 
GO
INSERT [dbo].[Feedbacks] ([FeedbackID], [CustomerFID], [ItemFID], [FeedbackDate], [Rating], [Comments]) VALUES (1, 1, 1, CAST(N'2026-04-25T14:51:00.000' AS DateTime), 8, N'Very Fresh and fast Delivery')
GO
INSERT [dbo].[Feedbacks] ([FeedbackID], [CustomerFID], [ItemFID], [FeedbackDate], [Rating], [Comments]) VALUES (2, 1, 1, CAST(N'2026-04-15T14:51:00.000' AS DateTime), 6, N'Very Fresh Product and fast Delivery')
GO
SET IDENTITY_INSERT [dbo].[Feedbacks] OFF
GO
SET IDENTITY_INSERT [dbo].[Items] ON 
GO
INSERT [dbo].[Items] ([ItemID], [Name], [CategoryFID], [SPrice], [PPrice], [StockUnit], [AvailableStock], [Status], [ExpDate], [Rating], [Description], [Image]) VALUES (1, N'Apple', 2, CAST(250.00 AS Decimal(10, 2)), CAST(200.00 AS Decimal(10, 2)), N'kg', 50, N'InStock', CAST(N'2026-04-30' AS Date), 8, N'Fresh', N'apple.jpg')
GO
INSERT [dbo].[Items] ([ItemID], [Name], [CategoryFID], [SPrice], [PPrice], [StockUnit], [AvailableStock], [Status], [ExpDate], [Rating], [Description], [Image]) VALUES (2, N'Potato', 2, CAST(80.00 AS Decimal(10, 2)), CAST(60.00 AS Decimal(10, 2)), N'kg', 80, N'InStock', CAST(N'2026-04-30' AS Date), 8, N'Fresh', N'potato.jpg')
GO
SET IDENTITY_INSERT [dbo].[Items] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 
GO
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderFID], [ItemFID], [Quantity], [UnitPrice]) VALUES (1, 1, 1, 2, CAST(250.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderFID], [ItemFID], [Quantity], [UnitPrice]) VALUES (2, 1, 1, 1, CAST(250.00 AS Decimal(10, 2)))
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 
GO
INSERT [dbo].[Orders] ([OrderID], [CustomerFID], [OrderDate], [TotalAmount]) VALUES (1, 1, CAST(N'2026-04-25T14:56:00.000' AS DateTime), CAST(740.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[Orders] ([OrderID], [CustomerFID], [OrderDate], [TotalAmount]) VALUES (2, 1, CAST(N'2026-04-15T14:56:00.000' AS DateTime), CAST(180.00 AS Decimal(10, 2)))
GO
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD  CONSTRAINT [FK_Feedbacks_Customers] FOREIGN KEY([CustomerFID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Feedbacks] CHECK CONSTRAINT [FK_Feedbacks_Customers]
GO
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD  CONSTRAINT [FK_Feedbacks_Items] FOREIGN KEY([ItemFID])
REFERENCES [dbo].[Items] ([ItemID])
GO
ALTER TABLE [dbo].[Feedbacks] CHECK CONSTRAINT [FK_Feedbacks_Items]
GO
ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Categories] FOREIGN KEY([CategoryFID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_Categories]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Items] FOREIGN KEY([ItemFID])
REFERENCES [dbo].[Items] ([ItemID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Items]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerFID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers]
GO
