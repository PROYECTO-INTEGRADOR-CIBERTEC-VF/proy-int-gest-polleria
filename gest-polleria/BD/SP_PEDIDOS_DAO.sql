USE gestpolleriaBD
GO





CREATE   PROC [dbo].[usp_Pedidos_AbrirMesa]
    @IdMesa INT,
    @IdTipoPedido INT,       -- SALON
    @IdMesero INT = NULL,

    @IdPedidoNuevo INT OUTPUT,
    @Ok BIT OUTPUT,
    @Mensaje NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        -- 1) Validar mesa
        IF NOT EXISTS (SELECT 1 FROM dbo.Mesas WHERE IdMesa = @IdMesa)
        BEGIN
            SET @Ok = 0;
            SET @Mensaje = 'La mesa no existe';
            SET @IdPedidoNuevo = 0;
            ROLLBACK TRAN;
            RETURN;
        END

        -- 2) Obtener estado ABIERTO / REGISTRADO
        DECLARE @IdEstadoAbierto INT;

        SELECT TOP 1 @IdEstadoAbierto = IdEstadoPedido
        FROM dbo.EstadosPedido
        WHERE UPPER(Nombre) IN ('ABIERTO','REGISTRADO');

        IF @IdEstadoAbierto IS NULL
        BEGIN
            SET @Ok = 0;
            SET @Mensaje = 'No existe estado ABIERTO o REGISTRADO en EstadosPedido';
            SET @IdPedidoNuevo = 0;
            ROLLBACK TRAN;
            RETURN;
        END

        -- 3) Verificar que la mesa no tenga pedido abierto
        IF EXISTS (
            SELECT 1
            FROM dbo.Pedidos
            WHERE IdMesa = @IdMesa
              AND IdEstadoPedido = @IdEstadoAbierto
        )
        BEGIN
            SET @Ok = 0;
            SET @Mensaje = 'La mesa ya tiene un pedido abierto';
            SET @IdPedidoNuevo = 0;
            ROLLBACK TRAN;
            RETURN;
        END

        -- 4) Insertar pedido
        INSERT INTO dbo.Pedidos
        (
            FechaHora,
            IdTipoPedido,
            IdEstadoPedido,
            IdMesa,
            IdMesero,
            TotalEstimado
        )
        VALUES
        (
            GETDATE(),
            @IdTipoPedido,
            @IdEstadoAbierto,
            @IdMesa,
            @IdMesero,
            0
        );

        SET @IdPedidoNuevo = SCOPE_IDENTITY();

        COMMIT TRAN;

        SET @Ok = 1;
        SET @Mensaje = 'Mesa abierta y pedido creado correctamente';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        SET @Ok = 0;
        SET @Mensaje = ERROR_MESSAGE();
        SET @IdPedidoNuevo = 0;
    END CATCH
END
GO




CREATE   PROC [dbo].[usp_Pedidos_EnviarCocina]
    @IdPedido INT,
    @Ok BIT OUTPUT,
    @Mensaje NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdEstadoPreparacion INT;

    BEGIN TRY
        BEGIN TRAN;

        -- 1) Validar pedido
        IF NOT EXISTS (SELECT 1 FROM dbo.Pedidos WHERE IdPedido = @IdPedido)
        BEGIN
            SET @Ok = 0;
            SET @Mensaje = 'El pedido no existe';
            ROLLBACK TRAN;
            RETURN;
        END

        -- 2) Validar que tenga detalle
        IF NOT EXISTS (
            SELECT 1 FROM dbo.PedidosDetalle WHERE IdPedido = @IdPedido
        )
        BEGIN
            SET @Ok = 0;
            SET @Mensaje = 'El pedido no tiene productos';
            ROLLBACK TRAN;
            RETURN;
        END

        -- 3) Obtener estado PREPARACION
        SELECT TOP 1 @IdEstadoPreparacion = IdEstadoPedido
        FROM dbo.EstadosPedido
        WHERE UPPER(Nombre) IN ('PREPARACION', 'EN PREPARACION');

        IF @IdEstadoPreparacion IS NULL
        BEGIN
            SET @Ok = 0;
            SET @Mensaje = 'No existe el estado PREPARACION';
            ROLLBACK TRAN;
            RETURN;
        END

        -- 4) Cambiar estado del pedido
        UPDATE dbo.Pedidos
        SET IdEstadoPedido = @IdEstadoPreparacion
        WHERE IdPedido = @IdPedido;

        COMMIT TRAN;

        SET @Ok = 1;
        SET @Mensaje = 'Pedido enviado a cocina correctamente';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        SET @Ok = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO



CREATE   PROC [dbo].[usp_Pedidos_CerrarMesa]
    @IdPedido INT,
    @Ok BIT OUTPUT,
    @Mensaje NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdEstadoEntregado INT;

    BEGIN TRY
        BEGIN TRAN;

        -- 1) Validar pedido
        IF NOT EXISTS (SELECT 1 FROM dbo.Pedidos WHERE IdPedido = @IdPedido)
        BEGIN
            SET @Ok = 0;
            SET @Mensaje = 'El pedido no existe';
            ROLLBACK TRAN;
            RETURN;
        END

        -- 2) Obtener estado ENTREGADO
        SELECT @IdEstadoEntregado = IdEstadoPedido
        FROM dbo.EstadosPedido
        WHERE Nombre = 'ENTREGADO';

        IF @IdEstadoEntregado IS NULL
        BEGIN
            SET @Ok = 0;
            SET @Mensaje = 'No existe el estado ENTREGADO';
            ROLLBACK TRAN;
            RETURN;
        END

        -- 3) Validar que tenga detalle
        IF NOT EXISTS (
            SELECT 1 FROM dbo.PedidosDetalle WHERE IdPedido = @IdPedido
        )
        BEGIN
            SET @Ok = 0;
            SET @Mensaje = 'El pedido no tiene productos';
            ROLLBACK TRAN;
            RETURN;
        END

        -- 4) Cerrar pedido (ENTREGADO)
        UPDATE dbo.Pedidos
        SET IdEstadoPedido = @IdEstadoEntregado
        WHERE IdPedido = @IdPedido;

        COMMIT TRAN;

        SET @Ok = 1;
        SET @Mensaje = 'Pedido entregado y mesa cerrada correctamente';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        SET @Ok = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO



CREATE PROC [dbo].[usp_Pedidos_Registrar]
    @IdTipoPedido INT,
    @IdEstadoPedido INT,
    @IdMesa INT = NULL,
    @IdCliente INT = NULL,
    @IdMesero INT = NULL,
    @IdRepartidor INT = NULL,
    @DireccionDelivery NVARCHAR(200) = NULL,
    @ReferenciaDireccion NVARCHAR(200) = NULL,
    @HoraEstimadaEntrega DATETIME2 = NULL,
    @Observaciones NVARCHAR(300) = NULL,
    @TotalEstimado DECIMAL(12,2),
    @IdPedido INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Pedidos (
        FechaHora,
        IdTipoPedido,
        IdEstadoPedido,
        IdMesa,
        IdCliente,
        IdMesero,
        IdRepartidor,
        DireccionDelivery,
        ReferenciaDireccion,
        HoraEstimadaEntrega,
        Observaciones,
        TotalEstimado
    )
    VALUES (
        SYSDATETIME(),
        @IdTipoPedido,
        @IdEstadoPedido,
        @IdMesa,
        @IdCliente,
        @IdMesero,
        @IdRepartidor,
        @DireccionDelivery,
        @ReferenciaDireccion,
        @HoraEstimadaEntrega,
        @Observaciones,
        @TotalEstimado
    );

    SET @IdPedido = SCOPE_IDENTITY();
END
GO



CREATE PROC [dbo].[usp_Pedidos_CambiarEstado]
    @IdPedido INT,
    @IdEstadoPedido INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Pedidos
    SET IdEstadoPedido = @IdEstadoPedido
    WHERE IdPedido = @IdPedido;
END
GO

