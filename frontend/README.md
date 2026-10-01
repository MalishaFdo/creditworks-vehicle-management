# Vehicle Manager – Frontend

React app for the Vehicle Manager API. It can run on the same computer as the backend or on a
different one.

## Requirements

- [Node.js](https://nodejs.org/) 20.19+ or 22.12+
- The backend API running somewhere this computer can reach

## Setup

1. Install the packages (run inside this `frontend` folder):

   ```bash
   npm install
   ```

2. Tell the app where the backend is. Copy `.env.example` to `.env` and set `API_URL`:

   ```
   API_URL=http://192.168.1.20:5080
   ```

   Use the backend computer's IP address. If `.env` is missing, `http://localhost:5080` is used.

3. Start the app:

   ```bash
   npm start
   ```

   Open <http://localhost:5173>.

Restart `npm start` after changing `.env`; it is only read at start-up.

## Running the backend for another computer

By default the backend only accepts connections from its own computer. On the **backend** computer,
start it so it listens on the network:

```bash
dotnet run --project CWVehicleManagerAPI --urls http://0.0.0.0:5080
```

Then find that computer's IP address (`ipconfig` on Windows, `ipconfig getifaddr en0` on macOS) and
put it in the frontend's `.env`. If the frontend cannot connect, allow port 5080 through the
backend computer's firewall.

## How it works

The app calls paths like `/api/vehicles`. The development server (`vite.config.js`) forwards every
`/api` request to `API_URL`, so the browser only talks to this computer and no CORS setup is needed.
