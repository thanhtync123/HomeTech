window.landingPage = {
    init: function () {
        console.log("LandingPage JS loaded ✅");

        const track = document.getElementById("slideTrack");
        const slides = track ? track.children : [];
        const prevBtn = document.getElementById("prevBtn");
        const nextBtn = document.getElementById("nextBtn");
        const pagination = document.getElementById("pagination");
        let index = 0;

        // tạo pagination dots
        if (pagination && slides.length > 0) {
            pagination.innerHTML = "";
            for (let i = 0; i < slides.length; i++) {
                const dot = document.createElement("span");
                dot.addEventListener("click", () => showSlide(i));
                pagination.appendChild(dot);
            }
        }

        function updatePagination() {
            if (!pagination) return;
            [...pagination.children].forEach((dot, i) => {
                dot.classList.toggle("active", i === index);
            });
        }

        function showSlide(i) {
            if (!track || slides.length === 0) return;

            if (i < 0) index = slides.length - 1;
            else if (i >= slides.length) index = 0;
            else index = i;

            track.style.transform = `translateX(-${index * 100}%)`;
            updatePagination();
        }

        if (prevBtn && nextBtn) {
            prevBtn.addEventListener("click", () => showSlide(index - 1));
            nextBtn.addEventListener("click", () => showSlide(index + 1));
        }

        // auto slide
        if (slides.length > 0) {
            setInterval(() => {
                showSlide(index + 1);
            }, 5000); // 5s
        }

        // hiển thị slide đầu tiên
        showSlide(0);

        console.log("LandingPage init finished 🚀");
    }
};
