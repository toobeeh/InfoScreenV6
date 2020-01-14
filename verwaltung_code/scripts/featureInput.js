$(document).ready(function () {


    let st = parseInt($("#Content_switchTimeMs_container").attr("data-init"));
    let et = parseInt($("#Content_showExamTimeMs_container").attr("data-init"));
    let ets = parseInt($("#Content_examTimesShown_container").attr("data-init"));

    let sliders = document.getElementsByClassName("jQuerySlider");

    // initialize sliders

    $("#switchTimeMs").slider(
        {
            min: 1000,
            max: 60000,
            value: st,
            slide: function (event, ui) {

                let switchTime = ui.value;
                let examTime = $("#showExamTimeMs").slider("option", "value");
                let examTimesShown = $("#showExamTimesShown").slider("option", "value");

                //Set span content
                $("#Content_switchTimeMs_container").text(Math.round(switchTime/100)/10);

                // Set examtime max val
                $("#showExamTimeMs").slider("option", "max", switchTime/2);
                if (examTime > switchTime/2) $("#showExamTimeMs").slider("option", "value", switchTime/2);

                // Set examtimesshown max val
                $("#examTimesShown").slider("option", "max", switchTime / examTime );
                if (examTimesShown > switchTime / examTime)
                    $("#examTimesShown").slider("option", "value", switchTime / examTime);
            },
            create: function (event, ui) {
                // update value on prog. change
                $("#Content_switchTimeMs_container").text(Math.round(st / 100) / 10);
            }
        }
    );

    $("#showExamTimeMs").slider(
        {
            min: 1000,
            max: 1000,
            value: et,
            slide: function (event, ui) {

                let switchTime = $("#switchTimeMs").slider("option", "value");
                let examTime = ui.value;
                let examTimesShown = $("#showExamTimesShown").slider("option", "value");

                // set span content
                $("#Content_showExamTimeMs_container").text(Math.round(examTime / 100) / 10);

                // Set examtimesshown max val
                $("#examTimesShown").slider("option", "max", switchTime /examTime);
                if ($("#examTimesShown").slider("option", "value") > switchTime / examTime)
                    $("#examTimesShown").slider("option", "value", switchTime / examTime);

             },
            change: function (event, ui) {
                // update value on prog. change
                $("#Content_showExamTimeMs_container").text(Math.round(ui.value / 100) / 10);
            },
            create: function (event, ui) {
                // update value on prog. change
                $("#Content_showExamTimeMs_container").text(Math.round(et / 100) / 10);
            }
        }
    );

    $("#examTimesShown").slider(
        {
            min: 1,
            max: 100,
            value: ets,
            slide: function (event, ui) {
                $("#Content_examTimesShown_container").text(Math.round(ui.value*10)/10);
            },
            change: function (event, ui) {
                // update value on prog. change
                $("#Content_examTimesShown_container").text(Math.round(ui.value * 10) / 10);
            },
            create: function (event, ui) {
                // update value on prog. change
                $("#Content_examTimesShown_container").text(Math.round(ets * 10) / 10);
            }
        }
    );

    // initialize checkboxe events

    $("#animateUpcomingExamLessons").change(function () {
        $("#Content_animateUpcomingExamLessons_container").text(this.checked ? "Aktiviert" : "Deaktiviert");
    });

    $("#displayClockTile").change(function () {
        $("#Content_displayClockTile_container").text(this.checked ? "Aktiviert" : "Deaktiviert");
    });

    $("#displayClassDetailsTile").change(function () {
        $("#Content_displayClassDetailsTile_container").text(this.checked ? "Aktiviert" : "Deaktiviert");
    });

    $("#displayUpcomingExamsTile").change(function () {
        $("#Content_displayUpcomingExamsTile_container").text(this.checked ? "Aktiviert" : "Deaktiviert");
    });

    // Get initial values

    $("#animateUpcomingExamLessons").prop('checked',
        $("#Content_animateUpcomingExamLessons_container").attr("data-init").includes("true") ? true : false);

    $("#displayClockTile").prop('checked',
        $("#Content_displayClockTile_container").attr("data-init").includes("true") ? true : false);

    $("#displayClassDetailsTile").prop('checked',
        $("#Content_displayClassDetailsTile_container").attr("data-init").includes("true") ? true : false);

    $("#displayUpcomingExamsTile").prop('checked',
        $("#Content_displayUpcomingExamsTile_container").attr("data-init").includes("true") ? true : false);

    // Write initial values to span

    $("#animateUpcomingExamLessons").trigger('change');
    $("#displayClockTile").trigger('change');
    $("#displayClassDetailsTile").trigger('change');
    $("#displayUpcomingExamsTile").trigger('change');


});

