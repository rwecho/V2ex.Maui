// scroll the element into view
export function scrollToElement(element) {
    element?.scrollIntoView({
        behavior: 'auto',
        block: 'center',
        inline: 'center'
    });
    console.log("scrollToElement");
}
