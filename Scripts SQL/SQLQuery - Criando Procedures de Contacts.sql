USE MiniPloomes;
GO
CREATE PROCEDURE GetContactByIdAndUser
	@contactId int,
	@userId int
AS
BEGIN
	SELECT * FROM Contacts WHERE Id = @contactId AND CreatorId = @userId;
END
GO


CREATE PROCEDURE AddContact
	@name VARCHAR(max),
	@creatorId int,
	@createDate DATETIME2(7),
	@id int OUTPUT
AS
BEGIN
	INSERT INTO Contacts VALUES (@name, @creatorId, @createDate)
	SELECT @id = SCOPE_IDENTITY()
END
GO


CREATE PROCEDURE UpdateContact
	@name VARCHAR(max),
	@id int
AS
BEGIN
	UPDATE Contacts SET Name = @name WHERE Id = @id
END
GO

CREATE PROCEDURE RemoveContact
	@id int
AS
BEGIN
	DELETE FROM Contacts WHERE Id = @id
END
GO
