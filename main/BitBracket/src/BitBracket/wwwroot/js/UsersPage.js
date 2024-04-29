function initialize() {
    const id = document.getElementById('hiddenElement').textContent;
    CheckStatus(id);
    
    
}
async function CheckStatus(id) {
    const response = await fetch("/api/BitUserApi/CheckIfFriends/" + id); 
    const data = await response.json();

    if (data) {
        // Code to execute if data is true
        const button = document.getElementById('friend-button');
        button.textContent = 'Remove Friend';
        button.disabled = false;
        button.addEventListener('click', () => RemoveFriend(id));
    } else {
        // Code to execute if data is false
        const response = await fetch("/api/BitUserApi/CheckIfRequestSent/" + id);
        const requestData = await response.json();
        if (requestData) {
            // Code to execute if data is true
            const button = document.getElementById('friend-button');
            button.textContent = 'Request Sent';
            button.disabled = true;
        } else {
            // Code to execute if data is false
            const button = document.getElementById('friend-button');
            button.textContent = 'Add Friend';
            button.disabled = false;
            button.addEventListener('click', () => SendFriendRequest(id));
        }
        
    }
}
async function RemoveFriend(id) {
    const response = await fetch("/api/BitUserApi/RemoveFriend/" + id, {
        method: 'PUT',
        });
    CheckStatus(id);
}
async function SendFriendRequest(id) {
    const button = document.getElementById('friend-button');
    const response = await fetch("/api/BitUserApi/SendFriendRequest/" + id, {
        method: "PUT",
        });
    button.textContent = 'Request Sent';
    button.disabled = true;

    CheckStatus(id);
}
document.addEventListener('DOMContentLoaded', initialize);
