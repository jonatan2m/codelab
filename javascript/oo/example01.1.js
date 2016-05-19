function A(name){
	this._name = name || "A";

	this.getName = function(){
		return this._name;
	};
}

function BA(){
	this.getName = function(){
		return "B" + this._name;
	};
}

BA.prototype = new A();

function CA(){
	A.apply(this, arguments);	
}

CA.prototype = new A();
