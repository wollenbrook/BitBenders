//wwwroot/js/tournament.js

document.addEventListener('DOMContentLoaded', function() {
    setupWhisperControls('BracketName', 'recordBtnBracketName', 'stopBtnBracketName', 'clearBtnBracketName');
    setupWhisperControls('playerNames', 'recordBtnPlayerNames', 'stopBtnPlayerNames', 'clearBtnPlayerNames');
    // Other initializations remain unchanged
});

function setupWhisperControls(inputId, recordBtnId, stopBtnId, clearBtnId) {
    const inputField = document.getElementById(inputId);
    const recordBtn = document.getElementById(recordBtnId);
    const stopBtn = document.getElementById(stopBtnId);
    const clearBtn = document.getElementById(clearBtnId);
    
    let mediaRecorder;
    let audioChunks = [];

    recordBtn.addEventListener('click', () => {
        navigator.mediaDevices.getUserMedia({ audio: true })
            .then(stream => {
                mediaRecorder = new MediaRecorder(stream);
                mediaRecorder.start();
                mediaRecorder.ondataavailable = event => {
                    audioChunks.push(event.data);
                };
                stopBtn.disabled = false;
                recordBtn.disabled = true;
            }).catch(error => console.error("Error accessing microphone: ", error));
    });

    stopBtn.addEventListener('click', () => {
        mediaRecorder.stop();
        mediaRecorder.onstop = async () => {
            const audioBlob = new Blob(audioChunks, { type: 'audio/mp3' });
            const formData = new FormData();
            formData.append('audioFile', audioBlob);

            try {
                const response = await fetch('/api/WhisperApi/transcribe', {
                    method: 'POST',
                    body: formData,
                });
                if (!response.ok) {
                    throw new Error(await response.text());
                }
                const resultText = await response.text();
                inputField.value = resultText;
            } catch (error) {
                console.error('Error:', error);
                inputField.value = error.message;
            }

            audioChunks = [];
            stopBtn.disabled = true;
            recordBtn.disabled = false;
        };
    });

    clearBtn.addEventListener('click', () => {
        inputField.value = '';
    });
}
// Get the tournament ID from the URL

const urlParams = new URLSearchParams(window.location.search);
const tournamentId = urlParams.get('id');

// Fetch tournament details
fetch(`/api/TournamentAPI/${tournamentId}`)
    .then(response => response.json())
    .then(tournament => {
        // Display tournament details
        document.getElementById('tournamentName').textContent = tournament.name;
        document.getElementById('tournamentLocation').textContent = tournament.location;
        document.getElementById('tournamentStatus').textContent = tournament.status;

        // Display tournament broadcast
        var broadcastType = tournament.broadcastType;
        var broadcastLink = tournament.broadcastLink
        if (broadcastType === 'Twitch') {
            new Twitch.Player("stream-embed", {
                channel: tournament.broadcastLink
              });
        } 
        else if (broadcastType === 'YouTube') {
            var youtubeEmbedUrl = `https://www.youtube.com/embed/live_stream?channel=${broadcastLink}`;
            var iframe = document.createElement('iframe');
            iframe.src = youtubeEmbedUrl;
            iframe.allowFullscreen = "";
            iframe.scrolling = "no";
            iframe.frameBorder = "0";
            iframe.allow = "autoplay; fullscreen";
            iframe.title = "YouTube";
            iframe.sandbox = "allow-modals allow-scripts allow-same-origin allow-popups allow-popups-to-escape-sandbox allow-storage-access-by-user-activation";
            document.getElementById('stream-embed').appendChild(iframe);
        } 
        else{
            document.getElementById('stream-embed').textContent = 'No broadcast available';
        }


        // Fetch user details
        fetch(`/api/TournamentAPI/User/${tournament.owner}`)
            .then(response => response.json())
            .then(user => {
                // Display user tag
                document.getElementById('userTag').textContent = user.tag;
            });
    });

    // Set Broadcast Link from the form
    $(document).ready(function() {
        $('#broadcastLinkForm').on('submit', function(e) {
            e.preventDefault();  // Prevent the form from being submitted in the traditional way
            var broadcastLink = $('#BroadcastLink').val(); // Broadcast Link string
            var broadcastType = $('#BroadcastType').val(); // Broadcast Type string
            if (!broadcastLink) {
                alert('You must enter a Twitch channel name or YouTube channel ID for the broadcast.');
                return;
            }
    
            var dataToSend = {
                NameOrID: broadcastLink,
                BroadcastType: broadcastType,
                TournamentId: tournamentId
            };
            //console.log(dataToSend);
    
            // Send the broadcastLink to the server
            $.ajax({
                url: '/api/TournamentAPI/Broadcast',
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(dataToSend),
                success: function(data) {
                    // Handle success - you might want to add the new bracket to the #bracketsList
                    alert("Broadcast Link Updated");
                    location.reload();
                },
                error: function(error) {
                    // Handle error - you might want to display an error message
                    alert("Failed to update broadcast link, try again later");
                }
            });
        });
    });
    /*
    function sortUsersBySkillLevel(users) {
        // Sort the users by skill level in descending order
        // Randomly order users with the same skill level
        users.sort((a, b) => {
            if (a.skillLevels === b.skillLevels) {
                // Randomly return -1 or 1
                return 0.5 - Math.random();
            }
            return b.skillLevels - a.skillLevels;
        });
    
        // Create a new list of usernames
        const usernames = users.map(user => user.username);
    
        // Return the list of usernames
        return usernames;
    }
    */

    // Get the BracketType select element
    const bracketTypeSelect = document.getElementById('BracketType');

    // Get the buttons
    const buttons = document.getElementsByClassName('hidetempb');

    // Add a hidden input field to the form
    let hiddenInput = document.createElement('input');
    hiddenInput.type = 'hidden';
    hiddenInput.id = 'hiddenInput';
    document.getElementById('createBracketForm').appendChild(hiddenInput);

    // Listen for changes on the BracketType select element
    bracketTypeSelect.addEventListener('change', function() {
        // Check if the selected option is 'Registered User Bracket'
        if (this.value === 'Registered User Bracket') {
            // Show Smart Seeding checkbox
            document.querySelector('.hidetemp').style.display = 'block';
             // Hide the Whisper buttons
            for(let button of buttons) {
                button.style.display = 'none';
            }
            // Fetch the registered users
            fetch(`/api/TournamentAPI/GetParticipates/${tournamentId}`)
                .then(response => response.json())
                .then(data => {
                    // Get the playerNames textarea
                    const playerNamesTextarea = document.getElementById('playerNames');

                    // Change the label text
                    playerNamesLabel.textContent = 'Registered Users (Seeded in Descending Order):';

                    // Clear the textarea
                    playerNamesTextarea.value = '';

                    // Populate the textarea with the fetched data
                    data.forEach(participant => {
                        playerNamesTextarea.value += participant.username + ', ';
                    });

                    // Remove the last comma and space
                    playerNamesTextarea.value = playerNamesTextarea.value.slice(0, -2);

                    // Update the hidden input value with the participant usernames
                    hiddenInput.value = playerNamesTextarea.value;
                    //console.log(hiddenInput.value);

                    // Make the textarea read-only
                    playerNamesTextarea.readOnly = true;

                    // Add draggable attribute to each name
                    let names = playerNamesTextarea.value.split(', ');

                    let playerNamesContainer = document.createElement('div');
                    playerNamesContainer.id = 'playerNamesContainer';
                    playerNamesContainer.style.display = 'flex'; // Add this line
                    playerNamesContainer.style.flexDirection = 'column'; // Add this line
                    const dictionaryThatHasNameAndWeight = {};

                    names.forEach(name => {
                        let span = document.createElement('span');
                        span.draggable = true;
                        span.textContent = name;
                        span.className = 'ui label ui-widget-content ui-corner-all'; // Add classes for jQuery UI styling
                        fetch(`/api/BitUserApi/GetEstimatedSkillLevel/${name}`)
                            .then(response => response.json())
                            .then(data => {
                                span.title = 'Estimated Skill Level:  ' + data;
                                dictionaryThatHasNameAndWeight[name] = data;
                            });
                        //this is the dictionary that has the name and weight associated with the user based on performance, not input from user
                        //console.log(dictionaryThatHasNameAndWeight);
                        // Create an input box for skill level
                        let input = document.createElement('input');
                        // Add CSS styles to the input box
                        input.style.width = '75px'; // Set the width of the input box
                        input.style.verticalAlign = 'middle'; // Align the input box vertically with the span
                        input.style.alignItems = 'right';
                        input.type = 'number';
                        input.min = 1;
                        input.max = 8;
                        input.value = 1;  // Default value
                        input.style.marginLeft = '10px';  // Add some space between the name and the input box
                        input.id = name;  // Set the id of the input box to the name of the player
                        // Append the input box to the span
                        span.appendChild(input);

                        playerNamesContainer.appendChild(span);
                    });
                    // Replace the textarea with the new container
                    playerNamesTextarea.replaceWith(playerNamesContainer);

                    // Make the names sortable and update the order when changed
                    $(playerNamesContainer).sortable({
                        update: function(event, ui) {
                            let updatedOrder = $(this).children().toArray().map(function(item) {
                                return item.textContent;
                            });
                            // Now updatedOrder is an array of usernames in the new order
                            // You can update your data or a hidden input field with this new order
                            hiddenInput.value = updatedOrder.join(', ');
                            playerNamesTextarea.value = hiddenInput.value;
                            //console.log(playerNamesTextarea.value)
                        }
                    });

                    // Apply jQuery UI styles to the name containers
                    $(playerNamesContainer).children().each(function() {
                        $(this).css({
                            'margin': '5px',
                            'padding': '5px',
                            'cursor': 'grab'
                        });
                    });
                });
        } else {
            // Hide Smart Seeding checkbox
            document.querySelector('.hidetemp').style.display = 'none';

            // Show the Whisper buttons
            for(let button of buttons) {
                button.style.display = 'inline-block';
            }

            // Change the label text back
            playerNamesLabel.textContent = 'Player Names (comma-delimited, In-Order Seeding):';

            // Get the playerNamesContainer
            const playerNamesContainer = document.getElementById('playerNamesContainer');

            // Create a new textarea
            let playerNamesTextarea = document.createElement('textarea');
            playerNamesTextarea.id = 'playerNames';

            // Replace the container with the new textarea
            playerNamesContainer.replaceWith(playerNamesTextarea);
        }
    });


    function sortUsersBySkillLevel(dictionaryWithPlayerNameAndWeight) {
        // Convert the dictionary to an array of objects
        const users = Object.keys(dictionaryWithPlayerNameAndWeight).map(username => {
            return {
                username: username,
                skillLevels: dictionaryWithPlayerNameAndWeight[username]
            };
        });
    
        // Sort the users by skill level in descending order
        // Randomly order users with the same skill level
        users.sort((a, b) => {
            if (a.skillLevels === b.skillLevels) {
                // Randomly return -1 or 1
                return 0.5 - Math.random();
            }
            return b.skillLevels - a.skillLevels;
        });
    
        // Create a new list of usernames
        const usernames = users.map(user => user.username);
    
        // Return the list of usernames
        return usernames;
    }


$(document).ready(function () {
        //var numAssociatedWithPlayerName = getElementById.name; This is the number associated with the player name based on input from user, which could be the recommended weight, or inputed weight
        dictionaryWithPlayerNameAndWeight = {};
        $('#createBracketForm').on('submit', function(e) {
            e.preventDefault();  // Prevent the form from being submitted in the traditional way
            // Create an array of player names
            if ($('#BracketType').val() === 'Registered User Bracket') {
                //var names = $('#hiddenInput').val().split(', ');
                if ($('#SmartSeedingAlgorithm').is(':checked')) {
                var names = $('#hiddenInput').val().split(', ');
                names.forEach(name => {
                    dictionaryWithPlayerNameAndWeight[name] = document.getElementById(name).value;
                })
                names = sortUsersBySkillLevel(dictionaryWithPlayerNameAndWeight);
                }
                else {
                    var names = $('#hiddenInput').val().split(', ');
                }
            }
            else {
            var names = $('#playerNames').val().split(',');
            }
            // Check if there are at least 2 names
            if (names.length < 2) {
                alert('You must enter at least 2 names.');
                return;
            }
            // Check if the format is 'Double Elimination' and there are only 2 players
            if ($('#Format').val() === 'Double Elimination' && names.length <= 2) {
                alert('Double elimination does not support 2 or less players.');
                return;
            }

            var bracketName = $('#BracketName').val(); // Bracket Name string
            if (!bracketName) {
                alert('You must enter a name for the bracket.');
                return;
            }

            if($('#BracketType').val() === 'Registered User Bracket'){
                var isRegisteredUserBracket = true;
            }
            else{
                var isRegisteredUserBracket = false;
            }
    
            var formData = {
                BracketName: $('#BracketName').val(), // Bracket Name string
                Names: $('#playerNames').val(), // Comma-delimited list of player names string
                Format: $('#Format').val(), // Single or Double Elimination string
                RandomSeeding: $('#RandomSeeding').is(':checked') // Random seeding boolean
            };
            //console.log(formData);
        
            
        // Create a format variable
        var format = formData.Format;

        // Create a RandomSeeding variable
        var randomSeeding = formData.RandomSeeding;

        // Creat a BracketName variable
        var bracketName = formData.BracketName;

        // If RandomSeeding is true, shuffle the player names
        if (randomSeeding) {
            names.sort(function() {
                // This function returns a random number between -0.5 and 0.5.
                // When used in the sort function, it results in a random order of the array elements.
                return 0.5 - Math.random();
            });
        }

        function roundToPowerOfTwo(names) {
            // Get the number of players
            const numPlayers = names.length;

            // Calculate the next power of two greater than or equal to numPlayers
            // This is the number of players we need for a balanced bracket
            const powerOfTwo = Math.pow(2, Math.ceil(Math.log2(numPlayers)));

            // Calculate how many null players we need to add to reach a power of two
            const nullsToAdd = powerOfTwo - numPlayers;

            // Add null players to the end of the player list
            const paddedPlayers = names.concat(Array(nullsToAdd).fill(null));

            // Generate the order of the teams for a balanced bracket
            const order = seeding(powerOfTwo);

            // Initialize an empty array to hold the teams
            const teams = [];

            // Split the players into teams for the first round of matches 
            // Each team consists of two players, and the order of the teams is determined by the order array
            for (let i = 0; i < powerOfTwo / 2; i++) {
                // Get the two players for this team from the paddedPlayers array
                // The indices of the players are determined by the order array
                const team = [paddedPlayers[order[i * 2] - 1], paddedPlayers[order[i * 2 + 1] - 1]];

                // Add the team to the teams array
                teams.push(team);
            }

            // Return the array of teams
            return teams;
        }

        function seeding(numPlayers){
            // Calculate the number of rounds needed for the given number of players
            // This is done by taking the base-2 logarithm of the number of players and subtracting 1
            var rounds = Math.log(numPlayers)/Math.log(2)-1;

            // Initialize the player seedings with the first two players
            var pls = [1,2];

            // Generate the seedings for each round
            for(var i=0;i<rounds;i++){
                // For each round, generate the next layer of seedings
                pls = nextLayer(pls);
            }

            // Return the final seedings
            return pls;

            // Function to generate the next layer of seedings
            function nextLayer(pls){
                var out=[];
                // Calculate the length of the next layer
                // This is done by doubling the length of the current layer and adding 1
                var length = pls.length*2+1;

                // For each player in the current layer
                pls.forEach(function(d){
                    // Add the player to the next layer
                    out.push(d);
                    // Add the opposite seeding to the next layer
                    // This is done by subtracting the player's seeding from the length
                    out.push(length-d);
                });

                // Return the next layer of seedings
                return out;
            }
        }

        // Create a results array with the same structure as the teams array, but filled with zeroes
        function createResultsArray(teams) {
            return teams.map(team => {
                return team.map(player => {
                    return player === null ? null : 0;
                });
            });
        }

        var teams = roundToPowerOfTwo(names);
        //console.log(teams);

        if (teams.length > 0) {
            var results = createResultsArray(teams);

            var singleElimination = {
                "teams": teams,
                "results": [results]  // Winners bracket
            };
            var doubleElimination = {
                teams: teams,
                results: [[results], [], []] // Winners bracket, Losers bracket
            };

            // Create a bracketFormat variable based on the format
            var bracketFormat;
            if (format === 'Single Elimination') {
                bracketFormat = singleElimination;
            } else if (format === 'Double Elimination') {
                bracketFormat = doubleElimination;
            }
            //console.log(bracketFormat);

            // Create a new JSON object that contains both the teams and results
            var bracketData = {
                teams: bracketFormat.teams,
                results: bracketFormat.results
            };
            
            var dataToSend = {
                bracketName: bracketName,
                tournamentId: tournamentId,
                bracketData: JSON.stringify(bracketData),
                isUserBracket: isRegisteredUserBracket
            };
            //console.log(dataToSend);

            // Send the bracketFormat to the server
            $.ajax({
                url: '/api/TournamentAPI/Create/Bracket',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(dataToSend),
                success: function(data) {
                    // Handle success - you might want to add the new bracket to the #bracketsList
                    alert("Bracket Created");
                   fetchBrackets(); // Fetch brackets again after a new one is created
                },
                error: function(error) {
                    // Handle error - you might want to display an error message
                    alert("Failed to create bracket, try again later");
                }
            });

        } 
        else {
            console.error("Javascript error: No teams available to initialize the bracket.");
        }
    })
});

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
                    window.location.href = '/Bracket/BracketPage?id=' + bracket.id;
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

// Fetch brackets on page load
fetchBrackets();