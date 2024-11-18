

$(document).ready(function(){
    $(".deleteme").click(function () {
        var photo = $(this);
        var id= photo.attr("data-id");

        alert(id);
        $.ajax({
            url: "/RemindWebApp/ProductGenerate/DelImage?id=" + id,
            type: "Get",
            success: function (response) {
                photo.parent().remove();
               // console.log(response);
            }

        });


    });
})