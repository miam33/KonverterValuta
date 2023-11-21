# KonverterValuta
C#, WPF, i MSSQL projekt
## UVOD
Ovo je projekt za konvertiranje valuta. Postoje dva taba. Prvi tab se zove "Konvrter Valuta" te služi za pretvaranje jedne valute u drugu. Funkcionira na način da u textbox "Unesite iznos" upišete željenu količinu koju želite pretvoriti.
Pomoću dropdownova koji se nalaze u textboxevima "Iz Valute" i "U Valutu" odaberete dvije željene valute, od ponuđenih. Kliknete tipku pretvori te se ispod naziva "Pretvorena Valuta" prikaže određeni iznos, a pomoću tipke "Očisti" obrišete sve i možete krenuti iznova s radnjom.
Drugi tab se zove "Administracija" i on služi za dodavanje novih, editanje i brisanje starih valuta. Funkcionira na način da u textbox "Unesite Iznos" unesete ratu valute za određenu valutu te u textbox "Naziv Valute" upišete kraticu za tu valutu. Na kraju kliknete tipku "Spremi"
i u tablici ispod se pojavi nova valuta. U tablici koja se nalazi ispod ponuđene su dvije opcije. Jedna opcija je edit, koja Vam omogućava uređivanje postojeće valute, a druga je delete i ona Vam omogućava brisanje valuta iz tabilice. 

## Koraci za pokretanje aplikacije:
1. Potrebno je skinuti aplikaciju sa GitHub-a na vlastito računalo.
2. Potrebno je instalirati Microsoft SQL Server LocalDB, to je pojednostavljena verzija SQL Servera, dizajnirana za razvoj aplikacija s laganom instalacijom i minimalnom konfiguracijom.
3. Potrebno je pokrenuti LocalDB instancu.
4. Nakon toga potrebni je u app.config datoteci pod "connectionString" postaviti putanju do datoteke vlastite baze podataka.

## Init skripta
CREATE TABLE Valute (
  Rata float,
  Naziv varchar(5)
);

INSERT INTO Valute (Rata, Naziv)
VALUES (0,91, EUR);
VALUES (0,80, GBP);
VALUES (83,28, INR);
VALUES (1,53, AUD);
VALUES (0,88, CHF);
VALUES (149,71, JPY);




