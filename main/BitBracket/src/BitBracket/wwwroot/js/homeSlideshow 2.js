document.addEventListener('DOMContentLoaded', function() {
    const images = document.querySelectorAll('.background-container img');
    let currentImageIndex = Math.floor(Math.random() * images.length); // Start at a random image

    function showImage(index) {
        images.forEach((img, idx) => {
            img.style.display = (idx === index) ? 'block' : 'none'; // Only display the current image
            img.classList.add('blur'); // Apply blur effect
        });
    }

    function nextImage() {
        currentImageIndex = (currentImageIndex + 1) % images.length; // Cycle to the next image
        showImage(currentImageIndex);
    }

    function startSlideshow() {
        showImage(currentImageIndex); // Show initial image
        setInterval(nextImage, 10000); // Change image every 10 seconds
    }

    startSlideshow(); // Start the slideshow
});
