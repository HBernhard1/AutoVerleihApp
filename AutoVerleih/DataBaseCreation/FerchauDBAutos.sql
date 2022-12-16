USE [Ferchau]
GO

/****** Object:  Table [dbo].[Autos]    Script Date: 16.12.2022 11:09:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Autos](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[Marke] [nvarchar](50) NULL,
	[Type] [nvarchar](20) NULL,
	[Farbe] [nvarchar](20) NULL,
	[MietPreis_HH] [numeric](18, 4) NULL,
	[MietPreis_TG] [numeric](18, 4) NULL,
	[MietPreis_WE] [numeric](18, 4) NULL,
	[KM_Stand] [int] NULL,
	[vermietet] [bit] NULL,
	[Bild] [nvarchar](100) NULL,
	[BildJpg] [image] NULL,
PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

