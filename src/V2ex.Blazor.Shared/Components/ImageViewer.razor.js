export function initializeModal(modalRef) {
    const modalOptions = {
        placement: 'center-center',
        backdrop: "dynamic",
        backdropClasses:
            'bg-gray-900/50 dark:bg-gray-900/80 fixed inset-0 z-40',
        closable: true,
        onHide: () => {
            document.body.classList.add('overflow-hidden');
        },
        onShow: () => {
        },
        onToggle: () => {
        },
    };

    const instanceOptions = {
        id: 'image-viewer-modal',
        override: true
    };

    return new Modal(modalRef, modalOptions, instanceOptions);
}

export function initializeSwiper(thisRef, swiperRef) {
    const swiper = new Swiper(swiperRef, {
        // Optional parameters
        direction: 'horizontal',
        loop: false,
        initialSlide: 0,

        touchStartPreventDefault: false,
        zoom: {
            maxRatio: 3,
        },
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
               
            },
            slideChange: function () {
                thisRef.invokeMethodAsync("OnSlideChangeAsync", this.activeIndex);
            },
        },
    });

    return swiper;
}
