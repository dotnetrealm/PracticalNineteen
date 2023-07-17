# Practical-19

ASP.NET Core Identity

## Setup

- Add connection string in the user secrets file of PracticalNineteen.API project

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=[DBSOURCENAME];Initial Catalog=[DBName];Persist Security Info=True;User ID=[YOURUSERID];Password=[******];TrustServerCertificate=True"
  },
  "Jwt": {
    "Audience": "https://localhost:44362",
    "Issuer": "https://localhost:44362",
    "Key": "E4BlHLpEYgmUxW7py4YYSBBFzlKyZDoXSs1URyc7"
  }
}
```

- Add connection string in the user secrets file of PracticalNineteen project

```json
{
  "API_URL": "https://localhost:44362/api/"
}
```

> **_NOTE:_** This configuration only works under "Development" enviorment.

## Migration

- Make sure you add connection string & JWT Configuration in secrets.json file
- In the package manager console select "PracticalNineteen.Data" project and then run the given command

```bash
Update-Database
```

> **_NOTE:_** Configure multiple startup project for API and MVC.
