// Get the modal
var popup = document.getElementById("popup");

// Get the image and insert it inside the modal - use its "alt" text as a caption
var thumbnail = document.getElementById("thumbnail");
var popupImg = document.getElementById("popup-img");

thumbnail.onclick = function(){
    popup.style.display = "block";
    popupImg.src = this.src;
}

// Get the <span> element that closes the modal
var close = document.getElementsByClassName("close")[0];

// When the user clicks on <span> (x), close the modal
close.onclick = function() { 
    popup.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
    if (event.target == popup) {
        popup.style.display = "none";
    }
}
