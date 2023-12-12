export function initialize(thisRef, initialSlide) {
    const swiper = new Swiper('.swiper', {
        // Optional parameters
        direction: 'horizontal',
        loop: false,
        initialSlide: initialSlide,

        touchStartPreventDefault: false,

        // If we need pagination
        pagination: {
            el: '.swiper-pagination',
        },

        // Navigation arrows
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },

        // And if we need scrollbar
        scrollbar: {
            el: '.swiper-scrollbar',
        },
        on: {
            init: function () {
                if (initialSlide == 0) {
                    thisRef.invokeMethodAsync("OnSlideChangeAsync", 0);
                }
            },
            slideChange: function () {
                thisRef.invokeMethodAsync("OnSlideChangeAsync", this.activeIndex);
            },
        },
    });

    return swiper;
}