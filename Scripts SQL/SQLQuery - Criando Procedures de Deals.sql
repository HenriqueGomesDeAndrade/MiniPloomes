USE MiniPloomes;
GO

CREATE PROCEDURE GetDeals
	@userId int
AS
BEGIN
	SELECT * FROM Deals WHERE CreatorId = @userId;
END
GO

CREATE PROCEDURE GetDealByIdAndUser
	@dealId int,
	@userId int
AS
BEGIN
	SELECT * FROM Deals WHERE Id = @dealId AND CreatorId = @userId;
END
GO

ALTER PROCEDURE GetDealByIdAndUser
	@dealId int,
	@userId int
AS
BEGIN
	SELECT
		D.Id, D.Title, D.Amount, D.ContactId, D.CreatorId, D.CreateDate,
		C.Id 'ContactId', C.Name 'ContactName', C.CreatorId 'ContactCreatorId', C.CreateDate 'ContactCreateDate',
		U.Name 'UserName', U.Email 'UserEmail', U.Token 'UserToken', U.CreateDate 'UserCreateDate'
	FROM 
	[dbo].[Users] U
	right join [dbo].[Contacts] C
	ON C.CreatorId = U.Id
	inner join [dbo].[Deals] D
	On D.ContactId = C.Id
	WHERE D.Id = @dealId AND D.CreatorId = @userId;
END
GO


CREATE PROCEDURE AddDeal
	@title VARCHAR(max),
	@amount decimal(19,4),
	@contactId int,
	@creatorId int,
	@createDate DATETIME2(7),
	@id int OUTPUT
AS
BEGIN
	INSERT INTO Deals VALUES (@title, @amount, @contactId, @creatorId, @createdate ) 
	SELECT @id = SCOPE_IDENTITY()
END
GO

CREATE PROCEDURE UpdateDeal
	@title VARCHAR(max),
	@amount decimal(19,4),
	@contactId int,
	@id int
AS
BEGIN
	UPDATE Deals SET Title = @title, Amount = @amount, ContactId = @contactId  WHERE Id = @id
END
GO

CREATE PROCEDURE RemoveDeal
	@id int
AS
BEGIN
	DELETE FROM Deals WHERE Id = @id
END
GO