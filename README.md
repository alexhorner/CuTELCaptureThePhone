# CuTEL Capture The Phone
*The addictive phone hunt and capture competition game!*

## Setup
- Build the `Dockerfile` under the tag `cutel/capturethephone:latest`.
- Place the `docker-compose.yaml` into a folder in which you want to deploy the game.
- Edit the `docker-compose.yaml` file with a new `POSTGRES_PASSWORD`.
- Launch the `docker-compose.yaml` without detach using `docker compose up` and monitor launch to see the PostgreSQL database has finished initialising. The game will keep failing with DB issues, which is to be expected.
- CTRL + C and `docker compose down` to stop the instance.
- Enter the `./data/capturethephone` directory and edit the `appsettings.json` file with a new `ApiKey` and the same DB password as you set for `POSTGRES_PASSWORD`.
- Deploy the Asterisk dialplan, pointing it to `localhost:5293` (or whatever you reconfigured the `docker-compose.yaml` file to) with the configured `ApiKey`.
- Log into the dashboard with new credentials and configure the whitelist and/or blacklist to allow phones.