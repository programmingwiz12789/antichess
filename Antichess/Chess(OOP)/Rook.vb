Public Class Rook
    Inherits Piece

    Public Sub New(row As Integer, col As Integer, color As Boolean)

        Me.row = row
        Me.col = col
        Me.color = color

        If Me.color Then

            Me.val = -50
            Me.img = Images.bRook

        Else

            Me.val = 50
            Me.img = Images.wRook

        End If

    End Sub

    Overrides Function generate_moves(board(,) As Piece)

        Dim moves As New List(Of Cell)

        For i = Me.row - 1 To 0 Step -1

            If TypeOf (board(i, Me.col)) Is Null Then

                moves.Add(New Cell(i, Me.col))

            Else

                If board(i, Me.col).color = Not Me.color Then

                    moves.Add(New Cell(i, Me.col))

                End If

                Exit For

            End If

        Next

        For j = Me.col + 1 To 7

            If TypeOf (board(Me.row, j)) Is Null Then

                moves.Add(New Cell(Me.row, j))

            Else

                If board(Me.row, j).color = Not Me.color Then

                    moves.Add(New Cell(Me.row, j))

                End If

                Exit For

            End If

        Next

        For i = Me.row + 1 To 7

            If TypeOf (board(i, Me.col)) Is Null Then

                moves.Add(New Cell(i, Me.col))

            Else

                If board(i, Me.col).color = Not Me.color Then

                    moves.Add(New Cell(i, Me.col))

                End If

                Exit For

            End If

        Next

        For j = Me.col - 1 To 0 Step -1

            If TypeOf (board(Me.row, j)) Is Null Then

                moves.Add(New Cell(Me.row, j))

            Else

                If board(Me.row, j).color = Not Me.color Then

                    moves.Add(New Cell(Me.row, j))

                End If

                Exit For

            End If

        Next

        Return moves

    End Function

End Class
