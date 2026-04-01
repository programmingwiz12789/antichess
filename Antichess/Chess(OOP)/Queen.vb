Public Class Queen
    Inherits Piece

    Public Sub New(row As Integer, col As Integer, color As Boolean)

        Me.row = row
        Me.col = col
        Me.color = color

        If Me.color Then

            Me.val = -90
            Me.img = Images.bQueen

        Else

            Me.val = 90
            Me.img = Images.wQueen

        End If

    End Sub

    Overrides Function generate_moves(board(,) As Piece)

        Dim moves As New List(Of Cell)

        Dim rook As New Rook(Me.row, Me.col, Me.color)
        Dim bishop As New Bishop(Me.row, Me.col, Me.color)

        For Each c As Cell In rook.generate_moves(board)

            moves.Add(c)

        Next

        For Each c As Cell In bishop.generate_moves(board)

            moves.Add(c)

        Next

        Return moves

    End Function

End Class
