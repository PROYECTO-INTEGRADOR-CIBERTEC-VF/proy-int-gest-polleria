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
    [ClaveHash] [nvarchar](200) NOT NULL,
    [NombreCompleto] [nvarchar](100) NOT NULL,
    [Email] [nvarchar](100) NULL,
    [Telefono] [nvarchar](20) NULL,
    [Activo] [bit] NOT NULL DEFAULT(1),
    [FechaRegistro] [datetime2](7) NOT NULL DEFAULT(SYSDATETIME()),
PRIMARY KEY CLUSTERED ([IdUsuario] ASC)
)
GO

-- Tabla UsuariosRoles
CREATE TABLE [dbo].[UsuariosRoles](
    [IdUsuario] [int] NOT NULL,
    [IdRol] [int] NOT NULL,
PRIMARY KEY CLUSTERED ([IdUsuario] ASC, [IdRol] ASC),
FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuarios]([IdUsuario]),
FOREIGN KEY ([IdRol]) REFERENCES [dbo].[Roles]([IdRol])
)
GO

-- Datos de prueba
INSERT INTO Roles (Nombre) VALUES ('Administrador'), ('Mesero')
GO

INSERT INTO Usuarios (UserName, ClaveHash, NombreCompleto)
VALUES 
('admin', 'admin123', 'Administrador Principal'),
('mesero1', 'mesero123', 'Juan Perez')
GO

INSERT INTO UsuariosRoles (IdUsuario, IdRol)
VALUES (1, 1), (2, 2)
GO

-- Stored Procedure Login
CREATE PROC [dbo].[usp_Usuarios_Login]
    @UserName NVARCHAR(50),
    @ClaveHash NVARCHAR(200),
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
    INNER JOIN dbo.UsuariosRoles ur ON u.IdUsuario = ur.IdUsuario
    INNER JOIN dbo.Roles r ON ur.IdRol = r.IdRol
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