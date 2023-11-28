function onVisible(element, callback) {
    new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.intersectionRatio > 0) {
                callback(element);
                observer.disconnect();
            }
        });
    }).observe(element);
    if (!callback) return new Promise(r => callback = r);
}
export function initialize(containerRef, thisRef) {
    onVisible(containerRef, () => {
        console.log("visible");
        thisRef.invokeMethodAsync("OnShowAsync");
    });
};
