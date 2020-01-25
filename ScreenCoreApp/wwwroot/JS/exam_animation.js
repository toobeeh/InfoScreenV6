// Controls the animation of the exam slide-in and out

var examsDictionary = { }; // Key: Exam span, Value: Normal content div to be hidden
var hideExamTimeMs;
var remainingTime;

var full_height;

$(document).ready(function () {

    if (!animateUpcomingExamLessons) return;

    let testSpans = document.getElementsByClassName('timetable_exam');

    if (testSpans != null) {

        for (let span of testSpans) {
            examsDictionary[span.id] = span.parentNode.children[1]; // Set normal content container as value to span ID key
        }

        remainingTime = switchTimeMs;
        hideExamTimeMs = (switchTimeMs - examTimesShown * showExamTimeMs) / examTimesShown

        showAndHide();
    }

});



function showAndHide() {
    remainingTime -= (switchTimeMs - examTimesShown * showExamTimeMs) / examTimesShown + showExamTimeMs;
    if (remainingTime < 0) return;
    setTimeout(function () {

        showExamSpans();
        setTimeout(function () {

            hideExamSpans();
            setTimeout(showAndHide, hideExamTimeMs / 2);

        }, showExamTimeMs);

    }, hideExamTimeMs / 2);
    
    
}

function hideExamSpans() {
    for (let span in examsDictionary) {
        // Make exam spans transparent
        document.getElementById(span).style.color = "transparent";
        document.getElementById(span).style.borderColor = "transparent";
    }
    // wait till animation time passed
    setTimeout(function () {
        for (let span in examsDictionary) {
            // remove exam spans from flow
            document.getElementById(span).style.display = "none";
            examsDictionary[span].style.display = "inline";
        }
        // transition time between hidden exam and shown lesson
        setTimeout(function () {
            for (let span in examsDictionary) {
                examsDictionary[span].style.color = "";
            }
        }, 100);

    }, examAnimationDuration / 2);
}

function showExamSpans() {
    for (let span in examsDictionary) {     
        // Make lesson spans transparent
        examsDictionary[span].style.color = "transparent";        
    }
    // wait till animation time passed
    setTimeout(function () {
        for (let span in examsDictionary) {
            // remove spans from flow
            examsDictionary[span].style.display = "none";
            document.getElementById(span).style.display = "inline";
        }
        // transition time between hidden lesson and shown exam
        setTimeout(function () {
            for (let span in examsDictionary) {
                document.getElementById(span).style.borderColor = "";
                document.getElementById(span).style.color = "";
            }
        }, 100);

    }, examAnimationDuration / 2);
}
