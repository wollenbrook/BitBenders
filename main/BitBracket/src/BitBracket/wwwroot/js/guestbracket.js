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
        let avatarAssignments = {};
        teams.flat().forEach(player => {
            if (player && !avatarAssignments[player]) {
                const randomAvatar = avatars[Math.floor(Math.random() * avatars.length)];
                avatarAssignments[player] = `${avatarFolder}${randomAvatar}`;
            }
        });

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
                        var input = $('<input type="text" class="ui input small">');
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
                            const avatar = avatarAssignments[data] ? avatarAssignments[data] : `${avatarFolder}${avatars[Math.floor(Math.random() * avatars.length)]}`;
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

    })
    .catch(error => {
        console.error('Error:', error);
        var errorMessage = document.createElement('p');
        errorMessage.textContent = 'Bracket has not been found, please check your link for typos, if problem presists we may be experiencing server issues.';
        errorMessage.className = 'error-message'; // Add the class to the element
        document.body.appendChild(errorMessage);
    });
