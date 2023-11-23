function throttle(callback, limit) {
    let waiting = false;
    return function () {
        if (!waiting) {
            callback.apply(this, arguments);
            waiting = true;
            setTimeout(function () {
                waiting = false;
            }, limit);
        } else {
            console.log("throttle");
        }
    }
}

export function initialize(dotNetHelper) {
    const container = document.querySelector("#infinite-list");
    if (!container) {
        return
    }

    const loadMoreThrottle = throttle(() => {
        dotNetHelper.invokeMethodAsync("LoadMore");
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
            console.log("load more", heightToBottom);
            loadMoreThrottle();
        }
    });
}

export function ensureLoadComplete(dotNetHelper) {
    const container = document.querySelector("#infinite-list");
    if (!container) {
        return
    }

    // if the scroll bar is not show, load more content
    if (container.scrollHeight <= container.clientHeight) {
        console.log("the scroll bar is not show, load more");
        dotNetHelper.invokeMethodAsync("LoadMore");
    }
}