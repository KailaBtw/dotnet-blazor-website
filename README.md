# dotnet-blazor-website

A .NET Blazor website (Blazor WebAssembly) with MudBlazor UI, pricing data demo, and sample pages. Built with C# and ASP.NET Core.

## Purpose

This project is a Blazor Web App used to explore Blazor layouts, MudBlazor components, and data-driven pages (e.g. Amazon-style price tracking with CSV and charts).

## GitHub Pages

This app is deployed as a **static Blazor WebAssembly** site to GitHub Pages using the workflow in `.github/workflows/deploy-gh-pages.yml`.  
Live URL (for this repo name):  
`https://kailabtw.github.io/dotnet-blazor-website/`

See **[docs/DEPLOY_ON_GH_PAGES.md](docs/DEPLOY_ON_GH_PAGES.md)** for the one‑time setup and how the workflow works.

## Quickstart (CLI)

You can run the app from the terminal without the VS Code C# extension.

**Prerequisites:** [.NET SDK](https://dotnet.microsoft.com/download) (check with `dotnet --version`).

From the project root:

```bash
dotnet restore
dotnet build
dotnet watch run
```

Then open **http://localhost:5049** (or the URL printed in the console) in a browser. Stop with `Ctrl+C`.

---

See **[docs/RUN_BLAZOR_CLI.md](docs/RUN_BLAZOR_CLI.md)** for more CLI options.

See **[docs/DEPLOY_ON_GH_PAGES.md](docs/DEPLOY_ON_GH_PAGES.md)** for more info on deploying to GH pages. 
