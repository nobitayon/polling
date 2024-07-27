jQueryModalPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#form-modal').modal('hide');

                    if (res.redirectUrl) {
                        window.location.href = res.redirectUrl;
                    }
                }
                else {
                    if (res.redirectUrl) {
                        window.location.href = res.redirectUrl;
                    }
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//jQueryModalPost = form => {
//    try {
//        if ($(form).valid()) {
//            $.ajax({
//                type: 'POST',
//                url: form.action,
//                data: new FormData(form),
//                contentType: false,
//                processData: false,
//                success: function (res) {
//                    if (res.isValid) {
//                        $('#form-modal').modal('hide');

//                        if (res.redirectUrl) {
//                            window.location.href = res.redirectUrl;
//                        }
//                    } else {
//                        if (res.redirectUrl) {
//                            window.location.href = res.redirectUrl;
//                        }
//                    }
//                },
//                error: function (err) {
//                    console.log(err);
//                }
//            });
//        }
//        return false;
//    } catch (ex) {
//        console.log(ex);
//    }
//}
