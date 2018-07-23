Public Class manyWrong
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
        Call statsG()
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

    Protected Function c09(ByVal instack As String) As Boolean
        c09 = False
        For i = 1 To Len(instack)
            If Mid(instack, i, 1) Like "*[0-9]*" Then
            Else
                c09 = True
            End If
        Next
    End Function

    Protected Sub statsG()
        Dim PerT As Double = 0
        Dim Temp As String = ""

        If per.Text = "" Then
            Response.Write("<script>alert('입력해 주세요.')</script>")
            Exit Sub
        End If

        If c09(per.Text) Then
            Response.Write("<script>alert('숫자만 입력 가능합니다')</script>")
            Exit Sub
        End If

        If Val(per.Text) > 100 Then
            Response.Write("<script>alert('입력 범위는 1~100입니다')</script>")
            Exit Sub
        ElseIf Val(per.Text) < 0 Then
            Response.Write("<script>alert('입력 범위는 1~100입니다')</script>")
            Exit Sub
        End If

        makeNote.Visible = True

        MyConn.Open()

        PerT = Val(per.Text) / 100

        Command = New Odbc.OdbcCommand("select * from (select id, word, fail_cnt / tot_cnt as fail_rate, true_cnt / tot_cnt as ture_rate from (SELECT id, word, count(1) as tot_cnt, sum(case success when 'o' then 1 else 0 end ) as true_cnt, sum(case success when 'x' then 1 else 0 end ) as fail_cnt FROM asp_project.stats where id = '" + Session("id") + "' group by id , word) as A order by id, fail_rate desc ) as B where fail_rate>='" + CStr(PerT) + "' limit 500;", MyConn)
        ComReader = Command.ExecuteReader

        list.Items.Clear()
        While ComReader.Read
            Temp = ComReader.Item(1)
            list.Items.Add(" 오답률:" + CStr(Int(ComReader.Item(2) * 100)) + "%  " + Temp)
        End While

        MyConn.Close()

        For i = 0 To list.Items.Count - 1
            list.Items(i).Selected = True
        Next
    End Sub

    Protected Sub getStats_Click(sender As Object, e As EventArgs) Handles getStats.Click

        Call statsG()

    End Sub

    Protected Sub makeNote_Click(sender As Object, e As EventArgs) Handles makeNote.Click

        MyConn.Open()

        Dim f As Boolean = False
        Dim t As String = Now

        For i = 0 To list.Items.Count - 1
            If list.Items(i).Selected = True Then
                f = True
                Command = New Odbc.OdbcCommand("insert into notes values('" + Session("id") + "','WrongWord_" + Now + "','" + Split(list.Items(i).Text, "%  ")(1) + "','" + t + "')", MyConn)
                Command.ExecuteNonQuery()
            End If
        Next

        If f = False Then
            Response.Write("<script>alert('선택된 단어가 없습니다.')</script>")
        Else
            Response.Write("<script>alert('단어장 이름이 \nWrongWord_" + Now + "로\n생성 되었습니다.')</script>")
        End If

        MyConn.Close()

    End Sub
End Class