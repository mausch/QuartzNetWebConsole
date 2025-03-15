Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Xml
Imports System.Xml.Linq

Public Module X
    Public ReadOnly XHTML1_0_Transitional_Doctype As XDocumentType = New XDocumentType("html", "-//W3C//DTD XHTML 1.0 Transitional//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", Nothing)

    Public ReadOnly XHTML1_0_Strict_Doctype As XDocumentType = New XDocumentType("html", "-//W3C//DTD XHTML 1.0 Strict//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", Nothing)

    Public ReadOnly HTML5_Doctype As XDocumentType = New XDocumentType("html", Nothing, Nothing, Nothing)

    Public ReadOnly nbsp As String = "\u00A0"

    Public ReadOnly raquo As String = "»"

    Public ReadOnly laquo As String = "«"

    Public ReadOnly rsaquo As String = ChrW(8250)

    Public ReadOnly lsaquo As String = ChrW(8249)

    Public ReadOnly copy As String = "©"

    Public ReadOnly amp As String = "&"

    Public ReadOnly lt As String = "<"

    Public ReadOnly gt As String = ">"

    Public ReadOnly XHTML_Namespace As XNamespace = XNamespace.[Get]("http://www.w3.org/1999/xhtml")

    Private ReadOnly emptyElems As HashSet(Of String) = New HashSet(Of String)() From {"area", "base", "basefont", "br", "col", "command", "frame", "hr", "img", "input", "isindex", "keygen", "link", "meta", "param", "source", "track", "wbr"}

    Public ReadOnly NoElements As IEnumerable(Of XElement) = Enumerable.Empty(Of XElement)()

    Public ReadOnly NoNodes As IEnumerable(Of XNode) = Enumerable.Empty(Of XNode)()

    Public Function A(name As String, value As String) As XAttribute
        Return New XAttribute(XName.[Get](name), value)
    End Function

    Public Function E(name As String, ParamArray content As Object()) As XElement
        Return New XElement(XName.[Get](name), content)
    End Function

    Public Function T(text As String) As XNode
        Return New XText(text)
    End Function

    Public Function Raw(xml As String) As XNode()
        Try
            Return XDocument.Parse("<x>" + xml + "</x>", LoadOptions.PreserveWhitespace).Document.Root.Nodes().ToArray
        Catch ex as Exception
            Throw New Exception($"Error parsing '{xml}'")
        End Try
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function Alter(e As XElement, pred As Func(Of Boolean), a As Action(Of XElement)) As XElement
        If pred() Then
            a(e)
        End If
        Return e
    End Function

    Public Function IsEmptyElement(elementName As String) As Boolean
        Return emptyElems.Contains(elementName)
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function IsEmptyElement(element As XElement) As Boolean
        Return X.IsEmptyElement(element.Name.LocalName)
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function FixEmptyElements(n As XNode) As XNode
        Dim e = TryCast(n, XElement)
        If e Is Nothing Then
            Return n
        End If
        Dim isEmptyElem = e.IsEmptyElement()
        If isEmptyElem AndAlso Not e.IsEmpty Then
            Return New XElement(e.Name, e.Attributes())
        End If
        Dim children = e.Nodes().Select(AddressOf FixEmptyElements)
        If Not isEmptyElem AndAlso e.IsEmpty Then
            Return New XElement(e.Name, {e.Attributes(), New XText(""), children})
        End If
        Return New XElement(e.Name, {e.Attributes(), children})
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function ApplyNamespace(n As XNode, ns As XNamespace) As XNode
        Dim e = TryCast(n, XElement)
        If e IsNot Nothing Then
            Dim arg_58_0 = ns + e.Name.LocalName
            Dim children = e.Nodes().Select(Function(x As XNode) x.ApplyNamespace(ns))
            Return New XElement(arg_58_0, {e.Attributes(), children})
        End If
        Return n
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function MakeHTMLCompatible(n As XNode) As XNode
        Return n.ApplyNamespace(X.XHTML_Namespace).FixEmptyElements()
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function MakeHTML5Doc(root As XElement) As XDocument
        Return New XDocument({X.HTML5_Doctype, root.MakeHTMLCompatible()})
    End Function

    Public Function CreateXmlWriter(output As Stream) As XmlWriter
        Dim settings = New XmlWriterSettings() With {.OmitXmlDeclaration = True, .ConformanceLevel = ConformanceLevel.Fragment, .NewLineHandling = NewLineHandling.None, .Encoding = New UTF8Encoding(False)}
        Return XmlWriter.Create(output, settings)
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Sub WriteToStream(n As XNode, output As Stream)
        If n Is Nothing Then
            Return
        End If
        Using xmlwriter = X.CreateXmlWriter(output)
            n.FixEmptyElements().WriteTo(xmlwriter)
        End Using
    End Sub

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Sub WriteToStream(nodes As IEnumerable(Of XNode), output As Stream)
        If nodes Is Nothing Then
            Return
        End If
        Dim root = TryCast(<x><%= nodes %></x>.FixEmptyElements(), XElement)
        Using xmlwriter = X.CreateXmlWriter(output)
            Using enumerator = root.Nodes().GetEnumerator()
                While enumerator.MoveNext()
                    enumerator.Current.WriteTo(xmlwriter)
                End While
            End Using
        End Using
    End Sub

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function WriteToString(nodes As IEnumerable(Of XNode)) As String
        If nodes Is Nothing Then
            Return ""
        End If
        Dim [string] As String
        Using ms = New MemoryStream()
            nodes.WriteToStream(ms)
            [string] = Encoding.UTF8.GetString(ms.ToArray())
        End Using
        Return [string]
    End Function

    '     <System.Runtime.CompilerServices.ExtensionAttribute()>
    '     Public Sub WriteToResponse(nodes As IEnumerable(Of XNode))
    '         Dim ctx As HttpContext = HttpContext.Current
    '         If ctx Is Nothing Then
    '             Throw New Exception("No current HttpContext")
    '         End If
    '         nodes.WriteToStream(ctx.Response.OutputStream)
    '     End Sub

    '     <System.Runtime.CompilerServices.ExtensionAttribute()>
    '     Public Sub WriteToResponse(elements As IEnumerable(Of XElement))
    '         Dim arg_1C_1 As Func(Of XElement, XNode)
    'If(arg_1C_1 = X.CS$<>9__CachedAnonymousMethodDelegate4) Is Nothing Then
    '	arg_1C_1 = (X.CS$<>9__CachedAnonymousMethodDelegate4 = (Function(x As XElement) x))
    '         End If
    '         elements.[Select](arg_1C_1).WriteToResponse()
    '     End Sub

    Public Function IsNullOrWhiteSpace(value As String) As Boolean
        If value IsNot Nothing Then
            For i As Integer = 0 To value.Length - 1
                If Not Char.IsWhiteSpace(value(i)) Then
                    Return False
                End If
            Next
        End If
        Return True
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function IsWhiteSpace(n As XNode) As Boolean
        Dim t As XText = TryCast(n, XText)
        Return t IsNot Nothing AndAlso X.IsNullOrWhiteSpace(t.Value)
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function Trim(nodes As IEnumerable(Of XNode)) As IEnumerable(Of XNode)
        Return nodes.SkipWhile(AddressOf IsWhiteSpace).Reverse().SkipWhile(AddressOf IsWhiteSpace).Reverse()
    End Function

    Public Function SpacesToNbsp(s As String) As String
        If s Is Nothing Then
            Return Nothing
        End If
        Return s.Replace(" ", " ")
    End Function

    Public Function Javascript(content As String) As XElement
        Dim cdata = New XCData("*/" + content + "/*")
        Dim begin = New XText("/*")
        Dim [end] = New XText("*/")
        Return X.E("script", {X.A("type", "text/javascript"), begin, cdata, [end]})
    End Function

    Public Function Javascript(content As XCData) As XElement
        Return X.Javascript(content.Value)
    End Function

    Public Function SelectOption(options As IEnumerable(Of XElement), value As String) As IEnumerable(Of XElement)
        Return options.Select(Function(e As XElement)
                                  Dim valueAtt = e.Attribute("value")
                                  If valueAtt Is Nothing Then
                                      Return e
                                  End If
                                  If valueAtt.Value <> value Then
                                      Return e
                                  End If
                                  Dim expr_31 = New XElement(e)
                                  expr_31.Add(X.A("selected", "selected"))
                                  Return expr_31
                              End Function)
    End Function

    Public Function UnselectOption([option] As XElement) As XElement
        Return [option].RemoveAttr("selected")
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function RemoveChildNodes(element As XElement) As XElement
        Dim expr_06 = New XElement(element)
        expr_06.RemoveNodes()
        Return expr_06
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function RemoveAttr(element As XElement) As XElement
        Dim expr_06 = New XElement(element)
        expr_06.RemoveAttributes()
        Return expr_06
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function RemoveAttr(element As XElement, attribute As String) As XElement
        Dim arg_30_0 = element.RemoveAttr()
        Dim attr = element.Attributes().Where(Function(a As XAttribute) a.Name.LocalName <> attribute).ToArray()
        arg_30_0.Add(attr)
        Return arg_30_0
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function AttributeValue(element As XElement, attr As String) As String
        Dim a As XAttribute = element.Attribute(attr)
        If a Is Nothing Then
            Return Nothing
        End If
        Return a.Value
    End Function

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function Match(Of T)(node As XNode, cdata As Func(Of XCData, T), comment As Func(Of XComment, T), text As Func(Of XText, T), instruction As Func(Of XProcessingInstruction, T), element As Func(Of XElement, T)) As T
        Dim ncdata = TryCast(node, XCData)
        If ncdata IsNot Nothing Then
            Return cdata(ncdata)
        End If
        Dim ncomment = TryCast(node, XComment)
        If ncomment IsNot Nothing Then
            Return comment(ncomment)
        End If
        Dim ntext = TryCast(node, XText)
        If ntext IsNot Nothing Then
            Return text(ntext)
        End If
        Dim ninstruction = TryCast(node, XProcessingInstruction)
        If ninstruction IsNot Nothing Then
            Return instruction(ninstruction)
        End If
        Dim nelement = TryCast(node, XElement)
        If nelement IsNot Nothing Then
            Return element(nelement)
        End If
        Throw New Exception("Unknown node type " + node.[GetType]().ToString())
    End Function
End Module
