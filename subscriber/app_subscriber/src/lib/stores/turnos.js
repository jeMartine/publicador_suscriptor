import { writable } from 'svelte/store';
import { turnosAPI } from '../api/turnos';

function createTurnosStore() {
    const { subscribe, set, update } = writable({
        turnos: [],
        loading: false,
        error: null
    });

    return {
        subscribe,
        
        fetchAll: async () => {
            update(state => ({ ...state, loading: true, error: null }));
            try {
                const turnos = await turnosAPI.getAll();
                update(state => ({ ...state, turnos, loading: false }));
            } catch (error) {
                update(state => ({ ...state, error: error.message, loading: false }));
            }
        },

        create: async (turnoData) => {
            update(state => ({ ...state, loading: true, error: null }));
            try {
                const nuevoTurno = await turnosAPI.create(turnoData);
                update(state => ({
                    ...state,
                    turnos: [...state.turnos, nuevoTurno],
                    loading: false
                }));
                return nuevoTurno;
            } catch (error) {
                update(state => ({ ...state, error: error.message, loading: false }));
                throw error;
            }
        },

        cambiarEstado: async (codigo, nuevoEstado) => {
            try {
                const turnoActualizado = await turnosAPI.cambiarEstado(codigo, nuevoEstado);
                update(state => ({
                    ...state,
                    turnos: state.turnos.map(t => 
                        t.codigo === codigo ? turnoActualizado : t
                    )
                }));
            } catch (error) {
                update(state => ({ ...state, error: error.message }));
                throw error;
            }
        }
    };
}

export const turnosStore = createTurnosStore();