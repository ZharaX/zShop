var updateOrders = false;
var displayingDetails = false;
var displayingOrders = false;
var displayingCartOrder = false;

$(document).ready(function () {
	onlyPositiveNumbers();

	/* Slide Customer Details window down */
	if (!displayingDetails) {
		$("#detailsContainer").show();
		slideBoxDown($("#customerDetails"));
	}

	if (updateOrders) {
		$("#detailsContainer").show();
		$("#customerDetails").css("margin-top", "10%");
		$("#detailsContainer").css("margin-left", "19%");

		$("#ordersContainer").show();
		$("#ordersContainer").css("margin-top", "10%");
		$("#ordersContainer").css("margin-left", "51%");
	}
	else {
		if (displayingDetails) {
			$("#detailsContainer").show();
		}
		if (displayingOrders) {
			$("#detailsContainer").show();
			$("#customerDetails").css("margin-top", "10%");
			$("#detailsContainer").animate({ marginLeft: "19%" },
				{
					duration: 250
				});
			$("#ordersContainer").show();
			$("#ordersContainer").animate({ marginLeft: "51%" },
				{
					duration: 250
				});
		}

		$("#btnEditCustomer").click(function () {
			$("#detailsContainer").show();
			$("#customerDetails").animate({ marginTop: "-150%" },
				{
					duration: 250,
					complete: function () {
						$("#customerEdit").show();
						slideBoxDown($("#customerEdit"));
					}
				});
		});
	}
	

	/* .atc (add to cart) button click -> calls ajax function */
	$(".atc").click(function () {
		addToCart($(this).attr("id").slice(5))
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

	/* .rfc (remove from cart) button click -> calls ajax function */
	$(".rfc").click(function () {
		removeFromCart($(this).attr("id").slice(14))
	});

	if ($("#message").is(":hidden")) {
		$("#message").fadeIn({
			duration: 2000,
			complete: function () {
				// IF NO ACTION IS MADE ON NOTIFICATION -> REMOVES ITSELF AFTER 7½ SECS
				var messageDelay = setTimeout(function () {
					$("#message").fadeOut();
					$("#message").remove();
				}, 5000);
			}
		});
	};
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

function removeFromCart(productID) {
	console.log("ProductID Removed From Cart: " + productID);

	$.ajax({
		url: "/Orders/Details/RemoveFromCart",
		type: "GET",
		data: { id: productID },
		success: function (data) {
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
		let max = parseInt($(this).attr("max"));
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

		if ($(this).val() == 0 || $(this).val() > max) {
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