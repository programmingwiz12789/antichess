Public Class King
    Inherits Piece

    Public Sub New(row As Integer, col As Integer, color As Boolean)

        Me.row = row
        Me.col = col
        Me.color = color

        If Me.color Then

            Me.val = -900
            Me.img = Images.bKing

        Else

            Me.val = 900
            Me.img = Images.wKing

        End If

    End Sub

    Overrides Function generate_moves(board(,) As Piece)

        Dim moves As New List(Of Cell)

        Dim dx() As Integer = {1, 1, 1, 0, -1, -1, -1, 0}
        Dim dy() As Integer = {-1, 0, 1, 1, 1, 0, -1, -1}

        Dim move As Integer = 0

        Do While move < 8

            If Me.row + dy(move) >= 0 And Me.row + dy(move) <= 7 And Me.col + dx(move) >= 0 And Me.col + dx(move) <= 7 Then

                If TypeOf (board(Me.row + dy(move), Me.col + dx(move))) Is Null Or board(Me.row + dy(move), Me.col + dx(move)).color = Not Me.color Then

                    moves.Add(New Cell(Me.row + dy(move), Me.col + dx(move)))

                End If

            End If

            move += 1

        Loop

        Return moves

    End Function

End Class
