
function drawWorkflow(wfData, parent, prevElement, plumbs) {
    var workflow, list, consecutiveItem, parallelItem, parallelList = document.createDocumentFragment();

    workflow = createEventElement(wfData);

    if (plumbs.length != 0) {
        plumbs.push({ source: prevElement.div, target: workflow, anchors: ["BottomCenter", "TopCenter"] });
    }

    if (prevElement == null) {
        prevElement = {};
    }

    prevElement.div = workflow;
    prevElement.event = wfData;

    var events = wfData.events;
    var groupData = _.groupBy(events, function (obj) {
        return obj.order;
    });

    list = $('<ul/>');

    var elements = _.toArray(groupData);
    for (var j = 0; j < elements.length; j++) {
            consecutiveItem = $('<li class="event-li"/>');
            var currentGroup = elements[j];

            if (currentGroup.length > 1) {
                parallelList = $('<ul class="inner-list"/>');
                // Concurrent tasks - Upper Connection
                plumbs.push({ source: prevElement.div, target: parallelList, anchors: ["BottomCenter", "TopCenter"] });
                for (var i = 0; i < currentGroup.length; i++) {
                    var event = currentGroup[i];
                    parallelItem = $('<li class="inner-item"/>');
                    var el = handleEvent(event, workflow, prevElement, plumbs);
                    parallelList.append(parallelItem.append(el));
                }
                prevElement.div = parallelList;
                prevElement.event = {};
                consecutiveItem.append(parallelList);
            } else {
                var event = currentGroup[0];
                var el = handleEvent(event, workflow, prevElement, plumbs);

                if (j === 0) {
                    // Workflow - First Connection
                    plumbs.push({ source: prevElement.div, target: el, anchors: ["TopCenter", "TopCenter"] });
                } else if (event.kind != "ChildWorkflow" && event.kind != "Workflow") {
                   plumbs.push({ source: prevElement.div, target: el, anchors: ["BottomCenter", "TopCenter"] });
                }

                prevElement.div = el;
                prevElement.event = event;
                list.append(consecutiveItem.append(el));
            }
            workflow.append(list.append(consecutiveItem));
    }
    plumbs.push({ source: prevElement.div, target: workflow, anchors: ["BottomCenter", "BottomCenter"] });
    parent.append(workflow);
    return workflow;
};

function handleEvent(event, workflow, prevElement, plumbs) {
    if (event.kind === "ChildWorkflow") {
        return drawWorkflow(event, workflow, prevElement, plumbs);
    } else {
        return createEventElement(event);
    }
}

function createEventElement(event) {
    var el = $("<div/>", {
        id: _.uniqueId('wi_')
    });
    el.addClass(event.kind.toLowerCase() + "-" + event.state.toLowerCase());

    var label = createLabel(event),
        icon = createIcon(event);

    if (event.kind != "ChildWorkflow" && event.kind != "Workflow") {
        setPopover(el, event);
        var auxEl = $('<div class="event-data-container"/>');
        auxEl.append(label);
        auxEl.append($('<div class="event-icon-container"/>').append(icon));
        el.append(auxEl);
    } else {
        el.append(label);
        el.append($('<div class="wf-icon-container"/>').append(icon));
    }
    return el;
};

function createIcon(event) {
    var icon = createSVG(event),
        path = document.createElementNS("http://www.w3.org/2000/svg", "path"),
        iconPath = getPath(event);

    path.setAttribute("d", iconPath);
    path.setAttribute("fill", "#000");
    icon.appendChild(path);

    return icon;
}

function createSVG(event) {
    var svg = document.createElementNS("http://www.w3.org/2000/svg", "svg:svg");

    svg.setAttribute("id", event.eventId);
    svg.setAttribute("width", 50);
    svg.setAttribute("height", 50);
    svg.setAttribute("viewBox", "0 0 50 50");
    svg.setAttribute("fill-opacity", 0.1);

    return svg;
}

function getPath(event) {
    var path;

    switch (event.kind) {
        case "Activity":
            path = "M31.229,17.736c0.064-0.571,0.104-1.148,0.104-1.736s-0.04-1.166-0.104-1.737l-4.377-1.557c-0.218-0.716-0.504-1.401-0.851-2.05l1.993-4.192c-0.725-0.91-1.549-1.734-2.458-2.459l-4.193,1.994c-0.647-0.347-1.334-0.632-2.049-0.849l-1.558-4.378C17.165,0.708,16.588,0.667,16,0.667s-1.166,0.041-1.737,0.105L12.707,5.15c-0.716,0.217-1.401,0.502-2.05,0.849L6.464,4.005C5.554,4.73,4.73,5.554,4.005,6.464l1.994,4.192c-0.347,0.648-0.632,1.334-0.849,2.05l-4.378,1.557C0.708,14.834,0.667,15.412,0.667,16s0.041,1.165,0.105,1.736l4.378,1.558c0.217,0.715,0.502,1.401,0.849,2.049l-1.994,4.193c0.725,0.909,1.549,1.733,2.459,2.458l4.192-1.993c0.648,0.347,1.334,0.633,2.05,0.851l1.557,4.377c0.571,0.064,1.148,0.104,1.737,0.104c0.588,0,1.165-0.04,1.736-0.104l1.558-4.377c0.715-0.218,1.399-0.504,2.049-0.851l4.193,1.993c0.909-0.725,1.733-1.549,2.458-2.458l-1.993-4.193c0.347-0.647,0.633-1.334,0.851-2.049L31.229,17.736zM16,20.871c-2.69,0-4.872-2.182-4.872-4.871c0-2.69,2.182-4.872,4.872-4.872c2.689,0,4.871,2.182,4.871,4.872C20.871,18.689,18.689,20.871,16,20.871z";
            break;
        case "Signal":
            path = "M24.264,20.958c-2.484-4.226-2.168-13.199-6.143-15.486c0.254-0.395,0.404-0.861,0.404-1.366c0-1.396-1.129-2.526-2.526-2.526c-1.396,0-2.527,1.131-2.527,2.526c0,0.505,0.151,0.973,0.406,1.367C9.905,7.76,10.221,16.732,7.736,20.958C5.585,21.523,4.25,22.311,4.25,23.18v1.125c0,1.604,3.877,2.938,9.077,3.283c-0.003,0.048-0.015,0.096-0.015,0.145c0,1.483,1.203,2.688,2.688,2.688c1.484,0,2.688-1.203,2.688-2.688c0-0.049-0.012-0.097-0.015-0.145c5.199-0.349,9.077-1.688,9.077-3.283V23.18C27.75,22.311,26.415,21.523,24.264,20.958zM14.472,4.105c0.002-0.843,0.685-1.525,1.527-1.527c0.843,0.002,1.526,0.685,1.528,1.527c-0.002,0.372-0.144,0.708-0.361,0.974c-0.359-0.096-0.745-0.15-1.166-0.15s-0.807,0.055-1.167,0.15C14.612,4.814,14.473,4.478,14.472,4.105z";
            break;
        case "Marker":
            path = "M9.5,3v10c8,0,8,4,16,4V7C17.5,7,17.5,3,9.5,3z M6.5,29h2V3h-2V29z";
            break;
        case "Timer":
            path = "M27.216,18.533c0-3.636-1.655-6.883-4.253-9.032l0.733-0.998l0.482,0.354c0.198,0.146,0.481,0.104,0.628-0.097l0.442-0.604c0.146-0.198,0.103-0.482-0.097-0.628l-2.052-1.506c-0.199-0.146-0.481-0.103-0.628,0.097L22.03,6.724c-0.146,0.199-0.104,0.482,0.096,0.628l0.483,0.354l-0.736,1.003c-1.28-0.834-2.734-1.419-4.296-1.699c0.847-0.635,1.402-1.638,1.403-2.778h-0.002c0-1.922-1.557-3.48-3.479-3.48c-1.925,0-3.48,1.559-3.48,3.48c0,1.141,0.556,2.144,1.401,2.778c-1.549,0.277-2.99,0.857-4.265,1.68L8.424,7.684l0.484-0.353c0.198-0.145,0.245-0.428,0.098-0.628l-0.44-0.604C8.42,5.899,8.136,5.855,7.937,6.001L5.881,7.5c-0.2,0.146-0.243,0.428-0.099,0.628l0.442,0.604c0.145,0.2,0.428,0.244,0.627,0.099l0.483-0.354l0.729,0.999c-2.615,2.149-4.282,5.407-4.282,9.057c0,6.471,5.245,11.716,11.718,11.716c6.47,0,11.716-5.243,11.718-11.716H27.216zM12.918,4.231c0.002-1.425,1.155-2.58,2.582-2.582c1.426,0.002,2.579,1.157,2.581,2.582c-0.002,1.192-0.812,2.184-1.908,2.482v-1.77h0.6c0.246,0,0.449-0.203,0.449-0.449V3.746c0-0.247-0.203-0.449-0.449-0.449h-2.545c-0.247,0-0.449,0.202-0.449,0.449v0.749c0,0.246,0.202,0.449,0.449,0.449h0.599v1.77C13.729,6.415,12.919,5.424,12.918,4.231zM15.5,27.554c-4.983-0.008-9.015-4.038-9.022-9.021c0.008-4.982,4.039-9.013,9.022-9.022c4.981,0.01,9.013,4.04,9.021,9.022C24.513,23.514,20.481,27.546,15.5,27.554zM15.5,12.138c0.476,0,0.861-0.385,0.861-0.86s-0.386-0.861-0.861-0.861s-0.861,0.386-0.861,0.861S15.024,12.138,15.5,12.138zM15.5,24.927c-0.476,0-0.861,0.386-0.861,0.861s0.386,0.861,0.861,0.861s0.861-0.386,0.861-0.861S15.976,24.927,15.5,24.927zM12.618,11.818c-0.237-0.412-0.764-0.553-1.176-0.315c-0.412,0.238-0.554,0.765-0.315,1.177l2.867,6.722c0.481,0.831,1.543,1.116,2.375,0.637c0.829-0.479,1.114-1.543,0.635-2.374L12.618,11.818zM18.698,24.07c-0.412,0.237-0.555,0.765-0.316,1.176c0.237,0.412,0.764,0.554,1.176,0.315c0.413-0.238,0.553-0.765,0.316-1.176C19.635,23.974,19.108,23.832,18.698,24.07zM8.787,15.65c0.412,0.238,0.938,0.097,1.176-0.315c0.237-0.413,0.097-0.938-0.314-1.176c-0.412-0.239-0.938-0.098-1.177,0.313C8.234,14.886,8.375,15.412,8.787,15.65zM22.215,21.413c-0.412-0.236-0.938-0.096-1.176,0.316c-0.238,0.412-0.099,0.938,0.314,1.176c0.41,0.238,0.937,0.098,1.176-0.314C22.768,22.178,22.625,21.652,22.215,21.413zM9.107,18.531c-0.002-0.476-0.387-0.86-0.861-0.86c-0.477,0-0.862,0.385-0.862,0.86c0.001,0.476,0.386,0.86,0.861,0.861C8.722,19.393,9.106,19.008,9.107,18.531zM21.896,18.531c0,0.477,0.384,0.862,0.859,0.86c0.476,0.002,0.862-0.382,0.862-0.859s-0.387-0.86-0.862-0.862C22.279,17.671,21.896,18.056,21.896,18.531zM8.787,21.413c-0.412,0.238-0.554,0.765-0.316,1.176c0.239,0.412,0.765,0.553,1.177,0.316c0.413-0.239,0.553-0.765,0.315-1.178C9.725,21.317,9.198,21.176,8.787,21.413zM21.352,14.157c-0.411,0.238-0.551,0.764-0.312,1.176c0.237,0.413,0.764,0.555,1.174,0.315c0.412-0.236,0.555-0.762,0.316-1.176C22.29,14.06,21.766,13.921,21.352,14.157zM12.304,24.067c-0.413-0.235-0.939-0.096-1.176,0.315c-0.238,0.413-0.098,0.939,0.312,1.178c0.413,0.236,0.939,0.096,1.178-0.315C12.857,24.832,12.715,24.308,12.304,24.067zM18.698,12.992c0.41,0.238,0.938,0.099,1.174-0.313c0.238-0.411,0.1-0.938-0.314-1.177c-0.414-0.238-0.937-0.097-1.177,0.315C18.144,12.229,18.286,12.755,18.698,12.992z";
            break;
        case "ExternalSignaling":
        case "ExternalCancellation":
            path = "M4.135,16.762c3.078,0,5.972,1.205,8.146,3.391c2.179,2.187,3.377,5.101,3.377,8.202h4.745c0-9.008-7.299-16.335-16.269-16.335V16.762zM4.141,8.354c10.973,0,19.898,8.975,19.898,20.006h4.743c0-13.646-11.054-24.749-24.642-24.749V8.354zM10.701,25.045c0,1.815-1.471,3.287-3.285,3.287s-3.285-1.472-3.285-3.287c0-1.813,1.471-3.285,3.285-3.285S10.701,23.231,10.701,25.045z";
            break;
        case "Workflow":
        case "ChildWorkflow":
            path = "M6.812,17.202l7.396-3.665v-2.164h-0.834c-0.414,0-0.808-0.084-1.167-0.237v1.159l-7.396,3.667v2.912h2V17.202zM26.561,18.875v-2.913l-7.396-3.666v-1.158c-0.358,0.152-0.753,0.236-1.166,0.236h-0.832l-0.001,2.164l7.396,3.666v1.672H26.561zM16.688,18.875v-7.501h-2v7.501H16.688zM27.875,19.875H23.25c-1.104,0-2,0.896-2,2V26.5c0,1.104,0.896,2,2,2h4.625c1.104,0,2-0.896,2-2v-4.625C29.875,20.771,28.979,19.875,27.875,19.875zM8.125,19.875H3.5c-1.104,0-2,0.896-2,2V26.5c0,1.104,0.896,2,2,2h4.625c1.104,0,2-0.896,2-2v-4.625C10.125,20.771,9.229,19.875,8.125,19.875zM13.375,10.375H18c1.104,0,2-0.896,2-2V3.75c0-1.104-0.896-2-2-2h-4.625c-1.104,0-2,0.896-2,2v4.625C11.375,9.479,12.271,10.375,13.375,10.375zM18,19.875h-4.625c-1.104,0-2,0.896-2,2V26.5c0,1.104,0.896,2,2,2H18c1.104,0,2-0.896,2-2v-4.625C20,20.771,19.104,19.875,18,19.875z";
            break;
    }

    return path;
}

function createLabel(event) {
    var label = $('<span/>');
    label.text(event.eventId + " " + event.name);

    if (event.kind == "ChildWorkflow" || event.kind == "Workflow") {
        label.addClass("wf-label-text");
        setPopover(label, event);
    } else {
        label.addClass("label-text");
    }

    return label;
};

function setPopover(el, event) {
    var htmlContent = '<div class="popover-detail"><span>';

    if (event.id != null) {
        htmlContent += "<strong>Id:</strong> " + event.id;
    } else {
        htmlContent += "<strong>Id:</strong> -";
    }
    if (event.version != null) {
        htmlContent += "<br/><strong>Version:</strong> " + event.version;
    }
    if (event.state != null) {
        htmlContent += "<br/><strong>State:</strong> " + event.state;
    }
    if (event.input != null) {
        htmlContent += "<br/><strong>Input:</strong> " + event.input;
    }
    if (event.result != null) {
        htmlContent += "<br/><strong>Result:</strong> " + event.result;
    }
    if (event.reason != null) {
        htmlContent += "<br/><strong>Reason:</strong> " + event.reason;
    }

    htmlContent += "</span></div>";
    el.popover({
        html: true,
        content: htmlContent,
        title: event.eventId + " - " + event.name,
        placement: "right"
    });
};