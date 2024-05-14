$(document).ready(function () {
    const id = getCookie("id");
    if (id != undefined) {
        const hub = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7250/chating", {
                headers: { 'authsmile': "123" }
            })
            .build();

        hub.start()
            .then(function () {
                hub.invoke("GetLastMessages", $('.chat-name').val());
                hub.invoke("Enter", Number(getCookie("id")), $('.chat-name').val());
            });

        hub.on('NewMessage', function (message) {
            addMessage(message.id, message.time, message.text, message.name);
        });
        hub.on("OldMessages", function (messages) {
            messages.forEach(function (message) {
                addMessage(message.id, message.time, message.text, message.name);
            })
        })

        hub.on('AddToGroup', function (id,time, name) {
            addMessage(id, time, "присоединился", name);
        });
        hub.on('Error', function (text) {
            addEvent(true, text);
        })
        hub.on('Apply', function (text, id) {
            addEvent(id, text);
        })
        function addEvent(id, text) {
            const newEventBlock = $('.event.template').clone();
            newEventBlock.find('.event-text').text(text);
            newEventBlock.removeClass('template');
            if (typeof id == "boolean") {
                newEventBlock.addClass('error');
                $('.messages').animate({
                    scrollTop: newEventBlock.offset().top + newEventBlock.height()
                }, 2000);
            }
            else {
                newEventBlock.addClass('apply');
                $('.replace' + id).prev().after(newEventBlock[0].outerHTML);
                $('.replace' + id).remove();
            }
        }

        $('.send-message-button').click(function () {
            SendMessage();
        });
        $('.new-message-text').keyup(function (e) {
            if (e.which == 13) {
                SendMessage();
            }
        })

        function SendMessage() {
            const message = $('.new-message-text').val();
            const chatName = $('.chat-name').val();
            const id = getCookie("id");
            $('.new-message-text').val('');
            hub.invoke('SendMessage', chatName, Number(id), message)
        }
        function addMessage(id, time, message, user) {
            const newMessageBlock = $('.message.template').clone();
            newMessageBlock.removeClass('template');
            if (newMessageBlock.find('.delete-message-button').length > 0) {
                newMessageBlock.find('.delete-message-button').val(id);
            }
            newMessageBlock.find('.message-time').text(time);
            newMessageBlock.find('.message-user-name').text(user);
            newMessageBlock.find('.message-text').text(message);
            $('.messages').append(newMessageBlock);
            $('.messages').animate({
                scrollTop: newMessageBlock.offset().top + newMessageBlock.height()
            }, 2000);

            newMessageBlock.find('.delete-message-button').on('click', function () {
                const messageId = Number($(this).val());
                hub.invoke("DeleteMessage", Number(getCookie("id")), messageId);
                $(this).parent().addClass('replace' + messageId);
            });
        }
    }
});