
var l = function (msg){
  console.log(msg);
}
var ln = ()=> { console.log('----------------------------');};
var la = console.log;
var evens = [0,2,4,6,8];
l('The resources can find on http://es6-features.org/')
l('and https://hacks.mozilla.org/category/es6-in-depth/')
ln()
l('Constans')
const PI = 3.141593;
ln();

l('Scoping: Block-Scoped Variables:')
ln();

function testLet(){  
  if(true){
    let i = 1;
    var i1 = 1;    
  }  
  l('    i is not defined. ' + 'value i1 is ' + i1);
}
testLet();
ln();

l('Arrow Functions: Expression Bodies')
l('    odds  = evens.map(v => v + 1)')
l('    odds  = evens.map(function (v) { return v + 1; });')
let odds = evens.map(n => n + 1);
l(odds);
let pairs = evens.map(n => ({even: n, odd: n +1}));
l(pairs[0].even + " e " + pairs[0].odd);
l('    Lexical this: More intuitive handling of current object context.')
var threes = [];
this.evens.forEach((v) => {
  if(v % 3 === 0)
    this.threes.push(v)
})
ln();

l('Extended Parameter Handling: ' + 'Default Parameter Values')
function f(x, y = 7, z = 42){return x+y+z;}
l(f(1))
l('Extended Parameter Handling: ' + 'Rest Parameter (similar ao arguments)')
function g(x,y,...a){ 
  return (x+y)*a.length;
}
l(g(1,2,'hello', true,7))
l('Extended Parameter Handling: ' + 'Spread Operator')
function g1(a,...b){  
  return a;
}
var _g1 = [2,...evens, 2]
l(_g1)
l(g1(_g1))
var arr1 = [0, 1, 2];
var arr2 = [3, 4, 5];
arr1.push(...arr2);
l(arr1)
ln()

l('Template Literals: String Interpolation')
var customer = {name: 'Foo'}
var message = `Hello, ${customer.name}`
l(message)
l('    Custom Interpolation')
function stringInterpolation(strings, ...values){
  l(strings[0] + ", " +values[0])
}
stringInterpolation `http://example.com/foo?bar=${customer.name}`
l('    Raw String Access')

function rawStringAccess(strings, ...values){
  l(strings[0])
  l(strings[1])
  l(strings.raw[0])
  l(strings.raw[1])
  l(values[0])
}
rawStringAccess `foo\n${ 42 }bar`
l(`foo\n${ 42 }bar`)
l(String.raw `foo\n${ 42 }bar`)
ln()

l('Extended Literals: Binary & Octal Literal')
l(0b1000)
l(0o75)
l('Unicode String & RegExp Literal')
l("𠮷".match(/./u)[0].length)
l("b".codePointAt(0))
ln()

l('Enhanced Regular Expression: Regular Expression Sticky Matching')
ln()

l('Enhanced Object Properties: Property Shorthand')
var x = 12, y = 10
var obj = {x,y}
l(obj)
l('Enhanced Object Properties: Computed Property Names')
let obj2 = {
  foo: 'bar',
  ['myvar_'+x]: x
}
l(obj2)
l('Enhanced Object Properties: Method Properties')
let obj3 = {
  foo(a,b){
    return a + " " + b
  },
  *bar (a){
    l(a)
  }
}
l(obj3.foo(1,2))
l(obj3.bar(1))
ln()

l('Destructuring Assignment: Array Matching (not clear)')

l('Destructuring Assignment: Object Matching, Shorthand Notation')
function objectMatching (){return {foo:'foo', bar:'bar'};}
var t = {foo, bar} = objectMatching();
console.log(t.foo)
//or using "var {foo, bar}". the properties 'foo and bar' will append on 'this'
l('Destructuring Assignment: Parameter Context Matching')

function parameterContextMatching1([name, val]){la(name, val)}
function parameterContextMatching2({name: n, val: v}){la(n, v)}
function parameterContextMatching3({name, val}){la(name, val)}
parameterContextMatching1(["bar", 42])
parameterContextMatching2({name: "foo", val: 7})
parameterContextMatching3({name: "bar", val: 42})

l('Destructuring Assignment: Fail-Soft Destructuring')
var list = [7, 42]
var [a = 1, b = 2, c = 3, d] = list
la(a,b,c,d)
ln()

l('Modules: Value Export/Import (the examples are commented)')
//examples
// lib/math.js
//export function sum(x, y){return x+y}
//export var pi = 3.141593

//someApp.js
//import * as math from "lib/math"
//l("2π = " + math.sum(math.pi, math.pi))

//otherApp.js
//import {sum, pi} from "lib/math"
//l("2π = " + sum(pi,pi))

l('Modules: Default & Wildcard  (the examples are commented)')
//  lib/mathplusplus.js
//export * from "lib/math"
//export var e = 2.71828182846
//export default (x) => Math.exp(x)

//  someApp.js
//import exp, { pi, e } from "lib/mathplusplus"
//console.log("e^{π} = " + exp(pi))
ln()

l('Classes: Class Definition')
class Shape{
  constructor (id, x, y){
    this.id = id
    this.move(x,y)
  }
  move(x,y){
    this.x = x
    this.y = y
  }
}
l('Classes: Class Inheritance')
class Rectangle extends Shape {
  constructor (id, x, y, width, height){
    super(id,x,y)
    this.width = width
    this.height = height
  }
}
class Circle extends Shape{
  constructor (id, x, y, radius){
    super(id, x, y)
    this.radius = radius
  }
}
l`Classes: Class Inheritance, From Expressions`

console.log`Support for mixin-style inheritance by extending
from expressions yielding function objects. 
[Notice: the generic aggregation function
is usually provided by a library like this one, of course]`
l`read more: http://es6-features.org/#ClassInheritanceFromExpressions`

l`Classes: Base Class Access (the same way in Java super.[prop|method])`

l`Classes: Static Members`

class Circle1 extends Shape {    
    static defaultCircle () {
        return new Circle1("default", 0, 0, 100)
    }
}
var defCircle = Circle1.defaultCircle()
l(defCircle)

l`Classes: Getter/Setter`
class Rectangle1 {
  constructor (width, height){
    this.width = width
    this.height = height
  }
  set width (width) {this._width = width}
  get width ()      {return this._width}
  set height (height) { this._height = height             }
  get height ()       { return this._height               }
  get area   ()       { return this._width * this._height }
}
var r1 = new Rectangle1(50, 20);
l(r1.area)
ln()

l `Symbol Type: Symbol Type`
l`Unique and immutable data type to be used as an identifier for object properties. Symbol can have an optional description, but for debugging purposes only.`
l('Symbol("foo") !== Symbol("foo") is ' + (Symbol("foo") !== Symbol("foo")))
const foo1 = Symbol()
const bar1 = Symbol()
l('typeof foo1 === "symbol" is ' + typeof foo1 === "symbol")
//l(typeof bar1 === "symbol")
let obj1 = {}
obj1[foo1] = "foo"
obj1[bar1] = "bar"
JSON.stringify(obj1) // {}
Object.keys(obj1) // []
Object.getOwnPropertyNames(obj1) // []
Object.getOwnPropertySymbols(obj1) // [ foo, bar ]

l(`Symbol Type: Global Symbols (Global symbols, indexed through unique keys.)`)
l('Symbol.for("app.foo") === Symbol.for("app.foo")')
l `read more: http://es6-features.org/#GlobalSymbols`
ln()

l`Iterators: Iterator & For-Of Operator`
l`Complete Guide: https://hacks.mozilla.org/2015/04/es6-in-depth-iterators-and-the-for-of-loop/`
var s = ['a','s','d','f','a']
for(let value of s){
  l(value)
}
l`The for–in loop is for looping over object properties.
The for–of loop is for looping over data—like the values in an array.
The for–of is not just for arrays. It also works on most array-like objects, like DOM NodeLists.`
l`It also works on Map and Set objects.`
//For example, a Set object is good for eliminating duplicates:
// make a set from an array of words
var uniqueS = new Set(s)
for(let value of uniqueS){
  l(value)
}
l`A Map is slightly different: the data inside it is made of key-value pairs, so you’ll want to use destructuring
 to unpack the key and value into two separate variables:`
var countFiveIterator = {
  counter: 1,
  lastCounter: 0,
  [Symbol.iterator]: function (){
    return this;
  },
  next: function (){    
    return {done: this.counter > 5, value: this.counter++};
  },
  return: function (){    
    this.lastCounter = this.counter
    this.counter = 1
    l(this.lastCounter) 
    return {done: true, value: this.counter};
  },
  throw: function (exc){
    this.counter = 1
    ln(exc)
  }
};

for(let v of countFiveIterator){
  l(v)  
  break;
}
for(let v of countFiveIterator){
  l(v)    
  //throw new Error('Crashed!')
}
for(let v of countFiveIterator){
  l(v)    
}
l`Generators: Generator Function, Iterator Protocol`
l`read more: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Iterators_and_Generators`
l`Iterators precisam que seja definida uma função next
e que o dev gerencie seu estado. Uma Generator Function, como o exemplo abaixo,
gerencia seu próprio contexto, bastando apenas seguir como o exemplo`
l`Generator Function`
function* idMaker(){
  var index = 0;
  while(true)
    yield index++;
} 

var gen = idMaker();
l(gen.next().value)
l(gen.next().value)
l(gen.next().value)

l`User-defined iterables: We can make our own iterables like this:`
var myIterable = {}
myIterable[Symbol.iterator] = function* () {
  yield 1;
  yield 2;
  yield 3;
};
for(let value of myIterable){
  l(value)
}
l`Built-in iterables: String, Array, TypedArray, Map and Set are
all built-in iterables, because the prototype objects of them all
have a Symbol.iterator method.`
l`Syntaxes expecting iterables: Some statements and expressions are expecting iterables,
for example the for-of loops, spread operator, yield*, and destructuring assignment.`

function* gen1(){
  yield* ["a", "b", "c"]
}
l(gen1().next())
var [a, b, c] = new Set(["a", "b", "c"])
l(a)
l `Advanced generators: Here is the fibonacci generator using next(x) to restart the sequence:`
function* fibonacci(){
  var fn1 = 0;
  var fn2 = 1;
  while(true){    
    [fn1, fn2] = [fn2, fn1 + fn2];
    var reset = yield fn2;
    if(reset){
      fn1 = 0;
      fn2 = 1;
    }
  }
}
var sequence = fibonacci();
l(sequence.next().value);
l(sequence.next().value);
l(sequence.next().value);
l(sequence.next(true).value);
l(sequence.next().value);
l(sequence.next().value);
l(sequence.next().value);

l('Generator Function, Direct Use Support for generator functions, a special variant of functions where the control flow can be paused and resumed, in order to produce sequence of values (either finite or infinite).')
function* range(start, end, step){
  while(start < end){
    yield start
    start += step
  } 
} 
for(let i of range(0,10,2)){
  l(i)
}

var numbers = [...range(0,10,2)];
l(numbers)
let [n1, n2, ...others] = numbers
la(n1, n2, others)
l `http://es6-features.org/#GeneratorControlFlow`
ln()

l`Map/Set & WeakMap/WeakSet: Set Data-Structure`

l`http://pt.slideshare.net/Tech_MX/set-data-structure-i`
let s1 = new Set()
s1.add('hello').add('goodbye').add('hello')
l(s1.size === 2)
l(s1.has('hello') === true)
l(s1.values())
for (let k of s1.values()){
  //insertion order
  l(k)
}
l`Map Data-Structure`
let m1 = new Map()
m1.set('hello', 42)
l(m1.get('hello'))
for(let [key1, val1] of m1.entries()){
  l(key1 + ' = ' + val1)
}
l`https://www.sitepoint.com/preparing-ecmascript-6-map-weakmap/`

l`Weak-Link Data-Structures: Memory-leak-free Object-key’d side-by-side data-structures.`
l `http://es6-features.org/#WeakLinkDataStructures`
ln()

l`Typed Arrays: Support for arbitrary byte-based data structures to implement network protocols, cryptography algorithms, file format manipulations, etc.`