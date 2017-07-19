var path = require('path');

module.exports = {
    entry: './src/index.js',
    output: {
        filename: 'bundles.js',
        path: path.resolve(__dirname, 'dist')
    }
}