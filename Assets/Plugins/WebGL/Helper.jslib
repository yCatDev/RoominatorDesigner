mergeInto(LibraryManager.library, {

    AppReady: function () {
        var container = document.getElementById("unity-container");
        var canvas = document.querySelector("#unity-canvas");
        canvas.width  = window.innerWidth;
        canvas.height = window.innerHeight;
        container.style.display = "block";
    },
    
    SaveRoom: function (userId, json)
    {
        window.saveCurrentRoom(Pointer_stringify(userId), Pointer_stringify(json));
    }
    
});