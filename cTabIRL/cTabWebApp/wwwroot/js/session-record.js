(function () {
    var isRecording = false;
    var recordButton = null;
    var _originalInitMap = window.initMap;

    window.initMap = function (mapInfos, worldName) {
        _originalInitMap(mapInfos, worldName);
        addRecordingButton();
    };

    function addRecordingButton() {
        if (!currentMap) { return; }
        if (recordButton) {
            recordButton.remove();
        }
        recordButton = L.control.overlayButton({
            content: '<i class="fas fa-circle"></i>',
            initialClass: isRecording ? 'btn-danger' : 'btn-outline-danger',
            position: 'bottomleft',
            click: function () { $('#recording').modal('show'); }
        });
        recordButton.addTo(currentMap);
        recordButton.j().attr('title', 'Session Recording');
    }

    function updateRecordingUI(status) {
        isRecording = status.isRecording;

        if (recordButton) {
            recordButton.setClass(isRecording ? 'btn-danger' : 'btn-outline-danger');
        }

        if (isRecording) {
            $('#recording-status-text').text('Recording in progress...');
            $('#recording-record-btn').html('<i class="fas fa-stop"></i> Stop recording').removeClass('btn-danger').addClass('btn-warning');
            $('#recording-download-section').addClass('d-none');
        } else {
            $('#recording-status-text').text('Not recording.');
            $('#recording-record-btn').html('<i class="fas fa-circle"></i> Start recording').removeClass('btn-warning').addClass('btn-danger');
            if (status.hasRecording) {
                $('#recording-download-link').attr('href', '/Session/Download?t=' + vm.token);
                $('#recording-download-section').removeClass('d-none');
            } else {
                $('#recording-download-section').addClass('d-none');
            }
        }
    }

    function startRecording() {
        fetch('/Session/Start?t=' + vm.token, { method: 'POST' })
            .then(function (r) { return r.json(); })
            .then(function (status) { updateRecordingUI(status); })
            .catch(function (e) { console.error('Start recording failed', e); });
    }

    function stopRecording() {
        fetch('/Session/Stop?t=' + vm.token, { method: 'POST' })
            .then(function (r) { return r.json(); })
            .then(function (status) { updateRecordingUI(status); })
            .catch(function (e) { console.error('Stop recording failed', e); });
    }

    $(function () {
        fetch('/Session/Status?t=' + vm.token)
            .then(function (r) { return r.json(); })
            .then(function (status) { updateRecordingUI(status); })
            .catch(function (e) { console.error('Status fetch failed', e); });

        $('#recording-record-btn').on('click', function () {
            if (isRecording) {
                stopRecording();
            } else {
                startRecording();
            }
        });
    });
}());
