(function () {

    var editpostform = document.querySelector("#postsave");
    
    editpostform.addEventListener("click", function (e) {

        var titleElm = this.form.querySelector("#EditedPost_Title");
        var oldtitle = titleElm.getAttribute("data-oldtitle");
        var newtitle = titleElm.value;
        var hasSlug = this.form.getAttribute("data-has-slug") == "True";

        if (oldtitle !== newtitle && hasSlug) {
            if (confirm("Changing the post title will update the post slug and break external links. \nDo you wish to update the slug?")) {
                this.form.querySelector("#updateSlug").value = true;
            }
            else {
                this.form.querySelector("#updateSlug").value = false;
            }
        }

    }, false);
})();   
