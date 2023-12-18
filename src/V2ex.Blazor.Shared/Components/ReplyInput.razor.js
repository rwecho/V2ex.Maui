const imgurClientIdPool = [
  "3107b9ef8b316f3",
  "442b04f26eefc8a",
  "59cfebe717c09e4",
  "60605aad4a62882",
  "6c65ab1d3f5452a",
  "83e123737849aa9",
  "9311f6be1c10160",
  "c4a4a563f698595",
  "81be04b9e4a08ce",
];
function imageHandler() {
  //prompt user to choose a local image and uploading it to imgur then insert the image url to the editor
  const quill = this.quill;
}

function emojiHandler() {
  const quill = this.quill;
  const range = quill.getSelection(true);

  quill.insertEmbed(range.index, "image", "https://i.imgur.com/io2SM1h.png");
}

export function initialize(containerRef, thisRef) {
  const editorDiv = containerRef.querySelector("#editor");
  var SizeStyle = Quill.import("attributors/style/size");
  Quill.register(SizeStyle, true);
  var quill = new Quill(editorDiv, {
    modules: {
      toolbar: false,
    },
    theme: "snow",
  });
  return quill;
}

export function insertEmoji(quill, emoji) {
  const range = quill.getSelection(true);
  quill.insertEmbed(range.index, "text", emoji);

  const nextIndex = range.index + emoji.length;
  quill.setSelection(nextIndex, Quill.sources.SILENT);
}

export function insertClassicEmoji(quill, emojiImageUrl) {
  const range = quill.getSelection(true);
  quill.insertEmbed(range.index, "image", emojiImageUrl);

  const nextIndex = range.index + emojiImageUrl.length;
  quill.setSelection(nextIndex, Quill.sources.SILENT);
}

export async function chooseImage(quill, thisRef) {
  const input = document.createElement("input");
  const promise = new Promise((resolve, reject) => {
    input.setAttribute("type", "file");
    input.setAttribute("accept", "image/png, image/gif, image/jpeg");
    input.classList.add("quill-image-input", "hidden");
    document.body.appendChild(input);

    input.addEventListener("cancel", function () {
        reject("cancel");
    });

    input.addEventListener("change", async function () {
      if (!input.files || !input.files[0]) {
        reject();
        return;
      }
      const file = input.files[0];
      const range = quill.getSelection(true);

      // reset the input value;
      input.value = "";

      const formData = new FormData();
      formData.append("image", file);

      //upload image to imgur
      const randomIndex = Math.floor(Math.random() * imgurClientIdPool.length);
      const clidenId = imgurClientIdPool[randomIndex];

      try {
        const response = await fetch("https://api.imgur.com/3/upload", {
          method: "POST",
          headers: { Authorization: `Client-ID ${clidenId}` },
          body: formData,
        });

        if (response.ok) {
          const resData = await response.json();
          if (resData.success) {
            //insert image url to editor
            quill.insertEmbed(range.index, "image", resData.data.link);
            resolve();
            return;
          }
        }
        reject();
      } catch (e) {
        reject(e.message);
      }
    });

    input.click();
  });

  try {
    return await promise;
  } finally {
    document.body.removeChild(input);
  }
}
