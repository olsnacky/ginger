function insertScript(src, callback) {
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

    script.src = src;
    document.body.appendChild(script);
}

function resizeAce() {
    var editor = document.querySelector('#editor');
    var main = document.querySelector('main')

    editor.parentNode.style.height = main.offsetHeight - 32 + "px";
    editor.style.height = editor.parentNode.style.height;
};

function resizeGraph() {
    var graph = document.querySelector('#graph-panel');
    var example = document.querySelector('.example-card');
    var rightCol = document.querySelector('#editor');
    
    // console.log(example.offsetHeight)
    graph.style.height = rightCol.offsetHeight - 47 + 'px';

    var width = example.offsetWidth;

    if (width > 0) {
        graph.style.width = example.offsetWidth + 'px';
    }
}

//listen for changes
window.onresize = function (event) {
    resizeAce();
    resizeGraph();
}

// set initially, thanks to team at Google for this
document.querySelector('.mdl-layout').addEventListener('mdl-componentupgraded', function () {
    resizeAce();
    // resizeGraph();
});

function callAce() {
    window.ace = ace;
    var editor = ace.edit("editor");

    editor.setTheme("ace/theme/chrome");
    editor.getSession().setMode("ace/mode/ginger");

    var example = ace.edit("example")
    example.setTheme("ace/theme/solarized_light");
    example.getSession().setMode("ace/mode/ginger");
    example.setReadOnly(true)

    editor.on('input', function () {
        drawGraph();
    });

    resizeAce()
    insertScript("http://d3js.org/d3.v3.min.js", drawGraph)
}

insertScript("js/ace/ace.js", callAce)

function drawGraph() {
    var graphPanel = document.querySelector('#graph-panel');
    graphPanel.innerHTML = "";

    var margin = { top: 20, right: 120, bottom: 20, left: 120 }
    var width = graphPanel.offsetWidth - margin.right - margin.left;
    var height = graphPanel.offsetHeight - margin.top - margin.bottom;

    var tree = d3.layout.tree()
        .size([height, width]);

    var diagonal = d3.svg.diagonal()
        .projection(function (d) { return [d.y, d.x]; });

    var svg = d3.select("#graph-panel").append('svg')
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");
    
    // get ace
    var editor = ace.edit("editor");

    d3.json("http://ginger.azurewebsites.net/api/graph/")
        .header('Content-type', 'application/json')
        .post(JSON.stringify(editor.getValue()), function (error, treeData) {
            // Resolve the edges' source and target names
            var nodesByName = {};
            var edges = treeData.graph.edges;
            var nodeData = treeData.graph.nodes;

            if (edges.length > 0) {
                edges.forEach(function (edge) {
                    // console.log(edge)
                    var parent = edge.source = nodeByName(edge.source);
                    var child = edge.target = nodeByName(edge.target);
                    if (parent.children) {
                        parent.children.push(child);
                    } else {
                        parent.children = [child];
                    }
                });

                var nodes = tree.nodes(edges[0].source);

                svg.selectAll(".link")
                    .data(edges)
                    .enter().append("path")
                    .attr("class", "link")
                    .attr("d", diagonal);

                var node = svg.selectAll(".node")
                    .data(nodes)
                    .enter().append("g")
                    .attr("class", "node")
                    .attr("transform", function(d) { return "translate(" + d.y + "," + d.x + ")"; })
                    
                node.append("circle")
                    .attr("r", 4.5)

                node.append("text")
                    .attr("dx", function (d) {
                        return d.children ? -10 : 10;
                    })
                    .attr("dy", function (d) {
                        return ".35em"
                    })
                    .style("text-anchor", function (d) {
                        return d.children ? "end" : "start";
                    })
                    .text(function (d) {
                        return getNodeName(nodeData, d.name);
                    })


            }


            function nodeByName(name) {
                return nodesByName[name] || (nodesByName[name] = { name: name });
            }

            function getNodeName(treeNodes, id) {
                for (var i = 0; i < treeNodes.length; i++) {
                    if (treeNodes[i].id == id) {
                        return treeNodes[i].label;
                    }
                }
            }
        });
}

