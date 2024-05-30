function optIn() {
    fetch(`/api/UserAnnouncementsApi/OptIn`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.optInConfirmation) {
            document.getElementById("optInButton").classList.add("disabled");
            document.getElementById("optInButton").disabled = true;
            document.getElementById("optOutButton").classList.remove("disabled");
            document.getElementById("optOutButton").disabled = false;
        }
    })
    .catch(error => console.error('Error:', error));
}

function optOut() {
    fetch(`/api/UserAnnouncementsApi/OptOut`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {
        if (!data.optInConfirmation) {
            document.getElementById("optInButton").classList.remove("disabled");
            document.getElementById("optInButton").disabled = false;
            document.getElementById("optOutButton").classList.add("disabled");
            document.getElementById("optOutButton").disabled = true;
        }
    })
    .catch(error => console.error('Error:', error));
}
