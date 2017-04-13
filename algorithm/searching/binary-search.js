/**
 * Algorithmic Paradigm: Divide and Conquer
 * 
 * Binary Search: Search a sorted array by repeatedly dividing the search interval in half.
 * Begin with an interval covering the whole array.
 * If the value of the search key is less than the item in the middle of the interval, narrow the interval to the lower half.
 * Otherwise narrow it to the upper half. Repeatedly check until the value is found or the interval is empty.
 * 
 * https://en.wikipedia.org/wiki/Binary_search_algorithm
 * 
 * Time Complexity: O(log n)
 */

function BinarySearch(arr, min, max, item) {

    if (min < max) {

        var mid = Math.floor((max + min) / 2);

        if (item === arr[mid]) return mid;
        else if (item < arr[mid]) return BinarySearch(arr, min, mid, item);
        else return BinarySearch(arr, mid + 1, max, item);
    }
    return -1;
}

var array = [-2, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

console.log(BinarySearch(array, 0, array.length - 1, 1) === 2);
console.log(BinarySearch(array, 0, array.length - 1, 0) === 1);
console.log(BinarySearch(array, 0, array.length - 1, 10) === -1);