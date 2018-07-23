<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="userinfo.aspx.vb" Inherits="Asp_Project.WebForm2" %>

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
        width:219px;
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
            회원가입 페이지 :) !!
        </h2>
        <div id="content">
        <form id="form1" runat="server" class="align">
            <br />
            아이디 : 
            <asp:Label ID="id" runat="server" Text="Label"></asp:Label>
            <br />
            닉네임 :
            <asp:Label ID="nic" runat="server" Text="Label"></asp:Label>
            <br />
            비밀번호 : 
            <asp:Label ID="pw" runat="server" Text="Label"></asp:Label>
            <br />
            최근로그인 :<br />
            <asp:Label ID="login" runat="server" Text="Label"></asp:Label>
            <br />
            가입 날자 :<br />
            <asp:Label ID="reg" runat="server" Text="Label"></asp:Label>
            <br />
            이메일 : <asp:Label ID="email" runat="server" Text="Label"></asp:Label>
            <br />
            성별 :
            <asp:Label ID="sex" runat="server" Text="Label"></asp:Label>
&nbsp;<br />
        </form>
        </div>
        </article>
    </section> 
</body>
</html>
