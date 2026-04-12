var ImzaRotaListModule = (function () {
    var _grid = null;
    var _apiBaseUrl = "https://localhost:7060/api/ImzaRota/";

    var _ajaxCall = function (url, type, data) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: type,
            contentType: "application/json",
            data: data ? JSON.stringify(data) : null,
            error: function (err) {
                var msg = err && err.responseText ? err.responseText : "Hata oluştu.";
                showNotification(msg, "error");
            }
        });
    };

    return {
        init: function () {
            this.initGrid();
            this.loadData();
        },

        initGrid: function () {
            _grid = $("#rotaListGrid").kendoGrid({
                noRecords: { template: "<div class='p-5 text-center text-muted'>Kayıt bulunamadı.</div>" },
                columns: [
                    {
                        field: "RotaAdi",
                        title: "Rota Tanımı",
                        template: "<span class='fw-bold text-dark'>#: RotaAdi #</span>"
                    },
                    {
                        title: "İşlemler",
                        width: "250px",
                        headerAttributes: { style: "text-align: center" },
                        attributes: { style: "text-align: center" },
                        command: [
                            {
                                name: "customEdit",
                                text: " Düzenle",
                                className: "btn-grid-action btn btn-outline-primary",
                                iconClass: "k-icon k-i-edit",
                                click: function (e) {
                                    e.preventDefault();
                                    var grid = $("#rotaListGrid").data("kendoGrid");
                                    var tr = $(e.currentTarget).closest("tr");
                                    var dataItem = grid.dataItem(tr);
                                    if (dataItem && dataItem.Id) {
                                        window.location.href = 'ImzaRotaAdd?id=' + dataItem.Id;
                                    } else {
                                        showNotification("Seçili satırın ID bilgisi alınamadı.", "error");
                                    }
                                }
                            },
                            {
                                name: "customDelete",
                                text: " Sil",
                                className: "btn-grid-action btn btn-outline-danger ms-2",
                                iconClass: "k-icon k-i-delete",
                                click: function (e) {
                                    e.preventDefault();
                                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                                    if (confirm(dataItem.RotaAdi + " silinecek. Emin misiniz?")) {
                                        _ajaxCall("ImzaRotaSil/" + dataItem.Id, "DELETE").done(function () {
                                            _grid.dataSource.remove(dataItem);
                                            showNotification("Başarıyla silindi.", "success");
                                        });
                                    }
                                }
                            }
                        ]
                    }
                ],
                sortable: true,
                dataSource: { data: [] }
            }).data("kendoGrid");
        },

        loadData: function () {
            _ajaxCall('ImzaRotaListesi', 'GET').done(function (res) {
                var list = Array.isArray(res) ? res : (res.data || res.rota || []);
                _grid.dataSource.data(list.map(x => ({ Id: x.id || x.Id, RotaAdi: x.rotaAdi || x.RotaAdi })));
            });
        }
    };
})();

$(document).ready(function () {
    ImzaRotaListModule.init();
});