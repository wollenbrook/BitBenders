const urlParams = new URLSearchParams(window.location.search);
const bracketId = urlParams.get('id');

// Fetch bracket details
function fetchBracketDetails() {
    fetch(`/api/TournamentAPI/bracket/display/${bracketId}`)
        .then(response => response.json())
        .then(bracket => {
            // Display bracket details
            document.getElementById('bracketName').textContent = bracket.name;
            document.getElementById('bracketStatus').textContent = bracket.status;
            //console.log(bracket);


            // Parse bracket.BracketData from JSON string to object
            const bracketData = JSON.parse(bracket.bracketData);
        
            // Separate teams and results
            const teams = bracketData.teams;
            const results = bracketData.results;
        
            // Create bracketFormat object
            const bracketFormat = {
                "teams": teams,
                "results": results
            };

            // Save function
            function saveFn(data) {
                var json = JSON.stringify(data);
                localStorage.setItem('bracketData', json);
                console.log(data);
            }

            $(function() {
                $('#minimal .demo').bracket({
                    init: bracketFormat,
                    save: saveFn,
                    disableToolbar: true,  // Not allowing resizing the bracket and changing its type
                    disableTeamEdit: true,  // Not allowing editing teams
                });
            });

            $('#bracket .score').addClass('score');

        });
}

// Fetch bracket details immediately and then every 15 seconds
fetchBracketDetails();
setInterval(fetchBracketDetails, 15000);

$(document).ready(function() {
    $('#backButton').click(function() {
      window.history.back();
    });
  });