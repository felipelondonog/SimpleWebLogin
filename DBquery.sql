USE MASTER;
GO
CREATE DATABASE DB_LOGIN;
GO
USE DB_LOGIN;
GO

CREATE TABLE USERS(
IdUser INT PRIMARY KEY IDENTITY,
UserName NVARCHAR(100),
Email NVARCHAR(250),
Pwd NVARCHAR(200),
ResetPwd bit,
Confirmed bit,
Token NVARCHAR(200)
);
GO