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

-- 1. Agregamos la columna que guardará el ID del mesero en la tabla Mesas
ALTER TABLE [dbo].[Mesas] 
ADD [IdMesero] INT NULL; 
GO

-- 2. Creamos el vínculo oficial (Clave Foránea) entre ambas tablas
ALTER TABLE [dbo].[Mesas] 
WITH CHECK ADD CONSTRAINT [FK_Mesas_Meseros] 
FOREIGN KEY([IdMesero]) REFERENCES [dbo].[Meseros] ([IdMesero]);
GO

-- 3. Activamos la validación de la regla
ALTER TABLE [dbo].[Mesas] CHECK CONSTRAINT [FK_Mesas_Meseros];
GO