/**
 * Union-Find Algorithm
 * A disjoint-set data structure is a data structure that keeps track of a set of elements
 * partitioned into a number of disjoint (non-overlapping) subsets.
 * A union-find algorithm is an algorithm that performs two useful operations on such a data structure:
 * Find: Determine which subset a particular element is in.
 *       This can be used for determining if two elements are in the same subset.
 * Union: Join two subsets into a single subset.
 * 
 * Resources: http://en.wikipedia.org/wiki/Disjoint-set_data_structure
 * 
 *  0
    |  \
    |    \
    1-----2
 */
//IMO: This approach is better than http://www.geeksforgeeks.org/union-find/
function UnionFindDetectedCycle(adjList, source) {

    function UnionFind(n) {
        let parent = [];

        for (var i = 0; i < n; i++) {
            parent[i] = i;
        }

        function root(p) {
            while (p != parent[p]) {
                // this approach is Path Compression.
                // It is recommended with tree isn't balanced
                p = parent[parent[p]];
            }
            return p;
        }

        return {
            union(p, q) {
                let rP = root(p);
                let rQ = root(q);
                parent[rP] = rQ;
            },
            connected(p, q) {
                return root(p) === root(q);
            }
        }
    }

    var uf = new UnionFind(adjList.length);

    function isCycle(edges, vertex) {

        if (edges.length === 0) return false;

        for (var i = 0; i < edges.length; i++) {
            let newVertex = edges[i];
            if (uf.connected(vertex, newVertex))
                return true;

            uf.union(vertex, newVertex);

            if (isCycle(adjList[i], i)) return true;
        }

        return false;
    }

    return isCycle(adjList[source], source);

}

//undirected
var graph = [
    [1, 2],
    [0, 2],
    [0, 1],
    []
];
console.log("Undirected")
console.log(UnionFindDetectedCycle(graph, 0) === true);
console.log(UnionFindDetectedCycle(graph, 1) === true);
console.log(UnionFindDetectedCycle(graph, 2) === true);
console.log(UnionFindDetectedCycle(graph, 3) === false);