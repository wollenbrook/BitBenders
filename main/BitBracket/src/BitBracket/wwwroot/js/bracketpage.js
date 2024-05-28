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

        // Avatar configuration
        const avatarFolder = '/avatars/';
        const avatars = [
            'joe.jpg', 'justen.jpg', 'chris.jpg', 'christian.jpg',
            'daniel.jpg', 'elliot.jpg', 'elyse.png', 'jenny.jpg',
            'laura.jpg', 'molly.png', 'nan.jpg', 'patrick.png',
            'rachel.png', 'steve.jpg', 'stevie.jpg', 'tom.jpg',
            'veronika.jpg'
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
                        const randomAvatar = avatars[Math.floor(Math.random() * avatars.length)];
                        bracketData.avatarAssignments[teams[i][j]] = `${avatarFolder}${randomAvatar}`;
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
                            const avatar = bracketData.avatarAssignments[data] ? bracketData.avatarAssignments[data] : `${avatarFolder}${avatars[Math.floor(Math.random() * avatars.length)]}`;
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