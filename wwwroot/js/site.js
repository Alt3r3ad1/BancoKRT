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
    input.value = input.value.replace(/[^0-9.]/g, '');

    if ((input.value.match(/\./g) || []).length > 1) {
        input.value = input.value.substr(0, input.value.lastIndexOf('.'));
    }

    if (parseFloat(input.value) < 0) {
        input.value = '';
    }
}

function validatePositiveNumberPaste(event) {
    event.preventDefault();
    const pasteData = event.clipboardData.getData('text');

    let positiveNumber = pasteData.replace(/[^0-9.]/g, '');

    if ((positiveNumber.match(/\./g) || []).length > 1) {
        positiveNumber = positiveNumber.substr(0, positiveNumber.lastIndexOf('.'));
    }

    if (parseFloat(positiveNumber) >= 0) {
        event.target.value = positiveNumber;
    }
}

function formatDecimalInput(input) {
    if (input.value !== '') {
        input.value = parseFloat(input.value).toFixed(2);
    }
}


