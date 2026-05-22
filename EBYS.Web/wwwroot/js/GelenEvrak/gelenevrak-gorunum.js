var GelenOnizlemeModule = (function () {
    var _asilEvrakFile = null;

    return {

        init: function () {
      
            var self = this;
            $("#AsilEvrakDosya").off("change").on("change", function () {
                self.dosyaSecildi(this);
            });
        },
   
        dosyaSecildi: function (input) {
            if (input.files && input.files[0]) {
                _asilEvrakFile = input.files[0];

            
                $("#no-file-message").hide();
                $("#pdf-preview-frame").show();

                var fileURL = URL.createObjectURL(_asilEvrakFile);
                $("#pdf-preview-frame").attr("src", fileURL);

                if (typeof showNotification !== "undefined") {
                    showNotification("Üst yazı önizlemeye yüklendi.", "info");
                }
            }
        },

       
        getAsilEvrak: function () {
            return _asilEvrakFile;
        }
    };
})();

$(document).ready(function () {
    GelenOnizlemeModule.init();
});