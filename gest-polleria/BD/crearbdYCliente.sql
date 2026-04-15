Create database gestpolleriaBD
go

use gestpolleriaBD


go

CREATE TABLE [dbo].[Clientes](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[TipoDocumento] [nvarchar](10) NULL,
	[NumeroDocumento] [nvarchar](20) NULL,
	[RazonSocial] [nvarchar](120) NULL,
	[Nombres] [nvarchar](80) NULL,
	[Apellidos] [nvarchar](80) NULL,
	[Direccion] [nvarchar](200) NULL,
	[Telefono] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[EsEmpresa] [bit] NOT NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Clientes] ADD  DEFAULT ((0)) FOR [EsEmpresa]
GO

ALTER TABLE [dbo].[Clientes] ADD  CONSTRAINT [DF_Clientes_Activo]  DEFAULT ((1)) FOR [Activo]
GO
