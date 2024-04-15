document.addEventListener('DOMContentLoaded', function() {
    fetchAnnouncements();
    listenForThemeChanges(); // Listen for theme changes
});

function fetchAnnouncements() {
    fetch('/api/UserAnnouncementsApi/Published')
        .then(response => {
            if (!response.ok) {
                throw new Error('Problem fetching user announcements');
            }
            return response.json();
        })
        .then(data => displayAnnouncements(data))
        .catch(error => {
            console.error('Fetch Error:', error);
            alert('There is a problem with the application. It will be fixed soon!');
        });
}

function displayAnnouncements(data) {
    const announcementsList2 = document.getElementById('announcementsList2');
    announcementsList2.innerHTML = ''; // Clear existing content
    if (!data.length) {
        announcementsList2.innerHTML = '<p>No published announcements found.</p>';
        return;
    }
    
    data.forEach(announcement => {
        announcementsList2.appendChild(createAnnouncementCard(announcement));
    });
}

function createAnnouncementCard(announcement) {
    const theme = document.body.classList.contains('dark-theme') ? 'dark' : 'light';
    const { backgroundColors, titleColors, descriptionColor, authorColor, strokeColors } = getColorSchemes(theme);

    const card = document.createElement('div');
    card.className = 'ui raised card';
    card.style.backgroundColor = getRandomColor(backgroundColors);
    card.style.color = descriptionColor;
    card.style.borderColor = getRandomColor(strokeColors);
    card.style.borderWidth = '4px';
    card.style.borderStyle = 'solid';
    card.style.boxShadow = '0 4px 8px 0 rgba(0, 0, 0, 0.2)';
    
    card.innerHTML = `
        <div class="content">
            <div class="header" style="color: ${getRandomColor(titleColors)};">${announcement.title}</div>
            <div class="meta">${new Date(announcement.creationDate).toLocaleDateString()}</div>
            <div class="description">
                <p>${announcement.description}</p>
                <p><strong>Author:</strong> ${announcement.author}</p>
                <p><strong>Tournament:</strong> ${announcement.tournamentName || 'N/A'}</p>
            </div>
        </div>
    `;
    
    return card;
}

function getColorSchemes(theme) {
    return {
        light: {
            backgroundColors: ['#ff6761', '#ff9a20', '#ffe00e', '#97db24', '#49d271'],
            titleColors: ['#3243cf', '#8f2db1', '#0c91a7', '#19bcd2', '#cb54eb'],
            descriptionColor: '#1b5372',
            authorColor: '#4c5c16',
            strokeColors: ['#fff2b7', '#ffec95', '#e7c209', '#d0a604']
        },
        dark: {
            backgroundColors: ['#131c6a', '#2e1467', '#4b1358', '#601736', '#6f1f1a'],
            titleColors: ['#b6aabb', '#e4dbe7', '#a8b4aa', '#a4807f', '#8c7a93'],
            descriptionColor: '#c5c0ac',
            authorColor: '#a3b2b6',
            strokeColors: ['#e4dbe7', '#b6aabb', '#8c7a93', '#9a7c85']
        }
    }[theme];
}

function getRandomColor(colors) {
    return colors[Math.floor(Math.random() * colors.length)];
}

function listenForThemeChanges() {
    document.addEventListener('themeChanged', function() {
        const cards = document.querySelectorAll('.ui.card');
        cards.forEach(card => {
            const theme = document.body.classList.contains('dark-theme') ? 'dark' : 'light';
            const colors = getColorSchemes(theme);
            card.style.backgroundColor = getRandomColor(colors.backgroundColors);
            card.style.color = colors.descriptionColor;
            card.style.borderColor = getRandomColor(colors.strokeColors);
        });
    });
}
