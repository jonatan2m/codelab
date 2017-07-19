const path = require('path');
const CleanWebpackPlugin = require('clean-webpack-plugin');

module.exports = {
    entry: {
        index: './src/index.js',
        print: './src/print.js'
    },
    plugins: [
        new CleanWebpackPlugin(['dist'])
    ],
    output: {
        filename: 'bundles.js',
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, 'dist')
    }
}