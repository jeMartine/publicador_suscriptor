import { api } from './client';

export const clientesAPI = {
    getAll: async () => {
        return await api.get('/clientes');
    },

    getByCedula: async (cedula) => {
        return await api.get(`/clientes/${cedula}`);
    },

    create: async (clienteData) => {
        return await api.post('/clientes', clienteData);
    },
};