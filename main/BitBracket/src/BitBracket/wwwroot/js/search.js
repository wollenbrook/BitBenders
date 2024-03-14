function initialize() {
    const searchAllButton = document.getElementById("searchAllButtonId");
    searchAllButton.addEventListener("click", getAllBitUsers);
}

function getAllBitUsers() {
    console.log("Getting all Bit Users...");
    fetch("/api/BitUserApi")
        .then(response => response.json())
        .then(response => displayBitUsers(response))
        .catch(error => console.error(error));
}

function displayBitUsers(bitUsers) {
    console.log("Displaying Bit Users...");
    console.log(bitUsers);
    const container = document.getElementById("bitUsersContainer");
    container.innerHTML = "";

    bitUsers.forEach(bitUser => {
        const userDiv = document.createElement("div");
        userDiv.classList.add("user");

        const name = document.createElement("h3");
        name.textContent = bitUser.username;
        userDiv.appendChild(name);


        const tagname = document.createElement("p");
        tagname.textContent = bitUser.tag;

        const bio = document.createElement("p");
        bio.textContent = bitUser.bio;
        userDiv.appendChild(bio);
        userDiv.appendChild(tagname);

        userDiv.addEventListener("click", () => {
            console.log("Clicked on user: " + bitUser.username);
            window.location.href = `/Home/SearchProfiles/${bitUser.username}`;
        });
        container.appendChild(userDiv);
    });
}

document.addEventListener('DOMContentLoaded', initialize);
