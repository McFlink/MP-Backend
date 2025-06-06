# MP Backend – MP Fishing Supply AB 🎣

Detta är backend-API:t för MP Fishing Supply AB:s webbplattform. Projektet är utvecklat i **.NET 9 (ASP.NET Core)** och är integrerat med **Identity**, **JWT**, **roller**, **e-postverifiering via SendGrid**, samt framtida stöd för **BankID-autentisering**.

## 🔧 Teknikstack

- ASP.NET Core 9
- Entity Framework Core
- ASP.NET Identity
- PostgreSQL (via Docker under utveckling)
- JWT Authentication (med HttpOnly cookies)
- Email-verifiering via SendGrid
- GitHub Actions för CI (build pipeline)

## 🔐 Autentisering & Roller

- `Retailer` – för företag/återförsäljare som kan lägga beställningar
- `Customer` – privatpersoner, t.ex. vid bokning av fiskepaket (BankID-stöd kommer)
- `Admin` – intern administratör

## 📬 Funktioner

- Registrering med e-postverifiering
- Inloggning med JWT + HttpOnly-cookie
- Rollhantering och skyddade endpoints
- CI-pipeline via GitHub Actions (.yml i `.github/workflows/`)
- Förberedd för produktion (separerade miljöinställningar, SendGrid + säkerhetsinställningar)


## 🧪 Att testa funktioner

1. Registrera en användare via Swagger eller frontend
2. Verifiera e-post via länk som skickas med SendGrid
3. Logga in – JWT-token sätts i cookie
4. Skyddade endpoints tillgängliga beroende på roll

## 🛡️ Säkerhet & framtid

- HttpOnly + Secure + SameSite-strikt policy
- Roll-baserad åtkomst
- Kommande: BankID-integrering för `Customer`-flöden

## 📦 Deployment

Backend kommer hostas separat från frontend. Kommunikation sker via API-anrop med tokenhantering i cookies.

## 🧠 Övrigt

Projektet är privat. Ingen öppen källkod, ingen licens.

---

