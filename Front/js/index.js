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
                paisesHtml += `Pais: ${paisesData[i].nombre} - Copa del Mundo: ${paisesData[i].mundial} <br/> `
            }
            paises.innerHTML = paisesHtml
            actualizarGrafico()
        })
        .catch(function (error) {
            console.log(error);
        })
}

let btnGuardarPais = document.getElementById("btnGuardarPais")

btnGuardarPais.addEventListener("click", (e) => {
    let nombrePais = document.getElementById("nombrePais")
    let mundialPais = document.getElementById("mundialPais")
    let pais = {
        nombre: nombrePais.value,
        mundial: mundialPais.value
    }
    if (pais.nombre !== '' && pais.nombre !== null &&
        pais.mundial !== '' && pais.mundial !== null) {
        axios.post(baseUrl + "/paises", pais)
            .then(function (response) {
                console.log(response)
                actualizarPaises()
            })
            .catch(function (error) {
                alert(error)
            })
        nombrePais.value = ''
        mundialPais.value = ''
    } else {
        alert('Verificar campo pais o mundial/es')
    }
})


const MundialesPaises = document.getElementById('MundialesPaises');

let data = {
    labels: [],
    datasets: [
        {
            label: 'Mundial/es',
            data: []
        }
    ],
    backgroundColor: []
};

var grafico = new Chart(MundialesPaises, {
    type: 'bar',
    data: data,
    options: {
        responsive: true,
        plugins: {
            legend: {
                position: 'top',
            },
            title: {
                display: true,
                text: 'Tabla Copas del Mundo'
            }
        }
    },
})

function actualizarGrafico() {
    MundialesPaises.innerHTML = ""
    axios.get(baseUrl + '/paises')
        .then(function (response) {
            let Copas = response.data;
            console.log(Copas)
            grafico.data.labels = Copas.map(x => x.nombre)
            grafico.data.datasets[0].data = Copas.map(x => x.mundial)
            grafico.data.datasets[0].backgroundColor = Copas.map(x => x.color)
            grafico.update()
        })
        .catch(function (error) {
            console.log(error);
        })
}