export function setTitle(title) {

    if (window.chrome) {
        // windows
        const data = {
            type: "setTitle",
            payload: title
        }
        window.chrome.webview.postMessage(JSON.stringify(data))
    }

    if (window.interOp) {
        window.interOp.setTitle(title);
    }
}