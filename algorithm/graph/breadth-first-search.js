/*
https://en.wikipedia.org/wiki/Breadth-first_search
Time Complexity: O(V+E) where V is number of vertices in the graph and E is number of edges in the graph.

Applications of Breadth First Traversal
source: http://www.geeksforgeeks.org/applications-of-breadth-first-traversal/

1) Shortest Path and Minimum Spanning Tree for unweighted graph
2) Peer to Peer Networks
3) Crawlers in Search Engines
4) Social Networking Websites
5) GPS Navigation systems
6) Broadcasting in Network
7) In Garbage Collection
8) Cycle detection in undirected graph
9) Ford–Fulkerson algorithm
10) To test if a graph is Bipartite
11) Path Finding
12) Finding all nodes within one connected component

Esse algoritmo não resolve nenhum problema especifico mas prepara a
estrutura para a resolução dos problemas acima de maneira eficiente
*/

function BreadthFirst(adjList, source) {
  var bfsInfo = [];
  
  adjList.forEach((item, index) => {
    bfsInfo[index] = {
      distance: null,
      predecessor: null
    };
  });
  
  var queue = [];

  bfsInfo[source].distance = 0;
  queue.push(source);
    
  while(queue.length > 0) {
  	source = queue.shift();  	

  	adjList[source].forEach((item) => {
  		if(bfsInfo[item].distance === null) {
  			bfsInfo[item].predecessor = source;
  			bfsInfo[item].distance = bfsInfo[source].distance + 1;  			
  			queue.push(item);	  	  			
  		}
  	}); 	
  }  

  return bfsInfo;  
}

var graph1 = [
  [1,2,3],
  [0,4],
  [0],
  [0,5],
  [1,6],
  [3],
  [4]
];

var result = BreadthFirst(graph1, 0);
result.forEach((item, elem) =>  {
  console.log(
    `${elem}: distance: ${item.distance} predecessor: ${item.predecessor}`);
});


