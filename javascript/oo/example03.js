
//example without inheritance, but it's better option
var o = Object.create({}, {
  foo: {     
    value: 4,
    writable: true    
  },
  bar: {
    configurable: true,    
    enumerable: true,
    get: function (){return this.foo + 5;},
    set: function (value){console.log('valor: ' + value);}
  }
});
o.foo = 2;
o.bar = 1;
console.log(Object.keys(o), o.foo, o.bar);
