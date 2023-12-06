// scroll the element into view
export function scrollToElement(element) {
    element?.scrollIntoView({
        behavior: 'auto',
        block: 'center',
        inline: 'center'
    });
    console.log("scrollToElement");
}

export function triggerClick(element) {
    element?.click();
    console.log("triggerClick");
};

export function throttle(callback, limit) {
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


export function getPageTitle() {
    return document.title;
}
