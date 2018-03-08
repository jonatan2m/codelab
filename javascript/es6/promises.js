/**********
  Promises

Promise states
pending: may transition to fulfilled or rejected
fulfilled (kept promise): must have a value
rejected (broken promise): must have a reason for rejecting the promise

promise.then( onFulfilled, onRejected );
or
promise.catch( errorHandler );
 
******************/
var l = console.log;

var fn = (result) => l(result);

var t = Promise.resolve(1).then(function (resolve){
  l(resolve)
})


let promise1 = new Promise( function( resolve, reject ) {
    setTimeout( () => resolve(10), 5000);
    // call resolve( value ) to resolve a promise
    // call reject( reason ) to reject a promise
});
 
promise1.then(fn)


var p1 = Promise.resolve("Jonatan");

p1.then((value) => value + " Machado" )
.then((value) => {
  var name = value + " Martins";
  l(name);
  return name;
})
.then((value) => value.toUpperCase())
.then(fn)


var loan1 = new Promise( (resolve, reject) => { 
  setTimeout( () => resolve( 110 ) , 1000 ); 
}); 
var loan2 = new Promise((resolve, reject) => { 
  setTimeout( () => resolve( 120 ) , 2000 ); 
});
var loan3 = new Promise( (resolve, reject) => {
  resolve( 'Bankrupt' );
});
 
Promise.all([ loan1, loan2, loan3 ]).then( value => { 
  console.log(value);
}, reason => {
  console.log(reason);
} );
 