CREATE TABLE [dbo].[Item]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Status] INT NULL, 
    [Comment] NVARCHAR(300) NULL, 
    [Category] INT NOT NULL,
    FOREIGN KEY ([Category]) REFERENCES [dbo].[Category]([Id])
)
