
Public Class account
    Inherits System.Web.UI.Page

    Protected StrConn As String = "DRIVER={MySQL ODBC 5.1 Driver};" + "Server=telsam.codns.com;" + "port=3306;" + "option=16384;" + "database=Asp_Project;" + "uid=root;" + "password=tkdlaekd0;"    'SQL Connect
    Protected MyConn As New Odbc.OdbcConnection(StrConn)
    Protected Command As New Odbc.OdbcCommand()

    Protected Sub submit_Click(sender As Object, e As EventArgs) Handles submit.Click

        On Error GoTo err

        If check() <> "" Then
            Response.Write("<script>alert('" + check() + "')</script>")
            Exit Sub
        End If

        MyConn.Open()

        Command = New Odbc.OdbcCommand("SELECT * FROM account where id like '" + id.Text + "'", MyConn) '중복 확인

        If Command.ExecuteScalar = "" Then
            Command = New Odbc.OdbcCommand("insert into account values(" + ccovcov() + ")", MyConn) '아이디 등록
            Command.ExecuteNonQuery()
        Else
            Response.Write("<script>alert('이미 존재 하는 아이디 입니다.')</script>")
            MyConn.Close()
            Exit Sub
        End If

        Response.Write("<script>alert('회원가입을 축하합니다.')</script>")

        MyConn.Close()  '데이터 베이스 연결 종료
        Exit Sub

err:
        Response.Write("<script>alert('회원 가입중 오류가 발생 하였습니다. \n관리자에게 문의 바랍니다 \noneokrock@nate.com')</script>")

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

    Protected Function check() As String    '회원가입 조건

        check = ""

        If id.Text = "" Then
            check = "아이디를 입력해 주세요."
            Exit Function
        ElseIf passwd_chk(id.Text) Then
            check = "A-z,0-9,_를 제외한 특수문자를\n아이디로 사용할 수 없습니다."
            Exit Function
        ElseIf Len(id.Text) < 6 Then
            check = "아이디는 6글자 이상 20글자 이하로 사용 가능합니다."
            Exit Function
        ElseIf Len(id.Text) > 20 Then
            check = "아이디는 6글자 이상 20글자 이하로 사용 가능합니다."
            Exit Function
        End If

        If passwd.Text = "" Then
            check = "패스워드를 입력해 주세요."
        ElseIf Len(passwd.Text) < 8 Then
            check = "패스워드를 8자리 이상으로 입력해 주세요."
        End If

    End Function
    Public Function passwd_chk(ByVal instack As String) As Boolean

        passwd_chk = False

        For i = 1 To Len(instack)
            If Mid(instack, i, 1) Like "*[A-z]*" Or Mid(instack, i, 1) = "_" Or Mid(instack, i, 1) Like "*[0-9]*" Then
            Else
                passwd_chk = True
            End If
        Next

    End Function

    Protected Function ccovcov() As String  '콜론 뭉치기
        ccovcov = ccov(id.Text) + "," + ccov(name.Text) + "," + ccov(MD5CalcString(passwd.Text)) + "," + ccov("") + "," + ccov(Now) + "," + ccov(email.Text) + "," + ccov(gender.SelectedValue)
    End Function
    Protected Function ccov(text As String) As String   '콜론 붙이기
        ccov = "'" + text + "'"
    End Function

End Class