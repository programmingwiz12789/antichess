Public MustInherit Class Piece

    Public row, col, val As Integer
    Public eval(8, 8) As Integer
    Public color As Boolean
    Public img As Image

    Public Overridable Function generate_moves(board(,) As Piece)
        Dim list As New List(Of Cell)
        Return list
    End Function

    Protected Function reverse(arr2D(,) As Integer)
        Dim rowCnt As Integer = arr2D.GetLength(0)
        Dim colCnt As Integer = arr2D.GetLength(1)
        Dim reversedArr2D(rowCnt, colCnt) As Integer
        For i = 0 To rowCnt - 1
            For j = 0 To colCnt - 1
                reversedArr2D(i, j) = arr2D(rowCnt - 1 - i, j)
            Next
        Next
        Return reversedArr2D
    End Function

End Class
