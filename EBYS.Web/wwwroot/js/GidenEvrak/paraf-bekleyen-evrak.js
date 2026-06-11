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
                            // Düzenle butonu yetki kontrolü
                            var editHtml = dataItem.CanEdit
                                ? `<li><a class='dropdown-item py-2' href='#' onclick='EvrakBekleyenListModule.edit("${dataItem.Id}")'><i class='fas fa-edit text-primary me-2'></i>Düzenle</a></li>`
                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Düzenle <small>(Yetki Yok)</small></a></li>`;

                            // Sil butonu yetki kontrolü
                            var deleteHtml = dataItem.CanEdit
                                ? `<li><a class='dropdown-item py-2 text-danger' href='#' onclick='EvrakBekleyenListModule.cancel("${dataItem.Id}")'><i class='fas fa-trash-alt me-2'></i>Sil</a></li>`
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
                                            <a class='dropdown-item py-2' href='#' onclick='EvrakBekleyenListModule.onay("${dataItem.Id}")'>
                                                <i class='fas fa-file-signature text-success me-2'></i>Parafla
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
                        field: "OlusturanKullanici",
                        title: "Oluşturan",
                        template: "<div class='d-flex align-items-center'></i>#: OlusturanKullanici #</div>",
                        width: "120px"
                    },
                    { field: "Konu", title: "Konu", width: "200px" },
                    {
                        field: "FullKonuKodu",
                        title: "Konu Kodu",
                        width: "200px",
                        template: "<span class='badge bg-light text-dark border'>#: FullKonuKodu #</span>"
                    },
                    {
                        field: "CreatTime",
                        title: "Oluşturma Zamanı",
                        width: "250px",
                        template: "#= kendo.toString(kendo.parseDate(CreatTime), 'dd.MM.yyyy HH:mm') #"
                    }
                ],
                dataSource: { data: [] }
            }).data("kendoGrid");
        },

        loadData: function () {
  
            AkisService.getJson('Akis/paraf-bekleyen-listele').done(function (res) {
                var list = Array.isArray(res) ? res : (res.data || []);
                var mappedList = list.map(x => ({
                    Id: x.id || x.Id,
                    OlusturanKullanici: x.olusturanKullanici ,
                    Konu: x.konu ,
                    FullKonuKodu: x.fullKonuKodu ,
                    CreatTime: x.creat_time,
                    CanEdit: x.editYapabilirMi
                }));
                _grid.dataSource.data(mappedList);
            });
        },

        onay: function (id) {
            if (!confirm("Seçili evrakı onaylamak istediğinize emin misiniz?")) {
                return;
            }
           
            ApiService.postJson('Akis/Onayla/' + id).done(function (response) {
                // IslemSonuc sınıfına göre kontrol yapıyoruz
                if (response.basariliMi) {
                    showNotification(response.mesaj, "success");
                    EvrakBekleyenListModule.loadData();

                } else {
                    showNotification(response.mesaj, "warning");
                }
            });
        },

        edit: function (id) {

            window.location.href = '/Home/Index?id=' + id;
        },

        cancel: function (id) {
            if (confirm("Bu evrakı silmek istediğinize emin misiniz?")) {
                ApiService.deleteJson('Akis/EvrakSil/' + id).done(function (response) {
                    showNotification(response, "success");
                    EvrakBekleyenListModule.loadData();
                });
            }
        }
    };
})();

$(document).ready(function () {
    EvrakBekleyenListModule.init();
});