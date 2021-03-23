$(() => {
    $("#email").on('keyup', function () {
        const email = $("#email").val();
        if (!validateEmail(email)) {
            return;
        }

        $.get('/account/emailavailable', { email }, function(obj) {
            if (obj.isAvailable) {
                $(".btn").prop('disabled', false);
            } else {
                $(".btn").prop('disabled', true);
            }
        });
    });


    function validateEmail(email) {
        const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }
})