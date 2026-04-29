//var EvrakBekleyenListModule = (function () {
//    var _grid = null;
//    var _apiBaseUrl = "https://localhost:7060/api/Akis/";

//    var _ajaxCall = function (url, type, data) {
//        return $.ajax({
//            url: _apiBaseUrl + url,
//            type: type,
//            contentType: "application/json",
//            data: data ? JSON.stringify(data) : null,
//            error: function (err) {
//                var msg = "Sistem hatası oluştu.";
//                if (err.responseJSON && err.responseJSON.mesaj) {
//                    msg = err.responseJSON.mesaj;
//                } else if (err.responseText) {
//                    msg = err.responseText;
//                }
//                showNotification(msg, "error");
//            }
//        });
//    };

//    return {
//        init: function () {
//            this.initGrid();
//            this.loadData();
//        },

//        initGrid: function () {
//            _grid = $("#gridBekleyenler").kendoGrid({
//                noRecords: { template: "<div class='p-5 text-center text-muted'>İmza bekleyen evrak bulunamadı.</div>" },
//                sortable: true,
//                pageable: { pageSize: 10, buttonCount: 5 },
//                columns: [
//                    {
//                        title: "İşlemler",
//                        width: "120px",
//                        headerAttributes: { style: "text-align: center" },
//                        attributes: { style: "text-align: center" },
//                        template: function (dataItem) {
//                            // Düzenle butonu yetki kontrolü
//                            var editHtml = dataItem.CanEdit
//                                ? `<li><a class='dropdown-item py-2' href='#' onclick='EvrakBekleyenListModule.edit("${dataItem.Id}")'><i class='fas fa-edit text-primary me-2'></i>Düzenle</a></li>`
//                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Düzenle <small>(Yetki Yok)</small></a></li>`;

//                            // Sil butonu yetki kontrolü
//                            var deleteHtml = dataItem.CanEdit
//                                ? `<li><a class='dropdown-item py-2 text-danger' href='#' onclick='EvrakBekleyenListModule.cancel("${dataItem.Id}")'><i class='fas fa-trash-alt me-2'></i>Sil</a></li>`
//                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Sil <small>(Yetki Yok)</small></a></li>`;

//                            return `
//                                <div class='dropdown'>
//                                    <button class='btn btn-link btn-sm p-0 border-0'
//                                            type='button'
//                                            data-bs-toggle='dropdown'
//                                            data-bs-popper-config='{"strategy":"fixed"}'
//                                            aria-expanded='false'
//                                            style='text-decoration: none;'>
//                                        <i class='fas fa-cog text-primary' style='font-size: 18px;'></i>
//                                    </button>
//                                    <ul class='dropdown-menu dropdown-menu-end shadow-lg border-0' style='border-radius: 12px; min-width: 160px;'>
//                                        <li>
//                                            <a class='dropdown-item py-2' href='#' onclick='EvrakBekleyenListModule.onay("${dataItem.Id}")'>
//                                                <i class='fas fa-file-signature text-success me-2'></i>İmzala
//                                            </a>
//                                        </li>
//                                        ${editHtml}
//                                        <li><hr class='dropdown-divider opacity-50'></li>
//                                        ${deleteHtml}
//                                    </ul>
//                                </div>`;
//                        }
//                    },
//                    {
//                        field: "OlusturanKullanici",
//                        title: "Oluşturan",
//                        template: "<div class='d-flex align-items-center'></i>#: OlusturanKullanici #</div>",
//                        width: "120px"
//                    },
//                    { field: "Konu", title: "Konu", width: "200px" },
//                    {
//                        field: "FullKonuKodu",
//                        title: "Konu Kodu",
//                        width: "200px",
//                        template: "<span class='badge bg-light text-dark border'>#: FullKonuKodu #</span>"
//                    },
//                    {
//                        field: "CreatTime",
//                        title: "Oluşturma Zamanı",
//                        width: "250px",
//                        template: "#= kendo.toString(kendo.parseDate(CreatTime), 'dd.MM.yyyy HH:mm') #"
//                    }
//                ],
//                dataSource: { data: [] }
//            }).data("kendoGrid");
//        },

//        loadData: function () {
//            var self = this;
//            _ajaxCall('imza-bekleyen-listele', 'GET').done(function (res) {
//                // Senin mapleme mantığın: API'den gelen küçük harf karmaşasını burada çözüyoruz
//                var list = Array.isArray(res) ? res : (res.data || []);
//                var mappedList = list.map(x => ({
//                    Id: x.id || x.Id,
//                    OlusturanKullanici: x.olusturanKullanici ,
//                    Konu: x.konu ,
//                    FullKonuKodu: x.fullKonuKodu ,
//                    CreatTime: x.creat_time,
//                    CanEdit: x.editYapabilirMi
//                }));
//                _grid.dataSource.data(mappedList);
//            });
//        },

//        onay: function (id) {
//            if (!confirm("Seçili evrakı onaylamak istediğinize emin misiniz?")) {
//                return;
//            }
//            // URL düzeltildi: Parametreyi query string olarak gönderiyoruz (id=5 gibi)
//            // Eğer [FromBody] bekliyorsan data kısmına { id: id } göndermelisin
//            _ajaxCall('Onayla/' + id, 'POST').done(function (response) {


//                // C#'taki IslemSonuc sınıfına göre kontrol yapıyoruz
//                if (response.basariliMi) {
//                    showNotification(response.mesaj, "success");
//                    // Listeyi yenile (Grid kullanıyorsan)
//                    if (EvrakBekleyenListModule && EvrakBekleyenListModule.loadData) {
//                        EvrakBekleyenListModule.loadData();
//                    }
//                } else {
//                    // Servis bazen 200 dönüp başarısız mesajı da verebilir (isteğe bağlı)
//                    showNotification(response.mesaj, "warning");
//                }
//            }).fail(function (err) {
//                // BadRequest(sonuc) döndüğünde buraya düşer
//                if (err.responseJSON) {
//                    showNotification(err.responseJSON.mesaj, "error");
//                }
//            });;
//        },

//        edit: function (id) {

//            window.location.href = '/Home/Index?id=' + id;
//        },

//        cancel: function (id) {
//            if (confirm("Bu evrakı silmek istediğinize emin misiniz?")) {
//                $.ajax({
//                    url: "https://localhost:7060/api/Evrak/EvrakSil/" + id,
//                    type: "DELETE",
//                    success: function (response) {
//                        showNotification(response, "success");
//                        EvrakBekleyenListModule.loadData();
//                    },
//                    error: function (err) {
//                        showNotification(err.responseText, "error");
//                    }
//                });
//            }
//        }
//    };
//})();

//$(document).ready(function () {
//    EvrakBekleyenListModule.init();
//});


//var EvrakBekleyenListModule = (function () {
//    var _grid = null;
//    var _onizlemeDialog = null;
//    var _apiBaseUrl = "https://localhost:7060/api/";

//    var _ajaxCall = function (url, type, data) {
//        return $.ajax({
//            url: _apiBaseUrl + url,
//            type: type,
//            contentType: "application/json",
//            data: data ? JSON.stringify(data) : null,
//            error: function (err) {
//                var msg = "Sistem hatası oluştu.";
//                if (err.responseJSON && err.responseJSON.mesaj) {
//                    msg = err.responseJSON.mesaj;
//                } else if (err.responseText) {
//                    msg = err.responseText;
//                }
//                showNotification(msg, "error");
//            }
//        });
//    };

//    // -------------------------------------------------------
//    // Kendo Dialog'u başlat (sayfa yüklendiğinde bir kez)
//    // -------------------------------------------------------
//    var _initDialog = function () {
//        _onizlemeDialog = $("#onizlemeDialog").kendoDialog({
//            width: "860px",
//            title: "Evrak Önizleme",
//            closable: true,
//            modal: true,
//            visible: false,
//            actions: [
//                { text: "Kapat" },
//                {
//                    text: "PDF İndir",
//                    primary: true,
//                    action: function () {
//                        OnizlemeModule.pdfIndir();
//                        return false; // dialogu kapatma
//                    }
//                }
//            ]
//        }).data("kendoDialog");
//    };

//    return {
//        init: function () {
//            _initDialog();
//            this.initGrid();
//            this.loadData();
//        },

//        initGrid: function () {
//            _grid = $("#gridBekleyenler").kendoGrid({
//                noRecords: { template: "<div class='p-5 text-center text-muted'>İmza bekleyen evrak bulunamadı.</div>" },
//                sortable: true,
//                pageable: { pageSize: 10, buttonCount: 5 },
//                columns: [
//                    {
//                        title: "İşlemler",
//                        width: "120px",
//                        headerAttributes: { style: "text-align: center" },
//                        attributes: { style: "text-align: center" },
//                        template: function (dataItem) {
//                            var editHtml = dataItem.CanEdit
//                                ? `<li><a class='dropdown-item py-2' href='#' onclick='EvrakBekleyenListModule.edit("${dataItem.Id}")'><i class='fas fa-edit text-primary me-2'></i>Düzenle</a></li>`
//                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Düzenle <small>(Yetki Yok)</small></a></li>`;

//                            var deleteHtml = dataItem.CanEdit
//                                ? `<li><a class='dropdown-item py-2 text-danger' href='#' onclick='EvrakBekleyenListModule.cancel("${dataItem.Id}")'><i class='fas fa-trash-alt me-2'></i>Sil</a></li>`
//                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Sil <small>(Yetki Yok)</small></a></li>`;

//                            return `
//                                <div class='dropdown'>
//                                    <button class='btn btn-link btn-sm p-0 border-0'
//                                            type='button'
//                                            data-bs-toggle='dropdown'
//                                            data-bs-popper-config='{"strategy":"fixed"}'
//                                            aria-expanded='false'
//                                            style='text-decoration: none;'>
//                                        <i class='fas fa-cog text-primary' style='font-size: 18px;'></i>
//                                    </button>
//                                    <ul class='dropdown-menu dropdown-menu-end shadow-lg border-0' style='border-radius: 12px; min-width: 160px;'>
//                                        <li>
//                                            <a class='dropdown-item py-2' href='#' onclick='EvrakBekleyenListModule.onay("${dataItem.Id}")'>
//                                                <i class='fas fa-file-signature text-success me-2'></i>İmzala
//                                            </a>
//                                        </li>
//                                        ${editHtml}
//                                        <li><hr class='dropdown-divider opacity-50'></li>
//                                        ${deleteHtml}
//                                    </ul>
//                                </div>`;
//                        }
//                    },
//                    // ── YENİ: Dosyalar kolonu ──────────────────────────────────
//                    {
//                        title: "Dosyalar",
//                        width: "90px",
//                        headerAttributes: { style: "text-align: center" },
//                        attributes: { style: "text-align: center" },
//                        template: function (dataItem) {
//                            return `<button class='btn btn-link btn-sm p-0'
//                                        title='Evrakı Önizle'
//                                        onclick='EvrakBekleyenListModule.onizle("${dataItem.Id}")'>
//                                        <i class='fas fa-file-alt text-secondary' style='font-size:18px;'></i>
//                                    </button>`;
//                        }
//                    },
//                    // ──────────────────────────────────────────────────────────
//                    {
//                        field: "OlusturanKullanici",
//                        title: "Oluşturan",
//                        template: "<div class='d-flex align-items-center'>#: OlusturanKullanici #</div>",
//                        width: "120px"
//                    },
//                    { field: "Konu", title: "Konu", width: "200px" },
//                    {
//                        field: "FullKonuKodu",
//                        title: "Konu Kodu",
//                        width: "200px",
//                        template: "<span class='badge bg-light text-dark border'>#: FullKonuKodu #</span>"
//                    },
//                    {
//                        field: "CreatTime",
//                        title: "Oluşturma Zamanı",
//                        width: "250px",
//                        template: "#= kendo.toString(kendo.parseDate(CreatTime), 'dd.MM.yyyy HH:mm') #"
//                    }
//                ],
//                dataSource: { data: [] }
//            }).data("kendoGrid");
//        },

//        loadData: function () {
//            _ajaxCall('Akis/imza-bekleyen-listele', 'GET').done(function (res) {
//                var list = Array.isArray(res) ? res : (res.data || []);
//                var mappedList = list.map(x => ({
//                    Id: x.id || x.Id,
//                    OlusturanKullanici: x.olusturanKullanici,
//                    Konu: x.konu,
//                    FullKonuKodu: x.fullKonuKodu,
//                    CreatTime: x.creat_time,
//                    CanEdit: x.editYapabilirMi
//                }));
//                _grid.dataSource.data(mappedList);
//            });
//        },

//        // -------------------------------------------------------
//        // Önizleme popup aç — DB'den veri çek, şablonu doldur
//        // -------------------------------------------------------
//        onizle: function (id) {
//            // Loading göster, dialogu aç
//            $("#onizleme-yukleniyor").show();
//            $("#onizleme-icerik").hide();
//            _onizlemeDialog.open();

//            // Evrak içeriğini API'den çek
//            // Endpoint adını kendi controller route'una göre düzenle
//            _ajaxCall('Evrak/EvrakGetir/' + id, 'GET').done(function (evrak) {
//                OnizlemeModule.verileriYukleDB(evrak);
//                $("#onizleme-yukleniyor").hide();
//                $("#onizleme-icerik").show();
//            }).fail(function () {
//                $("#onizleme-yukleniyor").hide();
//                $("#onizleme-icerik").html("<div class='alert alert-danger m-3'>Evrak yüklenemedi.</div>").show();
//            });
//        },

//        onay: function (id) {
//            if (!confirm("Seçili evrakı onaylamak istediğinize emin misiniz?")) return;
//            _ajaxCall('Akis/Onayla/' + id, 'POST').done(function (response) {
//                if (response.basariliMi) {
//                    showNotification(response.mesaj, "success");
//                    EvrakBekleyenListModule.loadData();
//                } else {
//                    showNotification(response.mesaj, "warning");
//                }
//            }).fail(function (err) {
//                if (err.responseJSON) showNotification(err.responseJSON.mesaj, "error");
//            });
//        },

//        edit: function (id) {
//            window.location.href = '/Home/Index?id=' + id;
//        },

//        cancel: function (id) {
//            if (confirm("Bu evrakı silmek istediğinize emin misiniz?")) {
//                $.ajax({
//                    url: "https://localhost:7060/api/Evrak/EvrakSil/" + id,
//                    type: "DELETE",
//                    success: function (response) {
//                        showNotification(response, "success");
//                        EvrakBekleyenListModule.loadData();
//                    },
//                    error: function (err) {
//                        showNotification(err.responseText, "error");
//                    }
//                });
//            }
//        }
//    };
//})();

//$(document).ready(function () {
//    EvrakBekleyenListModule.init();
//});

var EvrakBekleyenListModule = (function () {
    var _grid = null;
    var _onizlemeDialog = null;
    var _apiBaseUrl = "https://localhost:7060/api/";

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

    // -------------------------------------------------------
    // Kendo Dialog'u başlat (sayfa yüklendiğinde bir kez)
    // -------------------------------------------------------
    var _initDialog = function () {
        _onizlemeDialog = $("#onizlemeDialog").kendoDialog({
            width: "1200px", // Genişliği biraz artırdık ki A4 rahat sığsın
            height: "750px",
            title: "Evrak Önizleme",
            closable: true,
            modal: true,
            visible: false,
            actions: [
                { text: "Kapat" },
                {
                    text: "PDF İndir",
                    primary: true,
                    action: function () {
                        // PDF Reader içindeki dosyayı indirtiyoruz
                        OnizlemeModule.pdfIndir();
                        return false;
                    }
                }
            ]
        }).data("kendoDialog");
    };

    return {
        init: function () {
            _initDialog();
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
                            var editHtml = dataItem.CanEdit
                                ? `<li><a class='dropdown-item py-2' href='#' onclick='EvrakBekleyenListModule.edit("${dataItem.Id}")'><i class='fas fa-edit text-primary me-2'></i>Düzenle</a></li>`
                                : `<li><a class='dropdown-item disabled text-muted py-2' href='#'><i class='fas fa-lock me-2'></i>Düzenle <small>(Yetki Yok)</small></a></li>`;

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
                                        <i class='fas fa-cog text-primary' style='font-size: 18px;'></i>
                                    </button>
                                    <ul class='dropdown-menu dropdown-menu-end shadow-lg border-0' style='border-radius: 12px; min-width: 160px;'>
                                        <li>
                                            <a class='dropdown-item py-2' href='#' onclick='EvrakBekleyenListModule.onay("${dataItem.Id}")'>
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
                    // ── YENİ: Dosyalar kolonu ──────────────────────────────────
                    {
                        title: "Dosyalar",
                        width: "90px",
                        headerAttributes: { style: "text-align: center" },
                        attributes: { style: "text-align: center" },
                        template: function (dataItem) {
                            return `<button class='btn btn-link btn-sm p-0'
                                        title='Evrakı Önizle'
                                        onclick='EvrakBekleyenListModule.onizle("${dataItem.Id}")'>
                                        <i class='fas fa-file-alt text-secondary' style='font-size:18px;'></i>
                                    </button>`;
                        }
                    },
                    // ──────────────────────────────────────────────────────────
                    {
                        field: "OlusturanKullanici",
                        title: "Oluşturan",
                        template: "<div class='d-flex align-items-center'>#: OlusturanKullanici #</div>",
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
            _ajaxCall('Akis/imza-bekleyen-listele', 'GET').done(function (res) {
                var list = Array.isArray(res) ? res : (res.data || []);
                var mappedList = list.map(x => ({
                    Id: x.id || x.Id,
                    OlusturanKullanici: x.olusturanKullanici,
                    Konu: x.konu,
                    FullKonuKodu: x.fullKonuKodu,
                    CreatTime: x.creat_time,
                    CanEdit: x.editYapabilirMi
                }));
                _grid.dataSource.data(mappedList);
            });
        },

        // -------------------------------------------------------
        // Önizleme popup aç — DB'den veri çek, şablonu doldur
        // -------------------------------------------------------
        onizle: function (id) {
            var self = this;
            _onizlemeDialog.open(); // Pop-up'ı aç
            $("#onizleme-yukleniyor").show();
            $("#popupDosyaListesi").empty();
            $("#pdf-frame-popup").attr("src", "about:blank");

            $.get('https://localhost:7060/api/Evrak/EvrakGetir/' + id, function (evrak) {
                $("#onizleme-yukleniyor").hide();

                // 1. LİSTEYE "ÜST YAZI"YI EKLE (Senin oluşturduğun şablon)
                var btnUstYazi = $(`
            <a href="#" class="list-group-item list-group-item-action active">
                <div class="d-flex justify-content-between align-items-center">
                    <span><i class="fas fa-file-alt me-2 text-primary"></i>Üst Yazı</span>
                    <span class="badge bg-primary rounded-pill">Asıl</span>
                </div>
            </a>`);

                btnUstYazi.click(function (e) {
                    e.preventDefault();
                    $(this).addClass("active").siblings().removeClass("active");
                    // Senin o meşhur HTML-to-PDF motorunu popup'taki iframe için çalıştırır
                    OnizlemeModule.verileriYukleDB(evrak, "#pdf-frame-popup");
                });
                $("#popupDosyaListesi").append(btnUstYazi);

                // 2. LİSTEYE "EKLERİ" EKLE (Varsa)
                if (evrak.ekler && evrak.ekler.length > 0) {
                    evrak.ekler.forEach(function (ek, index) {
                        var btnEk = $(`
                    <a href="#" class="list-group-item list-group-item-action">
                        <i class="fas fa-paperclip me-2 text-secondary"></i>Ek-${index + 1}: ${ek.ad || ek.Ad}
                    </a>`);

                        btnEk.click(function (e) {
                            e.preventDefault();
                            $(this).addClass("active").siblings().removeClass("active");
                            // Eklerin (PDF veya Resim) gösterimi
                            self.ekOnizle(ek);
                        });
                        $("#popupDosyaListesi").append(btnEk);
                    });
                }

                // 3. İLK AÇILIŞ: Varsayılan olarak "Üst Yazı"yı göster
                OnizlemeModule.verileriYukleDB(evrak, "#pdf-frame-popup");

            }).fail(function () {
                alert("Evrak detayları alınamadı.");
                $("#onizleme-yukleniyor").hide();
            });
        },
        ekOnizle: function (ek) {
            var base64Data = ek.dosyaIcerik || ek.DosyaIcerik;
            if (!base64Data) return;

            var contentType = ek.mimeType || ek.MimeType || "application/pdf";

            // Eğer PDF ise direkt Iframe'e göm (Siyah barlı arayüz gelir)
            if (contentType.includes("pdf")) {
                $("#pdf-frame-popup").attr("src", "data:application/pdf;base64," + base64Data + "#toolbar=1");
            } else {
                // Resim ise Base64 URL oluşturup bas
                $("#pdf-frame-popup").attr("src", "data:" + contentType + ";base64," + base64Data);
            }
        },

        

        onay: function (id) {
            if (!confirm("Seçili evrakı onaylamak istediğinize emin misiniz?")) return;
            _ajaxCall('Akis/Onayla/' + id, 'POST').done(function (response) {
                if (response.basariliMi) {
                    showNotification(response.mesaj, "success");
                    EvrakBekleyenListModule.loadData();
                } else {
                    showNotification(response.mesaj, "warning");
                }
            }).fail(function (err) {
                if (err.responseJSON) showNotification(err.responseJSON.mesaj, "error");
            });
        },

        edit: function (id) {
            window.location.href = '/Home/Index?id=' + id;
        },

        cancel: function (id) {
            if (confirm("Bu evrakı silmek istediğinize emin misiniz?")) {
                $.ajax({
                    url: "https://localhost:7060/api/Evrak/EvrakSil/" + id,
                    type: "DELETE",
                    success: function (response) {
                        showNotification(response, "success");
                        EvrakBekleyenListModule.loadData();
                    },
                    error: function (err) {
                        showNotification(err.responseText, "error");
                    }
                });
            }
        }
    };
})();

$(document).ready(function () {
    EvrakBekleyenListModule.init();
});

