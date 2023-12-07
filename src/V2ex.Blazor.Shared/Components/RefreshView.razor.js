export function initialize(mainElementId, thisRef) {
    const id = "#" + mainElementId;
   return PullToRefresh.init({
       mainElement: id,
       triggerElement: id,
       onRefresh() {
            thisRef.invokeMethodAsync('OnRefreshAsync');
       },
       shouldPullToRefresh() {
           const element = document.querySelector(id);
           return element && element.scrollTop === 0;
       }
    })
}

export function destroy(instance) {
    if (instance) {
        instance.destroy();
    }
}