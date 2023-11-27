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
        },
        onShow: () => {
            console.log('modal is shown');
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

export function toggleModal(modal) {
    modal.toggle();
}

export function showModal(modal) {
    modal.show();
}