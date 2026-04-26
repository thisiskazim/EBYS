//var OnizlemeModule = (function () {
//    var _imzaciAd = "";
//    var _imzaciUnvan = "";

//    return {
//        setImzaci: function (ad, unvan) {
//            _imzaciAd = ad;
//            _imzaciUnvan = unvan;
//        },

//        verileriYukle: function () {
//            try {
//                // 1. Tüm modüllerden güncel verileri topla
//                var bilgiler = EvrakBilgiModule.getData();
//                var alicilar = AliciModule.getData() || [];
//                var ilgiler = (typeof IlgilerModule !== "undefined") ? IlgilerModule.getData() : [];
//                var ekler = (typeof EklerModule !== "undefined") ? EklerModule.getData() : [];
//                console.log("veriler:", bilgiler, alicilar, ilgiler, ekler);
//                // 2. Temel Alanları Bas
//                $("#view-sayi").text("11428951-" + (bilgiler.KonuKoduId || "000"));
//                $("#view-konu").text(bilgiler.Konu || "");

//                // Kendo Editor İçerikleri
//                var editorGövde = $("#EvrakEditor").data("kendoEditor");
//                if (editorGövde) $("#view-icerik").html(editorGövde.value());

//                var editorAlt = $("#AltMetinEditor").data("kendoEditor");
//                if (editorAlt) $("#view-imza-alti-icerik").html(editorAlt.value());

//                // 3. İlgiler (Resmi format: a, b, c...)
//                if (ilgiler.length > 0) {
//                    var ilgiHtml = "<strong>İlgi :</strong> ";
//                    ilgiler.forEach((i, idx) => {
//                        var harf = String.fromCharCode(97 + idx) + ")";
//                        ilgiHtml += (idx === 0 ? "" : "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + harf + " " + i.IlgiMetni;
//                    });
//                    $("#view-ilgiler-alani").html(ilgiHtml).show();
//                } else { $("#view-ilgiler-alani").hide(); }

//                // 4. Muhatap ve Dağıtım Mantığı
//                if (alicilar.length > 1) {
//                    $("#view-muhatap-baslik").html("<strong>DAĞITIM YERLERİNE</strong>");
//                    $("#view-dagitim-alani").show();

//                    var geregiHtml = "<strong>Gereği:</strong><br>", bilgiHtml = "<strong>Bilgi:</strong><br>";
//                    var gVar = false, bVar = false;

//                    alicilar.forEach(a => {
//                        if (a.IsBilgi) { bilgiHtml += "- " + a.MuhatapAdi + "<br>"; bVar = true; }
//                        else { geregiHtml += "- " + a.MuhatapAdi + "<br>"; gVar = true; }
//                    });
//                    $("#view-geregi-listesi").html(gVar ? geregiHtml : "").toggle(gVar);
//                    $("#view-bilgi-listesi").html(bVar ? bilgiHtml : "").toggle(bVar);
//                } else if (alicilar.length === 1) {
//                    $("#view-muhatap-baslik").html("<strong>" + alicilar[0].MuhatapAdi.toUpperCase() + "</strong>");
//                    $("#view-dagitim-alani").hide();
//                }

//                // 5. Dinamik İmza (Rotadan gelen)
//                $("#view-imza-ad").text((_imzaciAd || "").toUpperCase());
//                $("#view-imza-unvan").text(_imzaciUnvan || "");

//                // 6. Ekler
//                if (ekler.length > 0) {
//                    var ekHtml = "<strong>Ek :</strong><br>";
//                    ekler.forEach((e, idx) => {
//                        ekHtml += (idx + 1) + "- " + e.Ad + "<br>";
//                    });
//                    $("#view-ekler-listesi").html(ekHtml).show();
//                } else { $("#view-ekler-listesi").hide(); }

//            } catch (err) {
//                console.error("Önizleme yüklenirken hata oluştu:", err);
//            }
//        },

//        pdfIndir: function () {
//            kendo.drawing.drawDOM($(".a4-sayfa"), {
//                paperSize: "A4",
//                margin: { top: "1cm", bottom: "1cm" },
//                scale: 0.8,
//                avoidLinks: true
//            })
//                .then(function (group) {
//                    return kendo.drawing.exportPDF(group);
//                })
//                .done(function (data) {
//                    kendo.saveAs({
//                        dataURI: data,
//                        fileName: "Evrak_Onizleme.pdf"
//                    });
//                });
//        }
//    };
//})();



var OnizlemeModule = (function () {
    var _imzaciAd = "";
    var _imzaciUnvan = "";

    // GidenEvrakUpdateDTO -> şablona bas
    var _doldur = function (evrak) {
        // Sayı
        $("#view-sayi").text("11428951-" + (evrak.konuKoduId || evrak.KonuKoduId || "000"));
      
        // Konu
        $("#view-konu").text(evrak.konu || evrak.Konu || "");

        // Ana metin
        $("#view-icerik").html(evrak.icerik || evrak.Icerik || "");

        // İmza altı içerik
        var altMetin = evrak.imzaAltindaOlanIcerik || evrak.ImzaAltindaOlanIcerik || "";
        // HTML'de id'yi ikiye ayırdık: view-imza-alti-icerik-ust ve -alt
        // Eğer eski tek id kullanıyorsan ikisine de bas, yoksa istediğini seç
        $("#view-imza-alti-icerik-ust").html(altMetin);
        $("#view-imza-alti-icerik-alt").html("");

        // İlgiler
        var ilgiler = evrak.ilgiler || evrak.Ilgiler || [];
        if (ilgiler.length > 0) {
            var ilgiHtml = "<strong>İlgi :</strong> ";
            ilgiler.forEach(function (i, idx) {
                var harf = String.fromCharCode(97 + idx) + ")";
                ilgiHtml += (idx === 0 ? "" : "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + harf + " " + (i.ilgiMetni || i.IlgiMetni || "");
            });
            $("#view-ilgiler-alani").html(ilgiHtml).show();
        } else {
            $("#view-ilgiler-alani").hide();
        }

        // Muhataplar
        var muhataplar = evrak.muhataplar || evrak.Muhataplar || [];
        if (muhataplar.length > 1) {
            $("#view-muhatap-baslik").html("<strong>DAĞITIM YERLERİNE</strong>");
            $("#view-dagitim-alani").show();

            var geregiHtml = "<strong>Gereği:</strong><br>", bilgiHtml = "<strong>Bilgi:</strong><br>";
            var gVar = false, bVar = false;

            muhataplar.forEach(function (a) {
                var adi = a.adi || a.MuhatapAdi || "";
                var isBilgi = a.isBilgi || a.IsBilgi || false;
                if (isBilgi) { bilgiHtml += "- " + adi + "<br>"; bVar = true; }
                else { geregiHtml += "- " + adi + "<br>"; gVar = true; }
            });

            $("#view-geregi-listesi").html(gVar ? geregiHtml : "").toggle(gVar);
            $("#view-bilgi-listesi").html(bVar ? bilgiHtml : "").toggle(bVar);
        } else if (muhataplar.length === 1) {
            var tekAdi = (muhataplar[0].adi || muhataplar[0].adi || "").toUpperCase();
            $("#view-muhatap-baslik").html("<strong>" + tekAdi + "</strong>");
            $("#view-dagitim-alani").hide();
        } else {
            $("#view-muhatap-baslik").html("");
            $("#view-dagitim-alani").hide();
        }

        // İmza
        $("#view-imza-ad").text((_imzaciAd || "").toUpperCase());
        $("#view-imza-unvan").text(_imzaciUnvan || "");

        // Ekler
        var ekler = evrak.ekler || evrak.Ekler || [];
        if (ekler.length > 0) {
            var ekHtml = "<strong>Ek :</strong><br>";
            ekler.forEach(function (e, idx) {
                ekHtml += (idx + 1) + "- " + (e.ad || e.Ad || "") + "<br>";
            });
            $("#view-ekler-listesi").html(ekHtml).show();
        } else {
            $("#view-ekler-listesi").hide();
        }
    };

    return {
        setImzaci: function (ad, unvan) {
            _imzaciAd = ad;
            _imzaciUnvan = unvan;
        },

        // ── Evrak Oluşturma sayfası: form/editor'dan okur ──────────────────
        verileriYukle: function () {
            try {
                var bilgiler = EvrakBilgiModule.getData();
                var alicilar = AliciModule.getData() || [];
                var ilgiler = (typeof IlgilerModule !== "undefined") ? IlgilerModule.getData() : [];
                var ekler = (typeof EklerModule !== "undefined") ? EklerModule.getData() : [];
                console.log("veriler", bilgiler, alicilar)
                // DTO formatına çevir, ortak _doldur'a gönder
                var editorGövde = $("#EvrakEditor").data("kendoEditor");
                var editorAlt = $("#AltMetinEditor").data("kendoEditor");

                var evrakObj = {
                    konuKoduId: bilgiler.KonuKoduId,
                    konu: bilgiler.Konu,
                    icerik: editorGövde ? editorGövde.value() : "",
                    imzaAltindaOlanIcerik: editorAlt ? editorAlt.value() : "",
                    muhataplar: alicilar.map(function (a) {
                        return { adi: a.Adi, isBilgi: a.IsBilgi };
                    }),
                    ilgiler: ilgiler.map(function (i) {
                        return { ilgiMetni: i.IlgiMetni };
                    }),
                    ekler: ekler.map(function (e) {
                        return { ad: e.Ad };
                    })
                };

                _doldur(evrakObj);
            } catch (err) {
                console.error("Önizleme yüklenirken hata oluştu:", err);
            }
        },

        // ── İmza Bekleyen Liste sayfası: API'den gelen DTO'yu doğrudan basar ──
        verileriYukleDB: function (evrak) {
            try {
                _doldur(evrak);
                console.log("dbden çek", evrak);
            } catch (err) {
                console.error("DB önizleme yüklenirken hata oluştu:", err);
            }
        },

        pdfIndir: function () {
            kendo.drawing.drawDOM($(".a4-sayfa"), {
                paperSize: "A4",
                margin: { top: "1cm", bottom: "1cm" },
                scale: 0.8,
                avoidLinks: true
            })
                .then(function (group) {
                    return kendo.drawing.exportPDF(group);
                })
                .done(function (data) {
                    kendo.saveAs({
                        dataURI: data,
                        fileName: "Evrak_Onizleme.pdf"
                    });
                });
        }
    };
})();