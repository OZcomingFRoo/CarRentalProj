//This a simple Java Script from W3schools for Settin and Getting cookies

//exdays is a variable for how many days they expire date will occur
//CookieName = Name CookieVal = Value of Cookie
function SetCookie(CookieName, CookieVal, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = CookieName + "=" + CookieVal + ";" + expires + ";path=/";
}

//This will return a string value.
//Either with a word or it will return ""/empty string (NOT NULL)
function GetCookie(CookieName) {
    var name = CookieName + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}