//Add clock to div
var clockContainer;

window.onload = function () {
    clockContainer = document.getElementById('js_clock');
    if (clockContainer != null) {
        this.setTimeValue();
        this.setInterval(setTimeValue, 200);
    }
}

function setTimeValue() {
    let now = new Date();
    let s = now.getSeconds();
    let m = now.getMinutes();
    let h = now.getHours();
    let stamp = "";
    stamp += "<span id='hour'>" + (h < 10 ? "0" + h : h) + ":</span>";
    stamp += "<span id='minute'>" + (m < 10 ? "0" + m : m) + "</span>";
    stamp += "<span id='second'>:" + (s < 10 ? "0" + s : s) + "</span>";
    clockContainer.innerHTML = stamp;
}