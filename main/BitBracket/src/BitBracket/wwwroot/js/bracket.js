$(document).ready(function() {
    setupAudioControls();

    $('#createBracketForm').on('submit', function(e) {
        e.preventDefault();
        var names = $('#Names').val().split(',');
        if (names.length < 2) {
            alert('You must enter at least 2 names.');
            return;
        }
        // Check if the format is 'Double Elimination' and there are only 2 players
        if ($('#Format').val() === 'Double Elimination' && names.length <= 2) {
            alert('Double elimination does not support 2 or less players.');
            return;
        }

        $('#generateButton').show();
        var formData = {
            Names: $('#Names').val(),
            Format: $('#Format').val(),
            RandomSeeding: $('#RandomSeeding').is(':checked')
        };

        var names = randomSeedNames(formData.RandomSeeding, names);
        var teams = roundToPowerOfTwo(names);

        if (teams.length > 0) {
            var results = createResultsArray(teams);
            var bracketFormat = setupBracket(formData.Format, teams, results);

            $('#generateButton').click(function() {
                generateBracketLink(bracketFormat);
            }).prop('disabled', false);

            initializeBracket(bracketFormat);
        } else {
            console.error("No teams available to initialize the bracket.");
        }
    });
});

function setupAudioControls() {
    const recordBtn = $('#recordBtn');
    const stopBtn = $('#stopBtn');
    const clearBtn = $('#clearBtn');
    const namesInput = $('#Names');

    let mediaRecorder;
    let audioChunks = [];

    recordBtn.click(function() {
        navigator.mediaDevices.getUserMedia({ audio: true }).then(stream => {
            mediaRecorder = new MediaRecorder(stream);
            mediaRecorder.start();
            mediaRecorder.ondataavailable = event => {
                audioChunks.push(event.data);
            };
            stopBtn.prop('disabled', false);
            recordBtn.prop('disabled', true);
        }).catch(error => console.error("Error accessing microphone: ", error));
    });

    stopBtn.click(function() {
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
                namesInput.val(resultText);
            } catch (error) {
                console.error('Error:', error);
            }

            audioChunks = [];
            stopBtn.prop('disabled', true);
            recordBtn.prop('disabled', false);
        };
    });

    clearBtn.click(function() {
        namesInput.val('');
    });
}

function randomSeedNames(randomSeeding, names) {
    if (randomSeeding) {
        names.sort(() => 0.5 - Math.random());
    }
    return names;
}

function createResultsArray(teams) {
    return teams.map(team => team.map(player => player === null ? null : 0));
}

function roundToPowerOfTwo(names) {
    const numPlayers = names.length;
    const powerOfTwo = Math.pow(2, Math.ceil(Math.log2(numPlayers)));
    const nullsToAdd = powerOfTwo - numPlayers;
    const paddedPlayers = names.concat(Array(nullsToAdd).fill(null));
    const order = seeding(powerOfTwo);
    const teams = [];
    for (let i = 0; i < powerOfTwo / 2; i++) {
        teams.push([paddedPlayers[order[i * 2] - 1], paddedPlayers[order[i * 2 + 1] - 1]]);
    }
    return teams;
}

function seeding(numPlayers) {
    var rounds = Math.log(numPlayers)/Math.log(2)-1;
    var pls = [1, 2];
    for(var i=0; i<rounds; i++) {
        pls = nextLayer(pls);
    }
    function nextLayer(pls) {
        var out = [];
        var length = pls.length*2+1;
        pls.forEach(d => {
            out.push(d);
            out.push(length - d);
        });
        return out;
    }
    return pls;
}

function setupBracket(format, teams, results) {
    var bracketFormat = {
        teams: teams,
        results: format === 'Single Elimination' ? [results] : [[results], [], []]
    };
    return bracketFormat;
}

function generateBracketLink(bracketFormat) {
    var dataToSend = {
        bracketData: JSON.stringify({ teams: bracketFormat.teams, results: bracketFormat.results })
    };

    console.log("Sending data:", dataToSend);

    $.ajax({
        url: '/api/GuidAPI',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(dataToSend),
        success: function(data) {
            var guidLink = data.guid;
            var baseUrl = window.location.origin;
            var fullUrl = baseUrl + '/bracket/guestbracketview?id=' + guidLink;
            var button = $('#generateButton');
            button.text(fullUrl);
            navigator.clipboard.writeText(fullUrl);
            button.off('click').click(function() {
                window.open(fullUrl, '_blank');
            });
        },
        error: function(error) {
            alert("Failed to create Link, try again later");
        }
    });
}

function initializeBracket(bracketFormat) {
    $('#minimal .demo').bracket({
        init: bracketFormat,
        save: (data) => {
            localStorage.setItem('bracketData', JSON.stringify(data));
            console.log("Bracket saved:", data);
        },
        disableToolbar: true,
        disableTeamEdit: true
    });
}


// $(document).ready(function() {
//     $('#createBracketForm').on('submit', function(e) {
//         e.preventDefault();  // Prevent the form from being submitted in the traditional way
//         // Create an array of player names
//         var names = $('#Names').val().split(',');
//         if (names.length < 2) {
//             alert('You must enter at least 2 names.');
//             return;
//         }

//         // Show the #generateButton
//         $('#generateButton').show();

//         var formData = {
//             Names: $('#Names').val(), // Comma-delimited list of player names string
//             Format: $('#Format').val(), // Single or Double Elimination string
//             RandomSeeding: $('#RandomSeeding').is(':checked') // Random seeding boolean
//         };

//         // Process the form data...

//         // Create a format variable
//         var format = formData.Format;

//         // Create a RandomSeeding variable
//         var randomSeeding = formData.RandomSeeding;

//         // If RandomSeeding is true, shuffle the player names
//         names = randomSeedNames(randomSeeding, names);

//         // Balance the number of players to a power of two
//         // This also does seeding order for the players with the seeding function seeding(numPlayers)
//         var teams = roundToPowerOfTwo(names);
        
//         //console.log(teams);

//         if (teams.length > 0) {
//               // Create a results array with the same structure as the teams array, but filled with zeroes
//             var results = createResultsArray(teams);

//             var singleElimination = {
//                 "teams": teams,
//                 "results": [results]  // Winners bracket
//             };
//             var doubleElimination = {
//                 teams: teams,
//                 results: [[results], [], []] // Winners bracket, Losers bracket
//             };


//             // Create a bracketFormat variable based on the format
//             var bracketFormat;
//             if (format === 'Single Elimination') {
//                 bracketFormat = singleElimination;
//             } else if (format === 'Double Elimination') {
//                 bracketFormat = doubleElimination;
//             }
//             //console.log(bracketFormat);

//             // Get the button element
//             var button = document.getElementById('generateButton');

//             // Attach a click event listener to the button
//             button.addEventListener('click', function() {
//                 generateBracketLink(bracketFormat);
//             }, {once: true});


//             // Save function
//             function saveFn(data) {
//                 var json = JSON.stringify(data);
//                 localStorage.setItem('bracketData', json);
//                 console.log(data);
//             }

//             $(function() {
//                 $('#minimal .demo').bracket({
//                     init: bracketFormat,
//                     save: saveFn,
//                     disableToolbar: true,  // Not allowing resizing the bracket and changing its type
//                     disableTeamEdit: true  // Not allowing editing teams
//                 });
//             });

//         } 
//         else {
//             console.error("No teams available to initialize the bracket.");
//         }
//     })
// });

// // needs to be turned into a function for proper testing
// function randomSeedNames(randomSeeding, names) {
//     if (randomSeeding) {
//         names.sort(function() {
//             return 0.5 - Math.random();
//         });
//     }
//     return names;
// }

// function createResultsArray(teams) {
//     return teams.map(team => {
//         return team.map(player => {
//             return player === null ? null : 0;
//         });
//     });
// }

// function seeding(numPlayers) {
//     // Calculate the number of rounds needed for the given number of players
//             // This is done by taking the base-2 logarithm of the number of players and subtracting 1
//             var rounds = Math.log(numPlayers)/Math.log(2)-1;

//             // Initialize the player seedings with the first two players
//             var pls = [1,2];

//             // Generate the seedings for each round
//             for(var i=0;i<rounds;i++){
//                 // For each round, generate the next layer of seedings
//                 pls = nextLayer(pls);
//             }

//             // Return the final seedings
//             return pls;

//             // Function to generate the next layer of seedings
//             function nextLayer(pls){
//                 var out=[];
//                 // Calculate the length of the next layer
//                 // This is done by doubling the length of the current layer and adding 1
//                 var length = pls.length*2+1;

//                 // For each player in the current layer
//                 pls.forEach(function(d){
//                     // Add the player to the next layer
//                     out.push(d);
//                     // Add the opposite seeding to the next layer
//                     // This is done by subtracting the player's seeding from the length
//                     out.push(length-d);
//                 });

//                 // Return the next layer of seedings
//                 return out;
//             }
// }

// function roundToPowerOfTwo(names) {
//      // Get the number of players
//      const numPlayers = names.length;

//      // Calculate the next power of two greater than or equal to numPlayers
//      // This is the number of players we need for a balanced bracket
//      const powerOfTwo = Math.pow(2, Math.ceil(Math.log2(numPlayers)));

//      // Calculate how many null players we need to add to reach a power of two
//      const nullsToAdd = powerOfTwo - numPlayers;

//      // Add null players to the end of the player list
//      const paddedPlayers = names.concat(Array(nullsToAdd).fill(null));

//      // Generate the order of the teams for a balanced bracket
//      const order = seeding(powerOfTwo);

//      // Initialize an empty array to hold the teams
//      const teams = [];

//      // Split the players into teams for the first round of matches
//      // Each team consists of two players, and the order of the teams is determined by the order array
//      for (let i = 0; i < powerOfTwo / 2; i++) {
//          // Get the two players for this team from the paddedPlayers array
//          // The indices of the players are determined by the order array
//          const team = [paddedPlayers[order[i * 2] - 1], paddedPlayers[order[i * 2 + 1] - 1]];

//          // Add the team to the teams array
//          teams.push(team);
//      }

//      // Return the array of teams
//      return teams;
// }

// function generateBracketLink(bracketFormat) {
//     var button = document.getElementById('generateButton');
    
//     // Create a new JSON object that contains both the teams and results
//     var bracketData = {
//         teams: bracketFormat.teams,
//         results: bracketFormat.results
//     };
    
//     var dataToSend = {
//         bracketData: JSON.stringify(bracketData)
//     };

//     console.log(dataToSend);

//     // Send the bracketFormat to the server
//     $.ajax({
//         url: '/api/GuidAPI',
//         type: 'POST',
//         contentType: 'application/json',
//         data: JSON.stringify(dataToSend),
//         success: function(data) {
//             console.log(data);
//             // Handle success
//             // Retrieve the GUID link
//             var guidLink = data.guid;
//             // Create the full URL
//             var baseUrl = window.location.origin;
//             var fullUrl = baseUrl + '/bracket/guestbracketview?id=' + guidLink;
//             button.innerHTML = fullUrl;
//             navigator.clipboard.writeText(fullUrl);
//             $.ajax({
//                 url: '/api/GuidAPI',
//                 type: 'POST',
//                 contentType: 'application/json',
//                 data: JSON.stringify(dataToSend),
//                 success: function(data) {
//                     console.log(data);
//                     // Handle success
//                     // Retrieve the GUID link
//                     var guidLink = data.guid;
//                     // Create the full URL
//                     var baseUrl = window.location.origin;
//                     var fullUrl = baseUrl + '/bracket/guestbracketview?id=' + guidLink;
//                     button.innerHTML = fullUrl;
//                     navigator.clipboard.writeText(fullUrl);
//                     // Add an event listener to the button
//                     button.addEventListener('click', function() {
//                         window.open(fullUrl, '_blank');
//                     });
//                 },
//                 error: function(error) {
//                     // Handle error 
//                     alert("Failed to create Link, try again later");
//                 }
//             });
//         },
//         error: function(error) {
//             // Handle error 
//             alert("Failed to create Link, try again later");
//         }
//     });
// }

// // Exported functions for testing

// /*if (typeof module !== 'undefined' && module.exports) {
//     module.exports = {
//         createResultsArray,
//         roundToPowerOfTwo,
//         seeding,
//         randomSeedNames
//     };
//     }
// */
