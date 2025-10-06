import adapter from '@sveltejs/adapter-static';
import { vitePreprocess } from '@sveltejs/vite-plugin-svelte';

const config = {
	preprocess: vitePreprocess(),
	kit: {
		adapter: adapter({
            // Directorio donde se generarán los archivos estáticos
			pages: 'build', 
			assets: 'build',
            // CRUCIAL: Esto le dice al servidor (tu .NET backend) que si una ruta 
            // no existe (ej. /turnos/123), devuelva el index.html principal, 
            // permitiendo a SvelteKit manejar el routing en el lado del cliente.
			fallback: 'index.html', 
			precompress: false, // Generalmente no se necesita si .NET lo maneja
			strict: true
		}),
		alias: {
			$components: './src/components',
			$lib: './src/lib'
		}
	}
};

export default config;