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


var currentMap = null;
var currentMapInfos = null;
var selfMarker = null;
var existingMarkers = {};
var centerOnPosition = true;
var centerOnPositionButton;
var fullScreenButton = null;

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


function initMap(mapInfos) {
    if (mapInfos == currentMapInfos) {
        return;
    }
    if (currentMap != null) {
        existingMarkers = {};
        selfMarker = null;
        currentMap.remove();
    }
    var map = L.map('map', {
        minZoom: mapInfos.minZoom,
        maxZoom: mapInfos.maxZoom + 2,
        maxNativeZoom: mapInfos.maxZoom,
        crs: mapInfos.CRS
    });
    L.tileLayer('https://jetelain.github.io/Arma3Map' + mapInfos.tilePattern, {
        attribution: mapInfos.attribution,
        tileSize: mapInfos.tileSize,
        maxNativeZoom: mapInfos.maxZoom
    }).addTo(map);
    map.setView(mapInfos.center, mapInfos.maxZoom);
    map.on('mousedown', function () { setCenterOnPosition(false); });
    map.on('touchstart', function () { setCenterOnPosition(false); });

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

function pad(n, width) {
    n = n + '';
    return n.length >= width ? n : new Array(width - n.length + 1).join('0') + n;
}

function updateClock(date) {
    var dateObj = new Date(date);
    $('#date').text(pad(dateObj.getUTCHours(), 2) + ':' + pad(dateObj.getUTCMinutes(), 2));
}


function updatePosition(x, y, heading, grp, veh) {
    $('#position').text(pad(Math.trunc(x), 5) + ' - ' + pad(Math.trunc(y), 5));
    $('#heading').text('' + Math.trunc(heading * 6400 / 360));

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

    var connection = new signalR.HubConnectionBuilder()
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
        connection.invoke("WebHello", {});
        connected();
    }).catch(connectionLost);

    connection.onreconnecting(connectionLost);
    connection.onreconnected(function () {
        connection.invoke("WebHello", {});
        connected();
    });
});