<!-- <script>
    import { turnosAPI } from '../api/turnos'; 
    import { serviciosAPI } from '../api/servicios'; 
    import { onMount } from 'svelte';

    let servicios = [];
    let loading = false;
    let error = null;
    let success = false;
    let codigoTurnoCreado = ''; 

    let formData = {
        cliente: {
            cedula: '',
            nombre: '',
            requiereAsistencia: false
        },
        codigoServicio: ''
    };

    onMount(async () => {
        try {
            servicios = await serviciosAPI.getAll();
        } catch (e) {
            error = 'Error al cargar servicios';
        }
    });

    async function handleSubmit() {
        loading = true;
        error = null;
        success = false;
        codigoTurnoCreado ='';
        try {
            const turno = await turnosAPI.create(formData);
            codigoTurnoCreado = turno.codigo;

            success = true;
            
            // Limpiar formulario
            formData = {
                cliente: { cedula: '', nombre: '', requiereAsistencia: false },
                codigoServicio: ''
            };

            // Opcional: emitir evento
            // dispatch('turnoCreado', turno);
        } catch (e) {
            error = e.message;
        } finally {
            loading = false;
        }
    }
</script>

<form on:submit|preventDefault={handleSubmit} class="formulario">
    <h2>Crear Nuevo Turno</h2>

    {#if error}
        <div class="alert alert-error">{error}</div>
    {/if}

    {#if success}
        <div class="alert alert-success">
            ¡Turno creado exitosamente! Su número de turno es: 
            <strong>{codigoTurnoCreado}</strong>
        </div>
    {/if}

    <div class="form-group">
        <label for="cedula">Cédula</label>
        <input
            id="cedula"
            type="text"
            bind:value={formData.cliente.cedula}
            required
            placeholder="123456789"
        />
    </div>

    <div class="form-group">
        <label for="nombre">Nombre</label>
        <input
            id="nombre"
            type="text"
            bind:value={formData.cliente.nombre}
            required
            placeholder="Juan Pérez"
        />
    </div>

    <div class="form-group checkbox">
        <label>
            <input
                type="checkbox"
                bind:checked={formData.cliente.requiereAsistencia}
            />
            Requiere asistencia
        </label>
    </div>

    <div class="form-group">
        <label for="servicio">Servicio</label>
        <select
            id="servicio"
            bind:value={formData.codigoServicio}
            required
        >
            <option value="">Seleccione un servicio</option>
            {#each servicios as servicio}
                <option value={servicio.codigo}>
                    {servicio.nombre}
                </option>
            {/each}
        </select>
    </div>

    <button type="submit" disabled={loading}>
        {loading ? 'Creando...' : 'Crear Turno'}
    </button>
</form>

<style>
    .formulario {
        max-width: 500px;
        margin: 0 auto;
        padding: 2rem;
        background: white;
        border-radius: 8px;
        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    }

    .form-group {
        margin-bottom: 1rem;
    }

    label {
        display: block;
        margin-bottom: 0.5rem;
        font-weight: 500;
    }

    input, select {
        width: 100%;
        padding: 0.5rem;
        border: 1px solid #ddd;
        border-radius: 4px;
    }

    button {
        width: 100%;
        padding: 0.75rem;
        background: #007bff;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }

    button:disabled {
        background: #ccc;
        cursor: not-allowed;
    }

    .alert {
        padding: 1rem;
        border-radius: 4px;
        margin-bottom: 1rem;
    }

    .alert-error {
        background: #fee;
        color: #c00;
    }

    .alert-success {
        background: #efe;
        color: #0a0;
    }
</style> -->