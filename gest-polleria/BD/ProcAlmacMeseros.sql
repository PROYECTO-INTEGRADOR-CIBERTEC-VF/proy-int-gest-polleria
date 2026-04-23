----- PROCEDIMIENTOS
CREATE OR ALTER PROCEDURE usp_ListarMeseros
AS
BEGIN
    SELECT IdMesero, Nombre, Apellido, DNI, Telefono, Turno, Activo FROM dbo.Meseros
END;
GO

CREATE OR ALTER PROCEDURE usp_RegistrarMesero
    @Nombre NVARCHAR(50), @Apellido NVARCHAR(50), @DNI CHAR(8), 
    @Telefono NVARCHAR(15), @Turno NVARCHAR(20), @Activo BIT
AS
BEGIN
    INSERT INTO dbo.Meseros (Nombre, Apellido, DNI, Telefono, Turno, Activo)
    VALUES (@Nombre, @Apellido, @DNI, @Telefono, @Turno, @Activo)
END;
GO

CREATE OR ALTER PROCEDURE usp_AsignarMeseroMesa
    @IdMesero INT,
    @IdMesa INT
AS
BEGIN
    INSERT INTO dbo.AsignacionMeseros (IdMesero, IdMesa)
    VALUES (@IdMesero, @IdMesa);
END;
GO

CREATE OR ALTER PROCEDURE usp_BuscarMeseroPorId
    @IdMesero INT
AS
BEGIN
    SELECT IdMesero, Nombre, Apellido, DNI, Telefono, Turno, Activo 
    FROM dbo.Meseros WHERE IdMesero = @IdMesero
END;
GO

CREATE OR ALTER PROCEDURE usp_ActualizarMesero
    @IdMesero INT,
    @Nombre NVARCHAR(50), @Apellido NVARCHAR(50), @DNI CHAR(8), 
    @Telefono NVARCHAR(15), @Turno NVARCHAR(20), @Activo BIT
AS
BEGIN
    UPDATE dbo.Meseros SET 
        Nombre = @Nombre, Apellido = @Apellido, DNI = @DNI, 
        Telefono = @Telefono, Turno = @Turno, Activo = @Activo
    WHERE IdMesero = @IdMesero
END;
GO

CREATE OR ALTER PROCEDURE usp_DesactivarMesero
    @IdMesero INT
AS
BEGIN
    UPDATE dbo.Meseros SET Activo = 0 WHERE IdMesero = @IdMesero
END;
GO

CREATE OR ALTER PROCEDURE usp_ActivarMesero
    @IdMesero INT
AS
BEGIN
    UPDATE dbo.Meseros SET Activo = 1 WHERE IdMesero = @IdMesero
END;
GO