/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["../**/*.{razor,cshtml,html}", "./node_modules/flowbite/**/*.js"],
  darkMode: "class",
  theme: {
    extend: {},
  },
  plugins: [require("flowbite/plugin")],
  safelist: ["bg-gray-900/50", "list-decimal", "list-disc", "overscroll-contain"],
};
