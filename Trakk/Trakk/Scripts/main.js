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


    $(document).on('click', ".editevent a", function () {
        var id = $(this).parent().parent().attr("id");
        $("#mainBody").load("Events/Edit", { id });
    });

    $(".sportsButton").click(function() {
        $("#mainBody").load("Partials/UserSportList");
    });


    var currentMembers = [];

    $("#addMember").click(function () {
        $(".selected-users .id").each(function() {
            currentMembers.push($(this).text());
        });


        $(".selected-user-list .selected").each(function() {
            var newId = $(this).children(".id").text();
            if (!currentMembers.includes(newId)) {
                $(".selected-users").append("<li class=\" col-xs-6 user-widget\"><div class=\"image\"></div><span hidden class=\"id\">" + $(this).children(".id").text() + "</span><span class=\"name\">" + $(this).children(".name").text() + "</span><span class=\"glyphicon glyphicon-remove removeMember\"></span></li>");
            }
        });
        currentMembers = [];
    });

    $(document).on('click', ".selected-user-list li", function () {
        $(this).toggleClass("selected");
    });


    $("#createTeam").click(function () {
        var players = $(".selected-users .id").map(function() { return $(this).text(); }).get();
        players.push($(".userid").text());
        var team = { TeamName: $("#teamName").val(), SportId : $("#teamSport").val(), PlayerIds : players}

        $.ajax({
            url: "/Teams/Create",
            dataType: "json",
            type: "POST",
            data: team,
            success: function (data) {

            }
        });
    });

    $(document).on('click', ".pitchFrame div", function (e) {
        e.stopPropagation();
    });

    $(".pitchFrame img").click(function (e) {
        $(".clicked span").fadeOut();
        $(".clicked").removeClass("clicked");
        var posX = $(this).parent().position().left, posY = $(this).parent().position().top;
        var pitchframeheight = $(".pitchFrame").height();
        var pitchframewidth = $(".pitchFrame").width();
        var xcoord = e.pageX - posX;
        var ycoord = e.pageY - posY;
        var xpercent = xcoord / pitchframewidth * 100;
        var ypercent = ycoord / pitchframeheight * 100;
        $(".saveButton").removeClass("saved");
        $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
        $("<div style=\"position: absolute; top:" + Math.round(ypercent) + "%; left:" + Math.round(xpercent) + "% \"; class=\"pitchLocation draggable\"><input type=\"text\" placeholder=\"Position\"></input></div>").appendTo('.pitchFrame').draggable({
            start: function () {

            },
            drag: function () {

            },
            stop: function () {
                $(".saveButton").removeClass("saved");
                $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
            }
        });
    });


    $(".saveButton").click(function() {
        var positions = [];
        $(".pitchLocation").each(function() {
            var xcoord  = parseInt($(this).css('left'));
            var ycoord = parseInt($(this).css('top'));
            var pitchframeheight = $(".pitchFrame").height();
            var pitchframewidth = $(".pitchFrame").width();
            var xpercent = Math.round(xcoord / pitchframewidth * 100);
            var ypercent = Math.round(ycoord / pitchframeheight * 100);
            var positiontext = $(this).children("input").val();

            var position = { top: ypercent, left: xpercent, text: positiontext }
            positions.push(position);
        });
        var name = $("#formation-name").val();
        var fixtureid = $("#fixture-id").text();
        var teamid = $("#homeTeam").val();
        var id = $(".pitchFrame").attr("id");
       

        if ($(".saveButton").hasClass('indb')) {

            var updateFormation = { Id: id, TeamId: teamid, FormationJson: JSON.stringify(positions), Name: name }

            $.ajax({
                url: "/Fixtures/UpdateFormation",
                dataType: "json",
                type: "POST",
                data: updateFormation,
                success: function(data) {
                    if (data.Success) {
                        $(".saveButton").addClass("saved");
                        $(".saveButton span").removeClass("glyphicon-save").addClass("glyphicon-saved");
                        $(".saveButton").addClass('indb');
                    }
                }
            });
        } else {

            var newFormation = { TeamId: teamid, FormationJson: JSON.stringify(positions), Name: name }

            $.ajax({
                url: "/Fixtures/CreateFormation",
                dataType: "json",
                type: "POST",
                data: newFormation,
                success: function(data) {
                    if (data.Success) {
                        $(".saveButton").addClass("saved");
                        $(".saveButton span").removeClass("glyphicon-save").addClass("glyphicon-saved");
                        $(".saveButton").addClass('indb');
                        $(".pitchFrame").attr('id', data.IdReturn);
                    }
                }
            });
        }
        $("#homeTeam").trigger('change');
    });

    $(document).on('change', "#homeTeam", function() {
        var teamId = $(this).val();
        $(".formations-bar").load("/Fixtures/GetTeamFormations", {teamId});
    });

    $("#homeTeam").trigger('change');

    $(document).on('click', '.formation-widget', function () {
        $(".selected").removeClass("selected");
        $(this).children(".formation-circle").addClass("selected");
        var teamid = $("#homeTeam").val();
        var id = $(this).attr('id');
        $.ajax({
            url: "/Fixtures/GetFormation",
            dataType: "json",
            type: "POST",
            data: {teamId : teamid, formationId: $(this).attr("id")},
            success: function (data) {

                $(".saveButton").addClass("saved");
                $(".saveButton span").removeClass("glyphicon-save").addClass("glyphicon-saved");
                $(".saveButton").addClass('indb');
                $(".pitchFrame").attr('id', id);
                $(".pitchLocation").remove();
                var position = JSON.parse(data.FormationJson);

                $("#formation-name").val(data.Name);
                for (var i = 0; i < position.length; i++) {
                    var name = position[i].text;
                    if (name == undefined) {
                        name = "";
                    }
                    $("<div style=\"position: absolute; top:" + position[i].top + "%; left:" + position[i].left + "% \"; class=\"pitchLocation draggable\"><input type=\"text\" value=\""+name+"\" placeholder=\"Position\"></input></div>").appendTo('.pitchFrame').draggable({
                        start: function () {

                        },
                        drag: function () {

                        },
                        stop: function () {
                            $(".saveButton").removeClass("saved");
                            $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
                        }
                    });
   
                }
            }
        });
    });

    $(document).on('input', ".pitchLocation input", function() {
        $(".saveButton").removeClass("saved");
        $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
    });

    $(document).on('mousedown', '.pitchLocation', function (event) {
        switch (event.which) {
            case 1:
                if ($(this).hasClass("clicked")) {
                    $(this).remove();
                    $(".saveButton").removeClass("saved");
                    $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
                } 
                break;
            case 2:
                break;
            case 3:
                event.preventDefault();
                if ($(this).hasClass("clicked")) {
                    $(".clicked span").hide();
                    $(".clicked").removeClass("clicked");
                    $(this).children("input").fadeIn();
                } else {
                    $(this).children("input").hide();
                    $(this).addClass("clicked").append("<span class=\"glyphicon glyphicon-trash\"></span>");
                }

                break;
            default:
    }
    });

    $(".Box").contextmenu(function (e) {

    });



    $("#editTeam").click(function () {

        var team = { TeamId: $(".form-horizontal").attr("id"), TeamName: $("#teamName").val(), SportId : $("#teamSport").val(), PlayerIds : $(".selected-users .id").map(function(){return $(this).text();}).get()}

        $.ajax({
            url: "/Teams/Edit",
            dataType: "json",
            type: "POST",
            data: team,
            success: function (data) {

            }
        });
    });

    $(document).on('click',".removeMember",function() {
        $(this).parent().remove();
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
        var token = $('input[name="__RequestVerificationToken"]').val();

        var headers = {};

        headers['__RequestVerificationToken'] = token;

        $.ajax({
            url: "/Events/Create",
            dataType: "json",
            type: "POST",
            data: newEvent,
            success: function (data) {
            }
        });
    });


    $(document).on('click', '#editEvent', function () {
        var id = $(".eventId").attr("id");
        var teamid = $(".teamId").attr("id");
        var users = $(".selected .id").map(function () { return $(this).text(); }).get();
        var type = $("#Event_Type").val();
        var start = $("#Event_Start").val();
        var end = $("#Event_End").val();
        var title = $("#Event_Title").val();
        var comments = $("#Event_Comments").val();
        var location = $("#Event_Location").val();


        //validation here
        var newEvent = { EventId: id, TeamId: teamid,Type: type, Title: title, Start: start, End: end, Location: location, UserIds: users, Comments: comments }

        $.ajax({
            url: "/Events/Edit",
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
                        $(".selected-users").append("<li class=\" col-xs-6 user-widget\"><div class=\"image\"></div><span hidden class=\"id\">" + matchedMembers[i].Id + "</span><span class=\"name\">" + matchedMembers[i].Name + "</span><span class=\"glyphicon glyphicon-remove removeMember\"></span></li>");

                    }
                }
            }
        });
    });



    $(document).on('click', ".selected-users li", function() {
        $(this).toggleClass("selected");
    });

    $(document).on('click', "#inviteall", function() {
        if ($(".selected-users li").each(function() {
            if (!$(this).hasClass("selected")) {
                $(this).addClass("selected");
            }
        }));
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

    $(".calendarButton").trigger('click');

});