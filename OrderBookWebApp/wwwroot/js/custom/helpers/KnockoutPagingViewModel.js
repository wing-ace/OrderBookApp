function KnockoutPagingViewModel(options) {
    var self = this;

    self.PageSize = ko.observable(options.PageSize || 10);

    self.CurrentPage = ko.observable(options.CurrentPage || 1);

    self.TotalCount = ko.observable(options.TotalCount || 0);

    self.PageCount = ko.computed(function () {
        return Math.ceil(self.TotalCount() / self.PageSize());
    }, self);

    self.FirstNumber = ko.computed(function () {

        if (self.TotalCount() === 0)
            return 0;

        return (self.CurrentPage() - 1) * self.PageSize() + 1;
    }, self);

    self.LastNumber = ko.computed(function () {

        if (self.TotalCount() === 0)
            return 0;

        if (self.TotalCount() < self.CurrentPage() * self.PageSize())
            return self.TotalCount();

        return self.CurrentPage() * self.PageSize();
    }, self);

    self.SetCurrentPage = function (page) {
        if (page < self.FirstPage)
            page = self.FirstPage;

        if (page > self.LastPage())
            page = self.LastPage();

        self.CurrentPage(page);

        if (!options.SelectedPageCallback)
            throw 'SelectedPageCallback is not defined!';

        options.SelectedPageCallback(page);
    };

    self.FirstPage = 1;

    self.LastPage = ko.computed(function () {
        return self.PageCount();
    }, self);

    self.NextPage = ko.computed(function () {
        var next = self.CurrentPage() + 1;
        if (next > self.LastPage())
            return null;
        return next;
    }, self);

    self.PreviousPage = ko.computed(function () {
        var previous = self.CurrentPage() - 1;
        if (previous < self.FirstPage)
            return null;
        return previous;
    }, self);

    self.NeedPaging = ko.computed(function () {
        return self.PageCount() > 1;
    }, self);

    self.NextPageActive = ko.computed(function () {
        return self.NextPage() != null;
    }, self);

    self.PreviousPageActive = ko.computed(function () {
        return self.PreviousPage() != null;
    }, self);

    self.LastPageActive = ko.computed(function () {
        return (self.LastPage() != self.CurrentPage());
    }, self);

    self.FirstPageActive = ko.computed(function () {
        return (self.FirstPage != self.CurrentPage());
    }, self);

    var maxPageCount = 7;

    self.generateAllPages = function () {
        var pages = [];
        for (var i = self.FirstPage; i <= self.LastPage(); i++)
            pages.push(i);

        return pages;
    };

    self.generateMaxPage = function () {
        var current = self.CurrentPage();
        var pageCount = self.PageCount();
        var first = self.FirstPage;

        var upperLimit = current + parseInt((maxPageCount - 1) / 2);
        var downLimit = current - parseInt((maxPageCount - 1) / 2);

        while (upperLimit > pageCount) {
            upperLimit--;
            if (downLimit > first)
                downLimit--;
        }

        while (downLimit < first) {
            downLimit++;
            if (upperLimit < pageCount)
                upperLimit++;
        }

        var pages = [];
        for (var i = downLimit; i <= upperLimit; i++) {
            pages.push(i);
        }
        return pages;
    };

    self.GetPages = ko.computed(function () {
        self.CurrentPage();
        self.TotalCount();

        if (self.PageCount() <= maxPageCount) {
            return ko.observableArray(self.generateAllPages());
        } else {
            return ko.observableArray(self.generateMaxPage());
        }
    }, self);

    self.Update = function (e) {
        self.TotalCount(e.TotalCount);
        self.PageSize(e.PageSize);
        self.SetCurrentPage(e.CurrentPage);
    };

    self.GoToFirst = function () {
        self.SetCurrentPage(self.FirstPage);
    };

    self.GoToPrevious = function () {
        var previous = self.PreviousPage();
        if (previous != null)
            self.SetCurrentPage(previous);
    };

    self.GoToNext = function () {
        var next = self.NextPage();
        if (next != null)
            self.SetCurrentPage(next);
    };

    self.GoToLast = function () {
        self.SetCurrentPage(self.LastPage());
    };
}