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
                if (request.status === 'Pending') { // Only show pending requests
                    const node = document.createElement('div');
                    node.innerHTML = `
                        <p>${request.senderUsername} - ${request.status}
                            <button onclick="acceptRequest(${request.requestId}, ${tournamentId})">Approve</button>
                            <button onclick="denyRequest(${request.requestId}, ${tournamentId})">Deny</button>
                        </p>
                    `;
                    container.appendChild(node);
                }
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
                node.innerHTML = `<p>${participant.username}
                                    <button onclick="removeParticipant(${participant.userId}, ${tournamentId})">Remove</button>
                                  </p>`;
                container.appendChild(node);
            });
            $('.ui.accordion').accordion('refresh'); // Refresh accordion to account for new content
        });
}

function acceptRequest(requestId, tournamentId) {
    fetch(`/api/TournamentAPI/AcceptRequest/${requestId}`, { method: 'PUT' })
        .then(response => {
            if (response.ok) {
                alert("Request accepted");
                fetchParticipationRequests(tournamentId);
                fetchApprovedParticipants(tournamentId); // Refresh the lists
            }
        });
}

function denyRequest(requestId, tournamentId) {
    fetch(`/api/TournamentAPI/DenyOrRemoveRequest/${requestId}`, { method: 'PUT' })
        .then(response => {
            if (response.ok) {
                alert("Request denied");
                fetchParticipationRequests(tournamentId); // Refresh the list
            }
        });
}

function removeParticipant(userId, tournamentId) {
    fetch(`/api/TournamentAPI/RemoveParticipant/${userId}/${tournamentId}`, { method: 'PUT' })
        .then(response => {
            if (response.ok) {
                alert("Participant removed");
                fetchApprovedParticipants(tournamentId); // Refresh the list
            }
        });
}
