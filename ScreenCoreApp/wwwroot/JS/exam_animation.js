
var examsDictionary = { }; // Key: Exam span, Value: Normal content div to be hidden
var hideExamTimeMs;
var remainingTime;

$(document).ready(function () {

    if (!animateUpcomingExamLessons) return;

    let testSpans = document.getElementsByClassName('timetable_exam');

    if (testSpans != null) {

        for (let span of testSpans) {
            examsDictionary[span.id] = span.parentNode.children[1].id = span.id + "_lesson"; // Set normal content container as value to span ID key
        }
     
        remainingTime = switchTimeMs;
        hideExamTimeMs = (switchTimeMs - examTimesShown * showExamTimeMs) / examTimesShown
        
        showAndHide();
    }

})


function showAndHide() {
    remainingTime -= (switchTimeMs - examTimesShown * showExamTimeMs) / examTimesShown + showExamTimeMs;
    if (remainingTime < 0) return;
    setTimeout(function () {

        showExam();
        setTimeout(function () {

            hideExam();
            setTimeout(showAndHide, hideExamTimeMs / 2);

        }, showExamTimeMs);

    }, hideExamTimeMs / 2);
    
    
}

function hideExam() {
    for (let span in examsDictionary) {
        $(document.getElementById(span)).slideToggle(200, function () {
            $(document.getElementById(examsDictionary[span])).slideToggle(200);
        });        
    }
}

function showExam() {
    for (let span in examsDictionary) {
        $(document.getElementById(examsDictionary[span])).slideToggle(200, function () {
            $(document.getElementById(span)).slideToggle(200);
        });  
    }
}