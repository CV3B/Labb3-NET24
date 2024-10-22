### Labb 3 - Quiz Configurator ###

Vi bygger en applikation för att konfigurera quiz-frågor, och köra quiz-rundor. Appen ska skrivas i WPF och XAML, och byggas på Model-View-ViewModel (MVVM)-arkitektur.

MVVM är ett arkitektoniskt designmönster i mjukvaruutveckling, som underlättar separationen av utvecklingen av ett grafiskt användargränssnitt från den underliggande back-end logiken. Vi kommer gemensamt gå igenom hur detta fungerar.

Eftersom vi har ganska begränsad tid, cirka 3 veckor, för att bygga projektet, och vi ännu inte gått igenom alla delar som kommer behövas, så kommer labben vara upplagd på så vis att vi, delvis, gör den gemensamt som en code-along, så att alla får grundstrukturen rätt.

Vi kommer även ha gemensam handledning / Q&A där vi gemensamt kollar på att lösa de problem som uppstår för er under utvecklingens gång.

Jag har byggt ihop en (mer eller mindre) färdig version av projektet som ni kommer få tillgång till redan från början, så att ni själva kan prova att använda den färdiga mjukvaran och fundera kring hur jag löst olika delar i designen. Det är inget krav på att era lösningar ska se ut exakt som mina, utan se det mer som inspiration. Så länge er app uppfyller kraven för G, respektive VG, så kan ni välja att utforma den på ett annat sätt.

Låt oss börja med att kolla på appen jag byggt!

Vi kommer bygga ihop appen från början, gemensamt, som en code-along med start från nästa kurstillfälle. Vi kommer använda oss av MVVM och Data binding, så koden kommer att vara annorlunda strukturerad än i uppgifterna vi gjort hittills. Därför föreslår jag att ni väntar med att börja skriva C#-kod tills vi sätter igång och gör det tillsammans.

Det ni däremot kan börja med är att strukturera upp och skriva XAML-kod för de olika komponenterna ni kan se i min app.

#### Skapa en mapp i ert VisualStudio-projekt som ni döper till “Dialogs”; i denna kan ni skapa 2 nya fönster (Add->Window (WPF)): PackOptionsDialog.xaml, och CreateNewPackDialog.xaml; Om ni kollar i min app så är det dialogrutorna man får upp när man väljer Edit->Pack Options, respektive File->New Question Pack i menyn. 

#### Skapa en annan mapp i ert VS-projekt som ni döper till “Views”; i denna kan ni skapa 3 nya UserControls: MenuView.xaml (i denna bygger ni programmets huvudmeny), ConfigurationView.xaml (denna innehåller de komponenter man ser när man är i konfigurationsläget (View->Edit)); PlayerView.xaml (denna innehåller de komponenter man ser när man är i spelläget (View->Play)).

#### MainWindow.xaml kommer i princip bestå av ovanstående kontroller på såvis att MenuView alltid kommer visas högst upp, och ConfigurationView ELLER PlayerView visas i resten av fönstret, beroende på vilket läge man är i. (För tillfället kan bara kommentera ut den ena eller andra för att testa hur det kommer se ut i respektive läge).

#### OBS! Det finns inget krav på att dessa måste vara klara tills vi börjar bygga gemensamt med code-along, utan detta är bara förslag om ni vill börja någonstans; så ni har lite förberett när vi kommer till de delarna. OCH, som sagt, implementera inte funktionalitet i C# / Code-behinds än, utan bygg bara upp struktur på komponenterna i XAML.


Översikt över appens uppbyggnad

Appen består i huvudsak av två delar (för G-nivån): en “configurator” för att skapa paket med frågor; och en “player” där man kan köra quiz-rundor. För VG-nivån finns även en tredje del som ni hittar i menyn under File->Import Questions; denna del kopplar upp sig mot ett API online för att importera frågor från deras databas. Observera att denna delen inte är obligatorisk. För godkänt behöver all funktionalitet FÖRUTOM import via API fungera.


### Configuration Mode

Man ska kunna bygga “Question Packs”, det vill säga “paket” med frågor. Man behöver kunna lägga till, ta bort, och redigera befintliga frågor. Alla frågor har fyra alternativ, varav ett korrekt. Man ska även kunna ändra inställningar för själva paketet “Pack Options”: Välja namn, märka upp med svårighetsgrad, samt sätta tidsgräns på frågorna. Man ska även kunna skapa nya “Packs”, och ta bort befintliga.

Det ska gå att lägga till / ta bort frågor på flera sätt: Via menyn, Via snabbknappar på tangentbordet, och via knappar i appen. Även “Pack Options” ska gå att öppna på alla 3 sätt.


### Play Mode

När man startar play mode ska den visa hur många frågor det är i det aktiva “Frågepaketet”, samt vilken fråga man är på i turordning. Man ska alltså svara på alla frågor i paketet innan man får ett resultat som visar hur många rätt man hade. Ordningen frågorna visas i ska dock slumpas från gång till gång; likadant med ordningen som svarsalternativen visas i. Det ska finnas någon form av timer som räknar ner (betänketid per fråga ska gå att ställa in när man konfigurerar packen), och efter man klickat på ett svar (eller tiden tar slut) ska användaren få feedback på om man svarat rätt/fel, samt vilket det korrekta svarsalternativet är.

Det behöver dock inte se ut som i mitt exempel så länge funktionaliteten ovan finns på plats. Vill man till exempel visa tiden i form av en cirkel/klockvisare, eller markera svarsalternativen med olika färger på knapparna istället för ikoner så går det bra.


### Menyn

Menyn ska ha ikoner för de olika alternativen (t.ex med font-awesome). Meny alternativen ska gå att aktivera från tangentbordet både med snabbvalsknappar, och med alt-knappen, till exempel Ctrl+O, samt Alt+E, O för “Pack Options”.


### Json

Appen ska spara paket och frågor i Json, så att man kan ladda in dem vid ett senare tillfälle.

Du kan välja att göra detta på ett av följande två sätt:


Skapa en json fil i en mapp under användarens AppData\Local som lagrar samtliga “question packs” och laddar in dem automatiskt när programmet startar. Detta är alternativet jag gjort i min app. För att hitta mappen kan du använda:
Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
Tänk på att programmet ska fungera även om mappen och/eller filen inte finns; den måste i så fall skapa upp mapp / fil.


Använd OpenFileDialog() / SaveFileDialog() och låt användaren spara/ladda varje “question pack” i en separat fil.

Oavsett alternativ så ska filerna sparas och läsas in asynkront.



### Full Screen

Det ska finnas ett menyalternativ för att köra programmet i helskärmsläge.



Importera frågor via API ( VG-uppgift )

Open Trivia Database tillhandahåller quiz-frågor via ett öppet API på https://opentdb.com

Användaren ska kunna välja bland alla frågekategorier som opentdb tillhandahåller. Dessutom ska man kunna välja svårighetsgrad, samt hur många frågor man vill importera.

Anropen till API:et ska göras asynkront så att GUI inte låses upp medan man hämtar kategorier / frågor.

Felhantering måste fungera, och användaren ska få veta om operationen gått bra eller om något gått fel (i så fall vad som gått fel). Responskoder, och vad dessa innebär, går att hitta i API:ets dokumentation. Tänk även på att andra fel (än felkoder från API:et) kan uppstå, t.ex om din dator inte har någon internetanslutning… detta ska inte krascha programmet.



### Redovisning

Uppgiften ska lösas individuellt. (Vi gör delar gemensamt, men alla skriver sin egen kod).

Checka in din lösning som ett NYTT repo på Github.

Lämna in uppgiften på ithsdistans med en kommentar med github-länken.



### Betygskriterier


För godkänt

Appen ska vara byggd med MVVM, och i huvudsak använda data binding och commands.

Minimera koden i XAML-filernas code-behind.

Appen ska asynkront lagra och läsa in “frågepaket” i json.

Man ska kunna lägga till, ta bort, och konfigurera frågepaket (namn, tid, svårighetsgrad).

Frågor ska kunna läggas till / tas bort via tangentbord, menyalternativ och knappar i app.

I spelläget ska alla frågor och svar i ett paket visas i slumpmässigt vald ordning.

Förvald betänketid ska visas och räkna ner.

Korrekt svar och spelaren svar ska visas, innan appen går vidare till nästa fråga.

Efter alla frågor gått igenom visas ett resultat.

Det ska finnas ett helskärmsläge.



### För väl godkänt

Man ska kunna importera frågor från Open Trivia Database, via deras API.

Man kan välja kategori, svårighetsgrad och antal frågor som ska importeras.

All kommunikation med API:et sker via asynkrona anrop.

Felhantering ska fungera och användaren ska informeras om status på importen.

För VG ställs även större krav på att självständigt kunna utveckla applikationen och hitta lösningar på buggar och andra problem. (Det betyder inte att man inte får fråga om man kör fast eller undrar över något, men jag kommer prioritera att hjälpa alla att få godkänt i första hand).

Det är också högre krav på kvalitet vad gäller kod, UI-design och testning.