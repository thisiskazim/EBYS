var GelenEvrakListModule = (function () {
    var _grid = null;
    var _onizlemeDialog = null;
    var _apiBaseUrl = "https://localhost:7060/api/GelenEvrak/";

    var _initDialog = function () {
        _onizlemeDialog = $("#onizlemeDialog").kendoDialog({
            width: "1300px",
            height: "800px",
            title: "Evrak Detay Önizleme",
            closable: true,
            modal: true,
            visible: false,
            actions: [{ text: "Kapat" }]
        }).data("kendoDialog");
    };

    return {
        init: function () {
            _initDialog();
            this.initGrid();
            this.loadData();
        },

        initGrid: function () {
            _grid = $("#gridGelenEvraklar").kendoGrid({
                noRecords: { template: "<div class='p-5 text-center text-muted'>Kayıtlı gelen evrak bulunamadı.</div>" },
                sortable: true,
                pageable: { pageSize: 15, refresh: true },
                columns: [
                    {
                        title: "İŞLEMLER",
                        width: "100px",
                        attributes: { style: "text-align: center" },
                        template: function (dataItem) {
                            return `
                            <div class='dropdown'>
                                <button class='btn btn-light btn-sm border shadow-sm' type='button' data-bs-toggle='dropdown' aria-expanded='false'>
                                    <i class='fas fa-ellipsis-v text-primary'></i>
                                </button>
                                <ul class='dropdown-menu shadow-lg border-0' style='min-width: 220px; border-radius:10px;'>
                                    <li><h6 class="dropdown-header text-uppercase small fw-bold">Evrak Yönetimi</h6></li>
                                    <li><a class='dropdown-item py-2' href='#' onclick='GelenEvrakListModule.edit(${dataItem.Id})'><i class='fas fa-edit me-2 text-warning'></i>Düzenle</a></li>
                                    <li><a class='dropdown-item py-2' href='#' onclick='GelenEvrakListModule.evrakAdimlari(${dataItem.Id})'><i class='fas fa-shoe-prints me-2 text-info'></i>Evrak Adımları</a></li>
                                    <li><hr class='dropdown-divider'></li>
                                    <li><h6 class="dropdown-header text-uppercase small fw-bold">Akış İşlemleri</h6></li>
                                    <li><a class='dropdown-item py-2' href='#' onclick='GelenEvrakListModule.teslimAl(${dataItem.Id})'><i class='fas fa-hand-holding me-2 text-success'></i>Şahsım Adına Teslim Al</a></li>
                                    <li><a class='dropdown-item py-2' href='#' onclick='GelenEvrakListModule.personeleSevkEt(${dataItem.Id})'><i class='fas fa-user-tag me-2 text-primary'></i>Personele Sevk Et</a></li>
                                    <li><hr class='dropdown-divider'></li>
                                    <li><a class='dropdown-item py-2 text-danger' href='#' onclick='GelenEvrakListModule.delete(${dataItem.Id})'><i class='fas fa-trash-alt me-2'></i>Sil</a></li>
                                </ul>
                            </div>`;
                        }
                    },
                    {
                        title: "DOSYA",
                        width: "80px",
                        attributes: { style: "text-align: center" },
                        template: (dataItem) => `<button class='btn btn-link btn-sm' onclick='GelenEvrakListModule.onizle(${dataItem.Id})'><i class='fas fa-file-pdf fa-lg text-danger'></i></button>`
                    },
                    { field: "EvrakSayisi", title: "Kurum Sayısı", width: "150px" },
                    { field: "Konu", title: "Evrak Konusu", width: "250px" },
                    { field: "MuhatapAdi", title: "Gönderen Makam", width: "200px" },
                    {
                        field: "EvrakTarihi",
                        title: "Evrak Tarihi",
                        width: "130px",
                        template: "#= kendo.toString(kendo.parseDate(EvrakTarihi), 'dd.MM.yyyy') #"
                    },
                    { field: "OlusturanKullanici", title: "Kaydeden", width: "150px" }
                ]
            }).data("kendoGrid");
        },

        loadData: function () {
            $.get(_apiBaseUrl + "EvrakListele", function (res) {
                _grid.dataSource.data(res);
            });
        },

        onizle: function (id) {
            _onizlemeDialog.open();
            $("#onizleme-yukleniyor").show();
            $("#popupDosyaListesi").empty();

            $.get(_apiBaseUrl + "EvrakGetir/" + id, function (evrak) {
                $("#onizleme-yukleniyor").hide();

                // Gelen evrakta "Üst Yazı" aslında ekler listesinin ilk elemanı (Indis 0)
                if (evrak.ekler && evrak.ekler.length > 0) {
                    evrak.ekler.forEach(function (ek, index) {
                        var isMain = (index === 0);
                        var icon = isMain ? "fa-file-signature text-primary" : "fa-paperclip text-secondary";
                        var label = isMain ? "Üst Yazı (Asıl)" : ek.ad;

                        var btn = $(`
                            <a href="#" class="list-group-item list-group-item-action ${isMain ? 'active' : ''}">
                                <div class="d-flex align-items-center">
                                    <i class="fas ${icon} me-2"></i>
                                    <span class="small fw-bold">${label}</span>
                                </div>
                            </a>`);

                        btn.click(function (e) {
                            e.preventDefault();
                            $(this).addClass("active").siblings().removeClass("active");
                            GelenEvrakListModule.pdfGoster(ek.dosyaIcerik, ek.mimeType);
                        });
                        $("#popupDosyaListesi").append(btn);

                        // İlk dosyayı otomatik aç
                        if (isMain) GelenEvrakListModule.pdfGoster(ek.dosyaIcerik, ek.mimeType);
                    });
                }
            });
        },

        pdfGoster: function (base64, mimeType) {
            var type = mimeType || "application/pdf";
            $("#pdf-frame-popup").attr("src", `data:${type};base64,${base64}#toolbar=1`);
        },
        edit: id => window.location.href = '/GelenEvrak/EvrakOlustur?id=' + id,
        evrakAdimlari: id => console.log("Adımlar: " + id),
        teslimAl: id => alert(id + " nolu evrak üzerinize alındı."),
        delete: id => confirm("Silmek istediğinize emin misiniz?") ? console.log("Siliniyor...") : null,

        filterGrid: function () {
            var qKonu = $("#filterKonu").val();
            var qOlusturan = $("#filterOlusturan").val();
            var qKod = $("#filterKonuKodu").val();

            _grid.dataSource.filter({
                logic: "and",
                filters: [
                    { field: "Konu", operator: "contains", value: qKonu },
                    { field: "OlusturanKullanici", operator: "contains", value: qOlusturan },
                    { field: "FullKonuKodu", operator: "contains", value: qKod }
                ]
            });
        }
};
})();

$(document).ready(() => GelenEvrakListModule.init());