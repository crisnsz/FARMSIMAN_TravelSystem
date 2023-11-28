


let DataTableTravels;

const generateReportButton = document.getElementById("GenerateReport");
const transporterIDInput = document.getElementById("transporter_ID");
const reportIframe = document.getElementById("reportIframe");
const generateElement = document.getElementById("Generate");

const initialDateInput = document.getElementById("InitialDate");
const finalDateInput = document.getElementById("FinalDate");


const ModalGenReporte = document.getElementById("GenReporte");



document.addEventListener("DOMContentLoaded", setInitialAndFinalDateFields);

document.addEventListener("DOMContentLoaded", function () {


    DataTableTravels = new DataTable('#DataTable', {
        "searching": true,
        "oLanguage": {
            "oPaginate": {
                "sNext": "Siguiente",
                "sPrevious": "Anterior",
            },
            "sZeroRecords": "No se encontraron resultados",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sEmptyTable": "No hay registros",
            "sInfoEmpty": "Mostrando 0 de 0 Entradas",
            "sSearch": "Buscar",
            "sLengthMenu": "Mostrar _MENU_Registros Por Página",
            "sInfo": "Mostrando _START_ a _END_ Entradas"
        },

    });

    setInitialAndFinalDateFields();
});

//ModalGenReporte.addEventListener("click", async function () {



//    $('#ModalGenReporte').modal('show');


//    const myModal = document.getElementById('ModalGenReporte')
    
//});


function setInitialAndFinalDateFields() {
    const { formattedFirstDay, formattedLastDay } = getFirstAndLastDayOfCurrentMonth();
    initialDateInput.value = formattedFirstDay;
    finalDateInput.value = formattedLastDay;
}
function getFirstAndLastDayOfCurrentMonth() {

    const today = new Date(); // Get the current date
    const firstDay = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDay = new Date(today.getFullYear(), today.getMonth() + 1, 0);

    // Format the dates in YYYY-MM-DD format (required for input type date)
    const formattedFirstDay = firstDay.toISOString().split('T')[0];
    const formattedLastDay = lastDay.toISOString().split('T')[0];

    return { formattedFirstDay, formattedLastDay };
}


generateReportButton.addEventListener("click", async function () {
    try {

        console.log(DataTableTravels.rows().count())

        if (DataTableTravels.rows().count() <= 0) {
            alert("No hay registros");
            return;
        }


        const transporterID = transporterIDInput.value;
        const initialDate = initialDateInput.value;
        const finalDate = finalDateInput.value;

        if (!validateForm(transporterID, initialDate, finalDate)) {
            return;
        }

        const reportURL = `ReportTravel.aspx?transporter_ID=${transporterID}&initialDate=${initialDate}&finalDate=${finalDate}`;

        reportIframe.src = reportURL

        generateElement.classList.toggle("collapse");

    } catch (error) {
        console.error('Error:', error);
    }
});

async function GenerateReport(transporter_ID, initialDate, finalDate) {
    try {
        const response = await fetch("/Travel/GenerateReportJS", {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            body: JSON.stringify({ transporter_ID, initialDate, finalDate }),
        });
        if (!response.ok) {
            throw new Error("Request failed with status: " + response.status);
        }
        const data = await response.json();
        return data;
    } catch (error) {

        // Log the error
        console.error(error);

        // Display a user-friendly message
        alert("An error occurred. Please try again later.");

        // Rethrow the error for higher-level error handling
        throw error;
    }
}


function validateForm(transporter_ID, vinitialDate, vfinalDate) {
    // Check if the selected date is less than the current date.
    let initialDate = new Date(vinitialDate);
    let finalDate = new Date(vfinalDate);

    const isTransporter_IDValid = transporter_ID !== '';
    const isInitialDateValid = isValidDate(initialDate);
    const isFinalDateValid = isValidDate(finalDate);

    const transporterValidationSpan = document.getElementById("transporter_IDValidation").querySelector('span');
    const initialDateValidationSpan = document.getElementById("InitialDateValidation").querySelector('span');
    const finalDateValidationSpan = document.getElementById("FinalDateValidation").querySelector('span');

    transporterValidationSpan.textContent = isTransporter_IDValid ? '' : "Seleccione un Transportista";
    initialDateValidationSpan.textContent = isInitialDateValid ? '' : "Fecha Inválida";
    finalDateValidationSpan.textContent = isFinalDateValid ? '' : "Fecha Inválida";

    return isTransporter_IDValid && isInitialDateValid && isFinalDateValid;
}

function isValidDate(date) {
    return Object.prototype.toString.call(date) === "[object Date]" && !isNaN(date);
}