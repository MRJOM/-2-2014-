<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="tryeng.aspx.vb" Inherits="Asp_Project.tryeng" %>

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
            회원가입 페이지 :) !!
        </h2>
        <div id="content">
        <form id="form1" runat="server" class="align">
            <br />

            <asp:RadioButtonList ID="list" runat="server">
            </asp:RadioButtonList>
            <asp:Label ID="label" runat="server" Text="선택된 단어장 : " Visible="False"></asp:Label>
            <br />
            <asp:Label ID="selectedNote" runat="server" Text="Label" Visible="False"></asp:Label>
            <asp:Button ID="getlist" runat="server" Text="단어장 가져오기" Width="220px" />

            <asp:RadioButtonList ID="senlist" runat="server">
            </asp:RadioButtonList>
            <asp:Button ID="getSen" runat="server" Text="문장리스트 가져오기" Visible="False" Width="220px" />

        </form>
        </div> 
        </article> 
     </section> 
</body>