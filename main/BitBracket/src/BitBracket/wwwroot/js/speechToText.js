document.addEventListener('DOMContentLoaded', function() {
    const startBtn = document.getElementById('startRecord');
    const stopBtn = document.getElementById('stopRecord');
    const languageSelect = document.getElementById('languageSelect');
    const resultBox = document.getElementById('transcriptionResult');

    let mediaRecorder;
    let audioChunks = [];

    startBtn.addEventListener('click', () => {
        if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
            console.log('UserMedia supported.');
            navigator.mediaDevices.getUserMedia({ audio: true })
                .then(stream => {
                    mediaRecorder = new MediaRecorder(stream);
                    mediaRecorder.ondataavailable = e => audioChunks.push(e.data);
                    mediaRecorder.start();

                    startBtn.disabled = true;
                    stopBtn.disabled = false;
                })
                .catch(err => console.error('Error accessing media devices:', err));
        } else {
            console.error('getUserMedia not supported on your browser!');
        }
    });

    stopBtn.addEventListener('click', () => {
        mediaRecorder.stop();
        stopBtn.disabled = true;

        mediaRecorder.onstop = () => {
            const audioBlob = new Blob(audioChunks, { type: 'audio/wav' });
            const formData = new FormData();
            formData.append('audioFile', audioBlob);
            formData.append('language', languageSelect.value);

            fetch('/api/WhisperApi/transcribe', {
                method: 'POST',
                body: formData,
            })
            .then(response => response.text())
            .then(text => {
                resultBox.value = text;
                startBtn.disabled = false;
            })
            .catch(err => console.error('Transcription error:', err));
        };
    });
});
