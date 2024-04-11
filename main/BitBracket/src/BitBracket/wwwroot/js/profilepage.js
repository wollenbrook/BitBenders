//wwwroot/js/profilepage.js

// Enable editing of the div content for Tag and Bio
document.getElementById('tagId').contentEditable = true;
document.getElementById('bioId').contentEditable = true;

// Function to update profile picture
document.getElementById('profileImage').addEventListener('click', function () {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = 'image/*';

    input.onchange = async (e) => {
        const file = e.target.files[0];
        const formData = new FormData();
        formData.append('file', file);

        try {
            const response = await fetch('/api/BitUserApi/Image', {
                method: 'POST',
                body: formData
            });
            if (response.ok) {
                // Reload the page to update the profile picture
                location.reload();
            } else {
                alert('Error uploading file.');
            }
        } catch (error) {
            alert('Error: ' + error);
        }
    };

    input.click();
});

// Functionality to save Tag and Bio
document.getElementById('saveTagButtonId').addEventListener('click', async function () {
    const updatedTag = document.getElementById('tagId').innerText;
    await updateUserInfo('tag', updatedTag);
});

document.getElementById('saveBioButtonId').addEventListener('click', async function () {
    const updatedBio = document.getElementById('bioId').innerText;
    await updateUserInfo('bio', updatedBio);
});

// Generic function to update user info (Tag or Bio)
async function updateUserInfo(field, value) {
    const url = `/api/BitUserApi/${field}Change/${value}`;
    try {
        const response = await fetch(url, { method: 'PUT' });
        if (response.ok) {
            alert(`${field} updated successfully.`);
            location.reload(); // Optionally reload if you want immediate visual feedback
        } else {
            alert(`Failed to update ${field}.`);
        }
    } catch (error) {
        alert(`Error updating ${field}: ${error}`);
    }
}
