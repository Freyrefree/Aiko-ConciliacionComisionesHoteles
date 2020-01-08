Public Module Enumeraciones
    Public Enum TiposDeDatos
        NUMERICO = 0
        TEXTO = 1
        MONEDA = 2
        FECHA = 3
    End Enum

    Public Enum Operadores
        NUMERICO_IGUAL = 0
        TEXTO_IGUAL = 1
        TEXTO_CONTIENE = 2
        MONEDA_IGUAL = 3
        FECHA_IGUAL = 4
    End Enum

    Public ReadOnly ListaTiposDatos As List(Of String) = New List(Of String) From {"NUMERICO", "TEXTO", "MONEDA", "FECHA"}
    Public ReadOnly ListaTiposOperadoresNumerico As List(Of String) = New List(Of String) From {"IGUALDAD"}
    Public ReadOnly ListaTiposOperadoresTexto As List(Of String) = New List(Of String) From {"IGUALDAD", "CONTIENE"}
    Public ReadOnly ListaTiposOperadoresMoneda As List(Of String) = New List(Of String) From {"IGUALDAD"}
    Public ReadOnly ListaTiposOperadoresFecha As List(Of String) = New List(Of String) From {"IGUALDAD"}
    Public ReadOnly ListaMeses As List(Of String) = New List(Of String) From {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
    Public ReadOnly ListaMesesAbreviados As List(Of String) = New List(Of String) From {"Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"}
    Public ReadOnly RegexDictionary As Dictionary(Of UInt16, String) = New Dictionary(Of UShort, String) From {
        {1, "(\d+)(\r|\n|\r\n)([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{3})(\r|\n|\r\n)(((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?))(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(([A-Z]{2}) (\d*) \/ (Clase [A-Z])) el ([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2}) ([A-Z]+)(\r|\n|\r\n)([A-Z ]+)"},
        {2, "(\d+)(\r|\n|\r\n)([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{3})(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(([A-Z]{2}) (\d+) \/ (Clase [A-Z] el (([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2}))))(\r|\n|\r\n)([A-Z ]+)"},
        {3, "(\d+)(\r|\n|\r\n)([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{3})(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)()(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{2} (\d+) \/ (Clase [A-Z]) el ([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2}) ([A-Z]+))(\r|\n|\r\n)([A-Z ]+)"},
        {4, "(\d+)(\r|\n|\r\n)([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{3})(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)()(\d+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{2} (\d+) \/ (Clase [A-Z] el (([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2}) [A-Z]+)))(\r|\n|\r\n)([A-Za-z \.]+)"},
        {5, "(\d+)(\r|\n|\r\n)([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{3})(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)A(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(([A-Z]{2}) (\d*) \/ (Clase [A-Z] el ([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2}) ([A-Z]+)))(\r|\n|\r\n)([A-Z ]+)"},
        {6, "(\d+)(\r|\n|\r\n)([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{3})(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)(Número de identificación del IVA: ([A-Z]+[0-9]+))(\r|\n|\r\n)(Ref.: ([A-Z]+ el (([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2}))))(\r|\n|\r\n)([A-Z ]+)"},
        {7, "(\d+)(\r|\n|\r\n)([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{3})(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?))(\r|\n|\r\n)(Número de identificación del IVA: ([A-Z0-9]+))(\r|\n|\r\n)(Ref.: ([A-Z]+ el ([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})))(\r|\n|\r\n)(RC Low Cost)"},
        {8, "(\d+)(\r|\n|\r\n)([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{3})(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)(Service FEE)"}
    }
    '(\d+)(\r|\n|\r\n)([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)(\d+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)(\r|\n|\r\n)([A-Z]{3})*(\r|\n|\r\n)*((((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?))[-]*)(\r|\n|\r\n)((((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?))[-]*)(\r|\n|\r\n)([A-Z]{1}(\r|\n|\r\n))*(((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)(\r|\n|\r\n))*(([\+])(\r|\n|\r\n)((((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?)))(\r|\n|\r\n))*([A-Z]+)(\r|\n|\r\n)([A-Z]+)(\r|\n|\r\n)([\p{L} \.\,-_\(\)&%\$#""']+)
    ' Nueva regex para santander.
End Module
