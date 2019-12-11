
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

    viewFrame.style.display = "none";
    preFrame.style.display = "block";

    setTimeout(function () { viewFrame.src = viewFrame.src; },5000);

}

function viewLoaded() {
    let viewFrame = document.getElementById("view_frame");
    let preFrame = document.getElementById("preload_frame");

    preFrame.style.display = "none"; 
    viewFrame.style.display = "block";

    setTimeout(function () { preFrame.src = viewFrame.src; }, 5000);
}

