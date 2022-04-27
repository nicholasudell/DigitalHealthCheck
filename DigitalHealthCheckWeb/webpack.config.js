const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: './wwwroot/js/site.js',
    output: {
        path: path.resolve(__dirname, 'wwwroot/dist'),
        hashFunction: "sha256",
        clean: true
    },
    plugins: [new MiniCssExtractPlugin()],
    module: {
        rules: [
            { //Compile SCSS
                test: /\.s[ac]ss$/i,
                use: [
                    // Creates minified CSS from CommonJS
                    MiniCssExtractPlugin.loader,
                    // Translates CSS into CommonJS
                    "css-loader",
                    // Compiles Sass to CSS
                    "sass-loader",
                ],
            },
            { //Handle images found in CSS
                test: /\.(png|svg|jpg|jpeg|gif)$/i,
                type: 'asset/resource',
            },
            { //Handle fonts found in CSS
                test: /\.(woff|woff2|eot|ttf|otf)$/i,
                type: 'asset/resource',
            },
        ],
    },
};