<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Main.aspx.vb" Inherits="Asp_Project.main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
    A:link {text-decoration:none}
    A:visited {text-decoration:none}
    A:hover {text-decoration:none}
    </style>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no"/>
    <script src="js/respond.min.js"></script>
    <link rel="stylesheet" href="css/style.css" />
    <script src="js/modernizr.min.js"></script>
    <script src="js/jquery.min.js"></script>
    <script src="js/jquery.scrolledFix.js"></script>
    <script src="js/script.js"></script>
    <script src="js/respond.min.js"></script>
    <title>봉의고등학교</title>
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
            Bong-Eui high school
        </h2>
        <ul>
            <li class="clearfix">
                <div class="col-8">
					<h4>안녕하세요!</h4>
					<p>이 홈페이지는 정보올림피아드 공모전 출품작으로 만들어 졌으며, 회원가입후 무료로 이용 가능합니다. 지금 당장 문장을 넣고 단어를 외워보세요! 이 사이트는 당신의 단어 암기를 도와줄 것입니다.
                        회원님의 비밀번호는 md5(hash함수) 형식으로 암호화 되어 저장되며, 본사이트 관리자도 회원님의 비밀번호를 알 수 없으므로 안심하셔도 됩니다! 오류사항이나
                        <br />기타 건의사항은 관리자 메일로 보내주십시오. <br /><br />관리자 Email : oneokrock@nate.com
					</p>
				</div>
                <img class="col-4 last" src="image/image.jpg" alt="" width="296" height="206" />
                <div class="col-12 last">
                    <p></p>
                </div>
                <div class="col-12 last">
                    <p>Copyleft! 아이디어 무단도용을 제외한 모든 사용을 허가합니다.</p>
                </div>
            </li>

        </ul>
        </article>
        <img class="col-2" src="image/naver.jpg" />
        <img class="col-2" src="image/oxford.gif"/>
        <img class="col-2" src="image/logo%20(1).gif"/>
        <div class="col-2"><p>참조 및 정보제공</p></div>
    </section>
</body>
</html>