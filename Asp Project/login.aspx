<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="Asp_Project.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no"/>
    <link rel="stylesheet" href="css/style.css" />
    <title>봉의고등학교</title>
    <style>
    A:link {text-decoration:none}
    A:visited {text-decoration:none}
    A:hover {text-decoration:none}
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
            로그인이 필요한 서비스입니다. :_: Login in Plz
        </h2>
        <div id="content">
            <form id="form1" runat="server" class="align">
            
                    <br />
                    아이디 : <asp:TextBox ID="id" runat="server" Height="20px" Width="220px"></asp:TextBox>
                    <br />
                    비밀번호 : <asp:TextBox ID="passwd" runat="server" Height="20px" Width="220px" TextMode="Password"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="Submit" runat="server" Text="로그인" Width="220px" />
                    <a href="account.aspx">회원가입</a><br />
                    <img src="image/naver.jpg"/>
                    <br />
                    <img src="image/oxford.gif"/>
                    <br />
                    <img src="image/logo%20(1).gif"/>
                    <br />
            
            </form>
        </div>
        </article>

    </section>
</body>
</html>
