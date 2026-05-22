var GelenEklerModule = (function () {
    var _grid = null;
    var _tempFile = null;

    return {
        init: function () {
 
            _grid = $("#eklerGrid").kendoGrid({
                columns: [
                    { field: "Ad", title: "Ek Adı / Açıklama" },
                    {
                        field: "DosyaAdi",
                        title: "Dosya",
                        template: "#= DosyaAdi ? '<i class=\"k-icon k-i-file\"></i> ' + DosyaAdi : '<span class=\"text-muted\">Dosya Yok</span>' #"
                    },
                    {
                        command: [{
                            className: "btn btn-outline-danger ms-2",
                            text: "Sil",
                            click: function (e) {
                                e.preventDefault();
                                var dataItem = _grid.dataItem($(e.currentTarget).closest("tr"));
                                if (dataItem) _grid.dataSource.remove(dataItem);
                            }
                        }], title: "İşlem", width: "100px"
                    }
                ],
                dataSource: { data: [] }
            }).data("kendoGrid");
        },

        // ALT KISIM: Yan ekler için dosya seçildiğinde (Giden evrakla aynı)
        ekDosyaSecildi: function (input) {
            if (input.files && input.files[0]) {
                _tempFile = input.files[0];
                $("#secilenEkDosyaBilgisi").text("Seçilen: " + _tempFile.name).show();
                if (!$("#EkAdi").val()) $("#EkAdi").val(_tempFile.name);
            }
        },

        // Grid'e ekleme (Giden evrakla aynı)
        ekEkle: function () {
            var ad = $("#EkAdi").val();
            if (!ad && !_tempFile) return;

            _grid.dataSource.add({
                Id: 0,
                Ad: ad || (_tempFile ? _tempFile.name : ""),
                DosyaAdi: _tempFile ? _tempFile.name : "",
                DosyaObj: _tempFile
            });

            $("#EkAdi").val("");
            $("#EkDosya").val("");
            $("#secilenEkDosyaBilgisi").hide();
            _tempFile = null;
        },

        setData: function (ekler) {
            if (_grid && Array.isArray(ekler)) {
                var mapped = ekler.map(function (item) {
                    return {
                        Id: item.id || item.Id,
                        Ad: item.ad || item.Ad,
                        DosyaAdi: item.dosyaUzantisi ? (item.ad + item.dosyaUzantisi) : "",
                        DosyaObj: null // Mevcut dosyalar DB'den byte[] olarak gelecek, IFormFile değil
                    };
                });
                _grid.dataSource.data(mapped);
            }
        },

       
        getData: function () {
         
            return _grid.dataSource.data().map(function (item) {
                return {
                    Id: item.Id,
                    Ad: item.Ad,
                    Dosya: item.DosyaObj 
                };
            });
        }
    };
})();


$(document).ready(function () {
    GelenEklerModule.init();
});