//API GOOGLE MAPS
var map;
var directionsDisplay;
var directionsService;
var latlng;
var geocoder;
var marker;

//INICIALIZA GOOGLE MAPS
function initializeGMaps() {
    latlng = new google.maps.LatLng(-18.8800397, -47.05878999999999);
    directionsDisplay = new google.maps.DirectionsRenderer();
    directionsService = new google.maps.DirectionsService();
    geocoder = new google.maps.Geocoder();

    var options = {
        zoom: 12,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("mapa"), options);

    marker = new google.maps.Marker({
        map: map,
        draggable: true,
    });
};

//CALCULA ROTA
function calculaRota() {
    var request = {
        origin: origem,
        destination: destino,
        travelMode: google.maps.TravelMode.DRIVING
    };

    directionsService.route(request, function (result, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(result);
        }
    });
}

//MOSTRA ENDERECO
function mostraEndereco() {
    geocoder.geocode({ 'address': origem, 'region': 'BR' }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                var latitude = results[0].geometry.location.lat();
                var longitude = results[0].geometry.location.lng();

                console.log(results[0].formatted_address);

                var location = new google.maps.LatLng(latitude, longitude);
                marker.setPosition(location);
                map.setCenter(location);
                map.setZoom(15);
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