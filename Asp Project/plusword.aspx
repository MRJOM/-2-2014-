<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="plusword.aspx.vb" Inherits="Asp_Project.plusword" %>


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
        height:500px;
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
            단어 추가 (ﾟДﾟ)≡ﾟдﾟ)
        </h2>
        <div id="content">
            <form id="form2" runat="server" class="align">
            
                <asp:Label ID="noteN" runat="server" Text="이름" Visible="False"></asp:Label>
                <asp:TextBox ID="noteName" runat="server" Visible="False" Width="220px"></asp:TextBox>
                <asp:Button ID="makeNote" runat="server" Text="단어장 생성 하기" Width="220px" />
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="단어장 선택"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:Button ID="refresh" runat="server" Text="목록 새로고침" Width="115px" />
                    &nbsp;<br />
                    <asp:RadioButtonList ID="dblist" runat="server" Height="19px" Width="220px">
                    </asp:RadioButtonList>
                    <asp:Button ID="Extract" runat="server" Text="추출하기" Width="220px" />
                    <asp:TextBox ID="input_box" runat="server" Height="225px" TextMode="MultiLine" Width="220px"></asp:TextBox>
                    <br />
                    <asp:CheckBoxList ID="cb1" runat="server" EnableTheming="True" Width="220px" BorderStyle="None">
                    </asp:CheckBoxList>
                    <asp:Button ID="insert" runat="server" Text="선택목록 추가" Width="220px" Visible="False" />
                    <br />
            
            </form>
        </div>
        </article>

    </section>
</body>
</html>