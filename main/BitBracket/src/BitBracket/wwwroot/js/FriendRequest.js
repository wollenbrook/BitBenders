function initialize() {
    getFriendRequest();
    getFriendsList();

    function startTimer() {
        setInterval(getFriendRequest, 60000);    
        setInterval(getFriendsList, 60000);
    }

    startTimer();

}
async function getFriendsList() {
    const response =  await fetch("/api/BitUserApi/GetFriends"); 
    const data = await response.json();
    const friendList = document.getElementById("Friends");
    friendList.innerHTML = "";


    data.forEach(item => {
        const row = document.createElement("div");
        row.textContent = item.username; // Replace "name" with the actual property name from your JSON
        row.addEventListener("click", () => MovePage(item));
        friendList.appendChild(row);
    });
    
}
async function getFriendRequest() {
    const response = await fetch("/api/BitUserApi/GetFriendRequests"); 
    const data = await response.json();
    const friendRequestList = document.getElementById("FriendRequest"); 
    friendRequestList.innerHTML = "";
    data.forEach(item => {
        const row = document.createElement("div");
        row.textContent = item.sender.username; 

        const acceptButton = document.createElement("button");
        acceptButton.textContent = "Accept";
        acceptButton.classList.add("accept-button");
        acceptButton.addEventListener("click", () => AcceptFriendRequest(item.sender.id)); // Call AcceptFriendRequest function on button click

        const declineButton = document.createElement("button");
        declineButton.textContent = "Decline";
        declineButton.classList.add("decline-button");
        declineButton.addEventListener("click", () => DeclineFriendRequest(item.sender.id)); // Call DeclineFriendRequest function on button click

        row.appendChild(acceptButton);
        row.appendChild(declineButton);
        friendRequestList.appendChild(row);
    });
}
async function AcceptFriendRequest(id) {

    const response = await fetch("/api/BitUserApi/AcceptFriendRequest/" + id, {
        method: 'PUT',
    });
    
    getFriendsList();
    getFriendRequest();
}
async function DeclineFriendRequest(id) {
    const response = await fetch("/api/BitUserApi/DeclineFriendRequest/" + id, {
        method: 'PUT',
    });
    const data = await response.json();
    getFriendsList();
    getFriendRequest();
}
function MovePage(item) {
    window.location.href = `/Home/SearchProfiles/${item.id}`;
}
document.addEventListener('DOMContentLoaded', initialize);
