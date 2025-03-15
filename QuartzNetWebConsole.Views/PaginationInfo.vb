Public Class PaginationInfo
    Public ReadOnly PageSlide As Integer
    Public ReadOnly PageSize As Integer
    Public ReadOnly TotalItemCount As Integer
    Public ReadOnly PageUrl As String
    Public ReadOnly FirstItemIndex As Integer

    Public Sub New(pageSlide As Integer, pageSize As Integer, totalItemCount As Integer, pageUrl As String, firstItemIndex As Integer)
        Me.PageSlide = pageSlide
        Me.PageSize = pageSize
        Me.TotalItemCount = totalItemCount
        Me.PageUrl = pageUrl
        Me.FirstItemIndex = firstItemIndex
    End Sub

    Public Sub New(pageSize As Integer, totalItemCount As Integer, pageUrl As String, firstItemIndex As Integer)
        Me.new(2, pageSize, totalItemCount, pageUrl, firstItemIndex)
    End Sub

    Public ReadOnly Property CurrentPage As Integer
        Get
            Return FirstItemIndex \ PageSize + 1
        End Get
    End Property

    Public ReadOnly Property LastPage As Integer
        Get
            Return CInt(Math.Floor((CDec(TotalItemCount) - 1) / PageSize)) + 1
        End Get
    End Property

    Public ReadOnly Property HasNextPage As Boolean
        Get
            Return CurrentPage < LastPage
        End Get
    End Property

    Public ReadOnly Property HasPrevPage As Boolean
        Get
            Return CurrentPage > 1
        End Get
    End Property

    Public ReadOnly Property NextPageUrl As String
        Get
            Return If(HasNextPage, PageUrlFor(CurrentPage + 1), Nothing)
        End Get
    End Property

    Public ReadOnly Property PrevPageUrl As String
        Get
            Return If(HasPrevPage, PageUrlFor(CurrentPage - 1), Nothing)
        End Get
    End Property

    Public ReadOnly Property LastItemIndex As Integer
        Get
            Return Math.Min(FirstItemIndex + PageSize - 1, TotalItemCount)
        End Get
    End Property

    Public Function PageUrlFor(page As Integer) As String
        Dim start = (page - 1) * PageSize
        Return PageUrl.Replace("!0", start.ToString())
    End Function

    Public ReadOnly Property Pages As IEnumerable(Of Integer)
        Get
            Dim pageCount = LastPage
            Dim pageFrom = Math.Max(1, CurrentPage - PageSlide)
            Dim pageTo = Math.Min(pageCount, CurrentPage + PageSlide)
            pageFrom = Math.Max(1, Math.Min(pageTo - 2 * PageSlide, pageFrom))
            pageTo = Math.Min(pageCount, Math.Max(pageFrom + 2 * PageSlide, pageTo))
            Return Enumerable.Range(pageFrom, pageTo - pageFrom + 1)
        End Get
    End Property
End Class
