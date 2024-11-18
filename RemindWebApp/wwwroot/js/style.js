
$(document).ready(function () {
    // Carousel Auto-Cycle

    // $(window).on('scroll',function() {
    //     if ($(this).scrollTop() >50){  
    //         $('.header-sticky').addClass("is-sticky");
    //       }
    //       else{
    //         $('.header-sticky').removeClass("is-sticky");
    //       }
    //     });  
    // });Scrol Top




    var removefromasside;

    function createCookie(name, value, days) {
        var expires;

        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        } else {
            expires = "";
        }
        document.cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value) + expires + "; path=/";
    }

    function readCookie(name) {
        var nameEQ = encodeURIComponent(name) + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) === ' ')
                c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) === 0)
                return decodeURIComponent(c.substring(nameEQ.length, c.length));
        }
        return null;
    }

    function eraseCookie(name) {
        createCookie(name, "", -1);
    }

    function setCookie(key, value, expiry) {
        var expires = new Date();
        expires.setTime(expires.getTime() + (expiry * 24 * 60 * 60 * 1000));
        document.cookie = key + '=' + value + ';path=/' + ';expires=' + expires.toUTCString();
    }


    let products = [];
    //createCookie('InCard', products, 1000);

    $(document).on("click", ".addcard", function () {

        var productid = $(this).attr("data-id");
        var chechk = false;
        for (var i in products) {
            if (products[i] == productid) {
                chechk = true;
                break;
            }
        }

        if (chechk == false) {
                
            products.push(productid);

            
            createCookie('InCard', products.join('-'), 10)
            //setCookie('InCard', products.join('-'), 10);
            $.ajax({
                url: "/Card/AssideCard?id=" + productid,
                type: "GET",
                success: function (res) {
               
                    $(".asside_product").html(res)
                }
            });

            Swal.fire({
                position: 'top-center',
                type: 'success',
                title: 'Adding Card',
                showConfirmButton: false,
                timer: 1500
            });
            //$(this).css({
            //    "background-color": "#ff7a5f",
            //    "color": "#fff"
            //})

        } else
        {
            Swal.fire({
                position: 'top-end',
                type: 'warning',
                title: 'Item allready exists in cart',
                showConfirmButton: false,
                timer: 1500
            });
            //$(this).css({
            //    "background-color": "#fff",
            //    "color": "#ff7a5f"
            //})
        }



    
    });

    var cookie = readCookie('InCard');
    if (cookie != null) {
        products = readCookie('InCard').split("-");
        console.log(products);
    }

    $(".removefromcard").on("click", function () {   
        var productid = $(this).attr("data-id");

        for (var i in products) {
            if (products[i] == productid) {
                products.splice(i, 1);
                break;
            }
        }
        setCookie('InCard', products.join('-'), 10);

        this.parentNode.parentNode.remove();

        $.ajax({
            url: "/Card/AssideCard?id=" + productid,
            type: "GET",
            success: function (res) {
          
                $(".asside_product").empty();
                $(".asside_product").append(res);
            }
        });
    });
    
    //$(".assidetotal").on("click", function () {
    //    $.ajax({
    //        url: "/Card/AssideTotal?id=" + productid,
    //        type: "GET",
    //        success: function (res) {

    //            $(".total").empty();
    //            $(".total").html(res);
    //        }
    //    });
    //});


    var removecookie = readCookie('InCard');
    if (removecookie != null) {
        products = readCookie('InCard').split("-");
        console.log(products);
    }

    $(document).on("click",".assideremovecookie", function () {
        var productid = $(this).attr("data-id");
        for (var i in products) {
            if (products[i] == productid) {
                products.splice(i, 1);
                break;
            }
        }
        setCookie('InCard', products.join('-'), 10);

        this.parentNode.parentNode.remove();

        $.ajax({
            url: "/Card/DyncAsside?id=" + productid,
            type: "GET",
            success: function (res) {
             
                 $("#cardtable").empty();
                 $("#cardtable").html(res);
              
            }
        });
    });

    var assidecookie = readCookie('InCard');
    if (assidecookie != null) {
        products = readCookie('InCard').split("-");
        console.log(products);
    }
    
    $(".card-header").click(function () {
        if ($(this).next(".card-body").hasClass("active")) {
            $(this).next(".card-body").removeClass("active").slideUp();
            $(this).children("span").removeClass("fa-minus").addClass("fa-plus");
        }
        else {
            $(".card .card-body").removeClass("active").slideUp();
            $(".card .card-header span").removeClass("fa-minus").addClass("fa-plus");
            $(this).next(".card-body").addClass("active").slideDown();
            $(this).children("span").removeClass("fa-plus").addClass("fa-minus");
        }

    })

    
    $(document).on("click", ".temp", function () {
        var productid = $(this).attr("data-id");
        if (!($(this).hasClass("unlike"))) {
        
            $.ajax({
                url: "Home/AddLike?id=" + productid,
                type: "Get",
                success: function (response) {

                }
            });
            $(this).addClass("unlike");
            $(this).css({
                "background-color": "#ff7a5f",
                "color": "#fff"
            })


        }
        else {
            $.ajax({
                url: "Home/Removelike?id=" + productid,
                type: "Get",
                succsess: function (response) {

                }
            })
            $(this).removeClass("unlike");
            //$(this).css({
            //    "background-color": "#fff",
            //    "color": "#ff7a5f"
            //})

        }


    });

    $("#search").keyup(function () {
        var src = $(this).val();
        $("#search_list").empty();
        if (src.length > 0) {
            $.ajax({
                url: "Home/SearchProduct?str=" + src,
                type: "GET",
                success: function (res) {
                    $("#search_list").append(res)
                }
            });
            $("#search_list").css({
                "display": "block"
            });
            console.log("Selamin Eleykum");
        }
        else {
            $("#search_list").css({
                "display": "none"
            });
            console.log("Eleykum Salam");
        }
    });



    $(".show").change(function () {
        $(this).val();
        $.ajax({
            url: "/Product/Index?incdataid=" + dataId,
            type: "GET",
            success: function (res) {
                console.log(res);
            }
        });

    });
    var skipdetector = 4;

    $("#loadmore").click(function () {
        //console.log("Selamin Eleykum");
        var count = $("#countblognews").val();
        console.log(count)
        $.ajax({
            url: "BlogNews/LoadMore?skip=" + skipdetector,
            type: "Get",
            success: function (response) {
                $(".showmore").append(response);
                skipdetector += 4;
                console.log(skipdetector)
                if (skipdetector >= count) {
                    $("#loadmore").remove();
                }
            }
        });
    });

    var blogskip = 3;
    $("#loadblog").click(function () {
        var countblog = $("#blogcount").val();
        $.ajax({
            url: "Blog/LoadBlog?skip=" + blogskip,
            type: "Get",
            success: function (resp) {
                $(".hold").append(resp);
                blogskip += 3;
                if (blogskip >= countblog) {
                    $("#loadblog").remove();
                }
            }
        });
    });

    $(".pos").click(function () {

        var accaunt_show = $(".accaunt_ul");
        if (!accaunt_show.hasClass("active")) {
            accaunt_show.addClass("active");
            accaunt_show.css({ "visibility": "visible" });
            //console.log("visible");
        }
        else {
            accaunt_show.removeClass("active");
            accaunt_show.css({ "visibility": "hidden" });
            //console.log("hidden");
        }
    });


    $(".cover").click(function () {
        $(".modal_area").hide();
    });

    $(".main img").click(function () {
        //var img = $(this).attr("data-img")
        var img = $(this).data("img");
        var msg = $(this).data("msg");
        var index = $(this).data("index");

        $(".modal_core").children().eq(0).attr("src", `Img/lg/${img}`);
        $(".modal_core").children().eq(1).text(msg);

        if ($(".active")) { //Bu atributun yoxlanmasidir.
            $(".active").removeClass("active");
        }
        $(".navigation").children().eq(index).children().eq(1).addClass("active");
        $(".modal_area").show();
    })

    $(".navigation .item").click(function () {
        var img = $(this).data("img");
        var msg = $(this).data("msg");

        $(".modal_core").children().eq(0).attr("src", `Img/lg/${img}`);
        $(".modal_core").children().eq(1).text(msg);

        if ($(".active")) {
            $(".active").removeClass("active");
        }
        $(this).children().eq(1).addClass("active");
    });
    $("#right_i").click(function () {

        var nextdata = $(".active").parent().next();

        if (nextdata.length != 0) {
            var img = nextdata.data("img");
            var msg = nextdata.data("msg");


            $(".modal_core").children().eq(0).attr("src", `Img/lg/${img}`);
            $(".modal_core").children().eq(1).text(msg);

            var active = $(".active")
            if (active.hasClass("active")) {
                active.removeClass("active");
                console.log("Yes");
            }
            nextdata.children().eq(1).addClass("active");

        }
        else {
            var nextdata = $(".navigation").children().eq(0);
            console.log(nextdata.data("img"));

            var img = nextdata.data("img");
            var msg = nextdata.data("msg");
            $(".modal_core").children().eq(0).attr("src", `Img/lg/${img}`);
            $(".modal_core").children().eq(1).text(msg);

            var active = $(".active")
            if (active.hasClass("active")) {
                active.removeClass("active");
                console.log("Else Yes");
            }
            $(".navigation").children().eq(0).children().eq(1).addClass("active");
        }
    })

    $("#left_i").click(function () {
        var prevdata = $(".active").parent().prev();
        if (prevdata.length != 0) {

            console.log(prevdata.data("img"));
            var img = prevdata.data("img");
            var msg = prevdata.data("msg");


            $(".modal_core").children().eq(0).attr("src", `Img/lg/${img}`);
            $(".modal_core").children().eq(1).text(msg);
            var active = $(".active")
            if (active.hasClass("active")) {
                active.removeClass("active");
                console.log("Yes");
            }
            prevdata.children().eq(1).addClass("active");

        }
        else {
            prevdata = $(".navigation").children().eq(3);
            console.log(prevdata.data("img"));

            var img = prevdata.data("img");
            var msg = prevdata.data("msg");
            $(".modal_core").children().eq(0).attr("src", `Img/lg/${img}`);
            $(".modal_core").children().eq(1).text(msg);
            // if ($(".active")) {
            //     $(".active").removeClass("active");
            // }
            var active = $(".active")
            if (active.hasClass("active")) {
                active.removeClass("active");
                console.log("Yes");
            }
            prevdata.children().eq(1).addClass("active");
        }


    });

    $(".shop_basket").click(function () {
        $(".right_asside").show(1500);
    });

    $(".close").click(function () {
        $(".right_asside").hide(1500);
    });

    $(".cover_card").click(function () {
        $(".right_asside").hide(1500);
    })


});













