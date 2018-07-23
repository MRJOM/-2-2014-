Imports System.Threading

Public Class WebForm3
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

        Response.Buffer = False
        Response.Write("<table id='waiting' height='50' style='position:absolute;visibility:hidden;'> ")
        Response.Write("<tr><td align=center width=200 style='font-size:9pt; background:#d6d3ce;'> ")
        Response.Write("<b>예문을 추출하고 있습니다. <br>잠시만 기다려 주십시요..</b> ")
        Response.Write("<img src='./image/ajax-loader.gif'>")
        Response.Write("</td></tr></table> ")

        Response.Write("<script language='Javascript'> ")
        Response.Write("waiting.style.visibility='visible' ")
        Response.Write("</script>")
        Response.Flush()

        Response.Write("<script language='Javascript'> ")
        Response.Write("waiting.style.visibility='hidden' ")
        Response.Write("</script>")
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

    Protected Sub inference_get(temp As String)

        Const quote As String = """"

        Dim winhttp As New WinHttp.WinHttpRequest
        Dim Page As Integer
        Dim k(6) As String
        Dim f As Integer = 0

        k(0) = "0"
        k(1) = "1"
        k(2) = "2"
        k(3) = "3"
        k(4) = "11"
        k(5) = "12"
        k(6) = "13"

        Dim tt As String

        For Page = 0 To 6
            winhttp.Open("GET", "http://endic.naver.com/search_example.nhn?sLn=kr&ifAjaxCall=true&query=" + temp + "&fieldType=" + k(Page) + "&txtType=0&langType=0&isTranslatedType=2&degreeType=0&timeType=4:3:2:1&ui=full")
            winhttp.Send()
            Dim Sentence_All_find As String = Replace(winhttp.ResponseText, quote, "")
            Dim so_temp As String = Sentence_All_find
            For i = 1 To 40
                On Error Resume Next
                Dim no_temp As String = Split(Split(so_temp, "<div class=")((i)), "상세</a>")(0)
                Dim Sentence_Eng As String = LCase(Split(Split(no_temp, "type=hidden value=")(1), ">")(0))   ' 영문장
                Dim Word_source As String = Split(Split(no_temp, "source fnt_k10>")(1), "<")(0)           ' 출처
                Dim Sentence_Kor As String = Split(Split(no_temp, "class=N=a:xmp.detail>")(1), "</a>")(0)   ' 번역
                Sentence_Kor = Replace(Replace(Sentence_Kor, "<b>", " "), "</b>", " ")
                Sentence_Kor = Replace((Split(Split(Sentence_Kor, "<")(1), ">")(0)), " ", "")
                If no_temp = "" Then
                    Exit For
                End If

                tt = Replace(Sentence_Eng, temp, "!!CHECK!!")
                If Sentence_Kor <> "" And Sentence_Eng <> "" And InStr(tt, "!!CHECK!!") Then
                    If Word_source = "" Or InStr(Word_source, "명언") Or InStr(Word_source, "YBM") Or InStr(Word_source, "네이버") Then
                        Command = New Odbc.OdbcCommand("insert into example_sentence values('" + temp + "','" + Sentence_Eng + "','" + Sentence_Kor + "','" + Now + "','" + Word_source + "'," + k(Page) + ")", MyConn)
                        Command.ExecuteNonQuery()
                        f = 1
                    End If
                End If
                no_temp = ""
                Word_source = ""
                Sentence_Eng = ""
                Sentence_Kor = ""
            Next
        Next

        If f = 0 Then
            Command = New Odbc.OdbcCommand("insert into example_sentence values('" + temp + "','error404','','" + Now + "','',-1)", MyConn)
            Command.ExecuteNonQuery()
        End If

    End Sub

    Protected Sub inference_out(sentence As String, translation As String, fromWD As String, typet As Integer, selected As String)

        Dim TypeOut(16) As String
        Dim temp(5) As String
        Dim starTemp As String = ""

        Dim C As Integer = 0


        TypeOut(0) = "일반"
        TypeOut(1) = "정치"
        TypeOut(2) = "경제/금융"
        TypeOut(3) = "IT/과학"
        TypeOut(11) = "명언"
        TypeOut(12) = "속담"
        TypeOut(13) = "원서"

        sq5.Items.Clear()
        currect.Text = selected

        typez.Text = TypeOut(typet)

        question.Text = Replace(LCase(sentence), selected, "(  빈칸  )")
        transe.Text = translation
        fromWord.Text = fromWD

        If selectMode.SelectedIndex = 0 Then
            starTemp = ""
            anc.Visible = True
            hint.Visible = True
            For i = 2 To Len(selected)
                starTemp = starTemp + "*"
            Next
            Label4.Text = "정답 글자수 : " + CStr(Len(selected))
            hint.Text = "힌트 : " + Mid(selected, 1, 1) + starTemp
        Else
            anc.Visible = False
            hint.Visible = False
            Label4.Text = "정답"
            Command = New Odbc.OdbcCommand("SELECT * FROM words order by rand() limit 5", MyConn)
            ComReader = Command.ExecuteReader

            While ComReader.Read
                C = C + 1
                temp(C) = ComReader.Item(0)
            End While

            temp(Int((5 * Rnd()) + 1)) = selected

            For i = 1 To 5
                sq5.Items.Add(temp(i))
            Next
        End If

    End Sub

    Protected Sub space_inference()

        Dim Selected As String
        Dim temp As String = ""
        Dim a, b, c, d As String
        Dim cnt As Integer = 0

        MyConn.Open()

        While True
            a = ""
            b = ""
            c = ""
            d = ""
            temp = ""

            Command = New Odbc.OdbcCommand("SELECT word FROM notes where id='" + Session("id") + "' and note_name='" + noteTable.SelectedItem.Text + "' order by rand() limit 1", MyConn)
            Selected = Command.ExecuteScalar

            Command = New Odbc.OdbcCommand("SELECT * FROM example_sentence where word='" + Selected + "'", MyConn)
            ComReader = Command.ExecuteReader

            While ComReader.Read()
                temp = ComReader.Item(1)
            End While

            If temp = "" Then
                Call inference_get(Selected)
            End If

            Command = New Odbc.OdbcCommand("SELECT * FROM example_sentence where word='" + Selected + "'", MyConn)
            ComReader = Command.ExecuteReader

            While ComReader.Read()
                temp = ComReader.Item(1)
            End While

            If temp <> "error404" Then
                Command = New Odbc.OdbcCommand("SELECT * FROM example_sentence where word='" + Selected + "' order by rand() limit 1", MyConn)
                ComReader = Command.ExecuteReader

                While ComReader.Read()
                    a = ComReader.Item(1)
                    b = ComReader.Item(2)
                    c = ComReader.Item(4)
                    d = ComReader.Item(5)
                End While

                inference_out(a, b, c, d, Selected)
                Exit While
            Else
                cnt = cnt + 1
            End If

            If cnt > 5 Then
                Response.Write("<script>alert('존재하는 데이터가 없습니다.')</script>")
                Exit While
            End If
        End While

        MyConn.Close()

    End Sub

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

    Protected Sub start_Click(sender As Object, e As EventArgs) Handles start.Click

        anc.Text = ""

        If selectMode.SelectedIndex = -1 Then
            Response.Write("<script>alert('유형을 선택해 주십세요')</script>")
            Exit Sub
        End If

        If noteTable.SelectedIndex = -1 Then
            Response.Write("<script>alert('단어장을 선택해주세요.\n단어장 목록 새로고침을 누르시면 보입니다.')</script>")
            Exit Sub
        End If

        space_inference()

        question.Visible = True
        transe.Visible = True
        fromWord.Visible = True
        Label4.Visible = True
        sq5.Visible = True
        check.Visible = True
        getCurrect.Visible = True
        label0.Visible = True
        label1.Visible = True
        typez.Visible = True
        label9.Visible = True

    End Sub

    Protected Sub check_Click(sender As Object, e As EventArgs) Handles check.Click

        Dim isright As String = ""

        If selectMode.SelectedIndex = 0 Then
            isright = LCase(anc.Text)
        Else
            isright = sq5.SelectedItem.Text
        End If

        If currect.Text = isright Then
            Response.Write("<script>alert('정답 입니다!\n계속 하시려면 예문 가져오기를 클릭하세요.')</script>")
        Else
            Response.Write("<script>alert('오답 입니다!\n정답을 보고싶으면 정답 단어 보기를 클릭하세요.')</script>")
        End If

    End Sub

    Protected Sub getCurrect_Click(sender As Object, e As EventArgs) Handles getCurrect.Click

        Dim currectWord As String = currect.Text
        Dim message As String = ""

        message = currectWord

        MyConn.Open()

        Command = New Odbc.OdbcCommand("SELECT distinct(word_mean) FROM words where word='" + currectWord + "'", MyConn)
        ComReader = Command.ExecuteReader

        While ComReader.Read
            message = message + "\n" + ComReader.Item(0)
        End While
        Response.Write("<script>alert('" + message + "')</script>")

        MyConn.Close()
    End Sub

End Class