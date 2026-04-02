(function () {
    var isRecording = false;

    function updateRecordingUI(status) {
        isRecording = status.isRecording;

        document.getElementById('recording-status-icon').classList.toggle('d-none', !isRecording);
        document.getElementById('recording-status-text').classList.toggle('d-none', !isRecording);

        document.getElementById('recording-start-btn').classList.toggle('d-none', isRecording);
        document.getElementById('recording-stop-btn').classList.toggle('d-none', !isRecording);

        document.getElementById('recording-download-section').classList.toggle('d-none', isRecording || !status.hasRecording);
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

    document.addEventListener('DOMContentLoaded', function () {
        fetch('/Session/Status?t=' + vm.token)
            .then(function (r) { return r.json(); })
            .then(function (status) { updateRecordingUI(status); })
            .catch(function (e) { console.error('Status fetch failed', e); });

        document.getElementById('recording-start-btn').addEventListener('click', startRecording);
        document.getElementById('recording-stop-btn').addEventListener('click', stopRecording);
    });
}());
