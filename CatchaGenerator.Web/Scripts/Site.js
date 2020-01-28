/*
 * CathPCIWhiskerApplication JS
 */

if (typeof (jQuery) === "undefined")
    throw new Error("Cannot run without jQuery!!!!");

var captchaGeneratorModule = new function () {
    var utility = {
        objectToQueryString: function (obj) {
            var keys = Object.keys(obj);
            var result = "?";
            for (var i = 0; i < keys.length; i++) {
                result += keys[i] + "=" + obj[keys[i]];
                if (i !== keys.length - 1) {
                    result += "&";
                }
            }
            return result;
        }
    }
    var privateModule = {
        resultContainerSelector: ".result",
        resultViewContainerSelector: ".result-view",
        submitButtonSelector: ".submit-button",
        clearButtonSelector: ".clear-button",
        inputElementIdSelector: '#form > .input-element > input[type="text"]',
        selectElementIdSelector: "#form > .input-element > select",
        formElementIdSelector: "#form",
        captchaEndpoint: (window.pathBase ? window.pathBase : "/") + "api/captchagenerator/getcaptcha",
        getData: function () {
            var inputValues = {}
            var inputElements = jQuery(privateModule.inputElementIdSelector);
            for (var i = 0; i < inputElements.length; i++) {
                var inputElement = jQuery(inputElements[i]);
                inputValues[inputElement.attr("id")] = inputElement.val() === null ? "" : inputElement.val();
            }
            var selectElements = jQuery(privateModule.selectElementIdSelector);
            for (var j = 0; j < selectElements.length; j++) {
                var optionElement = jQuery(selectElements[j]);
                inputValues[optionElement.attr("id")] = optionElement.val() === null ? "" : optionElement.val();
            }
            return inputValues;
        },
        transformCaptchaData: function(data) {
            if (data.numberOfCharacters && data.numberOfCharacters === "0") {
                return { message: data.message }
            } else {
                return { numberOfCharacters: data.numberOfCharacters }
            }
        },
        submitSuccess: function (data) {
            jQuery(privateModule.resultContainerSelector).html('<img src="data:image/bmp;base64,' + data['CaptchaImage@'] + '" />');
            jQuery(privateModule.resultViewContainerSelector).show();
        },
        submitError: function (xhr, status, error) {
            jQuery(privateModule.resultContainerSelector).text(error);
            jQuery(privateModule.resultViewContainerSelector).show();
        },
        clickSubmitButtonEvent: function () {
            if (jQuery(privateModule.formElementIdSelector).valid()) {
                jQuery(privateModule.resultViewContainerSelector).hide();
                var data = privateModule.getData();
                data = privateModule.transformCaptchaData(data);
                var url = privateModule.captchaEndpoint + utility.objectToQueryString(data);
                jQuery.ajax({
                    url: url,
                    type: "GET",
                    mimeType: "application/json;",
                    success: privateModule.submitSuccess,
                    error: privateModule.submitError
                });
            }
        },
        clickClearButtonEvent: function () {
            jQuery(privateModule.resultContainerSelector).html("");
            jQuery(privateModule.resultViewContainerSelector).hide();
        }
    }
    var module = {
        init: function () {
            jQuery(document)
            .ready(function () {
                jQuery(privateModule.submitButtonSelector).click(privateModule.clickSubmitButtonEvent);
                jQuery(privateModule.clearButtonSelector).click(privateModule.clickClearButtonEvent);
                jQuery(privateModule.resultViewContainerSelector).hide();
                jQuery(privateModule.formElementIdSelector).validate();
            });
        }
    }
    return module;
}

// initialize
captchaGeneratorModule.init();