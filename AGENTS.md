# AGENTS.md

## Project
- Embeddable web admin for Quartz.NET scheduler
- .NET solution with main library, views, and tests

## Structure
- `QuartzNetWebConsole/` — main library
- `QuartzNetWebConsole.Views/` — Razor views
- `QuartzNetWebConsole.Tests/` — unit tests
- `SampleApp/` — reference implementation

## Commands

There's a flake.nix file for the development environment. You must use this for all dev commands e.g. `nix develop --command dotnet test`

