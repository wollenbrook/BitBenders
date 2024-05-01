document.addEventListener('DOMContentLoaded', function() {
    fetchTournaments();
});

function fetchTournaments() {
    fetch('/api/TournamentPlayersApi/tournaments')
        .then(response => response.json())
        .then(tournaments => displayTournaments(tournaments))
        .catch(error => console.error('Error fetching tournaments:', error));
}

function displayTournaments(tournaments) {
    const container = document.getElementById('tournamentsList');
    container.innerHTML = '';

    tournaments.forEach(tournament => {
        container.innerHTML += `
            <div class="ui card">
                <div class="content">
                    <div class="header">${tournament.name}</div>
                    <div class="meta">${tournament.location}</div>
                    <div class="description">
                        Hosted by: <strong>${tournament.ownerName.userName}</strong>
                    </div>
                </div>
                <div class="extra content">
                    <button class="ui primary button request-join" data-tournament-id="${tournament.id}">
                        Request to Join
                    </button>
                </div>
            </div>
        `;
    });

    document.querySelectorAll('.request-join').forEach(button => {
        button.addEventListener('click', function() {
            const tournamentId = this.getAttribute('data-tournament-id');
            joinTournament(tournamentId);
        });
    });
}

function joinTournament(tournamentId) {
    // This payload assumes you somehow get the player ID from session or a global JS variable set on login
    const playerData = { playerId: 'loggedInUserId', tournamentId: tournamentId }; // Ensure you retrieve the logged-in user's ID appropriately

    fetch('/api/TournamentPlayersApi/join', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(playerData)
    })
    .then(response => {
        if (!response.ok) throw new Error('Failed to join the tournament');
        alert('Request to join sent successfully!');
        fetchTournaments(); // Refresh list or update UI accordingly
    })
    .catch(error => {
        console.error('Error joining tournament:', error);
        alert('Error sending join request.');
    });
}
