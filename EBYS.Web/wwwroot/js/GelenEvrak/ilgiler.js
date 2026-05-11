var IlgilerModule = (function () {
    var _grid = null;

    return {
        init: function () {
            _grid = $("#ilgilerGrid").kendoGrid({
                columns: [
                    { field: "IlgiMetni", title: "İlgi Metni" },
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
                        }],
                        title: "İşlem",
                        width: "100px"
                    }
                ],
                dataSource: {
                    data: [],
                    schema: {
                        model: {
                            fields: {
                                IlgiMetni: { type: "string" }
                            }
                        }
                    }
                },
                editable: false,

            }).data("kendoGrid");
        },

        ekle: function () {
            var input = $("#IlgiMetni").val();

            if (!input || input.trim() === "") {
                // notification fonksiyonun varsa kullanabilirsin
                alert("Lütfen bir ilgi metni giriniz.");
                return;
            }

            // Grid'e yeni satır ekle

            _grid.dataSource.add({
                IlgiMetni: input.trim()
            });
            // Metin kutusunu temizle ve odağı tekrar oraya ver
            $("#IlgiMetni").val("").focus();
        },
        setData: function (ilgiler) {
            if (_grid && Array.isArray(ilgiler)) {
                var mappedData = ilgiler.map(function (item) {
                    return {
                        // API'den gelen 'ilgiMetni' (küçük i) 
                        // Grid field'ı olan 'IlgiMetni' (Büyük I) ile eşleşmeli
                        IlgiMetni: item.ilgiMetni || item.IlgiMetni
                    };
                });
                _grid.dataSource.data(mappedData);
            }
        },
        // Servise gönderilecek veriyi toplamak için
        getData: function () {
            var data = _grid.dataSource.data();
            return data.map(function (item) {
                return {
                    IlgiMetni: item.IlgiMetni
                };
            });
        }
    };
})();

// Sayfa hazır olduğunda grid'i başlat
$(document).ready(function () {
    IlgilerModule.init();
});