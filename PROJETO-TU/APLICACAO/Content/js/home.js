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
            text: "Erro na requisição: " + response.msg,
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

//ABRE MODAL DE OPCOES
function ModalOpcoes(response) {
    $("#modalOpcoes").html("");
    $("#modalOpcoes").append(response);
    $("#modalOpc").modal("show");
}