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