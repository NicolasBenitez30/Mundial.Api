let baseUrl = "https://localhost:7197"

document.addEventListener("DOMContentLoaded", () => {
    actualizarPaises()
    actualizarGrafico()
})

function actualizarPaises() {
    axios.get(baseUrl + '/paises')
        .then(function (response) {
            let paises = document.getElementById("paises")
            paisesData = response.data
            var paisesHtml = ''
            for (let i = 0; i < paisesData.length; i++) {
                paisesHtml += `Pais: ${paisesData[i].nombre} - ${paisesData[i].instancia} <br/> `
            }
            paises.innerHTML = paisesHtml
        })
        .catch(function (error) {
            console.log(error);
        })
}

let btnGuardarPais = document.getElementById("btnGuardarPais")

btnGuardarPais.addEventListener("click", (e) => {
    let nombrePais = document.getElementById("nombrePais")
    let instanciaPais = document.getElementById("instanciaPais")
    let pais = {
        nombre: nombrePais.value,
        instancia: instanciaPais.value
    }
    if (pais.nombre !== '' && pais.nombre !== null &&
        pais.instancia !== '' && pais.instancia !== null) {
        axios.post(baseUrl + "/paises", pais)
            .then(function (response) {
                console.log(response)
                actualizarPaises()
            })
            .catch(function (error) {
                alert(error)
            })
        nombrePais.value = ''
        instanciaPais.value = ''
    } else {
        alert('Verificar campo pais o instancia')
    }
})


const parInstancia = document.getElementById('parInstancia');

var grafico = new Chart(parInstancia, {
    type: 'bar',
    data: {
        data: ["NO PARTICIPO", "FASE DE GRUPOS", "OCTAVOS DE FINAL", "CUARTOS DE FINAL", "SEMIFINAL", "TERCER PUESTO", "SUBCAMPEÓN", "CAMPEÓN"],
        labels: [0, 0, 0, 0, 0, 0, 0],
        datasets: [{
            label: '# de Instancias',
            borderWidth: 1
        }]
    }
});
function grafico() {
    grafico.data.labels[i].data = participaciones.map(x => x.año)
}

function actualizarGrafico() {
    parInstancia.innerHTML = ""
    axios.get(baseUrl + '/grafico')
        .then(function (response) {
            let participaciones = response.data;
            grafico.data.datasets[0].data = participaciones.map(x => x.participaciones)
            grafico.update()
        })
        .catch(function (error) {
            console.log(error);
        })
}