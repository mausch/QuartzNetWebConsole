Imports MiniMVC

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

    Public Sub StripeTrs(ByVal xml As XElement)
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

    Public Function XHTML(ByVal e As XElement) As XDocument
        StripeTrs(e)
        Return X.MakeHTML5Doc(e)
    End Function
End Module
