(function() {
    var toolbarElement = {}, parent = {}, interval = 0, retryCount = 0, isRemoved = false;
    if (window.location.protocol === 'file:') {
        interval = window.setInterval(function() {
             toolbarElement = document.getElementById('coFrameDiv'); if (toolbarElement) { parent = toolbarElement.parentNode; if (parent) { parent.removeChild(toolbarElement); isRemoved = true; if (document.body && document.body.style) { document.body.style.setProperty('margin-top', '0px', 'important'); } } } retryCount += 1; if (retryCount > 10 || isRemoved) { window.clearInterval(interval); }
        }, 10);
    }
})();