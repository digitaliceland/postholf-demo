# Inngangur 
Sýningardæmi fyrir samskipti við Pósthólf Ísland.is.  Dæminu er skipt tvö verkefni (e.project).  Annað verkefnið (DocumentindexCLI) er skeljarforrit sem sýnir hvernig hægt er að senda inn skjalatilvísanir í Pósthólfið á Ísland.is.
Í hinu verkefninu er búið að útfæra skil sem skjalaveitur þurfa að hafa hafa uppsett til að skila skjölum þegar notandi kallar eftir þeim.  Bæði samskiptin til og frá pósthólfinu notast við OAuth2 auðkenningu.


# Hvernig á að byggja forritið

Sýningardæmið er skrifað í ASP.NET Core 3.1

* [Install](https://www.microsoft.com/net/download/core#/current) 

Það þarf að vera með git uppsett til þess að sækja kóðann.
Kóði er sóttur með git clone:
```
git clone https://github.com/digitaliceland/postholf-demo.git
```
Farið inn í möppuna .\postholf-demo\src\IslandIs.Skjalaveita.Demo og keyrið dotnet build til að byggja bæði verkefnin.
```
cd .\postholf-demo\src\IslandIs.Skjalaveita.Demo
dotnet build
```
# Stillingar

Það þarf að stilla bæði vefþjónustuna og skeljarforritið með aðgangsupplýsingum.
Stillingar eru settar í skránna appsettings.json undir hvoru verkefni.
Notendur geta fengið aðgangsupplýsingar hjá Stafrænu Íslandi.

## IslandIs.Skjalaveita.DocumentindexCLI
**.\IslandIs.Skjalaveita.DocumentindexCLI\bin\Debug\netcoreapp3.1\appsettings.json**

**SenderKennitala:** Kennitala stofnunnar sem er að senda skjal.\
**SenderName:** Nafn stofnunnar\
**ClientId:** Einkenni biðlara (gefið út af Stafrænu Íslandi)\
**ClientSecret:** Aðgangsorð biðlara

```javascript
{
  "ConsoleApplicationSettings": {
    "SenderKennitala": "<<SenderKennitala>>",
    "SenderName": "<<SenderName>>"
  },
  "DocumentindexServiceSettings": {
    "BaseUrl": "https://test-skjalatilkynning-island-is.azurewebsites.net/api/v1/documentindexes/",
    "Authority": "https://test-identity-island-is.azurewebsites.net",
    "Scope": "https://test-skjalatilkynning-island-is.azurewebsites.net/.default",
    "ClientId": "<<ClientId>>",
    "ClientSecret": "<<ClientSecret>>"
  }
}
```

## IslandIs.Skjalaveita.Api
**.\IslandIs.Skjalaveita.Api\bin\Debug\netcoreapp3.1\appsettings.json**

**Audience:** Þjónustan hjá Skjalaveitu (sjá https://tools.ietf.org/html/rfc7519#section-4.1.3)  
**Scope:** Stilling á aðgangsheimild (sjá https://tools.ietf.org/html/rfc6749#section-3.3)

```javascript
{
  "AllowedHosts": "*",
  "IdPSettings": {
    "Audience": "<<Audience>>",
    "Scope": "<<Scope>>",
    "Authorities": [
      "https://test-identity-island-is.azurewebsites.net"
    ]
  }
}
```

# Keyrsla

## IslandIs.Skjalaveita.DocumentindexCLI

Til að keyra skeljarforritið er eftirfarandi skipun keyrð:
```
.\IslandIs.Skjalaveita.DocumentindexCLI\bin\Debug\netcoreapp3.1\IslandIs.Skjalaveita.DocumentindexCLI
```
Forritið skilar tilbaka hvaða stikar (e. arguments) þurfa að vera settir.\
T.d. ef sækja á flokka þá er eftirfarandi skipun keyrð (rofinn /c settur).
```
.\IslandIs.Skjalaveita.DocumentindexCLI\bin\Debug\netcoreapp3.1\IslandIs.Skjalaveita.DocumentindexCLI /c
```
## IslandIs.Skjalaveita.Api

Ef það á að keyra upp vefþjóninn (API) þá er eftirfarandi skipun keyrð:
```
.\IslandIs.Skjalaveita.Api\bin\Debug\netcoreapp3.1\IslandIs.Skjalaveita.Api
```
Aðgerðin keyrir upp sjálfstæðan vefþjón.\
Hægt er að skoða OpenAPI skilgreiningu fyrir þjónustuna með því að opna eftirfarandi slóð í vafra:
```
https://localhost:5001/swagger/index.html
```

# Tilvísanir

- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
