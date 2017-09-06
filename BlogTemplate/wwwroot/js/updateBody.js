$("#file-input").change(insertFiles, false);

function insertFiles() {
    var selectedFiles = document.getElementById("file-input");
    for (var i = 0; i < selectedFiles.files.length; i++) {
        var postBody = $("#post-body");
        var name = selectedFiles.files[i].name;
        postBody.val(postBody.val() + "\n![" + name + "](/Uploads/" + name + ")");
    }
}
