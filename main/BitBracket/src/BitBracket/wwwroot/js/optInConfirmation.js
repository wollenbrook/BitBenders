document.addEventListener('DOMContentLoaded', function () {
    fetchCurrentOptInStatus();
});

function fetchCurrentOptInStatus() {
    fetch('/api/UserAnnouncementsApi/GetOptInConfirmation')
    .then(response => response.json())
    .then(data => {
        const checkbox = document.getElementById('optInConfirmation');
        checkbox.checked = data.OptInConfirmation;
        updateLabel(data.OptInConfirmation);
    })
    .catch(error => {
        console.error('Error fetching Opt-In status:', error);
        document.getElementById('statusText').textContent = 'Error fetching current settings.';
    });
}

function updateOptInConfirmation() {
    const optIn = document.getElementById('optInConfirmation').checked;
    fetch('/api/UserAnnouncementsApi/UpdateOptInConfirmation', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(optIn)
    })
    .then(response => {
        if (response.ok) {
            updateLabel(optIn);
            alert('Notification settings updated successfully.');
        } else {
            throw new Error('Failed to update settings.');
        }
    })
    .catch(error => {
        console.error('Error updating Opt-In status:', error);
        alert(error.message);
    });
}

function updateLabel(isOptedIn) {
    const label = document.getElementById('optInLabel');
    const statusText = document.getElementById('statusText');
    label.textContent = isOptedIn ? 'Opt-In to Notifications:' : 'Opt-Out of Notifications:';
    statusText.textContent = isOptedIn ? 'You are currently opted in to receive notifications.' : 'You are currently opted out of notifications.';
}
