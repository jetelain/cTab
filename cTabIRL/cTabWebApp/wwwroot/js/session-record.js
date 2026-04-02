(function () {
    var isRecording = false;

    function updateRecordingUI(status) {
        isRecording = status.isRecording;

        $('#recording-status-icon').toggleClass('d-none', !isRecording);
        $('#recording-status-text').toggleClass('d-none', !isRecording);

        if (isRecording) {
            $('#recording-record-btn').html('<i class="fas fa-stop"></i> Stop recording').removeClass('btn-danger').addClass('btn-warning');
            $('#recording-download-section').addClass('d-none');
        } else {
            $('#recording-record-btn').html('<i class="fas fa-circle"></i> Start recording').removeClass('btn-warning').addClass('btn-danger');
            if (status.hasRecording) {
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
