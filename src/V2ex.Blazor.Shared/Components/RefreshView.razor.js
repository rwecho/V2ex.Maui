export function initialize(mainElementId, thisRef) {
    const id = "#" + mainElementId;
   return PullToRefresh.init({
       mainElement: id,
       triggerElement: id,
       onRefresh() {
            thisRef.invokeMethodAsync('OnRefreshAsync');
       },
       shouldPullToRefresh() {
           const scrollView = document.querySelector(id).querySelector("#scrollView");
           if (scrollView) {
               return scrollView.scrollTop === 0;
           }
           return true;
       }
    })
}

export function destroy(instance) {
    if (instance) {
        instance.destroy();
    }
}