var GelenEvrakListModule = (function () {
    var _grid = null;
    var _onizlemeDialog = null;
    var _apiBaseUrl = "https://localhost:7060/api/GelenEvrak/";



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
                } else if (err.responseText) {
                    msg = err.responseText;
                }
                showNotification(msg, "error");
            }
        });
    };

    //var _initDialog = function () {
    //    _onizlemeDialog = $("#onizlemeDialog").kendoDialog({
    //        width: "1300px",
    //        height: "800px", // İçerideki 720px'lik d-flex'i rahat taşısın
    //        title: "Evrak Detay Önizleme",
    //        closable: true,
    //        modal: true,
    //        visible: false,
    //        actions: [{ text: "Kapat" }]
    //    }).data("kendoDialog");
    //};

    return {
        init: function () {
          /*  _initDialog();*/
            this.initGrid();
            this.loadData();
        },

        initGrid: function () {
            _grid = $("#gridGelenEvraklar").kendoGrid({
                noRecords: { template: "<div class='p-5 text-center text-muted'>Kayıtlı gelen evrak bulunamadı.</div>" },
                sortable: true,
                resizable: true,
                pageable: { pageSize: 15, refresh: true, buttonCount: 5 },
                columns: [
                    {
                        title: "İŞLEMLER",
                        width: "100px",
                        headerAttributes: { style: "text-align: center" },
                        attributes: { style: "text-align: center" },
                        template: function (dataItem) {
                            var canEdit = dataItem.editYapabilirMi;

                            var editHtml = canEdit
                                ? `<li><a class='dropdown-item py-2' href='#' onclick='GelenEvrakListModule.edit("${dataItem.id}")'><i class='fas fa-edit text-warning me-2'></i>Düzenle</a></li>`
                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Düzenle <small>(Yetki Yok)</small></a></li>`;

                            var deleteHtml = canEdit
                                ? `<li><a class='dropdown-item py-2 text-danger' href='#' onclick='GelenEvrakListModule.delete("${dataItem.id}")'><i class='fas fa-trash-alt me-2'></i>Sil</a></li>`
                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Sil <small>(Yetki Yok)</small></a></li>`;

                            return `
                                <div class='dropdown'>
                                    <button class='btn btn-link btn-sm p-0 border-0'
                                            type='button'
                                            data-bs-toggle='dropdown'
                                            data-bs-popper-config='{"strategy":"fixed"}'
                                            aria-expanded='false'
                                            style='text-decoration: none;'>
                                        <i class='fas fa-ellipsis-v text-primary' style='font-size: 18px;'></i>
                                    </button>
                                    <ul class='dropdown-menu dropdown-menu-end shadow-lg border-0' style='border-radius: 12px; min-width: 200px; z-index: 9999;'>
                                        <li><h6 class="dropdown-header text-uppercase small fw-bold">Evrak Yönetimi</h6></li>
                                        <li><a class='dropdown-item py-2' href='#' onclick='GelenEvrakListModule.evrakAdimlari("${dataItem.id}")'><i class='fas fa-shoe-prints text-info me-2'></i>Evrak Hareketleri</a></li>
                                        ${editHtml}
                                        <li><hr class='dropdown-divider opacity-50'></li>
                                        <li><h6 class="dropdown-header text-uppercase small fw-bold">Akış İşlemleri</h6></li>
                                        <li><a class='dropdown-item py-2' href='#' onclick='GelenEvrakListModule.teslimAl("${dataItem.id}")'><i class='fas fa-hand-holding-check text-success me-2'></i>Teslim Al</a></li>
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

                            var html = `<div class='evrak-dosya-konteynir' onclick='GelenEvrakListModule.toggleEkler(this)'>
                        <div class='small fw-bold text-primary'>
                            <i class='fas fa-folder me-1'></i>${ekListesi.length} Adet Dosya
                            <i class='fas fa-chevron-down float-end mt-1 small'></i>
                        </div>
                        <div class='ek-listesi-gizli'>`;

                            ekListesi.forEach(function (ek) {
                                var uzanti = ek.dosyaUzantisi || "";
                                var icon = GelenEvrakListModule.getIconByExtension(uzanti);
                                var action = uzanti.toLowerCase().includes("pdf")
                                    ? `EvrakOnizlemeModule.ac(${ek.id}, 'gelen')`
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
                    { field: "kayitNo", title: "Kayıt No", width: "100px", attributes: { class: "fw-bold" } },
                    { field: "evrakSayisi", title: "Kurum Sayısı", width: "180px" },
                    { field: "konu", title: "Evrak Konusu", width: "250px" },
                    { field: "gonderenMuhatapAdi", title: "Gönderen Makam", width: "200px" },
                    {
                        field: "evrakTarihi",
                        title: "Evrak Tarihi",
                        width: "120px",
                        template: "#= evrakTarihi ? kendo.toString(kendo.parseDate(evrakTarihi), 'dd.MM.yyyy') : '' #"
                    },
                    {
                        field: "suAnKimde",
                        title: "Şu An Kimde",
                        width: "180px",
                        template: "<span class='badge bg-info text-dark'>#: suAnKimde #</span>"
                    }
                ]
            }).data("kendoGrid");
        },

        loadData: function () {
            kendo.ui.progress($("#gridGelenEvraklar"), true);
            $.get(_apiBaseUrl + "EvrakListele", function (res) {
                _grid.dataSource.data(res);
                kendo.ui.progress($("#gridGelenEvraklar"), false);
            });
        },
        dosyaIndir: function (ekId) {
            // Backend'de "DosyaIndir/{id}" şeklinde bir endpoint olmalı
            window.location.href = _apiBaseUrl + "DosyaIndir/" + ekId;
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
        },
       

        evrakAdimlari: function (id) {
            var self = this;

          
            var winElement = $("#historyWindow");
            var win = winElement.data("kendoWindow");

            if (!win) {
                win = winElement.kendoWindow({
                    width: "850px",
                    title: "Evrak Hareketleri",
                    visible: false,
                    modal: true,
                    actions: ["Close"]
                }).data("kendoWindow");
            }

    
            var gridElement = $("#historyGrid");

         
            kendo.ui.progress($("#gridGelenEvraklar"), true);

      
            _ajaxCall('evrak-sevk-hareketleri/' + id, 'GET')
                .done(function (res) {

         
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
                                        sevkTarihi: { type: "date" }
                                    }
                                }
                            }
                        },
                        sortable: true,
                        columns: [
                            {
                                field: "sevkEdenKullaniciAdSoyad",
                                title: "Sevk Eden (Kimden)",
                                width: "200px",
                                template: "<div class='fw-bold text-secondary'><i class='fas fa-user-share me-1'></i>#: sevkEdenKullaniciAdSoyad #</div>"
                            },
                            {
                                field: "alanKullaniciAdSoyad",
                                title: "Alıcı (Kime)",
                                width: "200px",
                                template: function (dataItem) {
                                    return dataItem.alanKullaniciAdSoyad
                                        ? `<div class='fw-bold text-primary'><i class='fas fa-user-check me-1'></i>${dataItem.alanKullaniciAdSoyad}</div>`
                                        : `<span class='badge bg-warning text-dark'>Beklemede</span>`;
                                }
                            },
                            {
                                field: "sevkTarihi",
                                title: "Sevk Tarihi",
                                width: "150px",
                                template: "#= sevkTarihi ? kendo.toString(kendo.parseDate(sevkTarihi), 'dd.MM.yyyy HH:mm') : '' #"
                            },
                            {
                                field: "aciklama",
                                title: "Not",
                                template:
                                    function (dataItem) {
                                        return dataItem.aciklama
                                            ? `<div class='fw-bold text-primary'><i class='fas fa-user-check me-1'></i>${dataItem.aciklama}</div>`
                                            : `<span class='badge bg-warning text-dark'>Yok</span>`;
                                    }
                            },
                        ]
                    });

                    var activeWin = $("#historyWindow").data("kendoWindow");

                    if (activeWin) {
                        activeWin.center().open();
                    } else {
                        console.error("Kendo Window başlatılamadı. HTML tarafında #historyWindow ID'li bir div olduğundan emin olun.");
                    }
                })
                .always(function () {
                    kendo.ui.progress($("#gridGelenEvraklar"), false); // İşlem bitince loading kapat
                });
        },
       
        edit: id => window.location.href = '/GelenEvrak/GelenEvrak?id=' + id,

        delete: function (id) {
            if (confirm("Bu evrakı silmek istediğinize emin misiniz?")) {
                $.ajax({
                    url: _apiBaseUrl + "EvrakSil/" + id,
                    type: "DELETE",
                    success: function (res) {
                        showNotification("Evrak başarıyla silindi.", "success");
                        GelenEvrakListModule.loadData();
                    }
                });
            }
        },

        teslimAl: function (id) {
            // Çift tıklamayı veya işlem sırasında beklemeyi yönetmek için progress açıyoruz
            kendo.ui.progress($("#gridGelenEvraklar"), true);

            _ajaxCall("SahsimaTeslimAl/" + id, "POST")
                .done(function (res) {
                    // Başarılıysa bildirim ver ve ana grid'i tazele
                    showNotification("Evrak üzerinize başarıyla alındı.", "success");
                    GelenEvrakListModule.loadData();
                })
                .always(function () {
                    kendo.ui.progress($("#gridGelenEvraklar"), false);
                });
        },

        personeleSevkEt: function (id) {
            // Burada bir Kendo Window veya Modal açıp personel seçtireceğiz
            console.log("Sevk edilecek Evrak ID: " + id);
            // Örn: SevkModule.open(id);
        }
    };
})();

$(document).ready(() => GelenEvrakListModule.init());