document.addEventListener('DOMContentLoaded', function() {
    const startRecording = document.getElementById('startRecording');
    const stopRecording = document.getElementById('stopRecording');
    const languageSelector = document.getElementById('languageSelector');
    const targetInputId = document.getElementById('targetInputId'); // The ID of the input where the text should be inserted

    let mediaRecorder;
    let audioChunks = [];

    startRecording.addEventListener('click', async () => {
        audioChunks = [];
        const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
        mediaRecorder = new MediaRecorder(stream);

        mediaRecorder.ondataavailable = event => {
            audioChunks.push(event.data);
        };

        mediaRecorder.onstop = async () => {
            const audioBlob = new Blob(audioChunks);
            const formData = new FormData();
            formData.append("audioData", audioBlob);
            formData.append("language", languageSelector.value);

            try {
                const response = await fetch('/WhisperApi/ConvertSpeechToText', {
                    method: 'POST',
                    body: formData
                });
                if (response.ok) {
                    const text = await response.text();
                    document.getElementById(targetInputId.value).value = text; // Insert the text into the target input
                } else {
                    console.error('Speech to text failed', await response.text());
                }
            } catch (error) {
                console.error('Error:', error);
            }
        };

        mediaRecorder.start();
        startRecording.disabled = true;
        stopRecording.disabled = false;
    });

    stopRecording.addEventListener('click', () => {
        mediaRecorder.stop();
        startRecording.disabled = false;
        stopRecording.disabled = true;
    });
});
