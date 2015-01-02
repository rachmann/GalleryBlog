

/* -------------------- animate icon --------------------- */

animationHover('#my-icon-1', 'fadeInUp');
animationHover('#my-icon-2', 'fadeInUp');
animationHover('#my-icon-3', 'fadeInUp');

function animationHover(element, animation) {
    element = $(element);
    element.hover(
        function () {
            element.addClass('animated ' + animation);
        },
        function () {
            //wait for animation to finish before removing classes
            window.setTimeout(function () {
                element.removeClass('animated ' + animation);
            }, 1000);
        });
}

function animationClick(element, animation) {
    element = $(element);
    element.click(
        function () {
            element.addClass('animated ' + animation);
            //wait for animation to finish before removing classes
            window.setTimeout(function () {
                element.removeClass('animated ' + animation);
            }, 1000);

        });
}


/* -------------------- Knob --------------------- */

$(".dial").knob({
    'release': function (v) { /*make something*/ }
});

/* -------------------- Isotope --------------------- */

$(function () {

    var $container = $('#container');

    $container.isotope({
        itemSelector: '.element',
        getSortData: {
            symbol: function ($elem) {
                return $elem.attr('data-symbol');
            },
            category: function ($elem) {
                return $elem.attr('data-category');
            },
            number: function ($elem) {
                return parseInt($elem.find('.number').text(), 10);
            },
            weight: function ($elem) {
                return parseFloat($elem.find('.weight').text().replace(/[\(\)]/g, ''));
            },
            name: function ($elem) {
                return $elem.find('.name').text();
            }
        }
    });


    var $optionSets = $('#options .option-set'),
        $optionLinks = $optionSets.find('a');

    $optionLinks.click(function () {
        var $this = $(this);
        // don't proceed if already selected
        if ($this.hasClass('selected')) {
            return false;
        }
        var $optionSet = $this.parents('.option-set');
        $optionSet.find('.selected').removeClass('selected');
        $this.addClass('selected');

        // make option object dynamically, i.e. { filter: '.my-filter-class' }
        var options = {},
            key = $optionSet.attr('data-option-key'),
            value = $this.attr('data-option-value');
        // parse 'false' as false boolean
        value = value === 'false' ? false : value;
        options[key] = value;
        if (key === 'layoutMode' && typeof changeLayoutMode === 'function') {
            // changes in layout modes need extra logic
            changeLayoutMode($this, options)
        } else {
            // otherwise, apply new options
            $container.isotope(options);
        }

        return false;
    });


});


/* -------------------- Slideshow --------------------- */

$("#slider4").responsiveSlides({
    auto: false,
    pager: false,
    nav: true,
    speed: 500,
    namespace: "callbacks",
    before: function () {
        $('.events').append("<li>before event fired.</li>");
    },
    after: function () {
        $('.events').append("<li>after event fired.</li>");
    }
});





/* -------------------- maps --------------------- */


$("#maps").gmap3({
    marker: {
        address: "Tour Eiffel, Avenue Anatole France, Paris"
    },
    map: {
        options: {
            zoom: 14
        }
    }
});




/* -------------------- canvas off --------------------- */


(function (window, document, undefined) {

    // helper functions

    var trim = function (str) {
        return str.trim ? str.trim() : str.replace(/^\s+|\s+$/g, '');
    };

    var hasClass = function (el, cn) {
        return (' ' + el.className + ' ').indexOf(' ' + cn + ' ') !== -1;
    };

    var addClass = function (el, cn) {
        if (!hasClass(el, cn)) {
            el.className = (el.className === '') ? cn : el.className + ' ' + cn;
        }
    };

    var removeClass = function (el, cn) {
        el.className = trim((' ' + el.className + ' ').replace(' ' + cn + ' ', ' '));
    };

    var hasParent = function (el, id) {
        if (el) {
            do {
                if (el.id === id) {
                    return true;
                }
                if (el.nodeType === 9) {
                    break;
                }
            }
            while ((el = el.parentNode));
        }
        return false;
    };

    // normalize vendor prefixes

    var doc = document.documentElement;

    var transform_prop = window.Modernizr.prefixed('transform'),
        transition_prop = window.Modernizr.prefixed('transition'),
        transition_end = (function () {
            var props = {
                'WebkitTransition': 'webkitTransitionEnd',
                'MozTransition': 'transitionend',
                'OTransition': 'oTransitionEnd otransitionend',
                'msTransition': 'MSTransitionEnd',
                'transition': 'transitionend'
            };
            return props.hasOwnProperty(transition_prop) ? props[transition_prop] : false;
        })();

    window.App = (function () {

        var _init = false, app = {};

        var inner = document.getElementById('inner-wrap'),

            nav_open = false,

            nav_class = 'js-nav';


        app.init = function () {
            if (_init) {
                return;
            }
            _init = true;

            var closeNavEnd = function (e) {
                if (e && e.target === inner) {
                    document.removeEventListener(transition_end, closeNavEnd, false);
                }
                nav_open = false;
            };

            app.closeNav = function () {
                if (nav_open) {
                    // close navigation after transition or immediately
                    var duration = (transition_end && transition_prop) ? parseFloat(window.getComputedStyle(inner, '')[transition_prop + 'Duration']) : 0;
                    if (duration > 0) {
                        document.addEventListener(transition_end, closeNavEnd, false);
                    } else {
                        closeNavEnd(null);
                    }
                }
                removeClass(doc, nav_class);
            };

            app.openNav = function () {
                if (nav_open) {
                    return;
                }
                addClass(doc, nav_class);
                nav_open = true;
            };

            app.toggleNav = function (e) {
                if (nav_open && hasClass(doc, nav_class)) {
                    app.closeNav();
                } else {
                    app.openNav();
                }
                if (e) {
                    e.preventDefault();
                }
            };

            // open nav with main "nav" button
            document.getElementById('nav-open-btn').addEventListener('click', app.toggleNav, false);

            // close nav with main "close" button
            document.getElementById('nav-close-btn').addEventListener('click', app.toggleNav, false);

            // close nav by touching the partial off-screen content
            document.addEventListener('click', function (e) {
                if (nav_open && !hasParent(e.target, 'nav')) {
                    e.preventDefault();
                    app.closeNav();
                }
            },
            true);

            addClass(doc, 'js-ready');

        };

        return app;

    })();

    if (window.addEventListener) {
        window.addEventListener('DOMContentLoaded', window.App.init, false);
    }

})(window, window.document);