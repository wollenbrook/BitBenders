//wwwroot/js/whisper.js

document.addEventListener('DOMContentLoaded', function () {
    const recordBtn = document.getElementById('recordBtn');
    const stopBtn = document.getElementById('stopBtn');
    const clearBtn = document.getElementById('clearBtn');
    const audioInput = document.getElementById('audioInput');

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
            });
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
                audioInput.value = resultText;
            } catch (error) {
                console.error('Error:', error);
                audioInput.value = error.message;
            }

            audioChunks = [];
            stopBtn.disabled = true;
            recordBtn.disabled = false;
        };
    });

    clearBtn.addEventListener('click', () => {
        console.log("Clear button clicked"); // Debug statement
        audioInput.value = ''; // Clears the input
    });
});

