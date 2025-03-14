/// <reference path="../../d.ts/leaflet.d.ts" />
/// <reference path="../../d.ts/leaflet-distortableimage.d.ts" />

declare namespace L {
    namespace control {
        function overlayButton(options: { position: string, content: string, click: () => void }): L.Control;
    }
}

declare function $(selector: string): any;

namespace CTab {

    interface IntelItem {
        listItem?: HTMLElement;
        projected?: L.DistortableImageOverlay;
        marker?: L.Marker;
        popup?: L.Popup;
        isProjected?: boolean;
        entry: IntelEntry;
    }

    interface IntelEntry {
        id: string;
        imageUri: string;
        location: number[];
        imageArea: number[][];
        imageProject: boolean;
    }

    interface IntelBackend {
        removeEntry(id: string);
    }

    function generateFeedItem(item: IntelItem, entry: IntelEntry, ui: IntelUI): HTMLElement {

        let listItem = document.createElement('div');
        listItem.className = 'col p-1';
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
        listItem.appendChild(a);
        return listItem;
    }

    function generatePopupContent(item: IntelItem, entry: IntelEntry, ui: IntelUI): HTMLElement {

        let popupContent = document.createElement('div');
        let popupLink = document.createElement('a');
        popupLink.href = entry.imageUri;
        popupLink.target = '_blank';
        let popupImg = document.createElement('img');
        popupImg.src = entry.imageUri;
        popupImg.width = 300;
        popupLink.appendChild(popupImg);
        popupContent.appendChild(popupLink);

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
            label.textContent = 'Show on map';

            switchContainer.appendChild(checkbox);
            switchContainer.appendChild(label);

            checkbox.addEventListener('change', function () {
                item.isProjected = this.checked;
                if (this.checked) {
                    item.projected.addTo(ui.currentMap);
                } else {
                    item.projected.remove();
                }
            });
            btnContainer.appendChild(switchContainer);
        }

        if (ui.backend) {
            let removeButton = document.createElement('button');
            removeButton.className = 'btn btn-outline-danger btn-sm';
            removeButton.textContent = 'Remove';
            removeButton.addEventListener('click', function () {
                ui.backend.removeEntry(entry.id);
                ui.currentMap.closePopup();
            });
            btnContainer.appendChild(removeButton);
        }
        popupContent.appendChild(btnContainer);
        return popupContent;
    }

    export class IntelUI {

        intelItems = {};
        intelLayer: L.LayerGroup;
        currentMap: L.Map;
        intelButtom: L.Control;
        markersCheckbox: HTMLInputElement;
        photoIcon: L.Icon;
        backend?: IntelBackend;
        constructor() {
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
            this.markersCheckbox = document.getElementById('intel-feed-markers') as HTMLInputElement;
            this.markersCheckbox.addEventListener('change', function () {
                if (this.checked) {
                    self.intelLayer.addTo(self.currentMap);
                } else {
                    self.intelLayer.remove();
                }
            });
        }

        attachToMap(map: L.Map, backend?: IntelBackend) {
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
            this.intelButtom = L.control.overlayButton({
                position: 'bottomleft',
                content: '<i class="fas fa-rss"></i>',
                click: function () {
                    $('#intel-feed').modal('show');
                }
            }).addTo(map);
        }

        updateIntel(entries: IntelEntry[]) {
            let ids: string[] = [];
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

        private addEntry(entry: IntelEntry) {
            let item = this.intelItems[entry.id] = { entry: entry } as IntelItem;

            // Create list item
            item.listItem = generateFeedItem(item, entry, this);
            document.getElementById('intel-feed-items').prepend(item.listItem);

            // Create marker popup
            let latlng = [entry.location[1], entry.location[0]] as [number, number];

            item.popup = L.popup()
                .setLatLng(latlng)
                .setContent(generatePopupContent(item, entry, this));

            // Create icon marker
            item.marker = L.marker(latlng, { icon: this.photoIcon }).bindPopup(item.popup).addTo(this.intelLayer);

            // Create projected image if needed
            if (entry.imageProject) {
                var corners = entry.imageArea.map(x => [x[1], x[0]] as [number, number]);
                // our corners : _worldTopLeft, _worldTopRight, _worldBottomRight, _worldBottomLeft
                // wanted : topLeft, topRight, bottomLeft, bottomRight
                item.projected = new L.DistortableImageOverlay(entry.imageUri, { corners: [corners[0], corners[1], corners[3], corners[2]] });
            }
        }

        private removeEntry(id: string) {
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
}