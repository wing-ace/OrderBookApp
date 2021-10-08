ko.bindingHandlers.inputmask = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        const obj = allBindingsAccessor();
        $(element).inputmask(obj.settings);

        const eventHandlerType = obj.eventHandlerType !== undefined ? obj.eventHandlerType : 'focusout';

        ko.utils.registerEventHandler(element, eventHandlerType, function () {
            const observable = valueAccessor();
            var value = $(element).val();
            const charactersdelete = allBindingsAccessor().charactersToRemove;

            if (value !== null && value !== undefined && charactersdelete !== null && charactersdelete !== undefined) {
                for (let i = 0; i < charactersdelete.length; i++) {
                    value = value.toString().split(charactersdelete[i]).join('');
                }
                value = value.toString().split('_').join('');
            }

            observable(value);
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        const observable = valueAccessor();
        const charactersdelete = allBindingsAccessor().charactersToRemove;

        if (value !== null && value !== undefined && charactersdelete !== null && charactersdelete !== undefined) {
            for (let i = 0; i < charactersdelete.length; i++) {
                value = value.toString().split(charactersdelete[i]).join('');
            }
            value = value.toString().split('_').join('');
        }

        observable(value);
        $(element).val(value);
    }
};

ko.bindingHandlers.numericText = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            precision = ko.utils.unwrapObservable(allBindingsAccessor().precision) || ko.bindingHandlers.numericText.defaultPrecision,
            formattedValue = value.toFixed(precision);

        ko.bindingHandlers.text.update(element, function () { return formattedValue; });
    },
    defaultPrecision: 1
};