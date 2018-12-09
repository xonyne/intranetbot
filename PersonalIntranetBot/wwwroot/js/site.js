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
    $("#editSocialLinkModal").find("#InputSocialLinkAttendeeID").val($(this).attr("social-link-attendee-id"));
    $("#editSocialLinkModal").modal("show");
});

$(".hideEditSocialLinkModal").click(function () {
    $("#editSocialLinkModal").modal("hide");
});

$("#saveSocialLinkButton").click(function () {
    $.ajax({
        type: "POST",
        url: "/Attendee/SaveSocialLink",
        data: "AttendeeId=" + $("#InputSocialLinkAttendeeID").val() + "&SocialLinkId=" + $("#InputSocialLinkID").val() + "&URL=" + $("#InputSocialLinkURL").val(),
        success: function (msg) {
            // update information in attendee details dialog
            $("#socialLink-" + $("#InputSocialLinkID").val()).attr("href", $("#InputSocialLinkURL").val());
            $("#socialLinkEditButton-" + $("#InputSocialLinkID").val()).attr("social-link-url", $("#InputSocialLinkURL").val());
            $("#socialLink-" + $("#InputSocialLinkID").val()).text($("#InputSocialLinkURL").val());
            // update last updated hint
            $("#lastupdated-" + $("#InputSocialLinkAttendeeID").val()).text(msg);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
});

// Image URL
$(".editImageUrl").click(function () {
    $("#editImageURLModal").find("#InputImageURL").val($(this).attr("src"));
    $("#editImageURLModal").find("#InputImageUrlAttendeeID").val($(this).attr("image-url-attendee-id"));
    $("#editImageURLModal").modal("show");
    $("#editImageURLModal").find("#InputImageURL").focus();
});

$(".hideEditImageURLModal").click(function () {
    $("#editImageURLModal").modal("hide");
});

$("#saveImageURLButton").click(function () {
    $.ajax({
        type: "POST",
        url: "/Attendee/SaveImageURL",
        data: "AttendeeId=" + $("#InputImageUrlAttendeeID").val() + "&ImageURL=" + $("#InputImageURL").val(),
        success: function (msg) {
            // update information in attendee details dialog
            $("#attendee-img-" + $("#InputImageUrlAttendeeID").val()).attr("src", $("#InputImageURL").val());
            // update last updated hint
            $("#lastupdated-" + $("#InputImageUrlAttendeeID").val()).text(msg);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
});

$("#deleteAllDatabaseContentButton").click(function () {
    var answer = confirm("Are you sure you want to delete ALL data in the database?");
    if (answer == true) {
        $.ajax({
            type: "GET",
            url: "/Settings/DeleteAllDatabaseContent",
            success: function (msg) {
                alert(msg);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }

});
