document.addEventListener('DOMContentLoaded', function () {
    
});
var tournamentName = document.getElementById('tournament-name').textContent;
// Fetch tournament details
fetch(`/api/TournamentAPI/Search/${tournamentName}`)
    .then(response => response.json())
    .then(tournament => {
        tournament = tournament[0];
        // Display tournament broadcast
        var broadcastType = tournament.broadcastType;
        var broadcastLink = tournament.broadcastLink;
        if (broadcastType === 'Twitch') {
            new Twitch.Player("stream-embed", {
                width: 175,  // specify the width
                height: 175, 
                channel: tournament.broadcastLink
            });
        }
        else if (broadcastType === 'YouTube') {
            var youtubeEmbedUrl = `https://www.youtube.com/embed/live_stream?channel=${broadcastLink}`;
            var iframe = document.createElement('iframe');
            iframe.width = "175";
            iframe.height = "175";
            iframe.src = youtubeEmbedUrl;
            iframe.allowFullscreen = "";
            iframe.scrolling = "no";
            iframe.frameBorder = "0";
            iframe.allow = "autoplay; fullscreen";
            iframe.title = "YouTube";
            iframe.sandbox = "allow-modals allow-scripts allow-same-origin allow-popups allow-popups-to-escape-sandbox allow-storage-access-by-user-activation";
            document.getElementById('stream-embed').appendChild(iframe);
        }
        else {
            document.getElementById('stream-embed').textContent = 'No broadcast available';
        }
})