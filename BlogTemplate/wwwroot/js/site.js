(function () {

    var editpostform = document.querySelector("#editpost");

    editpostform.addEventListener("submit", function (e) {
        var titleElm = this.querySelector("#newPost_Title");
        var oldtitle = titleElm.getAttribute("data-oldtitle");
        var newtitle = titleElm.value;

        if (oldtitle !== newtitle) {
            if (confirm("Changing the post title will update the poist slug and break external links. \r\rDo you wish to update the slug?")) {

                this.querySelector("#updateslug").value = true;
            }
        }
    }, false);

})();   