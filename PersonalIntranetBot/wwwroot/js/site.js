// Write your Javascript code.
$(".attendeeDetailsButton").click(function () {
    $("#" + $(this).attr("attendee-modal-id")).modal("show");
});

$(".hideAttendeeeDetailsModal").click(function () {
    $("#" + $(this).attr("attendee-modal-id")).modal("hide");
});

$(".descButton").click(function () {
    $("#meetingDetailsModal").find("#content").html($(this).attr("meeting-description"));
});

$(".descButton").click(function () {
    $("#meetingDetailsModal").modal("show");
});

$(".hideMeetingDetailsModal").click(function () {
    $("#meetingDetailsModal").modal("hide");
});