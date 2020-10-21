L.Control.OverlayButton = L.Control.extend({
    options: {
        position: 'bottomright',
        initialClass: 'btn-outline-secondary',
        content: 'A',
        click: null
    },

    previousClass: '',

    onAdd: function (map) {
        this.previousClass = this.options.initialClass;
        this._container = L.DomUtil.create('button', 'btn ' + this.options.initialClass);
        L.DomEvent.disableClickPropagation(this._container);
        this._container.innerHTML = this.options.content;
        if (this.options.click) {
            $(this._container).on('click', this.options.click);
        }
        return this._container;
    },

    onRemove: function (map) {

    },

    j: function () {
        return $(this._container);
    },
    setClass: function (name) {
        $(this._container).removeClass(this.previousClass);
        $(this._container).addClass(name);
        this.previousClass = name;
    }
});

L.control.overlayButton = function (options) {
    return new L.Control.OverlayButton(options);
};

L.Control.OverlayNotify = L.Control.extend({
    options: {
        position: 'bottomleft',
        text: ''
    },
    onAdd: function (map) {
        this._container = L.DomUtil.create('div', 'notify-message');
        $(this._container).text(this.options.text);
        L.DomEvent.disableClickPropagation(this._container);
        return this._container;
    },
    onRemove: function (map) {
    },
    text: function (value) {
        $(this._container).text(value);
    }
});

L.control.overlayNotify = function (options) {
    return new L.Control.OverlayNotify(options);
};

var currentMap = null;
var currentMapInfos = null;
var selfMarker = null;
var existingMarkers = {};
var centerOnPosition = true;
var centerOnPositionButton = null;
var fullScreenButton = null;
var tempUserPopup = null;
var userMarkerData = {};
var connection = null;
var useMils = true;
var inboxButton = null;
var composeButton = null;
var knownTo = {};
var existingMessages = {};
var displayedMessage = null;
var noSleepButton = null;
var isNoSleep = false;
var noSleep = null;

function pad(n, width) {
    n = n + '';
    return n.length >= width ? n : new Array(width - n.length + 1).join('0') + n;
}

function updateButtons() {
    centerOnPositionButton.setClass(centerOnPosition ? 'btn-primary' : 'btn-outline-secondary');

    if (fullScreenButton) {
        fullScreenButton.setClass(document.fullscreenElement ? 'btn-primary' : 'btn-outline-secondary');

        fullScreenButton.j().find('i').removeClass('fa-expand');
        fullScreenButton.j().find('i').removeClass('fa-compress');
        fullScreenButton.j().find('i').addClass(document.fullscreenElement ? 'fa-compress' : 'fa-expand');
    }
    if (noSleepButton) {
        noSleepButton.setClass(isNoSleep ? 'btn-primary' : 'btn-outline-secondary');
    }
}

function fullScreenToggle() {
    if (document.fullscreenElement) {
        document.exitFullscreen().then(updateButtons);
    } else {
        document.documentElement.requestFullscreen().then(updateButtons);
    } 
}

function noSleepToggle() {
    isNoSleep = !isNoSleep;
    if (!noSleep) {
        noSleep = new NoSleep();
    }
    if (isNoSleep) {
        noSleep.enable();
    }
    else {
        noSleep.disable();
    }
    updateButtons();
}

function setCenterOnPosition(value) {
    centerOnPosition = value;
    updateButtons();
}

function toHeadingUnit(degrees) {
    var text;
    if (useMils) {
        text = '' + Math.trunc(degrees * 6400 / 360);
    }
    else {
        text = '' + degrees + '°';
    }
    text += ' ' + octants[Math.round(degrees / 45)];
    return text;
}

function bearing(latlng1, latlng2) {
    return ((Math.atan2(latlng2.lng - latlng1.lng, latlng2.lat - latlng1.lat) * 180 / Math.PI) + 360) % 360;
}

function generateLocationInfos(latlng) {
    var infos = pad(Math.trunc(latlng.lng), 5) + ' - ' + pad(Math.trunc(latlng.lat), 5);
    if (selfMarker) {
        var pos1 = latlng;
        var pos2 = selfMarker.getLatLng();
        infos += '<br />' + Math.trunc(currentMap.distance(pos1, pos2)) + 'm ' + toHeadingUnit(Math.trunc(bearing(pos2, pos1)));
    }
    return infos;
}

function generateMenu(id, latlng) {
    if (id == 0) {
        userMarkerData = {};
    }
    var div = $('<div style="min-width:150px;"></div>');
    if (id == 0) {
        var a = $('<div class="text-center"></div>');
        a.html(generateLocationInfos(latlng));
        a.appendTo(div);
    }
    menus['' + id].forEach(function (entry) {
        var a = $('<a class="dropdown-item" href="#"></a>');
        a.text(entry.label);
        a.attr('title', entry.tooltip);
        a.on('click', function () {
            if (entry.select1 !== null) userMarkerData.d1 = entry.select1;
            if (entry.select2 !== null) userMarkerData.d2 = entry.select2;
            if (entry.select3 !== null) userMarkerData.d3 = entry.select3;
            if (entry.nextMenu) {
                tempUserPopup.setContent(generateMenu(entry.nextMenu, latlng));
            } else {
                tempUserPopup.remove();
                connection.invoke('WebAddUserMarker',
                    {
                        x: Math.trunc(latlng.lng),
                        y: Math.trunc(latlng.lat),
                        data: [userMarkerData.d1 || 0, userMarkerData.d2 || 0, userMarkerData.d3 || 0]
                    });
            }
            return false;
        });
        a.appendTo(div);
    });
    return div.get(0);
}

function showMenu(latLng, content) {
    if (!tempUserPopup) {
        tempUserPopup = L.popup({ className: 'menupopup' });
    }
    tempUserPopup.setLatLng(latLng);
    tempUserPopup.setContent(content);
    tempUserPopup.openOn(currentMap);
}

function showMarkerMenu(marker) {
    var div = $('<div style="min-width:150px;"></div>');
    var a = $('<div class="text-center"></div>');
    a.html(generateLocationInfos(marker.getLatLng()));
    a.appendTo(div);

    if (marker.options.marker.kind == 'u') {
        var a = $('<a class="dropdown-item" href="#"></a>');
        a.text(texts.deleteMarker);
        a.on('click', function () {
            connection.invoke("WebDeleteUserMarker", { id: marker.options.marker.id });
            tempUserPopup.remove();
            return false;
        });
        a.appendTo(div);
    }
    else {
        $('<div class="text-center"></div>').text(marker.options.marker.name).appendTo(div);
    }
    showMenu(marker.getLatLng(), div.get(0));
}

function updateUnread() {
    var unread = $('#inbox-list').find('i.fa-envelope').length;
    var span = inboxButton.j().find('span');
    span.text('' + unread);
    span.attr('class', unread > 0 ? 'badge badge-danger' : 'badge badge-secondary');
    inboxButton.setClass(unread > 0 ? 'btn-primary' : 'btn-outline-secondary');
}

function removeAllMarkers() {
    Object.getOwnPropertyNames(existingMarkers).forEach(function (id) {
        existingMarkers[id].remove();
    });
    existingMarkers = {};
    if (selfMarker) {
        selfMarker.remove();
        selfMarker = null;
    }
    if (tempUserPopup) {
        tempUserPopup.remove();
        tempUserPopup = null;
    }
    $('#compose-to').empty();
    knownTo = {};
}

function clearMessage() {
    $('#inbox-title').text('');
    $('#inbox-message').text(texts.noMessageSelected);
    $('#inbox-delete').hide();
    displayedMessage = null;
}

function clearInbox() {
    clearMessage();
    $('#inbox-list').empty();
    $('#outbox-list').empty();
    existingMessages = {};
    updateUnread();
}

function initMap(mapInfos) {
    if (mapInfos == currentMapInfos) {
        removeAllMarkers();
        clearInbox();
        return;
    }
    if (currentMap != null) {
        removeAllMarkers();
        clearInbox();
        currentMap.remove();
    }
    var map = L.map('map', {
        minZoom: mapInfos.minZoom,
        maxZoom: mapInfos.maxZoom + 2,
        maxNativeZoom: mapInfos.maxZoom,
        crs: mapInfos.CRS,
        doubleClickZoom: false
    });
    L.tileLayer('https://jetelain.github.io/Arma3Map' + mapInfos.tilePattern, {
        attribution: mapInfos.attribution,
        tileSize: mapInfos.tileSize,
        maxNativeZoom: mapInfos.maxZoom
    }).addTo(map);
    map.setView(mapInfos.center, mapInfos.maxZoom);
    map.on('mousedown', function () { setCenterOnPosition(false); });
    map.on('touchstart', function () { setCenterOnPosition(false); });
    map.on('dblclick contextmenu', function (e) { showMenu(e.latlng, generateMenu(0, e.latlng)); });

    (centerOnPositionButton = L.control.overlayButton({
        content: '<i class="fas fa-location-arrow"></i>',
        click: function () { setCenterOnPosition(!centerOnPosition); }
    })).addTo(map);

    if (document.documentElement.requestFullscreen) {
        (fullScreenButton = L.control.overlayButton({
            content: '<i class="fas fa-expand"></i>',
            click: fullScreenToggle
        })).addTo(map);
    } else {
        (noSleepButton = L.control.overlayButton({
            content: '<i class="fas fa-sun"></i>',
            click: noSleepToggle
        })).addTo(map);
    }

    (inboxButton = L.control.overlayButton({
        position: 'topright',
        content: '<i class="fas fa-inbox"></i>&nbsp;<span class="badge badge-secondary">0</span>',
        click: function () {
            $('#inbox').modal('show');
        }
    })).addTo(map);

    (composeButton = L.control.overlayButton({
        position: 'topright',
        content: '<i class="far fa-envelope"></i>',
        click: function () {
            $('#compose').modal('show');
        }
    })).addTo(map);

    L.latlngGraticule({
        zoomInterval: [
            { start: 0, end: 10, interval: 1000 }
        ]}).addTo(map);
    L.control.scale({ maxWidth: 200, imperial: false }).addTo(map);

    L.control.overlayButton({
        content: '<i class="fas fa-bars"></i>',
        click: function () { $('#help').modal('show'); },
        position: 'bottomleft'
    }).addTo(map);

    currentMap = map;
    currentMapInfos = mapInfos;
    selfMarker = null;
    updateButtons();
};


function updateClock(date) {
    var dateObj = new Date(date);
    $('#date').text(pad(dateObj.getUTCHours(), 2) + ':' + pad(dateObj.getUTCMinutes(), 2));
}


function updatePosition(x, y, heading, grp, veh) {
    $('#position').text(pad(Math.trunc(x), 5) + ' - ' + pad(Math.trunc(y), 5));
    $('#heading').text(toHeadingUnit(heading));

    var marker = existingMarkers[veh || grp];
    if (marker) {
        if (selfMarker && !selfMarker.options.marker) {
            selfMarker.remove();
        }
        selfMarker = marker;
        var latLng = marker.getLatLng();
        if (latLng.lat != y || latLng.lng != x) {
            marker.setLatLng([y, x]);
        }
    }
    else {
        if (selfMarker && !selfMarker.options.marker) {
            var latLng = selfMarker.getLatLng();
            if (latLng.lat != y || latLng.lng != x) {
                selfMarker.setLatLng([y, x]);
            }
        } else {
            selfMarker = L.marker([y, x], { icon: createIcon({ symbol: '10031000001211000000' }) }).addTo(currentMap);
        }
    }
    if (centerOnPosition) {
        currentMap.setView([y, x]);
    }
}

function createIcon(marker) {

    if (/^img:/.test(marker.symbol)) {
        var iconHtml = $('<div></div>').append(
            $('<div></div>')
                .addClass('text-marker-content')
                .text(marker.name)
                .prepend($('<img src="/img/' + marker.symbol.substr(4) + '" width="32" height="32" />&nbsp;')))
            .html();

        return new L.DivIcon({
            className: 'text-marker',
            html: iconHtml,
            iconAnchor: [16, 16]
        });
    }

    var sym = new ms.Symbol(marker.symbol, { size: 24, additionalInformation: marker.name });
    return L.icon({
        iconUrl: sym.asCanvas(window.devicePixelRatio).toDataURL(),
        iconSize: [sym.getSize().width, sym.getSize().height],
        iconAnchor: [sym.getAnchor().x, sym.getAnchor().y]
    });
}

function updateMarkers(makers) {

    var markersToKeep = [];
    var toToKeep = [];

    makers.forEach(function (marker) {
        if (!marker.vehicle || !existingMarkers[marker.vehicle]) {
            var existing = existingMarkers[marker.id];
            if (existing) {
                existing.setLatLng([marker.y, marker.x]);
                if (marker.symbol != existing.options.marker.symbol || marker.name != existing.options.marker.name) {
                    existing.options.marker = marker;
                    existing.setIcon(createIcon(marker));
                }
            }
            else {
                var newMarker = L.marker([marker.y, marker.x], { icon: createIcon(marker), marker: marker }).addTo(currentMap);
                newMarker.on('click', function () { showMarkerMenu(newMarker); });
                existingMarkers[marker.id] = newMarker;
                if (marker.kind == 'u') {
                    // TODO: Notify New Marker
                }
            }
            markersToKeep.push(marker.id);
        }
        if (marker.kind == 'g') {
            if (!knownTo[marker.id]) {
                knownTo[marker.id] = $('<option value="' + marker.id + '"></option>').text(marker.name).appendTo('#compose-to');
            }
            toToKeep.push(marker.id);
        }
    });

    Object.getOwnPropertyNames(existingMarkers).forEach(function (id) {
        if (markersToKeep.indexOf(id) == -1) {
            existingMarkers[id].remove();
            delete existingMarkers[id];
        }
    });

    Object.getOwnPropertyNames(knownTo).forEach(function (id) {
        if (toToKeep.indexOf(id) == -1) {
            knownTo[id].remove();
            delete knownTo[id];
        }
    });
}

function updateMarkersPosition(makers) {
    makers.forEach(function (marker) {
        var existing = existingMarkers[marker.id];
        if (existing) {
            var latLng = existing.getLatLng();
            if (latLng.lat != marker.y || latLng.lng != marker.x) {
                existing.setLatLng([marker.y, marker.x]);
            }
        }
    });
}

function displayMessage(link, message) {
    $('#inbox-list a').removeClass('active');
    $('#outbox-list a').removeClass('active');
    $(link).addClass('active');
    $('#inbox-title').text(message.title);
    $('#inbox-message').text(message.body);
    $('#inbox-delete').show();
    if (message.state == 0) {
        $(link).find('i').removeClass('fa fa-envelope');
        $(link).find('i').addClass('far fa-envelope-open');
        message.state = 1;
        connection.invoke("WebMessageRead", { id: message.id });
    }
    displayedMessage = message;
    updateUnread();
}

function updateInbox(messages) {
    var messagesToKeep = [];
    messages.forEach(function (message) {
        var existing = existingMessages[message.id];
        if (!existing) {
            var li = $('<li class="nav-item w-100" style="font-size:0.8em"><a class="nav-link p-1 text-truncate" href="#"><i class=""></i> <span></span></a></li>');
            li.find('span').text(message.title);
            li.find('a').on('click', function () { displayMessage(this, message); return false; });
            if (message.state != 2) { // sent mail
                li.find('i').addClass(message.state == 0 ? 'fa fa-envelope' : 'far fa-envelope-open');
                li.appendTo($('#inbox-list'));
            }
            else {
                li.find('i').addClass('fas fa-share');
                li.appendTo($('#outbox-list'));
            }
            existingMessages[message.id] = li;
            if (message.state == 0) {
                // TODO: Notify New Mail
            } else if (message.state == 2) {
                // TODO: Notify Mail Sent
            }
        } else {
            if (message.state == 1) {
                existing.find('i').removeClass('fa fa-envelope');
                existing.find('i').addClass('far fa-envelope-open');
            }
        }
        messagesToKeep.push(message.id);
    });

    Object.getOwnPropertyNames(existingMessages).forEach(function (id) {
        if (messagesToKeep.indexOf(id) == -1) {
            existingMessages[id].remove();
            delete existingMessages[id];
            if (displayedMessage && displayedMessage.id == id) {
                clearMessage();
            }
        }
    });

    updateUnread();
}

$(function () {

    $('#statusbar').on('click', function () { if (connection.state === signalR.HubConnectionState.Disconnected) { connection.start(); } });

    initMap(Arma3Map.Maps[vm.initialMap || 'altis']); // Starts on altis by default

    clearMessage();

    function connectionLost(e) {
        if (e) {
            $('#status').text(texts.disconnected);
            $('#statusbadge').attr('class', 'badge badge-danger');
        }
    }
    function connected() {
        $('#status').text(texts.waiting);
        $('#statusbadge').attr('class', 'badge badge-warning');
    }
    function started() {
        $('#status').text(texts.connected);
        $('#statusbadge').attr('class', 'badge badge-success');
    }

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/hub")
        .withAutomaticReconnect()
        .build();

    connection.on("Mission", function (missionData) {
        try {
            var worldName = missionData.worldName.toLowerCase();
            if (Arma3Map.Maps[worldName]) {
                initMap(Arma3Map.Maps[worldName]);
            } else {
                // TODO !
            }
            updateClock(missionData.date);
            started();
        }
        catch (e) {
            console.error(e);
        }
    });

    connection.on("SetPosition", function (positionData) {
        updateClock(positionData.date);
        updatePosition(positionData.x, positionData.y, positionData.heading, positionData.group, positionData.vehicle);
    });

    connection.on("UpdateMarkers", function (data) {
        try {
            updateMarkers(data.makers);
        }
        catch (e) {
            console.error(e);
        }
    });

    connection.on("UpdateMarkersPosition", function (data) {
        try {
            updateMarkersPosition(data.makers);
        }
        catch (e) {
            console.error(e);
        }
    });

    connection.on("Devices", function (data) {
        if (data.level == 0) {
            removeAllMarkers();
        }
        if (data.level == 3) {
            inboxButton.addTo(currentMap);
            composeButton.addTo(currentMap);
        }
        else {
            inboxButton.remove();
            composeButton.remove();
        }
        useMils = data.useMils;
    });

    connection.on("UpdateMessages", function (data) {
        try {
            updateInbox(data.messages);
        }
        catch (e) {
            console.error(e);
        }
    });

    connection.start().then(function () {
        connected();
        connection.invoke("WebHello", { token: vm.token });
    }).catch(connectionLost);

    connection.onreconnecting(connectionLost);
    connection.onreconnected(function () {
        connected();
        connection.invoke("WebHello", { token: vm.token });
    });


    $('#compose-send').on('click', function () {
        var to = $('#compose-to').val();
        var body = $('#compose-text').val();

        connection.invoke("WebSendMessage", { to: to, body: body});

        $('#compose-text').val('');
        $('#compose').modal('hide');
    });

    $('#inbox-delete').on('click', function () {
        if (displayedMessage) {
            connection.invoke("WebDeleteMessage", { id: displayedMessage.id });
            clearMessage();
        }
    });

    if (!document.documentElement.requestFullscreen) {
        // If fullscreen not available, ensure that height never needs scrolling
        $('.map').css('height', (window.innerHeight - 30) + 'px');
        $(window).on('resize', function () { $('.map').css('height', (window.innerHeight - 30) + 'px'); window.scrollTo(0, 0); });
    }


});