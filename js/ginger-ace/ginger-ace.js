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
    var main = document.querySelector('main')
    
    // we set a timeout to handle an issue in material design
    // where the 'main' element size is not set when this is triggered

    editor.parentNode.style.height = main.offsetHeight - 32 + "px";
    editor.style.height = editor.parentNode.style.height;
};

//listen for changes
window.onresize = function (event) {
    resizeAce();
}

//set initially
resizeAce();

function callback() {
    window.ace = ace;
    var editor = ace.edit("editor");
    
    editor.setTheme("ace/theme/chrome");
    editor.getSession().setMode("ace/mode/ginger");
    
    var example = ace.edit("example")
    example.setTheme("ace/theme/solarized_light");
    example.getSession().setMode("ace/mode/ginger");
    example.setReadOnly(true)
    
    resizeAce()
}