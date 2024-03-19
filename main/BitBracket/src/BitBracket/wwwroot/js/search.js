function initialize() {
    const searchAllButton = document.getElementById("searchAllButtonId");
    const searchButton = document.getElementById("searchButtonId");
    searchButton.addEventListener("click", getSearchUsers );
    searchAllButton.addEventListener("click", getAllBitUsers);
}

function getSearchUsers() {
    console.log("Getting specific bit users");
    const searchInput = document.getElementById("searchInputId");
    const searchValue = searchInput.value;
    fetch("/api/BitUserApi/Search/" + searchValue)
        .then(response => response.json())
        .then(response => displayBitUsers(response))
        .catch(error => console.error(error));
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
        const userCard = document.createElement("div");
        userCard.classList.add("user-card");
        userCard.style.height = "120px"; // Set the height of the card to 200 pixels
        userCard.style.width = "calc(33% - 20px)"; // Set the width of the card to 33% minus the margin
        userCard.style.display = "inline-block"; // Set the display property to inline-block
        userCard.style.backgroundColor = "white"; // Set the background color to white
        userCard.style.borderRadius = "10px"; // Add border radius to the card
        userCard.style.boxShadow = "0 2px 4px rgba(0, 0, 0, 0.1)"; // Add box shadow to the card
        userCard.style.padding = "20px"; // Add padding to the card
        userCard.style.cursor = "pointer"; // Add cursor pointer to indicate it's clickable
        userCard.style.margin = "10px"; // Add margin to create gap between cards

        const name = document.createElement("h3");
        name.textContent = bitUser.username;
        name.style.fontSize = "16px"; // Set the font size of the name to 16 pixels
        name.style.marginBottom = "10px"; // Add margin bottom to create space between name and other elements
        userCard.appendChild(name);

        const tagname = document.createElement("p");
        tagname.textContent = bitUser.tag;
        tagname.style.fontSize = "12px"; 
        tagname.style.marginBottom = "10px"; 
        userCard.appendChild(tagname);

        const bio = document.createElement("p");
        bio.textContent = bitUser.bio;
        bio.style.fontSize = "12px"; 
        userCard.appendChild(bio);
        const tooltip = document.createElement("span");
        tooltip.classList.add("tooltip");
        tooltip.textContent = `Username: ${bitUser.username}\nTag: ${bitUser.tag}\nBio: ${bitUser.bio}`;
        userCard.appendChild(tooltip);

        userCard.addEventListener("mouseover", () => {
            console.log("should be visible now")
            tooltip.style.visibility = "visible";
        });

        userCard.addEventListener("mouseout", () => {
            console.log("should be hidden now") 
            tooltip.style.visibility = "hidden";
        });

        userCard.addEventListener("click", () => {
            console.log("Clicked on user: " + bitUser.username);
            window.location.href = `/Home/SearchProfiles/${bitUser.id}`;
        });

        container.appendChild(userCard);
    });
};
document.addEventListener('DOMContentLoaded', initialize);
