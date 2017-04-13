/**
 * Search an element in a sorted and rotated array
 */

//Content of binary-search.js
//I didn't use 'import' because it requires some configuration on env.
function BinarySearch(arr, min, max, item) {

    if (min <= max) {

        var mid = Math.floor((max + min) / 2);

        if (item === arr[mid]) return mid;
        else if (item < arr[mid]) return BinarySearch(arr, min, mid - 1, item);
        else return BinarySearch(arr, mid + 1, max, item);
    }
    return -1;
}

function BinarySearchRotatedArray(arr, min, max, item) {

    function getPivotRecursive(array, min, max) {

        if (min > max) return -1;
        if (min === max) return min;

        var mid = Math.floor((max + min) / 2);

        if (arr[mid] < arr[max] && arr[mid] < arr[min])
            return getPivotRecursive(arr, min, mid);
        else if (arr[mid] > arr[mid + 1] && arr[mid] > arr[mid - 1])
            return mid;
        else if (arr[mid] > arr[max] && arr[mid] > arr[min])
            return getPivotRecursive(arr, mid, max);
    }

    var pivot = getPivotRecursive(arr, min, max);

    if (item >= array[0]) return BinarySearch(arr, min, pivot, item);
    else if (item < array[0]) return BinarySearch(arr, pivot + 1, max, item);
}

var array = [4, 5, 6, 7, 8, 9, 1, 2, 3];

console.log(BinarySearchRotatedArray(array, 0, array.length - 1, 1) === 6);
console.log(BinarySearchRotatedArray(array, 0, array.length - 1, 10) === -1);
console.log(BinarySearchRotatedArray(array, 0, array.length - 1, 3) === 8);
console.log(BinarySearchRotatedArray(array, 0, array.length - 1, 4) === 0);
console.log(BinarySearchRotatedArray(array, 0, array.length - 1, 9) === 5);

array = [10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9];

console.log(BinarySearchRotatedArray(array, 0, array.length - 1, 1) === 3);
console.log(BinarySearchRotatedArray(array, 0, array.length - 1, 10) === 0);
console.log(BinarySearchRotatedArray(array, 0, array.length - 1, 3) === 5);
console.log(BinarySearchRotatedArray(array, 0, array.length - 1, 4) === 6);
console.log(BinarySearchRotatedArray(array, 0, array.length - 1, 9) === 11);