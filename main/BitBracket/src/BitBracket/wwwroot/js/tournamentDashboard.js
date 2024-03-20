$(document).ready(function() {
    $('#tournamentForm').on('submit', function(e) {
        e.preventDefault();

        var tournamentName = $('#tournamentName').val();
        var tournamentLocation = $('#tournamentLocation').val();

        $.ajax({
            url: '/api/TournamentAPI/Create',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                Name: tournamentName,
                Location: tournamentLocation
            }),
            success: function(data) {
                // Handle success - you might want to add the new tournament to the #tournamentsList
                alert("Tournament Created");
                fetchTournaments(); // Fetch tournaments again after a new one is created
            },
            error: function(error) {
                // Handle error - you might want to display an error message
                alert("Failed to create tournament, try again later");
            }
        });
    });

    // Fetch tournaments function
    function fetchTournaments() {
        fetch('/api/TournamentAPI')
            .then(response => response.json())
            .then(tournaments => {
                // Get the tournamentsList element
                const tournamentsList = document.getElementById('tournamentsList');
                // Clear the tournamentsList before adding new items
                tournamentsList.innerHTML = '';

               // Create a container for each tournament
                tournaments.forEach(tournament => {
                    const container = document.createElement('div');
                    container.className = 'ui segment';
                    container.style.cursor = 'pointer'; // Change cursor to pointer on hover
                    container.onclick = function() {
                        // Redirect to tournament page
                        window.location.href = '/Bracket/TournamentPage?id=' + tournament.id;
                    };

                    // Add tournament name and status to the container
                    const name = document.createElement('div');
                    name.className = 'ui header';
                    name.textContent = tournament.name;
                    container.appendChild(name);

                    const status = document.createElement('p');
                    status.className = 'ui sub header';
                    status.textContent = `Status: ${tournament.status}`;
                    container.appendChild(status);

                    // Add the container to the tournamentsList
                    tournamentsList.appendChild(container);
                });
            });
    }

    // Fetch tournaments on page load
    fetchTournaments();
});