USE MiniPloomes;
GO
CREATE PROCEDURE GetUserByToken
	@token NVARCHAR(254)
AS
BEGIN
	SELECT * FROM Users WHERE Token = @token;
END
GO


CREATE PROCEDURE ValidateUser
	@email NVARCHAR(254),
	@password NVARCHAR(max)
AS
BEGIN
	SELECT * FROM Users WHERE Email = @email AND Password = @password
END
GO


CREATE PROCEDURE AddUser
	@name NVARCHAR(max),
	@email NVARCHAR(254),
	@password NVARCHAR(max),
	@token NVARCHAR(254),
	@createDate DATETIME2(7)
AS
BEGIN
	INSERT INTO Users VALUES (@name, @email, @password, @token, @createDate)
END
GO


CREATE PROCEDURE UpdateUser
	@name NVARCHAR(max),
	@email NVARCHAR(254),
	@password NVARCHAR(max),
	@token NVARCHAR(254)
AS
BEGIN
	UPDATE Users SET Name = @name, Email = @email, Password = @password WHERE Token = @token
END
GO


CREATE PROCEDURE RemoveUserByToken
	@token NVARCHAR(254)
AS
BEGIN
	DELETE FROM Users WHERE Token = @token
END
GO


CREATE PROCEDURE RemoveToken
	@token NVARCHAR(254)
AS
BEGIN
	UPDATE Users SET Token = NULL WHERE Token = @token
END
GO

CREATE PROCEDURE UpdateTokenById
	@newToken NVARCHAR(254),
	@id int
AS
BEGIN
	UPDATE Users SET Token = @newToken WHERE Id = @id
END
GO


