// Meeting description
$(".meetingDescriptionButton").click(function () {
    $("#meetingDetailsModal").find("#contentMeetingDetailsModal").html($(this).attr("meeting-description"));
    $("#meetingDetailsModal").modal("show");
});

$(".hideMeetingDetailsModal").click(function () {
    $("#meetingDetailsModal").modal("hide");
});


// Attendee details
$(".attendeeDetailsButton").click(function () {
    $("#" + $(this).attr("attendee-modal-id")).modal("show");
});

$(".hideAttendeeeDetailsModal").click(function () {
    $("#" + $(this).attr("attendee-modal-id")).modal("hide");
});

// Social links
$(".editSocialLinkButton").click(function () {
    $("#editSocialLinkModal").find("#inputSocialLinkURL").val($(this).attr("social-link-url"));
    $("#editSocialLinkModal").modal("show");
});

$(".hideEditSocialLinkModal").click(function () {
    $("#editSocialLinkModal").modal("hide");
});


