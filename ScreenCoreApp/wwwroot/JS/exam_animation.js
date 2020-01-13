
var examsDictionary = { }; // Key: Exam span, Value: Normal content div to be hidden

$(document).ready(function () {

    let testSpans = document.getElementsByClassName('timetable_exam');

    if (testSpans != null) {

        for (let span of testSpans) {
            examsDictionary[span.id] = span.parentNode.children[1].id = span.id + "_lesson"; // Set normal content container as value to span ID key
        }

        setTimeout(showExam, 2000);
        setTimeout(hideExam, 6000);
    }

})

function hideExam() {
    for (let span in examsDictionary) {
        $(document.getElementById(span)).slideToggle("fast", function () {
            $(document.getElementById(examsDictionary[span])).slideToggle("fast");
        });        
    }
}

function showExam() {
    for (let span in examsDictionary) {
        $(document.getElementById(examsDictionary[span])).slideToggle("fast", function () {
            $(document.getElementById(span)).slideToggle("fast");
        });  
    }
}