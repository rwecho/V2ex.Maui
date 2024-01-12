/** @type {import('tailwindcss').Config} */
const plugin = require('tailwindcss/plugin')

module.exports = {
  content: ["../**/*.{razor,cshtml,html}", "./node_modules/flowbite/**/*.js"],
  darkMode: "class",
  theme: {
    extend: {},
  },
  plugins: [
    require("flowbite/plugin"),
    plugin(function ({ addVariant }) {
      addVariant('ios', `:is(.ios &)`)
      addVariant('android', `:is(.android &)`)
      addVariant('windows', `:is(.windows &)`)
    })
  ],
  safelist: ["bg-gray-900/50", "list-decimal", "list-disc", "overscroll-contain"],
};
