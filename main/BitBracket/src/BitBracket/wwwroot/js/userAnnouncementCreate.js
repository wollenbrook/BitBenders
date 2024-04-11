// Located at wwwroot/js/userAnnouncementCreate.js

document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('userAnnouncementCreateForm').addEventListener('submit', function(e) {
        e.preventDefault();

        const announcement = {
            title: document.getElementById('title').value,
            description: document.getElementById('description').value,
            tournamentId: document.getElementById('tournament').value,
            creationDate: document.getElementById('date').value,
            isDraft: document.getElementById('isDraft').checked
        };

        fetch('/api/UserAnnouncementsApi', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(announcement)
        }).then(response => {
            if (response.ok) {
                alert('Announcement saved successfully.');
                window.location.reload();
            } else {
                alert('Failed to save the announcement.');
            }
        }).catch(error => {
            console.error('Error:', error);
        });
    });

    // Populate the tournament dropdown (implementation depends on your API)
});
