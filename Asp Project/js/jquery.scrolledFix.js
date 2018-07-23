;(function($) {
	
	$.fn.scrolledFix = function(options) {
		
		// 기본옵션 설정
		$.fn.scrolledFix.defaults = {
			scrolled_point : $(this).offset().top
		}; 
		
		// 옵션 오버라이딩.
		options = $.extend($.fn.scrolledFix.defaults, options);
		
		// win, doc, html, body 참조 함수 설정.
		refInit();
		
		// 플러그인 코드 처리.
		return this.each(function() {
			
			// 플러그인 적용 대상 참조.
			var $this = $(this),
				$win  = $(win),
				cloned = false,
				$gnb_ul = $('#gnb').find('ul');
			
			// 플러그인 적용대상의 현재 설정 값을 data()에 저장합니다.
			$this.data({
				'pos' : $this.css('position'),
				'top' : $this.css('top'),
				'left': $this.css('left')
			});
			
			// 문서가 로드될 때, 스크롤 될 때 scrolledFix 함수 호출.
			$win.bind('load scroll', scrolledFix);
			
			// 스크롤 시, 특정영역을 넘어가면 내비게이션이 멈추게 하는 함수.
			function scrolledFix() {
			
				// 브라우저 및 기기 판별
				var UA = navigator.userAgent,
					iOS5 = UA.match(/iPhone OS 5/ig),
					android3 = UA.match(/android 3/ig),
					win_scrollTop = $win.scrollTop(),
					point = options.scrolled_point;
			
				// options.scrolled_point 값 검사 및 특정 지점보다 낮은 경우 함수 실행하지 않음.
				if(
					typeof point !== 'number' || 
					win_scrollTop < point - $this.outerHeight() &&
					(!iOS5 || !android3)
				) return false;
				
				if(win_scrollTop >= point) {	
					
					// 플러그인 적용대상에 고정 스타일 설정.
					$this
						.css({
							'position': 'fixed',
							'z-index': 100,
							'top': 0,
							'left': 0,
							'width': '100%'
						})
						.addClass('scrolled');
					
					// 내비게이션이 복제되지 않았다면...	
					if(!cloned) {
						$gnb_ul
							.clone()
							.appendTo('#header-bar div');
						cloned = true;	
					};
					
				} else {
					
					// 플러그인 적용대상 스타일을 되돌림.
					$this
						.css({
							'position': $this.data('pos'),
							'top': $this.data('top'),
							'left': $this.data('left')
						})
						.removeClass('scrolled');
					
					// 내비게이션이 복제되어 있다면...
					if(cloned) {
						$('#header-bar ul').remove();
						cloned = false;
					};
				
				};	
			
			};
				
		}); // e: return
	}; // e: plugin
	
	// win, doc, html, body 참조 함수
	function refInit() {
		win 	= window, 
		doc 	= document,
		html 	= document.documentElement,
		body 	= document.body;
	};
	
})(jQuery);