// scroll the element into view
export function scrollToElement(element) {
  element?.scrollIntoView({
    behavior: "auto",
    block: "center",
    inline: "center",
  });
  console.log("scrollToElement");
}

export function triggerClick(element) {
  element?.click();
  console.log("triggerClick");
}

// the throttle function
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
  };
}

// the debounce function
export function debounce(callback, delay) {
  let timer;
  return function () {
    let context = this;
    let args = arguments;
    clearTimeout(timer);
    timer = setTimeout(function () {
      callback.apply(context, args);
    }, delay);
  };
}

export function getPageTitle() {
  return document.title;
}
