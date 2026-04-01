Public Class Knight
    Inherits Piece

    Public Sub New(row As Integer, col As Integer, color As Boolean)

        Me.row = row
        Me.col = col
        Me.color = color

        If Me.color Then

            Me.val = -30
            Me.img = Images.bKnight

        Else

            Me.val = 30
            Me.img = Images.wKnight

        End If

    End Sub

    Overrides Function generate_moves(board(,) As Piece)

        Dim moves As New List(Of Cell)

        Dim dx() As Integer = {1, 2, 2, 1, -1, -2, -2, -1}
        Dim dy() As Integer = {-2, -1, 1, 2, 2, 1, -1, -2}

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
