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
  let input = document.querySelector(".quill-image-input");
  if (input == null) {
    input = document.createElement("input");
    input.setAttribute("type", "file");
    input.setAttribute("accept", "image/png, image/gif, image/jpeg");
    input.classList.add("quill-image-input", "hidden");
    document.body.appendChild(input);
  }

  input.addEventListener("change", async function () {
    if (!input.files || !input.files[0]) {
      return;
    }
    const file = input.files[0];
    const range = quill.getSelection(true);

    const formData = new FormData();
    formData.append("image", file);

    //upload image to imgur
    const randomIndex = Math.floor(Math.random() * imgurClientIdPool.length);
    const clidenId = imgurClientIdPool[randomIndex];
    const response = await fetch("https://api.imgur.com/3/upload", {
      method: "POST",
      headers: { Authorization: `Client-ID ${clidenId}` },
      body: formData,
    });

    input.value = "";

    if (response.ok) {
      const resData = await response.json();
      if (resData.success) {
        //insert image url to editor
        quill.insertEmbed(range.index, "image", resData.data.link);
        alert("上传成功");
        return;
      }
    }

    alert("上传失败");
  });

  input.click();
}

function emojiHandler() {
  const quill = this.quill;
    const range = quill.getSelection(true);

    quill.insertEmbed(range.index, "image", "https://i.imgur.com/io2SM1h.png");
}

export function initialize() {
  var quill = new Quill("#editor", {
    modules: {
      toolbar: {
        container: [
          ["image"], //add image here
              ["emoji"],

        ],
        handlers: {
          image: imageHandler, //Add it here
          emoji: emojiHandler,
        },
      },
    },
    theme: "snow",
  });

  return quill;
}

export function test(quill) {
  alert(quill);
}
