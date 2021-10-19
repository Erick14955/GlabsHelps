$("#ButtonNuevo").click(function (eve) {
    $("#modal-content").load("/Equipos/Create");
});

$("#EditarButton").click(function (eve) {
    $("#modal-content").load("/Equipos/Edit/" + $(this).data("id"));
});

$("#DetallesButton").click(function (eve) {
    $("#modal-content").load("/Equipos/Details/" + $(this).data("id"));
});

$("#EliminarButton").click(function (eve) {
    $("#modal-content").load("/Equipos/Delete/" + $(this).data("id"));
});