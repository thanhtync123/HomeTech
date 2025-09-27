window.landingPage = {
    init: function () {
        const slideTrack = document.getElementById("slideTrack");
        const slides = document.querySelectorAll(".slide");
        const prevBtn = document.getElementById("prevBtn");
        const nextBtn = document.getElementById("nextBtn");

        let currentIndex = 0;
        const slideCount = slides.length;

        // Hàm hiển thị slide theo index
        function showSlide(index) {
            const slideWidth = slides[0].clientWidth;
            slideTrack.style.transform = `translateX(-${index * slideWidth}px)`;
        }

        // Nút prev
        if (prevBtn) {
            prevBtn.addEventListener("click", () => {
                currentIndex = (currentIndex - 1 + slideCount) % slideCount;
                showSlide(currentIndex);
            });
        }

        // Nút next
        if (nextBtn) {
            nextBtn.addEventListener("click", () => {
                currentIndex = (currentIndex + 1) % slideCount;
                showSlide(currentIndex);
            });
        }

        // Auto scroll sau 5 giây
        setInterval(() => {
            currentIndex = (currentIndex + 1) % slideCount;
            showSlide(currentIndex);
        }, 5000);

        console.log("LandingPage carousel initialized");
    }
};
