USE [SRO_CERTIFICATION]
GO
/****** Object:  Table [dbo].[Content]    Script Date: 27.07.2016 04:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Content](
	[ID] [tinyint] NOT NULL,
	[Name] [varchar](64) NULL,
 CONSTRAINT [PK__Content__3214EC2719B8A7C8] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Division]    Script Date: 27.07.2016 04:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Division](
	[ID] [tinyint] NOT NULL,
	[Name] [varchar](32) NOT NULL,
	[DBConfig] [varchar](256) NULL,
 CONSTRAINT [PK__Division__3214EC271A898B6D] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Farm]    Script Date: 27.07.2016 04:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Farm](
	[ID] [tinyint] NOT NULL,
	[DivisionID] [tinyint] NULL,
	[Name] [varchar](32) NOT NULL,
	[DBConfig] [varchar](256) NULL,
 CONSTRAINT [PK__Farm__3214EC278DCA4064] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FarmContent]    Script Date: 27.07.2016 04:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FarmContent](
	[FarmID] [tinyint] NOT NULL,
	[ContentID] [tinyint] NOT NULL,
 CONSTRAINT [PK__FarmCont__1FEBC01E1A6A80B4] PRIMARY KEY CLUSTERED 
(
	[FarmID] ASC,
	[ContentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Module]    Script Date: 27.07.2016 04:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Module](
	[ID] [tinyint] NOT NULL,
	[Name] [varchar](64) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerBody]    Script Date: 27.07.2016 04:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerBody](
	[ID] [smallint] IDENTITY(1,1) NOT NULL,
	[DivisionID] [tinyint] NULL,
	[FarmID] [tinyint] NULL,
	[ShardID] [smallint] NULL,
	[MachineID] [int] NULL,
	[ModuleID] [tinyint] NULL,
	[ModuleType] [tinyint] NOT NULL CONSTRAINT [DF__ServerBod__Modul__145C0A3F]  DEFAULT ((0)),
	[CertifierID] [smallint] NULL,
	[BindPort] [smallint] NOT NULL,
 CONSTRAINT [PK__ServerBo__3214EC27C75D694C] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServerCord]    Script Date: 27.07.2016 04:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerCord](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ChildID] [smallint] NOT NULL,
	[ParentID] [smallint] NOT NULL,
	[BindType] [tinyint] NOT NULL,
 CONSTRAINT [PK__ServerCo__3214EC278202D17A] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServerMachine]    Script Date: 27.07.2016 04:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ServerMachine](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DivisionID] [tinyint] NULL,
	[Name] [varchar](32) NOT NULL,
	[PublicIP] [varchar](16) NOT NULL,
	[PrivateIP] [varchar](16) NOT NULL,
 CONSTRAINT [PK__ServerMa__3214EC276F13B687] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Shard]    Script Date: 27.07.2016 04:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Shard](
	[ID] [smallint] IDENTITY(1,1) NOT NULL,
	[FarmID] [tinyint] NULL,
	[ContentID] [tinyint] NULL,
	[Name] [varchar](32) NOT NULL,
	[DBConfig] [varchar](256) NULL,
	[LogDBConfig] [varchar](256) NULL,
	[MaxUser] [smallint] NOT NULL,
	[ShardManagerID] [smallint] NULL,
 CONSTRAINT [PK__Shard__3214EC270B93CBD2] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Content] ([ID], [Name]) VALUES (1, N'Silkroad_Dev')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (2, N'Silkroad_Korea_Yahoo_Official')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (3, N'Silkroad_Korea_Yahoo_Test_IN')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (4, N'SRO_China_Official')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (5, N'SRO_China_TestLocal')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (6, N'Silkroad_Joymax')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (7, N'JoymaxMessenger')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (8, N'ServiceManager')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (9, N'SRO_China_TestIn')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (10, N'SRO_Taiwan_TestIn')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (11, N'SRO_Taiwan_TestLocal')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (12, N'SRO_Taiwan_Official')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (13, N'SRO_DEEPDARK')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (14, N'SRO_Taiwan_BillingTest')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (15, N'SRO_Japan_Official')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (16, N'SRO_Japan_TestLocal')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (17, N'SRO_Japan_TestIn')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (18, N'SRO_Global_TestBed')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (19, N'SRO_Global_TestBed_In')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (20, N'SRO_EuropeTest')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (21, N'SRO_Vietnam_TestIn')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (22, N'SRO_Vietnam_TestLocal')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (23, N'SRO_Net2E_Official')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (24, N'Yahoo_Official_Test')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (25, N'SRO_GNGWC_TestIn')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (26, N'SRO_GNGWC_Official')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (27, N'SRO_China_OpenTest')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (29, N'SRO_GNGWC_Official_Final')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (30, N'CPRJ_Dev')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (31, N'SRO_INTERNAL_EU')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (32, N'SRO_INTERNAL_EU_QUEST')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (33, N'Vietnam_Dev')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (34, N'SRO_China_EuroTest')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (35, N'SRO_Taiwan_FOS CB')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (36, N'SRO_GameOn_Official_Test')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (37, N'SRO_Thailand_TestLocal')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (38, N'SRO_Thailand_Official')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (39, N'SRO_Russia_TestLocal')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (40, N'SRO_Russia_Official')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (41, N'SRO_Japan_TestOTP')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (42, N'SRO_Global_TestBed_OT')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (43, N'SRO_Japan_CGI_TestIn')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (44, N'SRO_Japan_TestLocal_We')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (45, N'SRO_R_JP_TestLocal_We')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (46, N'SRO_R_JP_RealLocal_We')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (47, N'SRO_R_CH_TestLocal_CIMO')
INSERT [dbo].[Content] ([ID], [Name]) VALUES (48, N'SRO_R_CH_RealLocal_CIMO')
INSERT [dbo].[Division] ([ID], [Name], [DBConfig]) VALUES (22, N'SRO_Vietnam_TestLocal', N'DRIVER={SQL Server};SERVER=.\SQLExpress;DSN=SRO_VT_ACCOUNT;UID=sa;PWD=123456;DATABASE=SRO_VT_ACCOUNT')
INSERT [dbo].[Farm] ([ID], [DivisionID], [Name], [DBConfig]) VALUES (20, 22, N'SRO_VDC-Net2E_TestIn', NULL)
INSERT [dbo].[FarmContent] ([FarmID], [ContentID]) VALUES (20, 22)
INSERT [dbo].[Module] ([ID], [Name]) VALUES (1, N'Certification')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (2, N'GlobalManager')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (3, N'DownloadServer')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (4, N'GatewayServer')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (5, N'FarmManager')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (6, N'AgentServer')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (7, N'SR_ShardManager')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (8, N'SR_GameServer')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (9, N'SR_Client')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (10, N'ServiceManager')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (11, N'MachineManager')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (12, N'JmxMsgSvr')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (13, N'JmxMessenger')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (14, N'SMC')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (15, N'CPRJ_Client')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (16, N'CPRJ_GameServer')
INSERT [dbo].[Module] ([ID], [Name]) VALUES (17, N'CPRJ_ShardManager')
SET IDENTITY_INSERT [dbo].[ServerBody] ON 

INSERT [dbo].[ServerBody] ([ID], [DivisionID], [FarmID], [ShardID], [MachineID], [ModuleID], [ModuleType], [CertifierID], [BindPort]) VALUES (1, NULL, NULL, NULL, 1, 1, 0, NULL, 32000)
INSERT [dbo].[ServerBody] ([ID], [DivisionID], [FarmID], [ShardID], [MachineID], [ModuleID], [ModuleType], [CertifierID], [BindPort]) VALUES (2, 22, NULL, NULL, 2, 2, 0, 1, 15880)
INSERT [dbo].[ServerBody] ([ID], [DivisionID], [FarmID], [ShardID], [MachineID], [ModuleID], [ModuleType], [CertifierID], [BindPort]) VALUES (3, 22, NULL, NULL, 2, 11, 0, 2, 25880)
INSERT [dbo].[ServerBody] ([ID], [DivisionID], [FarmID], [ShardID], [MachineID], [ModuleID], [ModuleType], [CertifierID], [BindPort]) VALUES (4, 22, NULL, NULL, 2, 4, 0, 2, 15779)
INSERT [dbo].[ServerBody] ([ID], [DivisionID], [FarmID], [ShardID], [MachineID], [ModuleID], [ModuleType], [CertifierID], [BindPort]) VALUES (5, 22, NULL, NULL, 2, 3, 0, 2, 15881)
INSERT [dbo].[ServerBody] ([ID], [DivisionID], [FarmID], [ShardID], [MachineID], [ModuleID], [ModuleType], [CertifierID], [BindPort]) VALUES (6, 22, 20, NULL, 2, 5, 0, 2, 15882)
INSERT [dbo].[ServerBody] ([ID], [DivisionID], [FarmID], [ShardID], [MachineID], [ModuleID], [ModuleType], [CertifierID], [BindPort]) VALUES (7, 22, 20, 3, 2, 7, 0, 6, 15883)
INSERT [dbo].[ServerBody] ([ID], [DivisionID], [FarmID], [ShardID], [MachineID], [ModuleID], [ModuleType], [CertifierID], [BindPort]) VALUES (8, 22, 20, 3, 2, 6, 0, 6, 15884)
INSERT [dbo].[ServerBody] ([ID], [DivisionID], [FarmID], [ShardID], [MachineID], [ModuleID], [ModuleType], [CertifierID], [BindPort]) VALUES (9, 22, 20, 3, 2, 8, 0, 6, 15885)
SET IDENTITY_INSERT [dbo].[ServerBody] OFF
SET IDENTITY_INSERT [dbo].[ServerCord] ON 

INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (21, 2, 1, 1)
INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (32, 3, 2, 1)
INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (42, 4, 2, 1)
INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (52, 5, 2, 1)
INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (62, 6, 2, 1)
INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (76, 7, 6, 1)
INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (86, 8, 6, 1)
INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (87, 8, 7, 1)
INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (89, 8, 9, 1)
INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (96, 9, 6, 1)
INSERT [dbo].[ServerCord] ([ID], [ChildID], [ParentID], [BindType]) VALUES (97, 9, 7, 1)
SET IDENTITY_INSERT [dbo].[ServerCord] OFF
SET IDENTITY_INSERT [dbo].[ServerMachine] ON 

INSERT [dbo].[ServerMachine] ([ID], [DivisionID], [Name], [PublicIP], [PrivateIP]) VALUES (1, NULL, N'Certification Manager', N'127.0.0.1', N'127.0.0.1')
INSERT [dbo].[ServerMachine] ([ID], [DivisionID], [Name], [PublicIP], [PrivateIP]) VALUES (2, 22, N'VSERVER', N'0.0.0.0', N'0.0.0.0')
SET IDENTITY_INSERT [dbo].[ServerMachine] OFF
SET IDENTITY_INSERT [dbo].[Shard] ON 

INSERT [dbo].[Shard] ([ID], [FarmID], [ContentID], [Name], [DBConfig], [LogDBConfig], [MaxUser], [ShardManagerID]) VALUES (3, 20, 22, N'TestShard', N'DRIVER={SQL Server};SERVER=.\SQLExpress;DSN=SRO_VT_SHARD;UID=sa;PWD=123456;DATABASE=SRO_VT_SHARD', N'DRIVER={SQL Server};SERVER=.\SQLExpress;DSN=SRO_VT_LOG;UID=sa;PWD=123456;DATABASE=SRO_VT_LOG', 1000, 7)
SET IDENTITY_INSERT [dbo].[Shard] OFF
ALTER TABLE [dbo].[Farm]  WITH CHECK ADD  CONSTRAINT [FK_Farm_Division] FOREIGN KEY([DivisionID])
REFERENCES [dbo].[Division] ([ID])
GO
ALTER TABLE [dbo].[Farm] CHECK CONSTRAINT [FK_Farm_Division]
GO
ALTER TABLE [dbo].[FarmContent]  WITH CHECK ADD  CONSTRAINT [FK_FarmContent_Content] FOREIGN KEY([ContentID])
REFERENCES [dbo].[Content] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[FarmContent] CHECK CONSTRAINT [FK_FarmContent_Content]
GO
ALTER TABLE [dbo].[FarmContent]  WITH CHECK ADD  CONSTRAINT [FK_FarmContent_Farm] FOREIGN KEY([FarmID])
REFERENCES [dbo].[Farm] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[FarmContent] CHECK CONSTRAINT [FK_FarmContent_Farm]
GO
ALTER TABLE [dbo].[ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_Division] FOREIGN KEY([DivisionID])
REFERENCES [dbo].[Division] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ServerBody] CHECK CONSTRAINT [FK_ServerBody_Division]
GO
ALTER TABLE [dbo].[ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_Farm] FOREIGN KEY([FarmID])
REFERENCES [dbo].[Farm] ([ID])
GO
ALTER TABLE [dbo].[ServerBody] CHECK CONSTRAINT [FK_ServerBody_Farm]
GO
ALTER TABLE [dbo].[ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_Module] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[Module] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ServerBody] CHECK CONSTRAINT [FK_ServerBody_Module]
GO
ALTER TABLE [dbo].[ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_ServerBody] FOREIGN KEY([CertifierID])
REFERENCES [dbo].[ServerBody] ([ID])
GO
ALTER TABLE [dbo].[ServerBody] CHECK CONSTRAINT [FK_ServerBody_ServerBody]
GO
ALTER TABLE [dbo].[ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_ServerMachine] FOREIGN KEY([MachineID])
REFERENCES [dbo].[ServerMachine] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ServerBody] CHECK CONSTRAINT [FK_ServerBody_ServerMachine]
GO
ALTER TABLE [dbo].[ServerBody]  WITH CHECK ADD  CONSTRAINT [FK_ServerBody_Shard] FOREIGN KEY([ShardID])
REFERENCES [dbo].[Shard] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ServerBody] CHECK CONSTRAINT [FK_ServerBody_Shard]
GO
ALTER TABLE [dbo].[ServerCord]  WITH CHECK ADD  CONSTRAINT [FK_ServerCoord_ServerBody] FOREIGN KEY([ChildID])
REFERENCES [dbo].[ServerBody] ([ID])
GO
ALTER TABLE [dbo].[ServerCord] CHECK CONSTRAINT [FK_ServerCoord_ServerBody]
GO
ALTER TABLE [dbo].[ServerCord]  WITH CHECK ADD  CONSTRAINT [FK_ServerCoord_ServerBody1] FOREIGN KEY([ParentID])
REFERENCES [dbo].[ServerBody] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ServerCord] CHECK CONSTRAINT [FK_ServerCoord_ServerBody1]
GO
ALTER TABLE [dbo].[ServerMachine]  WITH CHECK ADD  CONSTRAINT [FK_ServerMachine_Division] FOREIGN KEY([DivisionID])
REFERENCES [dbo].[Division] ([ID])
GO
ALTER TABLE [dbo].[ServerMachine] CHECK CONSTRAINT [FK_ServerMachine_Division]
GO
ALTER TABLE [dbo].[Shard]  WITH CHECK ADD  CONSTRAINT [FK_Shard_Content] FOREIGN KEY([ContentID])
REFERENCES [dbo].[Content] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Shard] CHECK CONSTRAINT [FK_Shard_Content]
GO
ALTER TABLE [dbo].[Shard]  WITH CHECK ADD  CONSTRAINT [FK_Shard_Farm] FOREIGN KEY([FarmID])
REFERENCES [dbo].[Farm] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Shard] CHECK CONSTRAINT [FK_Shard_Farm]
GO
ALTER TABLE [dbo].[Shard]  WITH CHECK ADD  CONSTRAINT [FK_Shard_ServerBody] FOREIGN KEY([ShardManagerID])
REFERENCES [dbo].[ServerBody] ([ID])
GO
ALTER TABLE [dbo].[Shard] CHECK CONSTRAINT [FK_Shard_ServerBody]