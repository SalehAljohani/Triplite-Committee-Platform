$.noConflict();

jQuery(document).ready(function ($) {

	"use strict";

	[].slice.call(document.querySelectorAll('select.cs-select')).forEach(function (el) {
		new SelectFx(el);
	});

	jQuery('.selectpicker').selectpicker;

	$('.search-trigger').on('click', function (event) {
		event.preventDefault();
		event.stopPropagation();
		$('.search-trigger').parent('.header-left').addClass('open');
	});

	$('.search-close').on('click', function (event) {
		event.preventDefault();
		event.stopPropagation();
		$('.search-trigger').parent('.header-left').removeClass('open');
	});

	$('.equal-height').matchHeight({
		property: 'max-height'
	});


	// Menu Trigger
	$('#menuToggle').on('click', function (event) {
		var windowWidth = $(window).width();
		if (windowWidth < 1010) {
			$('body').removeClass('open');
			if (windowWidth < 7600) {
				$('#left-panel').slideToggle();
			} else {
				$('#left-panel').toggleClass('open-menu');
			}
		} else {
			$('body').toggleClass('open');
			$('#left-panel').removeClass('open-menu');
		}

	});


	//$(".menu-item-has-children.dropdown").each(function () {
	//	$(this).on('click', function () {
	//		var $temp_text = $(this).children('.dropdown-toggle').html();
	//		$(this).children('.sub-menu').prepend('<li class="subtitle">' + $temp_text + '</li>');
	//	});
	//});


	// Load Resize 
	$(window).on("load resize", function (event) {
		var windowWidth = $(window).width();
		if (windowWidth < 1010) {
			$('body').addClass('small-device');
		} else {
			$('body').removeClass('small-device');
			$('#left-panel').removeAttr('style');
		}
	});


});