export function initializeSwiper() {
    const swiper = new Swiper('.swiper-tabs', {
        slidesPerView: 5,
        spaceBetween: 0,
        freeMode: true
    });
    console.log("initialize swiper.");
}