CREATE NONCLUSTERED INDEX IDX_Contacts 
ON [dbo].[Contacts](CreatorId)
INCLUDE([Id],[Name],[CreateDate]);
GO

CREATE NONCLUSTERED INDEX IX_Deals 
ON [dbo].[Deals]([CreatorId],[ContactId])
INCLUDE([Id], [Title], [Amount], [CreateDate]);
GO
