# WeatherApp
## Maturitní práce

## Specifikace požadavků

Martin Goffa <br/>
goffa.martin@skola.ssps.cz <br/>
27. 10. 2021

Verze 1.0

* Úvod
  * Účel dokumentu
    * Účelem tohoto dokumentu je vytvoření Mobilní Android aplikace, která bude zobrazovat online data o počasí
  * Kontakty
    * Martin Goffa goffa.martin@skola.ssps.cz
  * Odkazy na ostatní dokumenty
    * https://openweathermap.org/api
* Celkový popis
  * Funkce
    * Aplikace bude volat API, ze kterého bude zobrazovat data o počasí
    * Data by měla uživateli dát stručný přehled o tom, jaké je počasí v ním určené lokaci
    * Data se budou skládat z aktuálního počasí, hodinové předpovědi, denní předpovědi
    * Uživatel bude mít možnost ukládat si lokace do seznamu k rychlému přístupu
  * Uživatelské skupiny
    * běžný uživatel (aplikace má pouze jeden způsob užívání)
  * Omezení návrhu a implementace   
    * Volání API pouze v případě potřeby obnovení dat
    * V případě, že nezbude čas bude zkrácená týdenní předpověď (pokud ano, bude k dispozici obsáhlá)
* Požadavky na rozhraní
  * Uživatelská rozhraní 
    * Xamarin Forms
  * Softwarová rozhraní 
    * Android
* Vlastnosti systému
  1. Výběr lokace
    * Vstup: uživatel napíše jméno lokace do vyhledávače nebo se lokalizuje pomocí GPS
    * Akce: uživateli se zobrazí výsledek vyhledávání a on si vybere jím požadovanou lokaci
    * Výstup: zobrazení informací o počasí (den, datum, teplota, počasí, vlhkost, síla větru, oblačnost, tlak, hodinová předpověd, týdenní předpověd)
  2. Vypsání nových dat
    * Vstup: uživatel může refreshnout data pomocí swipnutí dolů
    * Akce: volání API na základě výběru
    * Výstup: zobrazení požadovaných informací o počasí pomocí labelů v přehledném rozložení
  3. Refresh
    * Vstup: Swipnutí dolů
    * Akce: zavolání API a spracování dat
    * Výstup: Aktualizovaná data se zobrazí v uživatelském rozhraní  
* Nefunkční požadavky
    * Odezva
      * Systém úspěšně vyhledá a vypíše požadovaná data do 4 sekund
    * Spolehlivost
      * 99% šance že systém úspěšně vyhledá a vypíše požadovaná data 
    * Bezpečnost
      * Systém nepracuje s žádnými osobními daty

## Funkční specifikace

Verze 1.0

* Úvod
  * Účel dokumentu
    * Účelem tohoto dokumentu je vytvoření mobilní Android aplikace, která bude zobrazovat online data o počasí
  * Kontakty
    * Martin Goffa goffa.martin@skola.ssps.cz
  * Odkazy na ostatní dokumenty
    * https://openweathermap.org/api
* Scénáře
  * Způsoby použití
    * Uživatel má možnost najít aktuální data o počasí v určité oblasti, kterou zvolí, pakliže se nachází v databázi.
    * Těmito daty je myšleno: teplota, počasí, vlhkost, síla větru, oblačnost, tlak, hodinová předpověd, týdenní předpověd  
  * Personas
    * Každý uživatel má stejná práva v aplikaci, nijak se nepřihlašují, nijak se neliší 
  * Vymezení rozsahu
    * Aplikace bude pouze v anglickém jazyce
* Celková hrubá architektura
  * Pracovní tok
    * Uživatel spustí aplikaci
    * zadá jméno lokace a vybere z výběru vyhledávání nebo se lokalizuje pomocí GPS
    * zobrazí se mu data o počasí k dané lokaci
  * Detaily
    * Aplikace bude mít MainPage
      * Navigace (hledání, seznam uložených, nastavení)
      * Data o počasí
    * pozadí pomocí gradientu (v případě času navíc => měnící se na základě počasí)
    * Tmavě šedý text a šedo barevné ikony
    * Při prvním spuštění aplikace lokalizuje telefon a nastaví lokaci, pakliže nemá přístup k poloze, je lokace defaultně nastavena na London
    * Vypsání pomocí labelů a obrázků vyhovující danému počasí
      * Label vlevo nahoře: Název lokace
      * Label pod názvem: Velkým písmem Aktuální teplota v uživatelem zvolených jednotkách
      * Obrázek vpravo od teploty: Obrázek symbolizující aktuální počasí (slunečno, zataženo, déšť....)
      * Labely dole pod teplotou budou obsahovat informace o pocitové teplotě, min a max teplotě, 
    * Blok s daty o počasí
      * vlhkost, síla větru, oblačnost, tlak, UV index, rosný bod
    * Hodinová předpověď 
      * Řádek rozdělen do sloupců
      * Každý sloupec symbolizuje jednu hodinu
      * V bloku je čas, informace kolik bude stupňů a obrázek symbolizující počasí 
      * S bloky lze pohybovat do stran
    * Týdenní předpověď 
      * Sloupec rozdělený do řádků
      * Každý řádek symbolizuje jeden den
      * V bloku je den, datum, informace kolik bude stupňů a obrázek symbolizující počasí
    * Refresh pomocí swipnutí dolů
  * Všechny dohodnuté principy 
    * Data vypisována na základě API 
    * Data se vypisují ve zvolených jednotkách v nastavení
    
    Prototyp v aplikaci Adobe XD:
    
    ![design prototype1](https://github.com/goffamartin/WeatherApp/blob/7bc7cba7376aee6561628e130da1c1d8988c3039/WeatherApp_Prototype1.png)
    ![design prototype2](https://github.com/goffamartin/WeatherApp/blob/7bc7cba7376aee6561628e130da1c1d8988c3039/WeatherApp_Prototype2.png)
    
