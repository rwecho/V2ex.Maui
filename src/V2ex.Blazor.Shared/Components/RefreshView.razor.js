export function initialize(mainElementId, thisRef) {
   return PullToRefresh.init({
       mainElement: "#" + mainElementId,
       triggerElement: "#" + mainElementId,
       onRefresh() {
            thisRef.invokeMethodAsync('OnRefreshAsync');
        },
    })
}

export function destroy(instance) {
    if (instance) {
        instance.destroy();
    }
}