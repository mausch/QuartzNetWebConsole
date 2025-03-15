Public Module Helpers
    Public Function SimpleForm(action As String, button As String) As XElement
        Return _
        <form method="post" action=<%= action %>>
            <input type="submit" value=<%= button %>/>
        </form>
    End Function

    Public ReadOnly Stylesheet As XElement() = _
        <x>
            <link rel="stylesheet" type="text/css" href="static.ashx?r=styles.css&amp;t=text%2Fcss"/>
            <button id="time-toggle" style="position: fixed; top: 10px; right: 10px;">Toogle local/UTC time</button>
            <script src="static.ashx?r=time-toggle.js&amp;t=application%2Fjavascript"></script>
        </x>.Elements.ToArray()

    Public Function YesNo(b As Boolean) As String
        Return If(b, "Yes", "No")
    End Function

    Public Function KV(Of K, V)(key As K, value As V) As KeyValuePair(Of K, V)
        Return New KeyValuePair(Of K, V)(key, value)
    End Function

    Public Sub StripeTrs(xml As XElement)
        For Each table In xml...<table>
            Dim t = table
            Dim trs = From x In t...<tr>
            trs = trs.Skip(1).WhereOdd()
            For Each tr In trs
                Dim clas = tr.Attribute("class")
                If clas IsNot Nothing Then
                    clas.SetValue(clas.Value + " " + "alt")
                Else
                    tr.Add(New XAttribute("class", "alt"))
                End If
            Next
        Next
    End Sub

    Public Function XHTML(e As XElement) As XDocument
        StripeTrs(e)
        Return e.MakeHTML5Doc()
    End Function

    Public Function IfNullable(Of T)(value As Boolean?, ifNull As T, ifTrue As T, ifFalse As T) As T
        If Not value.HasValue Then
            Return ifNull
        End If
        Return If(value.Value, ifTrue, ifFalse)
    End Function
End Module
