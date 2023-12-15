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

export function clickImageInput(imageInputRef) {
    imageInputRef.click();
}
/** 以下 Client ID 来自「V2EX_Polish」*/
const imgurClientIdPool = [
    '3107b9ef8b316f3',
    '442b04f26eefc8a',
    '59cfebe717c09e4',
    '60605aad4a62882',
    '6c65ab1d3f5452a',
    '83e123737849aa9',
    '9311f6be1c10160',
    'c4a4a563f698595',
    '81be04b9e4a08ce',
]
async function upload(file) {
    if (!file) return
    const formData = new FormData()
    formData.append('image', file)
    // 随机获取一个 Imgur Client ID。
    const randomIndex = Math.floor(Math.random() * imgurClientIdPool.length)
    const clidenId = imgurClientIdPool[randomIndex]
  
    // 使用详情参考 Imgur API 文档：https://apidocs.imgur.com/
    const res = await fetch('https://api.imgur.com/3/upload', {
      method: 'POST',
      headers: {Authorization: `Client-ID ${clidenId}`},
      body: formData,
    })
  
    if (res.ok) {
      const resData = await res.json()
      if (resData.success) {
        return resData.data.link;
      }
    }
}

export async function handleImageInput(imageInputRef) {
    console.log(imageInputRef.files)
    if (imageInputRef.files.length == 0) return;
    return await upload(imageInputRef.files[0])
}