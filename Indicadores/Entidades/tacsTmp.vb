Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("tacsTmp")>
Partial Public Class tacsTmp
    Public Property id As Integer

    <StringLength(50)>
    Public Property RecordType As String

    <StringLength(255)>
    Public Property TACSRecordID As String

    <StringLength(50)>
    Public Property LastName As String

    <StringLength(50)>
    Public Property FirstName As String

    <StringLength(50)>
    Public Property TxnCd As String

    <StringLength(50)>
    Public Property Confirmation As String

    <StringLength(50)>
    Public Property Arrival As String

    <StringLength(50)>
    Public Property Departure As String

    Public Property ReportRevenue As Decimal?

    Public Property ReportCom As Decimal?

    <StringLength(50)>
    Public Property ReportCurrency As String

    Public Property PayCom As Decimal?

    <StringLength(50)>
    Public Property PayCurrency As String

    <StringLength(50)>
    Public Property HotelGroupCode As String

    <StringLength(255)>
    Public Property HotelGroupName As String

    <StringLength(50)>
    Public Property PropertyCode As String

    <StringLength(255)>
    Public Property PropertyName As String

    <StringLength(50)>
    Public Property PropertyAddr1 As String

    <StringLength(50)>
    Public Property PropertyAddr2 As String

    <StringLength(50)>
    Public Property PropertyCity As String

    <StringLength(50)>
    Public Property PropertyStateCode As String

    <StringLength(50)>
    Public Property PropertyPostalCode As String

    <StringLength(50)>
    Public Property PropertyCountry As String

    <StringLength(50)>
    Public Property Propertytaxid As String

    <StringLength(50)>
    Public Property HoldbackCurrency As String

    Public Property Holdback As Decimal?

    Public Property Fee As Decimal?

    Public Property PayeeIDfromPayor As Integer?

    <StringLength(50)>
    Public Property TacsagencyId As String

    Public Property Iata As Integer?

    <StringLength(50)>
    Public Property Arc_num As String

    <StringLength(50)>
    Public Property AgencyLegalName As String

    <StringLength(50)>
    Public Property AgencyName As String

    <StringLength(50)>
    Public Property AgencyAttn As String

    <StringLength(50)>
    Public Property AgencyAddr1 As String

    <StringLength(50)>
    Public Property AgencyAddr2 As String

    <StringLength(50)>
    Public Property AgencyCity As String

    <StringLength(50)>
    Public Property AgencyStateCode As String

    Public Property AgencyZip As Integer?

    <StringLength(50)>
    Public Property AgencyCountryCode As String

    <StringLength(50)>
    Public Property PropertyPhone As String

    Public Property PaymentID As Integer?

    Public Property ChequeNumber As Integer?

    Public Property PayDate As Integer?

    <StringLength(50)>
    Public Property RevenueReportCurrency As String

    Public Property RoomNights As Integer?

    Public Property estatusConciliado As Integer?
End Class
