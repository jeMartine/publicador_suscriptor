# 🏦 Sistema de Turnos Bancarios – Patrón Publicador/Suscriptor

Este proyecto implementa el **patrón de arquitectura Publicador–Suscriptor** utilizando **.NET 9**, **Svelte**, **Docker** y **MQTT (Mosquitto)**.

El sistema simula un banco donde los clientes solicitan turnos según el tipo de servicio requerido. Los turnos son gestionados por un **publicador**, que notifica la llegada de nuevos clientes, y un **suscriptor**, que recibe las actualizaciones en tiempo real.

---

## 📘 Descripción del Proyecto

La aplicación consiste en un sistema de asignación de turnos bancarios.  
Cada cliente se registra con su número de cédula, indicando si requiere asistencia especial y el servicio que necesita.

Los servicios disponibles son:

### 🧾 Trámites generales
- Solicitar certificados  
- Entregar o activar tarjetas  
- Activar primera clave  
- Modificar producto  
- Gestionar cheques  
- Modificar montos para transferencias  
- Gestionar débitos automáticos  
- Actualizar datos personales  
- Consultar información de cuenta AFC  

### 📄 Solicitar documentos

### 💰 Transacciones en caja
- Enviar, retirar y recibir dinero  
- Realizar pagos, giros y avances  
- Compensación para corresponsales bancarios  

### 👨‍💼 Asesoría
- Solicitar, modificar o pedir información general de productos y servicios financieros  

Al solicitar el turno, el sistema genera un código como `TG01` o `SD01`, donde:
- Las letras indican el tipo de servicio.  
- Los números indican la posición en la fila.  
- Si hay más de 99 personas, se añade una letra antes (por ejemplo: `TGA01`).  

El cliente espera en la sala hasta que su turno sea asignado a un asesor.

---

## 🧩 Arquitectura del Sistema

El proyecto aplica el **patrón Publicador–Suscriptor** de la siguiente forma:

| Componente | Descripción |
|-------------|-------------|
| **Mosquitto (MQTT Broker)** | Intermediario de mensajería entre publicador y suscriptor. |
| **Publisher (.NET + Svelte)** | Envía mensajes (nuevos turnos creados) al broker MQTT. |
| **Subscriber (.NET + Svelte)** | Escucha los mensajes del broker y actualiza su vista en tiempo real. |

---

## ⚙️ Requisitos Previos

---
- [Docker Desktop](https://www.docker.com/)
- [Git](https://git-scm.com/)
- (Opcional) [Visual Studio Code](https://code.visualstudio.com/) o [Visual Studio 2022](https://visualstudio.microsoft.com/) para edición del código.

---

## 🚀 Instalación y Ejecución

### 1️⃣ Clonar el repositorio

```console
git clone https://github.com/tu_usuario/taller2-arquitectura.git
cd taller2-arquitectura
```

2️⃣ Construir y levantar los contenedores

```bash
docker-compose up --build

```
Esto levantará tres contenedores:

---
mqtt-broker: servicio Mosquitto en el puerto 1883 (TCP) y 9001 (WebSocket)

publisher: backend y frontend del publicador en el puerto 5000

subscriber: backend y frontend del suscriptor en el puerto 5001


## 🌐 Acceso a los Servicios
Servicio	URL	Descripción
Frontend Publicador	http://localhost:5000	Interfaz para registrar nuevos turnos.

Frontend Suscriptor	http://localhost:5001	Interfaz que muestra en tiempo real los turnos asignados.

MQTT Broker	localhost:1883 o localhost:9001	Canal de comunicación entre publicador y suscriptor.

## 🧠 Funcionamiento del Patrón
Publicador:
Cuando un cliente solicita un turno, el publicador publica un mensaje con los datos del nuevo turno (por ejemplo, { "codigo": "TG01", "servicio": "Trámites Generales" }).

Broker MQTT (Mosquitto):
Recibe el mensaje y lo reenvía a todos los suscriptores conectados al tópico correspondiente.

Suscriptor:
Escucha el tópico y actualiza su vista mostrando el nuevo turno disponible.

## 🧩 Tecnologías Utilizadas
- .NET 9 (ASP.NET Core Minimal APIs)
- Svelte (Frontend Framework)
- Docker y Docker Compose
- Mosquitto (MQTT Broker)
- C# / JavaScript / HTML / CSS

## 🧪 Pruebas
Puedes verificar el funcionamiento conectando un cliente MQTT (por ejemplo, MQTT Explorer o mosquitto_sub) al broker local (localhost:1883) y suscribiéndote al tópico de turnos.

## 📚 Créditos
Proyecto académico desarrollado para la materia Arquitectura de Software, demostrando la aplicación del patrón Publicador–Suscriptor en un sistema distribuido real.

## 📝 Licencia
Este proyecto se distribuye bajo la licencia MIT.
Puedes usarlo, modificarlo y distribuirlo libremente dando crédito al autor original.
