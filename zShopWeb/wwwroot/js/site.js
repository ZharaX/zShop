var displayingDetails = false;
var displayingOrders = false;
var displayingCartOrder = false;

$(document).ready(function () {
	onlyPositiveNumbers();

	/* .atc (add to cart) button click -> calls ajax function */
	$(".atc").click(function () {
		addToCart($(this).attr("id").slice(5))
	});

	/* Slide Customer Details window down */
	if (!displayingDetails) {
		slideBoxDown($("#customerDetails"));
	}

	if (displayingOrders) {
		$("#customerDetails").css("margin-top", "10%");
		$("#detailsContainer").animate({ marginLeft: "19%" },
			{
				duration: 250
			});
		$("#ordersContainer").animate({ marginLeft: "51%" },
			{
				duration: 250
			});
	}

	$("#btnEditCustomer").click(function () {
		$("#customerDetails").animate({ marginTop: "-150%" },
			{
				duration: 250,
				complete: function () {
					$("#customerEdit").show();
					slideBoxDown($("#customerEdit"));
				}
			});
	});

	$("#cartAccept").click(function () {
		if ($("#detailsContainer").is(":visible")) {
			displayingDetails = false;
			displayingOrders = false;
			$("#detailsContainer").hide();
			$("#ordersContainer").hide();
		}

		$("#cartDetails").animate({ marginTop: "30%" }, { duration: 500 });
	});
});

function slideBoxDown(container) {
	$(container).animate({ marginTop: "10%" }, {
		duration: 500,
		complete: function () {
			displayingDetails = true;
		}
	});
};

function addToCart(productID) {
	console.log("ProductID Added To Cart: " + productID);

	$.ajax({
		url: "/Products/AddToCart",
		type: "GET",
		data: { id: productID },
		success: function (data) {
			$("body").html(data);
			window.location.reload();
		},
		error: function (e) {
			alert(this.url);
		}
	});
}

function onlyPositiveNumbers() {
	// NASTY HACK FOR WIERD VALIDATION ISSUE
	$(".opn").keyup(function () {
		if ($(this).val() > 999) {
			$(this).val(999);
		}
	});

	// WE WON'T ALLOW CHARACTERS! ONLY NUMERICS EVEN PASTE
	$(".opn").on("paste input", function () {
		// ONLY NUMBERS
		$(this).val($(this).val().replace(/[^0-9]/g, ""));

		// GET MIN/MAX VALIDATION VALUES
		let max = parseInt($(this).attr("data-val-range-max"));
		let min = parseInt($(this).attr("data-val-range-min"));

		// NO NEGATIVE NUMBERS
		if ($(this).val() < min) {
			$(this).val(0);
		}

		// NO NUMBERS EXCEEDING MAX VALIDATION SETTING
		if ($(this).val() > max) {
			$(this).val(max);
		}

		// THIS SETS THE INPUT TO MAX ON +1 CHAR LENGTH
		if ($(this).val().length > max.length) {
			$(this).val(max);
		}

		if ($(this).val() == 0) {
			$("#btnCartSubmit").attr("disabled", true);
		}
		else {
			$("#btnCartSubmit").attr("disabled", false);
		}
	});

	$(".opn").on("click", function () {
		$(this).select();
	});
}