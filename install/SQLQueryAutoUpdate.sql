/*
ALTER TABLE [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] DROP CONSTRAINT [FK_ECRMBO_KK]
GO
ALTER TABLE [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] DROP CONSTRAINT [FK_ECRMBO_ECBO]
GO
ALTER TABLE [CDN].[el_CRMBranzeOpisy] DROP CONSTRAINT [FK_ECBO_SLW]
GO
ALTER TABLE [CDN].[el_CRMBranzeOpisy] DROP CONSTRAINT [UK_branzaID_Opis]
GO
DROP TABLE [CDN].[ISK_el_CRMBranzeOpisy_KntKarty]
GO
*/

DROP TABLE [CDN].[el_CRMBranzeOpisy]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CDN].[el_CRMBranzeOpisy](
	[ElBranOpisID] [int] IDENTITY(1,1) NOT NULL,
	[branzaID] [int] NOT NULL,
	[Opis] [varchar](150) NULL,
 CONSTRAINT [PK_ElBranOpisID] PRIMARY KEY CLUSTERED 
(
	[ElBranOpisID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER AUTHORIZATION ON [CDN].[el_CRMBranzeOpisy] TO  SCHEMA OWNER 
GO
GRANT DELETE ON [CDN].[el_CRMBranzeOpisy] TO [CDNRaport] AS [CDN]
GO
GRANT INSERT ON [CDN].[el_CRMBranzeOpisy] TO [CDNRaport] AS [CDN]
GO
GRANT SELECT ON [CDN].[el_CRMBranzeOpisy] TO [CDNRaport] AS [CDN]
GO
GRANT UPDATE ON [CDN].[el_CRMBranzeOpisy] TO [CDNRaport] AS [CDN]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CDN].[ISK_el_CRMBranzeOpisy_KntKarty](
	[Knt_Karty_GIDNumer] [int] NOT NULL,
	[el_CRMBranzeOpisy_ElBranOpisID] [int] NOT NULL,
 CONSTRAINT [el_BraKntKart_Primary] PRIMARY KEY CLUSTERED 
(
	[Knt_Karty_GIDNumer] ASC,
	[el_CRMBranzeOpisy_ElBranOpisID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER AUTHORIZATION ON [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] TO  SCHEMA OWNER 
GO
GRANT DELETE ON [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] TO [CDNRaport] AS [CDN]
GO
GRANT INSERT ON [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] TO [CDNRaport] AS [CDN]
GO
GRANT SELECT ON [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] TO [CDNRaport] AS [CDN]
GO
GRANT UPDATE ON [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] TO [CDNRaport] AS [CDN]
GO

SET IDENTITY_INSERT [CDN].[el_CRMBranzeOpisy] ON 

INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (62, 1504, N'Firmy handlu hurtowego i detalicznego kupujące towar w CT ELTECH do dalszej odsprzedaży')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (79, 1505, N'Przemysł elektrotechniczny: fabryki kabli, transformatorów, tranzystorów, układów scalonych, żarówek, itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (80, 1505, N'Przemysł instalacyjno-grzewczy, producenci hydrauliki')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (110, 1505, N'Przemysł maszynowy: fabryki maszyn do różnych rodzajów przemysłu, maszyn budowlanych, itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (109, 1505, N'Przemysł maszynowy: fabryki obrabiarek, silników, kotłów, maszyn górniczych')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (74, 1505, N'Przemysł metalowy: zakłady produkcji narzędzi i wyrobów metalowych, opakowań przemysłowych, armatury, itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (78, 1505, N'Przemysł precyzyjny: fabryki zegarków, instrumentów pomiarowych, wag, urządzeń precyzyjnych, itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (88, 1506, N'Producenci pojazdów samochodowych, ich wyposażenia, podzespołów, części, itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (98, 1507, N'Producenci maszyn lotniczych, ich wyposażenia, podzespołów, części, itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (69, 1508, N'Producenci lokomotyw, wagonów, tramwajów itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (70, 1508, N'Producenci wyposażenia, podzespołów, części, itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (73, 1510, N'Producenci ciągników i maszyn rolniczych, ich wyposażenia, podzespołów, części, itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (64, 1511, N'Producenci statków, jachtów itp., ich wyposażenia, podzespołów, części, itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (67, 1512, N'Producenci sprzętu zbrojeniowego, środków transportu, broni i amunicji, itd.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (68, 1512, N'Producenci wyposażenia, podzespołów, części przeznaczonych dla celów wojskowych')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (83, 1513, N'Przemysł energetyczny: elektrownie i elektrociepłownie')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (82, 1513, N'Przemysł paliwowy: odwierty ropy naftowej, gazu ziemnego i gazu łupkowego, rafinerie, koksownie')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (81, 1513, N'Przemysł węglowy: kopalnie węgla kamiennego, kopalnie węgla brunatnego')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (84, 1513, N'Zakłady remontowe, naprawcze, serwisy działające na rzecz przemysłu paliwowo-energetycznego')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (85, 1514, N'Kopalnictwo i hutnictwo żelaza: kopalnie rudy żelaza, huty żelaza, stalownie, walcownie, odlewnie żelaza i stali, fabryki blach')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (87, 1514, N'Kuźnie, zakłady remontowe, naprawcze, serwisy działające na rzecz przemysłu metalurgiczno-hutniczego')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (86, 1514, N'Przemysł metali nieżelaznych: kopalnie rud metali nieżelaznych, huty metali nieżelaznych, odlewnie stopów nieżelaznych')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (65, 1515, N'Producenci konstrukcji elementów metalowych przeznaczonych do budownictwa')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (66, 1515, N'Producenci metalowych elementow stolarki budowlanej, bram przemysłowych, paneli metalowych, ogrodzeń, itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (71, 1516, N'Producenci sprzętu gospodarstwa domowego')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (72, 1516, N'Przemysł elektroniczny: producenci aparatów telefonicznych, telewizorów, urządzeń audiowizualnych itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (90, 1517, N'Kopalnie kamienia, granitu itp.')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (93, 1517, N'Przemysł ceramiki sanitarnej, przemysł ceramiki szlachetnej: fabryki porcelany stołowej i elektrotechnicznej, fabryki fajansów')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (111, 1517, N'Przemysł materiałów budowlanych: cementownie, cegielnie, wapienniki, fabryki ceramiki budowlanej')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (112, 1517, N'Przemysł materiałów budowlanych: fabryki domów (produkcja prefabrykatów betonowych)')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (92, 1517, N'Przemysł szklarski: huty szkła, fabryki opakowań szklanych')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (91, 1517, N'Zakłady remontowe, naprawcze, serwisy świadczące usługi dla w/w kopalnii')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (114, 1518, N'Fabryki nawozów sztucznych, włókien sztucznych, mas plastycznych, ')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (115, 1518, N'Fabryki wyrobów gumowych, chemii nieorganicznej i organicznej')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (113, 1518, N'Kopalnie siarki, soli')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (95, 1518, N'Producenci środków piorących i kosmetyków, zakłady farmaceutyczne')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (96, 1518, N'Produkcja wyrobów z tworzyw sztucznych, w tym opakowań z tworzyw')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (97, 1518, N'Zakłady remontowe, naprawcze, serwisy świadczące usługi dla w/w kopalnii')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (100, 1519, N'Przemysł celulozowo-papierniczy: fabryki papieru, celulozy i tektury, opakowań przemysłowych')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (99, 1519, N'Przemysł drzewny: tartaki, fabryki płyt pilśniowych, sklejek, oklein, fabryki mebli, zapałek, opakowań przemysłowych')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (107, 1520, N'Inne wyżej niesklasyfikowane')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (106, 1520, N'Jednostki wojskowe')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (101, 1520, N'Przemysł lekki: przemysł włókienniczy (tekstylny), przemysł odzieżowy, przemysł skórzany')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (104, 1520, N'Przemysł poligraficzny')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (102, 1520, N'Przemysł spożywczy')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (103, 1520, N'Przemysł wysokiej technologii: producenci sprzętu komputerowego, oprogramowania i elektroniki')
INSERT [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID], [branzaID], [Opis]) VALUES (108, 1521, N'Export')
SET IDENTITY_INSERT [CDN].[el_CRMBranzeOpisy] OFF
SET ANSI_PADDING ON
GO
ALTER TABLE [CDN].[el_CRMBranzeOpisy] ADD  CONSTRAINT [UK_branzaID_Opis] UNIQUE NONCLUSTERED 
(
	[branzaID] ASC,
	[Opis] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [CDN].[el_CRMBranzeOpisy]  WITH CHECK ADD  CONSTRAINT [FK_ECBO_SLW] FOREIGN KEY([branzaID])
REFERENCES [CDN].[Slowniki] ([SLW_ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [CDN].[el_CRMBranzeOpisy] CHECK CONSTRAINT [FK_ECBO_SLW]
GO
ALTER TABLE [CDN].[ISK_el_CRMBranzeOpisy_KntKarty]  WITH CHECK ADD  CONSTRAINT [FK_ECRMBO_ECBO] FOREIGN KEY([el_CRMBranzeOpisy_ElBranOpisID])
REFERENCES [CDN].[el_CRMBranzeOpisy] ([ElBranOpisID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] CHECK CONSTRAINT [FK_ECRMBO_ECBO]
GO
ALTER TABLE [CDN].[ISK_el_CRMBranzeOpisy_KntKarty]  WITH CHECK ADD  CONSTRAINT [FK_ECRMBO_KK] FOREIGN KEY([Knt_Karty_GIDNumer])
REFERENCES [CDN].[KntKarty] ([Knt_GIDNumer])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [CDN].[ISK_el_CRMBranzeOpisy_KntKarty] CHECK CONSTRAINT [FK_ECRMBO_KK]
GO
