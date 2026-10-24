window.addEventListener("load", () => {
    const link = document.querySelector("a[data-post-logout-redirect]");

    if (link) {
        window.location.href = link.href;
    }
});
