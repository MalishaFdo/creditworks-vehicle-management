import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig(({ mode }) => {
  // Where the backend API is. Set API_URL in frontend/.env (see .env.example).
  // If it is not set, the API on this computer is used.
  const env = loadEnv(mode, process.cwd(), "");
  const apiUrl = env.API_URL || "https://localhost:7052";

  return {
    plugins: [react()],
    server: {
      port: 5173,
      // Send every request that starts with /api to the backend API.
      proxy: {
        "/api": {
          target: apiUrl,
          changeOrigin: true,
          // Accept the backend's self-signed .NET development certificate when API_URL is https://.
          secure: false,
        },
      },
    },
  };
});
