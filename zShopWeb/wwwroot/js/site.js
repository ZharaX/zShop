var displayingDetails = false;
var displayingOrders = false;

$(document).ready(function () {
	if (!displayingDetails) {
		slideBoxDown($("#customerDetails"));
	}

	if (displayingOrders) {
		//$("#detailsContainer").css({ marginLeft: "19%", marginTop: "10%" });
		$("#customerDetails").css("margin-top", "10%");
		$("#detailsContainer").animate({ marginLeft: "19%" },
			{
				duration: 250
			});
		//$("#ordersContainer").css("margin-left", "51%");
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
});

function slideBoxDown(container) {
	$(container).animate({ marginTop: "10%" }, {
		duration: 500,
		complete: function () {
			displayingDetails = true;
		}
	});
};