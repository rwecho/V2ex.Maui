export function initializeModal(containerRef, thisRef) {
    const $modalElement = containerRef.querySelector("#callupUserModal");
    const modalOptions = {
        placement: 'bottom-center',
        backdrop: "dynamic",
        backdropClasses:
            'bg-gray-900/50 dark:bg-gray-900/80 fixed inset-0 z-40',
        closable: true,
        onHide: () => {
            console.log('modal is hidden');
            document.body.classList.add('overflow-hidden');
            thisRef.invokeMethodAsync('OnModalHideJsInvoke');
        },
        onShow: () => {
            console.log('modal is shown');
            thisRef.invokeMethodAsync('OnModalShowJsInvoke');
        },
        onToggle: () => {
            console.log('modal has been toggled');
        },
    };

    const instanceOptions = {
        id: 'callupUserModal',
        override: true
    };

    return new Modal($modalElement, modalOptions, instanceOptions);
};
export function showModal(modal) {
    modal.show();
}

export function hideModal(modal) {
    modal.hide();
}