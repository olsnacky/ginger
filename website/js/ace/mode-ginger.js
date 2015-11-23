ace.define('ace/mode/ginger', ['require', 'exports', 'ace/mode/matching_brace_outdent', 'module'], function (require, exports, module) {
    "use strict";

    var oop = require("../lib/oop");
    // defines the parent mode
    var TextMode = require("./text").Mode;
    var Tokenizer = require("../tokenizer").Tokenizer;
    var MatchingBraceOutdent = require("./matching_brace_outdent").MatchingBraceOutdent;

    // defines the language specific highlighters and folding rules
    var MyNewHighlightRules = require("./ginger_highlight_rules").MyNewHighlightRules;
    //var MyNewFoldMode = require("./folding/mynew").MyNewFoldMode;

    var Mode = function () {
        // set everything up
        this.HighlightRules = MyNewHighlightRules;
        this.$outdent = new MatchingBraceOutdent();
        //this.foldingRules = new MyNewFoldMode();
    };
    oop.inherits(Mode, TextMode);

    (function () {
        // configure comment start/end characters
        this.lineCommentStart = "//";
        this.blockComment = { start: "/*", end: "*/" };

        // special logic for indent/outdent. 
        // By default ace keeps indentation of previous line
        this.getNextLineIndent = function (state, line, tab) {
            var indent = this.$getIndent(line);
            return indent;
        };

        this.checkOutdent = function (state, line, input) {
            return this.$outdent.checkOutdent(line, input);
        };

        this.autoOutdent = function (state, doc, row) {
            this.$outdent.autoOutdent(doc, row);
        };

        // create worker for live syntax checking
        this.createWorker = function (session) {
            var worker = new WorkerClient(["ace"], "ace/mode/ginger_worker", "GingerWorker");
            worker.attachToDocument(session.getDocument());
            worker.on("errors", function (e) {
                session.setAnnotations(e.data);
            });
            return worker;
        };

    }).call(Mode.prototype);

    exports.Mode = Mode;
});

ace.define('ace/mode/ginger_worker', ["require", "exports", "module"], function (require, exports, module) {
    "use strict";

    var oop = require("../lib/oop");
    var Mirror = require("../worker/mirror").Mirror;

    var GingertWorker = exports.GingerWorker = function (sender) {
        Mirror.call(this, sender);
        this.setTimeout(500);
    }

    oop.inherits(GingerWorker, Mirror);

    (function () {


        this.onUpdate = function () {
            var value = this.doc.getValue();

            if (!value)
                return this.sender.emit("annotate", []);


        }
    })
});

ace.define('ace/mode/ginger_highlight_rules', ["require", "exports", "module"], function (require, exports, module) {
    "use strict";

    var oop = require("../lib/oop");
    var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;

    var MyNewHighlightRules = function () {
        var keywords = (
            "if|while"
            );
        var builtinConstants = (
            "true|false"
            );
        var builtinFunctions = (
            "int|bool"
            );

        var keywordMapper = this.createKeywordMapper({
            "constant.language": builtinConstants,
            "keyword": keywords,
            "support.function": builtinFunctions
        }, "identifier");

        var integer = "(?:(?:[1-9]\\d*)|(?:0))";

        // regexp must not have capturing parentheses. Use (?:) instead.
        // regexps are ordered -> the first match is used
        this.$rules = {
            "start" : [
                {
                    token: 'constant.numeric',
                    regex: integer + "\\b"
                },
                {
                    token: keywordMapper,
                    regex: "[a-zA-Z_$][a-zA-Z0-9_$]*\\b"
                },
                {
                    token: "keyword.operator",
                    regex: "\\+|<|="
                },
                {
                    token : "paren.lparen",
                    regex : "[\\[\\(\\{]"
                },
                {
                    token: "paren.rparen",
                    regex: "[\\]\\)\\}]"
                },
                {
                    token: "text",
                    regex: "\\s+"
                }
            ]
        };
    };

    oop.inherits(MyNewHighlightRules, TextHighlightRules);

    exports.MyNewHighlightRules = MyNewHighlightRules;
});

ace.define("ace/mode/matching_brace_outdent", ["require", "exports", "module", "ace/range"], function (require, exports, module) {
    "use strict";

    var Range = require("../range").Range;

    var MatchingBraceOutdent = function () { };

    (function () {

        this.checkOutdent = function (line, input) {
            if (! /^\s+$/.test(line))
                return false;

            return /^\s*\}/.test(input);
        };

        this.autoOutdent = function (doc, row) {
            var line = doc.getLine(row);
            var match = line.match(/^(\s*\})/);

            if (!match) return 0;

            var column = match[1].length;
            var openBracePos = doc.findMatchingBracket({ row: row, column: column });

            if (!openBracePos || openBracePos.row == row) return 0;

            var indent = this.$getIndent(doc.getLine(openBracePos.row));
            doc.replace(new Range(row, 0, row, column - 1), indent);
        };

        this.$getIndent = function (line) {
            return line.match(/^\s*/)[0];
        };

    }).call(MatchingBraceOutdent.prototype);

    exports.MatchingBraceOutdent = MatchingBraceOutdent;
});