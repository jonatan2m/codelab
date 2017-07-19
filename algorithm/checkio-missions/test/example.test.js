const assert = require('assert');

describe('Example Test', () => {
    describe('hello world', () => {
        it('should return "hello world"', () => {
            assert.equal('hello world', ['h', 'e', 'l', 'l', 'o',
                ' ', 'w', 'o', 'r', 'l', 'd'].join(''))
        })
    })
})
