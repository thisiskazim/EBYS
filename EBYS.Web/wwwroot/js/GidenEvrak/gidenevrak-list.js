var GidenEvrakListModule = (function () {
    var _apiBaseUrl = "https://localhost:7060/api/";
    var _aktifOlanFiltre = 1;


    var _enumMap = {
        'tumGidenEvraklar': 1,//belge durumu tamamlanmış olan evraklar listesi olacak ve işlemler kısmı boş olacak
        'iadeEttiklerim': 2,// iade ettiklerim olack işlemler kısmı boş olcak 
        'sahsimaIadeEdilenler': 3,// burda işlemler kısmı olacak onay düzenle redder gibi durumlar olacak 
        'reddettiklerim': 4,//işlemler kısmı boş olacak 
        'banareddönen': 5 // işlemler kısmı boş olacak 
    };
   

    return {
        init: function () {
            this.initGrid();
            this.loadData();
        },

        initGrid: function () {
            _grid = $("#gridGidenEvraklar").kendoGrid({
                noRecords: { template: "<div class='p-5 text-center text-muted'>Kayıtlı giden evrak bulunamadı.</div>" },
                sortable: true,
                resizable: true,
                pageable: { pageSize: 15, refresh: true, buttonCount: 5 },
                columns: [ 
                    
                    {
                    title: "İşlemler",
                    width: "120px",
                    headerAttributes: { style: "text-align: center" },
                    attributes: { style: "text-align: center" },
                        template: function (dataItem) {
                            if (_aktifOlanFiltre !== _enumMap.sahsimaIadeEdilenler) {
                                return "<span class='text-muted small'>-</span>";
                            }   

                            var canEdit = dataItem.editYapabilirMi;

                            var editHtml = canEdit
                                ? `<li><a class='dropdown-item py-2' href='#' onclick='AkisOnayRedEditModule.edit("${dataItem.id}")'><i class='fas fa-edit text-primary me-2'></i>Düzenle</a></li>`
                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Düzenle <small>(Yetki Yok)</small></a></li>`;

                            var deleteHtml = canEdit
                                ? `<li><a class='dropdown-item py-2 text-danger' href='#' onclick='AkisOnayRedEditModule.cancel("${dataItem.id}", "#gridGidenEvraklar", () => GidenEvrakListModule.loadData())'><i class='fas fa-trash-alt me-2'></i>Sil</a></li>`
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
                                   <li><a class='dropdown-item py-2' href='#' onclick='GidenEvrakListModule.history("${dataItem.id}")'><i class='fas fa-shoe-prints text-info me-2'></i>Evrak Hareketleri</a></li>

                                  <li>
                                        <a class='dropdown-item py-2' href='#' onclick='AkisOnayRedEditModule.onayla("${dataItem.id}", "#gridGidenEvraklar", () => GidenEvrakListModule.loadData())'>
                                            <i class='fas fa-file-signature text-success me-2'></i>İmzala
                                        </a>
                                    </li>

                                    <li>
                                        <a class='dropdown-item py-2 text-warning' href='#' onclick='AkisOnayRedEditModule.iadePopupAc("${dataItem.id}", "#gridGidenEvraklar", () => GidenEvrakListModule.loadData())'>
                                            <i class='fas fa-reply me-2'></i>İade Et
                                        </a>
                                    </li>

                                    <li>
                                        <a class='dropdown-item py-2 text-danger' href='#' onclick='AkisOnayRedEditModule.reddetPopupAc("${dataItem.id}", "#gridGidenEvraklar", () => GidenEvrakListModule.loadData())'>
                                            <i class='fas fa-times-circle me-2'></i>Reddet
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

                            var html = `<div class='evrak-dosya-konteynir' onclick='GidenEvrakListModule.toggleEkler(this)'>
                        <div class='small fw-bold text-info'>
                            <i class='fas fa-folder me-1'></i>${ekListesi.length} Adet Dosya
                            <i class='fas fa-chevron-down float-end mt-1 small'></i>
                        </div>
                        <div class='ek-listesi-gizli'>`;

                            ekListesi.forEach(function (ek) {
                                var uzanti = ek.dosyaUzantisi || "";
                                var icon = GidenEvrakListModule.getIconByExtension(uzanti);
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
                        title: "Hareketler",
                        width: "120px",
                        attributes: { style: "text-align: center" },
                        template: `<button class='btn btn-outline-info btn-sm' onclick='GidenEvrakListModule.history("#: id #")'>
                                    <i class='fas fa-history'></i> Hareketler
                                   </button>`
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
                 
                ]
            }).data("kendoGrid");
        },
        //loadData: function () {
        //    var $gridEl = $("#gridGidenEvraklar");
        //    kendo.ui.progress($gridEl, true);

        //    // 🎯 FormData karmaşasını kaldırdık, projenin yeni parametrik standardına geçtik:
        //    var url = "GidenEvrak/GidenEvrakListesi?filtreTipi=" + _aktifOlanFiltre;

        //    ApiService.postJson(url, {})
        //        .done(function (res) {
        //            // Grid veri kaynağını yeniliyoruz
        //            _grid.dataSource.data(res);
        //            // 🎯 Veri yenilendiğinde kolonların kendilerini ve içindeki template mantığını yeniden tetiklemesini sağlıyoruz:
        //            _grid.refresh();
        //        })
        //        .always(function () {
        //            kendo.ui.progress($gridEl, false);
        //        });
        //},
        loadData: function () {
            kendo.ui.progress($("#gridGidenEvraklar"), true);
            var formData = new FormData();
            formData.append("filtreTipi", _aktifOlanFiltre);
            

            ApiService.postFormData("GidenEvrak/EvrakListele", formData).done(function (res) {
                $("#gridGidenEvraklar").data("kendoGrid").dataSource.data(res);
            }).always(function () {
                kendo.ui.progress($("#gridGidenEvraklar"), false);
            }); 
        },
        dosyaIndir: function (ekId) {
            window.location.href = _apiBaseUrl + "GidenEvrak/DosyaIndir/" + ekId;
        },

        history: function (id) {
         
            ApiService.getJson("GidenEvrak/evrak-hareketleri/" + id).done(function (res) {


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
                                return `<span class='badge bg-info'>${dataItem.adimDurumuStr}</span>`;
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
       
        tabFiltrele: function (tabKey) {
            _aktifOlanFiltre = _enumMap[tabKey];
            GidenEvrakListModule.loadData();
        },
        getIconByExtension: function (ext) {
            if (!ext) return "fas fa-file text-secondary";
            ext = ext.toLowerCase();
            if (ext.includes("pdf")) return "fas fa-file-pdf text-danger";
            if (ext.includes("xls")) return "fas fa-file-excel text-success";
            if (ext.includes("doc")) return "fas fa-file-word text-primary";
            if (ext.includes("jpg") || ext.includes("png")) return "fas fa-file-image text-warning";
            return "fas fa-file text-secondary";
        },
        toggleEkler: function (element) {
            var list = $(element).find('.ek-listesi-gizli');
            var icon = $(element).find('.fa-chevron-down, .fa-chevron-up');

            list.slideToggle('fast');
            icon.toggleClass('fa-chevron-down fa-chevron-up');
        }
    };
})();

$(document).ready(() => GidenEvrakListModule.init());