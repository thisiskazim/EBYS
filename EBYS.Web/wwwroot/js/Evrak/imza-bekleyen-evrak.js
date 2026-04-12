var EvrakBekleyenListModule = (function () {
    var _grid = null;
    var _apiBaseUrl = "https://localhost:7060/api/ImzaBekleyenEvrak/";

    var _ajaxCall = function (url, type, data) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: type,
            contentType: "application/json",
            data: data ? JSON.stringify(data) : null,
            error: function (err) {
                var msg = err && err.responseText ? err.responseText : "Hata oluştu.";
                showNotification(msg, "error");
                console.error(msg);
            }
        });
    };

    return {
        init: function () {
            this.initGrid();
            this.loadData();
        },

        initGrid: function () {
            _grid = $("#evrakBekleyenGrid").kendoGrid({
                noRecords: { template: "<div class='p-5 text-center text-muted'>İmza bekleyen evrak bulunamadı.</div>" },
                sortable: true,
                pageable: { pageSize: 10 },
                columns: [
                    {
                        title: "İşlemler",
                        width: "120px",
                        headerAttributes: { style: "text-align: center" },
                        attributes: { style: "text-align: center" },
                        template: function (dataItem) {
                            return `
                                <div class='dropdown'>
                                    <button class='btn btn-sm btn-outline-secondary dropdown-toggle' type='button' data-bs-toggle='dropdown' aria-expanded='false'>
                                        <i class='fas fa-cog'></i>
                                    </button>
                                    <ul class='dropdown-menu shadow-sm'>
                                        <li><a class='dropdown-item' href='#' onclick='EvrakBekleyenListModule.sign("${dataItem.Id}")'><i class='fas fa-pen-nib text-success me-2'></i>İmzala</a></li>
                                        <li><a class='dropdown-item' href='#' onclick='EvrakBekleyenListModule.edit("${dataItem.Id}")'><i class='fas fa-edit text-warning me-2'></i>Düzenle</a></li>
                                        <li><hr class='dropdown-divider'></li>
                                        <li><a class='dropdown-item text-danger' href='#' onclick='EvrakBekleyenListModule.cancel("${dataItem.Id}")'><i class='fas fa-trash-alt me-2'></i>İptal Et</a></li>
                                    </ul>
                                </div>`;
                        }
                    },
                    {
                        field: "OlusturanKullanici",
                        title: "Oluşturan",
                        template: "<div class='d-flex align-items-center'><i class='fas fa-user-circle me-2 text-secondary'></i>#: OlusturanKullanici #</div>"
                    },
                    { field: "EvrakKonu", title: "Konu" },
                    {
                        field: "FullKonuKodu",
                        title: "Konu Kodu",
                        width: "140px",
                        template: "<span class='badge bg-light text-dark border'>#: KonuKodu #</span>"
                    },
                    {
                        field: "CreatTime",
                        title: "Oluşturma Zamanı",
                        width: "180px",
                        template: "#= kendo.toString(kendo.parseDate(CreatTime), 'dd.MM.yyyy HH:mm') #"
                    }
                ],
                dataSource: { data: [] }
            }).data("kendoGrid");
        },

        loadData: function () {
            var self = this;
            _ajaxCall('Listele', 'GET').done(function (res) {
                // Senin mapleme mantığın: API'den gelen küçük harf karmaşasını burada çözüyoruz
                var list = Array.isArray(res) ? res : (res.data || []);
                var mappedList = list.map(x => ({
                    Id: x.id || x.Id,
                    OlusturanKullanici: x.olusturanKullanici ,
                    EvrakKonu: x.konu ,
                    FullKonuKodu: x.fullKonuKodu ,
                    CreatTime: x.creat_time
                }));
                _grid.dataSource.data(mappedList);
            });
        },

        sign: function (id) {
            // İmzalama logic'i buraya
            console.log("İmzalanacak ID:", id);
        },

        edit: function (id) {
            window.location.href = '/Evrak/Ekle?id=' + id;
        },

        cancel: function (id) {
            if (confirm("Bu evrakı iptal etmek istediğinize emin misiniz?")) {
                // İptal API çağrısı yapılabilir
            }
        }
    };
})();

$(document).ready(function () {
    EvrakBekleyenListModule.init();
});