//wwwroot/js/userAnnouncementCreate.js

document.addEventListener('DOMContentLoaded', function() {
    fetchTournaments(); // Load tournaments when the document is ready
    setupWhisperControls('title', 'recordBtnTitle', 'stopBtnTitle', 'clearBtnTitle');
    setupWhisperControls('description', 'recordBtnDescription', 'stopBtnDescription', 'clearBtnDescription');

    const form = document.getElementById('createAnnouncementForm');
    form.addEventListener('submit', function(event) {
        event.preventDefault();
        createAnnouncement();
    });
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

function fetchTournaments() {
    fetch('/api/UserAnnouncementsApi/Tournaments')
        .then(response => response.json())
        .then(tournaments => {
            const dropdown = document.getElementById('tournamentDropdown');
            tournaments.forEach(tournament => {
                const option = new Option(tournament.name, tournament.id);
                dropdown.appendChild(option);
            });
        })
        .catch(error => console.error('Error loading tournaments:', error));
}

function createAnnouncement() {
    const title = document.getElementById('title').value;
    const description = document.getElementById('description').value;
    const author = document.getElementById('author').value;
    const tournamentId = document.getElementById('tournamentDropdown').value;
    const isDraft = document.getElementById('statusDropdown').value === "true"; // Convert string to boolean

    const announcement = {
        title,
        description,
        author,
        tournamentId,
        isDraft
    };

    console.log(announcement); // Debugging to see the final object

    fetch('/api/UserAnnouncementsApi/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(announcement)
    })
    .then(response => {
        if (!response.ok) throw new Error('Failed to create the announcement');
        return response.json();
    })
    .then(data => {
        console.log(data); // Check server response
        alert('Announcement created successfully!');
        form.reset();
        window.location.href = '/Home/UserAnnouncementForm'; // Redirect back to UserAnnouncementForm
    })
    .catch(error => {
        console.error('Error creating announcement:', error);
        alert('Error creating announcement. Please try again.');
    });
}

