USE [master]
GO

CREATE DATABASE [PracticaS13]
GO

USE [PracticaS13]
GO

CREATE TABLE [dbo].[Abonos](
	[Id_Compra] [bigint] NOT NULL,
	[Id_Abono] [bigint] IDENTITY(1,1) NOT NULL,
	[Monto] [decimal](18, 2) NOT NULL,
	[Fecha] [datetime] NOT NULL,
 CONSTRAINT [PK_Abonos] PRIMARY KEY CLUSTERED 
(
	[Id_Abono] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Principal](
	[Id_Compra] [bigint] IDENTITY(1,1) NOT NULL,
	[Precio] [decimal](18, 5) NOT NULL,
	[Saldo] [decimal](18, 5) NOT NULL,
	[Descripcion] [varchar](500) NOT NULL,
	[Estado] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Principal] PRIMARY KEY CLUSTERED 
(
	[Id_Compra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Principal] ON 
GO
INSERT [dbo].[Principal] ([Id_Compra], [Precio], [Saldo], [Descripcion], [Estado]) VALUES (1, CAST(50000.00000 AS Decimal(18, 5)), CAST(50000.00000 AS Decimal(18, 5)), N'Producto 1', N'Pendiente')
GO
INSERT [dbo].[Principal] ([Id_Compra], [Precio], [Saldo], [Descripcion], [Estado]) VALUES (2, CAST(13500.00000 AS Decimal(18, 5)), CAST(13500.00000 AS Decimal(18, 5)), N'Producto 2', N'Pendiente')
GO
INSERT [dbo].[Principal] ([Id_Compra], [Precio], [Saldo], [Descripcion], [Estado]) VALUES (3, CAST(83600.00000 AS Decimal(18, 5)), CAST(83600.00000 AS Decimal(18, 5)), N'Producto 3', N'Pendiente')
GO
INSERT [dbo].[Principal] ([Id_Compra], [Precio], [Saldo], [Descripcion], [Estado]) VALUES (4, CAST(1220.00000 AS Decimal(18, 5)), CAST(1220.00000 AS Decimal(18, 5)), N'Producto 4', N'Pendiente')
GO
INSERT [dbo].[Principal] ([Id_Compra], [Precio], [Saldo], [Descripcion], [Estado]) VALUES (5, CAST(480.00000 AS Decimal(18, 5)), CAST(480.00000 AS Decimal(18, 5)), N'Producto 5', N'Pendiente')
GO
SET IDENTITY_INSERT [dbo].[Principal] OFF
GO

ALTER TABLE [dbo].[Abonos]  WITH CHECK ADD  CONSTRAINT [FK_Abonos_Principal] FOREIGN KEY([Id_Compra])
REFERENCES [dbo].[Principal] ([Id_Compra])
GO
ALTER TABLE [dbo].[Abonos] CHECK CONSTRAINT [FK_Abonos_Principal]
GO

/*PROCEDIMIENTOS ALMECENADOS*/

CREATE OR ALTER PROCEDURE dbo.sp_Compras_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id_Compra, Precio, Saldo, Descripcion, Estado
    FROM dbo.Principal
    ORDER BY CASE WHEN Estado='Pendiente' THEN 0 ELSE 1 END, Id_Compra;
END
GO

-- 2. Solo compras pendientes (para dropdown)
CREATE OR ALTER PROCEDURE dbo.sp_Compras_ListarPend
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id_Compra, Descripcion, Saldo
    FROM dbo.Principal
    WHERE Estado='Pendiente'
    ORDER BY Id_Compra;
END
GO

-- 3. Obtener saldo de una compra
CREATE OR ALTER PROCEDURE dbo.sp_Compras_Saldo
    @Id_Compra BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 Saldo FROM dbo.Principal WHERE Id_Compra=@Id_Compra;
END
GO

-- 4. Registrar abono (transaccional + validación)
CREATE OR ALTER PROCEDURE dbo.sp_Abonos_Registrar
    @Id_Compra BIGINT,
    @Monto DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @SaldoActual DECIMAL(18,5);

    SELECT @SaldoActual = Saldo FROM dbo.Principal WITH (UPDLOCK, ROWLOCK) WHERE Id_Compra=@Id_Compra;

    IF @SaldoActual IS NULL
    BEGIN
        RAISERROR('Compra no existe.', 16, 1);
        RETURN;
    END

    IF @Monto <= 0
    BEGIN
        RAISERROR('El monto del abono debe ser mayor a cero.', 16, 1);
        RETURN;
    END

    IF @Monto > @SaldoActual
    BEGIN
        RAISERROR('El abono no puede ser mayor al saldo.', 16, 1);
        RETURN;
    END

    BEGIN TRAN;

    INSERT INTO dbo.Abonos (Id_Compra, Monto, Fecha)
    VALUES (@Id_Compra, @Monto, SYSDATETIME());

    DECLARE @NuevoSaldo DECIMAL(18,5) = @SaldoActual - @Monto;
    UPDATE dbo.Principal
      SET Saldo = @NuevoSaldo,
          Estado = CASE WHEN @NuevoSaldo = 0 THEN 'Cancelado' ELSE Estado END
    WHERE Id_Compra = @Id_Compra;

    COMMIT TRAN;
END
GO