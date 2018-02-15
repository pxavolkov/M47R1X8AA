var mainPage = {
	initPageLoader: function(duration, dotsNumber, dotsInterval) {

		//hide loader
		setTimeout(function() {
			$("#loader").hide();
			clearInterval(timerLoader);}, 
		duration);

		//add dots and final word to loader message
		var loaderCount = 0;
		var timerLoader = setInterval(function() {
			if (loaderCount < dotsNumber){
				loaderCount++;
				$("#loaderMessage").text($("#loaderMessage").text() + ".");
			} else {
				$("#loaderMessage").text($("#loaderMessage").text() + " complete");
				clearInterval(timerLoader);
			}
		}, dotsInterval);
	},

	//change magic numbers in the bottom right
	initMagicNumbers: function(freq) {
		var ieGenerator = function(length) {
			var result = "";
			$.each(new Array(length), function (){ result += getRandomInt(0, 10)});
			return {value: result};
		};

		var GetMagicNumbers = function(mnGen) {
		    return mnGen.next(4).value + "." + mnGen.next(4).value + "." + mnGen.next(7).value + "." + mnGen.next(3).value;
		};

		var getGenerator = function(){
			var mnGen;
			if (typeof mnMaker != "undefined") { //if we can use generator
				mnGen = mnMaker();
				mnGen.next().value; //init - first time we can't send parameters
			} else {
				mnGen = { //custom non-generator for IE
					next: ieGenerator
				}
			}

			return mnGen;
		}

		
		var gen = getGenerator();
		setInterval(function() {
			$("#magicnumbers").text(GetMagicNumbers(gen));
		}, freq);
	},

	//rotate circles on hover in the bottom right
	initCircleRotation: function() {
		var angle1 = 70, angle2 = -60, angle3 = 50, stopRotation = true;

		var rotateCircles = function (){
			if (stopRotation){
				return;
			}
			$("#circle1").rotate({animateTo:angle1, duration:4000});
		    $("#circle2").rotate({animateTo:angle2, duration:4000});
		    $("#circle3").rotate({animateTo:angle3, duration:4000, callback:rotateCircles});
		    angle1 = angle1 == -20 ? 70 : -20;
		    angle2 = angle2 == 30 ? -60 : 30;
		    angle3 = angle3 == -40 ? 50 : -40;
		};

		$( "#containercircle" ).hover(
		  function() {
		  	stopRotation = false;
		  	rotateCircles();
		  }, function() {
		  	stopRotation = true;
		  }
		);
	},

	//add dots in the top right
	initProgressDots: function(initLength, maxLength, freq) {
		var dotsLength = initLength;
		var updateProgress = function() {
			var text = "";
			$.each(new Array(dotsLength), function (){ text += ". "});
			$("#dots").text(text);
			dotsLength += 1;
			if (dotsLength > maxLength) dotsLength = 1;
		};
		setInterval(updateProgress, freq);
	},

	//move both equalizers
	initEqualizers: function(freq){

		var getIncremented = function(length, lower, upper) {
			var current = parseInt(length, 10);
			var increment = getRandomInt(1, 4);
			var sign = current > upper ? -1 :    //bigger than maximum
				(current < lower ? 1 :           //less than minimum
					getRandomInt(0, 2) * 2 - 1); //get random either 1 or -1
			return current + (sign * increment);
		};

		setInterval(function() {
			$(".equalizer").children().each(function(i) {
				var str = this.style.height.substr(0, this.style.height.indexOf("px"));
				var d = getIncremented(str, 25, 30);
				this.style.height = d + "px";
			});
			$("#rightEq").children().each(function(i) {
				var str = this.width.substr(0, this.width.indexOf("%"));
				var d = getIncremented(str, 40, 55);
				this.width = d + "%";
			});
		}, freq);
	},

	initModal: function(freq){
		$('#deepweb').click(function() {
			$("#deepWebAccess").modal();
			return false;
		});

		$('#deepwebxs').click(function() {
			$("#deepWebAccess").modal();
			return false;
		});

		setInterval(function() {
			if ($("#gt").css('visibility') != "hidden") $('#gt').css('visibility','hidden'); else $('#gt').css('visibility','visible');
		}, freq);
	}

};


$(function() {
	mainPage.initPageLoader(8000, 5, 1000);
	mainPage.initCircleRotation();
	mainPage.initMagicNumbers(500);
	mainPage.initProgressDots(21, 35, 1000);
	mainPage.initEqualizers(100);
	mainPage.initModal(500);
});


function getRandomInt(min, max) {
  return Math.floor(Math.random() * (max - min)) + min;
}
