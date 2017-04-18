/**
 * Detect Cycle in a Directed Graph
 * Given a directed graph, check whether the graph contains a cycle or not
 * Depth First Traversal can be used to detect cycle in a Graph.
 * DFS for a connected graph produces a tree.
 * There is a cycle in a graph only if there is a back edge present in the graph.
 * A back edge is an edge that is from a node to itself (selfloop) or one of its
 * ancestor in the tree produced by DFS.
 * In the following graph, there are 3 back edges, marked with cross sign.
 * We can observe that these 3 back edges indicate 3 cycles present in the graph.
 * 
 * Resource:
 * http://www.geeksforgeeks.org/detect-cycle-in-a-graph/
 * 
 * 
 */

function DetectCycleDirectedGraph(adjList, source) {
    var bfsInfo = [];

    adjList.forEach((item, index) => {
        bfsInfo[index] = {
            visited: false,
            stacked: false
        };
    });

    return dfs(source);

    function dfs(source) {
        var items = adjList[source];

        if (bfsInfo[source].visited === false) {
            bfsInfo[source].visited = true;
            bfsInfo[source].stacked = true;

            for (var i = 0; i < items.length; i++) {
                var vertex = items[i];

                if (bfsInfo[vertex].visited === false && dfs(vertex))
                    return true;
                else if (bfsInfo[vertex].stacked)
                    return true;
            }
        }
        bfsInfo[source].stacked = false;
        return false;
    }
}

var graph = [
    [1, 2],
    [2],
    [0, 3],
    []
]
console.log(DetectCycleDirectedGraph(graph, 0) === true);
console.log(DetectCycleDirectedGraph(graph, 1) === true);
console.log(DetectCycleDirectedGraph(graph, 2) === true);
console.log(DetectCycleDirectedGraph(graph, 3) === false);