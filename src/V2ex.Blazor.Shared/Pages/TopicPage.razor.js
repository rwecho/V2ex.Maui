export function initializeCallUpUserModal(containerRef, thisRef) {
    const $modalElement = containerRef.querySelector("#callUpUserModal");
    const modalOptions = {
        placement: 'bottom-center',
        backdrop: "dynamic",
        backdropClasses:
            'bg-gray-900/50 dark:bg-gray-900/80 fixed inset-0 z-40',
        closable: true,
        onHide: () => {
            document.body.classList.add('overflow-hidden');
            thisRef.invokeMethodAsync('OnModalUserHideJsInvoke');
        },
        onShow: () => {
            thisRef.invokeMethodAsync('OnModalUserShowJsInvoke');
        },
        onToggle: () => {
        },
    };

    const instanceOptions = {
        id: 'callUpUserModal',
        override: true
    };

    return new Modal($modalElement, modalOptions, instanceOptions);
};

export function initializeReplyInputModal(containerRef, thisRef) {
    const $modalElement = containerRef.querySelector("#replyInputModal");
    const modalOptions = {
        placement: 'bottom-center',
        backdrop: "dynamic",
        backdropClasses:
            'bg-gray-900/50 dark:bg-gray-900/80 fixed inset-0 z-40',
        closable: true,
        onHide: () => {
            document.body.classList.add('overflow-hidden');
        },
        onShow: () => {
            thisRef.invokeMethodAsync('OnModalReplyShowJsInvoke');
        },
        onToggle: () => {
        },
    };

    const instanceOptions = {
        id: 'replyInputModal',
        override: true
    };

    return new Modal($modalElement, modalOptions, instanceOptions);
}