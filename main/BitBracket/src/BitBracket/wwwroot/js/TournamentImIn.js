function fetchTournaments(userId) {
    fetch(`/api/TournamentAPI/UserTournaments/${userId}`)
        .then(response => response.json())
        .then(data => {
            const container = document.getElementById('tournaments-container');
            if (data.length === 0) {
                document.getElementById('empty-placeholder').style.display = 'block';
            } else {
                data.forEach(tournament => {
                    const node = createTournamentElement(tournament, userId);
                    container.appendChild(node);
                });
            }
        })
        .catch(error => {
            console.error('Error fetching tournaments:', error);
            document.getElementById('empty-placeholder').style.display = 'block';
        });
}

function createTournamentElement(tournament, userId) {
    let div = document.createElement('div');
    div.className = 'item';
    div.innerHTML = `
        <div class="content">
            <h3 class="ui header">${tournament.name || 'Unnamed Tournament'}</h3>
            <p>${tournament.location || 'No location specified'}</p>
            <button onclick="withdraw(${tournament.id}, ${userId})" class="ui red button">Withdraw</button>
        </div>
    `;
    return div;
}

function withdraw(tournamentId, userId) {
    if (confirm('Are you sure you want to withdraw from this tournament?')) {
        fetch(`/api/TournamentAPI/Withdraw/${userId}/${tournamentId}`, { method: 'PUT' })
            .then(response => {
                if (response.ok) {
                    alert('You have successfully withdrawn from the tournament.');
                    fetchTournaments(userId); // Refresh the list
                } else {
                    alert('Failed to withdraw from the tournament.');
                }
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }
}
