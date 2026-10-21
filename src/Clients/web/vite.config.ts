import { defineConfig, lazyPlugins } from "vite-plus";
import react from "@vitejs/plugin-react";
import { TanStackRouterVite } from "@tanstack/router-plugin/vite";

// Aspire serves the app through the BFF, so the HMR websocket must bypass the
// proxy and connect straight to the Vite dev server port that Aspire assigns.
const devServerPort = process.env.PORT ? Number(process.env.PORT) : undefined;

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    hmr: devServerPort ? { clientPort: devServerPort } : undefined,
  },
  fmt: { ignorePatterns: ["src/routeTree.gen.ts"] },
  lint: {
    ignorePatterns: ["src/routeTree.gen.ts"],
    jsPlugins: [{ name: "vite-plus", specifier: "vite-plus/oxlint-plugin" }],
    rules: { "vite-plus/prefer-vite-plus-imports": "error" },
    options: { typeAware: true, typeCheck: true },
  },
  plugins: lazyPlugins(() => [
    TanStackRouterVite({
      target: "react",
      autoCodeSplitting: true,
    }),
    react(),
  ]),
});
