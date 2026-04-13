
var AliciModule = (function () {

    var _grid = null;
    return {
        init: function () {
            if (_grid) return;
            var gridElement = $("#muhatapGrid");
            if (gridElement.length > 0) {
                _grid = gridElement.kendoGrid({
                    columns: [
                        { field: "MuhatapAdi", title: "Alıcı Adı" },
                        { field: "IsBilgi", title: "Türü", template: "#= IsBilgi ? '<span class=\"badge bg-info\">Bilgi</span>' : '<span class=\"badge bg-primary\">Gereği</span>' #" },
                        {
                            command: [{
                                className: "btn-grid-action btn btn-outline-danger ms-2",
                                text: "Sil",
                                click: function (e) {
                                    e.preventDefault();
                                    var tr = $(e.currentTarget).closest("tr");
                                    var dataItem = _grid.dataItem(tr);
                                    if (dataItem) _grid.dataSource.remove(dataItem);
                                }
                            }], title: "İşlem", width: "100px"
                        }
                    ],
                    dataSource: { data: [], schema: { model: { id: "MuhatapId" } } },
                    editable: false
                }).data("kendoGrid");
            }
        },
        aliciEkle: function (isBilgi) {
            var multiSelect = $("#muhatapSecici").data("kendoMultiSelect");
            var grid = _grid || $("#muhatapGrid").data("kendoGrid");
            var selectedDataItems = multiSelect.dataItems();

            if (selectedDataItems.length === 0) {
                showNotification("Lütfen önce kurum seçiniz!", "warning");
                return;
            }

            selectedDataItems.forEach(function (item) {
                // grid.dataSource'un varlığını burada da kontrol ediyoruz
                if (grid.dataSource) {
                    var exists = grid.dataSource.data().some(x => x.MuhatapId === item.id);
                    if (!exists) {
                        grid.dataSource.add({
                            MuhatapId: item.id,
                            MuhatapAdi: item.adi,
                            IsBilgi: isBilgi
                        });
                    }
                }
            });
            multiSelect.value([]);
        },

        setData: function (muhataplar) {
            var grid = _grid || $("#muhatapGrid").data("kendoGrid");
            if (grid && Array.isArray(muhataplar)) {
                // Mevcut veriyi temizleyip yenisini ekliyoruz
                var mappedData = muhataplar.map(function (item) {
                    return {
                        MuhatapId: item.muhatapId || item.MuhatapId,
                        MuhatapAdi: item.muhatapAdi || item.MuhatapAdi || item.adi,
                        IsBilgi: item.isBilgi
                    };
                });
                grid.dataSource.data(mappedData);
            }
        },

        getData: function () {
            var grid = _grid || $("#muhatapGrid").data("kendoGrid");
            return grid ? grid.dataSource.data().toJSON() : [];
        }
    };
})();
