function formatCPF(input) {
    let cpf = input.value.replace(/\D/g, '');

    cpf = cpf.replace(/^(\d{3})(\d)/, '$1.$2');
    cpf = cpf.replace(/^(\d{3})\.(\d{3})(\d)/, '$1.$2.$3');
    cpf = cpf.replace(/\.(\d{3})(\d)/, '.$1-$2');

    input.value = cpf;
}

const CPF = document.getElementById('CPF')
if (CPF !== null) {
    document.getElementById('CPF').addEventListener('paste', (event) => {
        event.preventDefault();

        const pasteData = event.clipboardData.getData('text');

        const formattedData = pasteData.replace(/\D/g, '')
            .replace(/^(\d{3})(\d)/, '$1.$2')
            .replace(/^(\d{3})\.(\d{3})(\d)/, '$1.$2.$3')
            .replace(/\.(\d{3})(\d)/, '.$1-$2');

        event.target.value = formattedData;
    });
}

function validatePositiveNumber(input) {
    if (input.value < 0) {
        input.value = '';
    }
}

function validatePositiveNumberPaste(event) {
    event.preventDefault();
    const pasteData = event.clipboardData.getData('text');
    const positiveNumber = pasteData.replace(/[^0-9]/g, '');
    if (positiveNumber.length > 0) {
        event.target.value = positiveNumber;
    }
}

function formatValue(input) {
    let value = input.value.replace(/\D/g, '');
    value = (parseFloat(value) / 100).toFixed(2);
    input.value = value.replace(',', '.').replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

function formatValuePaste(event) {
    event.preventDefault();
    const pasteData = event.clipboardData.getData('text');
    const numericData = pasteData.replace(/\D/g, '');
    const formattedValue = (parseFloat(numericData) / 100).toFixed(2);
    const finalValue = formattedValue.replace(',', '.').replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    event.target.value = finalValue;
}