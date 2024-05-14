// This script handles fetching and managing participation requests and participants.

document.addEventListener('DOMContentLoaded', function () {
    fetchParticipationRequests();
    fetchApprovedParticipants();
    $('.ui.accordion').accordion(); // Initialize accordions
});

function fetchParticipationRequests() {
    const urlParams = new URLSearchParams(window.location.search);
    const tournamentId = urlParams.get('id');
    fetch(`/api/TournamentAPI/GetParticipateRequests/${tournamentId}`)
        .then(response => response.json())
        .then(data => {
            const container = document.getElementById('participationRequests');
            container.innerHTML = ''; // Clear previous entries
            data.forEach(request => {
                const node = document.createElement('div');
                node.innerHTML = `
                    <p>${request.senderUsername} - ${request.status}
                        <button onclick="acceptRequest(${request.requestId})">Approve</button>
                        <button onclick="declineRequest(${request.requestId})">Deny</button>
                    </p>
                `;
                container.appendChild(node);
            });
            $('.ui.accordion').accordion('refresh'); // Refresh accordion to account for new content
        });
}

function fetchApprovedParticipants() {
    const urlParams = new URLSearchParams(window.location.search);
    const tournamentId = urlParams.get('id');
    fetch(`/api/TournamentAPI/GetParticipates/${tournamentId}`)
        .then(response => response.json())
        .then(data => {
            const container = document.getElementById('approvedParticipants');
            container.innerHTML = ''; // Clear previous entries
            data.forEach(participant => {
                const node = document.createElement('div');
                node.innerHTML = `<p>${participant.Username}
                                    <button onclick="removeParticipant(${participant.UserId}, ${tournamentId})">Remove</button>
                                  </p>`;
                container.appendChild(node);
            });
            $('.ui.accordion').accordion('refresh'); // Refresh accordion to account for new content
        });
}

function acceptRequest(requestId) {
    fetch(`/api/TournamentAPI/AcceptRequest/${requestId}`, { method: 'PUT' })
        .then(response => {
            if (response.ok) {
                alert("Request accepted");
                fetchParticipationRequests(); // Refresh the list
            }
        });
}

function declineRequest(requestId) {
    fetch(`/api/TournamentAPI/DeclineRequest/${requestId}`, { method: 'PUT' })
        .then(response => {
            if (response.ok) {
                alert("Request declined");
                fetchParticipationRequests(); // Refresh the list
            }
        });
}

function removeParticipant(userId, tournamentId) {
    fetch(`/api/TournamentAPI/RemoveParticipate/${userId}/${tournamentId}`, { method: 'PUT' })
        .then(response => {
            if (response.ok) {
                alert("Participant removed");
                fetchApprovedParticipants(); // Refresh the list
            }
        });
}
