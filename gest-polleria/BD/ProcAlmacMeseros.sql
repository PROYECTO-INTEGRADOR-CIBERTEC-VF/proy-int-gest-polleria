----- PROCEDIMIENTOS
CREATE OR ALTER PROCEDURE usp_ListarMeseros
AS
BEGIN
    SELECT IdMesero, Nombre, Apellido, DNI, Telefono, Turno, Activo FROM dbo.Meseros
END;
GO

CREATE OR ALTER PROCEDURE usp_RegistrarMesero
    @Nombre NVARCHAR(50), 
    @Apellido NVARCHAR(50), 
    @DNI CHAR(8), 
    @Telefono NVARCHAR(15), 
    @Turno NVARCHAR(20), 
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Meseros (Nombre, Apellido, DNI, Telefono, Turno, Activo)
    VALUES (@Nombre, @Apellido, @DNI, @Telefono, @Turno, @Activo);

    SELECT 'Mesero registrado correctamente.' AS Mensaje;
END;
GO


-- PROCEDIMIENTO: ASIGNAR MESERO A MESA
CREATE OR ALTER PROCEDURE usp_AsignarMeseroMesa
    @IdMesero INT,
    @IdMesa INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 
        FROM dbo.AsignacionMeseros 
        WHERE IdMesero = @IdMesero AND IdMesa = @IdMesa
    )
    BEGIN
        INSERT INTO dbo.AsignacionMeseros (IdMesero, IdMesa)
        VALUES (@IdMesero, @IdMesa);

        SELECT 'Mesa asignada correctamente.' AS Mensaje;
    END
    ELSE
    BEGIN
        SELECT 'Aviso: Esta mesa ya se encuentra asignada a este mesero.' AS Mensaje;
    END
END;
GO

-- ACTUALIZAR: Validando si ya atendió pedidos
CREATE OR ALTER PROCEDURE usp_ActualizarMesero
    @IdMesero INT,
    @Nombre NVARCHAR(50), 
    @Apellido NVARCHAR(50), 
    @DNI CHAR(8), 
    @Telefono NVARCHAR(15), 
    @Turno NVARCHAR(20), 
    @Activo BIT
AS
BEGIN

    IF EXISTS (
        SELECT 1 FROM dbo.Meseros 
        WHERE IdMesero = @IdMesero 
        AND (Nombre <> @Nombre OR Apellido <> @Apellido OR DNI <> @DNI)
    )
    BEGIN

        IF EXISTS (SELECT 1 FROM Pedidos WHERE IdMesero = @IdMesero)
        BEGIN
            SELECT 'No se puede cambiar el Nombre o DNI porque el mesero ya tiene pedidos registrados.' AS Mensaje
            RETURN
        END
    END

    UPDATE dbo.Meseros SET 
        Nombre = @Nombre, 
        Apellido = @Apellido, 
        DNI = @DNI, 
        Telefono = @Telefono, 
        Turno = @Turno, 
        Activo = @Activo
    WHERE IdMesero = @IdMesero

    SELECT 'Mesero actualizado correctamente.' AS Mensaje
END;
GO

CREATE OR ALTER PROCEDURE usp_CambiarEstadoMesero
    @IdMesero INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Meseros WHERE IdMesero = @IdMesero)
    BEGIN
        -- Cambiamos el estado al opuesto (1 a 0 o 0 a 1)
        UPDATE dbo.Meseros 
        SET Activo = CASE WHEN Activo = 1 THEN 0 ELSE 1 END
        WHERE IdMesero = @IdMesero;

        -- Devolvemos el nuevo estado para saber qué pasó
        DECLARE @NuevoEstado BIT;
        SELECT @NuevoEstado = Activo FROM dbo.Meseros WHERE IdMesero = @IdMesero;

        IF @NuevoEstado = 1
            SELECT 'El mesero ha sido ACTIVADO correctamente.' AS Mensaje;
        ELSE
            SELECT 'El mesero ha sido DESACTIVADO correctamente.' AS Mensaje;
    END
    ELSE
    BEGIN
        SELECT 'Error: El ID del mesero no existe.' AS Mensaje;
    END
END;
GO