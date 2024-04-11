// wwwroot/js/optInConfirmation.js

document.addEventListener("DOMContentLoaded", function() {
    document.getElementById("saveOptInConfirmation").addEventListener("click", function() {
        const optInValue = document.getElementById("optInConfirmation").value === 'true';

        fetch('/api/UserAnnouncementsApi/OptInConfirmation', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                // Include authorization headers if necessary
            },
            body: JSON.stringify({ OptIn: optInValue })
        })
        .then(response => response.json())
        .then(data => {
            alert(data.message);
        })
        .catch(error => console.error('Error:', error));
    });
});
