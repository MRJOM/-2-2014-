<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="quiz.aspx.vb" Inherits="Asp_Project.WebForm1" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no"/>
    <link rel="stylesheet" href="css/style.css" />
    <title>봉의고등학교</title>
    <style>
    #align {  
        width:100%;  
        height:100px;
        text-align:center;  
    }  
     
    #content {  
        margin:0 auto;
        width:220px;
        height:400px;
        text-align:left;  
    }
</style>
</head>


<body lang="ko-kr">
     <header>
	    <div class="row">
		    <nav id="gnb">
			    <a href="#contents" class="blind">skip navigation</a>
			    <ul class="clearfix">
				    <li><a href="./main.aspx" title="홈">Home</a>&emsp;&emsp;<a href="./userinfo.aspx">회원정보</a>&emsp;&emsp;<a href="./editnote.aspx">단어장관리</a>&emsp;&emsp;<a href="./plusword.aspx">단어추가</a>&emsp;&emsp;<a href="./quiz.aspx">단어암기</a>&emsp;&emsp;<a href="./sentence.aspx">빈칸추론(예문)</a>&emsp;&emsp;<a href="./manyWrong.aspx">오답 정리 및 통계</a></li>

			    </ul>
		    </nav>
	    </div>
    </header>
    <section id="contents" class ="row">
        <article id="main">
        <h2>
            단어장 암기</h2>
        <div id="content">
        <form id="form1" runat="server" class="align">
            <asp:Label ID="Label1" runat="server" Text="문제 유형"></asp:Label>
            <asp:RadioButtonList ID="qu" runat="server" Width="120px">
                <asp:ListItem>주관식(영/한)</asp:ListItem>
                <asp:ListItem>객관식(영/한)</asp:ListItem>
                <asp:ListItem>객관식(한/영)</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Label ID="Label2" runat="server" Text="단어장"></asp:Label>
            <asp:Button ID="getlist" runat="server" Text="단어장 목록 새로고침" Width="220px" />
            <asp:RadioButtonList ID="noteTable" runat="server">
            </asp:RadioButtonList>
            <asp:Button ID="start" runat="server" Text="시작" Width="220px" />
            <br />
            문제: (<asp:Label ID="question" runat="server" Text="Null" Visible="False"></asp:Label>
            )<br />
            <br />
            <asp:Label ID="Label4" runat="server" Text="정답" Visible="False"></asp:Label>
            <asp:RadioButtonList ID="sq5" runat="server">
            </asp:RadioButtonList>
            <asp:TextBox ID="anc" runat="server" Visible="False" Width="220px"></asp:TextBox>
            <br />
            <asp:Button ID="check" runat="server" Text="정답 확인" Width="220px" Visible="False" />
            <br />
            <asp:Button ID="getCurrect" runat="server" Text="정답 단어 보기" Width="220px" Visible="False" />
            <br />
            <asp:Label ID="currect" runat="server" Text="Label" Visible="False"></asp:Label>
        </form>
        </div>
        </article>
    </section> 
</body>
</html>
