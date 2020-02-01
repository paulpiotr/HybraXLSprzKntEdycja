SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
ALTER TABLE [CDN].[el_CRMBranzeOpisy_KntKarty] DROP CONSTRAINT [FK_ECRMBO_KK]
GO

ALTER TABLE [CDN].[el_CRMBranzeOpisy_KntKarty] DROP CONSTRAINT [FK_ECRMBO_ECBO]
GO

DROP TABLE [CDN].[el_CRMBranzeOpisy_KntKarty]
GO

ALTER TABLE [CDN].[el_CRMBranzeOpisy] DROP CONSTRAINT [FK_ECBO_SLW]
GO

ALTER TABLE [CDN].[el_CRMBranzeOpisy] DROP CONSTRAINT [PK_ElBranOpisID]
GO

ALTER TABLE [CDN].[el_CRMBranzeOpisy] DROP COLUMN [ElBranOpisID]
GO
*/

ALTER TABLE [CDN].[el_CRMBranzeOpisy] WITH CHECK ADD CONSTRAINT [FK_ECBO_SLW] FOREIGN KEY([branzaID])
REFERENCES [CDN].[Slowniki] ([SLW_ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [CDN].[el_CRMBranzeOpisy] CHECK CONSTRAINT [FK_ECBO_SLW]
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'el_CRMBranzeOpisy' AND COLUMN_NAME = 'ElBranOpisID')
BEGIN
	ALTER TABLE [CDN].[el_CRMBranzeOpisy] ADD ElBranOpisID INT IDENTITY(1,1)
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME='el_CRMBranzeOpisy' AND CONSTRAINT_NAME = 'PK_ElBranOpisID')
BEGIN

ALTER TABLE [CDN].[el_CRMBranzeOpisy] ADD CONSTRAINT [PK_ElBranOpisID] PRIMARY KEY CLUSTERED 
(
	[ElBranOpisID] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

END
GO

CREATE TABLE [CDN].[el_CRMBranzeOpisy_KntKarty](
	[Knt_Karty_GIDNumer] [int] NOT NULL,
	[el_CRMBranzeOpisy_ElBranOpisID] [int] NOT NULL,
 CONSTRAINT [el_BraKntKart_Primary] PRIMARY KEY CLUSTERED 
(
	[Knt_Karty_GIDNumer] ASC,
	[el_CRMBranzeOpisy_ElBranOpisID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [CDN].[el_CRMBranzeOpisy_KntKarty] WITH CHECK ADD CONSTRAINT [FK_ECRMBO_ECBO] FOREIGN KEY([el_CRMBranzeOpisy_ElBranOpisID])
REFERENCES [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [CDN].[el_CRMBranzeOpisy_KntKarty] CHECK CONSTRAINT [FK_ECRMBO_ECBO]
GO

ALTER TABLE [CDN].[el_CRMBranzeOpisy_KntKarty] WITH CHECK ADD CONSTRAINT [FK_ECRMBO_KK] FOREIGN KEY([Knt_Karty_GIDNumer])
REFERENCES [CDN].[KntKarty] ([Knt_GIDNumer])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [CDN].[el_CRMBranzeOpisy_KntKarty] CHECK CONSTRAINT [FK_ECRMBO_KK]
GO

SELECT TOP (1000) [branzaID] ,[Opis] ,[ElBranOpisID] FROM [ERPXL].[CDN].[el_CRMBranzeOpisy]
GO