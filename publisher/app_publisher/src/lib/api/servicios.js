import { api } from './client';

export const serviciosAPI = {
    getAll: async () => {
        return await api.get('/servicio');
    },

    getByCodigo: async (codigo) => {
        return await api.get(`/servicio/${codigo}`);
    },

    create: async (servicioData) => {
        return await api.post('/servicio', servicioData);
    },
};