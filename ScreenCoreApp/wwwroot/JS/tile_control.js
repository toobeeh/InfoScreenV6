// Control how many exams are displayed in exam container depending on size

$(document).ready(function () {

    // if frame ist not visible yet start observing until frame is visible (-> scrollheight can only be checked if frame is visible!)
    if (window.parent.document.getElementById(window.frameElement.id).style.display == "none")
        frameObserver.observe(window.parent.document.getElementById(window.frameElement.id), { childList: false, attributes: true, subtree: false });
    else fitExamContainer();

});

var frameObserver = new MutationObserver(function () {

    if (window.parent.document.getElementById(window.frameElement.id).style.display != "block") return;

    // removes as log the divs which contain single tests until there is no overflow
    fitExamContainer();
   
});

function fitExamContainer() {
    while (document.getElementById("upcoming_exams").parentNode.clientHeight < document.getElementById("upcoming_exams").parentNode.scrollHeight) {

        document.getElementById("upcoming_exams").removeChild(
            document.getElementById("upcoming_exams").children[document.getElementById("upcoming_exams").childElementCount - 1]);
    }   
}
