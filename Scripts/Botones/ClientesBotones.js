$("#ButtonNuevo").click(function (eve) {
    $("#modal-content").load("/Clientes/Create");
});

$("#EditarButton").click(function (eve) {
    $("#modal-content").load("/Clientes/Edit/" + $(this).data("id"));
});

$("#DetallesButton").click(function (eve) {
    $("#modal-content").load("/Clientes/Details/" + $(this).data("id"));
});

$("#EliminarButton").click(function (eve) {
    $("#modal-content").load("/Clientes/Delete/" + $(this).data("id"));
});