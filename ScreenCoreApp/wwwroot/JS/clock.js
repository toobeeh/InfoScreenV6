//Add clock to div
var clockContainer;

// IIFE to add JS clock, has to be executed as soon as the container is available and before document is ready 
// to avoid 'blinkeng' when the clock is added after the frame is activated

(function () {

    clockContainer = document.getElementById('js_clock');
    if (clockContainer != null) {
        setTimeValue();
        setInterval(setTimeValue, 2000);
    }
})();

function setTimeValue() {
    let now = new Date();
    //let s = now.getSeconds();
    let m = now.getMinutes();
    let h = now.getHours();
    let stamp = "";
    stamp += "<span id='hour'>" + (h < 10 ? "0" + h : h) + ":</span>";
    stamp += "<span id='minute'>" + (m < 10 ? "0" + m : m) + "</span>";
    //stamp += "<span id='second'>:" + (s < 10 ? "0" + s : s) + "</span>";
    clockContainer.innerHTML = stamp;
}
