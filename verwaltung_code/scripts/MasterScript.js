function showNav()
{
    let nav = event.currentTarget.nextElementSibling;
    let nav_ar = document.getElementsByClassName("navContainer");
    let out;

    for (let i = 0; i < nav_ar.length; i++) { if (nav_ar[i].style.display != "none") out = nav_ar[i]; }


    if (out == null) {
        $(nav).slideDown("fast"); return;
    }

    if (getChildIndex(nav) > getChildIndex(out)) {
        $(out).slideUp("fast");
        $(nav).slideDown("fast");
    }
    else {
        $(nav).slideDown("fast");
        $(out).slideUp("fast");
    }

}
function getChildIndex(elem)
{
    let i = 0;
    while ((elem = elem.previousSibling) != null)++i;
    return i;
}



function scrollDown() {

    if (scrollDir == "down") {
        $('html, body').animate({
            scrollTop: $(document).height() - $(window).height()
        },
            1000,
            "easeOutQuint"
        );
    }
    else
    {
        $('html, body').animate({
            scrollTop: 0
        },
            1000,
            "easeOutQuint"
        );
    }
   
}

var lastScrollPos = pageYOffset || document.documentElement.scrollTop;
var scrollTimeout;
var scrollDir = "down";

window.onscroll = function ()
{

    let currScrollPos = window.pageYOffset || document.documentElement.scrollTop;
    if (currScrollPos > lastScrollPos) {
        document.getElementById("ScrollDown").innerHTML = "∨";
        scrollDir = "down";
    }

    else
    {
        document.getElementById("ScrollDown").innerHTML = "∧";
        scrollDir = "up";
    }

    
    lastScrollPos = currScrollPos;

    if (currScrollPos >= (document.documentElement.scrollHeight - document.documentElement.clientHeight) *0.9)
    {
        if (scrollTimeout != null) this.clearTimeout(scrollTimeout);
        document.getElementById("ScrollDown").innerHTML = "∧";
        scrollDir = "up";
        return;
    }


    $(document.getElementById("ScrollDown")).slideDown("fast");

    if (scrollTimeout != null) this.clearTimeout(scrollTimeout);
    

    scrollTimeout = setTimeout(function () {
        $(document.getElementById("ScrollDown")).slideUp("fast");
        scrollTimeout = null;
    }, 650);



}


function updateText() {
    let select = event.currentTarget;
    let label;

    let labels = document.getElementsByTagName('label');
    for (let i = 0; i < labels.length; i++) {
        if (labels[i].htmlFor != '' && document.getElementById(labels[i].htmlFor) == select) label = labels[i];
    }

    label.innerText = select.value.substring(select.value.lastIndexOf("\\")+1);

}

