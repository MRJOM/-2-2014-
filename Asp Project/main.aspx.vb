Public Class main

    Inherits System.Web.UI.Page
    Protected StrConn As String = "DRIVER={MySQL ODBC 5.1 Driver};" + "Server=telsam.codns.com;" + "port=3306;" + "option=16384;" + "database=Asp_Project;" + "uid=root;" + "password=tkdlaekd0;"    'SQL Connect
    Protected MyConn As New Odbc.OdbcConnection(StrConn)
    Protected Command As New Odbc.OdbcCommand()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call SessionCheck()
        If Session("id") <> "" And Session("wellcome") <> 1 Then
            Session("wellcome") = 1
            Response.Write("<script>alert('어서오십시오! " + Session("id") + "님')</script>")
        End If

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

End Class