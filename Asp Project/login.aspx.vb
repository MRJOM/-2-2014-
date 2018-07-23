Public Class login

    Inherits System.Web.UI.Page

    Protected StrConn As String = "DRIVER={MySQL ODBC 5.1 Driver};" + "Server=telsam.codns.com;" + "port=3306;" + "option=16384;" + "database=Asp_Project;" + "uid=root;" + "password=tkdlaekd0;"    'SQL Connect
    Protected MyConn As New Odbc.OdbcConnection(StrConn)
    Protected Command As New Odbc.OdbcCommand()
    Protected ComReader As Odbc.OdbcDataReader

    Protected Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click

        MyConn.Open()

        Dim idt, pwt As String
        idt = ""
        pwt = ""

        Command = New Odbc.OdbcCommand("SELECT * FROM account where id='" + id.Text + "'", MyConn)
        ComReader = Command.ExecuteReader

        While ComReader.Read
            idt = ComReader.Item(0)
            pwt = ComReader.Item(2)
        End While

        If idt = id.Text And pwt = MD5CalcString(passwd.Text) Then

            Session("id") = id.Text
            Session("UserProtect") = MD5CalcString(id.Text + "UserProtect_AspProject")

            Command = New Odbc.OdbcCommand("update account set login_dt='" + Now + "'", MyConn) '최근 로그인 기록 갱신
            Command.ExecuteNonQuery()

            Command = New Odbc.OdbcCommand("insert into login_log values('" + id.Text + "','" + Now + "','" + Request.UserHostAddress + "')", MyConn)   '로그인 성공 로그
            Command.ExecuteNonQuery()
            Response.Redirect("main.aspx")

        Else
            Response.Write("<script>alert('아이디 또는 비밀번호가 틀립니다.')</script>")
            Command = New Odbc.OdbcCommand("insert into error_log values('" + id.Text + "','" + Now + "','" + Request.UserHostAddress + "','Login Fail')", MyConn)   '로그인 실패 로그
            Command.ExecuteNonQuery()
        End If

        MyConn.Close()

    End Sub

    Protected Sub SessionCheck()    '섹션 입력 해킹 방지 코드

        MyConn.Open()

        If Session("id") <> "" Then
            If Session("UserProtect") <> MD5CalcString(Session("id") + "UserProtect_AspProject") Then
                Command = New Odbc.OdbcCommand("insert into error_log values('" + Session("id") + "','" + Now + "','" + Request.UserHostAddress + "','Session Hacking')", MyConn)   '로그인 로그
                Command.ExecuteNonQuery()
                Session("id") = ""
                Session("UserProtect") = ""
            Else
                Response.Redirect("main.aspx")
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

        Call SessionCheck()

    End Sub

End Class