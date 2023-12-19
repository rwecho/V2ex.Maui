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

export function insertTextEmoji(quill, emoji) {
  const range = quill.getSelection(true);
  quill.insertEmbed(range.index, "text", emoji);

  const nextIndex = range.index + emoji.length;
  quill.setSelection(nextIndex, Quill.sources.SILENT);
}

export function insertImageEmoji(quill, emojiImageUrl) {
    const range = quill.getSelection(true);
    // add a space before the image
    var index = range.index;
    quill.insertEmbed(index, "image", emojiImageUrl);
    index += emojiImageUrl.length;

    quill.setSelection(index, Quill.sources.SILENT);
}

export function insertImage(quill, imageUrl) {

    const range = quill.getSelection(true);
    // add a space before the image
    var index = range.index;
    quill.insertEmbed(range.index, "text", "\n");
    index++;
    quill.insertEmbed(index, "image", imageUrl);
    index += imageUrl.length;
    quill.setSelection(index, Quill.sources.SILENT);
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

            insertImage(quill, resData.data.link);
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

export function getQuillContents(quill)
{
    const delta = quill.getContents();

    let fragments = [];
    for (var i = 0; i < delta.ops.length; i++) {
        const ops = delta.ops[i];
        if (!ops.insert) {
            continue;
        }
        if (ops.insert.image) {
            fragments.push(' ');
            fragments.push(ops.insert.image);
            fragments.push(' ');
        }
        else{
            fragments.push(ops.insert);
        }
    }
    return  fragments.join("");
}

export function clearQuill(quill) {
    quill.setContents([]);
}