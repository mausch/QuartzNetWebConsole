Imports System.Runtime.CompilerServices

Public Module EnumerableExtensions
    <Extension()>
    Public Function WhereOdd(Of T)(s As IEnumerable(Of T)) As IEnumerable(Of T)
        Return s.Select(Function(e, i) New With {.e = e, .i = i}).
            Where(Function(e) e.i Mod 2 <> 0).
            Select(Function(e) e.e)
    End Function

    <Extension()>
    Public Function WhereEven(Of T)(s As IEnumerable(Of T)) As IEnumerable(Of T)
        Return s.Select(Function(e, i) New With {.e = e, .i = i}).
            Where(Function(e) e.i Mod 2 = 0).
            Select(Function(e) e.e)
    End Function
End Module
