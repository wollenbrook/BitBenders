document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('createAnnouncementForm');
    const titleInput = document.getElementById('title');
    const descriptionInput = document.getElementById('description');
    const authorInput = document.getElementById('author');
    const adminKeyInput = document.getElementById('adminKey');
    const errorMessageDisplay = document.getElementById('errorMessage'); // Add this element to your HTML

    // Function to fetch and display announcements
    function fetchAnnouncements() {
        fetch('/api/AnnouncementsApi')
            .then(response => response.json())
            .then(data => displayAnnouncements(data))
            .catch(error => console.error('Fetch Error:', error));
    }

    function displayAnnouncements(data) {
        const announcementsList = document.getElementById('announcementsList');
        announcementsList.innerHTML = ''; // Clear existing content
    
        // Sort announcements by CreationDate, newest first
        const sortedAnnouncements = data.sort((a, b) => new Date(b.creationDate) - new Date(a.creationDate));
    
        sortedAnnouncements.forEach(a => {
            const card = document.createElement('div');
            card.className = 'card mb-3'; // Bootstrap card class
    
            const cardBody = document.createElement('div');
            cardBody.className = 'card-body';
    
            const title = document.createElement('h5');
            title.className = 'card-title';
            title.textContent = `${a.title}`;
    
            const description = document.createElement('p');
            description.className = 'card-text';
            description.textContent = `Description: ${a.description}`;
    
            const cardFooter = document.createElement('div');
            cardFooter.className = 'card-footer text-muted d-flex justify-content-between align-items-center';
    
            const author = document.createElement('span');
            author.textContent = `Author: ${a.author}`;
    
            const time = document.createElement('span');
            // Parse the CreationDate and format it for display
            const creationdate = new Date(a.creationDate);
            time.textContent = `When: ${creationdate.toLocaleDateString()} ${creationdate.toLocaleTimeString()}`;
    
            // Assemble the card
            cardFooter.appendChild(author);
            cardFooter.appendChild(time);
            cardBody.appendChild(title);
            cardBody.appendChild(description);
            card.appendChild(cardBody);
            card.appendChild(cardFooter);
            announcementsList.appendChild(card);
        });
    }
    

    // Handle form submission
    form.addEventListener('submit', function(e) {
        e.preventDefault();

        // Basic validation
        if (!titleInput.value.trim()) {
            errorMessageDisplay.textContent = 'Title is required.';
            return;
        }

        const formData = {
            title: titleInput.value.trim(),
            description: descriptionInput.value.trim(),
            author: authorInput.value.trim(),
        };
        const adminKey = adminKeyInput.value.trim();

        fetch(`/api/AnnouncementsApi?adminKey=${encodeURIComponent(adminKey)}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(formData)
        })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else if (response.status === 401) {
                throw new Error('Wrong admin key. Try again, please.');
            } else if (response.status === 400) {
                throw new Error('Please check your input and try again.');
            } else {
                throw new Error('An error occurred. Please try again later.');
            }
        })
        .then(() => {
            // Show a success message
            alert('Announcement posted successfully!');
            // Clear the form for a new submission
            document.getElementById('createAnnouncementForm').reset();
            // Optionally, refresh or update the list of announcements
        })
        .catch(error => {
    // Show an error message
            alert(error.message);
        });
    });

    fetchAnnouncements(); // Initial fetch of announcements
});
