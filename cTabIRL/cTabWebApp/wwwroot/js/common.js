
function pad(n, width) {
    n = n + '';
    return n.length >= width ? n : new Array(width - n.length + 1).join('0') + n;
}

var useMils = false;
function toHeadingUnit(degrees) {
    var text;
    if (useMils) {
        text = '' + Math.trunc(degrees * 6400 / 360);
    }
    else {
        text = '' + degrees.toFixed(2) + '°';
    }
    text += ' ' + octants[Math.round(degrees / 45)];
    return text;
}

function setupFullViewHeight(selector) {
    $(selector).css('height', (window.innerHeight - 30) + 'px');
    $(window).on('resize', function () { $(selector).css('height', (window.innerHeight - 30) + 'px'); window.scrollTo(0, 0); });
}

function setupCopyButtons() {
    $('.btn-copy').on('click', function () {
        var target = $('#' + $(this).attr('data-copy')).get(0);
        target.select();
        document.execCommand("copy");
    });
}


