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
    $("#editSocialLinkModal").find("#InputSocialLinkURL").val($(this).attr("social-link-url"));
    $("#editSocialLinkModal").find("#InputSocialLinkID").val($(this).attr("social-link-id"));
    $("#editSocialLinkModal").modal("show");
});

$(".hideEditSocialLinkModal").click(function () {
    $("#editSocialLinkModal").modal("hide");
});

$("#saveSocialLinkButton").click(function () {
    $.ajax({
        type: "POST",
        url: "SocialLink/Edit/" + $("#InputSocialLinkID").val(),
        data: { socialLinkId: $("#InputSocialLinkURL").val(), url: $("#InputSocialLinkID").val() },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            alert("Social Link " + $("#InputSocialLinkURL").val() + " successfully saved!");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });

});



