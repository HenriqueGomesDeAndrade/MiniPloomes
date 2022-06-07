USE MiniPloomes;
GO

CREATE PROCEDURE GetContacts
	@userId int
AS
BEGIN
	SELECT * FROM Contacts WHERE CreatorId = @userId;
END
GO



CREATE PROCEDURE GetContactByIdAndUser
	@contactId int,
	@userId int
AS
BEGIN
	SELECT * FROM Contacts WHERE Id = @contactId AND CreatorId = @userId;
END
GO

ALTER PROCEDURE GetContactByIdAndUser
	@contactId int,
	@userId int
AS
BEGIN
	SELECT C.Id,
		C.Name,
		C.CreatorId,
		C.CreateDate,
		U.Name 'UserName',
		U.Email,
		U.Token,
		U.CreateDate 'U_CreateDate'
	FROM 
	[dbo].[Contacts] C
	inner join [dbo].[Users] U
	ON C.CreatorId = U.Id
	WHERE C.Id = @contactId AND CreatorId = @userId;
END
GO

select * from users
select * from contacts


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