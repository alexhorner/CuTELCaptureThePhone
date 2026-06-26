/*
 * MapLibre style generated from the EMF Camp map's style source
 * (https://github.com/emfcamp/map, web/src/style/{map_style,emf,basemap}.ts), ISC License -
 * Copyright EMF Camp contributors.
 *
 * Map data included in this style is licensed separately by its respective sources:
 * - basemap source: (c) OpenStreetMap contributors, Open Database License (ODbL) -
 *   see https://www.openstreetmap.org/copyright
 * - slope/hillshade sources: elevation data (c) Environment Agency 2022. All rights reserved.
 * - site_plan/villages sources: (c) EMF Camp - https://www.emfcamp.org
 */
let emfMapStyle = {
    "version": 8,
    "name": "EMF",
    "center": [
        -2.3784,
        52.0411
    ],
    "zoom": 16,
    "bearing": 0,
    "pitch": 0,
    "sources": {
        "basemap": {
            "type": "vector",
            "tiles": [
                "https://map.emfcamp.org/tiles/basemap/{z}/{x}/{y}"
            ],
            "maxzoom": 14,
            "attribution": "\u00a9 OpenStreetMap contributors"
        },
        "site_plan": {
            "type": "vector",
            "tiles": [
                "https://map.emfcamp.org/tiles/_main/{z}/{x}/{y}"
            ]
        },
        "villages": {
            "type": "geojson",
            "data": "/Api/EmfCamp/VillagesGeoJson"
        },
        "slope": {
            "type": "raster",
            "tiles": [
                "https://map.emfcamp.org/tiles/slope/{z}/{x}/{y}"
            ],
            "tileSize": 256,
            "attribution": "Elevation data \u00a9 Environment Agency 2022. All rights reserved."
        },
        "hillshade": {
            "type": "raster",
            "tiles": [
                "https://map.emfcamp.org/tiles/hillshade/{z}/{x}/{y}"
            ],
            "tileSize": 256,
            "attribution": "Elevation data \u00a9 Environment Agency 2022. All rights reserved."
        },
        "ortho": {
            "type": "raster",
            "tileSize": 256,
            "tiles": [
                "https://map.emfcamp.org/tiles/eastnor-ortho-202606/{z}/{x}/{y}"
            ]
        },
        "vehicles": {
            "type": "geojson",
            "data": {
                "type": "FeatureCollection",
                "features": []
            }
        },
        "grist_markers": {
            "type": "geojson",
            "data": {
                "type": "FeatureCollection",
                "features": []
            }
        }
    },
    "glyphs": "/dist/emfcamp/fonts/{fontstack}/{range}.pbf",
    "layers": [
        {
            "id": "background",
            "type": "background",
            "paint": {
                "background-color": "#DBE8A5"
            }
        },
        {
            "id": "water",
            "type": "fill",
            "source": "basemap",
            "source-layer": "water",
            "filter": [
                "==",
                "$type",
                "Polygon"
            ],
            "paint": {
                "fill-color": "rgb(194, 200, 202)",
                "fill-antialias": true
            }
        },
        {
            "id": "landuse_residential",
            "type": "fill",
            "source": "basemap",
            "source-layer": "landuse",
            "maxzoom": 16,
            "filter": [
                "all",
                [
                    "==",
                    "kind",
                    "residential"
                ]
            ],
            "paint": {
                "fill-color": "rgb(234, 234, 230)",
                "fill-opacity": [
                    "interpolate",
                    [
                        "exponential",
                        0.6
                    ],
                    [
                        "zoom"
                    ],
                    8,
                    0.8,
                    9,
                    0.6
                ]
            }
        },
        {
            "id": "background_forest",
            "type": "fill",
            "source": "basemap",
            "source-layer": "landuse",
            "filter": [
                "==",
                "kind",
                "wood"
            ],
            "paint": {
                "fill-color": "#528329"
            }
        },
        {
            "id": "waterway",
            "type": "line",
            "source": "basemap",
            "source-layer": "water",
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "LineString"
                ]
            ],
            "paint": {
                "line-color": "hsl(195, 17%, 78%)"
            }
        },
        {
            "id": "water_name",
            "type": "symbol",
            "source": "basemap",
            "source-layer": "water",
            "filter": [
                "==",
                "$type",
                "LineString"
            ],
            "layout": {
                "text-field": "{name}",
                "symbol-placement": "line",
                "text-rotation-alignment": "map",
                "symbol-spacing": 500,
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-size": 12
            },
            "paint": {
                "text-color": "rgb(157,169,177)",
                "text-halo-color": "rgb(242,243,240)",
                "text-halo-width": 1,
                "text-halo-blur": 1
            }
        },
        {
            "id": "building",
            "type": "fill",
            "source": "basemap",
            "source-layer": "buildings",
            "minzoom": 12,
            "paint": {
                "fill-color": "rgb(234, 234, 229)",
                "fill-outline-color": "rgb(219, 219, 218)",
                "fill-antialias": true
            }
        },
        {
            "id": "tunnel_motorway_casing",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 6,
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "LineString"
                ],
                [
                    "all",
                    [
                        "has",
                        "is_tunnel"
                    ],
                    [
                        "==",
                        "kind",
                        "motorway"
                    ]
                ]
            ],
            "layout": {
                "line-cap": "butt",
                "line-join": "miter"
            },
            "paint": {
                "line-color": "rgb(213, 213, 213)",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.4
                    ],
                    [
                        "zoom"
                    ],
                    5.8,
                    0,
                    6,
                    3,
                    20,
                    40
                ]
            }
        },
        {
            "id": "tunnel_motorway_inner",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 6,
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "LineString"
                ],
                [
                    "all",
                    [
                        "has",
                        "is_tunnel"
                    ],
                    [
                        "==",
                        "kind",
                        "motorway"
                    ]
                ]
            ],
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "rgb(234,234,234)",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.4
                    ],
                    [
                        "zoom"
                    ],
                    4,
                    2,
                    6,
                    1.3,
                    20,
                    30
                ]
            }
        },
        {
            "id": "aeroway-taxiway",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 12,
            "filter": [
                "all",
                [
                    "==",
                    "kind",
                    "aeroway"
                ],
                [
                    "==",
                    "kind_detail",
                    "taxiway"
                ]
            ],
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "hsl(0, 0%, 88%)",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.55
                    ],
                    [
                        "zoom"
                    ],
                    13,
                    1.8,
                    20,
                    20
                ]
            }
        },
        {
            "id": "aeroway-runway-casing",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 11,
            "filter": [
                "==",
                "kind",
                "aeroway"
            ],
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "hsl(0, 0%, 88%)",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.5
                    ],
                    [
                        "zoom"
                    ],
                    11,
                    4,
                    17,
                    55
                ]
            }
        },
        {
            "id": "aeroway-area",
            "type": "fill",
            "source": "basemap",
            "source-layer": "landuse",
            "minzoom": 4,
            "filter": [
                "==",
                "kind",
                "aeroway"
            ],
            "paint": {
                "fill-opacity": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    13,
                    0,
                    14,
                    1
                ],
                "fill-color": "rgba(255, 255, 255, 1)"
            }
        },
        {
            "id": "aeroway-runway",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 11,
            "maxzoom": 24,
            "filter": [
                "all",
                [
                    "==",
                    "kind",
                    "aeroway"
                ],
                [
                    "==",
                    "$type",
                    "LineString"
                ]
            ],
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "rgba(255, 255, 255, 1)",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.5
                    ],
                    [
                        "zoom"
                    ],
                    11,
                    4,
                    17,
                    50
                ]
            }
        },
        {
            "id": "highway_minor",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 8,
            "filter": [
                "==",
                "kind",
                "minor_road"
            ],
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "hsl(0, 0%, 80%)",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.55
                    ],
                    [
                        "zoom"
                    ],
                    13,
                    1.8,
                    20,
                    20
                ],
                "line-opacity": 0.9
            }
        },
        {
            "id": "highway_major_casing",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 11,
            "filter": [
                "==",
                "kind",
                "major_road"
            ],
            "layout": {
                "line-cap": "butt",
                "line-join": "miter"
            },
            "paint": {
                "line-color": "rgb(213, 213, 213)",
                "line-dasharray": [
                    12,
                    0
                ],
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.3
                    ],
                    [
                        "zoom"
                    ],
                    10,
                    3,
                    20,
                    23
                ]
            }
        },
        {
            "id": "highway_major_inner",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 11,
            "filter": [
                "==",
                "kind",
                "major_road"
            ],
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "#fff",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.3
                    ],
                    [
                        "zoom"
                    ],
                    10,
                    2,
                    20,
                    20
                ]
            }
        },
        {
            "id": "highway_major_subtle",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "maxzoom": 11,
            "filter": [
                "==",
                "kind",
                "major_road"
            ],
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "hsla(0, 0%, 85%, 0.69)",
                "line-width": 2
            }
        },
        {
            "id": "highway_motorway_casing",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 6,
            "filter": [
                "==",
                "kind",
                "highway"
            ],
            "layout": {
                "line-cap": "butt",
                "line-join": "miter"
            },
            "paint": {
                "line-color": "rgb(213, 213, 213)",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.4
                    ],
                    [
                        "zoom"
                    ],
                    5.8,
                    0,
                    6,
                    3,
                    20,
                    40
                ],
                "line-dasharray": [
                    2,
                    0
                ]
            }
        },
        {
            "id": "highway_motorway_inner",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 6,
            "filter": [
                "==",
                "kind",
                "highway"
            ],
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": [
                    "interpolate-hcl",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    6,
                    "hsla(0, 0%, 85%, 0.53)",
                    8,
                    "#fff"
                ],
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.4
                    ],
                    [
                        "zoom"
                    ],
                    4,
                    2,
                    6,
                    1.3,
                    20,
                    30
                ]
            }
        },
        {
            "id": "highway_motorway_subtle",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "maxzoom": 6,
            "filter": [
                "==",
                "kind",
                "highway"
            ],
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "hsla(0, 0%, 85%, 0.53)",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.4
                    ],
                    [
                        "zoom"
                    ],
                    4,
                    2,
                    6,
                    1.3
                ]
            }
        },
        {
            "id": "railway",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 13,
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "LineString"
                ],
                [
                    "all",
                    [
                        "!has",
                        "service"
                    ],
                    [
                        "!has",
                        "is_tunnel"
                    ],
                    [
                        "==",
                        "kind",
                        "rail"
                    ]
                ]
            ],
            "layout": {
                "line-join": "round"
            },
            "paint": {
                "line-color": "#dddddd",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.3
                    ],
                    [
                        "zoom"
                    ],
                    9,
                    2,
                    20,
                    7
                ]
            }
        },
        {
            "id": "railway_dashline",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 13,
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "LineString"
                ],
                [
                    "all",
                    [
                        "!has",
                        "service"
                    ],
                    [
                        "!has",
                        "is_tunnel"
                    ],
                    [
                        "==",
                        "class",
                        "rail"
                    ]
                ]
            ],
            "layout": {
                "line-join": "round"
            },
            "paint": {
                "line-color": "#fafafa",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.3
                    ],
                    [
                        "zoom"
                    ],
                    9,
                    1,
                    20,
                    6
                ],
                "line-dasharray": [
                    3,
                    3
                ]
            }
        },
        {
            "id": "highway_motorway_bridge_casing",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 6,
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "LineString"
                ],
                [
                    "all",
                    [
                        "==",
                        "brunnel",
                        "bridge"
                    ],
                    [
                        "==",
                        "kind",
                        "motorway"
                    ]
                ]
            ],
            "layout": {
                "line-cap": "butt",
                "line-join": "miter"
            },
            "paint": {
                "line-color": "rgb(213, 213, 213)",
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.4
                    ],
                    [
                        "zoom"
                    ],
                    5.8,
                    0,
                    6,
                    5,
                    20,
                    45
                ],
                "line-dasharray": [
                    2,
                    0
                ]
            }
        },
        {
            "id": "highway_motorway_bridge_inner",
            "type": "line",
            "source": "basemap",
            "source-layer": "roads",
            "minzoom": 6,
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "LineString"
                ],
                [
                    "all",
                    [
                        "==",
                        "brunnel",
                        "bridge"
                    ],
                    [
                        "==",
                        "kind",
                        "motorway"
                    ]
                ]
            ],
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": [
                    "interpolate-hcl",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    5.8,
                    "hsla(0, 0%, 85%, 0.53)",
                    6,
                    "#fff"
                ],
                "line-width": [
                    "interpolate",
                    [
                        "exponential",
                        1.4
                    ],
                    [
                        "zoom"
                    ],
                    4,
                    2,
                    6,
                    1.3,
                    20,
                    30
                ]
            }
        },
        {
            "id": "highway_name_motorway",
            "type": "symbol",
            "source": "basemap",
            "source-layer": "roads",
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "LineString"
                ],
                [
                    "==",
                    "kind",
                    "motorway"
                ]
            ],
            "layout": {
                "text-size": 10,
                "symbol-spacing": 350,
                "text-font": [
                    "Open Sans Regular"
                ],
                "symbol-placement": "line",
                "text-rotation-alignment": "viewport",
                "text-pitch-alignment": "viewport",
                "text-field": "{ref}"
            },
            "paint": {
                "text-color": "rgb(117, 129, 145)",
                "text-halo-color": "hsl(0, 0%, 100%)",
                "text-translate": [
                    0,
                    2
                ],
                "text-halo-width": 1,
                "text-halo-blur": 1
            }
        },
        {
            "id": "place_other",
            "type": "symbol",
            "source": "basemap",
            "source-layer": "places",
            "maxzoom": 14,
            "filter": [
                "==",
                "kind",
                "neighbourhood"
            ],
            "layout": {
                "text-size": 10,
                "text-transform": "uppercase",
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-justify": "center",
                "text-offset": [
                    0.5,
                    0
                ],
                "text-anchor": "center",
                "text-field": "{name}"
            },
            "paint": {
                "text-color": "rgb(117, 129, 145)",
                "text-halo-color": "rgb(242,243,240)",
                "text-halo-width": 1,
                "text-halo-blur": 1
            }
        },
        {
            "id": "place_suburb",
            "type": "symbol",
            "source": "basemap",
            "source-layer": "places",
            "maxzoom": 15,
            "filter": [
                "==",
                "kind",
                "macrohood"
            ],
            "layout": {
                "text-size": 10,
                "text-transform": "uppercase",
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-justify": "center",
                "text-offset": [
                    0.5,
                    0
                ],
                "text-anchor": "center",
                "text-field": "{name}"
            },
            "paint": {
                "text-color": "rgb(117, 129, 145)",
                "text-halo-color": "rgb(242,243,240)",
                "text-halo-width": 1,
                "text-halo-blur": 1
            }
        },
        {
            "id": "place_town",
            "type": "symbol",
            "source": "basemap",
            "source-layer": "places",
            "maxzoom": 15,
            "filter": [
                "==",
                "kind",
                "locality"
            ],
            "layout": {
                "text-size": 10,
                "icon-image": [
                    "step",
                    [
                        "zoom"
                    ],
                    "circle-11",
                    8,
                    ""
                ],
                "text-transform": "uppercase",
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-justify": "left",
                "text-offset": [
                    0.5,
                    0.2
                ],
                "icon-size": 0.4,
                "text-anchor": [
                    "step",
                    [
                        "zoom"
                    ],
                    "left",
                    8,
                    "center"
                ],
                "text-field": "{name}"
            },
            "paint": {
                "text-color": "rgb(117, 129, 145)",
                "text-halo-color": "rgb(242,243,240)",
                "text-halo-width": 1,
                "text-halo-blur": 1,
                "icon-opacity": 0.7
            }
        },
        {
            "id": "place_city",
            "type": "symbol",
            "source": "basemap",
            "source-layer": "places",
            "maxzoom": 14,
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "Point"
                ],
                [
                    "all",
                    [
                        "!=",
                        "capital",
                        2
                    ],
                    [
                        "==",
                        "class",
                        "city"
                    ],
                    [
                        ">",
                        "rank",
                        3
                    ]
                ]
            ],
            "layout": {
                "text-size": 10,
                "icon-image": [
                    "step",
                    [
                        "zoom"
                    ],
                    "circle-11",
                    8,
                    ""
                ],
                "text-transform": "uppercase",
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-justify": "left",
                "text-offset": [
                    0.5,
                    0.2
                ],
                "icon-size": 0.4,
                "text-anchor": [
                    "step",
                    [
                        "zoom"
                    ],
                    "left",
                    8,
                    "center"
                ],
                "text-field": "{name}"
            },
            "paint": {
                "text-color": "rgb(117, 129, 145)",
                "text-halo-color": "rgb(242,243,240)",
                "text-halo-width": 1,
                "text-halo-blur": 1,
                "icon-opacity": 0.7
            }
        },
        {
            "id": "place_capital",
            "type": "symbol",
            "source": "basemap",
            "source-layer": "places",
            "maxzoom": 12,
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "Point"
                ],
                [
                    "all",
                    [
                        "==",
                        "capital",
                        2
                    ],
                    [
                        "==",
                        "class",
                        "city"
                    ]
                ]
            ],
            "layout": {
                "text-size": 14,
                "icon-image": [
                    "step",
                    [
                        "zoom"
                    ],
                    "star-11",
                    8,
                    ""
                ],
                "text-transform": "uppercase",
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-justify": "left",
                "text-offset": [
                    0.5,
                    0.2
                ],
                "icon-size": 1,
                "text-anchor": [
                    "step",
                    [
                        "zoom"
                    ],
                    "left",
                    8,
                    "center"
                ],
                "text-field": "{name}"
            },
            "paint": {
                "text-color": "rgb(117, 129, 145)",
                "text-halo-color": "rgb(242,243,240)",
                "text-halo-width": 1,
                "text-halo-blur": 1,
                "icon-opacity": 0.7
            }
        },
        {
            "id": "place_city_large",
            "type": "symbol",
            "source": "basemap",
            "source-layer": "places",
            "maxzoom": 12,
            "filter": [
                "all",
                [
                    "==",
                    "$type",
                    "Point"
                ],
                [
                    "all",
                    [
                        "!=",
                        "capital",
                        2
                    ],
                    [
                        "<=",
                        "rank",
                        3
                    ],
                    [
                        "==",
                        "class",
                        "city"
                    ]
                ]
            ],
            "layout": {
                "text-size": 14,
                "icon-image": [
                    "step",
                    [
                        "zoom"
                    ],
                    "circle-11",
                    8,
                    ""
                ],
                "text-transform": "uppercase",
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-justify": "left",
                "text-offset": [
                    0.5,
                    0.2
                ],
                "icon-size": 0.4,
                "text-field": "{name}"
            },
            "paint": {
                "text-color": "rgb(117, 129, 145)",
                "text-halo-color": "rgb(242,243,240)",
                "text-halo-width": 1,
                "text-halo-blur": 1,
                "icon-opacity": 0.7
            }
        },
        {
            "id": "bounding_box",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "bounding_box",
            "paint": {
                "fill-color": "#DBE8A5"
            }
        },
        {
            "id": "slope",
            "type": "raster",
            "source": "slope",
            "minzoom": 5
        },
        {
            "id": "hillshade",
            "type": "raster",
            "source": "hillshade",
            "minzoom": 5
        },
        {
            "id": "ortho",
            "type": "raster",
            "source": "ortho",
            "minzoom": 15
        },
        {
            "id": "background_site",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "boundary",
            "paint": {
                "fill-color": "#DBE8A5"
            }
        },
        {
            "id": "background_areas_catering",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "areas_catering",
            "paint": {
                "fill-color": "#d6cca0"
            }
        },
        {
            "id": "background_areas_backstage",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "areas_backstage",
            "paint": {
                "fill-pattern": "no-access"
            }
        },
        {
            "id": "background_areas_parking",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "parking",
            "paint": {
                "fill-color": "#d6cca0"
            }
        },
        {
            "id": "background_areas_camping",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "areas_camping",
            "filter": [
                "!=",
                "type",
                "crew"
            ],
            "paint": {
                "fill-color": [
                    "match",
                    [
                        "get",
                        "type"
                    ],
                    "vehicles",
                    "#c3c152",
                    "accessible",
                    "#73d579",
                    "#AFC944"
                ]
            }
        },
        {
            "id": "background_areas_camping_outline",
            "type": "line",
            "source": "site_plan",
            "source-layer": "areas_camping",
            "filter": [
                "!=",
                "type",
                "crew"
            ],
            "paint": {
                "line-color": "rgba(10, 100, 10, 0.4)",
                "line-blur": 7,
                "line-width": 3
            }
        },
        {
            "id": "background_natural_hedges_polygon",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "hedges",
            "layout": {},
            "paint": {
                "fill-color": "#528329"
            }
        },
        {
            "id": "background_water_linestring",
            "type": "line",
            "source": "site_plan",
            "source-layer": "surfacewater_line",
            "layout": {
                "line-cap": "round",
                "line-join": "round",
                "line-round-limit": 1.1
            },
            "paint": {
                "line-color": "#2EADD9",
                "line-width": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    13,
                    1,
                    15,
                    2,
                    18,
                    6
                ]
            }
        },
        {
            "id": "background_water_polygon",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "surfacewater_area",
            "layout": {},
            "paint": {
                "fill-pattern": "water"
            }
        },
        {
            "id": "background_water_polygon_shadow",
            "type": "line",
            "source": "site_plan",
            "source-layer": "surfacewater_area",
            "layout": {},
            "paint": {
                "line-color": "#1D718C",
                "line-width": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    13,
                    0,
                    18,
                    2
                ]
            }
        },
        {
            "id": "paths_grass",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "path_area",
            "filter": [
                "==",
                [
                    "get",
                    "type"
                ],
                "grass"
            ],
            "layout": {},
            "paint": {
                "fill-color": "#DBE8A5"
            }
        },
        {
            "id": "paths_tracks_case",
            "type": "line",
            "source": "site_plan",
            "source-layer": "tracks",
            "minzoom": 0,
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-width": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    12,
                    0,
                    17,
                    5
                ],
                "line-color": "rgba(132, 131, 131, 1)",
                "line-blur": 0.5
            }
        },
        {
            "id": "paths_trackway_case",
            "type": "line",
            "source": "site_plan",
            "source-layer": "path_area",
            "filter": [
                "==",
                [
                    "get",
                    "type"
                ],
                "trackway"
            ],
            "layout": {},
            "paint": {
                "line-width": 3,
                "line-color": "rgba(140, 140, 140, 1)"
            }
        },
        {
            "id": "paths_trackway",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "path_area",
            "filter": [
                "==",
                [
                    "get",
                    "type"
                ],
                "trackway"
            ],
            "layout": {},
            "paint": {
                "fill-color": "rgba(185, 185, 185, 1)"
            }
        },
        {
            "id": "paths_tracks",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "tracks",
            "minzoom": 0,
            "paint": {
                "fill-color": "rgba(177, 165, 147, 1)",
                "fill-outline-color": "rgba(98, 98, 97, 0)"
            }
        },
        {
            "id": "barrier_heras",
            "type": "line",
            "source": "site_plan",
            "source-layer": "fence",
            "filter": [
                "==",
                [
                    "get",
                    "type"
                ],
                "heras"
            ],
            "paint": {
                "line-color": "rgba(148, 63, 63, 1)"
            }
        },
        {
            "id": "barrier_ped",
            "type": "line",
            "source": "site_plan",
            "source-layer": "fence",
            "filter": [
                "==",
                [
                    "get",
                    "type"
                ],
                "ped"
            ],
            "paint": {
                "line-color": "#286f70"
            }
        },
        {
            "id": "barrier_wall",
            "type": "line",
            "source": "site_plan",
            "source-layer": "fence",
            "filter": [
                "==",
                [
                    "get",
                    "type"
                ],
                "wall"
            ],
            "paint": {
                "line-color": "#286f70"
            }
        },
        {
            "id": "structures_shadow",
            "type": "line",
            "source": "site_plan",
            "source-layer": "structure_polygon",
            "filter": [
                "==",
                [
                    "get",
                    "layer_type"
                ],
                "cover"
            ],
            "minzoom": 0,
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "rgba(0, 0, 0, 0.3)",
                "line-width": 6,
                "line-blur": 3
            }
        },
        {
            "id": "structures_polygon",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "structure_polygon",
            "filter": [
                "==",
                [
                    "get",
                    "layer_type"
                ],
                "cover"
            ],
            "minzoom": 0,
            "paint": {
                "fill-color": "#F9E200"
            }
        },
        {
            "id": "structures_outline",
            "type": "line",
            "source": "site_plan",
            "source-layer": "structure_polygon",
            "filter": [
                "==",
                [
                    "get",
                    "layer_type"
                ],
                "cover"
            ],
            "minzoom": 0,
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "rgba(90, 81, 31, 1)"
            }
        },
        {
            "id": "structures_structure_linestring",
            "type": "line",
            "source": "site_plan",
            "source-layer": "structure_linestring",
            "filter": [
                "==",
                [
                    "get",
                    "layer_type"
                ],
                "structure"
            ],
            "minzoom": 0,
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "rgba(90, 81, 31, 1)"
            }
        },
        {
            "id": "structures_structure_polygon",
            "type": "line",
            "source": "site_plan",
            "source-layer": "structure_polygon",
            "filter": [
                "==",
                [
                    "get",
                    "layer_type"
                ],
                "structure"
            ],
            "minzoom": 0,
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "rgba(90, 81, 31, 1)"
            }
        },
        {
            "id": "structures_guys",
            "type": "line",
            "source": "site_plan",
            "source-layer": "structure_linestring",
            "filter": [
                "==",
                [
                    "get",
                    "layer_type"
                ],
                "guys"
            ],
            "minzoom": 0,
            "paint": {
                "line-color": "rgb(153, 144, 93)"
            }
        },
        {
            "id": "structures_exits",
            "type": "line",
            "source": "site_plan",
            "source-layer": "structures_exits_linestring",
            "minzoom": 0,
            "paint": {
                "line-width": 3,
                "line-color": "rgb(153, 144, 93)"
            }
        },
        {
            "id": "walls",
            "type": "line",
            "source": "site_plan",
            "source-layer": "walls"
        },
        {
            "id": "fences",
            "type": "line",
            "source": "site_plan",
            "source-layer": "fences_permanent",
            "paint": {
                "line-color": "rgba(134, 134, 101, 1)"
            }
        },
        {
            "id": "buildings",
            "type": "fill-extrusion",
            "source": "basemap",
            "source-layer": "buildings",
            "layout": {},
            "paint": {
                "fill-extrusion-color": "rgba(189, 170, 85, 1)",
                "fill-extrusion-height": 4
            }
        },
        {
            "id": "services_comms_ducts_case",
            "type": "line",
            "source": "site_plan",
            "source-layer": "telecom_ducts",
            "layout": {},
            "paint": {
                "line-color": "rgba(150, 150, 150, 1)",
                "line-width": 3
            }
        },
        {
            "id": "services_comms_ducts",
            "type": "line",
            "source": "site_plan",
            "source-layer": "telecom_ducts",
            "layout": {},
            "paint": {
                "line-color": "rgba(187, 0, 218, 1)",
                "line-width": 2
            }
        },
        {
            "id": "services_water_lines",
            "type": "line",
            "source": "site_plan",
            "source-layer": "water_lines",
            "layout": {},
            "paint": {
                "line-color": "#00C5FF",
                "line-width": 2
            }
        },
        {
            "id": "services_water_points",
            "type": "circle",
            "source": "site_plan",
            "source-layer": "water_points",
            "paint": {
                "circle-color": "rgba(0, 197, 255, 1)",
                "circle-stroke-color": "rgba(22, 0, 195, 1)",
                "circle-stroke-width": 2,
                "circle-radius": 3
            }
        },
        {
            "id": "boundary",
            "type": "line",
            "source": "site_plan",
            "source-layer": "boundary",
            "layout": {
                "line-cap": "round",
                "line-join": "round"
            },
            "paint": {
                "line-color": "rgba(226, 11, 11, 1)",
                "line-dasharray": [
                    10,
                    3
                ],
                "line-width": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    10,
                    1,
                    17,
                    2
                ]
            }
        },
        {
            "id": "lighting_festoon",
            "type": "line",
            "source": "site_plan",
            "source-layer": "lighting_festoon",
            "layout": {},
            "paint": {
                "line-color": "rgba(200, 184, 42, 1)",
                "line-width": 2
            }
        },
        {
            "id": "lighting_festoon_labels",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "lighting_festoon",
            "minzoom": 19,
            "maxzoom": 24,
            "layout": {
                "symbol-placement": "line",
                "text-field": "Festoon ({length}m)",
                "text-size": 12,
                "text-optional": false,
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-keep-upright": true,
                "text-allow-overlap": false,
                "text-offset": [
                    0,
                    -1
                ]
            },
            "paint": {
                "text-halo-color": "rgba(241, 241, 241, 0.8)",
                "text-halo-width": 3,
                "text-color": "rgba(200, 184, 42, 1)"
            }
        },
        {
            "id": "background_trees",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "trees",
            "minzoom": 13,
            "layout": {
                "icon-image": "tree",
                "icon-size": [
                    "interpolate",
                    [
                        "exponential",
                        1.5
                    ],
                    [
                        "zoom"
                    ],
                    14,
                    0.01,
                    22,
                    [
                        "max",
                        [
                            "*",
                            [
                                "to-number",
                                [
                                    "coalesce",
                                    [
                                        "get",
                                        "crown_diameter"
                                    ],
                                    5
                                ]
                            ],
                            0.1
                        ],
                        0.3
                    ]
                ],
                "icon-allow-overlap": true,
                "icon-rotate": [
                    "%",
                    [
                        "*",
                        [
                            "get",
                            "gid"
                        ],
                        200
                    ],
                    360
                ]
            },
            "paint": {}
        },
        {
            "id": "water_points_event",
            "type": "symbol",
            "source": "site_plan",
            "minzoom": 16,
            "source-layer": "water_points_event",
            "filter": [
                "==",
                "public",
                true
            ],
            "layout": {
                "icon-image": "water-point",
                "icon-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    16,
                    0.3,
                    20,
                    1
                ],
                "icon-allow-overlap": true
            }
        },
        {
            "id": "toilets",
            "type": "symbol",
            "source": "site_plan",
            "minzoom": 16,
            "maxzoom": 20,
            "source-layer": "areas_event_centroid",
            "filter": [
                "==",
                "type",
                "toilets"
            ],
            "layout": {
                "icon-image": "toilet",
                "icon-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    16,
                    0.4,
                    20,
                    1.2
                ],
                "icon-allow-overlap": true
            }
        },
        {
            "id": "network_cable_case",
            "type": "line",
            "source": "site_plan",
            "source-layer": "network_cable",
            "minzoom": 17,
            "layout": {},
            "paint": {
                "line-color": "#999",
                "line-width": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "get",
                        "cores"
                    ],
                    1,
                    4,
                    24,
                    10
                ]
            }
        },
        {
            "id": "network_cable",
            "type": "line",
            "source": "site_plan",
            "source-layer": "network_cable",
            "minzoom": 17,
            "layout": {},
            "paint": {
                "line-color": [
                    "match",
                    [
                        "get",
                        "type"
                    ],
                    "cat5e",
                    "#01b6a3",
                    "smf",
                    "#f4f102",
                    "#333"
                ],
                "line-width": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "get",
                        "cores"
                    ],
                    1,
                    2,
                    24,
                    4
                ]
            }
        },
        {
            "id": "network_switch",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "network_location",
            "filter": [
                "==",
                "type",
                "colo"
            ],
            "minzoom": 16,
            "layout": {
                "icon-image": "network-switch",
                "icon-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    17,
                    0.2,
                    22,
                    1
                ],
                "icon-allow-overlap": true,
                "icon-anchor": "center",
                "text-variable-anchor": [
                    "top",
                    "bottom"
                ],
                "text-field": "{name}",
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    18,
                    9,
                    20,
                    14
                ],
                "text-optional": true,
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-radial-offset": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    18,
                    1,
                    20,
                    2
                ]
            },
            "paint": {
                "text-halo-color": "rgba(241, 241, 241, 0.8)",
                "text-halo-width": 3,
                "text-opacity": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    17.9,
                    0,
                    18,
                    1
                ]
            }
        },
        {
            "id": "power_cable",
            "type": "line",
            "source": "site_plan",
            "source-layer": "power_cable",
            "minzoom": 16,
            "layout": {},
            "paint": {
                "line-color": [
                    "match",
                    [
                        "get",
                        "phases"
                    ],
                    1,
                    "#0500A5",
                    3,
                    "#A50B07",
                    "#333"
                ],
                "line-width": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "get",
                        "amps"
                    ],
                    16,
                    1,
                    400,
                    6
                ]
            }
        },
        {
            "id": "power_distro",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "power_distro",
            "minzoom": 16,
            "layout": {
                "icon-image": "power-distro",
                "icon-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    16,
                    0.1,
                    23,
                    1
                ],
                "icon-allow-overlap": true,
                "icon-anchor": "center",
                "text-optional": true,
                "text-field": "{name}",
                "text-size": 11,
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-variable-anchor": [
                    "top",
                    "bottom",
                    "left",
                    "right"
                ],
                "text-radial-offset": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    18,
                    1.2,
                    20,
                    2
                ],
                "text-max-width": 6
            },
            "paint": {
                "text-halo-color": "rgba(241, 241, 241, 0.8)",
                "text-halo-width": 3,
                "text-opacity": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    17.9,
                    0,
                    18,
                    1
                ]
            }
        },
        {
            "id": "power_generator",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "power_generator",
            "minzoom": 16,
            "layout": {
                "icon-image": "power-generator",
                "icon-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    16,
                    0.3,
                    24,
                    1
                ],
                "icon-allow-overlap": true,
                "icon-anchor": "center",
                "text-optional": true,
                "text-field": "{name}",
                "text-size": 10,
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-variable-anchor": [
                    "top",
                    "bottom",
                    "left",
                    "right"
                ],
                "text-radial-offset": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    18,
                    1.2,
                    20,
                    2
                ],
                "text-max-width": 6
            },
            "paint": {
                "text-halo-color": "rgba(241, 241, 241, 0.8)",
                "text-halo-width": 3,
                "text-opacity": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    17.9,
                    0,
                    18,
                    1
                ]
            }
        },
        {
            "id": "power_distro_type",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "power_distro",
            "minzoom": 19,
            "layout": {
                "text-field": "[{type}]",
                "text-size": 9,
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-variable-anchor": [
                    "top",
                    "bottom",
                    "left",
                    "right"
                ],
                "text-radial-offset": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    18,
                    1.2,
                    20,
                    2
                ],
                "text-max-width": 6
            },
            "paint": {
                "text-halo-color": "rgba(241, 241, 241, 0.8)",
                "text-halo-width": 3,
                "text-opacity": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    17.9,
                    0,
                    18,
                    1
                ]
            }
        },
        {
            "id": "services_comms_manholes",
            "type": "circle",
            "source": "site_plan",
            "source-layer": "telecom_manholes",
            "minzoom": 15,
            "paint": {
                "circle-color": "rgba(187, 0, 218, 1)",
                "circle-stroke-width": 1
            }
        },
        {
            "id": "services_comms_manholes_label",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "telecom_manholes",
            "minzoom": 15,
            "layout": {
                "text-field": "{ref}",
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-size": 14,
                "text-offset": [
                    -1,
                    0
                ]
            },
            "paint": {
                "text-halo-color": "rgba(255, 255, 255, 1)",
                "text-halo-width": 2
            }
        },
        {
            "id": "power_cable_label",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "power_cable",
            "minzoom": 20,
            "layout": {
                "text-field": "{amps}/{phases}",
                "text-size": 10,
                "text-font": [
                    "Open Sans Regular"
                ],
                "symbol-placement": "line",
                "symbol-spacing": 500,
                "text-offset": [
                    0,
                    1
                ]
            },
            "paint": {
                "text-halo-color": "rgba(241, 241, 241, 0.8)",
                "text-halo-width": 3
            }
        },
        {
            "id": "network_cable_label",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "network_cable",
            "minzoom": 20,
            "layout": {
                "text-field": "{type} ({cores})",
                "text-size": 10,
                "text-font": [
                    "Open Sans Regular"
                ],
                "symbol-placement": "line",
                "symbol-spacing": 500,
                "text-offset": [
                    0,
                    1
                ]
            },
            "paint": {
                "text-halo-color": "rgba(241, 241, 241, 0.8)",
                "text-halo-width": 3
            }
        },
        {
            "id": "gridsquares",
            "type": "fill",
            "source": "site_plan",
            "source-layer": "grid",
            "paint": {
                "fill-opacity": 0
            }
        },
        {
            "id": "grid_lines",
            "type": "line",
            "source": "site_plan",
            "source-layer": "grid",
            "paint": {
                "line-color": "#666"
            }
        },
        {
            "id": "villages_symbol",
            "type": "circle",
            "source": "villages",
            "source-layer": "",
            "minzoom": 16,
            "paint": {
                "circle-color": "rgb(246, 163, 24)",
                "circle-radius": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    16,
                    6,
                    24,
                    26
                ],
                "circle-blur": 0.5,
                "circle-stroke-width": 0.5
            }
        },
        {
            "id": "villages_text",
            "type": "symbol",
            "source": "villages",
            "source-layer": "",
            "minzoom": 17,
            "maxzoom": 24,
            "layout": {
                "text-field": "{name}",
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-radial-offset": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    17,
                    1.1,
                    21,
                    2
                ],
                "text-variable-anchor": [
                    "top",
                    "bottom"
                ],
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    17,
                    8,
                    20,
                    16
                ]
            },
            "paint": {
                "text-halo-color": "rgba(250, 250, 250, 1)",
                "text-halo-width": 3,
                "text-halo-blur": 1
            }
        },
        {
            "id": "labels_camping",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "areas_camping_centroid",
            "minzoom": 16,
            "maxzoom": 19,
            "filter": [
                "!=",
                "type",
                "crew"
            ],
            "layout": {
                "text-field": [
                    "concat",
                    [
                        "match",
                        [
                            "get",
                            "type"
                        ],
                        "camping",
                        "Camping ",
                        "accessible",
                        "Accessible Camping",
                        "vehicles",
                        "Live-in Vehicles ",
                        ""
                    ],
                    [
                        "get",
                        "name"
                    ]
                ],
                "text-font": [
                    "Raleway SemiBold"
                ],
                "text-justify": "center",
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    16,
                    10,
                    20,
                    25
                ],
                "text-padding": 2,
                "text-max-width": 8
            },
            "paint": {
                "text-halo-width": 3,
                "text-halo-blur": 1,
                "text-halo-color": "rgba(241, 241, 241, 1)"
            }
        },
        {
            "id": "network_datenklo",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "network_location",
            "filter": [
                "==",
                "type",
                "dk"
            ],
            "minzoom": 17,
            "layout": {
                "icon-image": "datenklo",
                "icon-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    17,
                    0.3,
                    22,
                    1.8
                ],
                "icon-allow-overlap": true,
                "icon-anchor": "bottom",
                "text-anchor": "top",
                "text-field": "{name}",
                "text-size": 12,
                "text-optional": false,
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-offset": [
                    0,
                    0.3
                ]
            },
            "paint": {
                "text-halo-color": "rgba(241, 241, 241, 0.8)",
                "text-halo-width": 3,
                "text-opacity": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    17.9,
                    0,
                    18,
                    1
                ]
            }
        },
        {
            "id": "labels_streets",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "street_names",
            "minzoom": 16,
            "layout": {
                "text-field": "{name}",
                "text-font": [
                    "Raleway SemiBold"
                ],
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    16,
                    8,
                    17,
                    9,
                    23,
                    50
                ],
                "text-max-angle": 10,
                "symbol-placement": "line",
                "symbol-spacing": 500
            },
            "paint": {
                "text-halo-color": "rgba(120, 120, 120, 1)",
                "text-halo-width": 3,
                "text-halo-blur": 1,
                "text-color": "rgba(250, 250, 250, 1)"
            }
        },
        {
            "id": "labels_parking",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "parking_centroid",
            "minzoom": 16,
            "layout": {
                "text-field": "Parking:\n{name}",
                "text-font": [
                    "Raleway SemiBold"
                ],
                "text-justify": "center",
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    16,
                    10,
                    20,
                    25
                ],
                "text-padding": 2,
                "text-max-width": 6
            },
            "paint": {
                "text-halo-width": 3,
                "text-halo-blur": 1,
                "text-halo-color": "rgba(241, 241, 241, 1)"
            }
        },
        {
            "id": "labels_gate",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "gates",
            "minzoom": 16,
            "maxzoom": 24,
            "layout": {
                "text-field": "Gate {name}",
                "text-size": 12,
                "text-optional": false,
                "text-font": [
                    "Open Sans Regular"
                ],
                "text-keep-upright": true,
                "text-ignore-placement": false,
                "text-allow-overlap": false
            },
            "paint": {
                "text-halo-color": "rgba(241, 241, 241, 1)",
                "text-halo-width": 3,
                "text-halo-blur": 2,
                "text-color": "rgba(0, 0, 0, 1)"
            }
        },
        {
            "id": "labels_structures_5",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "structure_centroid",
            "filter": [
                "==",
                [
                    "get",
                    "importance"
                ],
                5
            ],
            "minzoom": 16,
            "layout": {
                "text-field": "{name}",
                "text-font": [
                    "Raleway SemiBold"
                ],
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    16,
                    9,
                    23,
                    50
                ],
                "text-max-angle": 10,
                "text-max-width": 6
            },
            "paint": {
                "text-halo-color": "rgba(120, 120, 120, 1)",
                "text-halo-width": 3,
                "text-halo-blur": 1,
                "text-color": "rgba(250, 250, 250, 1)"
            }
        },
        {
            "id": "labels_structures_4",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "structure_centroid",
            "filter": [
                "==",
                [
                    "get",
                    "importance"
                ],
                4
            ],
            "minzoom": 17,
            "layout": {
                "text-field": "{name}",
                "text-font": [
                    "Raleway SemiBold"
                ],
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    17,
                    8,
                    23,
                    40
                ],
                "text-max-angle": 10,
                "text-max-width": 6
            },
            "paint": {
                "text-halo-color": "rgba(120, 120, 120, 1)",
                "text-halo-width": 3,
                "text-halo-blur": 1,
                "text-color": "rgba(250, 250, 250, 1)"
            }
        },
        {
            "id": "labels_structures_3",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "structure_centroid",
            "filter": [
                "==",
                [
                    "get",
                    "importance"
                ],
                3
            ],
            "minzoom": 18,
            "layout": {
                "text-field": "{name}",
                "text-font": [
                    "Raleway SemiBold"
                ],
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    18,
                    8,
                    23,
                    30
                ],
                "text-max-angle": 10,
                "text-max-width": 6
            },
            "paint": {
                "text-halo-color": "rgba(120, 120, 120, 1)",
                "text-halo-width": 3,
                "text-halo-blur": 1,
                "text-color": "rgba(250, 250, 250, 1)"
            }
        },
        {
            "id": "labels_structures_2",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "structure_centroid",
            "filter": [
                "==",
                [
                    "get",
                    "importance"
                ],
                2
            ],
            "minzoom": 19,
            "layout": {
                "text-field": "{name}",
                "text-font": [
                    "Raleway SemiBold"
                ],
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    19,
                    8,
                    23,
                    25
                ],
                "text-max-angle": 10,
                "text-max-width": 6
            },
            "paint": {
                "text-halo-color": "rgba(120, 120, 120, 1)",
                "text-halo-width": 2,
                "text-halo-blur": 1,
                "text-color": "rgba(250, 250, 250, 1)"
            }
        },
        {
            "id": "labels_areas",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "areas_event_centroid",
            "filter": [
                "!=",
                "type",
                "toilets"
            ],
            "minzoom": 16,
            "maxzoom": 19,
            "layout": {
                "text-field": "{name}",
                "text-font": [
                    "Raleway SemiBold"
                ],
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    15,
                    9,
                    23,
                    40
                ],
                "text-max-angle": 10
            },
            "paint": {
                "text-halo-color": "rgba(120, 120, 120, 1)",
                "text-halo-width": 3,
                "text-halo-blur": 1,
                "text-color": "rgba(250, 250, 250, 1)"
            }
        },
        {
            "id": "labels_catering",
            "type": "symbol",
            "source": "site_plan",
            "source-layer": "areas_catering_centroid",
            "minzoom": 16,
            "layout": {
                "text-field": "Catering",
                "text-font": [
                    "Raleway SemiBold"
                ],
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    15,
                    7,
                    23,
                    30
                ],
                "text-max-angle": 10
            },
            "paint": {
                "text-halo-color": "rgba(120, 120, 120, 1)",
                "text-halo-width": 2,
                "text-halo-blur": 1,
                "text-color": "rgba(250, 250, 250, 1)"
            }
        },
        {
            "id": "grist-markers",
            "type": "symbol",
            "source": "grist_markers",
            "layout": {
                "icon-image": "marker",
                "icon-allow-overlap": true,
                "icon-anchor": "bottom",
                "text-offset": [
                    0,
                    0.5
                ],
                "text-field": "{name}",
                "text-font": [
                    "Raleway SemiBold"
                ],
                "text-size": [
                    "interpolate",
                    [
                        "linear"
                    ],
                    [
                        "zoom"
                    ],
                    15,
                    9,
                    23,
                    40
                ],
                "text-max-angle": 10
            },
            "paint": {
                "text-halo-color": "rgba(100, 100, 100, 1)",
                "text-halo-width": 3,
                "text-halo-blur": 1,
                "text-color": "rgba(250, 250, 250, 1)",
                "icon-opacity": [
                    "case",
                    [
                        "boolean",
                        [
                            "feature-state",
                            "grist-selected"
                        ],
                        false
                    ],
                    1,
                    0.3
                ]
            }
        }
    ]
};

// Required attribution for the data sources baked into emfMapStyle above (ODbL requires the
// OpenStreetMap credit to be shown; leaflet-maplibre-gl doesn't surface style "attribution"
// fields itself, so this is passed explicitly to L.maplibreGL({ attribution: ... }).
let emfMapAttribution = '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors | Site plan &copy; <a href="https://www.emfcamp.org">EMF Camp</a> | Elevation data &copy; Environment Agency 2022';
