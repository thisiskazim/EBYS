var EvrakGonderdiklerimModule = (function () {
    var _grid = null;


    

    return {
        init: function () {
            this.initGrid();
            this.loadData();
        },

        initGrid: function () {
            _grid = $("#gridGonderdiklerim").kendoGrid({
                noRecords: { template: "<div class='p-5 text-center text-muted'>Gönderilmiş evrak bulunamadı.</div>" },
                sortable: true,
                pageable: { pageSize: 10, buttonCount: 5 },
                columns: [
                      {
                        title: "DOSYALAR",
                        width: "200px",
                        template: function (dataItem) {
                            var ekListesi = dataItem.ekler || [];
                            if (ekListesi.length === 0) return "<span class='text-muted small'>Dosya yok</span>";

                            var html = `<div class='evrak-dosya-konteynir' onclick='EvrakOnizlemeModule.toggleEkler(this)'>
                        <div class='small fw-bold text-info'>
                            <i class='fas fa-folder me-1'></i>${ekListesi.length} Adet Dosya
                            <i class='fas fa-chevron-down float-end mt-1 small'></i>
                        </div>
                        <div class='ek-listesi-gizli'>`;

                            ekListesi.forEach(function (ek) {
                                var uzanti = ek.dosyaUzantisi || "";
                                var icon = EvrakOnizlemeModule.getIconByExtension(uzanti);
                                var action = uzanti.toLowerCase().includes("pdf")
                                    ? `EvrakOnizlemeModule.ac(${ek.id}, 'giden')`
                                    : `EvrakOnizlemeModule.dosyaIndir(${ek.id})`;

                                html += `<div class='mb-1'>
                        <a href='javascript:void(0)' onclick="event.stopPropagation(); ${action}" class='text-decoration-none text-dark small evrak-ek-link'>
                            <i class='${icon} me-1'></i>${ek.ad}
                        </a>
                     </div>`;
                            });
                            return html + "</div></div>";
                        }
                    },
                    {
                        field: "konu",
                        title: "Evrak Konusu",
                        width: "250px",
                        template: "<strong>#: konu #</strong>"
                    },
                    {
                        field: "fullKonuKodu",
                        title: "Konu Kodu",
                        width: "150px",
                        template: "<span class='badge bg-light text-dark border'>#: fullKonuKodu #</span>"
                    },
                    {
                        field: "suAnKimde",
                        title: "Durum / Şu An Kimde",
                        width: "180px",
                        template: "<i class='fas fa-user-clock text-primary me-2'></i>#: suAnKimde #"
                    },
                    {
                        field: "creat_time",
                        title: "Gönderim Tarihi",
                        width: "150px",
                        template: "#= kendo.toString(kendo.parseDate(creat_time), 'dd.MM.yyyy HH:mm') #"
                    },
                    {
                        title: "Hareketler",
                        width: "120px",
                        attributes: { style: "text-align: center" },
                        template: `<button class='btn btn-outline-info btn-sm' onclick='EvrakGonderdiklerimModule.history("#: id #")'>
                                    <i class='fas fa-history'></i> Hareketler
                                   </button>`
                    },
                    {
                        title: "İşlemler",
                        width: "100px",
                        attributes: { style: "text-align: center" },
                        template: function (dataItem) {
                            if (dataItem.geriCekilebilirMi) {
                                // DİKKAT: dataItem.id değil, dataItem.Id (Büyük I)
                                return `<button class='btn btn-warning btn-sm' onclick='EvrakGonderdiklerimModule.geriCek("${dataItem.id}")'>
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
            var $gridEl = $("#gridGonderdiklerim");
            kendo.ui.progress($gridEl, true); 

          
            ApiService.getJson("Akis/imzaya-gonderdiklerim").done(function (res) {
                    _grid.dataSource.data(res);
                })
                .always(function () {
                    kendo.ui.progress($gridEl, false); 
                });
        },

        history: function (id) {    
            var self = this;
            ApiService.getJson("Akis/evrak-hareketleri/" + id).done(function (res) {
                var winElement = $("#historyWindow");
                var win = winElement.data("kendoWindow");

                if (!win) {
                    win = winElement.kendoWindow({
                        width: "750px",
                        title: "Evrak Akış Hareketleri",
                        visible: false,
                        modal: true,
                        actions: ["Close"]
                    }).data("kendoWindow");
                }

             
                var gridElement = $("#historyGrid");
                if (gridElement.data("kendoGrid")) {
                    gridElement.data("kendoGrid").destroy(); 
                    gridElement.empty(); 
                }

               
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

            ApiService.postJson('Akis/GeriCek/' + id).done(function (response) {
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