Public Class tryeng

    Inherits System.Web.UI.Page
    Protected StrConn As String = "DRIVER={MySQL ODBC 5.1 Driver};" + "Server=telsam.codns.com;" + "port=3306;" + "option=16384;" + "database=Asp_Project;" + "uid=root;" + "password=tkdlaekd0;"    'SQL Connect
    Protected MyConn As New Odbc.OdbcConnection(StrConn)
    Protected Command As New Odbc.OdbcCommand()
    Protected ComReader As Odbc.OdbcDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("id") = "telsam"
        Session("UserProtect") = MD5CalcString(Session("id") + "UserProtect_AspProject")

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

    Protected Sub refresh()
        MyConn.Open()

        list.Items.Clear()

        Dim id As String = Session("id")

        Command = New Odbc.OdbcCommand("SELECT distinct(note_name) FROM notes where id='" + id + "'", MyConn)
        ComReader = Command.ExecuteReader

        While ComReader.Read
            list.Items.Add(ComReader.Item(0))
        End While

        MyConn.Close()

    End Sub


    Protected Sub getlist_Click(sender As Object, e As EventArgs) Handles getlist.Click
        If getlist.Text = "단어장 가져오기" Then
            Call refresh()
            getSen.Visible = True
            getlist.Text = "단어장 선택"
            selectedNote.Visible = False
            label.Visible = False
        Else
            If list.SelectedIndex = -1 Then
                Response.Write("<script>alert('단어장을 선택해주세요.\n단어장 목록 새로고침을 누르시면 보입니다.')</script>")
                Exit Sub
            End If
            getlist.Text = "단어장 가져오기"
            selectedNote.Visible = True
            selectedNote.Text = list.SelectedItem.Text
            list.Items.Clear()
            label.Visible = True
        End If

    End Sub
End Class