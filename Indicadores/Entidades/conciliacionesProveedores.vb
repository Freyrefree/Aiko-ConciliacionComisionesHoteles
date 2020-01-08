Imports System
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Linq

Partial Public Class conciliacionesProveedores
    Inherits DbContext

    Public Sub New(ByVal connectionString)
        Me.Database.Connection.ConnectionString = connectionString
    End Sub

    Public Overridable Property BDBCD As DbSet(Of BDBCD)
    Public Overridable Property BDBCDTmp As DbSet(Of BDBCDTmp)
    Public Overridable Property cityexpress As DbSet(Of cityexpress)
    Public Overridable Property cityexpressTmp As DbSet(Of cityexpressTmp)
    Public Overridable Property columnasBDBCD As DbSet(Of columnasBDBCD)
    Public Overridable Property columnasExcel As DbSet(Of columnasExcel)
    Public Overridable Property columnasInterfazBCD As DbSet(Of columnasInterfazBCD)
    Public Overridable Property conciliacion As DbSet(Of conciliacion)
    Public Overridable Property conciliacionDetalleCityExpress As DbSet(Of conciliacionDetalleCityExpress)
    Public Overridable Property conciliacionDetalleGestionCommtrack As DbSet(Of conciliacionDetalleGestionCommtrack)
    Public Overridable Property conciliacionDetalleOnyx As DbSet(Of conciliacionDetalleOnyx)
    Public Overridable Property conciliacionDetallePosadas As DbSet(Of conciliacionDetallePosadas)
    Public Overridable Property conciliacionDetalleTacs As DbSet(Of conciliacionDetalleTacs)
    Public Overridable Property gestionCommtrack As DbSet(Of gestionCommtrack)
    Public Overridable Property gestionCommtrackTmp As DbSet(Of gestionCommtrackTmp)
    Public Overridable Property onyx As DbSet(Of onyx)
    Public Overridable Property onyxComisionesPendientePago As DbSet(Of onyxComisionesPendientePago)
    Public Overridable Property onyxObservaciones As DbSet(Of onyxObservaciones)
    Public Overridable Property onyxPagadas As DbSet(Of onyxPagadas)
    Public Overridable Property onyxTMP As DbSet(Of onyxTMP)
    Public Overridable Property posadas As DbSet(Of posadas)
    Public Overridable Property posadasTmp As DbSet(Of posadasTmp)
    Public Overridable Property prePago As DbSet(Of prePago)
    Public Overridable Property prePagoTmp As DbSet(Of prePagoTmp)
    Public Overridable Property proveedores As DbSet(Of proveedores)
    Public Overridable Property tacs As DbSet(Of tacs)
    Public Overridable Property tacsObservaciones As DbSet(Of tacsObservaciones)
    Public Overridable Property tacsPagadas As DbSet(Of tacsPagadas)
    Public Overridable Property tacsTmp As DbSet(Of tacsTmp)
    Public Overridable Property moneda As DbSet(Of moneda)
    Public Overridable Property tipoCambio As DbSet(Of tipoCambio)
    Public Overridable Property tipoCambioDetalle As DbSet(Of tipoCambioDetalle)

    Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.Version) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.UniqueBookingID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.PNR) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.AgencyIDType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.BookingAgent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.GuestName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.CorporateID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.AgentRef1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.AgentRef2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.AgentRef3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.CostPrNight) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.FixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.Currency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.RateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.AccommodationType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.ConformationNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.HotelPropertyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.HotelChainID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.HotelName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.Address1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.Address2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.City) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.State) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.Zip) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.AirportCityCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.Phone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.Fax) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.CountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.AgentStatusCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.AgentPaymentCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.CodigoConfirmacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.Operardor) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.ClienteTexto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.TarifaSucursal) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.firstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.lastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCD)() _
            .Property(Function(e) e.proveedor) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.Version) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.UniqueBookingID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.PNR) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.AgencyIDType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.BookingAgent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.GuestName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.CorporateID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.AgentRef1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.AgentRef2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.AgentRef3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.CostPrNight) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.FixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.Currency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.RateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.AccommodationType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.ConformationNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.HotelPropertyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.HotelChainID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.HotelName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.Address1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.Address2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.City) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.State) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.Zip) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.AirportCityCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.Phone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.Fax) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.CountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.AgentStatusCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.AgentPaymentCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.CodigoConfirmacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.Operardor) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.ClienteTexto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of BDBCDTmp)() _
            .Property(Function(e) e.TarifaSucursal) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.Reservacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.ReferenciaOTA) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.Monto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.Moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.FormaPago) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.Hotel) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.Huesped) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.Estatus) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.Comision) _
            .IsUnicode(False)



        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.firstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.lastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.CondicionOKAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.CondicionNOAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.CondicionOKManual) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpress)() _
            .Property(Function(e) e.CondicionNOManual) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.Reservacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.ReferenciaOTA) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.Monto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.Moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.FormaPago) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.Hotel) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.Huesped) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.Estatus) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.Comision) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.firstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of cityexpressTmp)() _
            .Property(Function(e) e.lastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of columnasBDBCD)() _
            .Property(Function(e) e.nombreColumna) _
            .IsUnicode(False)

        modelBuilder.Entity(Of columnasExcel)() _
            .Property(Function(e) e.nombreColumna) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacion)() _
            .Property(Function(e) e.nombreConciliacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.idConciliacion) _
            .IsFixedLength()

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.dim_value) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.UserSpec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.Segmento) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.CodigoConfirmacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.Comision) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.Operador) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.Moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.CostoTotalDeLaReserva) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.Noches) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.ComOrig) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.TipoConciliacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleCityExpress)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.idConciliacion) _
            .IsFixedLength()

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.dim_value) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.UserSpec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.Segmento) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.CodigoConfirmacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.Comision) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.Operador) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.Moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.CostoTotalDeLaReserva) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.Noches) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.ComOrig) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleGestionCommtrack)() _
            .Property(Function(e) e.TipoConciliacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.idConciliacion) _
            .IsFixedLength()

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.dim_value) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.UserSpec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.Segmento) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.CodigoConfirmacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.Comision) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.Operador) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.Moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.CostoTotalDeLaReserva) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.Noches) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.ComOrig) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.TipoConciliacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleOnyx)() _
            .Property(Function(e) e.BookingStatusCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.idConciliacion) _
            .IsFixedLength()

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.dim_value) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.UserSpec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.Segmento) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.CodigoConfirmacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.Comision) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.Operador) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.Moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.CostoTotalDeLaReserva) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.Noches) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.ComOrig) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetallePosadas)() _
            .Property(Function(e) e.TipoConciliacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.idConciliacion) _
            .IsFixedLength()

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.dim_value) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.UserSpec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.Segmento) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.CodigoConfirmacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.Comision) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.Operador) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.Moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.CostoTotalDeLaReserva) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.Noches) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.ComOrig) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of conciliacionDetalleTacs)() _
            .Property(Function(e) e.TipoConciliacion) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Usrspec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Trans) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.SuppID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Supplier) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.DIN) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.OUT) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.PAID_AGY) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Confirmationcode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Curr) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Rate) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.First) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.IATA) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Last) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.nitec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Phone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.PNR) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Remark) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Address1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Address2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.VenType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.segnum) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Observaciones) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.Montototaldelareserva) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.No_trxconcatenada) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.CondicionOKAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.CondicionNOAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.CondicionOKManual) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrack)() _
            .Property(Function(e) e.CondicionNOManual) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Usrspec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Trans) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.SuppID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Supplier) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.DIN) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.OUT) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.PAID_AGY) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Confirmationcode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Curr) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Rate) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.First) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.IATA) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Last) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.nitec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Phone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.PNR) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Remark) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Address1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Address2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.VenType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.segnum) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Observaciones) _
            .IsUnicode(False)

        modelBuilder.Entity(Of gestionCommtrackTmp)() _
            .Property(Function(e) e.Montototaldelareserva) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.Version) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.UniqueBookingID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.PNR) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.AgencyIDType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.AgencyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.BookingAgent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.GuestName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.CorporateID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.AgentRef1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.AgentRef2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.AgentRef3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.CostPrNight) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.FixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.Currency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.RateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.AccommodationType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.ConformationNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.HotelPropertyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.HotelChainID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.HotelName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.Address1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.Address2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.City) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.State) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.Zip) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.AirportCityCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.Phone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.Fax) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.CountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.BookingStatusCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.ExtraInfoCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.ConfCommissionPercent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.ConfCostPrNight) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.ConfFixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.ConfCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.PaidStatus) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.BookingReferal) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.PaidCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.PaidCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.PaymentAccount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.OfficeIDBookingAgency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.Invoice_Or_Credit_Number) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_SavingCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_ATOLCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_VoucherType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_Reference1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_Reference2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_Reference3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_Reference4) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_HotelCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_AddressCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_DurationRackRate) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC_DurationRackCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.ConfCommissionVAT) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.PaidCommissionBC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.PaidCommissionNTFee) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.CommissionBookedCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.HotelVAT_ID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.PaidGrossCommissionAmount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.PaidGrossCommissionAmountCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.AccountingAmount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.AccountingCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.firstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.lastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.No_trxconcatenada) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.observaciones) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.TC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyx)() _
            .Property(Function(e) e.PaidCommissionMXN) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.Version) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.UniqueBookingID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.PNR) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.AgencyIDType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.AgencyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.BookingAgent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.GuestName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.CorporateID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.AgentRef1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.AgentRef2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.AgentRef3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.CostPrNight) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.FixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.Currency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.RateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.AccommodationType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.ConformationNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.HotelPropertyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.HotelChainID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.HotelName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.Address1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.Address2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.City) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.State) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.Zip) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.AirportCityCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.Phone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.Fax) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.CountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.BookingStatusCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.ExtraInfoCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.ConfCommissionPercent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.ConfCostPrNight) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.ConfFixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.ConfCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.PaidStatus) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.BookingReferal) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.PaidCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.PaidCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.PaymentAccount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.OfficeIDBookingAgency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.Invoice_Or_Credit_Number) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_SavingCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_ATOLCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_VoucherType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_Reference1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_Reference2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_Reference3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_Reference4) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_HotelCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_AddressCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_DurationRackRate) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC_DurationRackCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.ConfCommissionVAT) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.PaidCommissionBC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.PaidCommissionNTFee) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.CommissionBookedCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.HotelVAT_ID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.PaidGrossCommissionAmount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.PaidGrossCommissionAmountCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.AccountingAmount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.AccountingCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.firstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.lastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.No_trxconcatenada) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.observaciones) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.TC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.PaidCommissionMXN) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.CondicionOKAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxComisionesPendientePago)() _
            .Property(Function(e) e.CondicionNOAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.Version) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.UniqueBookingID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.PNR) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.AgencyIDType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.AgencyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.BookingAgent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.GuestName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.CorporateID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.AgentRef1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.AgentRef2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.AgentRef3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.CostPrNight) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.FixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.Currency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.RateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.AccommodationType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.ConformationNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.HotelPropertyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.HotelChainID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.HotelName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.Address1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.Address2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.City) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.State) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.Zip) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.AirportCityCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.Phone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.Fax) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.CountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.BookingStatusCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.ExtraInfoCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.ConfCommissionPercent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.ConfCostPrNight) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.ConfFixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.ConfCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.PaidStatus) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.BookingReferal) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.PaidCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.PaidCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.PaymentAccount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.OfficeIDBookingAgency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.Invoice_Or_Credit_Number) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_SavingCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_ATOLCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_VoucherType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_Reference1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_Reference2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_Reference3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_Reference4) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_HotelCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_AddressCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_DurationRackRate) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC_DurationRackCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.ConfCommissionVAT) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.PaidCommissionBC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.PaidCommissionNTFee) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.CommissionBookedCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.HotelVAT_ID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.PaidGrossCommissionAmount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.PaidGrossCommissionAmountCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.AccountingAmount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.AccountingCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.firstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.lastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.No_trxconcatenada) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.observaciones) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.PaidCommissionMXN) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.ClienteTexto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxObservaciones)() _
            .Property(Function(e) e.TarifaSucursal) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.Version) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.UniqueBookingID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.PNR) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.AgencyIDType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.AgencyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.BookingAgent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.GuestName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.CorporateID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.AgentRef1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.AgentRef2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.AgentRef3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.CostPrNight) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.FixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.Currency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.RateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.AccommodationType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.ConformationNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.HotelPropertyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.HotelChainID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.HotelName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.Address1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.Address2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.City) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.State) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.Zip) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.AirportCityCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.Phone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.Fax) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.CountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.BookingStatusCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.ExtraInfoCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.ConfCommissionPercent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.ConfCostPrNight) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.ConfFixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.ConfCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.PaidStatus) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.BookingReferal) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.PaidCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.PaidCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.PaymentAccount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.OfficeIDBookingAgency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.Invoice_Or_Credit_Number) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_SavingCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_ATOLCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_VoucherType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_Reference1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_Reference2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_Reference3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_Reference4) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_HotelCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_AddressCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_DurationRackRate) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC_DurationRackCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.ConfCommissionVAT) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.PaidCommissionBC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.PaidCommissionNTFee) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.CommissionBookedCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.HotelVAT_ID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.PaidGrossCommissionAmount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.PaidGrossCommissionAmountCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.AccountingAmount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.AccountingCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.firstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.lastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.No_trxconcatenada) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.observaciones) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.TC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.PaidCommissionMXN) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.CondicionOKAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.CondicionNOAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.CondicionOKManual) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxPagadas)() _
            .Property(Function(e) e.CondicionNOManual) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.Version) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.UniqueBookingID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.PNR) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.SequenceNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.AgencyIDType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.AgencyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.BookingAgent) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.GuestName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.CorporateID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.AgentRef1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.AgentRef2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.AgentRef3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.CostPrNight) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.FixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.Currency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.RateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.AccommodationType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.ConformationNo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.HotelPropertyID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.HotelChainID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.HotelName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.Address1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.Address2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.City) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.State) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.Zip) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.AirportCityCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.Phone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.Fax) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.CountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.BookingStatusCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.ExtraInfoCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.ConfCommissionPercent) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.ConfCostPrNight) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.ConfFixedCommission) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.ConfCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.PaidStatus) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.BookingReferal) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.PaidCommission) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.PaidCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.PaymentAccount) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.OfficeIDBookingAgency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.Invoice_Or_Credit_Number) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_SavingCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_ATOLCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_VoucherType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_Reference1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_Reference2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_Reference3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_Reference4) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_HotelCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_AddressCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_DurationRackRate) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.TC_DurationRackCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.ConfCommissionVAT) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.PaidCommissionBC) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.PaidCommissionNTFee) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.CommissionBookedCurrency) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.HotelVAT_ID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.PaidGrossCommissionAmount) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.PaidGrossCommissionAmountCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.AccountingAmount) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of onyxTMP)() _
            .Property(Function(e) e.AccountingCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.hotel) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.iata) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.clave) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.claveGDS) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.huesped) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.comision) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.noNoches) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.firstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.lastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.totalDeLaReserva) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.percentComision) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.CondicionOkAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.CondicionNoAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.CondicionOKManual) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadas)() _
            .Property(Function(e) e.CondicionNOManual) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadasTmp)() _
            .Property(Function(e) e.hotel) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadasTmp)() _
            .Property(Function(e) e.iata) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadasTmp)() _
            .Property(Function(e) e.clave) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadasTmp)() _
            .Property(Function(e) e.claveGDS) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadasTmp)() _
            .Property(Function(e) e.huesped) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadasTmp)() _
            .Property(Function(e) e.comision) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadasTmp)() _
            .Property(Function(e) e.moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of posadasTmp)() _
            .Property(Function(e) e.percentComision) _
            .IsUnicode(False)

        modelBuilder.Entity(Of prePago)() _
            .Property(Function(e) e.usrSpec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of prePago)() _
            .Property(Function(e) e.confirmationCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of prePago)() _
            .Property(Function(e) e.comisionAplicar) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of prePago)() _
            .Property(Function(e) e.operador) _
            .IsUnicode(False)

        modelBuilder.Entity(Of prePago)() _
            .Property(Function(e) e.moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of prePago)() _
            .Property(Function(e) e.costoTotaldeLaReserva) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of prePago)() _
            .Property(Function(e) e.comisionOriginal) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of prePagoTmp)() _
            .Property(Function(e) e.usrSpec) _
            .IsUnicode(False)

        modelBuilder.Entity(Of prePagoTmp)() _
            .Property(Function(e) e.confirmationCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of prePagoTmp)() _
            .Property(Function(e) e.comisionAplicar) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of prePagoTmp)() _
            .Property(Function(e) e.operador) _
            .IsUnicode(False)

        modelBuilder.Entity(Of prePagoTmp)() _
            .Property(Function(e) e.moneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of prePagoTmp)() _
            .Property(Function(e) e.costoTotaldeLaReserva) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of prePagoTmp)() _
            .Property(Function(e) e.comisionOriginal) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of proveedores)() _
            .Property(Function(e) e.nombre) _
            .IsUnicode(False)

        modelBuilder.Entity(Of proveedores)() _
            .Property(Function(e) e.activo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.RecordType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.TACSRecordID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.LastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.FirstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.TxnCd) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.Confirmation) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.Arrival) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.Departure) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.ReportRevenue) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.ReportCom) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.ReportCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PayCom) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PayCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.HotelGroupCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.HotelGroupName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PropertyCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PropertyName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PropertyAddr1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PropertyAddr2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PropertyCity) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PropertyStateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PropertyPostalCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PropertyCountry) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.Propertytaxid) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.HoldbackCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.Holdback) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.Fee) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.TacsagencyId) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.Arc_num) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.AgencyLegalName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.AgencyName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.AgencyAttn) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.AgencyAddr1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.AgencyAddr2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.AgencyCity) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.AgencyStateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.AgencyCountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.PropertyPhone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.RevenueReportCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacs)() _
            .Property(Function(e) e.observaciones) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.RecordType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.TACSRecordID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.LastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.FirstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.TxnCd) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.Confirmation) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.Arrival) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.Departure) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.ReportRevenue) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.ReportCom) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.ReportCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PayCom) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PayCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.HotelGroupCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.HotelGroupName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PropertyCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PropertyName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PropertyAddr1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PropertyAddr2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PropertyCity) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PropertyStateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PropertyPostalCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PropertyCountry) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.Propertytaxid) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.HoldbackCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.Holdback) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.Fee) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.TacsagencyId) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.Arc_num) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.AgencyLegalName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.AgencyName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.AgencyAttn) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.AgencyAddr1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.AgencyAddr2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.AgencyCity) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.AgencyStateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.AgencyCountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.PropertyPhone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.RevenueReportCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.observaciones) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsObservaciones)() _
            .Property(Function(e) e.TC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.RecordType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.TACSRecordID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.LastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.FirstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.TxnCd) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.Confirmation) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.Arrival) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.Departure) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.ReportRevenue) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.ReportCom) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.ReportCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PayCom) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PayCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.HotelGroupCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.HotelGroupName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PropertyCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PropertyName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PropertyAddr1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PropertyAddr2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PropertyCity) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PropertyStateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PropertyPostalCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PropertyCountry) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.Propertytaxid) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.HoldbackCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.Holdback) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.Fee) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.TacsagencyId) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.Arc_num) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.AgencyLegalName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.AgencyName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.AgencyAttn) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.AgencyAddr1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.AgencyAddr2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.AgencyCity) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.AgencyStateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.AgencyCountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PropertyPhone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.RevenueReportCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.observaciones) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.TC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PayComTC) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.PayCurrencyTC) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.CondicionOKAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.CondicionNOAuto) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.CondicionOKManual) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsPagadas)() _
            .Property(Function(e) e.CondicionNOManual) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.RecordType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.TACSRecordID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.LastName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.FirstName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.TxnCd) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.Confirmation) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.Arrival) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.Departure) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.ReportRevenue) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.ReportCom) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.ReportCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PayCom) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PayCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.HotelGroupCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.HotelGroupName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PropertyCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PropertyName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PropertyAddr1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PropertyAddr2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PropertyCity) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PropertyStateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PropertyPostalCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PropertyCountry) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.Propertytaxid) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.HoldbackCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.Holdback) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.Fee) _
            .HasPrecision(18, 3)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.TacsagencyId) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.Arc_num) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.AgencyLegalName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.AgencyName) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.AgencyAttn) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.AgencyAddr1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.AgencyAddr2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.AgencyCity) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.AgencyStateCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.AgencyCountryCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.PropertyPhone) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tacsTmp)() _
            .Property(Function(e) e.RevenueReportCurrency) _
            .IsUnicode(False)

        modelBuilder.Entity(Of moneda)() _
            .Property(Function(e) e.codigo) _
            .IsUnicode(False)

        modelBuilder.Entity(Of moneda)() _
            .Property(Function(e) e.nombreMoneda) _
            .IsUnicode(False)

        modelBuilder.Entity(Of tipoCambioDetalle)() _
            .Property(Function(e) e.valorMoneda) _
            .HasPrecision(18, 3)

    End Sub
End Class
