const urlParams = new URLSearchParams(window.location.search);
const bracketId = urlParams.get('id');

// Fetch bracket details CHANGE THIS
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
            "results": [results]  // Winners bracket
        };
        // needs fixing
        // Save function
        function saveFn(data) {
            var json = JSON.stringify(data);
            localStorage.setItem('bracketData', json);
            console.log(data);
            // update bracket data here
        }

        $(function() {
            $('#minimal .demo').bracket({
                init: bracketFormat,
                save: saveFn,
                disableToolbar: true,  // Not allowing resizing the bracket and changing its type
                disableTeamEdit: true  // Not allowing editing teams
            });
        });
    });