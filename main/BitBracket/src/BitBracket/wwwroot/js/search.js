

function initialize() {
    const searchAllButton = document.getElementById("searchAllButtonId");
    const searchButton = document.getElementById("searchButtonId");
    searchButton.addEventListener("click", Search);
    searchAllButton.addEventListener("click", getAllBitUsers);
}
async function Search() {
    const statusDropdown = document.getElementById("statusDropdownId");
    console.log('searching');
    const selectedOption = statusDropdown.value;
    if (selectedOption === "Users") {
        getSearchUsers();
    } else if (selectedOption === "Any" || selectedOption === "") {
        AnySearch();
    } else if (selectedOption === "Tournaments") {
        const type = "Tournaments";
        getSearchTournaments(type);
    }
}

async function getSearchTournaments(type) {
    const searchInput = document.getElementById("searchInputId");
    const searchValue = searchInput.value;
    try {
        const tournamentResponse = await fetch("/api/TournamentAPI/Search/" + searchValue);
        const tournamentdata = await tournamentResponse.json();
        displayTournaments(tournamentdata, type);
    } catch (error) {
        console.error(error);

    }
}
async function AnySearch() {
    const searchInput = document.getElementById("searchInputId");
    console.log('searching for anything');

    const searchValue = searchInput.value;
    try {
        const response = await fetch("/api/BitUserApi/Search/" + searchValue);
        const data = await response.json();
        displayBitUsers(data);
    } catch (error) {
        console.error(error);
    }
    try {
        const tournamentResponse = await fetch("/api/TournamentAPI/Search/" + searchValue);
        const tournamentdata = await tournamentResponse.json();
        displayTournaments(tournamentdata);
    } catch (error) {
        console.error(error);
    }
}
async function getSearchUsers() {
    //console.log("Getting specific bit users");
    const searchInput = document.getElementById("searchInputId");
    const searchValue = searchInput.value;
    try {
        const response = await fetch("/api/BitUserApi/Search/" + searchValue);
        const data = await response.json();
        displayBitUsers(data);
    } catch (error) {
        console.error(error);
    }
}
async function getAllBitUsers() {
    //console.log("Getting all Bit Users...");
    try {
        const response = await fetch("/api/BitUserApi");
        const data = await response.json();
        const tournamentResponse = await fetch("/api/TournamentAPI/All");
        const tournamentdata = await tournamentResponse.json();
        console.log(tournamentdata);
        displayBitUsers(data);
        displayTournaments(tournamentdata);
    } catch (error) {
        console.error(error);
    }
}
function displayTournaments(tournamentdata, type) {
    const container = document.getElementById("bitUsersContainer");
    if (type === "Tournaments") {
        container.innerHTML = "";
    }
    // check if tournaments are empty
    if (tournamentdata.length === 0) {
        const noTournamentsFound = document.createElement("p");
        noTournamentsFound.classList.add("no-bitusers-found");
        noTournamentsFound.textContent = "No tournaments found.";
        container.prepend(noTournamentsFound);
    }
    tournamentdata.forEach(tournament => {
        const tournamentCard = document.createElement("div");
        tournamentCard.classList.add("user-card");
        const type = document.createElement("h3");
        type.textContent = "Tournament";
        type.classList.add("form-info");

        tournamentCard.appendChild(type);
        const name = document.createElement("h3");
        name.textContent = tournament.name;
        name.classList.add("name");
        tournamentCard.appendChild(name);

        const location = document.createElement("p");
        location.textContent = tournament.location;
        location.classList.add("form-info");
        tournamentCard.appendChild(location);

        const status = document.createElement("p");
        status.textContent = tournament.status;
        status.classList.add("form-info");
        tournamentCard.appendChild(status);

        const tooltip = document.createElement("span");
        tooltip.classList.add("tooltip");
        tooltip.textContent = `Name: ${tournament.name}\nLocation: ${tournament.location}\nStatus: ${tournament.status}`;
        tournamentCard.appendChild(tooltip);

        tournamentCard.addEventListener("mouseover", () => {
            //console.log("should be visible now")
            tooltip.style.visibility = "visible";
        });

        tournamentCard.addEventListener("mouseout", () => {
            //console.log("should be hidden now")
            tooltip.style.visibility = "hidden";
        });




        tournamentCard.addEventListener("click", () => {
            //console.log("Clicked on user: " + bitUser.username);
            window.location.href = `/Home/Tournaments/${tournament.id}`;
        });
        container.appendChild(tournamentCard);


    })
}
function displayBitUsers(bitUsers) {

    const container = document.getElementById("bitUsersContainer");
    container.innerHTML = "";
    if (bitUsers.length === 0) {
        const noUsersFound = document.createElement("p");
        noUsersFound.classList.add("no-bitusers-found");
        noUsersFound.textContent = "No users found.";
        container.appendChild(noUsersFound);
    } else {

        bitUsers.forEach(bitUser => {
            const userCard = document.createElement("div");
            userCard.classList.add("user-card");
            const type = document.createElement("h3");
            type.textContent = "User";
            type.classList.add("form-info");
            userCard.appendChild(type);

            const name = document.createElement("h3");
            name.textContent = bitUser.username;
            name.classList.add("name");
            userCard.appendChild(name);

            const tagname = document.createElement("p");
            tagname.textContent = bitUser.tag;
            tagname.classList.add("form-info");
            userCard.appendChild(tagname);

            const bio = document.createElement("p");
            bio.textContent = bitUser.bio;
            bio.classList.add("form-info");
            userCard.appendChild(bio);

            const tooltip = document.createElement("span");
            tooltip.classList.add("tooltip");
            tooltip.textContent = `Username: ${bitUser.username}\nTag: ${bitUser.tag}\nBio: ${bitUser.bio}`;
            userCard.appendChild(tooltip);

            userCard.addEventListener("mouseover", () => {
                //console.log("should be visible now")
                tooltip.style.visibility = "visible";
            });

            userCard.addEventListener("mouseout", () => {
                //console.log("should be hidden now")
                tooltip.style.visibility = "hidden";
            });

            userCard.addEventListener("click", () => {
                //console.log("Clicked on user: " + bitUser.username);
                window.location.href = `/Home/SearchProfiles/${bitUser.id}`;
            });

            container.appendChild(userCard);

        });
    }
};
document.addEventListener('DOMContentLoaded', initialize);
