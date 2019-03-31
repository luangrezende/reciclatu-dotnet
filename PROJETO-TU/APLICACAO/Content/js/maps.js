//API GOOGLE MAPS
var map;
var directionsDisplay;
var directionsService;
var latlng;
var geocoder;
var marker;

//INICIALIZA GOOGLE MAPS
function initializeGMaps() {
    latlng = new google.maps.LatLng(-11.7484534, -61.064371);
    directionsDisplay = new google.maps.DirectionsRenderer();
    directionsService = new google.maps.DirectionsService();
    geocoder = new google.maps.Geocoder();

    var options = {
        zoom: 4,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("mapa"), options);
    directionsDisplay.setMap(map);
    directionsDisplay.setOptions({ suppressMarkers: true });

    marker = new google.maps.Marker({
        map: map,
        draggable: false,
    });
};

//CALCULA ROTA
function calculaRota() {
    var iconeOrigem = "https://img.icons8.com/color/48/000000/user-location.png";
    var iconeDestino = "https://img.icons8.com/color/48/000000/order-shipped.png";
    var latitude;
    var longitude;

    var request = {
        origin: origem,
        destination: destino,
        travelMode: google.maps.TravelMode.DRIVING
    };

    directionsDisplay.setOptions({
        options: {
            optimizeWaypoints: true,
        }
    });

    directionsService.route(request, function (result, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            geocoder.geocode({ 'address': origem, 'region': 'BR' }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        latitude = results[0].geometry.location.lat();
                        longitude = results[0].geometry.location.lng();

                        marker = new google.maps.Marker({
                            position: new google.maps.LatLng(latitude, longitude),
                            title: "Você está aqui",
                            map: map,
                            icon: iconeOrigem
                        });
                    }
                }
            });
            geocoder.geocode({ 'address': destino, 'region': 'BR' }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        latitude = results[0].geometry.location.lat();
                        longitude = results[0].geometry.location.lng();

                        marker = new google.maps.Marker({
                            position: new google.maps.LatLng(latitude, longitude),
                            title: "Retirar: " + results[0].formatted_address,
                            map: map,
                            icon: iconeDestino
                        });
                    }
                }
            });
            alert("Ae cuzao, a distancia é de " + result.routes[0].legs[0].distance.text + "\ne vc levará " + result.routes[0].legs[0].duration.text);
            console.log(result.routes[0].legs[0].duration);
            directionsDisplay.setDirections(result);
        }
    });
}

//MOSTRA ENDERECO
function mostraEndereco() {
    geocoder.geocode({ 'address': origem, 'region': 'BR' }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                latitude = results[0].geometry.location.lat();
                longitude = results[0].geometry.location.lng();

                console.log(results[0].formatted_address);

                marker = new google.maps.Marker({
                    map: map,
                    title: results[0].formatted_address,
                    draggable: false,
                });

                var location = new google.maps.LatLng(latitude, longitude);
                marker.setPosition(location);
                map.setCenter(location);
                map.setZoom(16);
            }
        }
    });
}

//EXECUTA ON START
$(document).ready(function () {
    initializeGMaps();
    setTimeout(function () {
        if (tipoRota == 1) {
            mostraEndereco();
        }
        else {
            calculaRota();
        }
    }, 250);
});