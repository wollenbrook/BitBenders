document.addEventListener('DOMContentLoaded', function() {
    fetchAnnouncements('Drafts', 'draftsList');
    fetchAnnouncements('Publish', 'publishedList');
});

function fetchAnnouncements(type, elementId) {
    fetch(`/api/UserAnnouncementsApi/${type}`)
        .then(response => response.json())
        .then(data => displayAnnouncements(data, elementId, type))
        .catch(error => console.error('Error:', error));
}

function displayAnnouncements(announcements, elementId, type) {
    const container = document.getElementById(elementId);
    container.innerHTML = '';
    announcements.forEach(a => {
        const card = document.createElement('div');
        card.className = 'ui card';
        let buttonsHtml = `<button class="ui red button" onclick="deleteAnnouncement(${a.id})">Delete</button>`;
        if (type === 'Drafts') {
            buttonsHtml = `<button class="ui primary button" onclick="publishAnnouncement(${a.id})">Publish</button>` + buttonsHtml;
        }
        card.innerHTML = `
            <div class="content">
                <div class="header">${a.title}</div>
                <div class="description">${a.description}</div>
            </div>
            <div class="extra content">
                ${buttonsHtml}
            </div>
        `;
        container.appendChild(card);
    });
}

function publishAnnouncement(id) {
    fetch(`/api/UserAnnouncementsApi/Publish/${id}`, { method: 'POST' })
        .then(response => {
            if (response.ok) {
                alert('Announcement published successfully');
                location.reload();
            } else {
                alert('Failed to publish announcement');
            }
        })
        .catch(error => console.error('Error:', error));
}

function deleteAnnouncement(id) {
    fetch(`/api/UserAnnouncementsApi/Delete/${id}`, { method: 'DELETE' })
        .then(response => {
            if (response.ok) {
                alert('Announcement deleted successfully');
                location.reload();
            } else {
                alert('Failed to delete announcement');
            }
        })
        .catch(error => console.error('Error:', error));
}
