var EklerModule = (function () {
    var _grid = null;
    var _tempFile = null; // Seçilen dosyayı geçici tutmak için

    return {
        init: function () {
            _grid = $("#eklerGrid").kendoGrid({
                columns: [
                    { field: "Ad", title: "Ek Adı / Açıklama" },
                    {
                        field: "DosyaAdi",
                        title: "Dosya",
                        template: function (dataItem) {
                            return dataItem.DosyaAdi ? `<i class='k-icon k-i-file'></i> ${dataItem.DosyaAdi}` : `<span class='text-muted'>Dosya Yok</span>`;
                        }
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
                        }],
                        title: "İşlem", width: "100px"
                    }
                ],
                dataSource: {
                    data: [],
                    schema: {
                        model: {
                            id: "Id",
                            fields: {
                                Id: { type: "number", defaultValue: 0 },
                                Ad: { type: "string" },
                                DosyaAdi: { type: "string" },
                                DosyaObj: { nullable: true } // IFormFile için dosya objesi
                            }
                        }
                    }
                }
            }).data("kendoGrid");
        },

        dosyaSecildi: function (input) {
            if (input.files && input.files[0]) {
                _tempFile = input.files[0];
                $("#secilenDosyaBilgisi").text("Seçilen: " + _tempFile.name).show();

                // Eğer input boşsa dosya adını otomatik yaz
                if (!$("#EkAdi").val()) {
                    $("#EkAdi").val(_tempFile.name);
                }
            }
        },

        ekle: function () {
            var ad = $("#EkAdi").val();

            if (!ad && !_tempFile) {
                alert("Lütfen bir ek adı girin veya dosya seçin.");
                return;
            }

            _grid.dataSource.add({
                Id: 0,
                Ad: ad || (_tempFile ? _tempFile.name : ""),
                DosyaAdi: _tempFile ? _tempFile.name : "",
                DosyaObj: _tempFile // Asıl dosya verisi burada
            });

            // Temizlik
            $("#EkAdi").val("");
            $("#EkDosya").val("");
            $("#secilenDosyaBilgisi").hide();
            _tempFile = null;
        },

        // Update aşamasında mevcut ekleri yüklemek için
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

        // Kaydetme anında FormData'ya basılacak veriyi hazırlar
        getData: function () {
            return _grid.dataSource.data().map(function (item) {
                return {
                    Id: item.Id,
                    Ad: item.Ad,
                    Dosya: item.DosyaObj // FormData append edilirken kullanılacak
                };
            });
        }
    };
})();

$(document).ready(function () {
    EklerModule.init();
});