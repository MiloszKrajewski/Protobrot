var path = require("path");
var CopyWebpackPlugin = require("copy-webpack-plugin");
var root = __dirname;

var config = {
	context: root,
	entry: [
		"babel-polyfill",
		"./Client/main.ts"
	],
	output: {
		path: path.join(root, "wwwroot"),
		filename: "bundle.js",
		publicPath: "/"
	},
	devtool: "source-map",
	resolve: {
		extensions: [".ts", ".tsx", ".js"]
	},
	module: {
		rules: [
			{ test: /\.tsx?$/, exclude: /node_modules/, loader: "awesome-typescript-loader?useBabel&useCache&cacheDirectory=./obj/.ts.cache" },
			{ test: /\.jsx?$/, exclude: /node_modules/, loader: "babel-loader" },
			{ test: /\.styl$/, exclude: /node_modules/, loader: "style-loader!css-loader?minimize=true!stylus-loader" },
			{ enforce: "pre", test: /\.(j|t)sx?$/, loader: "source-map-loader" }
		]
	},
	plugins: [
		new CopyWebpackPlugin([
			{ from: "Client/index.html" },
			{ from: "Client/favicon.ico" }
		])
	]
};

module.exports = config;