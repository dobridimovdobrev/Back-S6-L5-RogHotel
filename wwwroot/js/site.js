//hamburger menu

$("#sidebarToggle").click(function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});

// DataTables - Configurazione Globale Italiana
$.extend(true, $.fn.dataTable.defaults, {
    language: {
        "decimal": "",
        "emptyTable": "Nessun dato disponibile",
        "info": "Righe da _START_ a _END_ di _TOTAL_",
        "infoEmpty": "0 righe",
        "infoFiltered": "(filtrate da _MAX_ righe totali)",
        "lengthMenu": "Mostra _MENU_ righe",
        "loadingRecords": "Caricamento...",
        "processing": "Elaborazione...",
        "search": "Cerca:",
        "zeroRecords": "Nessun risultato trovato",
        "paginate": {
            "first": "Prima",
            "last": "Ultima",
            "next": "Successiva",
            "previous": "Precedente"
        }
    },
    responsive: true,
    pageLength: 10
});
