const path = require("path");
const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const OptimizeCSSPlugin = require("optimize-css-assets-webpack-plugin");
const bundleOutputDir = "./wwwroot/dist";
const VueLoaderPlugin = require("vue-loader/lib/plugin");

module.exports = () => {
  console.log("Building for \x1b[33m%s\x1b[0m", process.env.NODE_ENV);

  const isDevBuild = !(process.env.NODE_ENV && process.env.NODE_ENV === "production");
  const extractCSS = new ExtractTextPlugin("site.css");

  return [
    {
      stats: { modules: false },
      entry: { 'main': './ClientApp/boot-app.js' },
      resolve: {
        extensions: ['.js', '.vue'],
        alias: isDevBuild
          ? {
            'vue$': 'vue/dist/vue',
            'components': path.resolve(__dirname, './ClientApp/components'),
            'views': path.resolve(__dirname, './ClientApp/views'),
            'utils': path.resolve(__dirname, './ClientApp/utils'),
            'api': path.resolve(__dirname, './ClientApp/store/api')
          }
          : {
            'components': path.resolve(__dirname, './ClientApp/components'),
            'views': path.resolve(__dirname, './ClientApp/views'),
            'utils': path.resolve(__dirname, './ClientApp/utils'),
            'api': path.resolve(__dirname, './ClientApp/store/api')
          }
      },
      output: {
        path: path.join(__dirname, bundleOutputDir),
        filename: '[name].js',
        publicPath: '/dist/'
      },
      module: {
        rules: [
          { test: /\.vue$/, include: /ClientApp/, use: "vue-loader" },
          { test: /\.js$/, include: /ClientApp/, use: "babel-loader" },
          {
            test: /\.css$/,
            use: isDevBuild ? ["style-loader", "css-loader"] : ExtractTextPlugin.extract({ use: "css-loader" })
          },
          { test: /\.(png|jpg|gif|svg|woff|woff2|eot|ttf)$/, use: 'url-loader?limit=25000' }
        ]
      },
      plugins: [
        new VueLoaderPlugin(),
        new webpack.DllReferencePlugin({
          context: __dirname,
          manifest: require("./wwwroot/dist/vendor-manifest.json")
        })
      ].concat(isDevBuild
        ? [
          new webpack.SourceMapDevToolPlugin({
            filename: "[file].map",
            moduleFilenameTemplate: path.relative(bundleOutputDir, "[resourcePath]")
          })
        ]
        : [
          extractCSS,
          new OptimizeCSSPlugin({
            cssProcessorOptions: {
              safe: true
            }
          })
        ])
    }
  ];
}
