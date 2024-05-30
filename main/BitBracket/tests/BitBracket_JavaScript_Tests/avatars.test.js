// bracket.test.js
import '@testing-library/jest-dom';
import { initializeBracket } from '@js/bracket';

// Define the list of avatar links
const avatarLinks = [
    'https://bitbracketimagestorage.blob.core.windows.net/images/joe.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/justen.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/chris.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/christian.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/daniel.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/elliot.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/elyse.png',
    'https://bitbracketimagestorage.blob.core.windows.net/images/jenny.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/laura.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/molly.png',
    'https://bitbracketimagestorage.blob.core.windows.net/images/nan.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/patrick.png',
    'https://bitbracketimagestorage.blob.core.windows.net/images/rachel.png',
    'https://bitbracketimagestorage.blob.core.windows.net/images/steve.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/stevie.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/tom.jpg',
    'https://bitbracketimagestorage.blob.core.windows.net/images/veronika.jpg'
];

// Test to ensure avatars are assigned to players correctly
test('assigns avatars to players correctly', () => {
    const teams = [
        ['Alice', 'Bob'],
        ['Charlie', 'Dave'],
        [null, null]
    ];
    const bracketFormat = {};

    const avatarAssignments = initializeBracket(bracketFormat, teams);

    // Check that each player has an assigned avatar
    Object.keys(avatarAssignments).forEach(player => {
        expect(avatarLinks).toContain(avatarAssignments[player]);
    });

    // Check that there are no null entries in the assignments
    expect(Object.keys(avatarAssignments)).not.toContain(null);
});

// Test to ensure avatars are not reassigned to the same player
// test('does not reassign avatars to the same player', () => {
//     const teams = [
//         ['Alice', 'Bob'],
//         ['Alice', 'Dave'],
//         ['Charlie', 'Bob']
//     ];
//     const bracketFormat = {};

//     const avatarAssignments = initializeBracket(bracketFormat, teams);

//     // Check that each player has a unique avatar
//     expect(Object.keys(avatarAssignments).length).toBe(4); // Alice, Bob, Charlie, Dave

//     // Check that each player is assigned only one avatar
//     const uniqueAvatars = new Set(Object.values(avatarAssignments));
//     expect(uniqueAvatars.size).toBe(4);
// });

// Test to ensure teams with null players are handled correctly
test('handles teams with null players correctly', () => {
    const teams = [
        ['Alice', null],
        [null, 'Dave'],
        [null, null]
    ];
    const bracketFormat = {};

    const avatarAssignments = initializeBracket(bracketFormat, teams);

    // Check that null players are not assigned avatars
    expect(Object.keys(avatarAssignments)).not.toContain(null);
    expect(Object.keys(avatarAssignments).length).toBe(2); // Alice and Dave
});

// Test to ensure avatars are assigned randomly
test('assigns avatars randomly', () => {
    const teams = [
        ['Alice', 'Bob'],
        ['Charlie', 'Dave']
    ];
    const bracketFormat = {};

    const avatarAssignments1 = initializeBracket(bracketFormat, teams);
    const avatarAssignments2 = initializeBracket(bracketFormat, teams);

    // Check that not all avatars are the same (randomness)
    expect(avatarAssignments1).not.toEqual(avatarAssignments2);
});
