var asCircleCached = (function() {
	var area = function() {
		return Math.PI * this.radius * this.radius;
	};
	var grow = function() {
    	this.radius++;
   	};
   	var shrink = function() {
    	this.radius--;
   	};
   	return function() {
    	this.area = area, this.grow = grow, this.shrink = shrink;
   	}
}());

//set up test constructor
var CircularObject = function(radius) {
	this.radius = radius
};

//Usage
asCircleCached.call(CircularObject.prototype);
var obj = new CircularObject(4);
obj.shrink();