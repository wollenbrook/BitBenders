// Function to fetch and display announcements
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

// Function to display announcements
function displayAnnouncements(data) {
    const announcementsList = document.getElementById('announcementsList');

    if (!announcementsList) {
        console.warn('The announcements list element does not exist on this page.');
        return; // Exit the function early if the element is not found
    }

    announcementsList.innerHTML = ''; // Clear existing content

    // Specific color arrays for each category
    const backgroundColors = ['#131c6a', '#2e1467', '#4b1358', '#601736', '#6f1f1a'];
    const titleColors = ['#b6aabb', '#e4dbe7', '#a8b4aa', '#a4807f', '#8c7a93'];
    const descriptionColor = '#c5c0ac'; // Single color for descriptions
    const authorColor = '#a3b2b6'; // Single color for authors
    const whenColor = '#b6aabb'; // Single color for 'when'
    const strokeColors = ['#e4dbe7', '#b6aabb', '#8c7a93', '#9a7c85'];

    // Function to randomly select colors from arrays
    const getRandomColor = (colors) => colors[Math.floor(Math.random() * colors.length)];

    // Sort the announcements by CreationDate in descending order (newest first)
    const sortedAnnouncements = data.sort((a, b) => new Date(b.creationDate) - new Date(a.creationDate));

    sortedAnnouncements.forEach(a => {
        // Randomly selecting colors for background and stroke; fixed colors for other categories
        const bgColor = getRandomColor(backgroundColors);
        const strokeColor = getRandomColor(strokeColors);
        const titleColor = getRandomColor(titleColors);

        const card = document.createElement('div');
        card.className = 'ui raised card';
        card.style.backgroundColor = bgColor;
        card.style.borderColor = strokeColor;
        card.style.borderWidth = '4px'; 
        card.style.borderStyle = 'solid';
        card.style.boxShadow = '0 4px 8px 0 rgba(0, 0, 0, 0.2)'; // Add some shadow for depth
        card.innerHTML = `
            <div class="content">
                <div class="header" style="color: ${titleColor};">${a.title}</div>
                <div class="description" style="margin-top: 10px; color: ${descriptionColor};">
                    ${a.description}
                </div>
            </div>
            <div class="extra content" style="padding-top: 10px;">
                <div style="color: ${authorColor};"><strong>Author:</strong> ${a.author}</div>
                <div style="margin-top: 5px; color: ${whenColor};"><strong>When:</strong> ${new Date(a.creationDate).toLocaleDateString()} ${new Date(a.creationDate).toLocaleTimeString()}</div>
            </div>
        `;
        announcementsList.appendChild(card);
    });
}




// Initializing function to setup event listeners once DOM content is fully loaded
function initialize() {
    fetchAnnouncements(); // Initial fetch of announcements
}

document.addEventListener('DOMContentLoaded', initialize);
