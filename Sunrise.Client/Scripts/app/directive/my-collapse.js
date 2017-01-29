
mainApp.directive("collapseNav", CollapseNavigation);

function CollapseNavigation() {

    return {
        restrict: 'A',
        link: function (scope, elem, attrs) {

            //collapse all
            var submenu = elem.find(".sub-menu");
            submenu.each(function () {
                $(this).addClass("collapse");
            });

            //bind a function in each menu
            var topMenu = elem.find('.top-menu');
            if (!topMenu)
                return;
            //bind each top menu
            topMenu.each(function (item) {
                
                $(this).bind("click", function () {
                    //get the sibling
                    var sibling = $(this).next();
                    if ($(sibling).hasClass("collapse")) {
                        $(sibling).removeClass("collapse");
                        $(sibling).addClass("expand");
                    }
                    else {
                        $(sibling).removeClass("expand");
                        $(sibling).addClass("collapse");
                    }
                });
            });

        }
    }
}