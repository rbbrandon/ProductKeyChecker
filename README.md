# ProductKeyChecker Utility
This utility is designed to read a Windows 8-10 product key, either via user input or from the local system's firmware, and output a description of the edition of Windows the key is for.

## Command-Line Examples
Display the utility's "help menu":

```ProductKeyChecker.exe /?```
OR
```ProductKeyChecker.exe /h```
OR
```ProductKeyChecker.exe /help```

Use the embedded Windows OEM key:

```ProductKeyChecker.exe /oem```

Use a specified Windows key:

```ProductKeyChecker.exe /key YTMG3-N6DKC-DKB77-7M9GH-8HVX7```

## Example Output
### Windows 8.1 Pro OEM Key (embedded):
ProductKeyChecker.exe /oem
```Win 8.1 RTM Professional OEM:DM```

### Windows 10 Home Generic Key:
ProductKeyChecker.exe /key YTMG3-N6DKC-DKB77-7M9GH-8HVX7
```Win 10 RTM Core Retail```

### Windows 10 Pro Generic Key:
ProductKeyChecker.exe /key VK7JG-NPHTM-C97JM-9MPGT-3V66T
```Win 10 RTM Professional Retail```

### Windows 10 Education Generic Key:
ProductKeyChecker.exe /key YNMGQ-8RYV3-4PGQ3-C8XTP-7CFBY
```Win 10 RTM Education Retail```

### Windows 10 Pro Education Generic Key:
ProductKeyChecker.exe /key 8PTT6-RNW4C-6V7J2-C2D3X-MHBPB
```Win 10 RTM ProfessionalEducation Retail```

## Credits
* Leo Walthert for providing [PowerShell code to read an embeded Windows product key from the system firmware](https://gist.github.com/lwalthert/fe52f7fa98b4ea491345a0518750baa9).
* [pidgenx.fandom.com](https://pidgenx.fandom.com/wiki/PidGenX_Wiki) for providing the structure of the ["PidGenX" function](https://pidgenx.fandom.com/wiki/PidGenX_function) and the ["DigitalProductId4" data type](https://pidgenx.fandom.com/wiki/DigitalProductId4).
* Finnbarr P. Murphy for providing [code containing the structure of the EFI ACPI SDT Header and EFI ACPI MSDM Table](https://blog.fpmurphy.com/2015/02/retrieve-microsoft-windows-product-key-from-uefi-shell.html).
* Icon made by [Freepik](https://www.flaticon.com/authors/freepik) from [www.flaticon.com](http://www.flaticon.com/).