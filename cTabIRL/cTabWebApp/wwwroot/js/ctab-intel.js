/// <reference path="../../d.ts/leaflet.d.ts" />
/// <reference path="../../d.ts/leaflet-distortableimage.d.ts" />
var CTab;
(function (CTab) {
    function generateFeedItem(item, entry, ui) {
        let listItem = document.createElement('div');
        listItem.className = 'col p-1 text-center';
        let a = document.createElement('a');
        a.href = '#';
        a.addEventListener('click', (event) => {
            event.preventDefault();
            document.getElementById('intel-feed').classList.remove('show');
            item.popup.openOn(ui.currentMap);
            $('#intel-feed').modal('hide');
        });
        let img = document.createElement('img');
        img.className = 'w-100';
        img.src = entry.imageUri;
        a.appendChild(img);
        let labelDiv = document.createElement('div');
        labelDiv.innerText = getIntelLabel(entry);
        a.appendChild(labelDiv);
        listItem.appendChild(a);
        return listItem;
    }
    function generatePopupContent(item, entry, ui) {
        let popupContent = document.createElement('div');
        let popupLink = document.createElement('a');
        popupLink.href = entry.imageUri;
        popupLink.target = '_blank';
        let popupImg = document.createElement('img');
        popupImg.src = entry.imageUri;
        popupImg.width = 300;
        popupLink.appendChild(popupImg);
        popupContent.appendChild(popupLink);
        let labelDiv = document.createElement('div');
        labelDiv.className = 'text-center h6';
        labelDiv.innerText = getIntelLabel(entry);
        popupContent.appendChild(labelDiv);
        let btnContainer = document.createElement('div');
        btnContainer.className = 'mt-1 d-flex justify-content-between';
        if (entry.imageProject) {
            let switchContainer = document.createElement('div');
            switchContainer.className = 'custom-control custom-switch';
            let checkbox = document.createElement('input');
            checkbox.type = 'checkbox';
            checkbox.className = 'custom-control-input';
            checkbox.id = 'todo';
            let label = document.createElement('label');
            label.className = 'custom-control-label reset-font-size';
            label.htmlFor = checkbox.id;
            label.textContent = texts.showOnMap;
            switchContainer.appendChild(checkbox);
            switchContainer.appendChild(label);
            checkbox.addEventListener('change', function () {
                item.isProjected = this.checked;
                if (this.checked) {
                    item.projected.addTo(ui.currentMap);
                }
                else {
                    item.projected.remove();
                }
            });
            btnContainer.appendChild(switchContainer);
        }
        if (ui.backend) {
            let removeButton = document.createElement('button');
            removeButton.className = 'btn btn-outline-danger btn-sm';
            removeButton.textContent = texts.deletePhoto;
            removeButton.addEventListener('click', function () {
                if (window.confirm(texts.deletePhotoConfirm)) {
                    ui.backend.removeEntry(entry.id);
                    ui.currentMap.closePopup();
                }
            });
            btnContainer.appendChild(removeButton);
        }
        popupContent.appendChild(btnContainer);
        return popupContent;
    }
    function uploadIntelArchive(file, ui) {
        if (file.size > 50 * 1024 * 1024) { // 50 MB
            alert('File size must be below 50 MB');
            return;
        }
        let formData = new FormData();
        formData.append('archive', file);
        // Get the anti-forgery token
        let token = document.querySelector('input[name="__RequestVerificationToken"]').value;
        formData.append('__RequestVerificationToken', token);
        fetch(document.querySelector('#intel-archive-upload').action, {
            method: 'POST',
            body: formData
        }).then(response => {
            if (!response.ok) {
                console.error(response);
                if (response.status == 400) {
                    response.text().then(value => alert(value));
                }
                else {
                    alert(response.statusText);
                }
            }
        }).catch(error => {
            console.error(error);
            alert('File upload failed');
        });
    }
    function promptIntelArchive(ui) {
        let input = document.createElement('input');
        input.type = 'file';
        input.accept = '.zip,application/zip';
        input.addEventListener('change', function () {
            if (input.files.length > 0) {
                uploadIntelArchive(input.files[0], ui);
            }
        });
        input.click();
    }
    class IntelUI {
        constructor() {
            this.intelItems = {};
            this.photoIcon = L.icon({
                iconUrl: '/img/marker-photo.png',
                shadowUrl: '/img/marker-shadow.png',
                iconSize: [30, 40],
                iconAnchor: [15, 40],
                popupAnchor: [1, -34],
                tooltipAnchor: [16, -28],
                shadowSize: [40, 40],
                shadowAnchor: [13, 40],
            });
        }
        init() {
            let self = this;
            this.markersCheckbox = document.getElementById('intel-feed-markers');
            this.markersCheckbox.addEventListener('change', function () {
                if (this.checked) {
                    self.intelLayer.addTo(self.currentMap);
                }
                else {
                    self.intelLayer.remove();
                }
            });
            document.getElementById('intel-archive-restore').addEventListener('click', function (ev) {
                ev.preventDefault();
                promptIntelArchive(self);
            });
        }
        attachToMap(map, backend) {
            if (!this.markersCheckbox) {
                this.init();
            }
            this.backend = backend;
            this.currentMap = map;
            this.intelLayer = L.layerGroup();
            this.intelItems = {};
            document.getElementById('intel-feed-items').innerHTML = '';
            if (this.markersCheckbox.checked) {
                this.intelLayer.addTo(this.currentMap);
            }
            this.intelButtom = null;
        }
        updateIntel(entries) {
            if (!this.intelButtom) {
                // Generate button only if server have an actual intel feed
                this.intelButtom = L.control.overlayButton({
                    position: 'bottomleft',
                    content: '<i class="fas fa-rss"></i>',
                    click: function () {
                        $('#intel-feed').modal('show');
                    }
                }).addTo(this.currentMap);
            }
            let ids = [];
            entries.forEach(entry => {
                ids.push(entry.id);
                if (!this.intelItems[entry.id]) {
                    this.addEntry(entry);
                }
            });
            Object.getOwnPropertyNames(this.intelItems).forEach(id => {
                if (!ids.includes(id)) {
                    this.removeEntry(id);
                }
            });
        }
        addEntry(entry) {
            let item = this.intelItems[entry.id] = { entry: entry };
            // Create list item
            item.listItem = generateFeedItem(item, entry, this);
            document.getElementById('intel-feed-items').prepend(item.listItem);
            // Create marker popup
            let latlng = [entry.location[1], entry.location[0]];
            item.popup = L.popup()
                .setLatLng(latlng)
                .setContent(generatePopupContent(item, entry, this));
            // Create icon marker
            item.marker = L.marker(latlng, { icon: this.photoIcon }).bindPopup(item.popup).addTo(this.intelLayer);
            // Create projected image if needed
            if (entry.imageProject) {
                var corners = entry.imageArea.map(x => [x[1], x[0]]);
                // our corners : _worldTopLeft, _worldTopRight, _worldBottomRight, _worldBottomLeft
                // wanted : topLeft, topRight, bottomLeft, bottomRight
                item.projected = new L.DistortableImageOverlay(entry.imageUri, { corners: [corners[0], corners[1], corners[3], corners[2]] });
            }
        }
        removeEntry(id) {
            let item = this.intelItems[id];
            if (item.marker) {
                this.intelLayer.removeLayer(item.marker);
            }
            if (item.projected) {
                item.projected.remove();
            }
            if (item.listItem) {
                item.listItem.remove();
            }
            delete this.intelItems[id];
        }
    }
    CTab.IntelUI = IntelUI;
    function getIntelLabel(entry) {
        let date = new Date(entry.dateTime);
        return Math.trunc(entry.location[0]).toString().padStart(5, '0') + ' - ' + Math.trunc(entry.location[1]).toString().padStart(5, '0') + ', ' + date.getUTCHours().toString().padStart(2, '0') + ':' + date.getUTCMinutes().toString().padStart(2, '0');
    }
})(CTab || (CTab = {}));
