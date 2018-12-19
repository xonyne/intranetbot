// Meeting content
$(".showMeetingContentButton").click(function () {
    $("#" + $(this).attr("meetingContent-modal-id")).modal("show");
});

// Meeting comments

$(".addMeetingCommentButton").click(function () {
    $("#addMeetingCommentModal").find("#InputMeetingCommentMeetingId").val($(this).attr("meeting-id"));
    $("#addMeetingCommentModal").find("#InputMeetingCommentId").val("");
    $("#addMeetingCommentModal").find("#InputMeetingComment").val("");
    $("#addMeetingCommentModal").modal("show");
});

$(".editCommentButton").click(function () {
    $("#addMeetingCommentModal").find("#InputMeetingCommentMeetingId").val($(this).attr("meeting-id"));
    $("#addMeetingCommentModal").find("#InputMeetingCommentId").val($(this).attr("comment-id")).val();
    $("#addMeetingCommentModal").find("#InputMeetingComment").val($("#comment-" + $(this).attr("comment-id")).val());
    $("#addMeetingCommentModal").modal("show");
});

$(".hideAddMeetingCommentModal").click(function () {
    $("#addMeetingCommentModal").modal("hide");
});

$("#saveMeetingCommentButton").click(function () {
    $.ajax({
        type: "POST",
        url: "/MeetingContent/SaveMeetingComment",
        data: "MeetingId=" + $("#InputMeetingCommentMeetingId").val() + "&Comment=" + $("#InputMeetingComment").val(),
        success: function (result) {
            $("#meetingComments-" + $("#InputMeetingCommentMeetingId").val()).append(
                '<div id=\"comment-' + result.meetingCommentId + '\" class=\"panel panel-default\"><div class=\"panel-body\">' + result.comment +
                '<button class=\"editCommentButton\" comment-id=\"' + result.meetingCommentId + '" meeting-id=\"' + result.meetingId + '\"><i class=\"glyphicon glyphicon-pencil\" style=\"vertical-align:middle;margin-top: -5px\" /></button>' +
                '</div><small class=\"form-text text-muted\">' + getFullDateStringFromDate(new Date(result.lastUpdated)) + ' by ' + result.lastUpdatedBy + '</small></div>' 
            );
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
});

function getFullDateStringFromDate(d) {
    return ("0" + d.getDate()).slice(-2) + "." + ("0" + (d.getMonth() + 1)).slice(-2) + "." +
        d.getFullYear() + " " + ("0" + d.getHours()).slice(-2) + ":" + ("0" + d.getMinutes()).slice(-2) + ":" + ("0" + d.getSeconds()).slice(-2);
}

function checkEnterAndClickSave(e, buttonId) {
    if (e.keyCode == 13) {
        $("#" + buttonId).click();
        return false;
    }
}


// Attendee details
$(".showAttendeeDetailsButton").click(function () {
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
