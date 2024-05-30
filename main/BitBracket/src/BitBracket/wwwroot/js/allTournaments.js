document.addEventListener('DOMContentLoaded', function() {
    loadAllTournaments();
});

function loadAllTournaments() {
    fetch('/api/TournamentAPI/All/')
    .then(response => response.json())
    .then(tournaments => {
        const container = document.getElementById('tournamentsContainer');
        tournaments.forEach(t => {
            let card = document.createElement('div');
            card.innerHTML = `<h3>${t.name}</h3><p>${t.location}</p><p>Status: ${t.status}</p>`;
            card.onclick = () => window.location.href = `/Home/TournamentProfile?id=${t.id}`;
            container.appendChild(card);
        });
    })
    .catch(error => console.error('Error loading tournaments:', error));
}
