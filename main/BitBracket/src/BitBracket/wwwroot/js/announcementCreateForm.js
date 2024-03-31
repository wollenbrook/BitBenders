// Function to handle form submission with validation
function handleFormSubmission() {
    const form = document.getElementById('createAnnouncementForm');
    
    form.addEventListener('submit', function(e) {
        e.preventDefault();

        // Validation flags
        let isValid = true;
        let errorMessage = '';

        // Retrieve input values
        const title = document.getElementById('title').value.trim();
        const description = document.getElementById('description').value.trim();
        const author = document.getElementById('author').value.trim();
        const adminKey = document.getElementById('adminKey').value.trim();

        // Validate Title
        if (title.length > 50) {
            errorMessage += 'Title should be no more than 50 characters.\n';
            isValid = false;
        }

        // Validate Description
        if (description.length > 500) {
            errorMessage += 'Description should be no more than 500 characters.\n';
            isValid = false;
        }

        // Validate Author
        if (author.length > 50) {
            errorMessage += 'Author should be no more than 50 characters.\n';
            isValid = false;
        }

        // If validation fails, alert the user and stop the submission
        if (!isValid) {
            alert(errorMessage);
            return;
        }

        // Proceed with form submission
        fetch(`/api/AnnouncementsApi?adminKey=${encodeURIComponent(adminKey)}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ title, description, author })
        })
        .then(response => {
            if (!response.ok) {
                if (response.status === 401) { 
                    throw new Error('Incorrect Admin Key. Please try again.');
                }
                throw new Error('An error occurred during the creation process.');
            }
            return response.json();
        })
        .then(() => {
            alert('Announcement created successfully!');
            form.reset();
        })
        .catch(error => {
            console.error('Submission Error:', error);
            alert(error.message); // Display more specific error messages based on the catch block logic
        });
    });
}

// Initializing function to setup event listeners once DOM content is fully loaded
function initialize() {
    handleFormSubmission(); // Setup form submission handling
}

document.addEventListener('DOMContentLoaded', initialize);

//Uncomment the below line for js testing!!
// export { initialize };
