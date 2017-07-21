const median = require('../src/median.js');

const assert = require('assert');

describe("Median", () => {
    it("Lista com somente um item deve retorna o valor do item", () => {
        var list = [1];
        assert.equal(median(list), 1);
    });
    it("Lista com vários itens iguais deve retorna o valor do item", () => {
        var list = [1, 300, 2, 200, 1];
        assert.equal(median(list), 2);
    })
    it("Lista com número de itens impart deve retorna o valor do meio", () => {
        var list = [3, 6, 20, 99, 10, 15];
        assert.equal(median(list), 12.5);
    })
})