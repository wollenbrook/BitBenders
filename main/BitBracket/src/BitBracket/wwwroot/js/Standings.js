document.addEventListener('DOMContentLoaded', function () {
    var container = document.getElementById("content");
    container.innerHTML = "";
    FillContentWithStandings();
    
});

function FillContentWithStandings() {
    var container = document.getElementById("content");
    container.innerHTML = "";
    var table = document.createElement("table");
    table.classList.add("ui", "celled", "table");
    var thead = document.createElement("thead");
    var tr = document.createElement("tr");
    var th = document.createElement("th");
    th.textContent = "Tournament";
    tr.appendChild(th);
    th = document.createElement("th");
    th.textContent = "Username";
    tr.appendChild(th);
    th = document.createElement("th");
    th.textContent = "Placement";


    tr.appendChild(th);
    thead.appendChild(tr);
    table.appendChild(thead);
    var tbody = document.createElement("tbody");
    var name = document.getElementById("name").textContent;
    console.log(name);
    fetch(`/api/BitUserApi/GetPlacementsByUser/${name}`)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            data.forEach(user => {
                var tr = document.createElement("tr");
                var td = document.createElement("td");
                tr.appendChild(td);
                td = document.createElement("td");
                td.textContent = user.personNavigation.username;
                tr.appendChild(td);
                td = document.createElement("td");
                td.textContent = user.placement;
                tr.appendChild(td);

                tbody.appendChild(tr);
            });
            table.appendChild(tbody);
            container.appendChild(table);
        });
}