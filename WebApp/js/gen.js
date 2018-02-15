function* mnMaker(){
  var length = 4; //default length
  while(true){
  	var result = "";
  	$.each(new Array(length), function (){ result += getRandomInt(0, 10)});
  	length = yield result;
  }
}