Public Class WebForm1

    Inherits System.Web.UI.Page

    Protected StrConn As String = "DRIVER={MySQL ODBC 5.1 Driver};" + "Server=telsam.codns.com;" + "port=3306;" + "option=16384;" + "database=Asp_Project;" + "uid=root;" + "password=tkdlaekd0;"    'SQL Connect
    Protected MyConn As New Odbc.OdbcConnection(StrConn)
    Protected Command As New Odbc.OdbcCommand()
    Protected ComReader As Odbc.OdbcDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("id") = "" Then
            Response.Redirect("login.aspx")
        End If
        Call SessionCheck()
    End Sub

    Protected Sub SessionCheck()    '섹션 입력 해킹 방지 코드

        MyConn.Open()

        If Session("id") <> "" Then
            If Session("UserProtect") <> MD5CalcString(Session("id") + "UserProtect_AspProject") Then
                Command = New Odbc.OdbcCommand("insert into error_log values('" + Session("id") + "','" + Now + "','" + Request.UserHostAddress + "','Session Hacking')", MyConn)   '로그인 로그
                Command.ExecuteNonQuery()
                Session("id") = ""
                Session("UserProtect") = ""
            End If
        End If

        MyConn.Close()

    End Sub
    Protected Function MD5CalcString(ByVal strData As String) As String 'MD5

        Dim objMD5 As New System.Security.Cryptography.MD5CryptoServiceProvider

        Dim arrData() As Byte

        Dim arrHash() As Byte

        arrData = System.Text.Encoding.ASCII.GetBytes(strData)
        arrHash = objMD5.ComputeHash(arrData)

        objMD5 = Nothing

        Return ByteArrayToString(arrHash)
    End Function
    Protected Function ByteArrayToString(ByVal arrInput() As Byte) As String    'MD5

        Dim strOutput As New System.Text.StringBuilder(arrInput.Length)

        For i As Integer = 0 To arrInput.Length - 1

            strOutput.Append(arrInput(i).ToString("X2"))

        Next

        Return strOutput.ToString().ToLower
    End Function

    Protected Sub listRefresh()

        MyConn.Open()

        noteTable.Items.Clear()

        Dim id As String = Session("id")

        Command = New Odbc.OdbcCommand("SELECT distinct(note_name) FROM notes where id='" + id + "' and word!='MasterNT'", MyConn)
        ComReader = Command.ExecuteReader

        While ComReader.Read
            noteTable.Items.Add(ComReader.Item(0))
        End While

        MyConn.Close()

    End Sub

    Protected Sub getlist_Click(sender As Object, e As EventArgs) Handles getlist.Click
        listRefresh()
    End Sub

    Protected Function get_kor_means(word As String) As String

        get_kor_means = ""

        Command = New Odbc.OdbcCommand("SELECT distinct(word_mean) FROM words where word='" + word + "'and word_mean!='error404' order by rand() limit 3", MyConn)
        ComReader = Command.ExecuteReader

        While ComReader.Read
            get_kor_means = get_kor_means + ComReader.Item(0) + " ,"
        End While

        If Len(get_kor_means) = 0 Then
            Exit Function
        End If

        get_kor_means = Mid(get_kor_means, 1, Len(get_kor_means) - 1)


    End Function

    Protected Sub multiple_choice_kor() '객관식    한->영

        MyConn.Open()

        Dim selected_word As String = ""
        Dim question_str As String = ""
        Dim Temp(5) As String
        Dim TempMeans As String = ""

        Dim c As Integer = 0

        Command = New Odbc.OdbcCommand("SELECT distinct(word) FROM notes where id='" + Session("id") + "' and note_name='" + noteTable.SelectedItem.Text + "' and word!='MasterNT' order by rand() limit 1", MyConn)
        selected_word = Command.ExecuteScalar
        anc.Visible = False

        sq5.Items.Clear()

        Command = New Odbc.OdbcCommand("SELECT distinct(word) FROM words where word!='selected_word' and word_mean!='error404' and word_mean not like '-1::%' order by rand() limit 5", MyConn)
        ComReader = Command.ExecuteReader

        While ComReader.Read
            c = c + 1
            Temp(c) = ComReader.Item(0)
        End While

        question.Text = get_kor_means(selected_word)
        currect.Text = selected_word
        Temp(Int((5 * Rnd()) + 1)) = selected_word

        For i = 1 To 5
            sq5.Items.Add(Temp(i))
        Next

        MyConn.Close()

    End Sub

    Protected Sub multiple_choice_eng() '객관식    영->한

        MyConn.Open()

        Dim selected_word As String = ""
        Dim question_str As String = ""
        Dim Temp(5) As String
        Dim TempMeans As String = ""

        Dim c As Integer = 0

        Command = New Odbc.OdbcCommand("SELECT distinct(word) FROM notes where id='" + Session("id") + "' and note_name='" + noteTable.SelectedItem.Text + "' and word!='MasterNT' order by rand() limit 1", MyConn)
        selected_word = Command.ExecuteScalar
        anc.Visible = False

        sq5.Items.Clear()

        Command = New Odbc.OdbcCommand("SELECT distinct(word) FROM words where word!='selected_word' and word_mean!='error404'and word_mean not like '-1::%' order by rand() limit 5", MyConn)
        ComReader = Command.ExecuteReader

        While ComReader.Read
            c = c + 1
            Temp(c) = ComReader.Item(0)
        End While

        For i = 1 To 5
            Temp(i) = get_kor_means(Temp(i))
        Next

        TempMeans = get_kor_means(selected_word)

        question.Text = selected_word
        currect.Text = TempMeans
        Temp(Int((5 * Rnd()) + 1)) = TempMeans

        For i = 1 To 5
            sq5.Items.Add(Temp(i))
        Next

        MyConn.Close()

    End Sub

    Protected Sub essay_question()  '주관식

        MyConn.Open()

        Dim selected_word As String
        Dim question_str As String = ""

        Command = New Odbc.OdbcCommand("SELECT distinct(word) FROM notes where id='" + Session("id") + "' and note_name='" + noteTable.SelectedItem.Text + "'and word!='MasterNT' order by rand() limit 1", MyConn)
        selected_word = Command.ExecuteScalar
        anc.Visible = True
        sq5.Items.Clear()

        question_str = get_kor_means(selected_word)
        question.Text = question_str
        currect.Text = selected_word
        MyConn.Close()

    End Sub

    Protected Function check_note() As Long
        check_note = 0

        If noteTable.SelectedIndex = -1 Then
            Exit Function
        End If
        MyConn.Open()

        Command = New Odbc.OdbcCommand("SELECT count(id) FROM notes where id='" + Session("id") + "' and note_name='" + noteTable.SelectedItem.Text + "' and word!='MasterNT'", MyConn)
        check_note = Command.ExecuteScalar

        MyConn.Close()

    End Function


    Protected Sub start_Click(sender As Object, e As EventArgs) Handles start.Click

        If check_note() = 0 Then
            Response.Write("<script>alert('단어장에 단어가 존제하지 않거나 단어장을 선택하지 않았습니다.')</script>")
            Exit Sub
        End If

        check.Visible = True
        getCurrect.Visible = True
        question.Visible = True

        If qu.SelectedIndex = -1 Then
            Response.Write("<script>alert('유형을 선택해 주십시오')</script>")
            Exit Sub
        End If
        If noteTable.SelectedIndex = -1 Then
            Response.Write("<script>alert('단어장을 선택해주세요.\n단어장 목록 새로고침을 누르시면 보입니다.')</script>")
            Exit Sub
        End If

        If qu.SelectedIndex = 0 Then
            Call essay_question()
        ElseIf qu.SelectedIndex = 1 Then
            Call multiple_choice_eng()
        ElseIf qu.SelectedIndex = 2 Then
            Call multiple_choice_kor()
        End If

    End Sub

    Protected Sub getCurrect_Click(sender As Object, e As EventArgs) Handles getCurrect.Click

        Dim currectWord As String = ""
        Dim message As String = ""

        If qu.SelectedIndex = 0 Then
            currectWord = currect.Text
        ElseIf qu.SelectedIndex = 1 Then
            currectWord = question.Text
        ElseIf qu.SelectedIndex = 2 Then
            currectWord = currect.Text
        End If

        message = currectWord

        MyConn.Open()

        Command = New Odbc.OdbcCommand("SELECT distinct(word_mean) FROM words where word='" + currectWord + "' and word_mean!='error404'", MyConn)
        ComReader = Command.ExecuteReader

        While ComReader.Read
            message = message + "\n" + ComReader.Item(0)
        End While
        Response.Write("<script>alert('" + message + "')</script>")

        MyConn.Close()

    End Sub

    Protected Function check_essay() As Boolean

        Dim currectWord As String = ""
        Dim answerA As String = ""

        currectWord = currect.Text
        answerA = anc.Text

        If currect.Text = LCase(anc.Text) Then



            Response.Write("<script>alert('맞았습니다.')</script>")

            Command = New Odbc.OdbcCommand("insert into stats values('" + Session("id") + "','" + currectWord + "','" + currectWord + "','" + answerA + "','o','" + Now + "','" + CStr(qu.SelectedIndex) + "')", MyConn)
            Command.ExecuteNonQuery()

            anc.Text = ""
            MyConn.Close()
            check_essay = True

            If qu.SelectedIndex = 0 Then
                Call essay_question()
            ElseIf qu.SelectedIndex = 1 Then
                Call multiple_choice_eng()
            ElseIf qu.SelectedIndex = 2 Then
                Call multiple_choice_kor()
            End If


        Else
            Response.Write("<script>alert('오답입니다.')</script>")
            Command = New Odbc.OdbcCommand("insert into stats values('" + Session("id") + "','" + currectWord + "','" + currectWord + "','" + answerA + "','x','" + Now + "','" + CStr(qu.SelectedIndex) + "')", MyConn)
            Command.ExecuteNonQuery()
            check_essay = False
            MyConn.Close()
        End If

    End Function

    Protected Function check_multiful() As Boolean

        Dim orginal As String = ""
        Dim currectWord As String = currect.Text
        Dim answerA As String = sq5.SelectedItem.Text

        If qu.SelectedIndex = 1 Then
            orginal = question.Text
        Else
            orginal = currect.Text
        End If

        If currect.Text = sq5.SelectedItem.Text Then

            Response.Write("<script>alert('맞았습니다.')</script>")

            Command = New Odbc.OdbcCommand("insert into stats values('" + Session("id") + "','" + orginal + "','" + currectWord + "','" + answerA + "','o','" + Now + "','" + CStr(qu.SelectedIndex) + "')", MyConn)
            Command.ExecuteNonQuery()

            anc.Text = ""
            MyConn.Close()
            check_multiful = True

            If qu.SelectedIndex = 0 Then
                Call essay_question()
            ElseIf qu.SelectedIndex = 1 Then
                Call multiple_choice_eng()
            ElseIf qu.SelectedIndex = 2 Then
                Call multiple_choice_kor()
            End If

        Else
            Response.Write("<script>alert('오답입니다.')</script>")
            Command = New Odbc.OdbcCommand("insert into stats values('" + Session("id") + "','" + orginal + "','" + currectWord + "','" + answerA + "','x','" + Now + "','" + CStr(qu.SelectedIndex) + "')", MyConn)
            Command.ExecuteNonQuery()
            check_multiful = False
            MyConn.Close()
        End If

    End Function

    Protected Sub check_Click(sender As Object, e As EventArgs) Handles check.Click

        If check_note() = 0 Then
            Response.Write("<script>alert('단어장에 단어가 존재하지 않거나 단어장을 선택하지 않았습니다.\n단어장 목록 새로고침을 누르시면 보입니다.')</script>")
            Exit Sub
        End If

        MyConn.Open()

        If qu.SelectedIndex = 0 Then
            If check_essay() Then
                Call essay_question()
            End If
        ElseIf qu.SelectedIndex = 1 Then
            If check_multiful() Then
                Call multiple_choice_eng()
            End If
        ElseIf qu.SelectedIndex = 2 Then
            If check_multiful() Then
                Call multiple_choice_kor()
            End If
        End If

        MyConn.Close()

    End Sub

End Class
