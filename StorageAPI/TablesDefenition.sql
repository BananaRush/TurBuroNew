CREATE TABLE [dbo].[NewsModel] (
    [Id]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [Text]                NVARCHAR (MAX)  NOT NULL,
    [IconUri]             VARCHAR (1000)  NOT NULL,
    [Content]             NVARCHAR (MAX)  NULL,
    [AdvertisingImageUri] NVARCHAR (1000) NULL,
    [ContentType]         INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Slider] (
    [Id]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [Content]     NVARCHAR (1000) NOT NULL,
    [Caption]     NVARCHAR (1000) NOT NULL,
    [ContentType] INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Table] (
    [Id]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [Content]     NVARCHAR (1000) NOT NULL,
    [Caption]     NVARCHAR (1000) NOT NULL,
    [ContentType] INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Test] (
    [Id]        BIGINT          IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (200)  NOT NULL,
    [Question]  NVARCHAR (1000) NOT NULL,
    [Responses] NVARCHAR (MAX)  NOT NULL,
    [Answer]    INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[User] (
    [Id]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [Login]    NVARCHAR (50)  NOT NULL,
    [Password] NVARCHAR (500) NOT NULL,
    [Name]     NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[User_Test] (
    [Id]           BIGINT   IDENTITY (1, 1) NOT NULL,
    [UserId]       BIGINT   NOT NULL,
    [TestId]       BIGINT   NOT NULL,
    [Result]       REAL     NULL,
    [PassingStamp] DATETIME DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Test_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_User_Test_Test] FOREIGN KEY ([TestId]) REFERENCES [dbo].[Test] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_User_Test_UserId_TestId]
    ON [dbo].[User_Test]([UserId] ASC, [TestId] ASC);

