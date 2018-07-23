Imports System.Web.SessionState
Imports System.Text
Imports System.Threading

Public Class plusword
    Inherits System.Web.UI.Page

    Protected StrConn As String = "DRIVER={MySQL ODBC 5.1 Driver};" + "Server=telsam.codns.com;" + "port=3306;" + "option=16384;" + "database=Asp_Project;" + "uid=root;" + "password=tkdlaekd0;"    'SQL Connect
    Protected MyConn As New Odbc.OdbcConnection(StrConn)
    Protected Command As New Odbc.OdbcCommand()
    Protected ComReader As Odbc.OdbcDataReader

    Protected Orgin_data(5000) As String

    Protected winhttp As New WinHttp.WinHttpRequest '웹파싱 변수
    Protected DoubleColon As String = Chr(34) '파싱에 사용할 더블콜론(")
    Protected List_count As Long


    Protected Sub Extract_Click(sender As Object, e As EventArgs) Handles Extract.Click

        insert.Visible = True
        MyConn.Open()

        Call Convert_Orgin_data()   '원문내 단어 추출

        cb1.Items.Clear()
        For i = 1 To List_count     '검색
            Call Search(Orgin_data(i))
        Next

        For i = 0 To cb1.Items.Count - 1
            cb1.Items(i).Selected = True
        Next

        MyConn.Close()

    End Sub

    Protected Sub Search(data As String)

        Dim response As String = ""
        Dim responseTemp As String = ""
        Dim isDoExtract As Boolean = False
        Dim orginalWord As String = ""
        Dim check_data As String = ""

        check_data = data_exist(data)
        On Error Resume Next

        If isBenList(data) = True Then
            Exit Sub
        End If

        If check_data <> "" Then    '데이터 존재

            If check_data = "error404" Then '노데이터
            ElseIf InStr(check_data, "-1::") Then
                If isBenList(Split(check_data, "-1::")(1)) Then
                    Exit Sub
                End If
                cb1.Items.Add(Split(check_data, "-1::")(1)) '원형
            Else
                cb1.Items.Add(data)         '데이터 존재
            End If
        Else                        '데이터 없음
            winhttp.Open("GET", "http://m.endic.naver.com/search.nhn?query=" + data + "&searchOption=entryIdiom&preQuery=&forceRedirect=")
            winhttp.Send()
            responseTemp = winhttp.ResponseText

            If InStr(responseTemp, "검색결과가 없습니다") Then
                Command = New Odbc.OdbcCommand("insert into words values('" + LCase(data) + "','error404','" + Now + "','http://endic.naver.com/')", MyConn)
                Command.ExecuteNonQuery()
            Else
                '검색
                winhttp.Open("GET", "http://ac.endic.naver.com/ac?_callback=window.__jindo2_callback._3048&q=" + data + "&q_enc=utf-8&st=1100&r_format=json&r_enc=utf-8&r_lt=1000&r_unicode=0&r_escape=1")
                winhttp.Send()

                response = tag_clear(winhttp.ResponseText)  'html 태그 제거

                isDoExtract = Extracter(data, response)

                If InStr(responseTemp, "기본형인") Then '원형이 존재함
                    orginalWord = Split(Split(responseTemp, "<span>기본형인</span> '")(1), "'")(0)
                    Call orginal_word_serch(orginalWord)

                    If isDoExtract = False Then '단어의 의미가 존재하지 않고 원형만 존재
                        If isBenList(orginalWord) Then
                            Exit Sub
                        End If
                        Command = New Odbc.OdbcCommand("insert into words values('" + LCase(data) + "','-1::" + orginalWord + "','" + Now + "','http://endic.naver.com/')", MyConn)
                        Command.ExecuteNonQuery()
                    End If
                Else
                    If isDoExtract = False Then
                        Command = New Odbc.OdbcCommand("insert into words values('" + LCase(data) + "','error404','" + Now + "','http://endic.naver.com/')", MyConn)
                        Command.ExecuteNonQuery()
                    End If
                End If

                If isDoExtract = True Then  '단어가 존재함
                    cb1.Items.Add(data)
                End If
            End If
        End If

    End Sub

    '원형 추가
    Protected Sub orginal_word_serch(data As String)

        Dim response As String = ""
        Dim isDoExtract As Boolean = False
        Dim orginalWord As String = ""
        On Error Resume Next

        winhttp.Open("GET", "http://ac.endic.naver.com/ac?_callback=window.__jindo2_callback._3048&q=" + data + "&q_enc=utf-8&st=1100&r_format=json&r_enc=utf-8&r_lt=1000&r_unicode=0&r_escape=1")
        winhttp.Send()

        response = tag_clear(winhttp.ResponseText)  'html 태그 제거

        isDoExtract = Extracter(data, response)

        If isDoExtract = False Then
            Command = New Odbc.OdbcCommand("insert into words values('" + LCase(data) + "','error404','" + Now + "','http://endic.naver.com/')", MyConn)
            Command.ExecuteNonQuery()
        End If

    End Sub
    '파싱 및 데이터 추가
    Protected Function Extracter(data As String, x As String) As Boolean

        Dim temp As String = x
        Dim sptemp As String = ""
        Dim extemp As String = ""
        Extracter = False

        For i = 2 To UBound(Split(temp, "[" + DoubleColon)) Step 2

            sptemp = Split(Split(temp, "[" + DoubleColon)(i), DoubleColon)(0)
            If LCase(sptemp) = LCase(data) Then

                For j = 0 To UBound(Split(Split(Split(temp, "[" + DoubleColon)(i + 1), DoubleColon)(0), ";"))
                    extemp = Replace(Split(Split(Split(Split(temp, "[" + DoubleColon)(i + 1), DoubleColon)(0), ";")(j), ";")(0), "b", "")
                    If extemp <> "" Then
                        Command = New Odbc.OdbcCommand("insert into words values('" + LCase(data) + "','" + extemp + "','" + Now + "','http://endic.naver.com/')", MyConn)
                        Command.ExecuteNonQuery()
                        Extracter = True
                    End If
                Next
            End If
        Next
    End Function

    '데이터 존재 유무 확인
    Protected Function data_exist(x As String) As String

        data_exist = ""
        Command = New Odbc.OdbcCommand("SELECT * FROM words where word='" + x + "'", MyConn) '아이디 등록
        ComReader = Command.ExecuteReader

        While ComReader.Read
            data_exist = ComReader.Item(1)
            Exit While
        End While

    End Function
    'html태그 제거
    Protected Function tag_clear(x As String) As String

        tag_clear = x
        tag_clear = Replace(tag_clear, "u003e", "")
        tag_clear = Replace(tag_clear, "u003cb", "")
        tag_clear = Replace(tag_clear, "u003e", "")
        tag_clear = Replace(tag_clear, "u003c", "")
        tag_clear = Replace(tag_clear, "u003c", "")
        tag_clear = Replace(tag_clear, "\", "")
        tag_clear = Replace(tag_clear, "/", "")
        tag_clear = Replace(tag_clear, ",", ";")
        tag_clear = Replace(tag_clear, "'", "")

    End Function


    ' 문장 추가용 
    Protected Function incoding_sentence(ByVal instack As String)
        incoding_sentence = ""
        Dim temp As String = instack

        Temp = Replace(Temp, "[", " ")
        Temp = Replace(Temp, ":", " ")
        Temp = Replace(Temp, ";", " ")
        Temp = Replace(Temp, ",", " ")
        Temp = Replace(Temp, "\", " ")
        Temp = Replace(Temp, "]", " ")
        Temp = Replace(Temp, "^", " ")
        Temp = Replace(Temp, "_", " ")
        Temp = Replace(Temp, "`", " ")
        Temp = Replace(Temp, "'", " ")
        Temp = Replace(Temp, "!", " ")
        Temp = Replace(Temp, ".", " ")
        Temp = Replace(Temp, "@", " ")
        Temp = Replace(Temp, "#", " ")
        Temp = Replace(Temp, "$", " ")
        Temp = Replace(Temp, "%", " ")
        Temp = Replace(Temp, "(", " ")
        Temp = Replace(Temp, ")", " ")
        Temp = Replace(Temp, "-", " ")
        Temp = Replace(Temp, "?", " ")
        Temp = Replace(Temp, "’", " ")
        Temp = Replace(Temp, "‘", " ")
        Temp = Replace(Temp, DoubleColon, " ")
        Temp = Replace(Temp, Chr(13), " ")
        Temp = Replace(Temp, "" & vbLf & "", " ")
        For i = 1 To Len(temp)
            If Mid(temp, i, 1) Like "*[A-z]*" Or Mid(temp, i, 1) = "_" Or Mid(temp, i, 1) Like "*[0-9]*" Or Mid(temp, i, 1) = " " Then
                incoding_sentence = incoding_sentence + Mid(temp, i, 1)
            End If
        Next
    End Function

    ' 영문추출 특수문자 제거
    Protected Function incoding_Kor_Eng(ByVal instack As String)
        incoding_Kor_Eng = ""
        For i = 1 To Len(instack)
            If Mid(instack, i, 1) Like "*[A-z]*" Or Mid(instack, i, 1) = "_" Or Mid(instack, i, 1) Like "*[0-9]*" Then
                incoding_Kor_Eng = incoding_Kor_Eng + Mid(instack, i, 1)
            End If
        Next
    End Function
    '원본 문장 분해
    Protected Sub Convert_Orgin_data()

        Dim i As Long   '반복문
        Dim j As Long   '반복문
        Dim Temp As String  '임시변수
        Dim Flag As Integer '반복단어 제거용 플래그

        Dim CnvTemp As String 'Convert Temp

        Temp = input_box.Text

        '문장부호 띄어쓰기로 교체
        Temp = Replace(Temp, "[", " ")
        Temp = Replace(Temp, ":", " ")
        Temp = Replace(Temp, ";", " ")
        Temp = Replace(Temp, ",", " ")
        Temp = Replace(Temp, "\", " ")
        Temp = Replace(Temp, "]", " ")
        Temp = Replace(Temp, "^", " ")
        Temp = Replace(Temp, "_", " ")
        Temp = Replace(Temp, "`", " ")
        Temp = Replace(Temp, "'", " ")
        Temp = Replace(Temp, "!", " ")
        Temp = Replace(Temp, ".", " ")
        Temp = Replace(Temp, "@", " ")
        Temp = Replace(Temp, "#", " ")
        Temp = Replace(Temp, "$", " ")
        Temp = Replace(Temp, "%", " ")
        Temp = Replace(Temp, "(", " ")
        Temp = Replace(Temp, ")", " ")
        Temp = Replace(Temp, "-", " ")
        Temp = Replace(Temp, "?", " ")
        Temp = Replace(Temp, "’", " ")
        Temp = Replace(Temp, "‘", " ")
        Temp = Replace(Temp, DoubleColon, " ")
        Temp = Replace(Temp, Chr(13), " ")
        Temp = Replace(Temp, "" & vbLf & "", " ")

        '교체 끝

        List_count = cb1.Items.Count

        For i = 0 To cb1.Items.Count - 1    '중복데이터 리스트 추가 하지 않기.
            Orgin_data(i + 1) = cb1.Items(i).ToString
        Next
        cb1.Items.Clear()

        CnvTemp = ""

        For i = 0 To UBound(Split(Temp, " "))

            Flag = 0
            CnvTemp = LCase(Split(Split(Temp, " ")(i), " ")(0))

            If CnvTemp <> "" Then

                For j = 1 To List_count     '중복 체크 플레그 활용
                    If Orgin_data(j) = CnvTemp Then
                        Flag = 1
                        Exit For
                    End If
                Next

                If Flag = 0 Then
                    List_count = List_count + 1
                    Orgin_data(List_count) = incoding_Kor_Eng(CnvTemp)
                End If
            End If
        Next i

    End Sub

    '섹션 해킹 방지
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

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load               

        If Session("id") = "" Then
            Response.Redirect("login.aspx")
        End If

        Call SessionCheck()


        Response.Buffer = False
        Response.Write("<table id='waiting' height='50' style='position:absolute;visibility:hidden;'> ")
        Response.Write("<tr><td align=center width=200 style='font-size:9pt; background:#d6d3ce;'> ")
        Response.Write("<b>실행 중 입니다. <br>잠시만 기다려 주십시요..</b> ")
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

    Protected Sub listRefresh()
        MyConn.Open()

        dblist.Items.Clear()

        Dim id As String = Session("id")

        Command = New Odbc.OdbcCommand("SELECT distinct(note_name) FROM notes where id='" + id + "'", MyConn)
        ComReader = Command.ExecuteReader

        While ComReader.Read
            dblist.Items.Add(ComReader.Item(0))
        End While

        MyConn.Close()

    End Sub

    Protected Sub refresh_Click(sender As Object, e As EventArgs) Handles refresh.Click

        Call listRefresh()

    End Sub

    Protected Function isBenList(data As String) As Boolean
        isBenList = False

        Command = New Odbc.OdbcCommand("SELECT count(id) FROM word_ben where id='" + Session("id") + "' and word='" + data + "'", MyConn)

        If Command.ExecuteScalar = 1 Then
            isBenList = True
        End If


    End Function

    Protected Sub insert_Click(sender As Object, e As EventArgs) Handles insert.Click

        On Error Resume Next

        Dim id As String = Session("id")

        MyConn.Open()

        If dblist.SelectedIndex = -1 Then
            Response.Write("<script>alert('단어장을 선택해주세요.\n단어장 목록 새로고침을 누르시면 보입니다.')</script>")
            Exit Sub
        End If

        On Error Resume Next

        For i = 0 To cb1.Items.Count - 1
            If cb1.Items(i).Selected = True Then
                Command = New Odbc.OdbcCommand("insert into notes values('" + id + "','" + dblist.SelectedItem.Text + "','" + cb1.Items(i).Text + "','" + Now + "')", MyConn)
                Command.ExecuteNonQuery()
            Else
                Command = New Odbc.OdbcCommand("insert into word_ben values('" + id + "','" + cb1.Items(i).Text + "','" + Now + "')", MyConn)
                Command.ExecuteNonQuery()
            End If
        Next


        Command = New Odbc.OdbcCommand("insert into sentence values('" + Session("id") + "','" + dblist.SelectedItem.Text + "','No named','" + incoding_sentence(input_box.Text) + "','','" + Now + "')", MyConn)
        Command.ExecuteNonQuery()

        MyConn.Close()

        Response.Write("<script>alert('추가가 완료 되었습니다.')</script>")

    End Sub

    Protected Sub makeNote_Click(sender As Object, e As EventArgs) Handles makeNote.Click
        Dim ntn As String = noteName.Text
        Dim id As String = Session("id")
        MyConn.Open()

        On Error Resume Next

        If makeNote.Text = "단어장 생성 하기" Then
            noteN.Visible = True
            noteName.Visible = True
            noteName.Text = ""
            makeNote.Text = "생성"

        ElseIf ntn <> "" Then
            Command = New Odbc.OdbcCommand("SELECT * FROM notes where id='" + id + "' and note_name='" + ntn + "'", MyConn)

            If Command.ExecuteScalar = "" Then

                Command = New Odbc.OdbcCommand("insert into notes values('" + id + "','" + ntn + "','MasterNT','" + Now + "')", MyConn)
                Command.ExecuteNonQuery()

                noteN.Visible = False
                noteName.Visible = False
                makeNote.Text = "단어장 생성 하기"
                Response.Write("<script>alert('정상적으로 생성 완료 됬습니다.')</script>")

            Else
                Response.Write("<script>alert('이미 존재하는 단어장 이름입니다.')</script>")
            End If
        Else
            Response.Write("<script>alert('단어장의 길이는 최소 한글자 이상입니다.')</script>")
        End If

        MyConn.Close()
    End Sub
End Class