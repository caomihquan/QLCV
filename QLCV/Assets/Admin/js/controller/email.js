var product = {
    init: function () {
        product.registerEvents();
    },
    registerEvents: function () {
        $('.btn-images').off('click').on('click', function (e) {
            e.preventDefault();
            $('#imagesManage').modal('show');   
            $('#hidProductID').val($(this).data('id'));            
        });

    

        $('#btnChooImages').off('click').on('click', function (e) {
            e.preventDefault();
            var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
            var text = $('#doc').val()
            if (text=='') {
                alert('Khong dc de trong');
            }
            else if (!testEmail.test(text)) {
                
            }
            else {
                $('#imageList').append('<div style="float:left">' + text + '<a href="#" class="btn-delImage"><i class="fa fa-times"></i></a></div>');

                $('.btn-delImage').off('click').on('click', function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                });
                
            }
            $('#doc').val('');
            
        });

        $('#doc').keypress(function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                var text = $('#doc').val()
                var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
                if (text == '') {
                    
                }
                else if (!testEmail.test(text)) {

                }
                else {
                    $('#imageList').append('<div style="float:left">' + text + '<a href="#" class="btn-delImage"><i class="fa fa-times"></i></a></div>');

                    $('.btn-delImage').off('click').on('click', function (e) {
                        e.preventDefault();
                        $(this).parent().remove();
                    });

                }
                $('#doc').val('');
            }
        });

        $(document).on("keypress", "form", function (event) {
            return event.keyCode != 13;
        });

        $('#btnSaveImages').off('click').on('click', function () {
            var images = [];
            var id = $('#hidProductID').val();
            $.each($('#imageList div'), function (i, item) {
                images.push($(item).text());
            })
            $.ajax({
                url: '/CongVan/SaveImages',
                type: 'POST',
                data: {
                    id: id,
                    images: JSON.stringify(images)
                },
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        $('#imagesManage').modal('hide');
                        $('#imageList').html('');
                        alert('Save thành công');
                    }

                    //thong bao thanh cong
                }
            });
        });
    },

}
product.init();