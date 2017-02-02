
mainApp.directive("navToggle", function () {
    
    return {
        restrict: "A",
        link: function (scope, elem, attr) {
            $(elem).bind("click", function (ev) {
                if ($(this).parent().hasClass("open")) {
                    $(this).parent().removeClass("open");
                    $(window).unbind("click", handler);
                }
                else {
                    $(this).parent().addClass("open");
                    $(window).bind("click", handler);
                }
            });

            function handler(event) {
                if (!event.target.matches('.dropdown-toggle')) {
                    var myGroup = $(".my-group");
                    if (myGroup) {
                        if (myGroup.hasClass("open")) {
                            myGroup.removeClass("open");
                            $(window).unbind("click", handler);
                        }
                    }
                }
            }
           
        }
    }

})