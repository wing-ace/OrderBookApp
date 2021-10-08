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

    vm.BlockUiUntilOrdersDataIsReceived = async function () {
        if (vm.BuyOrders.OrdersData().length === 0 && vm.SellOrders.OrdersData().length === 0) {
            await vm.Filter.ApplyFilter();
            setTimeout(vm.BlockUiUntilOrdersDataIsReceived, 2000);
            return;
        }
    }

    vm.SellOrders = new function() {
        this.Title = ko.observable("Sell Orders");
        this.Type = ko.observable(vm.Config.OrderTypes.SellOrders);
        this.OrdersData = ko.observableArray([]);
        this.RefreshData = async function (selectedPage) {
            await refresOrdersData(vm.SellOrders, selectedPage, vm.SellOrders.Type());
        };
        this.PagerOptions = createPagerOptions(this.RefreshData);
        this.Pager = new KnockoutPagingViewModel(this.PagerOptions);
    };

    vm.BuyOrders = new function () {
        this.Title = ko.observable("Buy Orders");
        this.Type = ko.observable(vm.Config.OrderTypes.BuyOrders);
        this.OrdersData = ko.observableArray([]);
        this.RefreshData = async function (selectedPage) {
            await refresOrdersData(vm.BuyOrders, selectedPage, vm.BuyOrders.Type());
        };
        this.PagerOptions = createPagerOptions(this.RefreshData);
        this.Pager = new KnockoutPagingViewModel(this.PagerOptions);
    };

    async function refresOrdersData(ordersModel, selectedPage, orderType) {
        PageLoader.ShowLoader(vm.LoaderContainer);
        const ordersData = await webRequestSender.sendGetRequestAsync(buildUrlForFetchData(orderType, selectedPage));
        if (!ordersData) {
            PageLoader.HideLoader(vm.LoaderContainer);
            return;
        }

        updatePagerData(ordersModel.Pager, ordersData.data.pageNumber, ordersData.data.totalItemsCount);
        updateArrayValues(ordersModel.OrdersData, ordersData.data.ordersList);
        PageLoader.HideLoader(vm.LoaderContainer);
    };

    function buildUrlForFetchData(orderType, selectedPage) {
        const depthValue = vm.Filter.DepthValue() ? vm.Filter.DepthValue() : 0;
        return `${vm.Config.Urls.GetOrdersDataByType}?orderType=${orderType}&depthValue=${depthValue}&pageNumber=${selectedPage}&pageSize=10`
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
        vm.BlockUiUntilOrdersDataIsReceived();
    };

    constructor();
}