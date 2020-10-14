L.Control.OverlayButton = L.Control.extend({
    options: {
        position: 'bottomright',
        initialClass: 'btn-outline-secondary',
        content: 'A'
    },

    previousClass: '',

    onAdd: function (map) {
        this.previousClass = this.options.initialClass;
        this._container = L.DomUtil.create('button', 'btn ' + this.options.initialClass);
        L.DomEvent.disableClickPropagation(this._container);
        this._container.innerHTML = this.options.content;
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
var centerOnPositionButton;
var fullScreenButton = null;
var tempUserPopup = null;
var userMarkerData = {};
var connection = null;

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
}

function fullScreenToggle() {
    if (document.fullscreenElement) {
        document.exitFullscreen().then(updateButtons);
    } else {
        document.documentElement.requestFullscreen().then(updateButtons);
    } 
}

function setCenterOnPosition(value) {
    centerOnPosition = value;
    updateButtons();
}

var octants = ['N', 'NE', 'E', 'SE', 'S', 'SO', 'O', 'NO', 'N'];

function toHeadingUnit(degrees) {
    var text = '' + Math.trunc(degrees * 6400 / 360);

    text += ' ' + octants[Math.round(degrees / 45)];

    return text;
}

function bearing(latlng1, latlng2) {
    return ((Math.atan2(latlng2.lng - latlng1.lng, latlng2.lat - latlng1.lat) * 180 / Math.PI) + 360) % 360;
}
//             return Math.Atan2(dy, dx) * 180d / Math.PI;

function generateMenu(id) {
    if (id == 0) {
        userMarkerData = {};
    }
    var div = $('<div></div>');

    if (id == 0) {
        var a = $('<div class="text-center"></div>');
        var infos = pad(Math.trunc(tempUserPopup.getLatLng().lng), 5) + ' - ' + pad(Math.trunc(tempUserPopup.getLatLng().lat), 5);
        if (selfMarker) {
            var pos1 = tempUserPopup.getLatLng();
            var pos2 = selfMarker.getLatLng();
            infos += '<br />' + Math.trunc(currentMap.distance(pos1, pos2)) + 'm ' + toHeadingUnit(Math.trunc(bearing(pos2, pos1)));
        }
        a.html(infos);
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
                tempUserPopup.setContent(generateMenu(entry.nextMenu));
            } else {
                tempUserPopup.remove();
                console.log(userMarkerData);

                connection.invoke('WebAddUserMarker',
                    {
                        x: Math.trunc(tempUserPopup.getLatLng().lng),
                        y: Math.trunc(tempUserPopup.getLatLng().lat),
                        data: [userMarkerData.d1 || 0, userMarkerData.d2 || 0, userMarkerData.d3 || 0]
                    });
            }
        });
        a.appendTo(div);
    });

    //'<div><a class="dropdown-item" href="#">Position ennemie</a><a class="dropdown-item"  href="#">Bléssés</a><a class="dropdown-item"  href="#">Autre</a></div>'

    return div.get(0);
}

function initMap(mapInfos) {
    if (mapInfos == currentMapInfos) {
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
        return;
    }
    if (currentMap != null) {
        existingMarkers = {};
        selfMarker = null;
        tempUserPopup = null;
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
    map.on('dblclick contextmenu', function (e) {
        if (!tempUserPopup) {
            tempUserPopup = L.popup({ className: 'menupopup'});
        }
        tempUserPopup.setLatLng(e.latlng);
        tempUserPopup.setContent(generateMenu(0));
        tempUserPopup.openOn(map);
    });
    /**/
    (centerOnPositionButton = L.control.overlayButton({ content:'<i class="fas fa-location-arrow"></i>'})).addTo(map);
    centerOnPositionButton.j().on('click', function () { setCenterOnPosition(!centerOnPosition); });

    if (document.documentElement.requestFullscreen) {
        (fullScreenButton = L.control.overlayButton({ content: '<i class="fas fa-expand"></i>' })).addTo(map);
        fullScreenButton.j().on('click', fullScreenToggle);
    }

    (L.control.overlayButton({ position: 'topright', content: '<i class="fas fa-inbox"></i>&nbsp;<span class="badge badge-secondary">0</span>' })).addTo(map);

    L.latlngGraticule({
        zoomInterval: [
            { start: 0, end: 10, interval: 1000 }
        ]}).addTo(map);
    L.control.scale({ maxWidth: 200, imperial: false }).addTo(map);
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
        if (selfMarker) {
            selfMarker.remove();
            selfMarker.delete();
        }
        marker.setLatLng([y, x]);
    }
    else {
        if (selfMarker) {
            selfMarker.setLatLng([y, x]);
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
                existingMarkers[marker.id] = L.marker([marker.y, marker.x], { icon: createIcon(marker), marker: marker }).addTo(currentMap);
                if (marker.kind == 'u') {
                    // TODO: Notify
                }
            }
            markersToKeep.push(marker.id);
        }
    });

    Object.getOwnPropertyNames(existingMarkers).forEach(function (id) {
        if (markersToKeep.indexOf(id) == -1) {
            existingMarkers[id].remove();
            delete existingMarkers[id];
        }
    });
}

$(function () {

    $('#statusbar').on('click', function () { if (connection.state === signalR.HubConnectionState.Disconnected) { connection.start(); } });

    initMap(Arma3Map.Maps.altis); // Starts on altis by default

    function connectionLost(e) {
        if (e) {
            $('#status').text('Disconnected');
            $('#statusbadge').attr('class', 'badge badge-danger');
        }
    }
    function connected() {
        $('#status').text('Wait for cTab');
        $('#statusbadge').attr('class', 'badge badge-warning');
    }
    function started() {
        $('#status').text('Connected');
        $('#statusbadge').attr('class', 'badge badge-success');
    }

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/hub")
        .withAutomaticReconnect()
        .build();

    connection.on("Mission", function (missionData) {
        //console.trace(missionData); 

        var worldName = missionData.worldName.toLowerCase();
        if (Arma3Map.Maps[worldName]) {
            initMap(Arma3Map.Maps[worldName]);
        } else {
            // TODO !
        }
        updateClock(missionData.date);
        started();
    });


    connection.on("SetPosition", function (positionData) {
        updateClock(positionData.date);
        updatePosition(positionData.x, positionData.y, positionData.heading, positionData.group, positionData.vehicle);
    });
    connection.on("UpdateMarkers", function (data) {
        updateMarkers(data.makers);
    });




    connection.start().then(function () {
        connected();
        connection.invoke("WebHello", {});
    }).catch(connectionLost);

    connection.onreconnecting(connectionLost);
    connection.onreconnected(function () {
        connected();
        connection.invoke("WebHello", {});
    });
});