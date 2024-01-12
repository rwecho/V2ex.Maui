export function initialize(platform) {
    if (platform === "unknow") {
        return;
    }

    if (!document.documentElement.classList.contains(platform)) {
        document.documentElement.classList.add(platform);
    }
}