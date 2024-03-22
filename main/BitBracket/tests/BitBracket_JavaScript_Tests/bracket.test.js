const $ = require('jquery');
global.$ = $;
global.alert = jest.fn();

jest.mock('jquery', () => {
  return jest.fn().mockImplementation(() => {
    return {
      ready: (cb) => cb(),
      on: (event, cb) => cb({ preventDefault: jest.fn() }),
      val: () => 'player1'
    };
  });
});

require('@js/bracket.js');

describe('Form submission', () => {
  test('should alert if less than 2 names are entered', () => {
    expect(global.alert).toHaveBeenCalledWith('You must enter at least 2 names.');
  });
});

const bracket = require('@js/bracket.js');

test('roundToPowerOfTwo calculates the next power of two and the number of nulls to add', () => {
  const names = ['Alice', 'Bob', 'Charlie', 'Dave', 'Eve'];
  const result = bracket.roundToPowerOfTwo(names);

  // Check that the size of the result array is 4 (i.e., there are 4 pairs of players)
  expect(result.length).toBe(4);
  // Check that each sub-array has a length of 2
  result.forEach(pair => {
    expect(pair.length).toBe(2);
  });

  // Flatten the array and count the number of null values
  const nullCount = result.flat().filter(item => item === null).length;
  expect(nullCount).toBe(3);
});

describe('createResultsArray', () => {
    it('should create a results array with the same structure as the teams array, but filled with zeroes or nulls', () => {
        const teams = [
            ['Alice', 'Bob'],
            ['Charlie', 'Dave'],
            [null, 'Eve']
        ];
        const expectedResults = [
            [0, 0],
            [0, 0],
            [null, 0]
        ];
        const results = bracket.createResultsArray(teams);
        expect(results).toEqual(expectedResults);
    });
});

test('names array gets scrambled when randomSeeding is true', () => {
    const names = ['Alice', 'Bob', 'Charlie', 'Dave'];
    const originalNames = [...names];
    const result = bracket.randomSeedNames(true, names);
    originalNames.forEach(name => {
        expect(result).toContain(name);
    });
    expect(result.length).toEqual(originalNames.length);
});

test('names array does not get scrambled when randomSeeding is false', () => {
    const names = ['Alice', 'Bob', 'Charlie', 'Dave'];
    const originalNames = [...names];
    const result = bracket.randomSeedNames(false, names);
    expect(result).toEqual(originalNames);
});

test('seeding function returns the same pattern every time', () => {
  const patterns = {
      8: [1, 8, 4, 5, 2, 7, 3, 6],
      16: [1, 16, 8, 9, 4, 13, 5, 12, 2, 15, 7, 10, 3, 14, 6, 11],
      32: [1, 32, 16, 17, 8, 25, 9, 24, 4, 29, 13, 20, 5, 28, 12, 21, 2, 31, 15, 18, 7, 26, 10, 23, 3, 30, 14, 19, 6, 27, 11, 22],
      64: [1, 64, 32, 33, 16, 49, 17, 48, 8, 57, 25, 40, 9, 56, 24, 41, 4, 61, 29, 36, 13, 52, 20, 45, 5, 60, 28, 37, 12, 53, 21, 44, 2, 63, 31, 34, 15, 50, 18, 47, 7, 58, 26, 39, 10, 55, 23, 42, 3, 62, 30, 35, 14, 51, 19, 46, 6, 59, 27, 38, 11, 54, 22, 43]
  };

  for (const numPlayers in patterns) {
      const expectedPattern = patterns[numPlayers];
      const result = bracket.seeding(parseInt(numPlayers));
      expect(result).toEqual(expectedPattern);
  }
});