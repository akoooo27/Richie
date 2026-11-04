import { globalIgnores } from "eslint/config";
import pluginRouter from "@tanstack/eslint-plugin-router";
import reactRefresh from "eslint-plugin-react-refresh";
import { config as reactConfig } from "./react-internal.js";

/**
 * ESLint configuration for Vite + React + TanStack Router apps.
 *
 * @type {import("eslint").Linter.Config[]}
 * */
export const viteConfig = [
  ...reactConfig,
  globalIgnores(["src/routeTree.gen.ts"]),
  ...pluginRouter.configs["flat/recommended"],
  reactRefresh.configs.vite,
  {
    // File routes must export `Route`; the TanStack Router Vite plugin handles their HMR
    files: ["src/routes/**"],
    rules: {
      "react-refresh/only-export-components": "off",
    },
  },
];
