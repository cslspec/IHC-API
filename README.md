# IHC API

## About

The LK IHC Controller was a programmable building automation platform developed by Lauritz Knudsen and later maintained by Schneider Electric. For many years, it was one of the most widely deployed residential automation systems in Denmark, providing centralized control of lighting, switches, relays, sensors, and other building functions.

With the introduction of Ethernet-enabled controller models, the platform gained support for IP-based communication through a built-in Viewer module, allowing external systems to monitor and interact with controller resources over a standard IPv4 network. This capability made integration with third-party software and home automation platforms possible without requiring direct access to the underlying IHC bus.

Although the controller hardware reached end-of-life in mid-2023 and is no longer installed in new projects, a significant installed base remains in active operation. Many existing installations continue to provide reliable service, and replacement components are still available through established distribution channels.

This project provides a modern API for accessing and controlling LK IHC Controller systems. Its purpose is to expose controller functionality through a documented and developer-friendly interface, enabling integration with contemporary automation systems, custom applications, monitoring solutions, and home automation platforms.

The API has been developed and tested against Viewer version 6.1. Communication is performed over a standard TCP/IP network, allowing applications to interact with controller resources without requiring modifications to the existing installation.

By providing modern programmatic access to a mature and proven automation platform, this project helps extend the operational lifetime of existing LK IHC installations and enables their continued integration into today's connected building environments.

## Acknowledgements

Thanks to these projects and contributors for documenting LK IHC and exploring its integrations and communication protocols:

- [Jens Østergaard Nielsen (@dingusdk)](https://github.com/dingusdk), especially for the Home Assistant integration.
- [IHC® Captain](https://jemi.dk/ihc/), a PHP solution for working with LK IHC Controllers.
- [IHCClient](https://github.com/priiduonu/ihcclient), which connects older non-Ethernet controllers to Home Assistant.
- [IHCClientSDK](https://github.com/mmc41/IHCClientSDK), an open-source SDK for LK IHC communication.

## REST API

The IHC controller exposes its functionality through a set of SOAP web services (`/ws/AuthenticationService`, `/ws/ControllerService`, `/ws/TimeManagerService`, ...). IHC API is an ASP.NET Core web application that sits between your client and the controller: it logs in with a configured user account, calls the SOAP services on your behalf and returns plain JSON. It also downloads the project file from the controller and parses it into a structured model of groups, products, resources and scenes.

## Features

The API currently offers read access to the controller, resource values, user management and a few configuration updates:

| Area | Endpoints | Description |
| --- | --- | --- |
| Project | `GET /api/project/available`<br>`GET /api/project/info`<br>`GET /api/project/file`<br>`GET /api/project/model` | Project availability and metadata, the raw project XML, and the parsed project model. |
| Resources | `GET /api/resources/{resourceId}/runtime` | The current runtime value of a resource, such as an input, output or dimmer level. Resource IDs can be found in the project model. |
| Time | `GET /api/time/uptime`<br>`GET /api/time/localtime`<br>`GET` / `POST /api/time/settings`<br>`POST /api/time/settings/test` | Uptime, controller clock, reading and updating time settings, and a test of the configured time server. When updating, fields that are left out keep their current value, and a time server is tested before synchronization is enabled. |
| Configuration | `GET /api/config/system`<br>`GET /api/config/network`<br>`GET /api/config/dns`<br>`GET` / `POST /api/config/smtp`<br>`GET /api/config/email`<br>`GET /api/config/email/enable`<br>`GET /api/config/access` | System information, network and DNS settings, SMTP and email settings, and web access control. |
| Session | `POST /api/config/logout` | Logs out of the controller and clears the cached session. |
| Users | `GET /api/users`<br>`POST /api/users`<br>`PUT /api/users/{username}`<br>`DELETE /api/users/{username}` | List, add, update and remove users on the controller. Passwords are never returned. When updating, fields that are left out keep their current value. The `admin` user and the user the API logs in with cannot be removed. |

The full, up-to-date endpoint reference with request and response schemas is available in the built-in API documentation (see [API documentation](#api-documentation)).

### Sessions

You do not log in to IHC API itself. The API logs in to the controller with the credentials from the configuration the first time a request needs it, and reuses that session for 20 minutes before logging in again. Call `POST /api/config/logout` to end the session immediately.

### Errors

Errors are returned as [RFC 9457 problem details](https://www.rfc-editor.org/rfc/rfc9457):

- `400 Bad Request`: the request is invalid, for example when adding a user whose username already exists or removing the `admin` user.
- `404 Not Found`: the requested user or resource does not exist.
- `500 Internal Server Error`: the controller returned an error or an unexpected response.
- `503 Service Unavailable`: the controller cannot be reached or rejected the login (invalid account, connection restrictions or insufficient user rights).

> **Security note:** IHC API has no authentication of its own. Anyone who can reach the API can use it with the rights of the configured controller user. Only run it on a trusted network.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download) to build and run from source, or Docker.
- An IHC controller reachable over the network, and a user account on it.

## Configuration

Controller settings live in the `controller` section of [Ihc.WebApi/appsettings.json](Ihc.WebApi/appsettings.json):

```json
"controller": {
  "Address": "http://192.168.1.2",
  "UserName": "<INSERT USERNAME>",
  "Password": "<INSERT PASSWORD>"
}
```

| Setting | Description |
| --- | --- |
| `Address` | Base URL of the IHC controller, including `http://` or `https://`. |
| `UserName` | User name of an account on the controller. |
| `Password` | Password for that account. |

The application does not start if the `controller` section is missing.

To keep credentials out of the file, use environment variables instead. ASP.NET Core maps `controller__Address`, `controller__UserName`, `controller__Password` and `controller__Application` onto the same settings, and environment variables take precedence over `appsettings.json`.

By default the API listens on `http://*:8080`. Change `Kestrel:Endpoints:Http:Url` in `appsettings.json` to use another port.

## Running from source

```sh
git clone https://github.com/cslspec/IHC-API.git
cd IHC-API
dotnet run --project Ihc.WebApi
```

Then open http://localhost:8080/scalar in a browser.

## Running tests

Run the unit tests with:

```sh
dotnet test IHC.slnx
```

## Running with Docker

Build the image from the repository root:

```sh
docker build -t local/ihc-api -f Dockerfile .
```

Run it, passing the controller settings as environment variables:

```sh
docker run -it --rm -p 8080:8080 \
  -e controller__Address=http://192.168.0.2 \
  -e controller__UserName=myuser \
  -e controller__Password=mypassword \
  local/ihc-api
```

On Linux you may need to prefix the commands with `sudo`.

## API documentation

While the API is running, interactive documentation is available at:

- Scalar: http://localhost:8080/scalar
- Swagger UI: http://localhost:8080/swagger
- OpenAPI document: http://localhost:8080/openapi/v1.json

Example request:

```sh
curl http://localhost:8080/api/project/info
```

Runtime values carry a `type` field that tells which kind of value it is:

```sh
curl http://localhost:8080/api/resources/12345/runtime
```

```json
{
  "resourceId": 12345,
  "isRuntimeValue": true,
  "typeString": "",
  "value": { "type": "boolean", "value": true }
}
```

Adding a user (`Group` is `Administrators` or `Users`):

```sh
curl -X POST http://localhost:8080/api/users \
  -H "Content-Type: application/json" \
  -d '{"username":"jane","password":"secret","firstname":"Jane","group":"Users"}'
```

## Project structure

| Project | Description |
| --- | --- |
| [Ihc.WebApi](Ihc.WebApi/) | The ASP.NET Core application: controllers (`Controllers/`), services that call the controller's SOAP endpoints (`Services/`), response models (`Model/`) and error handling (`Exceptions/`). |
| [Ihc.Soap](Ihc.Soap/) | SOAP message contracts for the controller's web services, generated with `dotnet-svcutil`. IHC API uses them to serialize requests and deserialize responses. |
| [Ihc.Project](Ihc.Project/) | Parser for the IHC project file (XML) that builds the model of groups, products, resources, links and scenes. It has no dependencies on the other projects. |

## License

IHC API is released under the [MIT License](LICENSE).
