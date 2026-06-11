_apiBaseUrl = "https://localhost:7060/api/";

var ApiService = {

    getJson: function (url) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: "GET",
            contentType: "application/json",
            error: this._handleError
        });
    },


    // 📌 1. Standart JSON ve Listeleme İşleri İçin (İmza Bekleyenler vb.)
    postJson: function (url, data) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: "POST",
            contentType: "application/json",
            data: data ? JSON.stringify(data) : null,
            error: this._handleError
        });
    },

    // 📌 2. Dosya/Evrak Gönderilen FormData İşleri İçin (contentType: false olanlar)
    postFormData: function (url, formData) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            error: this._handleError
        });
    },

    delete: function (url) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: "DELETE",
            contentType: "application/json",
            error: this._handleError
        });
    },

    // Ortak Hata Yakalayıcı (Metotların içine tekrar tekrar yazmıyoruz)
    _handleError: function (err) {
        var msg = "Sistem hatası oluştu.";
        if (err && err.responseJSON && err.responseJSON.mesaj) {
            msg = err.responseJSON.mesaj;
        } else if (err && err.responseText) {
            msg = err.responseText;
        }
        showNotification(msg, "error");
    }
};