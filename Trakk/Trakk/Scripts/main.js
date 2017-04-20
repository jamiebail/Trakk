$(document).ready(function() {


    // Initialisers
    refreshFormationBar();


    $(".collapseEvents").on("click", function() {
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

    $('.event-container').mouseleave(function() {
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
            success: function(data) {
                if (data.length > 0 && $('.collapseEvents').hasClass("collapsed")) {
                    $('.collapseEvents').click();
                    $("#event-carousel").html(data);
                } else if (data.length == 0 && !$('.collapseEvents').hasClass("collapsed")) {
                    $('.collapseEvents').click();
                } else {
                    $("#event-carousel").html(data);
                }


                $(".playing-status-button").click(function() {
                    if ($(this).hasClass("open")) {
                        $(this).siblings(".availability-update").fadeOut();
                        $(this).parent().parent().find(".playing-status-button").animate({ opacity: 1 }, 200);
                        $(this).toggleClass("open");
                    } else {
                        $(this).siblings(".availability-update").fadeIn();
                        $(this).parent().parent().find(".playing-status-button").animate({ opacity: 0.2 }, 200);
                        $(this).toggleClass("open");
                    }
                });

                $(".availability-update .playing-status").click(function() {
                    var containertype = $(this).closest(".event-container");
                    var button = $(this);
                    var id = containertype.attr("id");
                    var text = $(this).text();
                    var color = $(this).css("background-color");
                    var attendance = $(this).attr("id");
                    if ($(containertype).hasClass("fixture-container")) {
                        $.ajax({
                            url: "/Fixtures/CreateAvailability",
                            dataType: "html",
                            data: { availability: attendance, eventId: id },
                            type: "POST",
                            success: function(data) {
                                $(button).parent().parent().find(".playing-status-button").css("background-color", color);
                                $(button).parent().parent().find(".playing-status-button").text(text);
                                $(button).parent().parent().find(".playing-status-button").trigger('click');
                            }
                        });
                    } else {
                        $.ajax({
                            url: "/Events/CreateAvailability",
                            dataType: "html",
                            data: { availability: attendance, eventId: id },
                            type: "POST",
                            success: function(data) {
                                $(button).parent().parent().find(".playing-status-button").css("background-color", color);
                                $(button).parent().parent().find(".playing-status-button").text(text);
                                $(button).parent().parent().find(".playing-status-button").trigger('click');
                            }
                        });
                    }
                });
            }
        });
    }


    $(document).on('click', ".playing-status-button", function() {
        if ($(this).hasClass("open")) {
            $(this).siblings(".availability-update").fadeOut();
            $(this).parent().parent().find(".playing-status-button").animate({ opacity: 1 }, 200);
            $(this).toggleClass("open");
        } else {
            $(this).siblings(".availability-update").fadeIn();
            $(this).parent().parent().find(".playing-status-button").animate({ opacity: 0.2 }, 200);
            $(this).toggleClass("open");
        }
    });

    $(document).on('click', ".fixture-widget", function() {
        var id = $(this).attr("id");
        $("#mainBody").load("Partials/UserFixtureDetails", { id }, function() {
            $(".pitchLocation-static input").prop('disabled', true);
            initialiseAutocompletes();
        });
    });

    $(document).on('click', ".fixture-container", function() {
        var id = $(this).attr("id");
        $("#mainBody").load("Partials/UserFixtureDetails", { id }, function() {
            $(".pitchLocation-static input").prop('disabled', true);
            initialiseAutocompletes();
        });

    });

    var data = [];
    function initialiseAutocompletes() {
        data = [];
        $(".available-List li").each(function() {
            var player = { value: $(this).attr("id"), label: $(this).text() };
            data.push(player);
        });

    }


    $(".collapsed-navbar li").click(function() {
        if (!$(".main-toggle").hasClass("collapsed")) {
            $(".navbar-toggle").trigger('click');
        }
    });

    $(document).on('input', "#homeScore", function () {
        var rows = $(this).val();
        initialiseAutocompletes();
        $(".goals-form ul").empty();
        if (rows <= 20) {
            var count = 0;
            for (var i = 0; i < rows; i++) {
                count++;
                $(".goals-form ul").append("<li class=\"row\"><h5 class=\"col-md-2\">Goal " + count + "</h5><input class=\"col-md-5 form-control pname \" type=\"text\" placeholder=\"Player Name\"/><div class=\"match-list\"></div><input class=\"timeinput col-md-offset-1 col-md-4 form-control \" type=\"text\" placeholder=\"Minute\"/></li><hr style=\"clear:both\"/>");
            }
            $(".goals-form .pname").autocomplete({
                source: data,
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    $(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $(this).attr('id', ui.item.value);
                }
            });
        };
    });

    $(document).on('click', ".submit-report", function () {
        $(".validation-error").removeClass("validation-error");
        var validationfail = false;
        var homescore = $("#homeScore").val();
        var awayscore = $("#awayScore").val();
        var fixtureid = $(".fixture-id").attr("id");
        var teamid = $(".team").text();
        var elements = [];
        var goals = [];
        var cards = [];
        $(".goals-form .row").each(function () {
            var id = $(this).children(".pname").attr("id");
            var minute = $(this).children('.timeinput').val(); 
            if (id == undefined) {
                elements.push($(this));
            }
            if (minute === "") {
                elements.push($(this));
            }
            if(id !== undefined && minute !== ""){
                var goal = { ScorerId: id, Minute: minute }
                goals.push(goal);
            }
        });

        $(".cards-form .row").each(function () {
            var colour = $(this).children().find(".selected-card").attr("id");
            var id = $(this).children("input").attr("id");
            if (id == undefined) {
                elements.push($(this));
            } if (colour == undefined) {
                elements.push($(this));
            } if (id !== undefined && colour !== undefined) {
                var card = { CardColour: colour, PlayerId: id }
                cards.push(card);
            }

        });

        if (elements.length > 0) {
            for (var i = 0; i < elements.length; i++) {
                $(elements[i]).addClass("validation-error");
            }
        } else {
            var reportIn = { TeamId: teamid, FixtureId: fixtureid ,HomeScore: homescore, AwayScore: awayscore, Goals: goals, Cards: cards }
            $.ajax({
                url: "/Fixtures/CreateReport",
                dataType: "html",
                data: reportIn,
                type: "POST",
                success: function (data) {
                    alert(data.message);
                }
            });
        }
    });

    function submitAcceptance(button, accepted) {
        var id = $(button).parent().attr("id");
       $.ajax({
            url: "/Teams/SubmitInvite",
            dataType: "json",
            data: { teamId: id, accepted: accepted },
            type: "POST",
            success: function (data) {
                alert(data.message);
            }
            });
    }

    $(document).on('click', '.accept-invite', function() {
        submitAcceptance($(this), true);
    });
    $(document).on('click', '.decline-invite', function() {
        submitAcceptance($(this), false);
    });


    $(document).on('input', ".cards-form > input", function () {
        var rows = $(this).val();
        initialiseAutocompletes();
        $(".cards-form ul").empty();
        if (rows <= 20) {
            var count = 0;
            for (var i = 0; i < rows; i++) {
                count++;
                $(".cards-form ul").append("<li class=\"row\"><h5 class=\"col-md-2\">Card " + count + "</h5><input class=\"col-md-5 form-control pname \" type=\"text\" placeholder=\"Player Name\"/><div class=\"cardframe\"><img id=\"Yellow\" src=\"/Images/yellow.png\"\> <img id=\"Red\" src=\"/Images/red.png\"\> <img id=\"Green\" src=\"/Images/green.png\"\></div> </li><hr style=\"clear:both\"/>");
            }
            $(".cards-form .pname").autocomplete({
                source: data,
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    $(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $(this).attr('id', ui.item.value);
                }
            });
        };
    });


    $(document).on('click', ".cardframe img", function () {
        $(this).siblings().removeClass("selected-card");
        $(this).toggleClass("selected-card");
    });
    

    $(document).on('click', ".event-container .availability-update .playing-status", function () {
        var containertype = $(this).closest(".event-container");
        var button = $(this);
        var id = containertype.attr("id");
        var text = $(this).text();
        var color = $(this).css("background-color");
        var attendance = $(this).attr("id");
        if ($(containertype).hasClass("fixture-container")) {
            $.ajax({
                url: "/Fixtures/CreateAvailability",
                dataType: "html",
                data: { availability: attendance, eventId: id },
                type: "POST",
                success: function(data) {
                    $(button).parent().parent().find(".playing-status-button").css("background-color", color);
                    $(button).parent().parent().find(".playing-status-button").text(text);
                    $(button).parent().parent().find(".playing-status-button").trigger('click');
                }
            });
        } else {
            $.ajax({
                url: "/Events/CreateAvailability",
                dataType: "html",
                data: { availability: attendance, eventId: id },
                type: "POST",
                success: function (data) {
                    $(button).parent().parent().find(".playing-status-button").css("background-color", color);
                    $(button).parent().parent().find(".playing-status-button").text(text);
                    $(button).parent().parent().find(".playing-status-button").trigger('click');
                }
            });
        }
    });
    
    $(document).on('click', ".team-member-widget-static .playing-status", function () {
        var id = $(".pitchFrame-static").attr("id");
        var text = $(this).text();
        var color = $(this).css("background-color");
        var attendance = $(this).attr("id");
        var button = $(this);
            $.ajax({
                url: "/Fixtures/CreateAvailability",
                dataType: "html",
                data: { availability: attendance, eventId: id },
                type: "POST",
                success: function (data) {
                    $(button).parent().parent().find(".playing-status-button").css("background-color", color);
                    $(button).parent().parent().find(".playing-status-button").text(text);
                    $(button).parent().parent().find(".playing-status-button").trigger('click');
                }
            });
    });



    $(".teamsButton").click(function() {
        $("#mainBody").load("Partials/UserTeamList");
    });
    $(".eventsButton").click(function() {
        $("#mainBody").load("Partials/UserEventList");
    });
    $(".fixtureButton").click(function() {
        $("#mainBody").load("Partials/UserFixtureList");
    });

    $(document).on('click', ".team-widget", function() {
        var id = $(this).attr("id");
        $("#mainBody").load("Partials/TeamDetailsPartial", { id });
    });

    $(document).on('click', ".createevent a", function() {
        $("#mainBody").load("Events/Create");
    });


    $(document).on('click', ".editevent a", function() {
        var id = $(this).parent().parent().attr("id");
        $("#mainBody").load("Events/Edit", { id });
    });

    $(".sportsButton").click(function() {
        $("#mainBody").load("Partials/UserSportList");
    });


    var currentMembers = [];

    $("#addMember").click(function() {
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

    $(document).on('click', ".selected-user-list li", function() {
        $(this).toggleClass("selected");
    });


    $("#makeCaptain").click(function () {
        $(".selected-users .selected").addClass("Captain").removeClass("selected");

    });

    $("#makeCommittee").click(function () {
        $(".selected-users .selected").addClass("Committee").removeClass("selected");
    });

    $("#editTeam").click(function () {
        var players = [];
        var teamid= $(".form-horizontal").attr("id");
        $(".selected-users li").each(function () {
            var userid = $(this).children(".id").html();
            if ($(this).hasClass("Captain")) {
                var role = { userId: userid, role: 2 , teamid: teamid}
                players.push(role);
            }
            else if ($(this).hasClass("Committee")) {
                var role = { userId: userid, role: 1, teamid: teamid }
                players.push(role);
            }
            else {
                var role = { userId: userid, role: 0, teamid: teamid }
                players.push(role);
            }
        });
        var team = {  TeamId: teamid, TeamName: $("#teamName").val(), SportId: $("#teamSport").val(), Roles: players }

        $.ajax({
            url: "/Teams/Edit",
            dataType: "json",
            type: "POST",
            data: team,
            success: function (data) {

            }
        });
    });

    $("#createTeam").click(function () {
        var players = [];
        $(".selected-users li").each(function () {
            var userid = $(this).children(".id").html();
            if ($(this).hasClass("Captain")) {
                var role = { userId: userid, role: 2 }
                players.push(role);
            }
            else if ($(this).hasClass("Committee")) {
                var role = { userId: userid, role: 1 }
                players.push(role);
            }
            else{
                var role = { userId: userid, role: 0 }
                players.push(role);
            }
        });
        var userid =  $(".userid").text();
        players.push({ UserId: userid, role: 2 });
        var team = { TeamName: $("#teamName").val(), SportId: $("#teamSport").val(), Roles: players }

        $.ajax({
            url: "/Teams/Create",
            dataType: "json",
            type: "POST",
            data: team,
            success: function(data) {

            }
        });
    });

    $(document).on('click', ".pitchFrame div", function(e) {
        e.stopPropagation();
    });


    $(".editbutton").click(function () {
        $(".plusbutton").removeClass("selected");
        if ($(this).hasClass("selected")) {
            $(".pitchLocation").draggable('disable');
        } else {
            $(".pitchLocation").draggable({
                start: function() {

                },
                drag: function() {

                },
                stop: function() {
                    $(".saveButton").removeClass("saved");
                    $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
                },
                containment: ".pitchFrame img"
            });
            $(".pitchLocation").draggable('enable');
        }
        $(this).toggleClass("selected");
    });


    $(".plusbutton").click(function () {
        $(".editbutton").removeClass("selected");
        $(this).toggleClass("selected");
        $("#formation-name").val("");
        $(".saveButton").removeClass('indb');
        $(".saveButton").removeClass("saved");
        $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
        $(".pitchLocation").remove();
        $(".formation-circle").removeClass("selected");
    });

    $(".pitchFrame img").click(function(e) {
        if ($(".editbutton").hasClass("selected") || $(".plusbutton").hasClass("selected")) {
            $(".clicked span").fadeOut();
            $(".clicked").removeClass("clicked");
            var posX = $(this).parent().position().left, posY = $(this).parent().position().top;
            var pitchframeheight = $(".pitchFrame img").height();
            var pitchframewidth = $(".pitchFrame img").width();
            var xcoord = e.pageX - posX;
            var ycoord = e.pageY - posY;
            var xpercent = xcoord / pitchframewidth * 100;
            var ypercent = ycoord / pitchframeheight * 100;
            $(".saveButton").removeClass("saved");
            $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
            $("<div style=\"position: absolute; top:" + Math.round(ypercent) + "%; left:" + Math.round(xpercent) + "% \"; class=\"pitchLocation draggable\"><input type=\"text\" placeholder=\"Position\"></input></div>").appendTo('.pitchFrame').draggable({
                start: function() {

                },
                drag: function() {

                },
                stop: function() {
                    $(".saveButton").removeClass("saved");
                    $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
                },
                containment: ".pitchFrame img"

            }).droppable({
                hoverClass: 'drop-hover',
                drop: function (event, ui) {
                    var draggable = $(ui.draggable[0]),
                        draggableOffset = draggable.offset(),
                        container = $(event.target),
                        containerOffset = container.offset();

                    $('.user-widget', event.target).appendTo(droppableParent).css({ opacity: 0 }).animate({ opacity: 1 }, 200);

                    draggable.appendTo(container).css({ left: draggableOffset.left - containerOffset.left, top: draggableOffset.top - containerOffset.top }).animate({ left: 0, top: 0 }, 200);
                }
            });
        }
    });


    $(".saveButton").click(function() {
        var positions = [];
        $(".pitchLocation").each(function() {
            var xcoord = parseInt($(this).css('left'));
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
        var teamid; 
        if ($("#homeTeam").length === 0) {
            teamid =
            $("#homeTeamEdit").text();
        } else {
           teamid = $("#homeTeam").val();
        }

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
                        refreshFormationBar();
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
                        refreshFormationBar();
                        $(".saveButton span").removeClass("glyphicon-save").addClass("glyphicon-saved");
                        $(".saveButton").addClass('indb');
                        $(".pitchFrame").attr('id', data.IdReturn);
                    }
                }
            });
        }

    });



    $(document).on('change', "#homeTeam", function() {
        var teamId = $(this).val();
        $(".formation-widget").remove();
        $(".pitchLocation").remove();
        $(".team-member-widget").load("/Partials/TeamMembersPartial", { id: teamId }, function() {
            $(".user-list").droppable({
                hoverClass: 'drop-hover',
                drop: function (event, ui) {
                    var draggable = $(ui.draggable[0]),
                        draggableOffset = draggable.offset(),
                        container = $(event.target),
                        containerOffset = container.offset();
                    refreshFormationBar();

                    draggable.appendTo(container).css({ left: draggableOffset.left - containerOffset.left, top: draggableOffset.top - containerOffset.top }).animate({ left: 0, top: 0 }, 200);

                }
            });
        });

        $(".user-widget").draggable({
            start: function () {

            },
            drag: function () {

            },
            stop: function () {
                $(".saveButton").removeClass("saved");
                $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
            },
            containment: "#formation-interface"
        });
       
    });



    function refreshFormationBar() {
        var teamid;
        if ($("#homeTeam").length === 0) {
            teamid =$("#homeTeamEdit").text();
        } else {
            teamid = $("#homeTeam").val();
        }
        $(".formations-bar").load("/Fixtures/GetTeamFormations", { teamId: teamid, second: false }, function () {
            if ($(".formations-bar").children().length === 4) {
                $(".formations-bar-second").load("/Fixtures/GetTeamFormations", { teamId: teamid, second: true });
            }
        });
    }

    $("#homeTeam").trigger('change');

    $(document).on('click', '.formation-widget', function() {
        $(".selected").removeClass("selected");
        $(this).children(".formation-circle").addClass("selected");
        var teamid;
        if ($("#homeTeam").length === 0) {
            teamid = $("#homeTeamEdit").text();
        } else {
            teamid = $("#homeTeam").val();
        }
        var id = $(this).attr('id');
        $.ajax({
            url: "/Fixtures/GetFormation",
            dataType: "json",
            type: "POST",
            data: { teamId: teamid, formationId: $(this).attr("id") },
            success: function(data) {

                $(".saveButton").addClass("saved");
                $(".saveButton span").removeClass("glyphicon-save").addClass("glyphicon-saved");
                $(".saveButton").addClass('indb');
                $(".pitchFrame").attr('id', id);
                $(".user-widget").appendTo(".user-list");
                

                $(".pitchLocation").remove();
             
                $(".saveButton").removeClass("saved");
                $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
            
                var position = JSON.parse(data.FormationJson);

                $("#formation-name").val(data.Name);
                for (var i = 0; i < position.length; i++) {
                    var name = position[i].text;
                    if (name == undefined) {
                        name = "";
                    }
                    $("<div style=\"position: absolute; top:" + position[i].top + "%; left:" + position[i].left + "% \"; class=\"pitchLocation draggable\"><input type=\"text\" value=\"" + name + "\" placeholder=\"Position\"></input></div>").appendTo('.pitchFrame').draggable({
                        start: function () {

                        },
                        drag: function () {

                        },
                        stop: function () {
                            $(".saveButton").removeClass("saved");
                            $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
                        },
                        containment: ".pitchFrame img"

                    }).droppable({
                        hoverClass: 'drop-hover',
                        drop: function (event, ui) {
                            var draggable = $(ui.draggable[0]),
                                draggableOffset = draggable.offset(),
                                container = $(event.target),
                                containerOffset = container.offset();

                            $('.user-widget', event.target).appendTo(droppableParent).css({ opacity: 0 }).animate({ opacity: 1 }, 200);

                            draggable.appendTo(container).css({ left: draggableOffset.left - containerOffset.left, top: draggableOffset.top - containerOffset.top }).animate({ left: 0, top: 0 }, 200);
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

    $(document).on('mousedown', '.pitchLocation', function(event) {
        switch (event.which) {
        case 1:
            if ($(this).hasClass("clicked")) {
                $(this).remove();
                $(this).children(".user-widget").appendTo(".user-list").draggable({
                    start: function() {

                    },
                    drag: function() {

                    },
                    stop: function() {
                        $(".saveButton").removeClass("saved");
                        $(".saveButton span").removeClass("glyphicon-saved").addClass("glyphicon-save");
                    },
                    containment: "#formation-interface"
                });
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

    $(".Box").contextmenu(function(e) {

    });



    $(document).on('click', ".removeMember", function() {
        $(this).parent().remove();
    });

    $(document).on('click', '#submitEvent', function() {
        var id = $("#selectedTeam").val();
        var users = $(".selected .id").map(function() { return $(this).text(); }).get();
        var type = $("#Event_Type").val();
        var start = $("#Event_Start").val();
        var end = $("#Event_End").val();
        var title = $("#Event_Title").val();
        var comments = $("#Event_Comments").val();
        var location = $("#Event_Location").val();


        //validation here
        var newEvent = { TeamId: id, Type: type, Title: title, Start: start, End: end, Location: location, UserIds: users, Comments: comments }
        var token = $('input[name="__RequestVerificationToken"]').val();

        var headers = {};

        headers['__RequestVerificationToken'] = token;

        $.ajax({
            url: "/Events/Create",
            dataType: "json",
            type: "POST",
            data: newEvent,
            success: function(data) {
            }
        });
    });

    $("#submitFixture").click(function() {
        var homeid = $("#homeTeam").val();
        var awayid = $("#awayTeam").val();
        var start = $("#Fixture_Start").val();
        var end = $("#Fixture_End").val();
        var comments = $("#Fixture_Comments").val();
        var location = $("#Fixture_Location").val();
        
        var playerpositions = [];
        $('.pitchLocation').each(function() { 
            var xcoord = parseInt($(this).css('left'));
            var ycoord = parseInt($(this).css('top'));
            var pitchframeheight = $(".pitchFrame").height();
            var pitchframewidth = $(".pitchFrame").width();
            var xpercent = Math.round(xcoord / pitchframewidth * 100);
            var ypercent = Math.round(ycoord / pitchframeheight * 100);
            var positiontext = $(this).children("input").val();

            var position = { index: $(this).index(), playerid: $(this).children("li").attr("id"), playername: $(this).children("li").children(".username").text(), positionname: positiontext, top: ypercent,left:  xpercent}
            playerpositions.push(position);
        });
        var fixture = { homeId: homeid, awayId: awayid, start: start, end: end, comments: comments, location: location, positions: JSON.stringify(playerpositions) }

        $.ajax({
            url: "/Fixtures/Create",
            dataType: "json",
            type: "POST",
            data: fixture,
            success: function (data) {

            }
        });
    });


    $("#editFixture").click(function () {
        var homeid = $("#homeTeamEdit").text();
        var awayid = $("#awayTeamEdit").text();
        var start = $("#Fixture_Start").val();
        var end = $("#Fixture_End").val();
        var comments = $("#Fixture_Comments").val();
        var location = $("#Fixture_Location").val();
        var id = $(".pitchFrame").attr("id");

        var playerpositions = [];
        $('.pitchLocation').each(function () {
            var xcoord = parseInt($(this).css('left'));
            var ycoord = parseInt($(this).css('top'));
            var pitchframeheight = $(".pitchFrame").height();
            var pitchframewidth = $(".pitchFrame").width();
            var xpercent = Math.round(xcoord / pitchframewidth * 100);
            var ypercent = Math.round(ycoord / pitchframeheight * 100);
            var positiontext = $(this).children("input").val();

            var position = { index: $(this).index(), playerid: $(this).children("li").attr("id"), playername: $(this).children("li").children(".username").text(), positionname: positiontext, top: ypercent, left: xpercent }
            playerpositions.push(position);
        });
        var fixture = { id: id, homeId: homeid, awayId: awayid, start: start, end: end, comments: comments, location: location, positions: JSON.stringify(playerpositions) }

        $.ajax({
            url: "/Fixtures/Edit",
            dataType: "json",
            type: "POST",
            data: fixture,
            success: function (data) {

            }
        });
    });




    $(document).on('click', '#editEvent', function() {
        var id = $(".eventId").attr("id");
        var teamid = $(".teamId").attr("id");
        var users = $(".selected .id").map(function() { return $(this).text(); }).get();
        var type = $("#Event_Type").val();
        var start = $("#Event_Start").val();
        var end = $("#Event_End").val();
        var title = $("#Event_Title").val();
        var comments = $("#Event_Comments").val();
        var location = $("#Event_Location").val();


        //validation here
        var newEvent = { EventId: id, TeamId: teamid, Type: type, Title: title, Start: start, End: end, Location: location, UserIds: users, Comments: comments }

        $.ajax({
            url: "/Events/Edit",
            dataType: "json",
            type: "POST",
            data: newEvent,
            success: function(data) {

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
                    success: function(data) {
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

    $(document).on('change', '#selectedTeam', function() {
        $(".selected-users").empty();
        $.ajax({
            url: "/Partials/GetTeamMembers",
            dataType: "json",
            data: {
                id: $(this).val()
            },
            success: function(data) {
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

    $(".calendarButton").trigger('click');

    $(".calendarButton").click(function() {
        $("#mainBody").load("Partials/UserCalendar", function() {
            $("#mainBody").load("Partials/UserCalendar", function() {

                var eventList;
                $.ajax({
                    url: "/Partials/UserCalendarEvents",
                    dataType: "json",
                    type: "POST",
                    success: function(data) {
                        eventList = data;
                        $('#calendar').fullCalendar({
                            events: eventList,
                            dayClick: function(date) {
                                getDateEvents(date.format());

                            }
                        });
                    }
                });

            });

        });

  

    });

    $(".calendarButton a").trigger('click');
});