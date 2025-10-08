// File: wwwroot/js/faceAuth.js

let videoStream = null;

// Tải TẤT CẢ các model cần thiết của face-api
export async function loadModels() {
    const MODEL_URL = window.location.origin + '/models';

    try {
        console.log("Đang tải models...");

        // 🧩 NẾU face-api.js chưa có, import từ CDN
        if (typeof faceapi === "undefined") {
            await import("https://cdn.jsdelivr.net/npm/face-api.js@0.22.2");
            console.log("Đã load face-api.js từ CDN!");
        }

        await faceapi.nets.tinyFaceDetector.loadFromUri(MODEL_URL);
        await faceapi.nets.faceLandmark68Net.loadFromUri(MODEL_URL);
        await faceapi.nets.faceRecognitionNet.loadFromUri(MODEL_URL);

        console.log("✅ Tải models thành công!");
    } catch (error) {
        console.error("❌ LỖI TẢI MODELS:", error);
    }
}

// Bật webcam và hiển thị trên thẻ <video>
export async function startVideo(videoElementId) {
    const video = document.getElementById(videoElementId);
    if (!video) {
        console.error(`Không tìm thấy thẻ video với id: ${videoElementId}`);
        return false;
    }

    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        try {
            videoStream = await navigator.mediaDevices.getUserMedia({ video: {} });
            video.srcObject = videoStream;
            return true;
        } catch (error) {
            console.error("Lỗi bật camera:", error);
            return false;
        }
    }
    return false;
}

// Chụp ảnh, phân tích và trả về mô tả khuôn mặt
export async function getFaceDescriptor(videoElementId) {
    const video = document.getElementById(videoElementId);
    if (!video || !faceapi.nets.tinyFaceDetector.params) {
        console.error("Models chưa được tải hoặc không tìm thấy video.");
        return null;
    }

    const detections = await faceapi.detectSingleFace(video, new faceapi.TinyFaceDetectorOptions())
        .withFaceLandmarks()
        .withFaceDescriptor();

    if (detections) {
        return detections.descriptor;
    }
    return null;
}

// Tắt webcam
export function stopVideo() {
    if (videoStream) {
        videoStream.getTracks().forEach(track => track.stop());
        videoStream = null;
    }
}
