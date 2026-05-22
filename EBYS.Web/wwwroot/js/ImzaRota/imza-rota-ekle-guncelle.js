
var ImzaRotaModule = (function () {

    var _grid = null;
    var _apiBaseUrl = "https://localhost:7060/api/ImzaRota/";



    var _ajaxCall = function (url, type, data) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: type,
            contentType: "application/json",
            data: data ? JSON.stringify(data) : null,
            error: function (err) {
                var msg = err && err.responseText ? err.responseText : "Bir hata oluştu.";
                showNotification(msg, "error");
            }
        });
    };


    var _fillForm = function (data) {
        if (!data) return;


        var rotaAdiTxt = $("#RotaAdi").data("kendoTextBox");
        if (rotaAdiTxt) rotaAdiTxt.value(data.rotaAdi);

        var gridData = (data.rotaAdimlari || []).map(function (x) {
            return {
                Id: x.id,
                KullaniciId: x.kullaniciId,
                AdSoyad: x.adSoyad || "Bilinmiyor",
                RolAdi: x.rolAdi || "Tanımsız",
                ImzaTuru: x.parafMiImzaMi,
                SiraNo: x.siraNo,
                ImzaTuruLabel: x.imzaTuruLabel || (x.parafMiImzaMi === 1 ? "İmza" : "Paraf")
            };
        });

        _grid.dataSource.data(gridData);
    };


    var _validatePayload = function (items, rotaAdi) {
        if (!rotaAdi || rotaAdi.trim() === "") return "Rota adı boş olamaz.";
        if (!items || items.length === 0) return "En az bir kişi ekleyin.";
        if (items.length > 5) return "En fazla 5 adım ekleyebilirsiniz.";

        var sonIndex = items.length - 1;
        var sonImzaTuru = parseInt(items[sonIndex].ImzaTuru);
        if (sonImzaTuru === 0) return 'İmza rotasında son kişi mutlaka "İmza" tipinde olmalıdır.';

        return null; 
    };

   
    return {
        init: function () {
            this.initGrid();
            this.initEvents();
            this.loadInitialData();
        },

        initGrid: function () {
            _grid = $("#rotaGrid").kendoGrid({
                columns: [
                    { field: "AdSoyad", title: "Ad Soyad" },
                    { field: "RolAdi", title: "Rol", width: 200 },
                    { field: "ImzaTuruLabel", title: "İmza Türü", width: 120 },
                    {
                        command: [{
                            text: "Sil",
                            click: function (e) {
                                e.preventDefault();
                                var tr = $(e.currentTarget).closest("tr");
                                var dataItem = _grid.dataItem(tr);
                                if (dataItem) _grid.dataSource.remove(dataItem);
                            }
                        }],
                        title: "İşlem", width: 100
                    }
                ],
                editable: false,
                dataSource: {
                    data: [],
                    schema: { model: { id: "Id" } } 
                },
                dataBound: function () {
       
                    var tbody = this.tbody;
                    if (!tbody.data('kendoSortable')) {
                        tbody.kendoSortable({
                            filter: ">tr",
                            cursor: 'move',
                            placeholder: function (element) { return element.clone().addClass('k-state-selected'); },
                            change: function (e) {
                                var ds = _grid.dataSource;
                                var item = ds.at(e.oldIndex);
                                ds.remove(item);
                                ds.insert(e.newIndex, item);
                            }
                        });
                    }
                }
            }).data('kendoGrid');
        },

        initEvents: function () {
            var self = this;

    
            $("#btnEkle").on('click', function () {
                var ddl = $("#Personel").data('kendoDropDownList');
                var dataItem = ddl ? ddl.dataItem() : null;

                if (!ddl || !ddl.value()) {
                    showNotification('Lütfen personel seçiniz', 'error');
                    return;
                }

                var currentItems = _grid.dataSource.data();
                var exists = currentItems.some(x => x.KullaniciId == ddl.value());
                if (exists) {
                    showNotification('Bu personel zaten eklenmiş.', 'error');
                    return;
                }

                var imzaVal = parseInt($("input[name='ImzaTuru']:checked").val());

                _grid.dataSource.add({
                    Id: 0,
                    KullaniciId: parseInt(ddl.value()),
                    AdSoyad: ddl.text(),
                    RolAdi: dataItem.RolAdi || dataItem.rolAdi || "",
                    ImzaTuru: imzaVal,
                    ImzaTuruLabel: imzaVal === 1 ? "İmza" : "Paraf"
                });

                ddl.value("");
            });

           
            $("#btnKaydet").on('click', function () {
                var rotaId = $("#RotaId").val();
                var rotaAdi = $("#RotaAdi").val();
                var gridItems = _grid.dataSource.data();

           
                var errorMsg = _validatePayload(gridItems, rotaAdi);
                if (errorMsg) {
                    showNotification(errorMsg, "error");
                    return;
                }

              
                var payload = {
                    Id: rotaId ? parseInt(rotaId) : 0,
                    RotaAdi: rotaAdi,
                    RotaAdimlari: gridItems.map((item, index) => ({
                        Id: item.Id || 0,
                        KullaniciId: item.KullaniciId,
                        ParafMiImzaMi: item.ImzaTuru,
                        SiraNo: index + 1 
                    }))
                };

                var action = payload.Id > 0 ? "ImzaRotaGuncelle" : "ImzaRotaEkle";

                _ajaxCall(action, "POST", payload).done(function () {
                    showNotification('İmza rotası başarıyla kaydedildi.', 'success');
                    setTimeout(function () { window.location.href = "/ImzaRota/ImzaRotaListe"; }, 1000);
                });
            });
        },

        loadInitialData: function () {
            var id = $("#RotaId").val();
            if (id && id !== "0" && id !== "") {
                _ajaxCall("ImzaRotaGetir/" + id, "GET").done(function (response) {
                    _fillForm(response);
                });
            }
        }
    };
})();


$(document).ready(function () {
    ImzaRotaModule.init();
});