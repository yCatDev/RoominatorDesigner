mergeInto(LibraryManager.library, {

    AppReady: function () {
        var container = document.getElementById("unity-container");
        var canvas = document.querySelector("#unity-canvas");
        canvas.width  = window.innerWidth;
        canvas.height = window.innerHeight;
        container.style.display = "block";
    }
});