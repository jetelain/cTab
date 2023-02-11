var connection = null;
var fullScreenButton = null;
var noSleepButton = null;
var isNoSleep = false;
var noSleep = null;

function updateButtons() {
    if (fullScreenButton) {
        fullScreenButton.removeClass('btn-outline-secondary');
        fullScreenButton.removeClass('btn-primary');
        fullScreenButton.addClass((document.fullscreenElement || isNoSleep) ? 'btn-primary' : 'btn-outline-secondary');
        fullScreenButton.find('i').removeClass('fa-expand');
        fullScreenButton.find('i').removeClass('fa-compress');
        fullScreenButton.find('i').addClass((document.fullscreenElement || isNoSleep) ? 'fa-compress' : 'fa-expand');
    }
}

function noSleepToggle() {
    isNoSleep = !isNoSleep;
    if (!noSleep) {
        noSleep = new NoSleep();
    }
    if (isNoSleep) {
        noSleep.enable();
        console.info('noSleep.enable');
    }
    else {
        noSleep.disable();
        console.info('noSleep.disable');
    }
    updateButtons();
}

function fullScreenToggle() {
    if (!document.documentElement.requestFullscreen) {
        return noSleepToggle();
    }
    if (document.fullscreenElement) {
        if (isNoSleep) {
            noSleepToggle();
        }
        document.exitFullscreen().then(updateButtons);
    } else {
        if (!isNoSleep) { // Mobile chrome now will sleep even in fullscreen mode
            noSleepToggle();
        }
        document.documentElement.requestFullscreen().then(updateButtons);
    } 
}

function updateClock(date) {
    var dateObj = new Date(date);
    $('#date').text(pad(dateObj.getUTCHours(), 2) + ':' + pad(dateObj.getUTCMinutes(), 2));
}

function vectLength(vect) {
    return Math.sqrt(vect[0] * vect[0] + vect[1] * vect[1] + vect[2] * vect[2]);
}

function updateInstrumentsAeronautic(airSpeed, groundSpeed, altitude, verticalSpeed, yaw, pitch, roll, heading) {
    
    $("#airSpeed").text('' + Math.round(airSpeed,));
    $("#groundSpeed").text('' + Math.round(groundSpeed));
    $("#altitude").text('' + Math.round(altitude));
    $("#verticalSpeed").text('' + Math.round(verticalSpeed));
    $("#yaw").text('' + yaw.toFixed(2)); // 90° of heading
    $("#pitch").text('' + pitch.toFixed(2));
    $("#roll").text('' + roll.toFixed(2));
    $("#heading2").text('' + heading.toFixed(2));
    $("#horizon-rot").css('transform', 'rotate(' + (-roll).toFixed(2) + 'deg) translateY(' + (-pitch).toFixed(2) + '%)');
    

    $('div.classicinstrument.heading div.heading').css('transform', 'rotate(' + (-heading).toFixed(2) + 'deg)');
    $('div.classicinstrument.attitude div.roll').css('transform', 'rotate(' + roll.toFixed(2) + 'deg)');
    $('div.classicinstrument.attitude div.roll div.pitch').css('top', (Math.max(Math.min(-pitch,30),-30) * 0.7).toFixed(3) + '%');
    $('div.classicinstrument.vario div.vario').css('transform', 'rotate(' + (Math.max(Math.min(verticalSpeed / 1000, 1.95), -1.95) * 90).toFixed(2) + 'deg)');
    $('div.classicinstrument.airspeed div.speed').css('transform', 'rotate(' + (90 + (Math.max(Math.min(airSpeed, 160), 0) * 2)).toFixed(2) + 'deg)');
    var needle = 90 + altitude % 1000 * 360 / 1000;
    var needleSmall = altitude / 10000 * 360;
    $('div.classicinstrument.altimeter div.needle').css('transform', 'rotate(' + needle.toFixed(2) + 'deg)');
    $('div.classicinstrument.altimeter div.needleSmall').css('transform', 'rotate(' + needleSmall.toFixed(2) + 'deg)');
}

function updateInstrumentsMetric(airSpeed, groundSpeed, altitude, verticalSpeed, yaw, pitch, roll, heading) {

    updateInstrumentsAeronautic(
        airSpeed / 0.514, // 1 kn = 0.514 m/s
        groundSpeed / 0.514, // 1 kn = 0.514 m/s
        altitude / 0.3048, // 1 ft = 0.3048 m
        verticalSpeed / 0.00508, // 1 ft/min = 0.00508 m/s
        yaw,
        pitch,
        roll,
        heading
    );
}

function updatePosition(x, y, heading, grp, veh, direction, up, groundVelocity, position, wind) {
    $('#position').text(pad(Math.trunc(x), 5) + ' - ' + pad(Math.trunc(y), 5));
    $('#heading').text(toHeadingUnit(heading));

    var dirLen = vectLength(direction);
    var upLen = vectLength(up);


    var yaw = Math.atan2(direction[1] / dirLen, direction[0] / dirLen);
    var pitch = -Math.asin(direction[2] / dirLen);
    var planeRightX = Math.sin(yaw);
    var planeRightY = -Math.cos(yaw);
    var roll = Math.asin((up[0] / upLen) * planeRightX + (up[1] / upLen) * planeRightY);
    if (up[2] < 0) {
        roll = Math.sign(roll) * Math.PI - roll;
    }
    var airVelocity = [
        wind[0] - groundVelocity[0],
        wind[1] - groundVelocity[1],
        wind[2] - groundVelocity[2]
    ];

    updateInstrumentsMetric(
        vectLength(airVelocity),
        vectLength(groundVelocity),
        position[2],
        groundVelocity[2],
        yaw * 180 / Math.PI,
        pitch * 180 / Math.PI,
        roll * 180 / Math.PI,
        heading);
}


$(function () {

    updateInstrumentsAeronautic(0, 0, 0, 0, 0, 0, 0, 0);

    $('#statusbar').on('click', function () { if (connection.state === signalR.HubConnectionState.Disconnected) { connection.start(); } });

    function connectionLost(e) {
        if (e) {
            $('#status').text(texts.disconnected);
            $('#statusbadge').attr('class', 'badge badge-danger');
        }
    }
    function connected() {
        $('#status').text(texts.waiting);
        $('#statusbadge').attr('class', 'badge badge-warning');
    }
    function started() {
        $('#status').text(texts.connected);
        $('#statusbadge').attr('class', 'badge badge-success');
    }

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/hub")
        .withAutomaticReconnect()
        .build();

    connection.on("Mission", function (missionData) {
        try {
            updateClock(missionData.date);
            started();
        }
        catch (e) {
            console.error(e);
        }
    });

    connection.on("SetPosition", function (positionData) {
        updateClock(positionData.date);
        console.log(positionData);
        try {
        updatePosition(
            positionData.x,
            positionData.y,
            positionData.heading,
            positionData.group,
            positionData.vehicle,
            positionData.vhlDir,
            positionData.vhlUp,
            positionData.vhlVel,
            positionData.vhlPos,
            positionData.wind
            );
        }
        catch (e) {
            console.error(e);
        }
    });

    connection.on("Devices", function (data) {
        if (data.vehicleMode == 2) {
            $("#not-helicopter").hide();
        } else {
            $("#not-helicopter").show();
        }
    });

    function sayHello() {
        if (vm.isSpectator) {
            connection.invoke("SpectatorHello", { spectatorToken: vm.spectatorToken });
        } else {
            connection.invoke("WebHello", { token: vm.token });
        }
    }

    connection.start().then(function () {
        connected();
        sayHello();
    }).catch(connectionLost);

    connection.onreconnecting(connectionLost);
    connection.onreconnected(function () {
        connected();
        sayHello();
    });
    
    if (!vm.isSpectator) {

    }

    setupFullViewHeight('.efis');

    setupCopyButtons();

    fullScreenButton = $('#fullscreen');
    fullScreenButton.on('click', fullScreenToggle);

});