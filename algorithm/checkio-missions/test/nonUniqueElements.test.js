const nonUniqueElements = require('../src/nonUniqueElements.js');

const assert = require('assert');

describe("Non-unique Elements", () => {
    it("Lista com apenas 1 elemento deve retonar lista vazia", () => {
        var elements = [1];
        assert.equal(nonUniqueElements(elements).length, 0);
    }),
        it("Lista com elementos únicos deve retonar lista vazia", () => {
            var elements = [1, 2, 3, 4, 5];
            assert.equal(nonUniqueElements(elements).length, 0);
        }),
        it("Lista com todos elementos repetidos deve retonar a mesma lista", () => {
            var elements = [5, 5, 5, 5, 5];
            assert.equal(
                nonUniqueElements(elements).toString(),
                [5, 5, 5, 5, 5]);
        }),
        it("Lista com alguns elemntos único, deve retornar nova lista sem eles.", () => {
            var elements = [1, 2, 3, 1, 3];
            assert.equal(
                nonUniqueElements(elements).toString(),
                [1, 3, 1, 3]);
        })
})
