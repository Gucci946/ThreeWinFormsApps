# Kolm Windows Forms rakendust

## Projekti eesmärk

Projekti eesmärk on luua kolm iseseisvat Windows Forms rakendust C# keeles ning kasutada nende loomisel objektorienteeritud programmeerimise põhimõtteid.

Projekt sisaldab:
1. pildi vaatamise programmi;
2. matemaatilist mängu;
3. sarnaste piltide mängu.

Kõik kasutajale nähtavad rakenduse tekstid on eesti keeles.

## Kasutatud tehnoloogiad

- C#
- .NET 8.0
- Windows Forms
- objektorienteeritud programmeerimine
- `Timer`
- `OpenFileDialog`
- `SaveFileDialog`
- `ColorDialog`
- `PictureBox`
- `FlowLayoutPanel`

## Miks on projekt OOP põhimõtetele üles ehitatud?

Kasutajaliides on eraldatud mängude ja andmete põhiloogikast. Vormid vastutavad kasutaja tegevuste, nuppude ja visuaalse osa eest. Mudeliklassid vastutavad andmete loomise, kontrollimise ja mängureeglite eest.

Näited:

- `MathQuestion` kirjeldab ühte matemaatikaülesannet.
- `MathGame` haldab matemaatilise mängu olekut, punkte ja vastuseid.
- `MemoryCard` kirjeldab ühte mängukaarti.
- `MemoryGame` haldab kaartide loomist, segamist, käike, paaride arvu ja punktide arvutamist.

## Projekti käivitamine

1. Ava `ThreeWinFormsApps.sln` Visual Studios.
2. Veendu, et arvutis on .NET 8 SDK ja Windows Forms tugi.
3. Määra `ThreeWinFormsApps` käivitusprojektiks.
4. Vali **Build -> Rebuild Solution**.
5. Käivita programm klahviga `F5`.
6. Peaaknas vali üks kolmest rakendusest.

## 1. Pildi vaatamise programm

### Põhifunktsioonid

- mitme pildi korraga avamine;
- järgmise ja eelmise pildi vaatamine;
- pildi suuruse näitamine;
- faili nime näitamine;
- automaatne slaidiesitlus;
- taustavärvi muutmine `ColorDialog` abil;
- pildi salvestamine PNG-, JPG- või BMP-vormingusse.

### Arendused

1. Lisatud taustavärvi muutmine `ColorDialog` abil.
2. Lisatud automaatne slaidiesitlus `Timer` abil.
3. Lisatud pildi salvestamine erinevatesse vormingutesse.

Need täiendused vastavad ülesande nõudele lisada pildi vaatamise programmile vähemalt kolm arendust.

## 2. Matemaatiline mäng

### Põhifunktsioonid

- juhuslikud matemaatilised ülesanded;
- liitmine, lahutamine, korrutamine ja jagamine;
- kolme raskusastme valik;
- vastuse kontrollimine;
- 60-sekundiline ajapiirang;
- punktisüsteem;
- õigete ja valede vastuste statistika;
- vastuse sisestamine ka klahviga Enter.

### Arendused

1. Lisatud erinevad tehtetüübid.
2. Lisatud ajapiirang ja punktisüsteem.
3. Lisatud raskusastmed ja statistika.

## 3. Sarnaste piltide mäng

Mängus tuleb leida kaks samasugust pilti. Kaartide avamisel näidatakse päris pildifaile. Kui kaks kaarti ei sobi kokku, pööratakse need väikese viivituse järel tagasi.

### Põhifunktsioonid

- päris pildifailide kasutamine;
- kaartide juhuslik segamine;
- paaride leidmine;
- käikude arvestus;
- mänguaja arvestus;
- jooksva skoori arvestus;
- parima tulemuse näitamine;
- 4 × 4 ja 6 × 6 mänguvälja valik;
- uue mängu alustamine.

### Arendused

1. Kasutatud sümbolite asemel tegelikke pildifaile.
2. Lisatud taimer, käikude arvestus ja punktisüsteem.
3. Lisatud kaks mängutaset: 4 × 4 ja 6 × 6.

## Edasise arendamise ideed

- kasutajaprofiil ja tulemuste salvestamine faili;
- heliefektid õigete ja valede vastuste jaoks;
- mängude statistika ja edetabel;
- rohkem pilte ja uusi mängutasemeid;
- erinevad slaidiesitluse kiirused.
