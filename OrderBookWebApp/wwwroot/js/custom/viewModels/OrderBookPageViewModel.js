// Knockout view model for order book page
function OrderBookPageViewModel(config) {
    var vm = this;
    vm.Config = config;
    vm.LoaderContainer = $('#order-book-container');
    vm.PageSize = 10;
    vm.DefaultPageNumber = 1;

    vm.Filter = {
        DepthValue: ko.observable(null),
        ApplyFilter: async function () {
            PageLoader.ShowLoader(vm.LoaderContainer);
            await vm.SellOrders.RefreshData(vm.DefaultPageNumber);
            await vm.BuyOrders.RefreshData(vm.DefaultPageNumber);
            PageLoader.HideLoader(vm.LoaderContainer);
        }
    };

    vm.SellOrders = new function() {
        this.Title = ko.observable("Sell Orders");
        this.OrdersData = ko.observableArray([]);
        this.RefreshData = async function (selectedPage) {
            await refresOrdersData(vm.SellOrders, selectedPage, vm.Config.Urls.GetSellOrdersData);
        };
        this.PagerOptions = createPagerOptions(this.RefreshData);
        this.Pager = new KnockoutPagingViewModel(this.PagerOptions);
    };

    vm.BuyOrders = new function () {
        this.Title = ko.observable("Buy Orders");
        this.OrdersData = ko.observableArray([]);
        this.RefreshData = async function (selectedPage) {
            await refresOrdersData(vm.BuyOrders, selectedPage, vm.Config.Urls.GetBuyOrdersData);
        };
        this.PagerOptions = createPagerOptions(this.RefreshData);
        this.Pager = new KnockoutPagingViewModel(this.PagerOptions);
    };

    async function refresOrdersData(ordersModel, selectedPage, baseUrl) {
        PageLoader.ShowLoader(vm.LoaderContainer);
        const ordersData = await webRequestSender.sendGetRequest(buildUrlForFetchData(baseUrl, selectedPage));
        if (!ordersData) {
            PageLoader.HideLoader(vm.LoaderContainer);
            return;
        }

        updatePagerData(ordersModel.Pager, ordersData.data.pageNumber, ordersData.data.totalItemsCount);
        updateArrayValues(ordersModel.OrdersData, ordersData.data.ordersList);
        PageLoader.HideLoader(vm.LoaderContainer);
    };

    function buildUrlForFetchData(baseUrl, selectedPage) {
        const depthValue = vm.Filter.DepthValue() ? vm.Filter.DepthValue() : 0;
        return `${baseUrl}?depthValue=${depthValue}&pageNumber=${selectedPage}&pageSize=10`
    };

    function updatePagerData(pager, currentPage, totalItemsCount) {
        pager.CurrentPage(currentPage);
        pager.TotalCount(totalItemsCount);
    };

    function createPagerOptions(onSelectPageCallback) {
        return {
            PageSize: vm.PageSize,
            CurrentPage: vm.DefaultPageNumber,
            TotalCount: 0,
            SelectedPageCallback: function (selectedPage) {
                onSelectPageCallback(selectedPage);
            }
        };
    };

    function updateArrayValues(targetArray, newData) {
        targetArray.removeAll();

        ko.utils.arrayForEach(newData || [],
            function (item) {
                targetArray.push(item);
            });
    };

    function constructor() {
        vm.Filter.ApplyFilter(vm.DefaultPageNumber);
    };

    constructor();
}