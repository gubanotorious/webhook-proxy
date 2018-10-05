# webhook-proxy
Webhook proxy to route and debug webhook calls to a local dev environment

## Overview
In developing solutions that rely on third-party webhooks, often those third-parties will require a SSL webhook address with a valid SSL certificate, even when using a sandbox / test environment.  This is a quick proxy that can be hosted on Azure (or other host) that can take advantage of a shared SSL certificate to meet the third-party requirements and route the request to a specified target.

## Usage
Modify appsettings.json to configure:
##### Target:
- Location: The URL to redirect requests to
- SecurityHeader: The security header to pass with the request (optional)

#### Disclaimer:
This is not intended to be a solution to be used in a production environment.  This is intended for sandbox testing when there is a hard requirement of a valid SSL secured webhook endpoint.
