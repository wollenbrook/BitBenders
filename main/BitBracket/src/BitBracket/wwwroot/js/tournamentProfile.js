function sendParticipationRequest(tournamentId, userId) {
    fetch(`/api/TournamentAPI/SendRequest/${userId}/${tournamentId}`, {
        method: 'POST'
    })
    .then(response => {
        if (response.ok) {
            alert('Request sent successfully!');
            checkParticipationStatus(tournamentId, userId);
        } else {
            alert('Failed to send request.');
        }
    });
}

function checkParticipationStatus(tournamentId, userId) {
    fetch(`/api/TournamentAPI/CheckParticipation/${userId}/${tournamentId}`)
        .then(response => response.json())
        .then(data => {
            const button = document.getElementById('participateButton');
            if (data.isParticipating) {
                button.classList.add('disabled');
                button.innerText = 'Participation Requested';
            } else {
                button.classList.remove('disabled');
                button.innerText = 'Send Request';
                button.addEventListener('click', () => sendParticipationRequest(tournamentId, userId));
            }
        });
}