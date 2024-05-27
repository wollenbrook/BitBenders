// Get the tournament ID from the URL

const urlParams = new URLSearchParams(window.location.search);
const tournamentId = urlParams.get('id');

// Fetch brackets function
function fetchBrackets() {
    fetch('/api/TournamentAPI/bracket/' + tournamentId)
        .then(response => response.json())
        .then(brackets => {
            // Get the bracketsList element
            const bracketsList = document.getElementById('bracketsList');
            // Clear the bracketsList before adding new items
            bracketsList.innerHTML = '';

            // Create a container for each bracket
            brackets.forEach(bracket => {
                const container = document.createElement('div');
                container.className = 'ui segment';
                container.style.cursor = 'pointer'; // Change cursor to pointer on hover
                container.onclick = function() {
                    // Redirect to bracket page
                    window.location.href = '/Bracket/BracketView?id=' + bracket.id;
                };

                // Add bracket name and status to the container
                const name = document.createElement('div');
                name.className = 'ui header';
                name.textContent = bracket.name;
                container.appendChild(name);

                const status = document.createElement('p');
                status.className = 'ui sub header';
                status.textContent = `Status: ${bracket.status}`;
                container.appendChild(status);

                // Add the container to the bracketsList
                bracketsList.appendChild(container);
            });
        });
}