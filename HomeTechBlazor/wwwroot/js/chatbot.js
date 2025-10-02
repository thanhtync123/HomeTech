window.chatbot = {
    init: function () {
        const input = document.getElementById("chatbot-input");
        const sendBtn = document.getElementById("chatbot-send");
        const messages = document.getElementById("chatbot-messages");
        const toggleBtn = document.getElementById("chatbot-toggle");
        const container = document.getElementById("chatbot-container");

        function appendMessage(text, isUser) {
            const div = document.createElement("div");
            div.className = isUser ? "user" : "bot";
            div.innerText = text;
            messages.appendChild(div);
            messages.scrollTop = messages.scrollHeight;
        }

        async function sendMessage() {
            const text = input.value.trim();
            if (!text) return;
            appendMessage(text, true);
            input.value = "";

            try {
                const res = await fetch("/api/chat", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ message: text })
                });
                const data = await res.json();
                appendMessage(data.reply, false);
            } catch (err) {
                appendMessage("❌ Lỗi kết nối chatbot", false);
            }
        }

        sendBtn.addEventListener("click", sendMessage);
        input.addEventListener("keypress", (e) => {
            if (e.key === "Enter") sendMessage();
        });

        toggleBtn.addEventListener("click", () => {
            container.classList.toggle("collapsed");
            if (container.classList.contains("collapsed")) {
                container.style.height = "40px";
            } else {
                container.style.height = "450px";
            }
        });

        console.log("Chatbot init ✅");
    }
};
