$(document).ready(function () {

    initSliders();
    
    initCheckBoxes()


    // Write initial values of checkboxes to span and datafield

    $("#animateUpcomingExamLessons").trigger('change');
    $("#displayClockTile").trigger('change');
    $("#displayClassDetailsTile").trigger('change');
    $("#displayUpcomingExamsTile").trigger('change');


});

function initSliders() {

    // get init values
    let st = parseInt($("#Content_switchTimeMs_container").attr("data-init"));
    let et = parseInt($("#Content_showExamTimeMs_container").attr("data-init"));
    let ets = parseInt($("#Content_examTimesShown_container").attr("data-init"));
    let ed = parseInt($("#Content_examAnimationDuration_container").attr("data-init"));

    // initialize sliders

    $("#switchTimeMs").slider( // Slider for content view duration
        {
            min: 2000,
            max: 60000,
            value: st,
            slide: function (event, ui) {
                sliderChanged(event, ui)
            },     
            create: function (event, ui) {
                $("#Content_switchTimeMs_container").text(Math.round(st / 100) / 10);
                $("#Content_switchTimeMs").val(st);
            },
            change: function (event, ui) {
                sliderChanged(event, ui)
            }
        }
    );

    $("#showExamTimeMs").slider(
        {
            min: 1000,
            max: 1000,
            value: et,
            slide: function (event, ui) {
                sliderChanged(event, ui)
            },
            change: function (event, ui) {
                sliderChanged(event, ui)
            },
            create: function (event, ui) {
                // update value on prog. change
                $("#Content_showExamTimeMs_container").text(Math.round(et / 100) / 10);
                $("#Content_showExamTimeMs").val(et);
            }
        }
    );

    $("#examTimesShown").slider(
        {
            min: 1,
            max: getValues(st,et).length,
            value: et,
            slide: function (event, ui) {
                sliderChanged(event, ui)
            },
            change: function (event, ui) {
                sliderChanged(event, ui)
            },
            create: function (event, ui) {
                // update value on prog. change
                $("#Content_examTimesShown_container").text(((st - (et*ets))/ets + et) / 1000); // Span shows (time show + time hide) aka interval length
                $("#Content_examTimesShown").val(ets); // Data field stores how many intervals per cycle are made
            }
        }
    );

    $("#examAnimationDuration").slider(
        {
            min: 0,
            max: et/2,
            value: ed,
            slide: function (event, ui) {
                sliderChanged(event, ui)
            },
            change: function (event, ui) {
                sliderChanged(event, ui)
            },
            create: function (event, ui) {
                // update value on prog. change
                $("#Content_examAnimationDuration_container").text(ed); // Span shows (time show + time hide) aka interval length
                $("#Content_examAnimationDuration").val(ed); // Data field stores how many intervals per cycle are made
            }
        }
    );

}

function initCheckBoxes() {
    // initialize checkbox events

    $("#animateUpcomingExamLessons").change(function () {
        $("#Content_animateUpcomingExamLessons_container").text(this.checked ? "Aktiviert" : "Deaktiviert");
        $("#Content_animateUpcomingExamLessons").val(this.checked ? "true" : "false");
    });

    $("#displayClockTile").change(function () {
        $("#Content_displayClockTile_container").text(this.checked ? "Aktiviert" : "Deaktiviert");
        $("#Content_displayClockTile").val(this.checked ? "true" : "false");
    });

    $("#displayClassDetailsTile").change(function () {
        $("#Content_displayClassDetailsTile_container").text(this.checked ? "Aktiviert" : "Deaktiviert");
        $("#Content_displayClassDetailsTile").val(this.checked ? "true" : "false");
    });

    $("#displayUpcomingExamsTile").change(function () {
        $("#Content_displayUpcomingExamsTile_container").text(this.checked ? "Aktiviert" : "Deaktiviert");
        $("#Content_displayUpcomingExamsTile").val(this.checked ? "true" : "false");
    });

    // Get and set initial values

    $("#animateUpcomingExamLessons").prop('checked',
        $("#Content_animateUpcomingExamLessons_container").attr("data-init").includes("true"));

    $("#displayClockTile").prop('checked',
        $("#Content_displayClockTile_container").attr("data-init").includes("true"));

    $("#displayClassDetailsTile").prop('checked',
        $("#Content_displayClassDetailsTile_container").attr("data-init").includes("true"));

    $("#displayUpcomingExamsTile").prop('checked',
        $("#Content_displayUpcomingExamsTile_container").attr("data-init").includes("true"));
}


function sliderChanged(event, ui) {

    let switchTime = $("#switchTimeMs").slider("option", "value");
    let examTime = $("#showExamTimeMs").slider("option", "value");
    let hideTime = $("#examTimesShown").slider("option", "value");
    let animationDuration = $("#examAnimationDuration").slider("option", "value");

    switchTime = Math.round(switchTime / 1000) * 1000;
    examTime = Math.round(examTime / 100) * 100;
    animationDuration = Math.round(animationDuration / 10) * 10;


    // Set examtime max val
    $("#showExamTimeMs").slider("option", "max", switchTime / 2);
    if (examTime > switchTime / 2) {
        $("#showExamTimeMs").slider("option", "value", switchTime / 2);
        examTime = switchTime / 2;
    }

    let hideMax = getValues(switchTime, examTime).length;

    // Set hidetime max val
    $("#examTimesShown").slider("option", "max", hideMax);
    if (hideTime > hideMax) {
        $("#examTimesShown").slider("option", "value", hideMax);
        hideTime = hideMax;
    }

    // Set animation duration max val
    $("#examAnimationDuration").slider("option", "max", examTime/2);
    if (animationDuration > examTime/2) {
        $("#examTimesShown").slider("option", "value", examTime/2);
        animationDuration = examTime/2;
    }


    //Set span content
    $("#Content_switchTimeMs_container").text(switchTime/1000);
    $("#Content_examTimesShown_container").text((getValues(switchTime,examTime)[hideTime-1] + examTime)/1000);
    $("#Content_showExamTimeMs_container").text(examTime / 1000);
    $("#Content_examAnimationDuration_container").text(animationDuration / 1000);

    // Set data field content
    $("#Content_switchTimeMs").val(switchTime);
    $("#Content_examTimesShown").val(switchTime/(getValues(switchTime, examTime)[hideTime-1] + examTime));
    $("#Content_showExamTimeMs").val(examTime);
    $("#Content_examAnimationDuration").val(animationDuration);

}


function getValues(slidetime, testtime ) {

    var values = new Array();

    
    for (let hidetime = 0; hidetime <= slidetime; hidetime ++) {
        if ((slidetime % (testtime + hidetime))  == 0) values.push(hidetime);
    }

    return values;
}

function checkHit(event, id) {

    event.currentTarget.classList.toggle("unchecked");
    id.click();
}
