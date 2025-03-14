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
var currentTacMapSynced = null;
var currentMapInfos = null;
var selfMarker = null;
var existingMarkers = {};
var existingMapMarkers = {};
var centerOnPosition = true;
var centerOnPositionButton = null;
var fullScreenButton = null;
var tempUserPopup = null;
var userMarkerData = {};
var connection = null;
var inboxButton = null;
var composeButton = null;
var knownTo = {};
var existingMessages = {};
var displayedMessage = null;
var noSleepButton = null;
var isNoSleep = false;
var noSleep = null;
var efisMapButton = null;
var ticButton = null;
var selfGroupId = null;
var selfIsInContact = false;
var groupsInContact = [];
var ctabFeatureLevel = 0;
var irlFeatureLevel = 0;
var preformatedMedevacUid = 'medevac';
let ctabCurrentDate = null;
let ctabIntel = new CTab.IntelUI();

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

function fullScreenToggle() {
    if (document.fullscreenElement) {
        if (isNoSleep) {
            noSleepToggle();
        }
        document.exitFullscreen().then(updateButtons);
    } else {
        if (!isNoSleep) { // Mobile chrome now will sleep even in fullscreen mode
            noSleepToggle();
        }
        document.documentElement.requestFullscreen().then(updateButtons);
    } 
}

function setCenterOnPosition(value) {
    centerOnPosition = value;
    updateButtons();
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
    var div = $('<div class="ctab-menu"></div>');
    if (id == 0) {
        var a = $('<div class="text-center"></div>');
        a.html(generateLocationInfos(latlng));
        a.appendTo(div);
    }
    var menu = menus['' + id];
    if (menu.class) {
        div.addClass(menu.class);
    }
    if (menu.title) {
        var a = $('<div class="text-center w-100" />').text(menu.title);
        a.appendTo(div);
    }
    menu.items.forEach(function (entry) {

        if (entry.featureLevel > ctabFeatureLevel) {
            return;
        }

        var a = $('<a class="dropdown-item" href="#"></a>');
        a.text(entry.label);
        a.attr('title', entry.tooltip);
        if (entry.preview) {
            a.css({ 'background-image': 'url(' + entry.preview + ')' });
        }

        //a.prepend($('<img />').attr({ src: entry.preview, height: 24 }));
        a.on('click', function () {
            if (entry.select1 !== null) userMarkerData.d1 = entry.select1;
            if (entry.select2 !== null) userMarkerData.d2 = entry.select2;
            if (entry.select3 !== null) userMarkerData.d3 = entry.select3;
            if (entry.nextMenu) {
                tempUserPopup.setContent(generateMenu(entry.nextMenu, latlng));
            } else {
                tempUserPopup.remove();
                connection.send('WebAddUserMarker',
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
            connection.send("WebDeleteUserMarker", { id: marker.options.marker.id });
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
    if (!vm.isSpectator) {
        var unread = $('#inbox-list').find('i.fa-envelope').length;
        if (inboxButton) {
            var span = inboxButton.j().find('span');
            span.text('' + unread);
            span.attr('class', unread > 0 ? 'badge badge-danger' : 'badge badge-secondary');
            inboxButton.setClass(unread > 0 ? 'btn-primary' : 'btn-outline-secondary');
        }
    }
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
    if (currentTacMapSynced) {
        currentTacMapSynced.close();
        currentTacMapSynced = null;
        $('#tacmap-disable').addClass('d-none');
    }
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

function initMap(mapInfos, worldName) {
    if (mapInfos == currentMapInfos) {
        removeAllMarkers();
        clearInbox();
        return;
    }
    if (!mapInfos.worldName) {
        mapInfos.worldName = worldName;
    }
    if (currentMap != null) {
        removeAllMarkers();
        clearInbox();
        currentMap.remove();
    }
    var map = L.map('map', {
        minZoom: mapInfos.minZoom,
        maxZoom: mapInfos.maxZoom + 4,
        maxNativeZoom: mapInfos.maxZoom,
        crs: mapInfos.CRS,
        doubleClickZoom: false
    });
    L.tileLayer('https://atlas.plan-ops.fr/data/1' + mapInfos.tilePattern, {
        attribution: mapInfos.attribution,
        tileSize: mapInfos.tileSize,
        maxNativeZoom: mapInfos.maxZoom
    }).addTo(map);
    map.setView(mapInfos.center, mapInfos.maxZoom);
    map.on('mousedown', function () { setCenterOnPosition(false); });
    map.on('touchstart', function () { setCenterOnPosition(false); });
    if (!vm.isSpectator) {
        map.on('dblclick contextmenu', function (e) { showMenu(e.latlng, generateMenu(0, e.latlng)); });
    }
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
    if (!vm.isSpectator) {
        (ticButton = L.control.overlayButton({
            content: 'TIC',
            click: function () { $('#tic').modal('show'); },
            initialClass: 'btn-outline-danger',
            position: 'topright'
        })).addTo(map);

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
    }



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

    (efisMapButton = L.control.overlayButton({
        position: 'bottomleft',
        content: '<i class="fas fa-helicopter"></i>',
        click: function () { document.location.href = $('#efislink').attr('href'); },
    })).addTo(map);
    efisMapButton.j().attr('title', $('#efislink').text());

    currentMap = map;
    currentMapInfos = mapInfos;
    selfMarker = null;

    ctabIntel.attachToMap(map, {
        removeEntry: function (id) {
            connection.send("WebDeleteIntel", { id: id });
        }
    });

    updateButtons();
};


function updateClock(date) {
    ctabCurrentDate = new Date(date);
    $('#date').text(pad(ctabCurrentDate.getUTCHours(), 2) + ':' + pad(ctabCurrentDate.getUTCMinutes(), 2));
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
    if (marker.symbol == "img:tic.png") {
        var iconHtml = $('<div></div>').append(
            $('<div></div>')
                .addClass('text-marker-content-small')
                .text(marker.name)
                .prepend($('<img src="/img/tic.png" width="56" height="56" />&nbsp;')))
            .html();
        return new L.DivIcon({
            className: 'text-marker',
            html: iconHtml,
            iconAnchor: [28, 28]
        });
    }
    if (/^img:/.test(marker.symbol)) {
        var iconHtml = $('<div></div>').append(
            $('<div></div>')
                .addClass('text-marker-content-small')
                .text(marker.name)
                .prepend($('<img src="/img/' + marker.symbol.substr(4) + '" width="32" height="32" />&nbsp;')))
            .html();

        return new L.DivIcon({
            className: 'text-marker',
            html: iconHtml,
            iconAnchor: [16, 16]
        });
    }
    var symOptions = { size: 24, additionalInformation: marker.name, strokeWidth: 6, outlineWidth:3 };
    if (marker.kind == 'u' && marker.heading < 360) {
        symOptions.direction = marker.heading;
    }
    var sym = new ms.Symbol(marker.symbol, symOptions);
    return L.icon({
        iconUrl: sym.asCanvas(window.devicePixelRatio*2).toDataURL(),
        iconSize: [sym.getSize().width, sym.getSize().height],
        iconAnchor: [sym.getAnchor().x, sym.getAnchor().y]
    });
}

function updateMarkers(makers) {

    var markersToKeep = [];
    var toToKeep = [];

    groupsInContact = [];
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
                var newMarker = L.marker([marker.y, marker.x], {
                    icon: createIcon(marker),
                    marker: marker,
                    zIndexOffset: marker.kind == 'u' ? -1000 : 0
                }).addTo(currentMap);
                newMarker.on('click', function () { showMarkerMenu(newMarker); });
                existingMarkers[marker.id] = newMarker;
                if (marker.kind == 'u') {
                    // TODO: Notify New Marker
                }
            }
            markersToKeep.push(marker.id);
        }
        if (marker.kind == 'u' && marker.symbol == "img:tic.png") {
            groupsInContact.push(marker.group);
        }
        if (marker.kind == 'g') {
            var to = knownTo[marker.id];
            if (!to) {
                knownTo[marker.id] = $('<option value="' + marker.id + '"></option>').text(marker.name).appendTo('#compose-to');
            } else if (to.text() != marker.name) {
                to.text(marker.name);
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

    updateIsInContact();
}

function updateIsInContact() {
    var isInContact = groupsInContact.indexOf(selfGroupId) != -1;
    if (selfIsInContact != isInContact) {
        selfIsInContact = isInContact;
        ticButton.setClass(isInContact ? 'btn-danger' : 'btn-outline-danger');
        $(isInContact ? '#tic-send' : '#tic-dismiss').hide();
        $(isInContact ? '#tic-dismiss' : '#tic-send').show();
    }
}

function generateIcon(data) {
    var url = '/img/markers/' + data.icon;
    if (data.label.length > 0 || data.dir) {
        var img = $('<img src="' + url + '" width="32" height="32" />');
        if (data.dir) {
            img.css('transform', 'rotate(' + data.dir + 'deg)')
        }
        var iconHtml = $('<div></div>').append(
            $('<div></div>')
                .addClass('text-marker-content')
                .css('color', '#' + data.color)
                .text(data.label)
                .prepend(img))
            .html();
        return new L.DivIcon({
            className: 'text-marker',
            html: iconHtml,
            iconAnchor: [16, 16]
        });
    }
    return L.icon({ iconUrl: url, iconSize: [32, 32], iconAnchor: [16, 16] });
}

function updateMapMarkers(msg) {

    var markersToKeep = [];

    function rotatePoints(center, points, yaw) {
        var res = [];
        var angle = yaw * (Math.PI / 180);
        for (var i = 0; i < points.length; i++) {
            var p = points[i];
            // translate to center
            var p2 = [p[0] - center[0], p[1] - center[1]];
            // rotate using matrix rotation
            var p3 = [Math.cos(angle) * p2[0] - Math.sin(angle) * p2[1], Math.sin(angle) * p2[0] + Math.cos(angle) * p2[1]];
            // translate back to center
            var p4 = [p3[0] + center[0], p3[1] + center[1]];
            // done with that point
            res.push(p4);
        }
        return res;
    }

    function process(list, update, create) {
        list.forEach(function (data) {
            var marker = existingMapMarkers[data.name];
            if (marker) {
                update(data, marker, marker.lastData);
                marker.lastData = data;
            }
            else {
                marker = create(data);
                if (marker) {
                    existingMapMarkers[data.name] = marker;
                    update(data, marker, { pos: []});
                    marker.lastData = data;
                    marker.addTo(currentMap);
                }
            }
            markersToKeep.push(data.name);
        });
    }

    function points(items) {
        var array = [];
        for (var i = 0; i < items.length; i+=2) {
            array.push(new L.LatLng(items[i + 1], items[i]));
        }
        return array;
    }

    process(msg.icons,
        function (m, e, lastData) {
            if (lastData.pos[1] != m.pos[1] || lastData.pos[0] != m.pos[0]) {
                e.setLatLng([m.pos[1], m.pos[0]]);
            }
            if (lastData.label != m.label || lastData.dir != m.dir || lastData.icon != m.icon) {
                e.setIcon(generateIcon(m));
            }
        },
        function (m) {
            return L.marker([m.pos[1], m.pos[0]], { interactive: false });
        });

    process(msg.simples,
        function (m, e, lastData) {
            if (lastData.color != m.color || lastData.alpha != m.alpha || lastData.brush != m.brush) {
                e.setStyle({ stroke: false, fillColor: '#' + m.color, fillOpacity: m.alpha * (m.brush =='SolidFull'?1:0.4) });
            }
        },
        function (m) {
            if (m.shape == 'rectangle') {
                if (m.dir) {
                    return L.polygon(rotatePoints([m.pos[1], m.pos[0]],[
                        [m.pos[1] - m.size[1], m.pos[0] - m.size[0]],
                        [m.pos[1] - m.size[1], m.pos[0] + m.size[0]],
                        [m.pos[1] + m.size[1], m.pos[0] + m.size[0]],
                        [m.pos[1] + m.size[1], m.pos[0] - m.size[0]]
                    ], m.dir), { interactive: false });
                }
                return L.rectangle([[m.pos[1] - m.size[1], m.pos[0] - m.size[0]], [m.pos[1] + m.size[1], m.pos[0] + m.size[0]]], { interactive: false });
            }
            return L.circle([m.pos[1], m.pos[0]], { radius: m.size[0], interactive: false });
        });

    process(msg.polylines,
        function (m, e, lastData) {
            if (lastData.color != m.color || lastData.alpha != m.alpha) {
                e.setStyle({ color: '#' + m.color, opacity: m.alpha });
            }
        },
        function (m) {
            return new L.Polyline(points(m.points), { interactive: false });
        });

    Object.getOwnPropertyNames(existingMapMarkers).forEach(function (name) {
        if (markersToKeep.indexOf(name) == -1) {
            existingMapMarkers[name].remove();
            delete existingMapMarkers[name];
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
        connection.send("WebMessageRead", { id: message.id });
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

var currentPreformated = { id: null };
var preformatedConfig = [];
var lastValues = {};
let ctabMessageTemlateUI = null;

function closePerformated() {
    if (ctabMessageTemlateUI) {
        ctabMessageTemlateUI.clear();
    }
    $('#compose-medevac').removeClass('btn-danger').addClass('btn-outline-danger');
    $('#compose-preformated').removeClass('btn-secondary').addClass('btn-outline-secondary');
    $('#compose-form-templates-help').addClass("d-none");
    $('#compose-text').removeClass("d-none");
    currentPreformated = { id: null };
}

function dateTimeLocalFormat(d) {
    if (!d) {
        return "";
    }
    function pad2Zero(s) {
        return s.padStart(2, '0');
    }
    return `${d.getUTCFullYear()}-${pad2Zero(d.getUTCMonth() + 1)}-${pad2Zero(d.getUTCDate())}T${pad2Zero(d.getUTCHours())}:${pad2Zero(d.getUTCMinutes())}`;
}

function showPerformated(uid) {

    if (!ctabMessageTemlateUI) {
        ctabMessageTemlateUI = new MessageTemplateUI(
            document.getElementById('compose-form-fields'),
            document.getElementById('compose-text'),
            {
                getCurrentLocation: () => $('#position').text(),
                getCallSign: () => (selfMarker && selfMarker.options.marker ? selfMarker.options.marker.name : null) ?? null,
                getCurrentDatetime: () => dateTimeLocalFormat(ctabCurrentDate)
            }
        );
    }

    closePerformated();

    if (uid == preformatedMedevacUid) {
        $('#compose-medevac').addClass('btn-danger').removeClass('btn-outline-danger');
    }
    else {
        $('#compose-preformated').addClass('btn-secondary').removeClass('btn-outline-secondary');
    }

    let config = preformatedConfig.find(e => e.uid == uid);

    currentPreformated = { id: uid, config: config };

    if (config) {
        ctabMessageTemlateUI.setup(config);
    } else {
        var content = $('<div class="mb-2" />');
        preformatedConfig.forEach(config => {
            content.append($('<a class="btn btn-sm btn-primary mr-2"></a>').text(config.title).on('click',function () {
                showPerformated(config.uid);
            }));
        });
        $('#compose-form-fields').append(content);
        $('#compose-form-templates-help').removeClass("d-none");
        $('#compose-text').addClass("d-none");
    }
}

function loadTacMapList() {
    fetch(vm.tacMapEndpoint + '/api/TacMaps?worldName=' + currentMapInfos.worldName, { mode: 'cors', credentials: 'include', cache: 'no-cache', redirect: 'error' })
        .then(result => {
            result.json().then(data => {
                $('#tacmap-notconnected').addClass('d-none');
                $('#tacmap-list').removeClass('d-none');
                $('#tacmap-list').empty();
                data.forEach(entry => {
                    $('#tacmap-list').append($('<a class="btn btn-outline-secondary btn-block text-left"></a>').text(entry.label).on('click', function () {
                        connection.send("WebSyncTacMap", { mapId: { tacMapID: entry.id, readToken: entry.readOnlyToken } });
                        $('#tacmaploader').modal('hide');
                    }).prepend($('<img />').attr({ src: entry.previewHref['256'], class: 'mr-2' }).css({ width: '64px', height: '64px'})));
                });
            });
        })
        .catch(err => {
            $('#tacmap-notconnected').removeClass('d-none');
            $('#tacmap-list').addClass('d-none');
        });
}


$(function () {

    $('#statusbar').on('click', function () { if (connection.state === signalR.HubConnectionState.Disconnected) { connection.start(); } });

    var worldName = vm.initialMap || 'altis';
    initMap(Arma3Map.Maps[worldName], worldName); // Starts on altis by default

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

        if (ctabFeatureLevel == 0 || irlFeatureLevel == 0) {
            ticButton.j().hide();
        } else {
            ticButton.j().show();
        }
    }

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/hub")
        .withAutomaticReconnect()
        .build();

    connection.on("Mission", function (missionData) {
        try {
            var worldName = missionData.worldName.toLowerCase();
            if (Arma3Map.Maps[worldName]) {
                initMap(Arma3Map.Maps[worldName], worldName);
            } else {
                // TODO !
            }
            updateClock(missionData.date);
            ctabFeatureLevel = missionData.ctabFeatureLevel;
            irlFeatureLevel = missionData.irlFeatureLevel;
            started();
        }
        catch (e) {
            console.error(e);
        }
    });

    connection.on("SetPosition", function (positionData) {
        updateClock(positionData.date);
        updatePosition(positionData.x, positionData.y, positionData.heading, positionData.group, positionData.vehicle);
        if (selfGroupId != positionData.group) {
            selfGroupId = positionData.group;
            updateIsInContact();
        }
    });

    connection.on("ActionRangeFinder", function (data) {
        try {
            setCenterOnPosition(false);

            var latlng = L.latLng(data.y, data.x);
            showMenu(latlng, generateMenu(0, latlng));
        }
        catch (e) {
            console.error(e);
        }
    });


    connection.on("UpdateMarkers", function (data) {
        try {
            updateMarkers(data.makers);
        }
        catch (e) {
            console.error(e);
        }
    });

    connection.on("UpdateMapMarkers", function (data) {
        try {
            updateMapMarkers(data);
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

        if (data.vehicleMode == 2) {
            efisMapButton.addTo(currentMap);
        }
        else {
            efisMapButton.remove();
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

    connection.on("UpdateSideFeed", function (data) {
        try {
            ctabIntel.updateIntel(data.entries);
        }
        catch (e) {
            console.error(e);
        }
    });

    connection.on("UpdateMessageTemplates", function (data) {
        try {
            preformatedConfig = data.templates;
            preformatedMedevacUid = preformatedConfig.find(e => e.type == "MedicalEvacuation")?.uid;
        }
        catch (e) {
            console.error(e);
        }
    });

    if (vm.tacMapEndpoint && window.Arma3TacMap) {
        connection.on("SyncTacMap", function (data) {
            if (currentTacMapSynced) {
                $('#tacmap-disable').addClass('d-none');
                try {
                    currentTacMapSynced.close();
                    currentTacMapSynced = null;
                }
                catch (e) {
                    console.error(e);
                }
            }
            if (data.mapId) {
                $('#tacmap-disable').removeClass('d-none');
                currentTacMapSynced = Arma3TacMap.connnectReadOnlyMap(currentMap, vm.tacMapEndpoint + '/MapHub', data.mapId, { 'mil': 0.25, 'basic': 1.0, 'line': 1.0 });
            }
        });
    }

    function sayHello() {
        if (vm.isSpectator) {
            connection.invoke("SpectatorHello", { spectatorToken: vm.spectatorToken });
        } else {
            connection.invoke("WebHello", { token: vm.token });
        }
    }

    connection.start().then(function () {
        connected();
        sayHello();
    }).catch(connectionLost);

    connection.onreconnecting(connectionLost);
    connection.onreconnected(function () {
        connected();
        sayHello();
    });

    if (!vm.isSpectator) {
        $('#compose-send').on('click', function () {
            var to = $('#compose-to').val();
            var body = $('#compose-text').val();

            var msg = { to: to, body: body };
            if (ctabMessageTemlateUI && ctabMessageTemlateUI.config) {
                msg.title = ctabMessageTemlateUI.config.shortTitle;
                msg.type = ctabMessageTemlateUI.config.type;
                msg.attachments = ctabMessageTemlateUI.getAttachments();
            }
            connection.send("WebSendMessage", msg);

            closePerformated();
            $('#compose-text').val('');
            $('#compose').modal('hide');
        });

        $('#compose-medevac').on('click', function () {
            if (currentPreformated.id == preformatedMedevacUid) {
                closePerformated();
            } else {
                showPerformated(preformatedMedevacUid);
            }
        });
        $('#compose-preformated').on('click', function () {
            if (currentPreformated.id && currentPreformated.id != preformatedMedevacUid) {
                closePerformated();
            } else {
                showPerformated('list');
            }
        });

        $('#inbox-delete').on('click', function () {
            if (displayedMessage) {
                connection.send("WebDeleteMessage", { id: displayedMessage.id });
                clearMessage();
            }
        });

        if (vm.tacMapEndpoint) {
            $('#tacmap-show').on('click', function () { $('#help').modal('hide'); $('#tacmaploader').modal('show'); loadTacMapList(); });
            $('#tacmap-refresh').on('click', loadTacMapList);
            $('#tacmap-disable').on('click', function () {
                connection.send("WebSyncTacMap", {});
                $('#help').modal('hide');
            });
        }

        $('#tic-send').on('click', function () {
            connection.send("WebTicAlert", { state: true});
        });
        $('#tic-dismiss').on('click', function () {
            connection.send("WebTicAlert", { state: false });
        }).hide();

    }

    setupFullViewHeight('.map');

    setupCopyButtons();
});
