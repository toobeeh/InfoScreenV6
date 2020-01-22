
let day = new Date().toISOString().substring(0, 11);
var lessonStartTimes = [
    day + '07:10',
    day + '08:00',
    day + '08:50',
    day + '09:50',
    day + '10:40',
    day + '11:40',
    day + '12:30',
    day + '13:20',
    day + '14:20',
    day + '15:10',
    day + '16:10',
    day + '17:00'
];

// IIFE -> JS has to be embedded at the right place of the html so the table is already created
// Not document.ready: Too much JS latency -> blinking marker
(function () {

    if (showProgress != "day") return;

    let lessonID = dayOfWeek + "_" + lesson;
    let lessonCell = document.getElementById(lessonID);

    if (!lessonCell) return;

    let timeMarker = document.createElement('div');
    let height = parseFloat(lessonCell.style.height);
    timeMarker.style.width = "100%";
    timeMarker.style.position = "absolute";
    timeMarker.style.top = "0";
    timeMarker.style.left = "0";

    // minutes since lesson started
    let minutes = (new Date() - new Date(lessonStartTimes[lesson])) / 1000 / 60;
    if (minutes > 50) return;

    let heightpermin = height / 50;
    height = heightpermin * minutes;

    timeMarker.style.height = height + "vh";
    //timeMarker.style.borderBottom = "2px solid red";
    timeMarker.classList.add("timetable_activeLessonProgress, timetable_activeLessonBorder");

    lessonCell.style.position = "relative";
    lessonCell.appendChild(timeMarker);


})();
