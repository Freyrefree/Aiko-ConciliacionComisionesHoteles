Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("onyxTMP")>
Partial Public Class onyxTMP
    Public Property id As Integer

    <StringLength(50)>
    Public Property Version As String

    <StringLength(50)>
    Public Property UniqueBookingID As String

    <StringLength(50)>
    Public Property PNR As String

    <StringLength(50)>
    Public Property SequenceNo As String

    <Column(TypeName:="date")>
    Public Property CreateDate As Date?

    <Column(TypeName:="date")>
    Public Property ModifyDate As Date?

    Public Property LineNo As Integer?

    <StringLength(50)>
    Public Property AgencyIDType As String

    <StringLength(50)>
    Public Property AgencyID As String

    <StringLength(50)>
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

    <StringLength(255)>
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

    Public Property StatusDateTime As Date?

    <StringLength(50)>
    Public Property BookingStatusCode As String

    <StringLength(50)>
    Public Property ExtraInfoCode As String

    Public Property ConfNoRooms As Integer?

    Public Property ConfNoNights As Integer?

    <Column(TypeName:="date")>
    Public Property ConfDateIn As Date?

    <Column(TypeName:="date")>
    Public Property ConfDateOut As Date?

    Public Property ConfCommissionPercent As Decimal?

    Public Property ConfCostPrNight As Decimal?

    <StringLength(50)>
    Public Property ConfFixedCommission As String

    <StringLength(50)>
    Public Property ConfCurrency As String

    <StringLength(50)>
    Public Property PaidStatus As String

    Public Property NTCommissionID As Integer?

    Public Property NTHotelAccount As Integer?

    <StringLength(50)>
    Public Property BookingReferal As String

    Public Property PaymentJournal As Integer?

    Public Property PaidCommission As Decimal?

    <StringLength(50)>
    Public Property PaidCurrency As String

    Public Property PaymentPoint As Integer?

    <StringLength(50)>
    Public Property PaymentAccount As String

    <Column(TypeName:="date")>
    Public Property PaymentDate As Date?

    <StringLength(50)>
    Public Property OfficeIDBookingAgency As String

    <StringLength(50)>
    Public Property Invoice_Or_Credit_Number As String

    <StringLength(50)>
    Public Property TC_SavingCode As String

    <StringLength(50)>
    Public Property TC_ATOLCode As String

    <StringLength(50)>
    Public Property TC_VoucherType As String

    <StringLength(50)>
    Public Property TC_Reference1 As String

    <StringLength(50)>
    Public Property TC_Reference2 As String

    <StringLength(50)>
    Public Property TC_Reference3 As String

    <StringLength(50)>
    Public Property TC_Reference4 As String

    <StringLength(50)>
    Public Property TC_HotelCode As String

    <StringLength(50)>
    Public Property TC_AddressCode As String

    <StringLength(50)>
    Public Property TC_DurationRackRate As String

    <StringLength(50)>
    Public Property TC_DurationRackCurrency As String

    Public Property ConfCommissionVATPercent As Integer?

    Public Property ConfCommissionVAT As Decimal?

    Public Property PaidCommissionBC As Decimal?

    Public Property PaidCommissionNTFee As Decimal?

    Public Property CommissionBookedCurrency As Decimal?

    <Column("HotelVAT-ID")>
    <StringLength(50)>
    Public Property HotelVAT_ID As String

    <Column("VAT-Amount-onFeeNTS")>
    Public Property VAT_Amount_onFeeNTS As Integer?

    <Column("VAT-Percentage-onFeeNTS")>
    Public Property VAT_Percentage_onFeeNTS As Integer?

    Public Property ISVATCalculated As Integer?

    Public Property PaidGrossCommissionAmount As Decimal?

    <StringLength(50)>
    Public Property PaidGrossCommissionAmountCurrency As String

    Public Property AccountingAmount As Decimal?

    <StringLength(50)>
    Public Property AccountingCurrency As String

    Public Property OnTacsDocument As Integer?

    <Column(TypeName:="date")>
    Public Property Fechadepago As Date?
End Class
