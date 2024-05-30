document.addEventListener('DOMContentLoaded', function() {
    handleFormSubmission();
    setupWhisperControls('title', 'recordTitleBtn', 'stopTitleBtn', 'clearTitleBtn');
    setupWhisperControls('description', 'recordDescBtn', 'stopDescBtn', 'clearDescBtn');
    setupWhisperControls('author', 'recordAuthorBtn', 'stopAuthorBtn', 'clearAuthorBtn');
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

function handleFormSubmission() {
    const form = document.getElementById('createAnnouncementForm');
    form.addEventListener('submit', function(e) {
        e.preventDefault();

        let isValid = true;
        let errorMessage = '';

        const title = document.getElementById('title').value.trim();
        const description = document.getElementById('description').value.trim();
        const author = document.getElementById('author').value.trim();
        const adminKey = document.getElementById('adminKey').value.trim();

        if (title.length > 50) {
            errorMessage += 'Title should be no more than 50 characters.\n';
            isValid = false;
        }

        if (description.length > 500) {
            errorMessage += 'Description should be no more than 500 characters.\n';
            isValid = false;
        }

        if (author.length > 50) {
            errorMessage += 'Author should be no more than 50 characters.\n';
            isValid = false;
        }

        if (!isValid) {
            alert(errorMessage);
            return;
        }

        fetch(`/api/AnnouncementsApi?adminKey=${encodeURIComponent(adminKey)}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ title, description, author })
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('An error occurred during the creation process.');
            }
            return response.json();
        })
        .then(() => {
            alert('Announcement created successfully!');
            form.reset();
            window.location.href = '/Home/Announcement'; // Redirect back to UserAnnouncementForm
        })
        .catch(error => {
            console.error('Submission Error:', error);
            alert(error.message);
        });
    });
}
