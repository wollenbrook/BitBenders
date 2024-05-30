const urlParams = new URLSearchParams(window.location.search);
const bracketId = urlParams.get('id');

// Fetch bracket details
fetch(`/api/TournamentAPI/bracket/display/${bracketId}`)
    .then(response => response.json())
    .then(bracket => {
        // Display bracket details
        document.getElementById('bracketName').textContent = bracket.name;
        document.getElementById('bracketStatus').textContent = bracket.status;

        // Parse bracket.BracketData from JSON string to object
        const bracketData = JSON.parse(bracket.bracketData);

        // Separate teams and results
        const teams = bracketData.teams;
        const results = bracketData.results;

        // Avatar configuration with links
        const avatarLinks = [
            'https://bitbracketimagestorage.blob.core.windows.net/images/joe.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/justen.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/chris.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/christian.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/daniel.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/elliot.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/elyse.png',
            'https://bitbracketimagestorage.blob.core.windows.net/images/jenny.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/laura.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/molly.png',
            'https://bitbracketimagestorage.blob.core.windows.net/images/nan.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/patrick.png',
            'https://bitbracketimagestorage.blob.core.windows.net/images/rachel.png',
            'https://bitbracketimagestorage.blob.core.windows.net/images/steve.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/stevie.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/tom.jpg',
            'https://bitbracketimagestorage.blob.core.windows.net/images/veronika.jpg'
        ];

        // Ensure avatar assignment
        if (!bracketData.avatarAssignments) {
            bracketData.avatarAssignments = {};
        }

        // Enhance team names with avatars
        for (let i = 0; i < teams.length; i++) {
            for (let j = 0; j < teams[i].length; j++) {
                if (teams[i][j]) {
                    if (!bracketData.avatarAssignments[teams[i][j]]) {
                        const randomAvatar = avatarLinks[Math.floor(Math.random() * avatarLinks.length)];
                        bracketData.avatarAssignments[teams[i][j]] = randomAvatar;
                    }
                }
            }
        }

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
            var dict = calculateTypeOfBracket(data);
            SavePlacements(dict);
            

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
                    console.log('Bracket data updated successfully');
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
                decorator: {
                    edit: function(container, data, doneCb) {
                        var input = $('<input type="text">');
                        input.val(data.name);
                        input.blur(function() {
                            var val = input.val();
                            if (val.length === 0) {
                                doneCb(null);  // Drop the team and remove the placeholder
                            } else {
                                data.name = val;
                                doneCb(data);
                            }
                        });
                        container.html(input);
                        input.focus();
                    },
                    render: function(container, data, score) {
                        if (data) {
                            const avatar = bracketData.avatarAssignments[data] ? bracketData.avatarAssignments[data] : avatarLinks[Math.floor(Math.random() * avatarLinks.length)];
                            container.html(`<div class="team"><img src="${avatar}" class="avatar"/><span class="name">${data}</span></div>`);
                        } else {
                            container.html('');
                        }
                    }
                },
                disableToolbar: true,  // Not allowing resizing the bracket and changing its type
                disableTeamEdit: true  // Not allowing editing teams
            });
        });
    });


function singleElimination(data) {
    var teams = data.teams;
    var results = data.results;
    var numberOfRounds = Math.log2(teams.length);
    var dictOfPlayerStandings = {};
    console.log(numberOfRounds);
    if (numberOfRounds === 0) {
        if (results[0][0][0][0] > results[0][0][0][1]) {
            winner = teams[0][0];
            second = teams[0][1];
        } else {
            winner = teams[0][1];
            second = teams[0][0];
        }
        dictOfPlayerStandings[1] = winner;
        dictOfPlayerStandings[2] = second;
        return dictOfPlayerStandings;

    }
    if (numberOfRounds === 1) {
        var finals = {};
        if (results[0][0][0][0] > results[0][0][0][1]) {
            var finalsR1 = teams[0][0];
            var fith = teams[0][1];
        }
        else {
            var finalsR1 = teams[0][1];
            var fith = teams[0][0];
        }
        if (results[0][0][1][0] > results[0][0][1][1]) {
            var finalsR2 = teams[1][0];
            var fourth = teams[1][1];
        } else {
            var finalsR2 = teams[1][1];
            var fourth = teams[1][0];
        }
        if (results[0][1][0][0] > results[0][1][0][1]) {
            var firstPlace = finalsR1;
            var secondPlace = finalsR2;
        } else {
            var firstPlace = finalsR2;
            var secondPlace = finalsR1;
        }
        if (results[0][1][1][0] > results[0][1][1][1]) {
            var thirdPlace = fith;
            var fourthPlace = fourth;
        } else {
            var thirdPlace = fourth;
            var fourthPlace = fith;
        }
        dictOfPlayerStandings[1] = firstPlace;
        dictOfPlayerStandings[2] = secondPlace;
        dictOfPlayerStandings[3] = thirdPlace;
        dictOfPlayerStandings[4] = fourthPlace;
        return dictOfPlayerStandings;
    }
    if (numberOfRounds === 2) {
        fithPlace = {}
        round2p1 = 'efa';
        round2p2 = 'efa';
        round2p3 = 'efa';
        round2p4 = 'efa';
        finalp1 = 'efa';
        finalp2 = 'efa';
        finalp3 = 'efa';
        finalp4 = 'efa';
        elimround2 = {}
        elimround3 = {}
        if (results[0][0][0][0] > results[0][0][0][1]) {
            fithPlace[1] = teams[0][1];
            var round2p1 = teams[0][0];
        } else {
            fithPlace[1] = teams[0][0];
            var round2p1 = teams[0][1];
        }
        if (results[0][0][1][0] > results[0][0][1][1]) {
            fithPlace[2] = teams[1][1];
            var round2p2 = teams[1][0];
        } else {
            fithPlace[2] = teams[1][0];
            var round2p2 = teams[1][1];
        }
        if (results[0][0][2][0] > results[0][0][2][1]) {
            fithPlace[3] = teams[2][1];
            var round2p3 = teams[2][0];
        } else {
            fithPlace[3] = teams[2][0];
            var round2p3 = teams[2][1];
        }
        if (results[0][0][3][0] > results[0][0][3][1]) {
            fithPlace[4] = teams[3][1];
            var round2p4 = teams[3][0];
        } else {
            fithPlace[4] = teams[3][0];
            var round2p4 = teams[3][1];
        } 
        //round 2
        if (results[0][1][0][0] > results[0][1][0][1]) {
            var finalp1 = round2p1;
            console.log(finalp1);
            var finalp3 = round2p2;
        } else {
            var finalp1 = round2p2;
            console.log(finalp1);
            var finalp3 = round2p1;
        }
        if (results[0][1][1][0] > results[0][1][1][1]) {
            var finalp2 = round2p3;
            console.log(finalp2);
            var finalp4 = round2p4;
        } else {
            var finalp2 = round2p4;
            console.log(finalp2);
            var finalp4 = round2p3;
        }
        //finals
        if (results[0][2][0][0] > results[0][2][0][1]) {
            var first = finalp1;
            console.log(first);
            var second = finalp2;
        } else {
            var first = finalp2;
            var second = finalp1;
        }
        if (results[0][2][1][0] > results[0][2][1][1]) {
            var third = finalp3;
            var fourth = finalp4;
        } else {
            var third = finalp4;
            var fourth = finalp3;
        }
        dictOfPlayerStandings[1] = first;
        dictOfPlayerStandings[2] = second;
        dictOfPlayerStandings[3] = third;
        dictOfPlayerStandings[4] = fourth;
        dictOfPlayerStandings[5] = fithPlace[1];
        dictOfPlayerStandings[6] = fithPlace[2];
        dictOfPlayerStandings[7] = fithPlace[3];
        dictOfPlayerStandings[8] = fithPlace[4];
        return dictOfPlayerStandings;
    }
    return 'nothing yet';


}
function calculateTypeOfBracket(data) {
    if (data.results.length === 1) {
        var dictOfStandings = singleElimination(data);
        return dictOfStandings;
    }
    else {
        return "Double Elimination";
    }
    var dictOfPlayers = {};
    return 'nothing';
}

function SavePlacements(dictionaryOfPlacemenets) {
    for (var key in dictionaryOfPlacemenets) {
        var Placement = key;

        const TournamentId = document.getElementById('TournyId').textContent;
        var BitUserName = dictionaryOfPlacemenets[key];
        if (key > 4) {
            Placement = 5;
        }
        $.ajax({
            url: `/api/BitUserApi/AddOrUpdatePlacement/${BitUserName}/${TournamentId}/${Placement}`,
            type: 'PUT',
            success: function(response) {
                console.log('Placement added successfully');
            },
            error: function(error) {
                console.error('Failed to add placement');
            }
        });
        
    }
}


    $(document).ready(function() {
        $('#backButton').click(function() {
          window.history.back();
        });
      });


