jQueryModalPost = form => {
    try {
        if (!$(form).valid()) {
            return false; 
        }
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
            error: function (xhr) {
                console.log("disini");
                console.log(xhr)
                $('#form-modal').modal('hide');
                document.body.innerHTML = xhr.responseText;
            }
        })
        return false;
    } catch (ex) {
        console.log("disana");
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
