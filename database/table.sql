USE [Khoa_Luan]
GO
/****** Object:  Table [dbo].[CartItems]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartItems](
	[CartItem_ID] [int] IDENTITY(1,1) NOT NULL,
	[Cart_ID] [int] NULL,
	[Product_ID] [int] NULL,
	[Size] [varchar](15) NULL,
	[Quantity] [int] NOT NULL,
	[Price_AtTime] [decimal](18, 2) NOT NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[ProductName] [nvarchar](64) NULL,
PRIMARY KEY CLUSTERED 
(
	[CartItem_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carts]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carts](
	[Cart_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NULL,
	[Created_Date] [datetime] NULL,
	[Is_Active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Cart_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Category_Id] [int] IDENTITY(1,1) NOT NULL,
	[Category_Name] [varchar](64) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [date] NULL,
	[Image_Url] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Category_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChatHistory]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatHistory](
	[Chat_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Response] [nvarchar](max) NULL,
	[SentAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Chat_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailConfirmations]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailConfirmations](
	[Confirmation_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NOT NULL,
	[Confirmation_Code] [varchar](100) NOT NULL,
	[Expiration_Date] [datetime] NOT NULL,
	[Is_Used] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Confirmation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MailConfirmations]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailConfirmations](
	[Mail_ID] [int] IDENTITY(1,1) NOT NULL,
	[Order_ID] [int] NULL,
	[Email_Sent] [bit] NULL,
	[Sent_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Mail_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[OrderItem_ID] [int] IDENTITY(1,1) NOT NULL,
	[Order_ID] [int] NULL,
	[Product_ID] [int] NULL,
	[Size] [varchar](15) NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderItem_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Order_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NULL,
	[Order_Date] [datetime] NULL,
	[Total_Amount] [decimal](18, 2) NOT NULL,
	[Order_Status] [nvarchar](50) NULL,
	[Shipping_Address] [nvarchar](255) NULL,
	[Email] [varchar](50) NULL,
	[Phone_Number] [varchar](15) NULL,
	[Is_Confirmed] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Order_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OtpConfirmations]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OtpConfirmations](
	[Otp_ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Otp_Code] [varchar](10) NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Is_Used] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Otp_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Product_ID] [int] IDENTITY(1,1) NOT NULL,
	[Product_Name] [nvarchar](64) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Discount] [int] NOT NULL,
	[ViewCount] [int] NOT NULL,
	[AverageRating] [decimal](3, 2) NOT NULL,
	[TotalSold] [int] NOT NULL,
	[Category_Id] [int] NULL,
	[Created_Date] [date] NULL,
	[Image_Url] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Product_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductSize]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductSize](
	[ProductSize_ID] [int] IDENTITY(1,1) NOT NULL,
	[Product_ID] [int] NULL,
	[Size] [nvarchar](15) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price_AtTime] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductSize_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[Review_ID] [int] IDENTITY(1,1) NOT NULL,
	[Product_ID] [int] NULL,
	[User_ID] [int] NULL,
	[Rating] [int] NULL,
	[Comment] [nvarchar](255) NULL,
	[Review_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Review_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_Name] [nvarchar](50) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Full_Name] [nvarchar](255) NULL,
	[Role] [varchar](16) NULL,
	[Created_Date] [date] NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Address] [nvarchar](255) NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[Gender] [bit] NULL,
	[DateOfBirth] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WishList]    Script Date: 4/15/2025 7:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WishList](
	[WishList_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NULL,
	[Product_ID] [int] NULL,
	[Added_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[WishList_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ChatHistory] ADD  DEFAULT (getdate()) FOR [SentAt]
GO
ALTER TABLE [dbo].[EmailConfirmations] ADD  DEFAULT ((0)) FOR [Is_Used]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((0)) FOR [Is_Confirmed]
GO
ALTER TABLE [dbo].[OtpConfirmations] ADD  DEFAULT (getdate()) FOR [Created_At]
GO
ALTER TABLE [dbo].[OtpConfirmations] ADD  DEFAULT ((0)) FOR [Is_Used]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [ViewCount]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0.00)) FOR [AverageRating]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [TotalSold]
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD FOREIGN KEY([Cart_ID])
REFERENCES [dbo].[Carts] ([Cart_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([Product_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Carts]  WITH CHECK ADD FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChatHistory]  WITH CHECK ADD FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmailConfirmations]  WITH CHECK ADD FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MailConfirmations]  WITH CHECK ADD FOREIGN KEY([Order_ID])
REFERENCES [dbo].[Orders] ([Order_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD FOREIGN KEY([Order_ID])
REFERENCES [dbo].[Orders] ([Order_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([Product_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([Category_Id])
REFERENCES [dbo].[Category] ([Category_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductSize]  WITH CHECK ADD FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([Product_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([Product_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WishList]  WITH CHECK ADD FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([Product_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WishList]  WITH CHECK ADD FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
ON DELETE CASCADE
GO
