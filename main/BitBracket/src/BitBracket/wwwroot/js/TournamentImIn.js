document.addEventListener('DOMContentLoaded', function() {
    fetchTournaments();
});

function fetchTournaments() {
    fetch('/api/ParticipateApi/GetParticipates')
        .then(response => response.json())
        .then(data => {
            const container = document.getElementById('tournaments-container');
            if (data.length === 0) {
                document.getElementById('empty-placeholder').style.display = 'block';
            } else {
                data.forEach(tournament => {
                    const tournamentElement = createTournamentElement(tournament);
                    container.appendChild(tournamentElement);
                });
            }
        })
        .catch(error => {
            console.error('Error fetching tournaments:', error);
            document.getElementById('empty-placeholder').style.display = 'block';
        });
}

function createTournamentElement(tournament) {
    let div = document.createElement('div');
    div.className = 'item';
    div.innerHTML = `
        <div class="content">
            <h3 class="ui header">${tournament.name || 'Unnamed Tournament'}</h3>
            <p>${tournament.location || 'No location specified'}</p>
            <button onclick="withdraw(${tournament.id})" class="ui red button">Withdraw</button>
        </div>
    `;
    return div;
}

function withdraw(tournamentId) {
    if (confirm('Are you sure you want to withdraw from this tournament?')) {
        fetch('/api/ParticipateApi/Withdraw/' + tournamentId, { method: 'PUT' })
            .then(response => {
                if (response.ok) {
                    alert('You have successfully withdrawn from the tournament.');
                    fetchTournaments(); // Refresh the list
                } else {
                    alert('Failed to withdraw from the tournament.');
                }
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }
}
