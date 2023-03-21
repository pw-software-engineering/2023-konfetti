import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';

import 'package:ticketer/pages/page_organizer_register.dart';

Widget _homeWidget() {
  return const MaterialApp(home: OrganizerRegisterPage());
}

void main() {
  testWidgets(
    'Should render organizer registration page',
    (WidgetTester tester) async {
      // Enter tested screen
      await tester.pumpWidget(_homeWidget());

      // Verify that we see all the components
      expect(
          find.widgetWithText(TextFormField, "Company name"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "City"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "Postal / Zip code"),
          findsOneWidget);
      expect(find.widgetWithText(TextFormField, "Address"), findsOneWidget);
      expect(
          find.widgetWithText(TextFormField, "Display name"), findsOneWidget);
      expect(
          find.widgetWithText(TextFormField, "Tax id number"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "Phone"), findsOneWidget);
      expect(find.widgetWithText(ElevatedButton, "Submit"), findsOneWidget);
    },
  );

  testWidgets(
    'Should validate fields on organizer registration page',
    (WidgetTester tester) async {
      // Enter tested screen
      await tester.pumpWidget(_homeWidget());

      // Verify that fields are validated
      await tester.enterText(
          find.widgetWithText(TextFormField, "Tax id number"), "1f23b");
      await tester.enterText(
          find.widgetWithText(TextFormField, "Postal / Zip code"), "00000");
      await tester.tap(find.widgetWithText(ElevatedButton, "Submit"));
      await tester.pump();

      expect(
          find.text("Please enter tax identification number"), findsOneWidget);
      expect(find.text("Please enter correct code in XX-XXX format"),
          findsOneWidget);
    },
  );
}
