import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

const port = Number(process.env.PORT)

export default defineConfig({
    plugins: [react()],
    server: {
        // The page is served by the BFF, so the HMR client would otherwise open its
        // socket against the BFF origin. Point it straight at the Vite dev server.
        ws: { clientPort: port || undefined },
    },
})