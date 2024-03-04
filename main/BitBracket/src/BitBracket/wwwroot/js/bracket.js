// Parse the JSON string into an object
var players = JSON.parse('@Html.Raw(jsonModel)');

// Create an array of player names
var names = players.map(function(player) {
    return player.PlayerTag;
});

function roundToPowerOfTwo(names) {
  const numPlayers = names.length;
  const powerOfTwo = Math.pow(2, Math.ceil(Math.log2(numPlayers)));
  const nullsToAdd = powerOfTwo - numPlayers;

  // Add nulls to the player list
  const paddedPlayers = names.concat(Array(nullsToAdd).fill(null));

  // Generate the order of the teams
  const order = seeding(powerOfTwo);

  // Initialize teams array
  const teams = [];

  // Split the players into teams (round 1 matches) based on the order
  for (let i = 0; i < powerOfTwo / 2; i++) {
    const team = [paddedPlayers[order[i * 2] - 1], paddedPlayers[order[i * 2 + 1] - 1]];
    teams.push(team);
  }

  return teams;
}

function seeding(numPlayers){
  var rounds = Math.log(numPlayers)/Math.log(2)-1;
  var pls = [1,2];
  for(var i=0;i<rounds;i++){
    pls = nextLayer(pls);
  }
  return pls;
  function nextLayer(pls){
    var out=[];
    var length = pls.length*2+1;
    pls.forEach(function(d){
      out.push(d);
      out.push(length-d);
    });
    return out;
  }
}

var teams = roundToPowerOfTwo(names);
console.log(teams);

if (teams.length > 0) {
    var singleElimination = {
        "teams": teams
    };

    $(function() {
        $('#minimal .demo').bracket({
            init: singleElimination
        });
    });
} 
else {
    console.error("No teams available to initialize the bracket.");
}