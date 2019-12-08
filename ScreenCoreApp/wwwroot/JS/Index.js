
var queryString = window.location.search; // Querystring with ID

/*
 * Uses two iframes to display the content for the screen
 * 
 * When a iframe content is loaded, it sets a timeout to load the second iframe after 5 seconds,
 * if the second iframe is loaded, the timeout is set for the first.
 *  -> No transmission glitch
 */
 

window.onload = function () {

    let viewFrame = document.getElementById("view_frame"); // when window is loaded, attach load event to iframes
    let preFrame = document.getElementById("preload_frame");

    preFrame.onload = previewLoaded;
    viewFrame.onload = viewLoaded;

    viewFrame.src = "Init" + queryString; // load content of view iframe, trigger viewLoaded

}


function previewLoaded() {
    let viewFrame = document.getElementById("view_frame");
    let preFrame = document.getElementById("preload_frame");

    viewFrame.style.display = "none"; //hide view, activate pre
    preFrame.style.display = "block";

    setTimeout(function () { viewFrame.src = viewFrame.src; },5000);

}

function viewLoaded() {
    let viewFrame = document.getElementById("view_frame");
    let preFrame = document.getElementById("preload_frame");

    preFrame.style.display = "none"; //hide pre, activate view
    viewFrame.style.display = "block";

    setTimeout(function () { preFrame.src = viewFrame.src; }, 5000); // load pre after 5 seconds, triggers previewLoaded
}

