/*
https://en.wikipedia.org/wiki/Depth-first_search
Time Complexity: O(V+E) where V is number of vertices in the graph and E is number of edges in the graph.

Applications of Depth First Search
source: http://www.geeksforgeeks.org/applications-of-depth-first-search/

1) For an unweighted graph, DFS traversal of the graph produces
 the minimum spanning tree and all pair shortest path tree.
2) Detecting cycle in a graph 
3) Path Finding
4) Topological Sorting
5) To test if a graph is bipartite
6) Finding Strongly Connected Components of a graph 
7) Solving puzzles with only one solution

Esse algoritmo não resolve nenhum problema especifico mas prepara a
estrutura para a resolução dos problemas acima de maneira eficiente
*/

function DepthFirst(adjList, source) {
  var bfsInfo = [];
  
  adjList.forEach((item, index) => {
    bfsInfo[index] = {
      distance: null,
      predecessor: null
    };
  });

  bfsInfo[source].distance = 0;
  dfs(source);

  function dfs(source){
    adjList[source].forEach((item) => {
      if(bfsInfo[item].distance === null) {
        bfsInfo[item].predecessor = source;
        bfsInfo[item].distance = bfsInfo[source].distance + 1;        
        dfs(item);             
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

var result = DepthFirst(graph1, 0);
result.forEach((item, elem) =>  {
  console.log(
    `${elem}: distance: ${item.distance} predecessor: ${item.predecessor}`);
});


