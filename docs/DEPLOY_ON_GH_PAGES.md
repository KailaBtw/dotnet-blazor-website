# Deploy to GitHub Pages

This project is already a **Blazor WebAssembly** app. GitHub Pages just needs the static files that `dotnet publish` produces.

The GH Pages URL for this repo is:

`https://<your-username>.github.io/<repo-name>/`

For this project:  
`https://kailabtw.github.io/dotnet-blazor-website/`

The app’s `<base href>` in `wwwroot/index.html` is set to:

```html
<base href="/dotnet-blazor-website/" />
```

If you ever rename the repo, update that path to match.

---

## 1. One‑time GitHub setup

1. **Push the code** to GitHub with:
   - `wwwroot/index.html`
   - `wwwroot/assets/mock_price_data.csv`
   - `.github/workflows/deploy-gh-pages.yml`
2. In the GitHub repo:
   - Go to **Settings → Pages**.
   - Under **Build and deployment → Source**, choose **GitHub Actions**.
3. In **Settings → Actions → General**:
   - Under **Workflow permissions**, select **Read and write permissions** and save.

---

## 2. How the workflow deploys

Workflow file: `/.github/workflows/deploy-gh-pages.yml`

On every push to `main`:

1. Checkout code.
2. Run `dotnet restore`.
3. Run `dotnet publish -c Release -o ./publish`.
4. Verify that `./publish/wwwroot/index.html` exists.
5. Copy `index.html` to `404.html` inside `./publish/wwwroot` so client-side routing works on refresh.
6. Upload `./publish/wwwroot` as the Pages artifact.
7. Deploy that artifact to **GitHub Pages**.

No manual steps are needed after this—push to `main` and GitHub Pages will update automatically.

---

## 3. Run locally

From the project root:

```bash
dotnet restore
dotnet watch run
```

Then open `http://localhost:5049` (or the URL from the console).  
See [RUN_BLAZOR_CLI.md](RUN_BLAZOR_CLI.md) for more CLI details.

---

## Quick reference

| Goal | Action |
|------|--------|
| Run locally | `dotnet watch run` (see `docs/RUN_BLAZOR_CLI.md`). |
| Deploy to GitHub Pages | Push to `main`; the `deploy-gh-pages` workflow publishes and deploys `publish/wwwroot`. |
