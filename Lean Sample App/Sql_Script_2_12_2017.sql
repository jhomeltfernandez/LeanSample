USE [master]
GO
/****** Object:  Database [RefresDb]    Script Date: 2/12/2017 11:34:22 PM ******/
CREATE DATABASE [RefresDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RefresDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQL2014\MSSQL\DATA\RefresDb.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'RefresDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQL2014\MSSQL\DATA\RefresDb_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [RefresDb] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RefresDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RefresDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RefresDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RefresDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RefresDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RefresDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [RefresDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RefresDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RefresDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RefresDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RefresDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RefresDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RefresDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RefresDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RefresDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RefresDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RefresDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RefresDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RefresDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RefresDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RefresDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RefresDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RefresDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RefresDb] SET RECOVERY FULL 
GO
ALTER DATABASE [RefresDb] SET  MULTI_USER 
GO
ALTER DATABASE [RefresDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RefresDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RefresDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RefresDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [RefresDb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'RefresDb', N'ON'
GO
USE [RefresDb]
GO
/****** Object:  User [JAdmin]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE USER [JAdmin] FOR LOGIN [JAdmin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [JAdmin]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [JAdmin]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [JAdmin]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [JAdmin]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [JAdmin]
GO
ALTER ROLE [db_datareader] ADD MEMBER [JAdmin]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [JAdmin]
GO
ALTER ROLE [db_denydatareader] ADD MEMBER [JAdmin]
GO
ALTER ROLE [db_denydatawriter] ADD MEMBER [JAdmin]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[ProfileId] [int] NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Status] [bit] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Destinations]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Destinations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_dbo.Destinations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Drivers]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Drivers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactNumber] [nvarchar](max) NULL,
	[Deleted] [bit] NULL,
	[DateHired] [datetime] NULL,
	[DateOut] [datetime] NULL,
	[Created] [datetime] NULL,
	[Modified] [datetime] NULL,
	[FirstName] [nvarchar](max) NULL,
	[MiddleName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Gender] [int] NOT NULL,
	[Age] [int] NOT NULL,
	[BirthDate] [datetime] NOT NULL,
	[Address] [nvarchar](max) NULL,
	[ImageId] [int] NULL,
 CONSTRAINT [PK_dbo.Drivers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LayoutSettings]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LayoutSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FixedLayout] [bit] NOT NULL,
	[BoxedLayout] [bit] NOT NULL,
	[ToggleSideBar] [bit] NOT NULL,
	[SideBarExpandOnHover] [bit] NOT NULL,
	[ToggleRightSideBarSlide] [bit] NOT NULL,
	[ToggleRightSideBarSkin] [bit] NOT NULL,
	[Skin] [int] NOT NULL,
 CONSTRAINT [PK_dbo.LayoutSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OtherExpences]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OtherExpences](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[SaleId] [int] NULL,
 CONSTRAINT [PK_dbo.OtherExpences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Rates]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DestinationId] [int] NOT NULL,
	[TruckId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[HelperCost] [decimal](18, 2) NOT NULL,
	[WaterCost] [decimal](18, 2) NOT NULL,
	[Deleted] [bit] NULL,
	[DriverCost] [decimal](18, 2) NULL,
 CONSTRAINT [PK_dbo.Rates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sales]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RateId] [int] NOT NULL,
	[Gross] [decimal](18, 2) NOT NULL,
	[FuelCost] [decimal](18, 2) NOT NULL,
	[Less] [decimal](18, 2) NOT NULL,
	[Net] [decimal](18, 2) NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_dbo.Sales] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Trucks]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trucks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Capacity] [decimal](18, 2) NULL,
	[DriverId] [int] NULL,
	[PlateNumber] [nvarchar](max) NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_dbo.Trucks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserImageDirectories]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserImageDirectories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Path] [nvarchar](max) NULL,
	[UserId] [nvarchar](max) NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NULL,
 CONSTRAINT [PK_dbo.UserImageDirectories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserImages]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserImages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[FileType] [nvarchar](max) NULL,
	[ImageDirectoryId] [int] NULL,
	[Path] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.UserImages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserProfiles]    Script Date: 2/12/2017 11:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Created] [datetime] NULL,
	[Modified] [datetime] NULL,
	[FirstName] [nvarchar](max) NULL,
	[MiddleName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Gender] [int] NOT NULL,
	[Age] [int] NOT NULL,
	[BirthDate] [datetime] NOT NULL,
	[Address] [nvarchar](max) NULL,
	[ImageId] [int] NULL,
 CONSTRAINT [PK_dbo.UserProfiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'ffc5f3f2-f00a-4734-b3be-7ca01df76269', N'Admin')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'4d76f736-0dea-4305-bfa2-ec2cbafcafa5', N'Driver')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'c33dd952-6428-4a46-b5cd-250a1ec3020f', N'SuperAdmin')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'07b613bb-aa29-43d5-acc3-84cdd6450f90', N'c33dd952-6428-4a46-b5cd-250a1ec3020f')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'07b613bb-aa29-43d5-acc3-84cdd6450f90', N'ffc5f3f2-f00a-4734-b3be-7ca01df76269')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5df0767c-d602-4402-a177-dab38418b312', N'4d76f736-0dea-4305-bfa2-ec2cbafcafa5')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5df0767c-d602-4402-a177-dab38418b312', N'c33dd952-6428-4a46-b5cd-250a1ec3020f')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5df0767c-d602-4402-a177-dab38418b312', N'ffc5f3f2-f00a-4734-b3be-7ca01df76269')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'660ffd68-148a-4623-86f4-7a687e80cca6', N'c33dd952-6428-4a46-b5cd-250a1ec3020f')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'75764cb6-1129-4622-9907-9ab2f2c343f0', N'ffc5f3f2-f00a-4734-b3be-7ca01df76269')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e80fbc8f-8ec3-4c09-a886-653711439f74', N'c33dd952-6428-4a46-b5cd-250a1ec3020f')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e80fbc8f-8ec3-4c09-a886-653711439f74', N'ffc5f3f2-f00a-4734-b3be-7ca01df76269')
INSERT [dbo].[AspNetUsers] ([Id], [ProfileId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Status], [Deleted]) VALUES (N'07b613bb-aa29-43d5-acc3-84cdd6450f90', 2, N'fortgeesntearciccount@gmail.com', 1, N'ALz8WkkPgEFx3ELZu+g0ze5PJRctX6t/GCRCLJEXRDqJ5Ej9zWrKKTlcnaQY64QbjA==', N'7e205316-2038-4aa2-a5c7-195d29a5d1e3', N'234324234324', 0, 0, NULL, 1, 0, N'fortgeesntearciccount@gmail.com', 1, 1)
INSERT [dbo].[AspNetUsers] ([Id], [ProfileId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Status], [Deleted]) VALUES (N'5df0767c-d602-4402-a177-dab38418b312', 4, N'jazonnazin@gmail.com', 0, N'ADet7kaw9Xcb+RLGLr2Z5Y8HFilEqvdgZnz1oUnoo5Ue1W7c64h2ix0M78FoRe1fLg==', N'8927d10b-1407-4c1e-8897-7c4589212995', N'xcvxcv', 0, 0, NULL, 1, 0, N'jazonnazin@gmail.com', 1, 1)
INSERT [dbo].[AspNetUsers] ([Id], [ProfileId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Status], [Deleted]) VALUES (N'660ffd68-148a-4623-86f4-7a687e80cca6', 1, N'FORtGeEsNtEaRcICcount@gmdddail.com', 1, NULL, N'89f27caa-3b6b-4994-827b-d926810e7d91', N'23423423', 0, 0, NULL, 1, 0, N'rist@risty.com', 1, 1)
INSERT [dbo].[AspNetUsers] ([Id], [ProfileId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Status], [Deleted]) VALUES (N'75764cb6-1129-4622-9907-9ab2f2c343f0', 5, N'admin@demo.com', 1, N'ANmfNChUSkMGGgp5Nm3DJQFoCEGvelo7ywVr3oLZgCgntbGgq78u1N7v4NlrWZkryQ==', N'a55f9d2a-b669-4e72-ba6a-1704867628fa', N'1234567890', 0, 0, NULL, 1, 0, N'admin@demo.com', 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [ProfileId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Status], [Deleted]) VALUES (N'e80fbc8f-8ec3-4c09-a886-653711439f74', 1005, N'jhomeltfernandez@gmail.com', 1, N'AAVUiPvSCI2I/lz2xKcxgP+0EOpe30Fb/qMS3tZ4ruqlPSZ7m3NamH0529wkQCn4ow==', N'49a056b5-53ca-4184-94b4-fd9d6bbd5680', N'+639309646717', 0, 0, NULL, 1, 0, N'jhomeltfernandez@gmail.com', 1, 0)
SET IDENTITY_INSERT [dbo].[Destinations] ON 

INSERT [dbo].[Destinations] ([Id], [Name], [Deleted]) VALUES (1, N'Panabo', NULL)
INSERT [dbo].[Destinations] ([Id], [Name], [Deleted]) VALUES (2, N'Cotabato', 0)
INSERT [dbo].[Destinations] ([Id], [Name], [Deleted]) VALUES (3, N'Panabo', 1)
INSERT [dbo].[Destinations] ([Id], [Name], [Deleted]) VALUES (4, N'Davao', 0)
INSERT [dbo].[Destinations] ([Id], [Name], [Deleted]) VALUES (5, N'Cagayan', 0)
INSERT [dbo].[Destinations] ([Id], [Name], [Deleted]) VALUES (6, NULL, 1)
INSERT [dbo].[Destinations] ([Id], [Name], [Deleted]) VALUES (7, NULL, 1)
INSERT [dbo].[Destinations] ([Id], [Name], [Deleted]) VALUES (8, NULL, 1)
SET IDENTITY_INSERT [dbo].[Destinations] OFF
SET IDENTITY_INSERT [dbo].[Drivers] ON 

INSERT [dbo].[Drivers] ([Id], [ContactNumber], [Deleted], [DateHired], [DateOut], [Created], [Modified], [FirstName], [MiddleName], [LastName], [Gender], [Age], [BirthDate], [Address], [ImageId]) VALUES (1, N'34234', 0, CAST(N'2016-02-01 00:00:00.000' AS DateTime), CAST(N'2016-02-01 00:00:00.000' AS DateTime), CAST(N'2016-02-03 12:31:37.463' AS DateTime), CAST(N'2017-01-26 20:31:26.127' AS DateTime), N'Risty', N'Risty', N'Risty', 1, 28, CAST(N'1989-02-03 00:00:00.000' AS DateTime), N'Risty, Risty, Risty', 3)
INSERT [dbo].[Drivers] ([Id], [ContactNumber], [Deleted], [DateHired], [DateOut], [Created], [Modified], [FirstName], [MiddleName], [LastName], [Gender], [Age], [BirthDate], [Address], [ImageId]) VALUES (2, N'234324234324', 0, CAST(N'2016-02-25 00:00:00.000' AS DateTime), CAST(N'2016-02-01 00:00:00.000' AS DateTime), CAST(N'2016-02-03 12:43:44.740' AS DateTime), CAST(N'2017-01-26 20:31:19.213' AS DateTime), N'IEnumerable', N'IEnumerable', N'IEnumerable', 0, 28, CAST(N'1989-02-03 00:00:00.000' AS DateTime), N'IEnumerable, IEnumerable, IEnumerable', 4)
SET IDENTITY_INSERT [dbo].[Drivers] OFF
SET IDENTITY_INSERT [dbo].[OtherExpences] ON 

INSERT [dbo].[OtherExpences] ([Id], [Name], [Amount], [SaleId]) VALUES (1, N'Chicks', CAST(150.00 AS Decimal(18, 2)), 1)
INSERT [dbo].[OtherExpences] ([Id], [Name], [Amount], [SaleId]) VALUES (3, N'sdsdg', CAST(456.00 AS Decimal(18, 2)), 2)
INSERT [dbo].[OtherExpences] ([Id], [Name], [Amount], [SaleId]) VALUES (5, N'Chick3', CAST(350.00 AS Decimal(18, 2)), 4)
INSERT [dbo].[OtherExpences] ([Id], [Name], [Amount], [SaleId]) VALUES (1003, N'AAAAaaa', CAST(0.00 AS Decimal(18, 2)), 2)
SET IDENTITY_INSERT [dbo].[OtherExpences] OFF
SET IDENTITY_INSERT [dbo].[Rates] ON 

INSERT [dbo].[Rates] ([Id], [DestinationId], [TruckId], [Amount], [HelperCost], [WaterCost], [Deleted], [DriverCost]) VALUES (1, 5, 3, CAST(1000.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(1300.00 AS Decimal(18, 2)), 0, CAST(200.00 AS Decimal(18, 2)))
INSERT [dbo].[Rates] ([Id], [DestinationId], [TruckId], [Amount], [HelperCost], [WaterCost], [Deleted], [DriverCost]) VALUES (2, 5, 2, CAST(1500.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), 1, CAST(100.00 AS Decimal(18, 2)))
INSERT [dbo].[Rates] ([Id], [DestinationId], [TruckId], [Amount], [HelperCost], [WaterCost], [Deleted], [DriverCost]) VALUES (3, 4, 3, CAST(1.00 AS Decimal(18, 2)), CAST(324234.00 AS Decimal(18, 2)), CAST(324324.00 AS Decimal(18, 2)), 1, CAST(32432.00 AS Decimal(18, 2)))
INSERT [dbo].[Rates] ([Id], [DestinationId], [TruckId], [Amount], [HelperCost], [WaterCost], [Deleted], [DriverCost]) VALUES (4, 2, 2, CAST(1000.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(250.00 AS Decimal(18, 2)), 1, CAST(100.00 AS Decimal(18, 2)))
INSERT [dbo].[Rates] ([Id], [DestinationId], [TruckId], [Amount], [HelperCost], [WaterCost], [Deleted], [DriverCost]) VALUES (5, 2, 3, CAST(1000.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), 0, CAST(150.00 AS Decimal(18, 2)))
INSERT [dbo].[Rates] ([Id], [DestinationId], [TruckId], [Amount], [HelperCost], [WaterCost], [Deleted], [DriverCost]) VALUES (6, 4, 3, CAST(100.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(122.00 AS Decimal(18, 2)), 0, CAST(100.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Rates] OFF
SET IDENTITY_INSERT [dbo].[Sales] ON 

INSERT [dbo].[Sales] ([Id], [RateId], [Gross], [FuelCost], [Less], [Net], [UserId], [Date]) VALUES (1, 2, CAST(1500.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(400.00 AS Decimal(18, 2)), CAST(750.00 AS Decimal(18, 2)), N'risty@ristymamparair.com', CAST(N'2016-02-25 00:00:00.000' AS DateTime))
INSERT [dbo].[Sales] ([Id], [RateId], [Gross], [FuelCost], [Less], [Net], [UserId], [Date]) VALUES (2, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(350.00 AS Decimal(18, 2)), CAST(1700.00 AS Decimal(18, 2)), CAST(-1506.00 AS Decimal(18, 2)), N'risty@ristymamparair.com', CAST(N'2016-02-04 00:00:00.000' AS DateTime))
INSERT [dbo].[Sales] ([Id], [RateId], [Gross], [FuelCost], [Less], [Net], [UserId], [Date]) VALUES (4, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1700.00 AS Decimal(18, 2)), CAST(-1050.00 AS Decimal(18, 2)), N'risty@ristymamparair.com', CAST(N'2016-02-18 00:00:00.000' AS DateTime))
INSERT [dbo].[Sales] ([Id], [RateId], [Gross], [FuelCost], [Less], [Net], [UserId], [Date]) VALUES (1003, 2, CAST(1500.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(400.00 AS Decimal(18, 2)), CAST(400.00 AS Decimal(18, 2)), N'risty@ristymamparair.com', CAST(N'2016-02-25 00:00:00.000' AS DateTime))
INSERT [dbo].[Sales] ([Id], [RateId], [Gross], [FuelCost], [Less], [Net], [UserId], [Date]) VALUES (1004, 2, CAST(1500.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(400.00 AS Decimal(18, 2)), CAST(900.00 AS Decimal(18, 2)), N'risty@ristymamparair.com', CAST(N'2016-02-25 00:00:00.000' AS DateTime))
INSERT [dbo].[Sales] ([Id], [RateId], [Gross], [FuelCost], [Less], [Net], [UserId], [Date]) VALUES (1006, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(1700.00 AS Decimal(18, 2)), CAST(-1000.00 AS Decimal(18, 2)), N'risty@ristymamparair.com', CAST(N'2016-02-18 00:00:00.000' AS DateTime))
INSERT [dbo].[Sales] ([Id], [RateId], [Gross], [FuelCost], [Less], [Net], [UserId], [Date]) VALUES (1008, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(1700.00 AS Decimal(18, 2)), CAST(-1000.00 AS Decimal(18, 2)), N'risty@ristymamparair.com', CAST(N'2016-02-18 00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Sales] OFF
SET IDENTITY_INSERT [dbo].[Trucks] ON 

INSERT [dbo].[Trucks] ([Id], [Name], [Capacity], [DriverId], [PlateNumber], [Deleted]) VALUES (2, N'Truck1', CAST(2.50 AS Decimal(18, 2)), 2, N'sdasdasdasd', 0)
INSERT [dbo].[Trucks] ([Id], [Name], [Capacity], [DriverId], [PlateNumber], [Deleted]) VALUES (3, N'truck 2', CAST(1.00 AS Decimal(18, 2)), 1, N'234324QDZD', 0)
INSERT [dbo].[Trucks] ([Id], [Name], [Capacity], [DriverId], [PlateNumber], [Deleted]) VALUES (4, N'kghjghjgj', CAST(654.00 AS Decimal(18, 2)), 1, N'3213321', 1)
SET IDENTITY_INSERT [dbo].[Trucks] OFF
SET IDENTITY_INSERT [dbo].[UserImages] ON 

INSERT [dbo].[UserImages] ([Id], [Name], [FileType], [ImageDirectoryId], [Path]) VALUES (1, N'default_avatar_male.jpg', NULL, NULL, N'/Images/User')
INSERT [dbo].[UserImages] ([Id], [Name], [FileType], [ImageDirectoryId], [Path]) VALUES (2, N'CxWim__wood-texture_3.jpg', NULL, NULL, N'/Images/User/risty@ristymamparair.com')
INSERT [dbo].[UserImages] ([Id], [Name], [FileType], [ImageDirectoryId], [Path]) VALUES (3, N'PRxPt__2013-12-26 10.58.37.jpg', NULL, NULL, N'/Images/User/Driver IEnumerable1989')
INSERT [dbo].[UserImages] ([Id], [Name], [FileType], [ImageDirectoryId], [Path]) VALUES (4, N'm1KKF__JES25.jpg', NULL, NULL, N'/Images/User/Driver IEnumerable1989')
INSERT [dbo].[UserImages] ([Id], [Name], [FileType], [ImageDirectoryId], [Path]) VALUES (5, N'default_avatar_male.jpg', NULL, NULL, N'/Images/User')
INSERT [dbo].[UserImages] ([Id], [Name], [FileType], [ImageDirectoryId], [Path]) VALUES (6, N'default_avatar_male.jpg', NULL, NULL, N'/Images/User')
INSERT [dbo].[UserImages] ([Id], [Name], [FileType], [ImageDirectoryId], [Path]) VALUES (1006, N'2Da4u__myphoto.png', NULL, NULL, N'/Images/User/jhomeltfernandez@gmail.com')
SET IDENTITY_INSERT [dbo].[UserImages] OFF
SET IDENTITY_INSERT [dbo].[UserProfiles] ON 

INSERT [dbo].[UserProfiles] ([Id], [Created], [Modified], [FirstName], [MiddleName], [LastName], [Gender], [Age], [BirthDate], [Address], [ImageId]) VALUES (1, CAST(N'2016-02-03 07:20:57.117' AS DateTime), CAST(N'2016-02-03 07:20:57.117' AS DateTime), N'Risty', N'CKJHJK', N'Mamparair', 1, 27, CAST(N'1989-02-03 00:00:00.000' AS DateTime), N'Kidapwan, Cotabato', 1)
INSERT [dbo].[UserProfiles] ([Id], [Created], [Modified], [FirstName], [MiddleName], [LastName], [Gender], [Age], [BirthDate], [Address], [ImageId]) VALUES (2, CAST(N'2016-02-03 07:27:02.810' AS DateTime), CAST(N'2016-02-23 11:48:07.333' AS DateTime), N'Risty', N'Risty', N'Risty', 1, 27, CAST(N'1989-02-03 00:00:00.000' AS DateTime), N'Kidapawan, North Cotabato', 2)
INSERT [dbo].[UserProfiles] ([Id], [Created], [Modified], [FirstName], [MiddleName], [LastName], [Gender], [Age], [BirthDate], [Address], [ImageId]) VALUES (3, CAST(N'2016-02-03 07:45:45.633' AS DateTime), CAST(N'2016-02-03 07:45:45.633' AS DateTime), N'test ', N'dsfsdf', N'Driver', 1, 27, CAST(N'1989-02-03 00:00:00.000' AS DateTime), N'Kabacan, Cotabato', 1)
INSERT [dbo].[UserProfiles] ([Id], [Created], [Modified], [FirstName], [MiddleName], [LastName], [Gender], [Age], [BirthDate], [Address], [ImageId]) VALUES (4, CAST(N'2016-02-23 11:30:56.617' AS DateTime), CAST(N'2016-02-23 12:25:04.480' AS DateTime), N'awsdf', N'fcbcvb', N'cvcvbcvbcvb', 1, 28, CAST(N'1988-02-23 00:00:00.000' AS DateTime), NULL, 5)
INSERT [dbo].[UserProfiles] ([Id], [Created], [Modified], [FirstName], [MiddleName], [LastName], [Gender], [Age], [BirthDate], [Address], [ImageId]) VALUES (5, CAST(N'2016-09-26 00:00:43.287' AS DateTime), CAST(N'2016-09-26 00:01:31.103' AS DateTime), N'Admin', N'Admin', N'Demo', 0, 27, CAST(N'1989-11-08 00:00:00.000' AS DateTime), N'Kabacan, North Cotabato', 6)
INSERT [dbo].[UserProfiles] ([Id], [Created], [Modified], [FirstName], [MiddleName], [LastName], [Gender], [Age], [BirthDate], [Address], [ImageId]) VALUES (1005, CAST(N'2017-02-12 23:09:45.013' AS DateTime), CAST(N'2017-02-12 23:09:45.013' AS DateTime), N'Jhomel', N'Tobias', N'Fernandez', 0, 28, CAST(N'1989-11-02 00:00:00.000' AS DateTime), N'Kabacan, North Cotabato', 1006)
SET IDENTITY_INSERT [dbo].[UserProfiles] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [RoleNameIndex]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_RoleId]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UserNameIndex]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ImageId]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_ImageId] ON [dbo].[Drivers]
(
	[ImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RateId]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_RateId] ON [dbo].[Sales]
(
	[RateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DriverId]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_DriverId] ON [dbo].[Trucks]
(
	[DriverId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ImageDirectoryId]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_ImageDirectoryId] ON [dbo].[UserImages]
(
	[ImageDirectoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ImageId]    Script Date: 2/12/2017 11:34:23 PM ******/
CREATE NONCLUSTERED INDEX [IX_ImageId] ON [dbo].[UserProfiles]
(
	[ImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Drivers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Drivers_dbo.UserImages_ImageId] FOREIGN KEY([ImageId])
REFERENCES [dbo].[UserImages] ([Id])
GO
ALTER TABLE [dbo].[Drivers] CHECK CONSTRAINT [FK_dbo.Drivers_dbo.UserImages_ImageId]
GO
ALTER TABLE [dbo].[Rates]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Rates_dbo.Destinations_DestinationId] FOREIGN KEY([DestinationId])
REFERENCES [dbo].[Destinations] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rates] CHECK CONSTRAINT [FK_dbo.Rates_dbo.Destinations_DestinationId]
GO
ALTER TABLE [dbo].[Rates]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Rates_dbo.Trucks_TruckId] FOREIGN KEY([TruckId])
REFERENCES [dbo].[Trucks] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rates] CHECK CONSTRAINT [FK_dbo.Rates_dbo.Trucks_TruckId]
GO
ALTER TABLE [dbo].[UserImages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserImages_dbo.UserImageDirectories_ImageDirectoryId] FOREIGN KEY([ImageDirectoryId])
REFERENCES [dbo].[UserImageDirectories] ([Id])
GO
ALTER TABLE [dbo].[UserImages] CHECK CONSTRAINT [FK_dbo.UserImages_dbo.UserImageDirectories_ImageDirectoryId]
GO
ALTER TABLE [dbo].[UserProfiles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserProfiles_dbo.UserImages_ImageId] FOREIGN KEY([ImageId])
REFERENCES [dbo].[UserImages] ([Id])
GO
ALTER TABLE [dbo].[UserProfiles] CHECK CONSTRAINT [FK_dbo.UserProfiles_dbo.UserImages_ImageId]
GO
USE [master]
GO
ALTER DATABASE [RefresDb] SET  READ_WRITE 
GO
