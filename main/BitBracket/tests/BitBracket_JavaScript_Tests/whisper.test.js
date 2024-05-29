import '@testing-library/jest-dom'; // Import jest-dom for extended Jest matchers for the DOM.
import { initialize } from '@js/whisperTests'; // Import the initialize function from the whisper.js module.
import { JSDOM } from 'jsdom'; // Import JSDOM to create a simulated DOM for testing.

describe('Whisper API JavaScript', () => {
    let mediaRecorderMock, streamMock; // Declare variables for mocking MediaRecorder and media stream.
    let recordBtn, stopBtn, clearBtn, audioInput; // Declare variables for DOM elements.

    beforeEach(() => {
        // Setup the DOM with buttons and input field
        const dom = new JSDOM(`
            <button id="recordBtn">Record</button>
            <button id="stopBtn" disabled>Stop</button>
            <button id="clearBtn">Clear</button>
            <input id="audioInput" />
        `);

        global.document = dom.window.document; // Set the global document object to the simulated DOM.
        global.window = dom.window; // Set the global window object to the simulated window.
        global.navigator = {
            mediaDevices: {
                getUserMedia: jest.fn() // Mock the getUserMedia function to simulate accessing the microphone.
            }
        };

        mediaRecorderMock = {
            start: jest.fn(), // Mock the start function of MediaRecorder.
            stop: jest.fn(), // Mock the stop function of MediaRecorder.
            ondataavailable: jest.fn(), // Mock the ondataavailable event of MediaRecorder.
            onstop: jest.fn() // Mock the onstop event of MediaRecorder.
        };

        streamMock = {}; // Create a mock object for the media stream.
        navigator.mediaDevices.getUserMedia.mockResolvedValue(streamMock); // Make getUserMedia return the mock stream.
        global.MediaRecorder = jest.fn(() => mediaRecorderMock); // Mock the MediaRecorder constructor to return the mock MediaRecorder.

        recordBtn = document.getElementById('recordBtn'); // Get the record button from the DOM.
        stopBtn = document.getElementById('stopBtn'); // Get the stop button from the DOM.
        clearBtn = document.getElementById('clearBtn'); // Get the clear button from the DOM.
        audioInput = document.getElementById('audioInput'); // Get the audio input field from the DOM.

        // Initialize the module to set up event listeners
        initialize();
    });

    test('Record button starts recording', async () => {
        // Simulate clicking the record button
        recordBtn.click();
        await new Promise(setImmediate); // Wait for all promises to resolve

        // Check if getUserMedia was called with the correct parameters
        expect(navigator.mediaDevices.getUserMedia).toHaveBeenCalledWith({ audio: true });
        // Check if MediaRecorder was instantiated with the mock stream
        expect(global.MediaRecorder).toHaveBeenCalledWith(streamMock);
        // Check if the media recorder started recording
        expect(mediaRecorderMock.start).toHaveBeenCalled();
        // Check if the stop button is enabled and the record button is disabled
        expect(stopBtn.disabled).toBe(false);
        expect(recordBtn.disabled).toBe(true);
    });

    test('Stop button stops recording and sends audio data', async () => {
        const fakeBlob = new Blob(['fake audio data'], { type: 'audio/mp3' }); // Create a fake Blob to simulate audio data.
        const formData = new FormData(); // Create a new FormData object.
        formData.append('audioFile', fakeBlob); // Append the fake Blob to the FormData.

        // Mock the fetch function to simulate the API call
        global.fetch = jest.fn(() =>
            Promise.resolve({
                ok: true, // Simulate a successful response
                text: () => Promise.resolve('Transcription text') // Simulate a successful transcription response
            })
        );

        mediaRecorderMock.ondataavailable = jest.fn((e) => {
            e.data = fakeBlob; // Set the data property of the event to the fake Blob
        });

        // Simulate the recording process
        recordBtn.click(); // Click the record button
        await new Promise(setImmediate); // Wait for all promises to resolve

        // Simulate clicking the stop button
        stopBtn.click(); // Click the stop button
        mediaRecorderMock.onstop(); // Trigger the onstop event
        await new Promise(setImmediate); // Wait for all promises to resolve

        // Check if the media recorder stopped recording
        expect(mediaRecorderMock.stop).toHaveBeenCalled();
        // Check if fetch was called with the correct arguments
        expect(global.fetch).toHaveBeenCalledWith('/api/WhisperApi/transcribe', expect.objectContaining({
            method: 'POST',
            body: formData
        }));
        // Check if the transcription text was set in the input field
        expect(audioInput.value).toBe('Transcription text');
    });

    test('Clear button clears the input', () => {
        // Set an initial value for the input field
        audioInput.value = 'Some text';
        // Simulate clicking the clear button
        clearBtn.click();
        // Check if the input value was cleared
        expect(audioInput.value).toBe('');
    });
});
