/*
source: https://js.checkio.org/mission/median/
*/

module.exports = (list) => {
    list.sort((a, b) => { return a - b });

    var total = list.length;

    if (total % 2 === 0) {
        let mid = (total / 2) << 0;
        let midHigh = list[mid];
        let midLow = list[mid - 1];

        return midLow + ((midHigh - midLow) / 2);

    } else {
        let mid = (total / 2) << 0
        return list[mid];
    }
};

/**
 * Other Implementation:
 * function median(data) {
    data = data.sort((a, b) => a - b)
    n = Math.floor(data.length / 2)
    return (data[n] + data[data.length - 1 - n]) / 2
}
 */