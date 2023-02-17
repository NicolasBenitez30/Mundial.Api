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


const InstanciasPaises = document.getElementById('InstanciasPaises');

// let NoParticipo = 1
// let FaseDeGrupos = 2
// let OctavosDeFinal = 3

let data = {
    labels: [],
    datasets: [
        {
            label: 'Instancia',
            data: ["No Participo", "Fase De Grupos", "OctavosDeFinal", "Cuartos De Final", "Semifinal", "Tercer Puesto", "Subcampeon", "Campeon"]
        }
    ]
}

var grafico = new Chart(InstanciasPaises, {
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
                text: 'Grafico de Instancias'
            }
        }
    },
})

function actualizarGrafico() {
    InstanciasPaises.innerHTML = ""
    axios.get(baseUrl + '/paises')
        .then(function (response) {
            let Instancias = response.data;
            console.log(Instancias)
            grafico.data.labels = Instancias.map(x => x.nombre)
            grafico.data.datasets[0].data = Instancias.map(x => x.instancia)
            grafico.update()
        })
        .catch(function (error) {
            console.log(error);
        })
}