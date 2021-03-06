USE [master]
GO
/****** Object:  Database [crmdb]    Script Date: 3/6/2021 4:19:38 PM ******/
CREATE DATABASE [crmdb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'crmdb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\crmdb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'crmdb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\crmdb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [crmdb] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [crmdb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [crmdb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [crmdb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [crmdb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [crmdb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [crmdb] SET ARITHABORT OFF 
GO
ALTER DATABASE [crmdb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [crmdb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [crmdb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [crmdb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [crmdb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [crmdb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [crmdb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [crmdb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [crmdb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [crmdb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [crmdb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [crmdb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [crmdb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [crmdb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [crmdb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [crmdb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [crmdb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [crmdb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [crmdb] SET  MULTI_USER 
GO
ALTER DATABASE [crmdb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [crmdb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [crmdb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [crmdb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [crmdb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [crmdb] SET QUERY_STORE = OFF
GO
USE [crmdb]
GO
/****** Object:  Table [dbo].[admins]    Script Date: 3/6/2021 4:19:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[admins](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[login] [nvarchar](20) NOT NULL,
	[password] [nvarchar](70) NOT NULL,
	[document_number] [nvarchar](20) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[surname] [nvarchar](100) NULL,
	[fathers_name] [nvarchar](100) NULL,
	[date_of_birth] [datetime] NOT NULL,
	[gender] [nvarchar](2) NOT NULL,
	[citizenship] [nvarchar](20) NOT NULL,
	[marital_status] [nvarchar](20) NOT NULL,
	[taxpayer_id] [nvarchar](20) NOT NULL,
	[address] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_admins] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[clients]    Script Date: 3/6/2021 4:19:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[clients](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[login] [nvarchar](20) NOT NULL,
	[password] [nvarchar](70) NOT NULL,
	[document_number] [nvarchar](20) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[surname] [nvarchar](100) NULL,
	[fathers_name] [nvarchar](100) NULL,
	[date_of_birth] [datetime] NOT NULL,
	[gender] [nvarchar](2) NOT NULL,
	[citizenship] [nvarchar](20) NOT NULL,
	[marital_status] [nvarchar](20) NOT NULL,
	[taxpayer_id] [nvarchar](20) NOT NULL,
	[address] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_clients] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[credit_applications]    Script Date: 3/6/2021 4:19:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[credit_applications](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[client_id] [int] NOT NULL,
	[purpose] [nvarchar](100) NOT NULL,
	[amount] [money] NOT NULL,
	[period] [int] NOT NULL,
	[is_approved] [bit] NOT NULL,
 CONSTRAINT [PK_credit_applications] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[forms]    Script Date: 3/6/2021 4:19:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[forms](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[client_id] [int] NOT NULL,
	[income] [money] NOT NULL,
	[credit_history] [int] NOT NULL,
	[defaults] [int] NOT NULL,
 CONSTRAINT [PK_forms] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[payments_schedules]    Script Date: 3/6/2021 4:19:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[payments_schedules](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[client_id] [int] NULL,
	[application_id] [int] NULL,
	[monthly_payment] [money] NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
 CONSTRAINT [PK_payments_schedules] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[credit_applications]  WITH CHECK ADD  CONSTRAINT [FK_credit_applications_clients] FOREIGN KEY([client_id])
REFERENCES [dbo].[clients] ([id])
GO
ALTER TABLE [dbo].[credit_applications] CHECK CONSTRAINT [FK_credit_applications_clients]
GO
ALTER TABLE [dbo].[forms]  WITH CHECK ADD  CONSTRAINT [FK_forms_clients] FOREIGN KEY([client_id])
REFERENCES [dbo].[clients] ([id])
GO
ALTER TABLE [dbo].[forms] CHECK CONSTRAINT [FK_forms_clients]
GO
ALTER TABLE [dbo].[payments_schedules]  WITH CHECK ADD  CONSTRAINT [FK_payments_schedules_credit_applications] FOREIGN KEY([client_id])
REFERENCES [dbo].[clients] ([id])
GO
ALTER TABLE [dbo].[payments_schedules] CHECK CONSTRAINT [FK_payments_schedules_credit_applications]
GO
USE [master]
GO
ALTER DATABASE [crmdb] SET  READ_WRITE 
GO
