// Control which tiles of timetable should be shown and adjust size of test container

$(document).ready(function () {

    // Hide tiles if set so
    if (!displayClassDetailsTile) document.getElementById("class_details").style.display = "none";
    if (!displayUpcomingExamsTile) document.getElementById("upcoming_exams").style.display = "none";

    if (!displayClassDetailsTile && displayUpcomingExamsTile) {
        document.getElementById("upcoming_exams").style.height = "100%";
    }

    // start observing until frame is visible
    frameObserver.observe(window.parent.document.getElementById(window.frameElement.id), { childList: false, attributes: true, subtree: false });

});

var frameObserver = new MutationObserver(function () {

    if (window.parent.document.getElementById(window.frameElement.id).style.display != "block") return;

    // removes as log the divs which contain single tests until there is no overflow

    while (document.getElementById("upcoming_exams").parentNode.clientHeight < document.getElementById("upcoming_exams").parentNode.scrollHeight) {

        document.getElementById("upcoming_exams").removeChild(
            document.getElementById("upcoming_exams").children[document.getElementById("upcoming_exams").childElementCount - 1]);
    }   
});
