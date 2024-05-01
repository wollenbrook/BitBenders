document.addEventListener('DOMContentLoaded', function () {
    const tournamentId = '@Model.Id';
    loadParticipationRequests(tournamentId);
    loadApprovedParticipants(tournamentId);
});

function loadParticipationRequests(tournamentId) {
    fetch(`/api/ParticipateApi/ParticipationRequests/${tournamentId}`)
        .then(response => response.json())
        .then(requests => {
            const container = document.getElementById('participationRequests');
            requests.forEach(request => {
                const requestElement = document.createElement('div');
                requestElement.innerHTML = `${request.senderUsername} - <button onclick="handleApprove(${request.id})">Approve</button> <button onclick="handleDeny(${request.id})">Deny</button>`;
                container.appendChild(requestElement);
            });
        });
}

function handleApprove(requestId) {
    fetch(`/api/ParticipateApi/AcceptRequest/${requestId}`, { method: 'PUT' })
        .then(response => response.ok ? alert('Request approved.') : alert('Failed to approve request.'));
}

function handleDeny(requestId) {
    fetch(`/api/ParticipateApi/DeclineRequest/${requestId}`, { method: 'PUT' })
        .then(response => response.ok ? alert('Request denied.') : alert('Failed to deny request.'));
}

function loadApprovedParticipants(tournamentId) {
    fetch(`/api/ParticipateApi/Participants/${tournamentId}`)
        .then(response => response.json())
        .then(participants => {
            const container = document.getElementById('approvedParticipants');
            participants.forEach(participant => {
                const participantElement = document.createElement('div');
                participantElement.textContent = participant.userUsername;
                container.appendChild(participantElement);
            });
        });
}
