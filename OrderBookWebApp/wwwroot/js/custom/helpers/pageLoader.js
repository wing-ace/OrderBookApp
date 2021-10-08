PageLoader = new function () {
    const self = this;

    self.ShowLoader = function (container) {
        if (!container)
            return;

        container.css("position", "relative");

        const mask = '<div id="spinner-block" style="position: absolute; width: 100%; top: 0; height: 100%; display: flex; align-items: center; background: #ffffffb8; z-index: 999;">' +
            '<div class="text-center" style="margin: 50%;">' +
            '<div class="spinner-border" role="status">' +
            '<span class="sr-only">Loading...</span>' +
            '</div>' +
            '</div>' +
            '</div>';

        container.append(mask);
    };

    self.HideLoader = function (container) {
        if (!container)
            return;

        container.css("position", "inherit");
        $("#spinner-block").remove();
    };
};