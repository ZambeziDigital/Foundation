window.openModal = function(id) {
    $('#' + id).modal('show');
}

window.getCanvas = function(canvasId) {
    var canvas = $("#" + canvasId);
    return canvas[0].toDataURL("image/png");
}


window.displayMessage = function(
    text = "You have successfully reset your password!", 
    icon = "success",
    confirmButtonText = "Ok, got it!", 
    buttonClass = "btn btn-primary") {
    Swal.fire({
        text: text,
        icon: icon,
        buttonsStyling: false,
        confirmButtonText: confirmButtonText,
        customClass: { confirmButton: buttonClass },
    });
}


window.createChart = function(chartId, chartData) {
    // Initialize chart
    var myChart = echarts.init(document.getElementById(chartId));

    // Use configuration item and data specified to show chart
    myChart.setOption(chartData);
};



window.themeControlClick = function() {
    $(themeController.node).on('click', function(e) {
        var target = $(e.target);

        if (target.data('theme-control')) {
            var control = target.data('theme-control');

            var value = e.target.type === 'radio' ? e.target.value : e.target.checked;

            if (control === 'phoenixTheme') {
                if (typeof value === 'boolean') {
                    value = value ? 'dark' : 'light';
                }
            }

            if (config.hasOwnProperty(control)) {
                window.config.set({
                    [control]: value
                });
            }

            window.history.replaceState(null, null, window.location.pathname);
            switch (control) {
                case 'phoenixTheme':
                    document.documentElement.classList[value === 'dark' ? 'add' : 'remove']('dark');
                    var clickControl = new CustomEvent('clickControl', {
                        detail: { control, value }
                    });
                    e.currentTarget.dispatchEvent(clickControl);
                    changeTheme($(themeController.node));
                    break;
                case 'phoenixNavbarVerticalStyle':
                    $('.navbar-vertical').removeClass('navbar-darker');
                    if (value !== 'default') {
                        $('.navbar-vertical').addClass(`navbar-${value}`);
                    }
                    break;
                case 'phoenixNavbarTopStyle':
                    $('.navbar-top').removeClass('navbar-darker');
                    if (value !== 'transparent') {
                        $('.navbar-top').addClass(`navbar-${value}`);
                    }
                    break;
                case 'phoenixNavbarTopShape':
                    if(getItemFromStore('phoenixNavbarPosition') === 'dual-nav'){
                        el.attr('disabled', true);
                    }
                    else
                        handlePageUrl(target.node);
                    break;
                case 'phoenixNavbarPosition':
                    handlePageUrl(target.node);
                    break;
                case 'phoenixIsRTL':
                    window.config.set({
                        phoenixIsRTL: target.node.checked
                    });
                    window.location.reload();
                    break;
                case 'phoenixSupportChat':
                    var supportChat = $('.support-chat-container');
                    supportChat.removeClass('show');
                    if (value) {
                        supportChat.addClass('show');
                    }
                    break;
                case 'reset':
                    window.config.reset();
                    window.location.reload();
                    break;
                default:
                    window.location.reload();
            }
        }
    });
};

// This function takes two parameters: the modal id and the input id
window.openModalAndFocus = function(modalId, inputId) {
    // First, open the modal using the modal id
    $('#' + modalId).modal('show');
    // Then, use the .on() method to attach a handler to the shown.bs.modal event
    // This event is fired when the modal has been made visible to the user
    $('#' + modalId).on('shown.bs.modal', function () {
        // Inside the handler, use the .focus() method to set the focus on the input element using the input id
        $('#' + inputId).focus();
    })
}




// This function that forcuses on the input element with the specified id
window.focusElement = function (id) {
    $('#' + id).focus();
    $('#' + id).select();
}


window.closeModal = function(id) {
    $('#' + id).modal('hide');
}

window.openOffcanvas = function (id) {
    $('#' + id).offcanvas('show');
}

window.closeOffcanvas = function (id) {
    $('#' + id).offcanvas('hide');
}

window.showToast = function (id) {
    $('#' + id).toast('show');
}

window.hideToast = function (id) {
    $('#' + id).toast('hide');
}

window.printWindow = function () {
    window.print();
}

window.initCanvas = function(canvasId) {
    var $canvas = $('#' + canvasId);
    var ctx = $canvas[0].getContext('2d');
    var writing = false;
    var erasing = false;
    var lastEvent;

    ctx.lineWidth = 1; // Make the strokes finer
    ctx.lineJoin = 'round'; // Make the strokes less pixelated
    ctx.lineCap = 'round'; // Make the lines smoother

    function resizeCanvas() {
        $canvas[0].width = $canvas.parent().width();
        $canvas[0].height = $canvas.parent().height();
    }

    function startWriting(event) {
        writing = true;
        lastEvent = event; // Store the last event
        event.preventDefault();
    }

    function write(event) {
        if (writing) {
            ctx.beginPath();
            // Move to the last position
            ctx.moveTo(lastEvent.clientX - $canvas.offset().left, lastEvent.clientY - $canvas.offset().top);
            // Draw a line to the current position
            ctx.lineTo(event.clientX - $canvas.offset().left, event.clientY - $canvas.offset().top);
            // Stroke the line
            ctx.stroke();
            // Update the last position
            lastEvent = event;
        }
    }

    function stopWriting() {
        writing = false;
    }

    $('#eraser').click(function() {
        erasing = !erasing;
        ctx.globalCompositeOperation = erasing ? 'destination-out' : 'source-over';
    });

    $canvas.on('mousedown', startWriting);
    $canvas.on('mousemove', write);
    $canvas.on('mouseup', stopWriting);
    $canvas.on('mouseout', stopWriting);

    // Add touch event listeners for mobile devices
    $canvas.on('touchstart', function(e) { startWriting(e.touches[0]); });
    $canvas.on('touchmove', function(e) { write(e.touches[0]); });
    $canvas.on('touchend', stopWriting);

    // Resize the canvas to fill the window
    $(window).resize(resizeCanvas);
    resizeCanvas(); // Initial resize
}

// $('#' + id).on('hidden.bs.toast', function () {
//     // Do something when the toast is dismissed
// });
//





function areaPiecesChartInitJQuery(datesAndScores) {
    const { getColor, getData, rgbaColor } = window.phoenix.utils;
    const $chartEl = $('.echart-area-pieces-chart-example');

    if ($chartEl.length) {
        const userOptions = getData($chartEl[0], 'echarts');
        const chart = window.echarts.init($chartEl[0]);
        const getDefaultOptions = () => ({
            tooltip: {
                trigger: 'axis',
                padding: [7, 10],
                backgroundColor: getColor('gray-100'),
                borderColor: getColor('gray-300'),
                textStyle: { color: getColor('dark') },
                borderWidth: 1,
                transitionDuration: 0,
                axisPointer: {
                    type: 'none'
                },
                formatter: params => tooltipFormatter(params)
            },
            xAxis: {
                type: 'category',
                boundaryGap: false,
                axisLine: {
                    lineStyle: {
                        color: getColor('gray-300'),
                        type: 'solid'
                    }
                },
                axisTick: { show: false },
                axisLabel: {
                    color: getColor('gray-400'),
                    margin: 15,
                    formatter: value => window.dayjs(value).format('MMM DD')
                },
                splitLine: {
                    show: false
                }
            },
            yAxis: {
                type: 'value',
                splitLine: {
                    lineStyle: {
                        color: getColor('gray-200')
                    }
                },
                boundaryGap: false,
                axisLabel: {
                    show: true,
                    color: getColor('gray-400'),
                    margin: 15
                },
                axisTick: { show: false },
                axisLine: { show: false }
            },
            visualMap: {
                type: 'piecewise',
                show: false,
                dimension: 0,
                seriesIndex: 0,
                pieces: [
                    {
                        gt: 1,
                        lt: 3,
                        color: rgbaColor(getColor('primary'), 0.4)
                    },
                    {
                        gt: 5,
                        lt: 7,
                        color: rgbaColor(getColor('primary'), 0.4)
                    }
                ]
            },
            series: [
                {
                    type: 'line',
                    name: 'Total',
                    smooth: 0.6,
                    symbol: 'none',
                    lineStyle: {
                        color: getColor('primary'),
                        width: 5
                    },
                    markLine: {
                        symbol: ['none', 'none'],
                        label: { show: false },
                        data: [{ xAxis: 1 }, { xAxis: 3 }, { xAxis: 5 }, { xAxis: 7 }]
                    },
                    areaStyle: {},
                    data: datesAndScores
                }
            ],
            grid: { right: 20, left: 5, bottom: 5, top: 8, containLabel: true }
        });
        echartSetOption(chart, userOptions, getDefaultOptions);
    }
}
