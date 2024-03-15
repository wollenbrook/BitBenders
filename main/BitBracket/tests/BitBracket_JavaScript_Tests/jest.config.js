module.exports = {
    // Other Jest configurations...
    moduleFileExtensions: ['js', 'json', 'jsx', 'node'],
    moduleNameMapper: {
      '^@js/(.*)$': '<rootDir>/../../src/BitBracket/wwwroot/js/$1',
    },
    // Setup Jest to use jsdom environment
    testEnvironment: 'jest-environment-jsdom',
  };
  