import { throttle } from "../utils.js"
export function initialize(dotNetHelper) {
    const container = document.querySelector("#collection-view #scrollView");
    if (!container) {
        return
    }

    const loadMoreThrottle = throttle(() => {
        dotNetHelper.invokeMethodAsync("RemainingReachedJsInvoke");
    }, 300);

    let lastScrollHeight = 0;

    // listen the container vertical scroll change event
    container.addEventListener("scroll", function (e) {
        const target = e.target;
        const scrollTop = target.scrollTop;
        const scrollHeight = target.scrollHeight;
        const clientHeight = target.clientHeight;
        const offset = 5;

        if (scrollHeight !== lastScrollHeight) {
            // the elements in the container have changed, so the scroll bar will change
            lastScrollHeight = scrollHeight;
            return;
        }


        // if the scroll bar is close to the bottom, load more content
        const heightToBottom = scrollHeight - scrollTop - clientHeight;
        if (heightToBottom < offset) {
            console.log("The scorll remaining reached", heightToBottom);
            loadMoreThrottle();
        }
    });
}