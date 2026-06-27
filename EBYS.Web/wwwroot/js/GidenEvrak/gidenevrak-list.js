var GidenEvrakListModule = (function () {
    var _apiBaseUrl = "https://localhost:7060/api/";
    var _aktifOlanFiltre = null;

    var _enumMap = {
        'tumGidenEvraklar': 1,
        'iadeEttiklerim': 2,
        'sahsimaIadeEdilenler': 3,
        'reddettiklerim': 4,
        'banareddönen': 5
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

        loadData: function () {
            kendo.ui.progress($("#gridGidenEvraklar"), true);
            var formData = new FormData();

            if (_aktifOlanFiltre !== null) {
                formData.append("durum", _aktifOlanFiltre);
            }

            ApiService.postFormData("GidenEvrak/GidenEvrakListesi", formData).done(function (res) {
                $("#gridGidenEvraklar").data("kendoGrid").dataSource.data(res);
            }).always(function () {
                kendo.ui.progress($("#gridGidenEvraklar"), false);
            }); 
        },
        dosyaIndir: function (ekId) {
            window.location.href = _apiBaseUrl + "GidenEvrak/DosyaIndir/" + ekId;
        },

        history: function (id) {
         
            ApiService.getJson("GidenEvrak/EvrakHareketleri/" + id).done(function (res) {


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