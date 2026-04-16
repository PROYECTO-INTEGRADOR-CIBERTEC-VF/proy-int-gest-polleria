USE gestpolleriaBD
go

CREATE TABLE [dbo].[Mesas](
	[IdMesa] [int] IDENTITY(1,1) NOT NULL,
	[NumeroMesa] [int] NOT NULL,
	[Descripcion] [nvarchar](100) NULL,
	[Capacidad] [int] NOT NULL,
	[Activa] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdMesa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Mesas_NumeroMesa] UNIQUE NONCLUSTERED 
(
	[NumeroMesa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Mesas] ADD  DEFAULT ((2)) FOR [Capacidad]
GO

ALTER TABLE [dbo].[Mesas] ADD  DEFAULT ((1)) FOR [Activa]
GO

ALTER TABLE [dbo].[Mesas]  WITH CHECK ADD  CONSTRAINT [CK_Mesas_Capacidad_Positive] CHECK  (([Capacidad]>(0)))
GO

ALTER TABLE [dbo].[Mesas] CHECK CONSTRAINT [CK_Mesas_Capacidad_Positive]
GO




CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[ClaveHash] [nvarchar](200) NOT NULL,
	[NombreCompleto] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Telefono] [nvarchar](20) NULL,
	[Activo] [bit] NOT NULL,
	[FechaRegistro] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((1)) FOR [Activo]
GO

ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (sysdatetime()) FOR [FechaRegistro]
GO




CREATE TABLE [dbo].[EstadosPedido](
	[IdEstadoPedido] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEstadoPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EstadosPedido_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[TiposPedido](
	[IdTipoPedido] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTipoPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_TiposPedido_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[Pedidos](
	[IdPedido] [int] IDENTITY(1,1) NOT NULL,
	[FechaHora] [datetime2](7) NOT NULL,
	[IdTipoPedido] [int] NOT NULL,
	[IdEstadoPedido] [int] NOT NULL,
	[IdMesa] [int] NULL,
	[IdCliente] [int] NULL,
	[IdMesero] [int] NULL,
	[IdRepartidor] [int] NULL,
	[DireccionDelivery] [nvarchar](200) NULL,
	[ReferenciaDireccion] [nvarchar](200) NULL,
	[HoraEstimadaEntrega] [datetime2](7) NULL,
	[Observaciones] [nvarchar](300) NULL,
	[TotalEstimado] [decimal](12, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Pedidos] ADD  DEFAULT (sysdatetime()) FOR [FechaHora]
GO

ALTER TABLE [dbo].[Pedidos] ADD  DEFAULT ((0)) FOR [TotalEstimado]
GO

ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO

ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD FOREIGN KEY([IdEstadoPedido])
REFERENCES [dbo].[EstadosPedido] ([IdEstadoPedido])
GO

ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD FOREIGN KEY([IdMesa])
REFERENCES [dbo].[Mesas] ([IdMesa])
GO

ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD FOREIGN KEY([IdMesero])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO

ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD FOREIGN KEY([IdRepartidor])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO

ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD FOREIGN KEY([IdTipoPedido])
REFERENCES [dbo].[TiposPedido] ([IdTipoPedido])
GO

ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [CK_Pedidos_TotalEstimado_NonNegative] CHECK  (([TotalEstimado]>=(0)))
GO

ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [CK_Pedidos_TotalEstimado_NonNegative]
GO











