import { api } from './client';

export const turnosAPI = {
    // Obtener todos los turnos
    getAll: async () => {
        return await api.get('/turnos');
    },

    // Obtener turno por código
    getByCodigo: async (codigo) => {
        return await api.get(`/turnos/${codigo}`);
    },

    // Obtener turnos por estado
    getByEstado: async (estado) => {
        return await api.get(`/turnos/estado/${estado}`);
    },

    // Crear turno
    create: async (turnoData) => {
        return await api.post('/turnos', turnoData);
    },

    // Cambiar estado del turno
    cambiarEstado: async (codigo, nuevoEstado) => {
        return await api.put(`/turnos/${codigo}/estado`, { nuevoEstado });
    },
};