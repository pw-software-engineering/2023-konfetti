import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/main.dart';

import 'package:ticketer/pages/page_organizer_register.dart';

void main() {
  testWidgets('Should render organizer registration page',
      (WidgetTester tester) async {
    // Build our app and trigger a frame.
    await tester.pumpWidget(const MyApp());

    await registerOrganizer(tester);

    // Verify that we see all the componenst
    expect(find.widgetWithText(TextFormField, "Company name"), findsOneWidget);
    expect(find.widgetWithText(TextFormField, "City"), findsOneWidget);
    expect(find.widgetWithText(TextFormField, "Postal / Zip code"),
        findsOneWidget);
    expect(find.widgetWithText(TextFormField, "Address"), findsOneWidget);
    expect(find.widgetWithText(TextFormField, "Display name"), findsOneWidget);
    expect(find.widgetWithText(TextFormField, "Tax id number"), findsOneWidget);
    expect(find.widgetWithText(TextFormField, "Phone"), findsOneWidget);
    expect(find.widgetWithText(ElevatedButton, "Submit"), findsOneWidget);
  });

  testWidgets("Should validate fields", (WidgetTester tester) async {
    // Build our app and trigger a frame.
    await tester.pumpWidget(const OrganizerRegisterPage());
  });
}

Future<void> registerOrganizer(WidgetTester tester) async {
  await tester.tap(find.widgetWithText(TextButton, "Register instead"));
  await tester.pump();

  await tester.enterText(
      find.widgetWithText(TextFormField, "e-mail"), "test@pl.pl");
  await tester.enterText(
      find.widgetWithText(TextFormField, "Password"), "123123");
  await tester.enterText(
      find.widgetWithText(TextFormField, "Repeat password"), "123123");
  await tester.tap(find.widgetWithText(ElevatedButton, "Register"));
}
