// Check and apply the user's preferred theme on initial load
document.addEventListener('DOMContentLoaded', function() {
    const preferredTheme = getCookie('preferredTheme'); // Retrieve the preferred theme from cookies
    if (preferredTheme) { // If a preferred theme exists
        applyTheme(preferredTheme); // Apply the preferred theme
    } else { // If no preferred theme is stored
        // Default to light theme if no preference is stored
        applyTheme('light-theme'); // Apply the light theme as default
    }
});

// Event listeners for theme change
document.getElementById('light-theme').addEventListener('click', function() {
    applyTheme('light-theme'); // Apply light theme when light theme button is clicked
});

document.getElementById('dark-theme').addEventListener('click', function() {
    applyTheme('dark-theme'); // Apply dark theme when dark theme button is clicked
});

function applyTheme(theme) {
    // Remove existing theme classes
    document.body.classList.remove('light-theme', 'dark-theme'); // Remove any existing theme classes from the body
    // Add the selected theme class to the body
    document.body.classList.add(theme); // Add the selected theme class to the body

    // Save the selected theme as a cookie
    setCookie('preferredTheme', theme, 1); // Save the selected theme as a cookie for future visits

    // Trigger a custom event indicating a theme change
    document.dispatchEvent(new CustomEvent('themeChanged', { detail: { theme } })); // Dispatch a custom event for theme change
}

// Helper function to set a cookie
function setCookie(name, value, days) {
    const d = new Date(); // Create a new Date object
    d.setTime(d.getTime() + (days * 24 * 60 * 60 * 1000)); // Set the expiration time for the cookie
    const expires = "expires=" + d.toUTCString(); // Format the expiration date for the cookie
    document.cookie = `${name}=${value};${expires};path=/`; // Set the cookie with name, value, and expiration
}

// Helper function to get a cookie value
function getCookie(name) {
    const nameEQ = name + "="; // Format the cookie name for comparison
    const ca = document.cookie.split(';'); // Split the cookie string into individual cookies
    for(let i = 0; i < ca.length; i++) { // Iterate through each cookie
        let c = ca[i]; // Get the current cookie
        while (c.charAt(0) == ' ') c = c.substring(1, c.length); // Trim any leading spaces
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length); // If the cookie name matches, return its value
    }
    return null; // Return null if the cookie is not found
}