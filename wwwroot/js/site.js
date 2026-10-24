(function () {
  var toggle = document.querySelector("[data-sidebar-toggle]");
  var backdrop = document.querySelector("[data-sidebar-backdrop]");

  function closeSidebar() {
    document.body.classList.remove("sidebar-open");
  }

  if (toggle) {
    toggle.addEventListener("click", function () {
      document.body.classList.toggle("sidebar-open");
      toggle.setAttribute("aria-expanded", document.body.classList.contains("sidebar-open") ? "true" : "false");
    });
  }

  if (backdrop) {
    backdrop.addEventListener("click", closeSidebar);
  }

  document.querySelectorAll(".app-sidebar a").forEach(function (link) {
    link.addEventListener("click", closeSidebar);
  });

  document.querySelectorAll(".app-content table td:last-child").forEach(function (cell) {
    if (cell.querySelector("table") || !cell.textContent.includes("|")) {
      return;
    }

    cell.classList.add("row-actions");
    cell.innerHTML = cell.innerHTML.replace(/\s*\|\s*/g, "");
    cell.querySelectorAll("a").forEach(function (link) {
      if (!link.classList.contains("btn")) {
        link.classList.add("text-action");
      }
    });
  });

  document.querySelectorAll(".app-content a").forEach(function (link) {
    var label = link.textContent.replace(/\s+/g, " ").trim();
    if (label === "Create New") {
      link.classList.add("btn-create");
    }
    if (label === "Back to List") {
      link.classList.add("back-link");
    }
  });
})();
