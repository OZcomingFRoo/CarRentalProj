
        $("#changer").on("change", function ()
        {
            var str = $("#changer").val();
            if (str == "Any")
                return;
            $.ajax("/CarRentz/NameOfModels?str=" + str, {
                
                success: function (r) {
                    alert(r);
                    $("#MAny").siblings().remove();
                    $("#MAny").css("color", "red");
                    for (var i = 0; i <= r.length; i++) 
                    {
                        $("#MAny").after("<option>" + r.pop() + "</option>");
                    }
                },
                error: function (r) {
                    alert("ASD");
                },
                method: "get"
            }
            );
        })
    
    $("#AddFill").click(function (){
        $("#FadeWrapper").slideToggle("fast");
        if ($("#AddFill").text() == "Additional Filters")
            $("#AddFill").text("Hide Additional Filter");
        else
            $("#AddFill").text("Additional Filter");
    })
    

    var ED, SD;
function DateChk()
{
    SD = $("#SD").val()
    ED = $("#ED").val()
    if (SD > ED)
    {
        $("#Sub").prop('disabled', true);
        $("#DateAlert").prop('hidden', false);
    }
    else
    {
        $("#Sub").prop('disabled', false);
        $("#DateAlert").prop('hidden', true);
    }
            
}

    Date.prototype.toDateInputValue = (function () {
        var local = new Date(this);
        local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
        return local.toJSON().slice(0, 10);
    });
$(function () {
    var now = new Date();
    $("#ED").val(now.toDateInputValue());
    $("#SD").val(now.toDateInputValue());
})
