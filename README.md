# Resource Manager CLI

A command-line tool for managing localization resources and automatically translating them using **Google Cloud Translation API**.  
The application is built with **.NET 10** and runs in the console.

---

## 📦 Requirements

- **.NET 10 SDK** installed on your machine
- Configuration files:
  - `config.json` — application settings
  - `key.json` — Google Translator service account key

All configuration files must be located in:  
`Documents\ResourceManagerConfig`

---

## ⚙️ Configuration

### `config.json`
This file defines the application settings:

```json
{
  "ResourcesFolder": "D:\\resources",
  "MainLanguage": "en",
  "TranslatorProjectID": "example-12345"
}
```

## 🖥️ Commands

Add resource language and automatically translates all existing resources from main language to new one.
```bash
add-language <isoLangCode>
```

Add a new resource in the main language from config and translate it into other languages.
```bash
add <text>
```

Display resources by resource ID.
```bash
 get <resourceId>
```

Update a resource by resource ID with a new value and translate it into other languages.
```bash
update <resourceId> <text>
```

Edit a resource by resource ID and language code with a new value.
Other languages remain unchanged.
```bash
edit <isoLangCode> <recourceId> <text>
```

Delete a resource by resource ID.
```bash
delete <resourceId>
```

Clear console
```bash
clear
```

Exit application
```bash
exit
```



