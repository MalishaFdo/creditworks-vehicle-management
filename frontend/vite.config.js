import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig(({ mode }) => {
  //Get the backend API URL or use the local API.
  const env = loadEnv(mode, process.cwd(), "");
  const apiUrl = env.API_URL || "https://localhost:7052";

  return {
    plugins: [react()],
    server: {
      port: 5173,
      //Send /api requests to the backend.
      proxy: {
        "/api": {
          target: apiUrl,
          changeOrigin: true,
          secure: false,
        },
      },
    },
  };
});
