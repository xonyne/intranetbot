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
    var socialLink = { SocialLinkId: "1", AttendeeId: "0", Type:"0", URL: "http"  }
    $.ajax({
        type: "POST",
        url: "/SocialLink/SaveSocialLink",
        data: "{link:" + JSON.stringify(socialLink) + "}",
        contentType: "application/json; charset=utf-8",
        success: function (msg) {
            alert("Social Link " + msg + " successfully saved!");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
});



