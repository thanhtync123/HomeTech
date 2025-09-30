// wwwroot/js/home.js

document.addEventListener("DOMContentLoaded", function () {
    // Revenue Chart (Line)
    const revenueCtx = document.getElementById("revenueChart");
    if (revenueCtx) {
        new Chart(revenueCtx, {
            type: "line",
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul"],
                datasets: [{
                    label: "Revenue",
                    data: [1200, 1900, 3000, 2500, 3200, 4000, 4500],
                    borderColor: "rgba(75, 192, 192, 1)",
                    backgroundColor: "rgba(75, 192, 192, 0.2)",
                    fill: true,
                    tension: 0.3
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { display: true }
                },
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
    }

    // Orders Chart (Pie)
    const ordersCtx = document.getElementById("ordersChart");
    if (ordersCtx) {
        new Chart(ordersCtx, {
            type: "pie",
            data: {
                labels: ["Pending", "In Progress", "Completed", "Cancelled"],
                datasets: [{
                    data: [12, 7, 25, 3],
                    backgroundColor: [
                        "rgba(255, 206, 86, 0.7)",
                        "rgba(54, 162, 235, 0.7)",
                        "rgba(75, 192, 192, 0.7)",
                        "rgba(255, 99, 132, 0.7)"
                    ],
                    borderColor: [
                        "rgba(255, 206, 86, 1)",
                        "rgba(54, 162, 235, 1)",
                        "rgba(75, 192, 192, 1)",
                        "rgba(255, 99, 132, 1)"
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { position: "bottom" }
                }
            }
        });
    }
});
