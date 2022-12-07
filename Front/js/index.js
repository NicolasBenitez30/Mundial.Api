let baseUrl = "https://localhost:7197"

document.addEventListener("DOMContentLoaded", () => {
    actualizarPaises()
    actualizarParticipaciones()
    actualizarGrafico()
})

function actualizarPaises() {
    axios.get(baseUrl + '/paises')
        .then(function (response) {
            let paises = document.getElementById("paises")
            paisesData = response.data
            var paisesHtml = ''
            for (let i = 0; i < paisesData.length; i++) {
                paisesHtml += `Pais: ${paisesData[i].nombre} <br/> `
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
    let pais = {
        nombre: nombrePais.value
    }
    if (pais.nombre !== '' && pais.nombre !== null) {
        axios.post(baseUrl + "/paises", pais)
            .then(function (response) {
                console.log(response)
                actualizarPaises()
            })
            .catch(function (error) {
                alert(error)
            })
        nombrePais.value = ''
    } else {
        alert('Verificar campo pais')
    }
})

let btnGuardarParticipacion = document.getElementById("btnGuardarParticipacion")

btnGuardarParticipacion.addEventListener("click", (e) => {
    let participacionPais = document.getElementById("participacionPais")
    console.log(participacionPais.value);
    let participacionSede = document.getElementById("participacionSede")
    let participacionAño = document.getElementById("participacionAño")
    let participacionInstancia = document.getElementById("participacionInstancia")
    let participacion = {
        sede: participacionSede.value,
        año: participacionAño.value,
        instancia: participacionInstancia.value
    }
    let participaciones = []
    participaciones.push(participacion)

    console.log(participacion)
    if (participacion.sede !== '' && participacion.sede !== null &&
        participacion.año !== '' && participacion.año !== null &&
        participacion.instancia !== '' && participacion.instancia !== null) {
        axios.post(baseUrl + `/paises/${participacionPais.value}/participaciones`, participaciones)
            .then(function (response) {
                actualizarParticipaciones(participacionPais.value)
                // actualizarGrafico()  
            })
            .catch(function (error) {
                alert()
            })
        participacionPais.value = ''
        participacionSede.value = ''
        participacionAño.value = ''
        participacionInstancia.value = ''
    } else {
        alert('La participacion no es el correcto')
    }
})

function actualizarParticipaciones(nombrePais) {
    console.log(nombrePais)
    axios.get(baseUrl + `/paises/uruguay/participaciones`)
        .then(function (response) {
            let participaciones = document.getElementById("participaciones")
            participacionesData = response.data
            console.log(participacionesData[0].participaciones)
            var participacionesHtml = ''
            for (let i = 0; i < participacionesData[0].participaciones.length; i++) {
                participacionesHtml += `${participacionesData[0].nombre} - ${participacionesData[0].participaciones[i].sede} - ${participacionesData[0].participaciones[i].año} - ${participacionesData[0].participaciones[i].instancia} <br/> `
            }
            participaciones.innerHTML = participacionesHtml
        })
        .catch(function (error) {
            console.log(error);
        })
}

const accesosDia = document.getElementById('accesosdia');

var grafico = new Chart(accesosDia, {
    type: 'bar',
    data: {
        labels: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
        datasets: [{
            label: '# de accesos',
            data: [0, 0, 0, 0, 0, 0, 0],
            borderWidth: 1
        }]
    }
});

function actualizarGrafico() {
    accesosDia.innerHTML = ""
    axios.get(baseUrl + '/api/acceso/dia')
        .then(function (response) {
            let accesos = response.data;
            grafico.data.datasets[0].data = accesos.map(x => x.accesos)
            grafico.update()
        })
        .catch(function (error) {
            console.log(error);
        })
}