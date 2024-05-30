test('sortUsersBySkillLevel sorts users by skill level in descending order', () => {
    const users = {
        'Alice': 5,
        'Bob': 3,
        'Charlie': 4,
        'Dave': 2
    };
    const result = sortUsersBySkillLevel(users);
    expect(result).toContain('Alice');
    expect(result).toContain('Charlie');
    expect(result).toContain('Bob'); 
    expect(result).toContain('Dave');

    // Check that 'Alice' (skill level 5) comes before 'Charlie' (skill level 4)
    expect(result.indexOf('Alice')).toBeLessThan(result.indexOf('Charlie'));

    // Check that 'Charlie' (skill level 4) comes before 'Bob' (skill level 3)
    expect(result.indexOf('Charlie')).toBeLessThan(result.indexOf('Bob'));

    // Check that 'Bob' (skill level 3) comes before 'Dave' (skill level 2)
    expect(result.indexOf('Bob')).toBeLessThan(result.indexOf('Dave'));
});

test('sortUsersBySkillLevel sorts users with skill levels 1-8 in descending order', () => {
    const users = {
        'Alice': 8,
        'Bob': 7,
        'Charlie': 6,
        'Dave': 5,
        'Eve': 4,
        'Frank': 3,
        'Grace': 2,
        'Hank': 1
    };
    const result = sortUsersBySkillLevel(users);
    expect(result).toEqual(['Alice', 'Bob', 'Charlie', 'Dave', 'Eve', 'Frank', 'Grace', 'Hank']);
});

test('sortUsersBySkillLevel sorts users with same and different skill levels correctly', () => {
    const users = {
        'Alice': 8,
        'Bob': 8,
        'Charlie': 1,
        'Dave': 1
    };
    const result = sortUsersBySkillLevel(users);
    expect(result.indexOf('Alice')).toBeLessThan(2);
    expect(result.indexOf('Bob')).toBeLessThan(2);
    expect(result.indexOf('Charlie')).toBeGreaterThan(1);
    expect(result.indexOf('Dave')).toBeGreaterThan(1);
});

