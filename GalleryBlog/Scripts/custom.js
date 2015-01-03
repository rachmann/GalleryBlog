

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


/* -------------------- Isotope --------------------- 

$(function () {

    var $container = $('#container');
    
        $container.isotope({
            itemSelector: '.element',
            getSortData: {
                symbol: function($elem) {
                    return $elem.attr('data-symbol');
                },
                category: function($elem) {
                    return $elem.attr('data-category');
                },
                number: function($elem) {
                    return parseInt($elem.find('.number').text(), 10);
                },
                weight: function($elem) {
                    return parseFloat($elem.find('.weight').text().replace(/[\(\)]/g, ''));
                },
                name: function($elem) {
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

*/