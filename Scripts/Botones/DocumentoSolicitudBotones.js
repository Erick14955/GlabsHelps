$("#ButtonNuevo").click(function (eve) {
    var url = $(this).data('url');
    $("#modal-content").load(url);
});

$("#EditarButton").click(function (eve) {
    $("#modal-content").load();
});

$("#EliminarButton").click(function (eve) {
    $("#modal-content").load();
});

$("#DetalleButton").click(function (eve) {
    $("#modal-content").load();
});