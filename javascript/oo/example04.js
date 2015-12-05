var A = {
  say: function (){
    console.log('Call A, name: ' + this.name);
  }
};

var objA = Object.create(A, {
  name: {
    value: 'A',
    writable: true
  }
});

var objB  = Object.create(objA, {
  name: {
    value: 'B',
    writable: false
  }
});

var objC = Object.create(objA);
objC.say = function(){
  console.log('Call C ' + this.name);
};

objA.say();
objA.name = 'new A';
objA.say();

objB.name = 'new B';
objB.say();

objC.say();
