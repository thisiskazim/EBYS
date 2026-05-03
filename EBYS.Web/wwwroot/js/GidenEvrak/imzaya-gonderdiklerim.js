var EvrakGonderdiklerimModule = (function () {
    var _grid = null;
    var _apiBaseUrl = "https://localhost:7060/api/Akis/";

    var _ajaxCall = function (url, type, data) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: type,
            contentType: "application/json",
            data: data ? JSON.stringify(data) : null,
            error: function (err) {
                var msg = "Sistem hatası oluştu.";
                if (err.responseJSON && err.responseJSON.mesaj) {
                    msg = err.responseJSON.mesaj;
                }
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
            // HTML'deki id ile aynı olmalı: gridGonderdiklerim
            _grid = $("#gridGonderdiklerim").kendoGrid({
                noRecords: { template: "<div class='p-5 text-center text-muted'>Gönderilmiş evrak bulunamadı.</div>" },
                sortable: true,
                pageable: { pageSize: 10, buttonCount: 5 },
                columns: [
                    {
                        field: "Konu",
                        title: "Evrak Konusu",
                        width: "250px",
                        template: "<strong>#: Konu #</strong>"
                    },
                    {
                        field: "FullKonuKodu",
                        title: "Konu Kodu",
                        width: "150px",
                        template: "<span class='badge bg-light text-dark border'>#: FullKonuKodu #</span>"
                    },
                    {
                        field: "SuAnKimde",
                        title: "Durum / Şu An Kimde",
                        width: "180px",
                        template: "<i class='fas fa-user-clock text-primary me-2'></i>#: SuAnKimde #"
                    },
                    {
                        field: "CreatTime",
                        title: "Gönderim Tarihi",
                        width: "150px",
                        template: "#= kendo.toString(kendo.parseDate(CreatTime), 'dd.MM.yyyy HH:mm') #"
                    },
                    {
                        title: "Geçmiş",
                        width: "120px",
                        attributes: { style: "text-align: center" },
                        template: `<button class='btn btn-outline-info btn-sm' onclick='EvrakGonderdiklerimModule.history("#: Id #")'>
                                    <i class='fas fa-history'></i> Hareketler
                                   </button>`
                    },
                    {
                        title: "İşlemler",
                        width: "100px",
                        attributes: { style: "text-align: center" },
                        template: function (dataItem) {
                            if (dataItem.GeriCekilebilirMi) {
                                // DİKKAT: dataItem.id değil, dataItem.Id (Büyük I)
                                return `<button class='btn btn-warning btn-sm' onclick='EvrakGonderdiklerimModule.geriCek("${dataItem.Id}")'>
                                              <i class='fas fa-undo'></i> Geri Çek
                                         </button>`;
                            }
                            return `<span class='badge bg-secondary opacity-75'>Geri Alınamaz</span>`;
                        }
                    }
                ],
                dataSource: { data: [] }
            }).data("kendoGrid");
        },

        loadData: function () {
            _ajaxCall('imzaya-gonderdiklerim', 'GET').done(function (res) {
                var list = Array.isArray(res) ? res : (res.data || []);
                var mappedList = list.map(x => ({
                    Id: x.id || x.Id,
                    Konu: x.konu || x.Konu,
                    FullKonuKodu: x.fullKonuKodu || x.FullKonuKodu,
                    CreatTime: x.creat_time || x.CreatTime,
                    SuAnKimde: x.suAnKimde || x.SuAnKimde,
                    GeriCekilebilirMi: x.geriCekilebilirMi || x.GeriCekilebilirMi
                }));
                _grid.dataSource.data(mappedList);
            });
        },

        history: function (id) {    
            var self = this;
            _ajaxCall('evrak-hareketleri/' + id, 'GET').done(function (res) {

                // 1. Önce Window'u al veya oluştur
                var winElement = $("#historyWindow");
                var win = winElement.data("kendoWindow");

                if (!win) {
                    win = winElement.kendoWindow({
                        width: "750px",
                        title: "Evrak Akış Geçmişi",
                        visible: false,
                        modal: true,
                        actions: ["Close"]
                    }).data("kendoWindow");
                }

                // 2. KRİTİK NOKTA: Eğer grid daha önce oluşturulmuşsa verisini temizle/yok et
                var gridElement = $("#historyGrid");
                if (gridElement.data("kendoGrid")) {
                    gridElement.data("kendoGrid").destroy(); // Eski grid yapısını tamamen sil
                    gridElement.empty(); // İçindeki HTML kalıntılarını temizle
                }

                // 3. Grid'i taptaze verilerle yeniden oluştur
                gridElement.kendoGrid({
                    dataSource: {
                        data: res,
                        schema: {
                            model: {
                                fields: {
                                    creat_time: { type: "date" }
                                }
                            }
                        }
                    },
                    sortable: true,
                    columns: [
                        {
                            field: "kullaniciAdSoyad",
                            title: "İşlemi Yapan",
                            width: "200px",
                            template: "<div class='fw-bold'>#: kullaniciAdSoyad #</div>"
                        },
                        {
                            field: "adimDurumuStr",
                            title: "İşlem",
                            width: "120px",
                            template: function (dataItem) {
                                var color = dataItem.adimDurumuStr.includes("İade") ? "danger" : "info";
                                return `<span class='badge bg-${color}'>${dataItem.adimDurumuStr}</span>`;
                            }
                        },
                        {
                            field: "creat_time",
                            title: "Tarih",
                            width: "150px",
                            template: "#= kendo.toString(kendo.parseDate(creat_time), 'dd.MM.yyyy HH:mm') #"
                        },
                        { field: "not", title: "Açıklama/Not" }
                    ]
                });

                win.center().open();
            });
        },

        geriCek: function (id) {
            if (!confirm("Bu evrakı geri çekmek istediğinize emin misiniz?")) return;

            _ajaxCall('GeriCek/' + id, 'POST').done(function (response) {
                if (response.basariliMi) {
                    showNotification(response.mesaj, "success");
                    EvrakGonderdiklerimModule.loadData();
                } else {
                    showNotification(response.mesaj, "warning");
                }
            });
        }
    };
})();

$(document).ready(function () {
    EvrakGonderdiklerimModule.init();
});