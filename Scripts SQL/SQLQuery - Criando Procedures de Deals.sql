USE MiniPloomes;
GO
CREATE PROCEDURE GetDealByIdAndUser
	@dealId int,
	@userId int
AS
BEGIN
	SELECT * FROM Deals WHERE Id = @dealId AND CreatorId = @userId;
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