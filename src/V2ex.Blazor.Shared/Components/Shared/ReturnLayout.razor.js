// disable right click when the screen is not wide enough
export function disableRightClick() {
  document.addEventListener("contextmenu", function (e) {
    e.preventDefault();
  });
  console.log("Right click disabled");
}

export function initialize() {
  if (window.document.body.clientWidth < 768) {
    disableRightClick();
  }
  // Initialize Flowbite, reference https://flowbite.com/docs/getting-started/quickstart/#init-functions
  if (initFlowbite) {
    initFlowbite();
    console.log("Flowbite initialized");
  }
}

export function goBack() {
    window.history.back();
}
