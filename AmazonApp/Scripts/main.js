
$(document).ready(function () {

    $('.previous_button').click(loadPreviousPage);
    $('.next_button').click(loadNextPage);

    $('.exchange_rate_select').change(updatePrices);

    getExchangeRates();
    getCategories();

    var activePage = 0;
    var totalPages = 0;

    var dataPages = {};

    var defaultExchangeRate;
    var moreResultsUrl;

    var canRetrieveAllProducts;

    $('.search_button').on('click', function () {
        loadPage();
    });

    function loadPage() {

        hideNextButton();
        hidePreviousButton();
        
        var loading = $('.loading_screen');

        var text = $('.search_field').val();
        var searchIndex = $('.search_index :selected').val();

        var pages = $('.page_number_container');
        var totalProducts = $(".total_products");

        pages.html("");
        totalProducts.html("");

        loading.html(getLoaderHtml());

        var table = $('.products_table tbody');

        $(".products_table tr").remove();

        $.getJSON('/Home/GetInitialProducts', { searchIndex: searchIndex, keyword: text }, function (data) {
            console.log("Got initial products");
            var products = data.Products;

            canRetrieveAllProducts = data.CanRetrieveAllProducts;

            for (var i = 0; i < products.length; i++) {

                var product = products[i];
                table.append(getRow(product));
            }

            loading.html("");

            totalProducts.html("Results: " + data.TotalProducts);

            activePage = 1;

            dataPages[activePage] = products;

            totalPages = data.PageCount;

            updatePageHtml();

            $('.page_number_container').css('visibility', 'visible');

            if (totalPages > 1) {
                showNextButton();
                getAdditionalProducts(activePage + 1, searchIndex, text);
            } else if (totalPages == 0) {
                $('.page_number_container').css('visibility', 'hidden');
            }
        });

    }

    function loadNextPage() {
        activePage++;

        if (activePage > 1) {
            showPreviousButton();
        }

        if (activePage == totalPages) {
            hideNextButton();
        }

        var nextPage = activePage + 1;

        if (dataPages[nextPage.toString()] == undefined && activePage < totalPages) {

            var text = $('.search_field').val();
            var searchIndex = $('.search_index :selected').val();
            getAdditionalProducts(nextPage, searchIndex, text);
        }

        updatePageHtml();
        updateRows();
    }

    function loadPreviousPage() {
        activePage--;

        if (activePage == 1) {
            hidePreviousButton();
        }

        if (activePage < totalPages) {
            showNextButton();
        }

        updatePageHtml();
        updateRows();
    }

    function updateRows() {

        var iterator = 0;

        $('.products_table > tbody  > tr').each(function () {
            var product = dataPages[activePage.toString()][iterator];

            if (product == undefined) {
                // Can happen on the final page. rowCount > productCount
                $(this).html("");
                $(this).css('height', "0");
            } else {
                $(this).html(getRowContent(product));
            }

            iterator++;
        });

        if (activePage == totalPages && !canRetrieveAllProducts) {
            var table = $('.products_table tbody');
            table.append(getMoreResultsUrlRow());
        } else {
            var moreResultsRow = $('.more_results_row');

            if (moreResultsRow != undefined) {
                moreResultsRow.remove();
            }
        }

        animateScrollTop();
    }

    function updatePrices() {
        var rate = $(this).val().split("/")[0];
        var code = $(this).val().split("/")[1];

        var index = 0;

        $('.products_table > tbody  > tr > .row_price').each(function () {

            var product = dataPages[activePage][index];

            index++;

            if (!product.HasPrice) {
                $(this).html("No offers");
            } else if (product.IsPriceTooLow) {
                $(this).html(product.FormattedPrice);
            } else {
                var real = product.PriceNumericValue;
                var recalculated = (real * rate).toFixed(2);
                $(this).html(recalculated + " " + code);
            }

        });
    }

    function getAdditionalProducts(page, searchIndex, text) {
        
        hideNextButton();
        showPagingLoader();

        $.getJSON('/Home/GetAdditionalProducts', { searchIndex: searchIndex, keyword: text }, function (data) {

            showNextButton();
            hidePagingLoader();

            console.log("Got additional products");

            moreResultsUrl = data.MoreResultsUrl;

            dataPages[page] = data.Products;
        });
    }

    function updatePageHtml() {
        $('.page_number_container').html("Page " + activePage + " of " + totalPages);
    }

    function getCategories() {
        var categorySelect = $('.search_index');

        $.getJSON('/Home/GetCategories', {}, function (data) {

            for (var i = 0; i < data.length; i++) {
                var category = data[i];
                categorySelect.append(getCategoryOption(category));
            }
        });
    }

    function getExchangeRates() {

        var rateSelect = $('.exchange_rate_select');

        $.getJSON('/Home/GetExchangeRates', {}, function (data) {

            if (data.IsOk) {
                defaultExchangeRate = data.Base;
                for (var i = 0; i < data.Rates.length; i++) {
                    var rate = data.Rates[i];
                    rateSelect.append(getRateOption(rate));
                }
            }
        });
    }

    function getCategoryOption(category) {
        return "<option value='" + category.Value + "'>" + category.Text + "</option>";
    }

    function getRateOption(rate) {

        if (rate.Code == defaultExchangeRate) {
            return "<option selected='selected' value='" + rate.Rate + "/" + rate.Code + "'>" + rate.Code + "</option>";
        }

        return "<option value='" + rate.Rate + "/" + rate.Code + "'>" + rate.Code + "</option>";
    }

    function getRow(product) {
        var row = $('<tr></tr>').append(getRowContent(product));
        return row;
    }

    function getRowContent(product) {
        return getImageHtml(product.ImageUrl) +
            '<td class="text_row row_title">' + product.Title + '</td>' +
            '<td class="text_row row_price">' + getCalculatedPrice(product) + '</td>';
    }

    function getMoreResultsUrlRow() {
        return $('<tr></tr>').append(
            getImageHtml("../Content/Images/icon_sad.png") +
            "<td class='row_title final_row'> Sadly, we cannot display any more results on this site.</td>" +
            "<td class='row_price final_row'> <a href='" + moreResultsUrl + "'> <br> Go to Amazon.com to see more results </td>"
            )
    }

    function getCalculatedPrice(product) {

        if (!product.HasPrice) {
            return "No offers";
        }

        if (product.IsPriceTooLow) {
            return product.FormattedPrice;
        }

        var rateSelect = $('.exchange_rate_select');

        var rate = rateSelect.val().split("/")[0];
        var code = rateSelect.val().split("/")[1];

        if (code == defaultExchangeRate) {
            return product.FormattedPrice;
        } else {
            var recalculated = (product.PriceNumericValue * rate).toFixed(2);
            return recalculated + " " + code;
        }
    }

    function getImageHtml(url) {
        return '<td><div class="row_image"><img class = "product_image" src="' + url +
            '" alt="" border=3 height=100 width=100></img></div></td>';
    }

    function getLoaderHtml() {
        return '<img class="loader" src="../Content/Images/loader.gif" alt="Loading..."/>'
    }

    function animateScrollTop() {
        var body = $("html, body");
        body.stop().animate({ scrollTop: 0 }, '300', 'swing', function () {
        });
    }


    function showNextButton() {
        $('.next_button').css('visibility', 'visible');
    }

    function hideNextButton() {
        $('.next_button').css('visibility', 'hidden');
    }

    function showPreviousButton() {
        $('.previous_button').css('visibility', 'visible');
    }

    function hidePreviousButton() {
        $('.previous_button').css('visibility', 'hidden');
    }

    function showPagingLoader() {
        $('.loading_small').css('visibility', 'visible');
    }

    function hidePagingLoader() {
        $('.loading_small').css('visibility', 'hidden');
    }

});