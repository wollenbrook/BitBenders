document.addEventListener('DOMContentLoaded', function() {
    fetchAnnouncements('Drafts', 'draftsList');
    fetchAnnouncements('Publish', 'publishedList');
});

function fetchAnnouncements(type, elementId) {
    console.log(`Fetching ${type}`);
    fetch(`/api/UserAnnouncementsApi/${type}`)
        .then(response => response.json())
        .then(data => {
            console.log(`${type} data received:`, data);
            displayAnnouncements(data, elementId, type);
        })
        .catch(error => console.error(`Error fetching ${type}:`, error));
}

function displayAnnouncements(announcements, elementId, type) {
    const container = document.getElementById(elementId);
    container.innerHTML = '';
    announcements.forEach(a => {
        let buttonsHtml = `<button class="ui red button" onclick="deleteAnnouncement(${a.id})">Delete</button>`;
        if (type === 'Drafts') {
            buttonsHtml = `
                <button class="ui blue button" onclick="updateAnnouncement(${a.id})">Update</button>
                <button class="ui primary button" onclick="publishAnnouncement(${a.id})">Publish</button>` + buttonsHtml;
        }
        const card = document.createElement('div');
        card.className = 'ui card';
        card.innerHTML = `
            <div class="content">
                <div class="header">${a.title}</div>
                <div class="description">${a.description}</div>
            </div>
            <div class="extra content">${buttonsHtml}</div>
        `;
        container.appendChild(card);
    });
}

function updateAnnouncement(id) {
    console.log(`Updating announcement with ID: ${id}`);
    fetch(`/api/UserAnnouncementsApi/${id}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById('announcementId').value = data.id;
            document.getElementById('announcementTitle').value = data.title;
            document.getElementById('announcementDescription').value = data.description;
            $('#updateModal').modal('show');
        })
        .catch(error => console.error('Error:', error));
}

function saveUpdatedAnnouncement() {
    const id = document.getElementById('announcementId').value;
    const title = document.getElementById('announcementTitle').value;
    const description = document.getElementById('announcementDescription').value;

    const announcement = {
        id,
        title,
        description,
        isDraft: true
    };

    fetch(`/api/UserAnnouncementsApi/Update/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(announcement)
    })
    .then(response => {
        if (response.ok) {
            console.log('Announcement updated successfully');
            alert('Announcement updated successfully');
            location.reload();
        } else {
            response.text().then(text => {
                console.error('Failed to update announcement:', text);
                alert('Failed to update announcement: ' + text);
            });
        }
    })
    .catch(error => console.error('Error:', error));
}

function publishAnnouncement(id) {
    console.log(`Publishing announcement with ID: ${id}`);
    fetch(`/api/UserAnnouncementsApi/Publish/${id}`, { method: 'POST' })
        .then(response => {
            if (response.ok) {
                console.log('Announcement published successfully');
                alert('Announcement published successfully');
                location.reload();
            } else {
                response.text().then(text => {
                    console.error('Failed to publish announcement:', text);
                    alert('Failed to publish announcement: ' + text);
                });
            }
        })
        .catch(error => console.error('Error:', error));
}

function deleteAnnouncement(id) {
    console.log(`Deleting announcement with ID: ${id}`);
    fetch(`/api/UserAnnouncementsApi/Delete/${id}`, { method: 'DELETE' })
        .then(response => {
            if (response.ok) {
                console.log('Announcement deleted successfully');
                alert('Announcement deleted successfully');
                location.reload();
            } else {
                response.text().then(text => {
                    console.error('Failed to delete announcement:', text);
                    alert('Failed to delete announcement: ' + text);
                });
            }
        })
        .catch(error => console.error('Error:', error));
}
