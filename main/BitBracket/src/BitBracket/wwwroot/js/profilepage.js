// Add click event listener to the bioId div
document.getElementById('bioId').addEventListener('click', function() {
  // Enable editing of the div content
  this.contentEditable = true;
});
document.getElementById('tagId').addEventListener('click', function () {
    // Enable editing of the div content
    this.contentEditable = true;
});


document.getElementById('profileImage').addEventListener('click', function () {
        // Prompt the user to choose an image file
        const input = document.createElement('input');
        input.type = 'file';
        input.accept = 'image/*';

        input.addEventListener('change', async (event) => {
        const file = event.target.files[0]; // Get the selected file

        const formData = new FormData(); // Create a new FormData object
        formData.append('file', file); // Append the file to the FormData object

  try {
    const response = await fetch('/api/BitUserApi/Image', {
      method: 'POST',
      body: formData
    });

            if (response.ok) {
                // File uploaded successfully
                console.log('File uploaded successfully');
            } else {
                // Handle the error case
                console.error('Error uploading file');
            }
        } catch (error) {
            console.error('Error:', error);
        }
    });
               
        // Trigger the file input dialog
        input.click();
    });
 
document.getElementById('saveTagButtonId').addEventListener('click', function () {
// Get the updated content of the bioId div
        var updatedContent = document.getElementById('tagId').innerHTML.trim();
        const url = '/api/BitUserApi/TagChange/' + updatedContent;
        const response = fetch(url, {
            method: 'PUT',
        });
        console.log('I hope it worked lol')
    // Send the updated content to the server to save in the database
    // You can use AJAX or fetch API to send the data to the server
    
    // Disable editing of the bioId div after saving
}
);
document.getElementById('saveBioButtonId').addEventListener('click', function() {
  // Get the updated content of the bioId div
    var updatedContent = document.getElementById('bioId').innerHTML.trim();
    const url = '/api/BitUserApi/BioChange/' + updatedContent;
    const response = fetch(url, {
        method: 'PUT',
    });
    console.log('I hope it worked lol')
  // Send the updated content to the server to save in the database
  // You can use AJAX or fetch API to send the data to the server

  // Disable editing of the bioId div after saving
});
