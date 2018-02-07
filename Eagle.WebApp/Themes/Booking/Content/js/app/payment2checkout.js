$(function () {
    // Called when token created successfully.
    var successCallback = function (data) {
        var myForm = document.getElementById('myCCForm');

        // Set the token as the value for the token input
        myForm.token.value = data.response.token.token;

        // IMPORTANT: Here we call `submit()` on the form element directly instead of using jQuery to prevent and infinite token request loop.
        myForm.submit();
    };

    // Called when token creation fails.
    var errorCallback = function (data) {
        if (data.errorCode === 200) {
            // This error code indicates that the ajax call failed. We recommend that you retry the token request.
        } else {
            alert(data.errorMsg);
        }
    };

    var tokenRequest = function (sellerId, publicKey, ccNo, cvv, expMonth, expYear, callback, errorCallback) {
        // Setup token request arguments
        var args = {
            sellerId: sellerId,
            publishableKey: publicKey,
            ccNo: ccNo,
            cvv: cvv,
            expMonth: expMonth,
            expYear: expYear
        };

        // Make the token request
        TCO.requestToken(callback, errorCallback, args);
    };

    // Pull in the public encryption key for our environment
    TCO.loadPubKey('sandbox');

    //$("#purchase-form").submit(function (e) {
    //    // Call our token request function
    //    tokenRequest();

    //    // Prevent form from submitting
    //    return false;
    //});
});