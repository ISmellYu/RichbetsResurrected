$(window).on("load", function() {

    setShowcase();
});

window.addEventListener("resize", function() {

    setShowcase();
});

function setShowcase() {

    if ($(".sc-text-title").parent().innerWidth() / $(".sc-text-title").width() < 3) {

        $(".sc-text-title").each(function() {
            $(this).css("transform", "scale(" + ($(this).parent().innerWidth() / $(this).width()) + ")");
        });

        $(".sc-desc").each(function() {
            $(this).css("margin-top", $(".sc-text-title").height() + "px");
        });

        $(".sc-presentation").css("height", $(".sc-text").height() + "px");

    }
    else{
        $(".sc-text-title").each(function() {
            $(this).css("transform", "scale(3)");
        });

        $(".sc-desc").each(function() {
            $(this).css("margin-top", $(".sc-text-title").height() + "px");
        });

        $(".sc-presentation").css("height", $(".sc-text").height() + "px");
    }
}


let status = 0;

let timer = setInterval(function() {

    if (status == 0) { 

        $(".showcase-container").animate({ opacity: 0 }, 1000);

        setTimeout(function() {

            showStyling();
            setShowcase();
            
            status = 1;

            $(".showcase-container").animate({ opacity: 1 }, 900);
        }, 1200);

    }else{

        $(".showcase-container").animate({ opacity: 0 }, 1000);

        setTimeout(function() {

            showLure();
            setShowcase();

            status = 0;

            $(".showcase-container").animate({ opacity: 1 }, 900);
        }, 1200);
    }

}, 10000);


function showLure(){
    $(".sc-text-title").text("Lure module");
    $(".sc-text-desc").text("A Lure Module is an item that members can use on any voice channel to increase the multiplier for users on it. You can hold it and use it anytime you need.");

    let [
        img1,
        img2,
        img3
    ] = Array(3).fill(null).map(() => $("<img>").addClass("sc-img"));

    img1.attr("src","/img/Itemshop/advanced-lure.png");
    img2.attr("src","/img/Itemshop/basic-lure.png");
    img3.attr("src","/img/Itemshop/regular-lure.png");

    img1.attr("alt","Advanced rule");
    img2.attr("alt","Basic rule");
    img3.attr("alt","Regular rule");

    $(".sc-presentation-left").each(function() {
        $(this).empty();
        $(this).append(img1);
        $(this).append(img2);
    });

    $(".sc-presentation-right").each(function() {
        $(this).empty();
        $(this).append(img3);
    });
}

function showStyling() {

    $(".sc-text-title").text("Custom styling");
    $(".sc-text-desc").text("Stand out with new custom username styling. Your new style will be seen on every page.");

    let [
        div1,
        div2,
        div3
    ] = Array(3).fill(null).map(() => $("<div>").text(clientInfo.userName).addClass("present-text"));

    div1.addClass("giraffe").addClass("pre-anim");
    div2.addClass("anim-fire").addClass("pre-anim");
    div3.addClass("strawberries");

    $(".sc-presentation-left").each(function() {
        $(this).empty();
        $(this).append(div1);
        $(this).append(div2);
       
    });

    $(".sc-presentation-right").each(function() {
        $(this).empty();
        $(this).append(div3);
    });
}