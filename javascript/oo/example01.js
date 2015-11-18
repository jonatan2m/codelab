function ClasseBase(){
  this.base = 0;
  console.log('objeto criado pelo meu construtor');
}

ClasseBase.prototype.add = function (value){this.base += value;};
ClasseBase.prototype.get = function (){return this.base;};
ClasseBase.prototype.constructor = ClasseBase;

var c = new ClasseBase();

c.add(2);

var d = new ClasseBase();

d.add(5);

console.log(c.get(), d.get());