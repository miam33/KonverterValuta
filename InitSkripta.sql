CREATE TABLE [dbo].[Currency_Master]
(
	[Id]				INT				IDENTITY(1,1) NOT NULL,
	[Amount]			FLOAT(53)		NULL,
	[CurrencyName]		NVARCHAR(50)	NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC)
);

INSERT INTO Currency_Master (Amount, CurrencyName) 
VALUES 
    (0.91, 'EUR'),    -- Euro
    (0.80, 'GBP'),    -- British Pound
    (83.28, 'INR'),   -- Indian Rupee
    (1.53, 'AUD'),    -- Australian Dollar
    (0.88, 'CHF'),    -- Swiss Franc
    (149.71, 'JPY'),  -- Japanese Yen
    (7.75, 'HKD'),    -- Hong Kong Dollar
    (6.36, 'CNY'),    -- Chinese Yuan
    (22.99, 'MXN'),   -- Mexican Peso
    (1.25, 'CAD'),    -- Canadian Dollar
    (8.56, 'SEK'),    -- Swedish Krona
    (7.44, 'DKK'),    -- Danish Krone
    (10.21, 'NOK'),   -- Norwegian Krone
    (14.87, 'ZAR'),   -- South African Rand
    (0.97, 'NZD'),    -- New Zealand Dollar
    (75.63, 'RUB'),   -- Russian Ruble
    (0.27, 'AED'),    -- UAE Dirham
    (3.67, 'SAR'),    -- Saudi Riyal
    (58.35, 'ARS'),   -- Argentine Peso
    (3.75, 'BRL'),    -- Brazilian Real
    (0.85, 'SGD');    -- Singapore Dollar
