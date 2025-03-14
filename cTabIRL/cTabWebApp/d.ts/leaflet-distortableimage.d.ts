/// <reference path="leaflet.d.ts" /> 

declare namespace L {

    export interface DistortableImageOverlayOptions {

    }

    export class DistortableImageOverlay extends L.ImageOverlay {
        constructor(imageUrl: string, options?: DistortableImageOverlayOptions);
    }
}