# Ticket Reservation System

## Frontend

To build front-end application You need [flutter SDK](https://docs.flutter.dev/get-started/install). During installation please remember to add flutter/bin to your `PATH` enviroment variable. After setting up flutter You just have to call
```
flutter pub get
flutter run
```
and Your application will be running.

---
In order to debug API calls using *Google Chrome* you need to modify Your flutter SDK otherwise You will be getting `XMLHttpRequest error`. Go to flutter SDK instalation location and find file
```
flutter/bin/cache/flutter_tool.stamp
```
delete this file. Next find file
```
flutter/packages/flutter_tools/lib/src/web/chrome.dart
```
in it find list of arguments for Chrome startup:
```dart
    final List<String> args = <String>[
      chromeExecutable,
      // Using a tmp directory ensures that a new instance of chrome launches
      // allowing for the remote debug port to be enabled.
      '--user-data-dir=${userDataDir.path}',
      '--remote-debugging-port=$port',
      // When the DevTools has focus we don't want to slow down the application.
      '--disable-background-timer-throttling',
      // Since we are using a temp profile, disable features that slow the
      // Chrome launch.
      '--disable-extensions',
      '--disable-popup-blocking',
      '--bwsi',
      '--no-first-run',
      '--no-default-browser-check',
      '--disable-default-apps',
      '--disable-translate',
      if (headless)
        ...<String>[
          '--headless',
          '--disable-gpu',
          '--no-sandbox',
          '--window-size=2400,1800',
        ],
      ...webBrowserFlags,
      url,
    ];
```
just comment out `'--disable-extension'` and add below it `'--disable-web-security'`. So finally it should look like this:
```dart
    final List<String> args = <String>[
      chromeExecutable,
      // Using a tmp directory ensures that a new instance of chrome launches
      // allowing for the remote debug port to be enabled.
      '--user-data-dir=${userDataDir.path}',
      '--remote-debugging-port=$port',
      // When the DevTools has focus we don't want to slow down the application.
      '--disable-background-timer-throttling',
      // Since we are using a temp profile, disable features that slow the
      // Chrome launch.
      // '--disable-extensions',
      '--disable-web-security',
      '--disable-popup-blocking',
      '--bwsi',
      '--no-first-run',
      '--no-default-browser-check',
      '--disable-default-apps',
      '--disable-translate',
      if (headless)
        ...<String>[
          '--headless',
          '--disable-gpu',
          '--no-sandbox',
          '--window-size=2400,1800',
        ],
      ...webBrowserFlags,
      url,
    ];
```

## Backend

All backend applications in this repository run with Docker technology as well as several services that are necessary for communication processes. In order to build all aplications at once you can go to `src/back/TicketManager/` directory where `docker-compose` file is. Now run build command
```
docker-compose build
```

Now in order to launch whole backend environment just start containers
```
docker-compose up
```

This command can be also followed with `-d` flag which will run all containers in background. If you add `--build` flag it will auto-build before launching.

When you want containers to stop run another docker command
```
docker-compose down
```

For now known issue is that using docker leaves dangling images. Those are images that were either used as an intermidiate step in building process or old application images. Removing all of them will result in building process to take longer as they contain NuGet data. If you want to free disk space simply run
```
docker image prune
```

Some essential variables are stored in file `variables.env` which has development environment variables used by main app. For production purposes those values should be changed approprietly

Secrets are stored in file `secrets.env`. This file is not stored in code repository. Values stored there should be:
- BlobStorageConnectionString='Blob.Connection.String'

### Test status

On master
---
[![Front-end - Test application master](https://github.com/pw-software-engineering/2023-konfetti/actions/workflows/ci_front_test.yml/badge.svg?branch=master)](https://github.com/pw-software-engineering/2023-konfetti/actions/workflows/ci_front_test.yml)

On dev
---

[![Front-end - Test application dev](https://github.com/pw-software-engineering/2023-konfetti/actions/workflows/ci_front_test.yml/badge.svg?branch=dev)](https://github.com/pw-software-engineering/2023-konfetti/actions/workflows/ci_front_test.yml)

### Build status
On master
---
[![Front-end - Build Android](https://github.com/pw-software-engineering/2023-konfetti/actions/workflows/ci_front_build_android.yml/badge.svg?branch=master)](https://github.com/pw-software-engineering/2023-konfetti/actions/workflows/ci_front_build_android.yml)

[![Front-end - Build iOS](https://github.com/pw-software-engineering/2023-konfetti/actions/workflows/ci_front_build_ios.yml/badge.svg?branch=master)](https://github.com/pw-software-engineering/2023-konfetti/actions/workflows/ci_front_build_ios.yml)

[![Front-end - Build Web](https://github.com/pw-software-engineering/2023-konfetti/actions/workflows/ci_front_build_web.yml/badge.svg?branch=master)](https://github.com/pw-software-engineering/2023-konfetti/actions/workflows/ci_front_build_web.yml)