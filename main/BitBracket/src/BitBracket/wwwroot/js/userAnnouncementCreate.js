document.addEventListener('DOMContentLoaded', function() {
    fetchTournaments(); // Load tournaments when the document is ready

    const form = document.getElementById('createAnnouncementForm');
    form.addEventListener('submit', function(event) {
        event.preventDefault();
        createAnnouncement();
    });
});

function fetchTournaments() {
    fetch('/api/UserAnnouncementsApi/Tournaments')
        .then(response => response.json())
        .then(tournaments => {
            const dropdown = document.getElementById('tournamentDropdown');
            tournaments.forEach(tournament => {
                const option = new Option(tournament.name, tournament.id);
                dropdown.appendChild(option);
            });
        })
        .catch(error => console.error('Error loading tournaments:', error));
}

function createAnnouncement() {
    const title = document.getElementById('title').value;
    const description = document.getElementById('description').value;
    const author = document.getElementById('author').value;
    const tournamentId = document.getElementById('tournamentDropdown').value;
    const isDraft = document.getElementById('statusDropdown').value === "true"; // Convert string to boolean

    const announcement = {
        title,
        description,
        author,
        tournamentId,
        isDraft
    };

    console.log(announcement); // Debugging to see the final object

    fetch('/api/UserAnnouncementsApi/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(announcement)
    })
    .then(response => {
        if (!response.ok) throw new Error('Failed to create the announcement');
        return response.json();
    })
    .then(data => {
        console.log(data); // Check server response
        alert('Announcement created successfully!');
    })
    .catch(error => {
        console.error('Error creating announcement:', error);
        alert('Error creating announcement. Please try again.');
    });
}

