$(document).ready(function() {

    $(".collapseEvents").on("click", function () {
        if ($(this).hasClass("collapsed")) {
            $("#event-carousel").slideDown();
            $(".emptycarousel").slideDown();
        } else {
            $("#event-carousel").slideUp();
            $(".emptycarousel").slideUp();
        }
        $(this).children("span").toggleClass("flipped");
        $(this).toggleClass("collapsed");
    });

    $('.event-container').hover(function() {
        $(this).siblings().css('background-color', "#f7f7f7");
    });

    $('.event-container').mouseleave(function () {
        $(this).siblings().css('background-color', "#f7f7f7");
        var centre = $(this).parent().children().eq(1);
        $(centre).css('background-color', "white");
    });

    function getDateEvents(date) {
        $.ajax({
            url: "/Partials/UserDayEvents",
            dataType: "html",
            data: { date: date },
            type: "POST",
            success: function (data) {
                if (data.length > 0 && $('.collapseEvents').hasClass("collapsed")) {
                    $('.collapseEvents').click();
                    $("#event-carousel").html(data);
                }
                else if (data.length == 0 && !$('.collapseEvents').hasClass("collapsed")) {
                    $('.collapseEvents').click();
                } else {
                    $("#event-carousel").html(data);
                }
                
            }
        });
    }

    $(".teamsButton").click(function() {
        $("#mainBody").load("Teams/Index");
    });

    $('#calendar').fullCalendar({
            });

    $(".calendarButton").click(function () {
        $("#mainBody").load("Partials/UserCalendar", function () {
           
            var eventList;
            $.ajax({
                url: "/Partials/UserCalendarEvents",
                dataType: "json",
                type: "POST",
                success: function(data) {
                    eventList = data;
                    $('#calendar').fullCalendar({
                        events: eventList,
                        dayClick: function (date) {
                            getDateEvents(date.format());
                        }
                    });
                }
            });
            
        });
       
    });

});