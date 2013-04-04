
var app = angular.module('simplewf', ['ngResource']);

app.filter('timeago', function () {
      return function (time) {
          if (time) return jQuery.timeago(time);
          else return "";
      };
  });

app.filter('removehash', function () {
    return function (input) {
        return input.replace(/#/g, "_");
    };
});

app.filter('normalize', function () {
    return function (input) {
        return input.replace(/\+/g, "-").replace(/\//g, "_").replace(/=/g, "_");
    };
});

app.directive('workflowCanvas', function () {
    return {
        link: function (scope, element, attrs) {
            scope.$watch('pane.flowInfo', function (newValue, oldValue) {

                if (angular.isUndefined(newValue)) {
                    return;
                }

                var plumbs = [];
                $(element).html('');
                drawWorkflow(scope.pane.flowInfo, $(element), null, plumbs);
   
                var plumb = jsPlumb.getInstance({
                    PaintStyle: { lineWidth: 6, strokeStyle: "#567567", outlineColor: "black", outlineWidth: 1 },
                    Connector: ["Bezier", { curviness: 30 }],
                    Endpoint: ["Dot", { radius: 5 }],
                    EndpointStyle: { fillStyle: "#567567" }
                });
   
                plumb.Defaults.Container = $(element);
                for (var i = 0; i < plumbs.length; i++) {
                    plumb.connect({ source: plumbs[i].source, target: plumbs[i].target, anchors: plumbs[i].anchors});
                }
                plumbs = [];
            })
        }
    }
});

app.directive('panes', function () {
    return {
        link: function (scope, element, attrs) {
            scope.$watch('panes', function (newValue, oldValue) {
                if (angular.isUndefined(newValue)) {
                    return;
                }
                $(element).tabdrop('layout');
            }, true)
        }
    }
});

function MainCntl($scope, $http, $timeout, $resource) {

    $scope.panes = [];
    $scope.selectedDomainIndex = -1;

    $scope.workflowListRefreshing = false;

    var Workflows = $resource('/domains/:domain/workflows');

    $scope.selectDomain = function (index) {
        $scope.selectedDomainIndex = index;
        $scope.workflowListRefreshing = true;
        domain = $scope.domains[index].name;
        Workflows.query({ domain: domain }, function (data) {
            var length = data.length,
                element = null;
            for (var i = 0; i < length; i++) {
                element = data[i];
                element.domain = domain;
            }
            $scope.workflows = data;
            $scope.workflowListRefreshing = false;
        });
    }

    $scope.getDomainClass = function (index) {
        if (index === $scope.selectedDomainIndex) {
            return "active";
        } else {
            return "";
        }
    }

    $scope.openWorkflowPane = function (workflow) {
        // Do not open a new pane if it its already open
        // TODO: This only works if the workflow is not refreshed.
        if (_.find($scope.panes, function (p) { return p.input === workflow }) != null) {
            return;
        }

        var pane = {};
        pane.info = {};
        pane.input = workflow;

        $scope.panes.push(pane);
    }

    $scope.closeWorkflowPane = function (pane) {
        $scope.panes.splice($scope.panes.indexOf(pane), 1);
    }

    $scope.getWorkflowStatusClass = function (event) {
        if (event === "OPEN") {
            return "label label-success";
        } else {
            return "label";
        }
    }

    $http.get('domains').success(function (data) {
        $scope.domains = data;
    });
}

function WorkflowCntl($scope, $timeout, $resource) {

    var Workflow = $resource('/domains/:domain/workflows/:workflowId/runs/:runId', {
        domain: safeBase64($scope.pane.input.domain),
        workflowId: safeBase64($scope.pane.input.execution.workflowId),
        runId: safeBase64($scope.pane.input.execution.runId)
    });

    (function getInfo() {
        $scope.pane.isRefreshing = true;
        $scope.pane.info2 = Workflow.get(null, function () {
            $scope.pane.info = $scope.pane.info2;
            $scope.pane.isRefreshing = false;
        });
        $timeout(getInfo, 10000);
    })();

    var Flow = $resource('/domains/:domain/workflows/:workflowId/runs/:runId/flow', {
        domain: safeBase64($scope.pane.input.domain),
        workflowId: safeBase64($scope.pane.input.execution.workflowId),
        runId: safeBase64($scope.pane.input.execution.runId)
    });

    (function getFlow() {
        $scope.pane.flowIsRefreshing = true;
        $scope.pane.flowInfo2 = Flow.get(null, function () {
            $scope.pane.flowInfo = $scope.pane.flowInfo2;
            $scope.pane.flowIsRefreshing = false;
        });
        $timeout(getFlow, 10000);
    })();
}

function safeBase64(value) {
    // See http://tools.ietf.org/html/rfc4648
    return value
        .replace(/\+/g, "-")
        .replace(/\//g, "_");
}