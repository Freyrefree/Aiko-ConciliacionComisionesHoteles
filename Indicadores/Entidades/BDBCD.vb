Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("BDBCD")>
Partial Public Class BDBCD
    Public Property id As Integer

    <StringLength(10)>
    Public Property Version As String

    <StringLength(50)>
    Public Property UniqueBookingID As String

    <StringLength(50)>
    Public Property PNR As String

    <StringLength(5)>
    Public Property SequenceNo As String

    <Column(TypeName:="date")>
    Public Property CreateDate As Date?

    <Column(TypeName:="date")>
    Public Property ModifyDate As Date?

    Public Property LineNo As Integer?

    <StringLength(50)>
    Public Property AgencyIDType As String

    Public Property AgencyID As Integer?

    <StringLength(20)>
    Public Property BookingAgent As String

    <StringLength(255)>
    Public Property GuestName As String

    <StringLength(50)>
    Public Property CorporateID As String

    <StringLength(50)>
    Public Property AgentRef1 As String

    <StringLength(50)>
    Public Property AgentRef2 As String

    <StringLength(50)>
    Public Property AgentRef3 As String

    Public Property NumberOfRooms As Integer?

    Public Property NumberOfNights As Integer?

    <Column(TypeName:="date")>
    Public Property DateIn As Date?

    <Column(TypeName:="date")>
    Public Property DateOut As Date?

    Public Property CommissionPercent As Integer?

    Public Property CostPrNight As Decimal?

    <StringLength(50)>
    Public Property FixedCommission As String

    <StringLength(50)>
    Public Property Currency As String

    <StringLength(50)>
    Public Property RateCode As String

    <StringLength(50)>
    Public Property AccommodationType As String

    <StringLength(50)>
    Public Property ConformationNo As String

    <StringLength(50)>
    Public Property HotelPropertyID As String

    <StringLength(50)>
    Public Property HotelChainID As String

    <StringLength(255)>
    Public Property HotelName As String

    <StringLength(255)>
    Public Property Address1 As String

    <StringLength(255)>
    Public Property Address2 As String

    <StringLength(50)>
    Public Property City As String

    <StringLength(50)>
    Public Property State As String

    <StringLength(50)>
    Public Property Zip As String

    <StringLength(50)>
    Public Property AirportCityCode As String

    <StringLength(50)>
    Public Property Phone As String

    <StringLength(50)>
    Public Property Fax As String

    <StringLength(50)>
    Public Property CountryCode As String

    <StringLength(50)>
    Public Property AgentStatusCode As String

    <StringLength(50)>
    Public Property AgentPaymentCode As String

    <Column(TypeName:="date")>
    Public Property FechaAplicacion As Date?

    <StringLength(50)>
    Public Property CodigoConfirmacion As String

    Public Property Comision As Integer?

    <StringLength(50)>
    Public Property Operardor As String

    <StringLength(255)>
    Public Property ClienteTexto As String

    Public Property TarifaSucursal As Decimal?

    <StringLength(50)>
    Public Property firstName As String

    <StringLength(50)>
    Public Property lastName As String

    Public Property estatusConciliado As Integer?

    <StringLength(50)>
    Public Property proveedor As String

    <Column(TypeName:="date")>
    Public Property mesProveedor As Date?
End Class
