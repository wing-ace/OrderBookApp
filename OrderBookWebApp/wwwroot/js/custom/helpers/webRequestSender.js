const webRequestSender = {
    sendGetRequest: async function (url) {
        try {
            const result = await $.ajax({
                url: url,
                method: "GET"
            });

            if (!result.isSuccess) {
                throw result.errorModel;
            }

            return result;
        } catch (error) {
            this._handleError(error);
            return null;
        }
    },

    _handleError: function (errorModel) {
        console.log(errorModel);
        //todo some custom logic with modal windows, page reloading or anything else
    }
}