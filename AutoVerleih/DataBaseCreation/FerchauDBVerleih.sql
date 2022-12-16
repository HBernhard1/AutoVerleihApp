USE [Ferchau]
GO

/****** Object:  Table [dbo].[Verleih]    Script Date: 16.12.2022 11:10:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Verleih](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KundenId] [int] NOT NULL,
	[AutoId] [int] NOT NULL,
	[DT_Von] [datetime] NOT NULL,
	[DT_Bis] [datetime] NULL,
	[DT_Rueckgabe] [datetime] NULL,
	[KM_gefahren] [int] NULL
) ON [PRIMARY]
GO

