import { debounce } from "../utils.js"

export function initialize(containerRef, thisRef) {
    const scrollDebounce = debounce((scrollTop, scrollHeight, clientHeight) => {
        thisRef.invokeMethodAsync("OnScrollAsync", scrollTop, scrollHeight, clientHeight);
    }, 300);

    // listen the container ref scroll event
    containerRef.addEventListener("scroll", (e) => {
        scrollDebounce(e.target.scrollTop, e.target.scrollHeight, e.target.clientHeight);
    });
}

export function scrollToTop(containerRef) {
    containerRef.scrollTo({
        top: 0,
        behavior: "smooth"
    });
}
