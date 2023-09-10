/** @type {import("prettier").Config} */
module.exports = {
    endOfLine: 'auto',
    bracketSameLine: true,
    singleQuote: true,
    bracketSpacing: true,
    semi: true,
    plugins: [require.resolve('prettier-plugin-tailwindcss')],
    tailwindConfig: './tailwind.config.ts',
  };