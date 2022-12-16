USE [Ferchau]
GO

/****** Object:  Table [dbo].[Kunden]    Script Date: 16.12.2022 11:10:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Kunden](
	[KundenID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Plz] [nvarchar](10) NULL,
	[Ort] [nvarchar](50) NULL,
	[Strasse] [nvarchar](50) NULL,
	[DT_AnAend] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[KundenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

