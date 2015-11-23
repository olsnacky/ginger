var script = document.createElement("script")
script.type = "text/javascript";

if (script.readyState) {  //IE
    script.onreadystatechange = function () {
        if (script.readyState == "loaded" ||
                script.readyState == "complete") {
            script.onreadystatechange = null;
            callback();
        }
    };
} else {  //Others
    script.onload = function () {
        callback();
    };
}

script.src = "js/ace/ace.js";
document.body.appendChild(script);

function resizeAce() {
    var editor = document.querySelector('#editor');
    var doc = editor.parentNode.ownerDocument;
    var window = doc.defaultView || doc.parentWindow;
    
    editor.parentNode.style.height = window.innerHeight - 32 + "px";
    editor.style.height = editor.parentNode.style.height;
};

//listen for changes
window.onresize = function (event) {
    resizeAce();
}

//set initially
resizeAce();


function callback() {
    var editor = ace.edit("editor");
    editor.setTheme("ace/theme/chrome");
    editor.getSession().setMode("ace/mode/ginger");
    resizeAce()
}