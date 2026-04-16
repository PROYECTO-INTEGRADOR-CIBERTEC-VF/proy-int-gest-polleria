USE gestpolleriaBD
GO

/****** Object:  StoredProcedure [dbo].[usp_Clientes_Listar]    Script Date: 15/04/2026 06:16:31 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROC [dbo].[usp_Clientes_Listar]
    @SoloActivos BIT = 1,
    @EsEmpresa BIT = NULL,
    @Buscar NVARCHAR(120) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdCliente,
        TipoDocumento,
        NumeroDocumento,
        RazonSocial,
        Nombres,
        Apellidos,
        Direccion,
        Telefono,
        Email,
        EsEmpresa,
        Activo
    FROM dbo.Clientes
    WHERE
        (@SoloActivos = 0 OR Activo = 1)
        AND (@EsEmpresa IS NULL OR EsEmpresa = @EsEmpresa)
        AND (
            @Buscar IS NULL
            OR RazonSocial LIKE '%' + @Buscar + '%'
            OR Nombres LIKE '%' + @Buscar + '%'
            OR Apellidos LIKE '%' + @Buscar + '%'
            OR NumeroDocumento LIKE '%' + @Buscar + '%'
            OR Telefono LIKE '%' + @Buscar + '%'
            OR Email LIKE '%' + @Buscar + '%'
        )
    ORDER BY IdCliente DESC;
END
GO




CREATE   PROC [dbo].[usp_Clientes_Obtener]
    @IdCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Clientes
    WHERE IdCliente = @IdCliente;
END
GO






CREATE   PROC [dbo].[usp_Clientes_Insertar]
    @TipoDocumento NVARCHAR(10),
    @NumeroDocumento NVARCHAR(20),
    @RazonSocial NVARCHAR(120),
    @Nombres NVARCHAR(80),
    @Apellidos NVARCHAR(80),
    @Direccion NVARCHAR(200),
    @Telefono NVARCHAR(20),
    @Email NVARCHAR(100),
    @EsEmpresa BIT,

    @IdClienteNuevo INT OUTPUT,
    @Ok BIT OUTPUT,
    @Mensaje NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT dbo.Clientes
        (TipoDocumento, NumeroDocumento, RazonSocial, Nombres, Apellidos,
         Direccion, Telefono, Email, EsEmpresa)
        VALUES
        (@TipoDocumento, @NumeroDocumento, @RazonSocial, @Nombres, @Apellidos,
         @Direccion, @Telefono, @Email, @EsEmpresa);

        SET @IdClienteNuevo = SCOPE_IDENTITY();
        SET @Ok = 1;
        SET @Mensaje = 'Cliente registrado correctamente';
    END TRY
    BEGIN CATCH
        SET @Ok = 0;
        SET @Mensaje = ERROR_MESSAGE();
        SET @IdClienteNuevo = 0;
    END CATCH
END
GO





CREATE   PROC [dbo].[usp_Clientes_Actualizar]
    @IdCliente INT,
    @TipoDocumento NVARCHAR(10),
    @NumeroDocumento NVARCHAR(20),
    @RazonSocial NVARCHAR(120),
    @Nombres NVARCHAR(80),
    @Apellidos NVARCHAR(80),
    @Direccion NVARCHAR(200),
    @Telefono NVARCHAR(20),
    @Email NVARCHAR(100),
    @EsEmpresa BIT,

    @Ok BIT OUTPUT,
    @Mensaje NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE dbo.Clientes
        SET TipoDocumento = @TipoDocumento,
            NumeroDocumento = @NumeroDocumento,
            RazonSocial = @RazonSocial,
            Nombres = @Nombres,
            Apellidos = @Apellidos,
            Direccion = @Direccion,
            Telefono = @Telefono,
            Email = @Email,
            EsEmpresa = @EsEmpresa
        WHERE IdCliente = @IdCliente;

        SET @Ok = 1;
        SET @Mensaje = 'Cliente actualizado correctamente';
    END TRY
    BEGIN CATCH
        SET @Ok = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO



CREATE OR ALTER PROC [dbo].[usp_Clientes_Eliminar]
    @IdCliente INT,
    @Ok BIT OUTPUT,
    @Mensaje NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM dbo.Clientes
        WHERE IdCliente = @IdCliente;

        IF @@ROWCOUNT = 0
        BEGIN
            SET @Ok = 0;
            SET @Mensaje = 'El cliente no existe';
            RETURN;
        END

        SET @Ok = 1;
        SET @Mensaje = 'Cliente eliminado correctamente';
    END TRY
    BEGIN CATCH
        SET @Ok = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO


CREATE   PROC [dbo].[usp_Clientes_Desactivar]
    @IdCliente INT,
    @Ok BIT OUTPUT,
    @Mensaje NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Clientes
    SET Activo = 0
    WHERE IdCliente = @IdCliente;

    IF @@ROWCOUNT = 0
    BEGIN
        SET @Ok = 0;
        SET @Mensaje = 'El cliente no existe';
        RETURN;
    END

    SET @Ok = 1;
    SET @Mensaje = 'Cliente desactivado correctamente';
END
GO





CREATE   PROC [dbo].[usp_Clientes_Activar]
    @IdCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Clientes
    SET Activo = 1
    WHERE IdCliente = @IdCliente;

    IF @@ROWCOUNT = 0
    SELECT 'El cliente no existe' AS Mensaje;
ELSE
    SELECT 'Cliente activado correctamente' AS Mensaje;
END
GO

