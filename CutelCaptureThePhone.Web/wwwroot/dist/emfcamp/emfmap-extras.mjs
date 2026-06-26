import { Layer, LayerGroup, LayerSwitcher } from "/dist/maplibregl-layer-switcher/layer-switcher.mjs";

// Native pixel dimensions (from each icon's SVG viewBox) used to rasterise it for map.addImage
const iconSizes = {
    "camping": [12.75583, 8.50394],
    "no-access": [14.17323, 14.17323],
    "water": [14.17323, 11.33858],
    "water-point": [50, 58],
    "tree": [227, 214],
    "toilet": [30, 67],
    "datenklo": [31, 70],
    "datenklo_active": [31, 70],
    "datenklo_down": [31, 70],
    "marker": [30, 47],
    "marker-light": [30, 47],
    "power-distro": [40, 40],
    "power-generator": [60, 30],
    "network-switch": [40, 40],
    "network-switch-active": [40, 40],
    "network-switch-down": [40, 40]
};

function loadSvgImage(map, name, ratio) {
    return new Promise((resolve, reject) => {
        let [width, height] = iconSizes[name];

        let image = new Image(Math.round(width * ratio), Math.round(height * ratio));

        image.onload = () => {
            let canvas = document.createElement("canvas");
            canvas.width = image.width;
            canvas.height = image.height;

            let context = canvas.getContext("2d");
            context.drawImage(image, 0, 0, canvas.width, canvas.height);

            map.addImage(name, context.getImageData(0, 0, canvas.width, canvas.height), { pixelRatio: ratio });

            resolve();
        };

        image.onerror = reject;
        image.src = `/dist/emfcamp/icons/${name}.svg`;
    });
}

// Loads the amenity icons (toilets, power, network, etc.) referenced by the EMF map style's icon-image/fill-pattern layers
export function loadEmfMapIcons(map) {
    let ratio = Math.min(Math.round(window.devicePixelRatio || 1), 2);

    return Promise.all(Object.keys(iconSizes).map(name => loadSvgImage(map, name, ratio)));
}

// Mirrors the official map.emfcamp.org layer groupings (see EventMap.layers in emfcamp/map's web/src/map.ts)
export function createEmfLayerSwitcher() {
    return new LayerSwitcher([
        new Layer("g", "Grid", "grid_"),
        new LayerGroup("Background", [
            new Layer("b", "Map", "background_", "background", true),
            new Layer("s", "Slope", "slope", "background"),
            new Layer("h", "Hillshade", "hillshade", "background"),
            new Layer("o", "Aerial imagery", "ortho", "background"),
        ]),
        new LayerGroup("EMF", [
            new Layer("a", "Labels", "labels_", true),
            new Layer("t", "Structures", "structures_", true),
            new Layer("p", "Paths", "paths_", true),
            new Layer("v", "Villages", "villages_", true),
        ]),
        new LayerGroup("Infrastructure", [
            new Layer("w", "Power", "power_"),
            new Layer("n", "Network", "network_"),
            new Layer("l", "Lighting", "lighting_"),
        ]),
        new Layer("bs", "Buried services", "services_"),
    ]);
}

// leaflet-maplibre-gl renders into an oversized, offset panning buffer rather than the visible
// viewport, so maplibre's own addControl positions things off-screen. Wrap the switcher as a
// Leaflet control instead - Leaflet positions the returned element correctly, while the switcher
// still drives visibility on the real underlying MapLibre style via glMap.
export function addEmfLayerSwitcherControl(leafletMap, layerSwitcher, glMap) {
    let LeafletLayerSwitcherControl = L.Control.extend({
        options: { position: "topright" },
        onAdd: function () {
            return layerSwitcher.onAdd(glMap);
        },
        onRemove: function () {
            layerSwitcher.onRemove();
        }
    });

    leafletMap.addControl(new LeafletLayerSwitcherControl());
}
