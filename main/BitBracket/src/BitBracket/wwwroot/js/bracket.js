$(document).ready(function() {
    setupAudioControls();

    $('#createBracketForm').on('submit', function(e) {
        e.preventDefault();
        var names = $('#Names').val().split(',');
        if (names.length < 2) {
            alert('You must enter at least 2 names.');
            return;
        }
        if ($('#Format').val() === 'Double Elimination' && names.length <= 2) {
            alert('Double elimination does not support 2 or less players.');
            return;
        }

        $('#generateButton').show();
        $("#captcha").show();
        var formData = {
            Names: $('#Names').val(),
            Format: $('#Format').val(),
            RandomSeeding: $('#RandomSeeding').is(':checked')
        };

        names = randomSeedNames(formData.RandomSeeding, names);
        var teams = roundToPowerOfTwo(names);

        if (teams.length > 0) {
            var results = createResultsArray(teams);
            var bracketFormat = setupBracket(formData.Format, teams, results);

            $('#generateButton').click(function() {
                generateBracketLink(bracketFormat);
            }).prop('disabled', false);

            initializeBracket(bracketFormat, teams);
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
    var rounds = Math.log(numPlayers) / Math.log(2) - 1;
    var pls = [1, 2];
    for (var i = 0; i < rounds; i++) {
        pls = nextLayer(pls);
    }
    function nextLayer(pls) {
        var out = [];
        var length = pls.length * 2 + 1;
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
    grecaptcha.ready(async function() {
        var recaptchaResponse = grecaptcha.getResponse();

        // Check if the reCAPTCHA has been completed
        if (recaptchaResponse.length === 0) {
            alert('Please complete the reCAPTCHA.');
            return;
        }
    
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
    });
}

function initializeBracket(bracketFormat, teams) {
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
    let avatarAssignments = {};
    teams.flat().forEach(player => {
        if (player && !avatarAssignments[player]) {
            const randomAvatar = avatarLinks[Math.floor(Math.random() * avatarLinks.length)];
            avatarAssignments[player] = randomAvatar;
        }
    });

    $('#minimal .demo').bracket({
        init: bracketFormat,
        save: (data) => {
            localStorage.setItem('bracketData', JSON.stringify(data));
            console.log("Bracket saved:", data);
        },
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
                    const avatar = avatarAssignments[data] ? avatarAssignments[data] : avatarLinks[Math.floor(Math.random() * avatarLinks.length)];
                    container.html(`<div class="team"><img src="${avatar}" class="avatar"/><span class="name">${data}</span></div>`);
                } else {
                    container.html('');
                }
            }
        },
        disableToolbar: true,
        disableTeamEdit: true
    });
}
