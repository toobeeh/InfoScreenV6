
var queryString = window.location.search;


window.onload = function () {
    let viewFrame = document.getElementById("view_frame");
    let preFrame = document.getElementById("preload_frame");

    preFrame.onload = previewLoaded;
    viewFrame.onload = viewLoaded;

    viewFrame.src = "Init" + queryString;

}


function previewLoaded() {
    let viewFrame = document.getElementById("view_frame");
    let preFrame = document.getElementById("preload_frame");

    console.log(preFrame.contentDocument.title);

    if (preFrame.contentDocument.title == "" || preFrame.contentDocument.title == "Internal Server Error") {
        viewLoaded();
        return;
    }

    viewFrame.style.display = "none";
    preFrame.style.display = "block";

    setTimeout(function () { viewFrame.src = viewFrame.src; }, switchTimeMs);

}

function viewLoaded() {
    let viewFrame = document.getElementById("view_frame");
    let preFrame = document.getElementById("preload_frame");

    console.log(viewFrame.contentDocument.title);

    if (viewFrame.contentDocument.title == "" || viewFrame.contentDocument.title == "Internal Server Error") {
        previewLoaded();
        return;
    }

    preFrame.style.display = "none"; 
    viewFrame.style.display = "block";

    setTimeout(function () { preFrame.src = viewFrame.src; }, switchTimeMs);
}


