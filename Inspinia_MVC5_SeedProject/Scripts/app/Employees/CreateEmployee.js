//import { arrayToJsonUser } from '../Utils/Utilities';


let tableNoAdded;
let tableAdded;

const ModalGenReporte = new bootstrap.Modal('#modalKM');


const MAX_KM = 50;
const MIN_KM = 1;

document.addEventListener("DOMContentLoaded", function () {
    tableNoAdded = new DataTable('#NoAdded', {
        "searching": false,
        "lengthChange": false,

        "order": [[0, 'asc']],
        "columnDefs": [{
            "targets": [0],
            "visible": false
        }],
        "oLanguage": {
            "oPaginate": {
                "sNext": "Siguiente",
                "sPrevious": "Anterior",
            },
            "sEmptyTable": "No hay registros",
            "sInfoEmpty": "Mostrando 0 de 0 Entradas",
            "sSearch": "Buscar",
            "sInfo": "Mostrando _START_ a _END_ Entradas",

        }
    });


    tableAdded = new DataTable('#Added', {
        "searching": false,
        "lengthChange": false,

        "order": [[0, 'asc']],
        "columnDefs": [{
            "targets": [0],
            "visible": false
        }],
        "oLanguage": {
            "oPaginate": {
                "sNext": "Siguiente",
                "sPrevious": "Anterior",
            },
            "sEmptyTable": "No hay registros",
            "sInfoEmpty": "Mostrando 0 de 0 Entradas",
            "sSearch": "Buscar",
            "sInfo": "Mostrando _START_ a _END_ Entradas",

        }
    });

});

document.getElementById("NoAdded").querySelector("tbody").addEventListener("click", function (event) {
    const target = event.target;

    if (target.tagName === "INPUT" && target.id === "AddSubsidiary") {
        

        let data = tableNoAdded.row(target.closest("tr")).data();
        let subsidiary_ID = data[0];

        console.log(data);

        ModalGenReporte.show();

        const myModal = document.getElementById('modalKM');
        const myInput = document.getElementById('km');

        myModal.addEventListener('shown.bs.modal', () => {
            myInput.focus();
        });

        document.getElementById("subsidiary_ID").value = subsidiary_ID;
        document.getElementById("dataButton").value = data;
    }
});



document.getElementById("btnAddSubsidiary").addEventListener("click", async function () {
    try {
        let subsidiary_ID = document.getElementById("subsidiary_ID").value;
        let data = document.getElementById("dataButton").value.split(',');;
        let KMVal = document.getElementById("km").value;

        if (!KMVal || KMVal < MIN_KM || KMVal > MAX_KM) {
            alert("Ingrese un número mayor que 0 y menor que 50");
            return;
        }

        const res = await syncToControllerAddPromise(subsidiary_ID, KMVal);

        console.log(res); // Data fetched from https://example.com/api/data

        if (!res || res.error) {
            // Handle null response or specific error case
            console.error(res ? res.errorMessage : "Response is null");
            return;
        }

        ModalGenReporte.hide();

        data[4] = '<input name="id02" type="button" id="DelSubsidiary" value="&#9668; Quitar &nbsp;&nbsp;" class="btn btn-primary btn-sm">'

        let newRowData = [data[0], data[1], data[2], KMVal, data[4]];
        //Cambiar id de #AddSubsidiary a #DelSubsidiary

        // Mueve la fila a tabla2
        tableAdded.row.add(newRowData).draw();

        // Elimina la fila de tabla1
        tableNoAdded.row(`[data-id="${data[0]}"]`).remove().draw();

        //Clean Data
        document.getElementById("subsidiary_ID").value = "";

        document.getElementById("dataButton").value = "";

        document.getElementById("km").value = "";
    } catch (error) {
        // Improved error handling
        throw new Error("Request failed: " + error.message);
    }
})


function handleAddSubsidiary(data, tableFrom, tableTo, kmId, btnId) {
    // Your common logic for adding a subsidiary
}

async function handleDelSubsidiary(data, row) {

    const jsonData = arrayToJsonUser(data);

    console.log(jsonData);


    const subsidiary_ID = jsonData.subsidiary_ID;
    const subsidiary_Name = jsonData.subsidiary_Name;

    const res = await syncToControllerRemovePromise(subsidiary_ID);

    console.log(res); // Data fetched from https://example.com/api/data

    if (!res || res.error) {
        // Handle null response or specific error case
        console.error(res ? res.errorMessage : "Response is null");
        return;
    }

    // Change id from #DelSubsidiary to #AddSubsidiary
    jsonData.subsidiary_Button = '<input name="id02" type="button" id="AddSubsidiary" value="Agregar &#9658" class="btn btn-primary btn-sm">'
    data[3] = jsonData.subsidiary_Button;

    // Move the row to table2
    tableNoAdded.row.add(data).draw();

    // Remove the row from table1
    tableAdded.row(row).remove().draw();
    tableNoAdded.order([0, 'asc']).draw();
}



async function syncToControllerAddPromise(id, km) {
    try {
        const subsidiary = {
            subsidiary_ID: id,
            employeeSubsidiary_DistanceKM: km,
        };

        const response = await fetch("/Employee/AddSubsidiary", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
            },
            body: JSON.stringify({ tbEmployeesSubsidiary: subsidiary }),
        });

        if (!response.ok) {
            throw new Error("Request failed with status: " + response.status);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        // Improved error handling
        throw new Error("Request failed: " + error.message);
    }
}


document.getElementById("Added").querySelector("tbody").addEventListener("click", async function (event) {
    const target = event.target;

    if (target.tagName === "INPUT" && target.id === "DelSubsidiary") {
        try {
            const row = target.closest("tr");
            const data = tableAdded.row(row).data();
            handleDelSubsidiary(data, row)

        } catch (error) {
            console.error(error); // Request failed
        }
    }
});

async function syncToControllerRemovePromise(id) {
    try {
        const subsidiary = {
            subsidiary_ID: id
        };

        const response = await fetch("/Employee/RemoveSubsidiary", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
            },
            body: JSON.stringify({ tbEmployeesSubsidiary: subsidiary }),
        });

        if (!response.ok) {
            throw new Error("Request failed with status: " + response.status);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        // Improved error handling
        throw new Error("Request failed: " + error.message);
    }
}

