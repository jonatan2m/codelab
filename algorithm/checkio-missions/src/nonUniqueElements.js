/*
source: https://js.checkio.org/mission/non-unique-elements/
*/

module.exports = function (elements) {
    var result = [];

    while (elements.length != 0) {
        var current = elements.shift();
        if (elements.indexOf(current) != -1
            || result.indexOf(current) != -1)
            result.push(current);
    }

    return result;
}

/**
 * Other Implementation
 * 
 *  return data.filter(function(a){
        return data.indexOf(a) !== data.lastIndexOf(a)    
    });
 */
