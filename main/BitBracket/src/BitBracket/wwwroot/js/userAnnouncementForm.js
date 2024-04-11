// wwwroot/js/userAnnouncementForm.js

document.addEventListener('DOMContentLoaded', function() {
    fetch('/api/UserAnnouncementsApi')
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            const listElement = document.getElementById('announcementsList');
            if (Array.isArray(data) && data.length > 0) {
                data.forEach(announcement => {
                    const div = document.createElement('div');
                    div.innerHTML = `<h3>${announcement.title} (by ${announcement.author})</h3>
                                     <p>${announcement.description}</p>
                                     <small>${announcement.creationDate}</small>
                                     <br>
                                     <small>Status: ${announcement.isDraft ? 'Draft' : 'Published'}</small>`;
                    listElement.appendChild(div);
                });
            } else {
                listElement.innerHTML = '<p>No announcements found.</p>';
            }
        })
        .catch(error => {
            console.error('Error fetching announcements:', error);
            document.getElementById('announcementsList').innerHTML = '<p>Error loading announcements.</p>';
        });
});
