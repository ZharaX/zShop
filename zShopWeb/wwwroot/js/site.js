var displayingDetails = false;
var displayingOrders = false;
var displayingCartOrder = false;

$(document).ready(function () {
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