CREATE DATABASE ThaiBev
COLLATE Thai_CI_AS;
GO


USE ThaiBev
GO


CREATE TABLE AspNetRoles
(
    Id NVARCHAR(128) NOT NULL,
    Name NVARCHAR(256) NOT NULL,
    CONSTRAINT PK_AspNetRoles PRIMARY KEY (Id)
);

CREATE UNIQUE INDEX IX_AspNetRoles_Name
ON AspNetRoles (Name);


CREATE TABLE AspNetUsers
(
    Id NVARCHAR(128) NOT NULL,
    Email NVARCHAR(256) NULL,
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(MAX) NULL,
    SecurityStamp NVARCHAR(MAX) NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEndDateUtc DATETIME NULL,
    LockoutEnabled BIT NOT NULL,
	NormalizedUserName nvarchar(256) NULL,
	ConcurrencyStamp nvarchar(max) NULL,
	LockoutEnd datetimeoffset(7) NULL,
	NormalizedEmail nvarchar(256) NULL,
	PhoneNumber nvarchar(30) NULL,
	PhoneNumberConfirmed bit NOT NULL,
    AccessFailedCount INT NOT NULL,
    UserName NVARCHAR(256) NOT NULL,

    CONSTRAINT PK_AspNetUsers PRIMARY KEY (Id)
);

CREATE UNIQUE INDEX IX_AspNetUsers_UserName
ON AspNetUsers (UserName);


CREATE TABLE AspNetUserRoles
(
    UserId NVARCHAR(128) NOT NULL,
    RoleId NVARCHAR(128) NOT NULL,

    CONSTRAINT PK_AspNetUserRoles PRIMARY KEY (UserId, RoleId),

    CONSTRAINT FK_AspNetUserRoles_AspNetUsers
        FOREIGN KEY (UserId)
        REFERENCES AspNetUsers (Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_AspNetUserRoles_AspNetRoles
        FOREIGN KEY (RoleId)
        REFERENCES AspNetRoles (Id)
        ON DELETE CASCADE
);


CREATE TABLE AspNetUserLogins
(
    LoginProvider NVARCHAR(128) NOT NULL,
    ProviderKey NVARCHAR(128) NOT NULL,
    UserId NVARCHAR(128) NOT NULL,

    CONSTRAINT PK_AspNetUserLogins
        PRIMARY KEY (LoginProvider, ProviderKey, UserId),

    CONSTRAINT FK_AspNetUserLogins_AspNetUsers
        FOREIGN KEY (UserId)
        REFERENCES AspNetUsers (Id)
        ON DELETE CASCADE
);

GO
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [NormalizedUserName], [ConcurrencyStamp], [LockoutEnd], [NormalizedEmail], [PhoneNumber], [PhoneNumberConfirmed], [AccessFailedCount], [UserName]) VALUES (N'47e0d9f4-f28e-4e02-aeb0-2b26f58b87a4', NULL, 0, N'AQAAAAIAAYagAAAAEDhCDjOqHdN7LnboPrJFPtRaCLXg4JcQ09kWUmbBp/ef1y1Kkd67ErXgPTdvOWXilQ==', N'MNXOPJAE6WIULT5OSUQ36RY7IUMAFTNP', 0, NULL, 1, N'TEST', N'9ad373a5-6ad1-46b9-ab8f-fd1b06dff93f', NULL, NULL, NULL, 0, 0, N'test')
GO
