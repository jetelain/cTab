// cTab Session Replay Engine
// Self-contained: no SignalR, no live-session globals

(function () {

    // ---- Map / rendering state ----
    var currentMap = null;
    var currentMapInfos = null;
    var selfMarker = null;
    var existingMarkers = {};
    var existingMapMarkers = {};
    var useMils = false;
    var currentWorldName = null;

    // ---- Playback state ----
    var replayData = null;
    var isPlaying = false;
    var replaySpeed = 1.0;
    var currentPlaybackMs = 0;
    var lastAppliedEventIndex = 0;
    var replayDuration = 0;
    var tickInterval = null;
    var lastTickTime = null;

    // ---- Utility (copied from common.js) ----
    function pad(n, width) {
        n = n + '';
        return n.length >= width ? n : new Array(width - n.length + 1).join('0') + n;
    }

    function toHeadingUnit(degrees) {
        var text;
        if (useMils) {
            text = '' + Math.trunc(degrees * 6400 / 360);
        } else {
            text = '' + degrees.toFixed(2) + '°';
        }
        return text;
    }

    // ---- Rendering (adapted from map.js) ----
    function createIcon(marker) {
        if (marker.symbol === 'img:tic.png') {
            var inner = document.createElement('div');
            inner.className = 'text-marker-content-small';
            var img = document.createElement('img');
            img.src = '/img/tic.png';
            img.width = 56;
            img.height = 56;
            inner.appendChild(img);
            inner.appendChild(document.createTextNode('\u00a0'));
            inner.appendChild(document.createTextNode(marker.name));
            return new L.DivIcon({ className: 'text-marker', html: inner.outerHTML, iconAnchor: [28, 28] });
        }
        if (/^img:/.test(marker.symbol)) {
            var inner = document.createElement('div');
            inner.className = 'text-marker-content-small';
            var img = document.createElement('img');
            img.src = '/img/' + marker.symbol.substr(4);
            img.width = 32;
            img.height = 32;
            inner.appendChild(img);
            inner.appendChild(document.createTextNode('\u00a0'));
            inner.appendChild(document.createTextNode(marker.name));
            return new L.DivIcon({ className: 'text-marker', html: inner.outerHTML, iconAnchor: [16, 16] });
        }
        var symOptions = { size: 24, additionalInformation: marker.name, strokeWidth: 6, outlineWidth: 3 };
        if (marker.kind === 'u' && marker.heading < 360) {
            symOptions.direction = marker.heading;
        }
        var sym = new ms.Symbol(marker.symbol, symOptions);
        return L.icon({
            iconUrl: sym.asCanvas(window.devicePixelRatio * 2).toDataURL(),
            iconSize: [sym.getSize().width, sym.getSize().height],
            iconAnchor: [sym.getAnchor().x, sym.getAnchor().y]
        });
    }

    function generateIcon(data) {
        var url = '/img/markers/' + data.icon;
        if (data.label.length > 0 || data.dir) {
            var inner = document.createElement('div');
            inner.className = 'text-marker-content';
            inner.style.color = '#' + data.color;
            var img = document.createElement('img');
            img.src = url;
            img.width = 32;
            img.height = 32;
            if (data.dir) { img.style.transform = 'rotate(' + data.dir + 'deg)'; }
            inner.appendChild(img);
            inner.appendChild(document.createTextNode(data.label));
            return new L.DivIcon({ className: 'text-marker', html: inner.outerHTML, iconAnchor: [16, 16] });
        }
        return L.icon({ iconUrl: url, iconSize: [32, 32], iconAnchor: [16, 16] });
    }

    function removeAllMarkers() {
        Object.keys(existingMarkers).forEach(function (id) { existingMarkers[id].remove(); });
        existingMarkers = {};
        Object.keys(existingMapMarkers).forEach(function (name) { existingMapMarkers[name].remove(); });
        existingMapMarkers = {};
        if (selfMarker) { selfMarker.remove(); selfMarker = null; }
    }

    function initReplayMap(mapInfos, worldName) {
        if (!mapInfos.worldName) { mapInfos.worldName = worldName; }
        if (currentMap) { removeAllMarkers(); currentMap.remove(); }
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
        L.latlngGraticule({ zoomInterval: [{ start: 0, end: 10, interval: 1000 }] }).addTo(map);
        L.control.scale({ maxWidth: 200, imperial: false }).addTo(map);
        currentMap = map;
        currentMapInfos = mapInfos;
        currentWorldName = worldName;
        selfMarker = null;
    }

    function updateClock(date) {
        var d = new Date(date);
        document.getElementById('replay-clock').textContent = pad(d.getUTCHours(), 2) + ':' + pad(d.getUTCMinutes(), 2);
    }

    function updatePosition(x, y, heading, grp, veh) {
        var marker = existingMarkers[veh || grp];
        if (marker) {
            if (selfMarker && !selfMarker.options.marker) { selfMarker.remove(); }
            selfMarker = marker;
            marker.setLatLng([y, x]);
        } else {
            if (selfMarker && !selfMarker.options.marker) {
                selfMarker.setLatLng([y, x]);
            } else {
                selfMarker = L.marker([y, x], { icon: createIcon({ symbol: '10031000001211000000', name: '', kind: 'u', heading: heading }) }).addTo(currentMap);
            }
        }
    }

    function updateMarkers(makers) {
        var markersToKeep = [];
        makers.forEach(function (marker) {
            if (!marker.vehicle || !existingMarkers[marker.vehicle]) {
                var existing = existingMarkers[marker.id];
                if (existing) {
                    existing.setLatLng([marker.y, marker.x]);
                    if (marker.symbol !== existing.options.marker.symbol || marker.name !== existing.options.marker.name) {
                        existing.options.marker = marker;
                        existing.setIcon(createIcon(marker));
                    }
                } else {
                    var newMarker = L.marker([marker.y, marker.x], {
                        icon: createIcon(marker),
                        marker: marker,
                        zIndexOffset: marker.kind === 'u' ? -1000 : 0
                    }).addTo(currentMap);
                    existingMarkers[marker.id] = newMarker;
                }
                markersToKeep.push(marker.id);
            }
        });
        Object.keys(existingMarkers).forEach(function (id) {
            if (markersToKeep.indexOf(id) === -1) {
                existingMarkers[id].remove();
                delete existingMarkers[id];
            }
        });
    }

    function updateMarkersPosition(makers) {
        makers.forEach(function (marker) {
            var existing = existingMarkers[marker.id];
            if (existing) { existing.setLatLng([marker.y, marker.x]); }
        });
    }

    function updateMapMarkers(msg) {
        var markersToKeep = [];

        function rotatePoints(center, points, yaw) {
            var res = [], angle = yaw * (Math.PI / 180);
            for (var i = 0; i < points.length; i++) {
                var p = points[i];
                var p2 = [p[0] - center[0], p[1] - center[1]];
                var p3 = [Math.cos(angle) * p2[0] - Math.sin(angle) * p2[1], Math.sin(angle) * p2[0] + Math.cos(angle) * p2[1]];
                res.push([p3[0] + center[0], p3[1] + center[1]]);
            }
            return res;
        }

        function latLngPoints(items) {
            var array = [];
            for (var i = 0; i < items.length; i += 2) {
                array.push(new L.LatLng(items[i + 1], items[i]));
            }
            return array;
        }

        function process(list, update, create) {
            list.forEach(function (data) {
                var marker = existingMapMarkers[data.name];
                if (marker) {
                    update(data, marker, marker.lastData);
                    marker.lastData = data;
                } else {
                    marker = create(data);
                    if (marker) {
                        existingMapMarkers[data.name] = marker;
                        update(data, marker, { pos: [] });
                        marker.lastData = data;
                        marker.addTo(currentMap);
                    }
                }
                markersToKeep.push(data.name);
            });
        }

        process(msg.icons,
            function (m, e, lastData) {
                if (lastData.pos[1] !== m.pos[1] || lastData.pos[0] !== m.pos[0]) { e.setLatLng([m.pos[1], m.pos[0]]); }
                if (lastData.label !== m.label || lastData.dir !== m.dir || lastData.icon !== m.icon) { e.setIcon(generateIcon(m)); }
            },
            function (m) { return L.marker([m.pos[1], m.pos[0]], { interactive: false }); });

        process(msg.simples,
            function (m, e, lastData) {
                if (lastData.color !== m.color || lastData.alpha !== m.alpha || lastData.brush !== m.brush) {
                    e.setStyle({ stroke: false, fillColor: '#' + m.color, fillOpacity: m.alpha * (m.brush === 'SolidFull' ? 1 : 0.4) });
                }
            },
            function (m) {
                if (m.shape === 'rectangle') {
                    if (m.dir) {
                        return L.polygon(rotatePoints([m.pos[1], m.pos[0]], [
                            [m.pos[1] - m.size[1], m.pos[0] - m.size[0]], [m.pos[1] - m.size[1], m.pos[0] + m.size[0]],
                            [m.pos[1] + m.size[1], m.pos[0] + m.size[0]], [m.pos[1] + m.size[1], m.pos[0] - m.size[0]]
                        ], m.dir), { interactive: false });
                    }
                    return L.rectangle([[m.pos[1] - m.size[1], m.pos[0] - m.size[0]], [m.pos[1] + m.size[1], m.pos[0] + m.size[0]]], { interactive: false });
                }
                return L.circle([m.pos[1], m.pos[0]], { radius: m.size[0], interactive: false });
            });

        process(msg.polylines,
            function (m, e, lastData) {
                if (lastData.color !== m.color || lastData.alpha !== m.alpha) { e.setStyle({ color: '#' + m.color, opacity: m.alpha }); }
            },
            function (m) { return new L.Polyline(latLngPoints(m.points), { interactive: false }); });

        Object.keys(existingMapMarkers).forEach(function (name) {
            if (markersToKeep.indexOf(name) === -1) { existingMapMarkers[name].remove(); delete existingMapMarkers[name]; }
        });
    }

    // ---- Event application ----
    function applyEvent(evt) {
        try {
            var d = evt.data;
            switch (evt.type) {
                case 'Mission':
                    var worldName = d.worldName.toLowerCase();
                    if (Arma3Map.Maps[worldName] && worldName !== currentWorldName) {
                        initReplayMap(Arma3Map.Maps[worldName], worldName);
                    }
                    updateClock(d.date);
                    break;
                case 'SetPosition':
                    updateClock(d.date);
                    updatePosition(d.x, d.y, d.heading, d.group, d.vehicle);
                    break;
                case 'UpdateMarkers':
                    updateMarkers(d.makers);
                    break;
                case 'UpdateMarkersPosition':
                    updateMarkersPosition(d.makers);
                    break;
                case 'UpdateMapMarkers':
                    updateMapMarkers(d);
                    break;
            }
        } catch (e) {
            console.error('applyEvent error', evt.type, e);
        }
    }

    function applyEventsUntil(targetMs) {
        while (lastAppliedEventIndex < replayData.events.length) {
            var evt = replayData.events[lastAppliedEventIndex];
            var evtMs = new Date(evt.data.timestamp).getTime() - new Date(replayData.recordingStart).getTime();
            if (evtMs > targetMs) { break; }
            applyEvent(evt);
            lastAppliedEventIndex++;
        }
    }

    // ---- Playback engine ----
    function tick() {
        if (!isPlaying || !replayData) { return; }
        var now = Date.now();
        var elapsed = (now - lastTickTime) * replaySpeed;
        lastTickTime = now;
        currentPlaybackMs = Math.min(currentPlaybackMs + elapsed, replayDuration);
        applyEventsUntil(currentPlaybackMs);
        updateTimeDisplay();
        if (currentPlaybackMs >= replayDuration) {
            pause();
        }
    }

    function play() {
        if (!replayData || isPlaying) { return; }
        isPlaying = true;
        lastTickTime = Date.now();
        tickInterval = setInterval(tick, 100);
        document.getElementById('replay-play-btn').innerHTML = '<i class="fas fa-pause"></i>';
    }

    function pause() {
        isPlaying = false;
        if (tickInterval) { clearInterval(tickInterval); tickInterval = null; }
        document.getElementById('replay-play-btn').innerHTML = '<i class="fas fa-play"></i>';
    }

    function seekTo(ms) {
        var wasPlaying = isPlaying;
        pause();
        currentPlaybackMs = Math.max(0, Math.min(ms, replayDuration));
        var savedCenter = currentMap ? currentMap.getCenter() : null;
        var savedZoom = currentMap ? currentMap.getZoom() : null;
        removeAllMarkers();
        lastAppliedEventIndex = 0;
        applyEventsUntil(currentPlaybackMs);
        if (savedCenter && currentMap) {
            currentMap.setView(savedCenter, savedZoom, { animate: false });
        }
        updateTimeDisplay();
        if (wasPlaying) { play(); }
    }

    function formatDuration(ms) {
        var s = Math.floor(ms / 1000);
        var m = Math.floor(s / 60);
        var h = Math.floor(m / 60);
        return pad(h, 2) + ':' + pad(m % 60, 2) + ':' + pad(s % 60, 2);
    }

    function updateTimeDisplay() {
        document.getElementById('replay-time').textContent = formatDuration(currentPlaybackMs) + ' / ' + formatDuration(replayDuration);
        document.getElementById('replay-timeline').value = replayDuration > 0 ? Math.round(currentPlaybackMs / replayDuration * 1000) : 0;
    }

    // ---- Recording loader ----
    function loadRecording(json) {
        pause();
        replayData = json;
        replayDuration = new Date(json.recordingEnd).getTime() - new Date(json.recordingStart).getTime();
        currentPlaybackMs = 0;
        lastAppliedEventIndex = 0;
        currentWorldName = null;

        document.getElementById('replay-overlay').classList.add('d-none');
        document.getElementById('replay-controls').classList.remove('d-none');
        document.getElementById('replay-recording-name').textContent = json.worldName + '  —  ' + new Date(json.recordingStart).toUTCString();
        var timeline = document.getElementById('replay-timeline');
        timeline.max = 1000;
        timeline.value = 0;

        // Initialize map on first Mission event, then apply up to start
        var missionEvent = json.events.find(function (e) { return e.type === 'Mission'; });
        if (missionEvent) {
            applyEvent(missionEvent);
        } else {
            var worldName = (json.worldName || 'altis').toLowerCase();
            var mapInfos = Arma3Map.Maps[worldName] || Arma3Map.Maps['altis'];
            initReplayMap(mapInfos, worldName);
        }
        updateTimeDisplay();
    }

    // ---- UI wiring ----
    document.addEventListener('DOMContentLoaded', function () {
        // Initial map (blank altis)
        var defaultMap = Arma3Map.Maps['altis'];
        initReplayMap(defaultMap, 'altis');

        document.getElementById('replay-file').addEventListener('change', function () {
            var file = this.files[0];
            if (!file) { return; }
            var reader = new FileReader();
            reader.onload = function (e) {
                try {
                    loadRecording(JSON.parse(e.target.result));
                } catch (err) {
                    alert('Failed to load recording: ' + err.message);
                }
            };
            reader.readAsText(file);
        });

        document.getElementById('replay-play-btn').addEventListener('click', function () {
            if (isPlaying) { pause(); } else { play(); }
        });

        document.getElementById('replay-timeline').addEventListener('input', function () {
            if (!replayData) { return; }
            seekTo(parseInt(this.value) / 1000 * replayDuration);
        });

        document.getElementById('replay-speed').addEventListener('change', function () {
            replaySpeed = parseFloat(this.value);
        });

        document.getElementById('replay-load-btn').addEventListener('click', function () {
            pause();
            document.getElementById('replay-overlay').classList.remove('d-none');
            document.getElementById('replay-controls').classList.add('d-none');
        });

        window.addEventListener('resize', function () {
            document.getElementById('map').style.height = (window.innerHeight - 46) + 'px';
        });
        document.getElementById('map').style.height = (window.innerHeight - 46) + 'px';

        document.querySelectorAll('[data-replay-src]').forEach(function (el) {
            el.addEventListener('click', function () {
                fetch(el.dataset.replaySrc)
                    .then(function (r) { return r.json(); })
                    .then(function (data) { loadRecording(data); })
                    .catch(function (e) { console.error('Failed to load recording', e); });
            });
        });
    });

}());
