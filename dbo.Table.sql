CREATE TABLE [dbo].[Currency_Master]
(
	[Id]				INT				IDENTITY(1,1) NOT NULL,
	[Amount]			FLOAT(53)		NULL,
	[CurrencyName]		NVARCHAR(50)	NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC)
);

