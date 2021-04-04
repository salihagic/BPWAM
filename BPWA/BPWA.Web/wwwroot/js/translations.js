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
        this.shouldSubmit = true;
        this.loadFromLocalStorage();
    }

    saveToLocalStorage() {
        window.localStorage.setItem(this.localStorageKey, JSON.stringify(this.translations));
    }

    submit() {
        var filteredTranslations = this.translations.filter((x) => x.culture != null && x.key != null && x.value != null);
        
        if (filteredTranslations.length > 0 && this.shouldSubmit) {

            $.ajax({
                url: '/Translations/AddOrUpdateRange',
                type: 'POST',
                data: {
                    translations: filteredTranslations
                },
                success: (data) => {
                    this.reset();
                    $('.datatable_search_form').submit();
                }
            });
        }
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

    findByCultureAndKeyId(culture, keyId) {
        return this.translations.find(
            x => x.culture === culture && x.keyId === keyId
        );
    }

    findByCultureAndKey(culture, key) {
        return this.translations.find(
            x => x.culture === culture && x.key === key
        );
    }
}

var translationsManager = new TranslationsManager();