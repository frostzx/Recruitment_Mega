USE [master]
GO
/****** Object:  Database [recruitment_Mega]    Script Date: 15/11/2024 17:30:23 ******/
CREATE DATABASE [recruitment_Mega]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'recrutment_Mega', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\recrutment_Mega.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'recrutment_Mega_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\recrutment_Mega_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [recruitment_Mega] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [recruitment_Mega].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [recruitment_Mega] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [recruitment_Mega] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [recruitment_Mega] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [recruitment_Mega] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [recruitment_Mega] SET ARITHABORT OFF 
GO
ALTER DATABASE [recruitment_Mega] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [recruitment_Mega] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [recruitment_Mega] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [recruitment_Mega] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [recruitment_Mega] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [recruitment_Mega] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [recruitment_Mega] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [recruitment_Mega] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [recruitment_Mega] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [recruitment_Mega] SET  DISABLE_BROKER 
GO
ALTER DATABASE [recruitment_Mega] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [recruitment_Mega] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [recruitment_Mega] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [recruitment_Mega] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [recruitment_Mega] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [recruitment_Mega] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [recruitment_Mega] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [recruitment_Mega] SET RECOVERY FULL 
GO
ALTER DATABASE [recruitment_Mega] SET  MULTI_USER 
GO
ALTER DATABASE [recruitment_Mega] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [recruitment_Mega] SET DB_CHAINING OFF 
GO
ALTER DATABASE [recruitment_Mega] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [recruitment_Mega] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [recruitment_Mega] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [recruitment_Mega] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'recruitment_Mega', N'ON'
GO
ALTER DATABASE [recruitment_Mega] SET QUERY_STORE = ON
GO
ALTER DATABASE [recruitment_Mega] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [recruitment_Mega]
GO
/****** Object:  Table [dbo].[ms_storage_location]    Script Date: 15/11/2024 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ms_storage_location](
	[location_id] [varchar](10) NOT NULL,
	[location_name] [varchar](100) NULL,
 CONSTRAINT [PK_ms_storage_location] PRIMARY KEY CLUSTERED 
(
	[location_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ms_user]    Script Date: 15/11/2024 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ms_user](
	[user_id] [bigint] IDENTITY(1,1) NOT NULL,
	[user_name] [varchar](20) NULL,
	[password] [varchar](50) NULL,
	[is_active] [bit] NOT NULL,
 CONSTRAINT [PK_ms_user] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tr_bpkb]    Script Date: 15/11/2024 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tr_bpkb](
	[agreement_number] [varchar](100) NOT NULL,
	[bpkb_no] [varchar](100) NULL,
	[branch_id] [varchar](10) NULL,
	[bpkb_date] [datetime] NULL,
	[faktur_no] [varchar](100) NULL,
	[faktur_date] [datetime] NULL,
	[location_id] [varchar](10) NULL,
	[police_no] [varchar](20) NULL,
	[bpkb_date_in] [datetime] NULL,
	[created_by] [varchar](20) NULL,
	[created_on] [datetime] NULL,
	[last_updated_by] [varchar](20) NULL,
	[last_updated_on] [datetime] NULL,
 CONSTRAINT [PK_tr_bpkb] PRIMARY KEY CLUSTERED 
(
	[agreement_number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tr_bpkb]  WITH CHECK ADD  CONSTRAINT [FK_tr_bpkb_ms_storage_location] FOREIGN KEY([location_id])
REFERENCES [dbo].[ms_storage_location] ([location_id])
GO
ALTER TABLE [dbo].[tr_bpkb] CHECK CONSTRAINT [FK_tr_bpkb_ms_storage_location]
GO
USE [master]
GO
ALTER DATABASE [recruitment_Mega] SET  READ_WRITE 
GO
