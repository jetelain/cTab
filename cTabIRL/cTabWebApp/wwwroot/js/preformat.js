// TEMP - This is a temporary file to hold the preformatting code until it can be integrated into maps.plan-ops.fr.
class MessageTemplateUI {
    constructor(composeFormFields, composeText, helper, defaultValues) {
        this.bootstrapVersion = 4;
        this.composeFormFields = composeFormFields;
        this.composeText = composeText;
        this.lastValues = defaultValues || {};
        this.helper = helper;
    }
    clear() {
        this.composeFormFields.innerHTML = '';
        this.composeText.readOnly = false;
        this.config = null;
    }
    getText() {
        if (!this.config) {
            return '';
        }
        var data = [];
        this.config.lines.forEach((line, lnum) => {
            var lineData = line.title ? line.title + ':' : '';
            line.fields.forEach((field, fnum) => {
                var id = 'l' + lnum + 'f' + fnum;
                var element = document.getElementById(id);
                switch (field.type) {
                    case 'checkbox':
                    case 'CheckBox':
                        if (element.checked) {
                            lineData = lineData + ' ' + field.title;
                            document.getElementById(id + '-box').classList.add('bg-primary', 'text-white');
                        }
                        else {
                            document.getElementById(id + '-box').classList.remove('bg-primary', 'text-white');
                        }
                        break;
                    default:
                        var value = ('' + element.value).trim();
                        if (value && value.length > 0) {
                            lineData = lineData + ' ' + (field.title || '') + value;
                            document.getElementById(id + '-box').classList.add('bg-primary', 'text-white');
                        }
                        else {
                            document.getElementById(id + '-box').classList.remove('bg-primary', 'text-white');
                        }
                        switch (field.type) {
                            case 'callsign':
                            case 'CallSign':
                            case 'frequency':
                            case 'Frequency':
                                this.lastValues[field.type.toLocaleLowerCase()] = value;
                                break;
                        }
                        break;
                }
            });
            data.push(lineData);
        });
        return data.join('\n');
    }
    updateComposeText() {
        if (this.config) {
            this.composeText.value = this.getText();
        }
    }
    setup(config) {
        this.config = config;
        this.composeText.readOnly = true;
        this.composeFormFields.innerHTML = '';
        const generatePreformated = this.updateComposeText.bind(this);
        config.lines.forEach((line, lnum) => {
            const fieldsDiv = document.createElement('div');
            fieldsDiv.className = 'form-inline';
            line.fields.forEach((field, fnum) => {
                const id = 'l' + lnum + 'f' + fnum;
                var width = '7em';
                if (line.fields.length == 1) {
                    width = '15em';
                }
                switch (field.type) {
                    case 'checkbox':
                    case 'CheckBox':
                        fieldsDiv.appendChild(this.generateCheckBox(id, generatePreformated, field));
                        break;
                    default:
                        fieldsDiv.appendChild(this.generateInput(id, generatePreformated, field, width));
                        break;
                }
            });
            var colDiv = document.createElement('div');
            colDiv.className = 'col';
            colDiv.textContent = line.title ? line.title + ': ' + (line.description || '') : (line.description || '');
            colDiv.appendChild(fieldsDiv);
            this.composeFormFields.appendChild(colDiv);
        });
        this.updateComposeText();
    }
    generateCheckBox(id, generatePreformated, field) {
        var inputGroup = document.createElement('div');
        inputGroup.className = 'input-group input-group-sm mb-2 mr-sm-2';
        var inputGroupPrepend = document.createElement('div');
        inputGroupPrepend.className = 'input-group-prepend';
        var inputGroupText = document.createElement('div');
        inputGroupText.className = 'input-group-text';
        inputGroupText.id = id + '-box';
        var checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.id = id;
        checkbox.addEventListener('click', generatePreformated);
        var label = document.createElement('label');
        label.className = 'form-check-label ml-1';
        label.htmlFor = id;
        label.textContent = field.title || '';
        inputGroupText.appendChild(checkbox);
        inputGroupText.appendChild(label);
        inputGroupPrepend.appendChild(inputGroupText);
        var descriptionLabel = document.createElement('label');
        descriptionLabel.className = 'form-control bg-light';
        descriptionLabel.htmlFor = id;
        descriptionLabel.textContent = field.description || '';
        inputGroup.appendChild(inputGroupPrepend);
        inputGroup.appendChild(descriptionLabel);
        return inputGroup;
    }
    generateInput(id, generatePreformated, field, width) {
        var inputGroup = document.createElement('div');
        inputGroup.className = 'input-group input-group-sm mb-2 mr-sm-2';
        var inputGroupPrepend = document.createElement('div');
        inputGroupPrepend.className = 'input-group-prepend';
        var inputGroupText = document.createElement('label');
        inputGroupText.className = 'input-group-text';
        inputGroupText.htmlFor = id;
        inputGroupText.id = id + '-box';
        inputGroupText.textContent = field.title || '';
        var input = document.createElement('input');
        input.className = 'form-control';
        input.id = id;
        input.placeholder = field.description;
        this.setupInputTypeSpecific(input, field);
        input.style.width = width;
        input.addEventListener('change', generatePreformated);
        input.addEventListener('keyup', generatePreformated);
        inputGroupPrepend.appendChild(inputGroupText);
        inputGroup.appendChild(inputGroupPrepend);
        inputGroup.appendChild(input);
        this.generateHelperButtons(field, inputGroup, input);
        return inputGroup;
    }
    generateHelperButtons(field, inputGroup, input) {
        if (this.helper) {
            let inputGroupAppend = inputGroup;
            if (this.bootstrapVersion < 5) {
                inputGroupAppend = document.createElement('div');
                inputGroupAppend.className = 'input-group-append';
            }
            switch (field.type) {
                case 'utm':
                case 'Grid':
                case 'GridNoMarker':
                    if (this.helper.getCurrentLocation) {
                        var locationButton = document.createElement('button');
                        locationButton.className = 'btn btn-outline-secondary';
                        locationButton.textContent = 'Here';
                        locationButton.addEventListener('click', () => {
                            input.value = this.helper.getCurrentLocation();
                            input.dispatchEvent(new Event('change'));
                        });
                        inputGroupAppend.appendChild(locationButton);
                    }
                    if (this.helper.promptLocation) {
                        var locationButton = document.createElement('button');
                        locationButton.className = 'btn btn-outline-secondary';
                        locationButton.textContent = 'Find';
                        locationButton.addEventListener('click', async () => {
                            var currentValue = input.value;
                            var newValue = await this.helper.promptLocation(currentValue);
                            if (newValue) {
                                input.value = newValue;
                                input.dispatchEvent(new Event('change'));
                            }
                        });
                        inputGroupAppend.appendChild(locationButton);
                    }
                    break;
                case 'frequency':
                case 'Frequency':
                    if (this.helper.getCurrentFrequency) {
                        var frequencyButton = document.createElement('button');
                        frequencyButton.className = 'btn btn-outline-secondary';
                        frequencyButton.textContent = 'Current';
                        frequencyButton.addEventListener('click', () => {
                            input.value = this.helper.getCurrentFrequency();
                            input.dispatchEvent(new Event('change'));
                        });
                        inputGroupAppend.appendChild(frequencyButton);
                    }
                    if (this.helper.promptFrequency) {
                        var frequencyButton = document.createElement('button');
                        frequencyButton.className = 'btn btn-outline-secondary';
                        frequencyButton.textContent = 'List';
                        frequencyButton.addEventListener('click', async () => {
                            var currentValue = input.value;
                            var newValue = await this.helper.promptFrequency(currentValue);
                            if (newValue) {
                                input.value = newValue;
                                input.dispatchEvent(new Event('change'));
                            }
                        });
                        inputGroupAppend.appendChild(frequencyButton);
                    }
                    break;
                case 'DateTime':
                    if (this.helper.getCurrentDatetime) {
                        var datetimeButton = document.createElement('button');
                        datetimeButton.className = 'btn btn-outline-secondary';
                        datetimeButton.textContent = 'Now';
                        datetimeButton.addEventListener('click', () => {
                            input.value = this.helper.getCurrentDatetime();
                            input.dispatchEvent(new Event('change'));
                        });
                        inputGroupAppend.appendChild(datetimeButton);
                    }
                    break;
            }
            if (this.bootstrapVersion < 5 && inputGroupAppend.children.length > 0) {
                inputGroup.appendChild(inputGroupAppend);
            }
        }
    }
    setupInputTypeSpecific(input, field) {
        switch (field.type) {
            case 'utm':
            case 'Grid':
            case 'GridNoMarker':
                input.type = 'text';
                input.value = (this.helper && this.helper.getCurrentLocation) ? this.helper.getCurrentLocation() : '';
                break;
            case 'callsign':
            case 'CallSign':
                input.type = 'text';
                input.value = this.lastValues['callsign'] || (this.helper && this.helper.getCallSign) ? this.helper.getCallSign() : '';
                break;
            case 'frequency':
            case 'Frequency':
                input.type = 'number';
                input.step = '0.025';
                input.value = this.lastValues['frequency'] || (this.helper && this.helper.getCurrentFrequency) ? this.helper.getCurrentFrequency() : '';
                break;
            case 'number':
            case 'Number':
                input.type = 'number';
                break;
            case 'DateTime':
                input.type = 'datetime-local';
                input.value = (this.helper && this.helper.getCurrentDatetime) ? this.helper.getCurrentDatetime() : '';
                break;
            default:
                input.type = 'text';
                break;
        }
    }
}
function showPerformated(config) {
    new MessageTemplateUI(document.getElementById('compose-form-fields'), document.getElementById('compose-text')).setup(config);
}
