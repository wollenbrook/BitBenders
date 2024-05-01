const urlParams = new URLSearchParams(window.location.search);
const bracketId = urlParams.get('id');

// Fetch bracket details
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
        // needs fixing
        // Save function
        function saveFn(data) {
            var json = JSON.stringify(data);
            localStorage.setItem('bracketData', json);
            console.log(data);

        // Prepare the data to send
        var dataToSend = {
            BracketId: bracket.id,
            BracketData: json
        };

        // Make the PUT request
        $.ajax({
            url: '/api/TournamentAPI/Bracket/Update',
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(dataToSend),
            success: function(response) {
                //console.log('Bracket data updated successfully');
                //console.log(response);
            },
            error: function(error) {
                console.error('Failed to update bracket data', error);
            }
        });
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