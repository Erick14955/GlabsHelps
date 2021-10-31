$("#ButtonNuevo").click(function (eve) {
    var url = $(this).data('url');
    $("#modal-content").load(url);
});

$("#EditarButton").click(function (eve) {
    $("#modal-content").load();
});

//function Open() {
//    var url = $(this).data('url');
//    var decodeUrl = decodeURIComponent(url);
//    $("#modal-content").load(decodeUrl);
//}

$("#EliminarButton").click(function (eve) {
    $("#modal-content").load();
});

//$("#PopupEdit").validate({
//    rules:

//})