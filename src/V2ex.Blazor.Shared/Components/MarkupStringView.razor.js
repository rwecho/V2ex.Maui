import { throttle } from "../utils.js"
export function initialize(containerRef, thisRef) {

    const anchorClickThrottle = throttle((href) => {
        thisRef.invokeMethodAsync("AnchorClickedJsInvoke", href);
    }, 300);

    const imgClickThrottle = throttle((src) => {
        thisRef.invokeMethodAsync("ImageClickedJsInvoke", src);
    }, 300);

    // disable the anchor click event
    containerRef.addEventListener("click", (e) => {
        if (e.target.tagName === "A") {
            e.preventDefault();
            anchorClickThrottle(e.target.href);
        }
        else if (e.target.tagName === "IMG") {
            e.preventDefault();
            imgClickThrottle(e.target.src);
        }
    });


    // highlight the code
    const codeElements = containerRef.querySelectorAll("pre code");
    for (let i = 0; i < codeElements.length; i++) {
        const codeElement = codeElements[i];
        hljs.highlightElement(codeElement);
    }
}
