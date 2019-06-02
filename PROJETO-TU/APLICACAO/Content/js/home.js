//AJAX RESULTS ERRO
function OnFailure(response) {
    $.ajax({
        statusCode: {
            404: function () {
                swal({
                    title: "Erro!",
                    text: "Página não encontrada\n" + response,
                    type: "error",
                    showCancelButton: false,
                    closeOnConfirm: false,
                    confirmButtonText: "Ok",
                    confirmButtonColor: "#1ab394"
                }, function () {
                    return false;
                });
            },
            500: function () {
                swal({
                    title: "Erro!",
                    text: "Erro de servidor\n" + response,
                    type: "error",
                    showCancelButton: false,
                    closeOnConfirm: false,
                    confirmButtonText: "Ok",
                    confirmButtonColor: "#1ab394"
                }, function () {
                    return false;
                });
            }
        }
    });
}

//AJAX RESULTS SUCCESS
function OnSuccess(response) {
    if (response.erro == true) {
        swal({
            title: "Erro!",
            text: response.msg,
            type: "error",
            showCancelButton: false,
            closeOnConfirm: true,
            confirmButtonText: "Ok",
            confirmButtonColor: "#1ab394"
        }, function () {
            return false;
        });
    }
    else {
        swal({
            title: "Pronto!",
            text: response,
            type: "success",
            showCancelButton: false,
            closeOnConfirm: false,
            confirmButtonText: "Ok",
            confirmButtonColor: "#1ab394"
        }, function () {
            location.reload();
        });
    }
}

//TOAST ALERTS
var toastr;
toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut",
}

//ABRE MODAL DE OPCOES
function ModalOpcoes(response) {
    $("#modalOpcoes").html("");
    $("#modalOpcoes").append(response);
    $("#modalOpc").modal("show");
}