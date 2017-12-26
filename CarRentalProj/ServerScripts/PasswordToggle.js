
    function show() {
        var x = document.getElementById("PStoggle");
        if (x.type === "password") {
            x.type = "text";
        } else {
            x.type = "password";
        }
    }

