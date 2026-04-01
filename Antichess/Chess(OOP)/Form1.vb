Imports System.Math

Public Class Antichess

    Dim Button(8, 8) As Button
    Dim board(8, 8) As Piece

    Dim over, turn As Boolean

    Dim row1, col1, row2, col2 As Integer

    Dim bestRow1, bestCol1, bestRow2, bestCol2 As Integer

    Function ElemExists(list As List(Of Cell), elem As Cell)

        Dim exists As Boolean = False

        For Each c As Cell In list

            If c.row = elem.row And c.col = elem.col Then

                exists = True

                Exit For

            End If

        Next

        Return exists

    End Function

    Function PossibleMoveExists(turn As Boolean)

        Dim exists As Boolean = False

        For i = 0 To 7

            For j = 0 To 7

                If board(i, j).color = turn Then

                    Dim cnt As Integer = 0

                    For Each c As Cell In board(i, j).generate_moves(board)

                        If MoveValid(i, j, c.row, c.col, turn) Then

                            cnt += 1

                        End If

                    Next

                    If cnt > 0 Then

                        exists = True

                        GoTo out

                    End If

                End If

            Next

        Next

out:

        Return exists

    End Function

    Sub MoveType(piece1 As Piece, ByRef piece2 As Piece, cell As Cell)

        If TypeOf (piece1) Is wPawn Then

            piece2 = New wPawn(cell.row, cell.col)

        ElseIf TypeOf (piece1) Is bPawn Then

            piece2 = New bPawn(cell.row, cell.col)

        ElseIf TypeOf (piece1) Is Rook Then

            piece2 = New Rook(cell.row, cell.col, piece1.color)

        ElseIf TypeOf (piece1) Is Knight Then

            piece2 = New Knight(cell.row, cell.col, piece1.color)

        ElseIf TypeOf (piece1) Is Bishop Then

            piece2 = New Bishop(cell.row, cell.col, piece1.color)

        ElseIf TypeOf (piece1) Is Queen Then

            piece2 = New Queen(cell.row, cell.col, piece1.color)

        ElseIf TypeOf (piece1) Is King Then

            piece2 = New King(cell.row, cell.col, piece1.color)

        ElseIf TypeOf (piece1) Is Null Then

            piece2 = New Null()

        End If

    End Sub

    Sub PieceMove(row1 As Integer, col1 As Integer, row2 As Integer, col2 As Integer)

        If TypeOf (board(row1, col1)) Is wPawn Then

            If row2 = 0 Then

                board(row2, col2) = New Queen(row2, col2, False)

            Else

                MoveType(board(row1, col1), board(row2, col2), New Cell(row2, col2))

            End If

            board(row1, col1) = New Null()

        ElseIf TypeOf (board(row1, col1)) Is bPawn Then

            If row2 = 7 Then

                board(row2, col2) = New Queen(row2, col2, True)

            Else

                MoveType(board(row1, col1), board(row2, col2), New Cell(row2, col2))

            End If

            board(row1, col1) = New Null()

        Else

            MoveType(board(row1, col1), board(row2, col2), New Cell(row2, col2))

            board(row1, col1) = New Null()

        End If

    End Sub

    Sub PieceUndo(temp(,) As Piece)

        For i = 0 To 7

            For j = 0 To 7

                MoveType(temp(i, j), board(i, j), New Cell(i, j))

            Next

        Next

    End Sub

    Function MoveValid(row1 As Integer, col1 As Integer, row2 As Integer, col2 As Integer, color As Boolean)

        Dim valid As Boolean = True

        Dim temp(8, 8) As Piece

        For i = 0 To 7

            For j = 0 To 7

                MoveType(board(i, j), temp(i, j), New Cell(i, j))

            Next

        Next

        PieceMove(row1, col1, row2, col2)

        If board(row1, col1).val <> 0 Then

            valid = False

        End If

        PieceUndo(temp)

        Return valid

    End Function

    Function CanCapture(color As Boolean)

        For i = 0 To 7

            For j = 0 To 7

                If board(i, j).color = color Then

                    For Each c As Cell In board(i, j).generate_moves(board)

                        If TypeOf board(c.row, c.col) IsNot Null Then

                            If board(c.row, c.col).color = Not color Then

                                Return True

                            End If

                        End If

                    Next

                End If

            Next

        Next

        Return False

    End Function

    Function Winner(color As Boolean)

        For i = 0 To 7

            For j = 0 To 7

                If TypeOf board(i, j) IsNot Null Then

                    If board(i, j).color = color Then

                        Return False

                    End If

                End If

            Next

        Next

        Return True

    End Function

    Function SBE(isMax As Boolean)

        If Winner(True) Then

            Return Integer.MaxValue - 10

        ElseIf Winner(False) Then

            Return Integer.MinValue + 10

        ElseIf Not PossibleMoveExists(isMax) Then

            Return 0

        Else

            Dim totEval As Integer = 0

            For i = 0 To 7

                For j = 0 To 7

                    If TypeOf board(i, j) IsNot Null Then

                        totEval += board(i, j).val

                    End If

                Next

            Next

            Return totEval

        End If

    End Function

    Function Minimax(depth As Integer, isMax As Boolean, alpha As Integer, beta As Integer)

        If depth = 1 Or Winner(True) Or Winner(False) Or Not PossibleMoveExists(isMax) Then

            Dim score As Integer = SBE(isMax)

            Return score - depth

        Else

            If isMax Then

                Dim best As Integer = Integer.MinValue + 5

                If CanCapture(isMax) Then

                    For i = 0 To 7

                        For j = 0 To 7

                            If board(i, j).color Then

                                For Each c As Cell In board(i, j).generate_moves(board)

                                    If TypeOf board(c.row, c.col) IsNot Null Then

                                        If MoveValid(i, j, c.row, c.col, isMax) And board(c.row, c.col).color = Not board(i, j).color Then

                                            Dim temp(8, 8) As Piece

                                            For y = 0 To 7

                                                For x = 0 To 7

                                                    MoveType(board(y, x), temp(y, x), New Cell(y, x))

                                                Next

                                            Next

                                            PieceMove(i, j, c.row, c.col)
                                            best = Max(best, Minimax(depth + 1, Not isMax, alpha, beta))
                                            PieceUndo(temp)

                                            alpha = Max(alpha, best)

                                            If alpha >= beta Then

                                                GoTo out1

                                            End If

                                        End If

                                    End If

                                Next

                            End If

                        Next

                    Next

                Else

                    For i = 0 To 7

                        For j = 0 To 7

                            If board(i, j).color Then

                                For Each c As Cell In board(i, j).generate_moves(board)

                                    If MoveValid(i, j, c.row, c.col, isMax) Then

                                        Dim temp(8, 8) As Piece

                                        For y = 0 To 7

                                            For x = 0 To 7

                                                MoveType(board(y, x), temp(y, x), New Cell(y, x))

                                            Next

                                        Next

                                        PieceMove(i, j, c.row, c.col)
                                        best = Max(best, Minimax(depth + 1, Not isMax, alpha, beta))
                                        PieceUndo(temp)

                                        alpha = Max(alpha, best)

                                        If alpha >= beta Then

                                            GoTo out1

                                        End If

                                    End If

                                Next

                            End If

                        Next

                    Next

                End If

out1:           Return best

            Else

                Dim best As Integer = Integer.MaxValue - 5

                If CanCapture(isMax) Then

                    For i = 0 To 7

                        For j = 0 To 7

                            If Not board(i, j).color Then

                                For Each c As Cell In board(i, j).generate_moves(board)

                                    If TypeOf board(c.row, c.col) IsNot Null Then

                                        If MoveValid(i, j, c.row, c.col, isMax) And board(c.row, c.col).color = board(i, j).color Then

                                            Dim temp(8, 8) As Piece

                                            For y = 0 To 7

                                                For x = 0 To 7

                                                    MoveType(board(y, x), temp(y, x), New Cell(y, x))

                                                Next

                                            Next

                                            PieceMove(i, j, c.row, c.col)
                                            best = Min(best, Minimax(depth + 1, Not isMax, alpha, beta))
                                            PieceUndo(temp)

                                            beta = Min(beta, best)

                                            If alpha >= beta Then

                                                GoTo out2

                                            End If

                                        End If

                                    End If

                                Next

                            End If

                        Next

                    Next

                Else

                    For i = 0 To 7

                        For j = 0 To 7

                            If Not board(i, j).color Then

                                For Each c As Cell In board(i, j).generate_moves(board)

                                    If MoveValid(i, j, c.row, c.col, isMax) Then

                                        Dim temp(8, 8) As Piece

                                        For y = 0 To 7

                                            For x = 0 To 7

                                                MoveType(board(y, x), temp(y, x), New Cell(y, x))

                                            Next

                                        Next

                                        PieceMove(i, j, c.row, c.col)
                                        best = Min(best, Minimax(depth + 1, Not isMax, alpha, beta))
                                        PieceUndo(temp)

                                        beta = Min(beta, best)

                                        If alpha >= beta Then

                                            GoTo out2

                                        End If

                                    End If

                                Next

                            End If

                        Next

                    Next

                End If

out2:           Return best

            End If

        End If

    End Function

    Sub FindBestMove()

        Dim bestScore As Integer = Integer.MinValue

        If CanCapture(True) Then

            For i = 0 To 7

                For j = 0 To 7

                    If board(i, j).color Then

                        For Each c As Cell In board(i, j).generate_moves(board)

                            If TypeOf board(c.row, c.col) IsNot Null Then

                                If MoveValid(i, j, c.row, c.col, True) And board(c.row, c.col).color = Not board(i, j).color Then

                                    Dim temp(8, 8) As Piece

                                    For y = 0 To 7

                                        For x = 0 To 7

                                            MoveType(board(y, x), temp(y, x), New Cell(y, x))

                                        Next

                                    Next

                                    PieceMove(i, j, c.row, c.col)
                                    Dim moveScore As Integer = Minimax(0, False, Integer.MinValue + 5, Integer.MaxValue - 5)
                                    PieceUndo(temp)

                                    If moveScore > bestScore Then

                                        bestScore = moveScore

                                        bestRow1 = i
                                        bestCol1 = j

                                        bestRow2 = c.row
                                        bestCol2 = c.col

                                    End If

                                End If

                            End If

                        Next

                    End If

                Next

            Next

        Else

            For i = 0 To 7

                For j = 0 To 7

                    If board(i, j).color Then

                        For Each c As Cell In board(i, j).generate_moves(board)

                            If MoveValid(i, j, c.row, c.col, True) Then

                                Dim temp(8, 8) As Piece

                                For y = 0 To 7

                                    For x = 0 To 7

                                        MoveType(board(y, x), temp(y, x), New Cell(y, x))

                                    Next

                                Next

                                PieceMove(i, j, c.row, c.col)
                                Dim moveScore As Integer = Minimax(0, False, Integer.MinValue + 5, Integer.MaxValue - 5)
                                PieceUndo(temp)

                                If moveScore > bestScore Then

                                    bestScore = moveScore

                                    bestRow1 = i
                                    bestCol1 = j

                                    bestRow2 = c.row
                                    bestCol2 = c.col

                                End If

                            End If

                        Next

                    End If

                Next

            Next

        End If

    End Sub

    Private Sub Chess_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        board(0, 0) = New Rook(0, 0, True)
        board(0, 1) = New Knight(0, 1, True)
        board(0, 2) = New Bishop(0, 2, True)
        board(0, 3) = New Queen(0, 3, True)
        board(0, 4) = New King(0, 4, True)
        board(0, 5) = New Bishop(0, 5, True)
        board(0, 6) = New Knight(0, 6, True)
        board(0, 7) = New Rook(0, 7, True)

        For j = 0 To 7

            board(1, j) = New bPawn(1, j)

        Next

        board(7, 0) = New Rook(7, 0, False)
        board(7, 1) = New Knight(7, 1, False)
        board(7, 2) = New Bishop(7, 2, False)
        board(7, 3) = New Queen(7, 3, False)
        board(7, 4) = New King(7, 4, False)
        board(7, 5) = New Bishop(7, 5, False)
        board(7, 6) = New Knight(7, 6, False)
        board(7, 7) = New Rook(7, 7, False)

        For j = 0 To 7

            board(6, j) = New wPawn(6, j)

        Next

        For i = 2 To 5

            For j = 0 To 7

                board(i, j) = New Null()

            Next

        Next

        Dim cnt As Integer = 1

        For i = 0 To 7

            For j = 0 To 7

                Button(i, j) = DirectCast(Me.Controls("Button" & cnt), Button)

                Button(i, j).ForeColor = Nothing

                If i = 0 Then
                    If j = 0 Or j = 7 Then
                        Button(i, j).Image = Images.bRook
                    ElseIf j = 1 Or j = 6 Then
                        Button(i, j).Image = Images.bKnight
                    ElseIf j = 2 Or j = 5 Then
                        Button(i, j).Image = Images.bBishop
                    ElseIf j = 3 Then
                        Button(i, j).Image = Images.bQueen
                    Else
                        Button(i, j).Image = Images.bKing
                    End If
                ElseIf i = 1 Then
                    Button(i, j).Image = Images.bPawn
                ElseIf i = 6 Then
                    Button(i, j).Image = Images.wPawn
                ElseIf i = 7 Then
                    If j = 0 Or j = 7 Then
                        Button(i, j).Image = Images.wRook
                    ElseIf j = 1 Or j = 6 Then
                        Button(i, j).Image = Images.wKnight
                    ElseIf j = 2 Or j = 5 Then
                        Button(i, j).Image = Images.wBishop
                    ElseIf j = 3 Then
                        Button(i, j).Image = Images.wQueen
                    Else
                        Button(i, j).Image = Images.wKing
                    End If
                Else
                    Button(i, j).Image = Nothing
                End If

                cnt += 1

            Next

        Next

        over = False

        row1 = -1
        col1 = -1

        row2 = -1
        col2 = -1

        turn = False

    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick

        bestRow1 = -1
        bestCol1 = -1
        bestRow2 = -1
        bestCol2 = -1

        Me.Text = "Thinking..."

        FindBestMove()

        Me.Text = "Chess"

        PieceMove(bestRow1, bestCol1, bestRow2, bestCol2)

        turn = False

        For k = 1 To 64

            Dim b As Button = DirectCast(Me.Controls("Button" & k), Button)
            Dim row As Integer = (k - 1) \ 8
            Dim col As Integer = (k - 1) Mod 8

            b.Image = board(row, col).img
            b.ForeColor = Nothing

            If row = bestRow1 And col = bestCol1 Then

                b.ForeColor = Color.Blue

            End If

            If row = bestRow2 And col = bestCol2 Then

                b.ForeColor = Color.Blue

            End If

        Next

        Timer1.Enabled = False

        If Winner(True) Then

            over = True

            MsgBox("Black wins!")

        ElseIf Winner(False) Then

            over = True

            MsgBox("White wins!")

        ElseIf PossibleMoveExists(turn) = False Then

            over = True

            MsgBox("Draw!")

        End If

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click, Button8.Click, Button7.Click, Button64.Click, Button63.Click, Button62.Click, Button61.Click, Button60.Click, Button6.Click, Button59.Click, Button58.Click, Button57.Click, Button56.Click, Button55.Click, Button54.Click, Button53.Click, Button52.Click, Button51.Click, Button50.Click, Button5.Click, Button49.Click, Button48.Click, Button47.Click, Button46.Click, Button45.Click, Button44.Click, Button43.Click, Button42.Click, Button41.Click, Button40.Click, Button4.Click, Button39.Click, Button38.Click, Button37.Click, Button36.Click, Button35.Click, Button34.Click, Button33.Click, Button32.Click, Button31.Click, Button30.Click, Button3.Click, Button29.Click, Button28.Click, Button27.Click, Button26.Click, Button25.Click, Button24.Click, Button23.Click, Button22.Click, Button21.Click, Button20.Click, Button2.Click, Button19.Click, Button18.Click, Button17.Click, Button16.Click, Button15.Click, Button14.Click, Button13.Click, Button12.Click, Button11.Click, Button10.Click, Button1.Click

        If over = False Then

            For i = 0 To 7

                For j = 0 To 7

                    If Button(i, j) Is sender Then

                        If row1 = -1 And col1 = -1 Then

                            row1 = i
                            col1 = j

                        ElseIf row2 = -1 And col2 = -1 Then

                            row2 = i
                            col2 = j

                        End If

                        GoTo out

                    End If

                Next

            Next

out:

            If Not board(row1, col1).color Then

                If row1 <> -1 And col1 <> -1 And row2 <> -1 And col2 <> -1 Then

                    If Not Winner(True) And Not Winner(False) And PossibleMoveExists(turn) Then

                        If ElemExists(board(row1, col1).generate_moves(board), New Cell(row2, col2)) Then

                            If CanCapture(False) Then

                                If TypeOf board(row2, col2) IsNot Null Then

                                    If MoveValid(row1, col1, row2, col2, False) And board(row2, col2).color = Not board(row1, col1).color Then

                                        PieceMove(row1, col1, row2, col2)

                                        turn = True

                                        For k = 1 To 64

                                            Dim b As Button = DirectCast(Me.Controls("Button" & k), Button)
                                            Dim row As Integer = (k - 1) \ 8
                                            Dim col As Integer = (k - 1) Mod 8

                                            b.Image = board(row, col).img

                                        Next

                                        If Not Winner(True) And Not Winner(False) And PossibleMoveExists(turn) Then

                                            Timer1.Enabled = True

                                        End If

                                        row1 = -1
                                        col1 = -1

                                    Else

                                        If Not board(row2, col2).color Then

                                            row1 = row2
                                            col1 = col2

                                        Else

                                            row1 = -1
                                            col1 = -1

                                        End If

                                    End If

                                End If

                            Else

                                If MoveValid(row1, col1, row2, col2, False) Then

                                    PieceMove(row1, col1, row2, col2)

                                    turn = True

                                    For k = 1 To 64

                                        Dim b As Button = DirectCast(Me.Controls("Button" & k), Button)
                                        Dim row As Integer = (k - 1) \ 8
                                        Dim col As Integer = (k - 1) Mod 8

                                        b.Image = board(row, col).img

                                    Next

                                    If Not Winner(True) And Not Winner(False) And PossibleMoveExists(turn) Then

                                        Timer1.Enabled = True

                                    End If

                                    row1 = -1
                                    col1 = -1

                                Else

                                    If Not board(row2, col2).color Then

                                        row1 = row2
                                        col1 = col2

                                    Else

                                        row1 = -1
                                        col1 = -1

                                    End If

                                End If

                            End If

                        Else

                            If Not board(row2, col2).color Then

                                row1 = row2
                                col1 = col2

                            Else

                                row1 = -1
                                col1 = -1

                            End If

                        End If

                        row2 = -1
                        col2 = -1

                    End If

                    If Winner(True) Then

                        over = True

                        MsgBox("Black wins!")

                    ElseIf Winner(False) Then

                        over = True

                        MsgBox("White wins!")

                    ElseIf PossibleMoveExists(turn) = False Then

                        over = True

                        MsgBox("Draw!")

                    End If

                End If

            Else

                row1 = -1
                col1 = -1

                row2 = -1
                col2 = -1

            End If

        End If

    End Sub

    Private Sub RestartBtn_Click(sender As System.Object, e As System.EventArgs) Handles RestartBtn.Click

        Chess_Load(sender, e)

    End Sub

End Class
