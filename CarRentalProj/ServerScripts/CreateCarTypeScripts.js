
 function IdBuilder() {
        var m = $("#model").val().substring(0, 3)
        var c = $("#company").val().substring(0, 2);
        var GT = $("#GT").val()
        if (GT == 'Auto')
            GT = '1';
        else
            GT = '0'
        $("#r").val(c + m + GT);
        var r = $("#r").val();
        if(r.length == 6)
        {
            $("#Pic").attr("disabled", false);
        }
        else {
            $("#Pic").attr("disabled", true);
        }
    }

function sumbit()
{
    var img = document.getElementById("Pic").files[0];
    var r = $("#r").val();
    var data = new FormData();
    data.append("files", img);
    data.append("Name", r); 
    $.ajax({
        url: '/CRSS/GetCarPic',
        data: data,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            if(data == true)
            {
                $("#r").attr("disabled", false);
                $("#ob").attr("disabled", false);
            }
            else
            {
                alert("please insert image");
            }
        }
    });
    $("#r").attr("disabled", false);
    document.getElementById("sub").click() // the actual submit !!
}
    
function CheckFileName() {

    var fileName = document.getElementById("Pic").value
    if (fileName == "") {
        alert("Browse to upload a valid File with jpg extension");
        return false;
    }
    else if (fileName.split(".")[1].toUpperCase() == "JPG") {
        alert("A Valid jpg file is attached !");
        $("#sub").attr("disabled", false);
    }
    else {
        document.getElementById("Pic").value = '';
        alert("Not a jpg File !");
        $("#sub").attr("disabled", true);
        $("#Prev").attr("src", "");
    }

    return true;

}

$("#Pic").change(function (e) {
    if (document.getElementById("Pic").value.split(".")[1].toUpperCase() == "JPG") {
        for (var i = 0; i < e.originalEvent.srcElement.files.length; i++) {

            var file = e.originalEvent.srcElement.files[i];

            var img = document.getElementById("Prev");
            var reader = new FileReader();
            reader.onloadend = function () {
                img.src = reader.result;
            }
            reader.readAsDataURL(file);
        }
    }
});