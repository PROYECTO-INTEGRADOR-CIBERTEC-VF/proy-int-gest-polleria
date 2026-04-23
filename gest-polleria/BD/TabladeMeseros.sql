CREATE TABLE dbo.Meseros (
    IdMesero INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    DNI CHAR(8) UNIQUE NOT NULL,
    Telefono NVARCHAR(15) NULL,
    Turno NVARCHAR(20) NOT NULL,
    Activo BIT DEFAULT 1 
);
GO

CREATE TABLE dbo.AsignacionMeseros (
    IdAsignacion INT PRIMARY KEY IDENTITY(1,1),
    IdMesero INT NOT NULL,
    IdMesa INT NOT NULL,
    FechaAsignacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Asignacion_Mesero FOREIGN KEY (IdMesero) REFERENCES dbo.Meseros(IdMesero),
    CONSTRAINT FK_Asignacion_Mesa FOREIGN KEY (IdMesa) REFERENCES dbo.Mesas(IdMesa)
);
GO