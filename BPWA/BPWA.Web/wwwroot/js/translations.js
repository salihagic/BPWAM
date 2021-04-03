class Translation {
    constructor(keyId, key, culture, value) {
        this.keyId = keyId;
        this.key = key;
        this.culture = culture;
        this.value = value;
    }
}

class TranslationsManager {
    static localStorageKey = "__translations__";

    constructor() {
        this.loadFromLocalStorage();
        if (this.translations.length > 0) {
            this.submit();
        }
    }

    saveToLocalStorage() {
        window.localStorage.setItem(this.localStorageKey, JSON.stringify(this.translations));
    }

    submit() {
        console.log('Sending to the database');

        $.ajax({
            url: '/Translations/AddRange',
            type: 'POST',
            data: {
                translations: this.translations
            },
            success: (data) => {
                this.reset();
            }
        });
    }

    reset() {
        window.localStorage.removeItem(this.localStorageKey);
        this.translations = [];
    }

    loadFromLocalStorage() {
        var localStorageTranslations = window.localStorage.getItem(this.localStorageKey);

        if (localStorageTranslations != null) {
            this.translations = JSON.parse(localStorageTranslations);
        } else {
            this.translations = [];
        }
    }

    addOrUpdate(translation) {
        var existingTranslationIndex = this.translations.findIndex(
            x => x.culture === translation.culture && x.keyId === translation.keyId
        );

        if (existingTranslationIndex > -1) {
            this.translations[existingTranslationIndex] = translation;
        } else {
            this.translations.push(translation);
        }

        this.saveToLocalStorage();
    }

    findByCultureAndKey(culture, key) {
        return this.translations.find(
            x => x.culture === culture && x.key === key
        );
    }

    findByCultureAndKeyId(culture, keyId) {
        return this.translations.find(
            x => x.culture === culture && x.keyId === keyId
        );
    }
}

var translationsManager = new TranslationsManager();

$(document).on('change', '.translation_item', function (e) {
    var keyId = $(this).data('keyid');
    var key = $(keyId).val();
    var culture = $(this).data('culture');
    var value = $(this).val();

    translationsManager.addOrUpdate(new Translation(keyId, key, culture, value));
});