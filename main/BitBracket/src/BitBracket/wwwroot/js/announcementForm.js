//wwwroot/js/announcementForm.js

// Ensure DOM is fully loaded
document.addEventListener('DOMContentLoaded', function() {
    fetchAnnouncements(); // Initial fetch of announcements
    listenForThemeChanges();
});

function fetchAnnouncements() {
    fetch('/api/AnnouncementsApi')
        .then(response => {
            if (!response.ok) {
                throw new Error('Problem fetching announcements');
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
    const announcementsList = document.getElementById('announcementsList');
    if (!announcementsList) {
        console.warn('The announcements list element does not exist on this page.');
        return;
    }

    announcementsList.innerHTML = ''; // Clear existing content
    data.forEach(announcement => {
        announcementsList.appendChild(createAnnouncementCard(announcement));
    });
}

function createAnnouncementCard(announcement) {
    // Check for theme
    const theme = document.body.classList.contains('dark-theme') ? 'dark' : 'light';
    
    // Define color schemes for themes
    const colorSchemes = {
        light: {
            backgroundColors: ['#ff6761', '#ff9a20', '#ffe00e', '#97db24', '#49d271'],
            titleColors: ['#3243cf', '#8f2db1', '#0c91a7', '#19bcd2', '#cb54eb'],
            descriptionColor: '#1b5372',
            authorColor: '#4c5c16',
            whenColor: '#4c5c16',
            strokeColors: ['#fff2b7', '#ffec95', '#e7c209', '#d0a604']
        },
        dark: {
            backgroundColors: ['#131c6a', '#2e1467', '#4b1358', '#601736', '#6f1f1a'],
            titleColors: ['#b6aabb', '#e4dbe7', '#a8b4aa', '#a4807f', '#8c7a93'],
            descriptionColor: '#c5c0ac',
            authorColor: '#a3b2b6',
            whenColor: '#b6aabb',
            strokeColors: ['#e4dbe7', '#b6aabb', '#8c7a93', '#9a7c85']
        }
    };

    // Get colors based on the current theme
    const { backgroundColors, titleColors, descriptionColor, authorColor, whenColor, strokeColors } = colorSchemes[theme];

    // Function to randomly select colors from arrays
    const getRandomColor = (colors) => colors[Math.floor(Math.random() * colors.length)];

    // Construct the announcement card
    const card = document.createElement('div');
    card.className = 'ui raised card';
    card.style.backgroundColor = getRandomColor(backgroundColors);
    card.style.borderColor = getRandomColor(strokeColors);
    card.style.borderWidth = '4px';
    card.style.borderStyle = 'solid';
    card.style.boxShadow = '0 4px 8px 0 rgba(0, 0, 0, 0.2)';

    const titleColor = getRandomColor(titleColors);

    card.innerHTML = `
        <div class="content">
            <div class="header" style="color: ${titleColor};">${announcement.title}</div>
            <div class="description" style="color: ${descriptionColor};">
                ${announcement.description}
            </div>
        </div>
        <div class="extra content" style="color: ${authorColor};">
            <strong>Author:</strong> ${announcement.author}<br>
            <strong>When:</strong> ${new Date(announcement.creationDate).toLocaleDateString()} ${new Date(announcement.creationDate).toLocaleTimeString()}
        </div>
    `;

    return card;
}

function listenForThemeChanges() {
    // Listen for theme change event triggered in themeSwitcher.js
    document.addEventListener('themeChanged', function() {
        fetchAnnouncements(); // Re-fetch and display announcements with new theme colors
    });
}
