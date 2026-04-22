USE gestpolleriaBD
GO

-- Tabla Roles
CREATE TABLE [dbo].[Roles](
    [IdRol] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [nvarchar](50) NOT NULL,
    [Activo] [bit] NOT NULL DEFAULT(1),
PRIMARY KEY CLUSTERED ([IdRol] ASC)
)
GO

-- Tabla Usuarios
CREATE TABLE [dbo].[Usuarios](
    [IdUsuario] [int] IDENTITY(1,1) NOT NULL,
    [UserName] [nvarchar](50) NOT NULL,
    [ClaveHash] [nvarchar](256) NOT NULL,
    [NombreCompleto] [nvarchar](150) NOT NULL,
    [IdRol] [int] NOT NULL,
    [Activo] [bit] NOT NULL DEFAULT(1),
PRIMARY KEY CLUSTERED ([IdUsuario] ASC),
FOREIGN KEY ([IdRol]) REFERENCES [dbo].[Roles]([IdRol])
)
GO

-- Datos de prueba
INSERT INTO Roles (Nombre) VALUES ('Administrador'), ('Mesero')
GO

INSERT INTO Usuarios (UserName, ClaveHash, NombreCompleto, IdRol)
VALUES 
('admin', 'admin123', 'Administrador Principal', 1),
('mesero1', 'mesero123', 'Juan Perez', 2)
GO

-- Stored Procedure Login
CREATE PROC [dbo].[usp_Usuarios_Login]
    @UserName NVARCHAR(50),
    @ClaveHash NVARCHAR(256),
    @Ok BIT OUTPUT,
    @Mensaje NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.IdUsuario,
        u.UserName,
        u.NombreCompleto,
        r.Nombre AS Rol
    FROM dbo.Usuarios u
    INNER JOIN dbo.Roles r ON u.IdRol = r.IdRol
    WHERE u.UserName = @UserName
      AND u.ClaveHash = @ClaveHash
      AND u.Activo = 1;

    IF @@ROWCOUNT > 0
    BEGIN
        SET @Ok = 1;
        SET @Mensaje = 'Login exitoso';
    END
    ELSE
    BEGIN
        SET @Ok = 0;
        SET @Mensaje = 'Usuario o contraseña incorrectos';
    END
END
GO