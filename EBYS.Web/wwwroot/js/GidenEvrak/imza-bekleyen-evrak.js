
var EvrakBekleyenListModule = (function () {
    var _grid = null;

    return {
        init: function () {
            this.initGrid();
            this.loadData();
        },

        initGrid: function () {
            _grid = $("#gridBekleyenler").kendoGrid({
                noRecords: { template: "<div class='p-5 text-center text-muted'>İmza bekleyen evrak bulunamadı.</div>" },
                sortable: true,
                pageable: { pageSize: 10, buttonCount: 5 },
                columns: [
                    {
                        title: "İşlemler",
                        width: "120px",
                        headerAttributes: { style: "text-align: center" },
                        attributes: { style: "text-align: center" },
                        template: function (dataItem) {
                          
                            var editHtml = dataItem.editYapabilirMi
                                ? `<li><a class='dropdown-item py-2' href='#' onclick='EvrakBekleyenListModule.edit("${dataItem.id}")'><i class='fas fa-edit text-primary me-2'></i>Düzenle</a></li>`
                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Düzenle <small>(Yetki Yok)</small></a></li>`;

                            var deleteHtml = dataItem.editYapabilirMi
                                ? `<li><a class='dropdown-item py-2 text-danger' href='#' onclick='EvrakBekleyenListModule.cancel("${dataItem.id}")'><i class='fas fa-trash-alt me-2'></i>Sil</a></li>`
                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Sil <small>(Yetki Yok)</small></a></li>`;

                            return `
                        <div class='dropdown'>
                            <button class='btn btn-link btn-sm p-0 border-0'
                                    type='button'
                                    data-bs-toggle='dropdown'
                                    data-bs-popper-config='{"strategy":"fixed"}'
                                    aria-expanded='false'
                                    style='text-decoration: none;'>
                                <i class='fas fa-ellipsis-v text-info' style='font-size: 18px;'></i>
                            </button>
                            <ul class='dropdown-menu dropdown-menu-end shadow-lg border-0' style='border-radius: 12px; min-width: 160px;'>
                                <li>
                                    <a class='dropdown-item py-2' href='#' onclick='EvrakBekleyenListModule.onay("${dataItem.id}")'>
                                        <i class='fas fa-file-signature text-success me-2'></i>İmzala
                                    </a>
                                </li>
                                ${editHtml}
                                <li><hr class='dropdown-divider opacity-50'></li>
                                ${deleteHtml}
                            </ul>
                        </div>`;
                        }
                    },
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
                        field: "olusturanKullanici", 
                        title: "Oluşturan",
                        template: "<div class='d-flex align-items-center'>#: olusturanKullanici #</div>",
                        width: "120px"
                    },
                    {
                        field: "konu", 
                        title: "Konu",
                        width: "200px",
                        template: "<div>#: konu #</div>" 
                    },
                    {
                        field: "fullKonuKodu", 
                        title: "Konu Kodu",
                        width: "200px",
                        template: "<span class='badge bg-light text-dark border'>#: fullKonuKodu #</span>"
                    },
                    {
                        field: "creat_time", 
                        title: "Oluşturma Zamanı",
                        width: "250px",
                        template: "#= creat_time ? kendo.toString(kendo.parseDate(creat_time), 'dd.MM.yyyy HH:mm') : '' #"
                    }
                ],
                dataSource: { data: [] }
            }).data("kendoGrid");
        },

     

        loadData: function () {
            var $gridEl = $("#gridBekleyenler"); 
            kendo.ui.progress($gridEl, true); 

            
            ApiService.getJson("Akis/imza-bekleyen-listele").done(function (res) {
                    _grid.dataSource.data(res);
                })
                .always(function () {
                    kendo.ui.progress($gridEl, false); 
                });
        },

        onay: function (id) {
            if (!confirm("Seçili evrakı onaylamak istediğinize emin misiniz?")) return;
            ApiService.postJson("Akis/Onayla/" + id)
                .done(function (response) {
                    showNotification(response.mesaj || "Evrak başarıyla onaylandı.", "success");
                    EvrakBekleyenListModule.loadData();
                });
        },

        edit: function (id) {
            window.location.href = '/Home/Index?id=' + id;
        },

        cancel: function (id) {
            if (confirm("Bu evrakı silmek istediğinize emin misiniz?")) {
                ApiService.delete("GidenEvrak/EvrakSil/" + id).done(function (response) {
                    showNotification(response.mesaj || "Evrak başarıyla silindi.", "success");
                    EvrakBekleyenListModule.loadData();
                });
            }
        }
    };
})();

$(document).ready(function () {
    EvrakBekleyenListModule.init();
});

