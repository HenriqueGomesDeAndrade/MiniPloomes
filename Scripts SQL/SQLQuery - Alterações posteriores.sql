/*Alterações posteriores na tabela*/

Use MiniPloomes;

ALTER TABLE [dbo].[Users]
ALTER COLUMN Token NVARCHAR(254) NULL

ALTER TABLE [dbo].[Users]
ADD CONSTRAINT UC_Token UNIQUE (Token);

ALTER TABLE [dbo].[Users]
ALTER COLUMN Email NVARCHAR(254) NOT NULL

ALTER TABLE [dbo].[Users]
ADD CONSTRAINT UC_Email UNIQUE (Email);