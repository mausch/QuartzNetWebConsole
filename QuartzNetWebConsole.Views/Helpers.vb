Public Module Helpers
    Public Function SimpleForm(ByVal action As String, ByVal button As String) As XElement
        Return _
        <form method="post" action=<%= action %>>
            <input type="submit" value=<%= button %>/>
        </form>
    End Function

    Public ReadOnly Stylesheet As XElement = <link rel="stylesheet" type="text/css" href="static.ashx?r=styles.css&amp;t=text%2Fcss"/>

    Public Function YesNo(ByVal b As Boolean) As String
        Return If(b, "Yes", "No")
    End Function

    Public Function KV(Of K, V)(ByVal key As K, ByVal value As V) As KeyValuePair(Of K, V)
        Return New KeyValuePair(Of K, V)(key, value)
    End Function

    Public Function Index(Of T)(ByVal a As IEnumerable(Of T)) As IEnumerable(Of KeyValuePair(Of Integer, T))
        Return a.Select(Function(e, i) KV(i, e))
    End Function

    Public Function SpacesToNbsp(ByVal s As String) As String
        Return s.Replace(" "c, ChrW(&HA0))
    End Function

    Public ReadOnly laquo As Char = ChrW(&HAB)
    Public ReadOnly raquo As Char = ChrW(&HBB)

End Module
