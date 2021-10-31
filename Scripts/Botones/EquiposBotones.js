$("#ButtonNuevo").click(function (eve) {
    $("#modal-content").load("/Equipos/Create");
});

$("#EditarButton").click(function (eve) {
    $("#modal-content").load();
});

$("#EliminarButton").click(function (eve) {
    $("#modal-content").load();
});

//document.addEventListener("DOMContentLoaded", Function(){
//    document.ge
//});