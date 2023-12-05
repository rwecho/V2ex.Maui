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

        // fix the issue: https://github.com/themesberg/flowbite/issues/745
        // blazor normalize the aria-selected attribute by removing the 'true' value.
        document.querySelectorAll("ul>li button[aria-selected]").forEach((element) => {
            element.setAttribute("aria-selected", "true");
        });

        initFlowbite();
        console.log("Flowbite initialized");
    }
}
