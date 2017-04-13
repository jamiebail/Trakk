$(document).ready(function() {

        function dragStart(ev) {
            $("#PitchOverlay").fadeIn();
            ev.dataTransfer.effectAllowed = 'move';
            ev.dataTransfer.setData("Text", ev.target.getAttribute('id'));

            return true;
        }

        function dragEnter(ev) {
            event.preventDefault();
            return true;
        }

        function dragOver(ev) {
            return false;
        }

        function dragDrop(ev) {
            $("#PitchOverlay").fadeOut();
            ev.preventDefault();
            var src = document.getElementById(ev.dataTransfer.getData("Text"));
            var srcParent = src.parentNode;
            var tgt = srcParent.children(ev.currentTarget.id);
            ev.currentTarget.replaceChild(tgt, src);
            ev.currentTarget.replaceChild(src, tgt);

            return false;
        }


        function SaveLocations() {
            var keeperUserID = $("#keeper li").attr('id');
            var defenceUserIDs = $(".defence");
        }


            var droppableParent;

            $('.user-widget').draggable({
                revert: 'invalid',
                revertDuration: 200,
                start: function() {
                    droppableParent = $(this).parent();

                    $(this).addClass('being-dragged');
                },
                stop: function() {
                    $(this).removeClass('being-dragged');
                }
            });

            $('.pitchLocation').droppable({
                hoverClass: 'drop-hover',
                drop: function(event, ui) {
                    var draggable = $(ui.draggable[0]),
                        draggableOffset = draggable.offset(),
                        container = $(event.target),
                        containerOffset = container.offset();

                    $('.widget', event.target).appendTo(droppableParent).css({ opacity: 0 }).animate({ opacity: 1 }, 200);

                    draggable.appendTo(container).css({ left: draggableOffset.left - containerOffset.left, top: draggableOffset.top - containerOffset.top }).animate({ left: 0, top: 0 }, 200);
                }
            });
        }
    
);