const urlParams = new URLSearchParams(window.location.search);
const guid = urlParams.get('id');

// Fetch bracket details
fetch(`/api/GuidAPI/${guid}`)
    .then(response => response.json())
    .then(guidBracket => {
    // Parse bracket.BracketData from JSON string to object
    const bracketData = JSON.parse(guidBracket.bracketData);
    console.log(bracketData);
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
        // update bracket data here
        
        // Prepare the data to send
        var dataToSend = {
            guid: guidBracket.guid,
            bracketData: json
        };

        // Make the PUT request
        $.ajax({
            url: '/api/GuidAPI',
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

    })
    .catch(error => {
        console.error('Error:', error);
        var errorMessage = document.createElement('p');
        errorMessage.textContent = 'Bracket has not been found, please check your link for typos, if problem presists we may be experiencing server issues.';
        errorMessage.className = 'text-styling large-text'; // Add the class to the element
        document.body.appendChild(errorMessage);
    });