﻿CREATE TABLE [dbo].[Item]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(30) UNIQUE NOT NULL, 
    [Status] INT NOT NULL, 
    [Comment] NVARCHAR(100) NULL, 
    [Category] INT NOT NULL,
    [UpdatedOn] DATETIME2 NOT NULL
)
