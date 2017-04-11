function hanoi (n) {
  var pegs = {
    A: [], B: [], C: []
  };
  
  function printBoard() {
    console.log("A>" + pegs.A);
    console.log("B>" + pegs.B);
    console.log("C>" + pegs.C);
    console.log("-----");
  }
  
  function move (disks, from, to) {
    
    if(disks === 0) {      
      printBoard();
      return;
    }
    else if(disks === 1) {
      pegs[to].push(pegs[from].pop());
      printBoard();
    }else{      
      var spared = getSpared(from, to);      
      move(disks - 1, from, spared);      
      
      pegs[to].push(pegs[from].pop());            
      printBoard();
      move(disks - 1, spared, to);

    }
  }
    
  function getSpared(from, to) {
    return "ABC".replace(from,"").replace(to,"");
  }
  
  for(var i = n; i > 0; i--){
        pegs.A.push(i);
  }
    
  move(n, 'A', 'C');
  printBoard();
}

hanoi(4)
