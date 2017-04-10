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
        $("#mainBody").load("Partials/UserTeamList");
    });
    $(".eventsButton").click(function() {
        $("#mainBody").load("Partials/UserEventList");
    });
    $(".fixtureButton").click(function() {
        $("#mainBody").load("Partials/UserFixtureList");
    });
        
    $(document).on('click', ".team-widget", function () {
        var id = $(this).attr("id");
        $("#mainBody").load("Partials/TeamDetailsPartial", { id });
    });

    $(document).on('click', ".createevent a", function() {
        $("#mainBody").load("Events/Create");
    });

    $(".sportsButton").click(function() {
        $("#mainBody").load("Partials/UserSportList");
    });

    $("#addMember").click(function() {
            $(".selected-users").append($(".selected-user-list .selected").removeClass("selected"));
    });

    $(document).on('click', ".selected-user-list li", function () {
        $(this).toggleClass("selected");
    });


    $("#createTeam").click(function () {

        var team = { TeamName: $("#teamName").val(), SportId : $("#teamSport").val(), PlayerIds : $(".selected-users .id").map(function(){return $(this).text();}).get()}

        $.ajax({
            url: "/Teams/Create",
            dataType: "json",
            type: "POST",
            data: team,
            success: function (data) {

            }
        });
    });

    $(document).on('click', '#submitEvent', function () {
        var id = $("#selectedTeam").val();
        var users = $(".selected .id").map(function () { return $(this).text(); }).get();
        var type = $("#Event_Type").val();
        var start = $("#Event_Start").val();
        var end = $("#Event_End").val();
        var title = $("#Event_Title").val();
        var comments = $("#Event_Comments").val();
        var location = $("#Event_Location").val();


        //validation here
        var newEvent = { TeamId: id, Type: type, Title: title, Start: start, End: end, Location: location, UserIds: users, Comments:comments }

        $.ajax({
            url: "/Events/Create",
            dataType: "json",
            type: "POST",
            data: newEvent,
            success: function (data) {

            }
        });
    });


    var matchedMembers;

    $(function() {
        function log(message) {
            $("<div>").text(message).prependTo("#log");
            $("#log").scrollTop(0);
        }

        $("#nameBox").autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: "/User/UserList",
                    dataType: "json",
                    data: {
                        term: request.term
                    },
                    success: function (data) {
                        matchedMembers = data;
                        $(".selected-user-list").empty();
                        if (matchedMembers.length > 0) {
                            for (var i = 0; i < matchedMembers.length; i++) {
                                $(".selected-user-list").append("<li><span hidden class=\"id\">" + matchedMembers[i].Id + "</span><span class=\"name\">" + matchedMembers[i].Name + "</span><span class=\"sport\">" + matchedMembers[i].Sport + "</span></li>");
                            }
                        } 
                    }

                });
            },
            minLength: 2,
            select: function(event, ui) {
                log("Selected: " + ui.item.value + " aka " + ui.item.id);
            }
        });
    });

    $(document).on('change', '#selectedTeam', function () {
        $(".selected-users").empty();
        $.ajax({
            url: "/Partials/GetTeamMembers",
            dataType: "json",
            data: {
                id: $(this).val()
            },
            success: function (data) {
                matchedMembers = data;
                $(".selected-user-list").empty();
                if (matchedMembers.length > 0) {
                    for (var i = 0; i < matchedMembers.length; i++) {
                        //$(".selected-users").append("<li><span hidden class=\"id\">" + matchedMembers[i].Id + "</span><span class=\"name\">" + matchedMembers[i].Name + "</span></li>");
                        $(".selected-users").append("<li class=\" col-lg-6 user-widget\"><div class=\"image\"></div><span hidden class=\"id\">" + matchedMembers[i].Id + "</span><span class=\"name\">" + matchedMembers[i].Name + "</span></li>");

                    }
                }
            }
        });
    });


    $(document).on('click', ".selected-users li", function() {
        $(this).toggleClass("selected");
    });

    $(document).on('click', "#inviteall", function() {
        $(".selected-users li").toggleClass("selected");
    });


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