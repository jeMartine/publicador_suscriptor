<script>
  import { onMount } from 'svelte';
  import { turnosAPI } from '../api/turnos'; // ajusta la ruta si tu API está en otro lugar

  let turnos = [];
  let loading = true;
  let error = '';

  // Columnas fijas (orden según pediste)
  const columns = [
    { key: 'tramites', title: 'Trámites generales' },
    { key: 'documentos', title: 'Solicitar documentos' },
    { key: 'caja', title: 'Transacciones en caja' },
    { key: 'asesoria', title: 'Asesoría' }
  ];

  // Objeto donde vamos a agrupar los turnos
  let grouped = {
    tramites: [],
    documentos: [],
    caja: [],
    asesoria: [],
    otros: []
  };

  onMount(() => cargarTurnos());

  async function cargarTurnos() {
    loading = true;
    error = '';
    try {
      const resp = await turnosAPI.getAll();
      // turnosAPI probablemente devuelve un objeto Axios -> resp.data
      const data = resp?.data ?? resp;
      turnos = Array.isArray(data) ? data : [];
      agruparTurnos();
    } catch (e) {
      console.error(e);
      error = 'No se pudieron cargar los turnos desde el servidor.';
    } finally {
      loading = false;
    }
  }

  function agruparTurnos() {
    // reset
    grouped = { tramites: [], documentos: [], caja: [], asesoria: [], otros: [] };

    for (const t of turnos) {
      const k = mapToColumn(t.servicio);
      grouped[k] = grouped[k] || [];
      grouped[k].push(t);
    }
  }

  // Mapea el objeto servicio a una de las keys de las columnas.
  // Usa servicio.codigo y servicio.nombre (robusto contra variantes).
  function mapToColumn(servicio) {
    if (!servicio) return 'otros';
    const code = (servicio.codigo || '').toLowerCase();
    const name = (servicio.nombre || '').toLowerCase();

    // Priorizar código (si tu backend usa códigos TG / AS etc)
    if (code === 'tg' || name.includes('tram')) return 'tramites';
    if (code === 'as' || name.includes('asesor') || name.includes('asesoría') || name.includes('asesoria')) return 'asesoria';
    if (name.includes('document') || name.includes('documento') || name.includes('solicit')) return 'documentos';
    if (name.includes('caja') || name.includes('transacc')) return 'caja';

    // fallback simple
    return 'otros';
  }

  // Marcar turno como atendido (o "sacarlo de la fila")
  async function sacarTurno(turno) {
    try {
      // asumo que el enum EstadoTurno usa 1 = Atendido (ajusta si tu enum difiere)
      await turnosAPI.cambiarEstado(turno.codigo, 1);
      await cargarTurnos();
    } catch (e) {
      console.error(e);
      error = 'No se pudo sacar el turno (cambiar estado).';
    }
  }
</script>

{#if loading}
  <p>Cargando turnos…</p>
{:else}
  {#if error}
    <div class="alert alert-error">{error}</div>
  {/if}

  <table class="turnos-table">
    <thead>
      <tr>
        {#each columns as col}
          <th>{col.title}</th>
        {/each}
      </tr>
    </thead>

    <tbody>
      <tr>
        {#each columns as col}
          <td>
            {#if grouped[col.key] && grouped[col.key].length > 0}
              <ul class="lista-turnos">
                {#each grouped[col.key] as turno}
                  <li class="turno-item">
                    <div class="datos">
                      <span class="codigo">{turno.codigo}</span>
                      <span class="nombre">{turno.cliente?.nombre ?? '—'}</span>
                    </div>
                    <div class="acciones">
                      <small class="estado">
                        {turno.estado === 0 ? 'Pendiente' : turno.estado === 1 ? 'Atendido' : 'Otro'}
                      </small>
                      <button on:click={() => sacarTurno(turno)} title="Sacar / marcar como atendido">
                        Sacar
                      </button>
                    </div>
                  </li>
                {/each}
              </ul>
            {:else}
              <div class="sin-turnos">—</div>
            {/if}
          </td>
        {/each}
      </tr>
    </tbody>
  </table>
{/if}

<style>
  .turnos-table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 1rem;
  }

  .turnos-table thead th {
    background: #0080ff;
    text-align: left;
    padding: 0.75rem;
    border-bottom: 1px solid #e2e8f0;
  }

  .turnos-table td {
    vertical-align: top;
    padding: 0.75rem;
    border-right: 1px solid #eef2f7;
    min-width: 200px;
    width: 25%; /* 4 columnas iguales */
  }

  .turnos-table td:last-child {
    border-right: none;
  }

  .lista-turnos {
    list-style: none;
    padding: 0;
    margin: 0;
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
  }

  .turno-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    background: white;
    padding: 0.5rem;
    border-radius: 6px;
    box-shadow: 0 1px 2px rgba(0,0,0,0.04);
  }

  .datos { display:flex; gap:0.5rem; align-items:center; }
  .codigo { font-weight: 700; margin-right: 0.5rem; }
  .nombre { color: #334155; }

  .acciones { display:flex; flex-direction:column; gap:0.25rem; align-items:flex-end; }
  .acciones button {
    padding: 0.25rem 0.5rem;
    border-radius: 4px;
    border: none;
    background: #ef4444;
    color: black;
    cursor: pointer;
    font-size: 0.85rem;
  }
  .acciones button:hover { background: #dc2626; }

  .sin-turnos { color:#94a3b8; font-style: italic; text-align:center; padding: 0.5rem; }
  .alert-error { background:#fee2e2; padding:0.5rem; border-radius:4px; color:#991b1b; margin-bottom:0.5rem; }
</style>
