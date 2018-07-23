<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="sentence.aspx.vb" Inherits="Asp_Project.WebForm3" %>

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
            예문(Example Sentence) 학습</h2>
        <div id="content">
        <form id="form1" runat="server" class="align">
            유형<asp:RadioButtonList ID="selectMode" runat="server">
                <asp:ListItem>주관식</asp:ListItem>
                <asp:ListItem>객관식</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Label ID="Label2" runat="server" Text="단어장"></asp:Label>
            <asp:Button ID="getlist" runat="server" Text="단어장 목록 새로고침" Width="220px" />
            <asp:RadioButtonList ID="noteTable" runat="server">
            </asp:RadioButtonList>
            <asp:Button ID="start" runat="server" Text="예문 가져오기" Width="220px" />
            <br />
            <asp:Label ID="label9" runat="server" Text="유형 : " Visible="False"></asp:Label>
            <asp:Label ID="typez" runat="server" Text="Null" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="label0" runat="server" Text="문장" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="question" runat="server" Text="Null" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="label1" runat="server" Text="해석" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="transe" runat="server" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="fromWord" runat="server" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="Label4" runat="server" Text="정답" Visible="False"></asp:Label>
            <asp:RadioButtonList ID="sq5" runat="server">
            </asp:RadioButtonList>
            <asp:TextBox ID="anc" runat="server" Visible="False" Width="220px"></asp:TextBox>
            <asp:Label ID="hint" runat="server" Text="정답" Visible="False"></asp:Label>
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
