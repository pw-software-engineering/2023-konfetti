import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/model/credentials.dart';
import 'package:ticketer/pages/login/page_organizer_register.dart';

Widget _homeWidget() {
  return MaterialApp(
    home: OrganizerRegisterPage(
      credentials: Credentials("jan.nowak@pw.edu.pl", "123456"),
    ),
  );
}

void main() {
  testWidgets(
    'Should render organizer registration page',
    (WidgetTester tester) async {
      // given
      await tester.pumpWidget(_homeWidget());

      // when then
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
      // given
      await tester.pumpWidget(_homeWidget());

      // when
      await tester.enterText(
          find.widgetWithText(TextFormField, "Tax id number"), "1f23b");
      await tester.enterText(
          find.widgetWithText(TextFormField, "Postal / Zip code"), "00000");
      await tester.tap(find.widgetWithText(ElevatedButton, "Submit"));
      await tester.pump();

      // then
      expect(
          find.text("Please enter tax identification number"), findsOneWidget);
      expect(find.text("Please enter correct code in XX-XXX format"),
          findsOneWidget);
    },
  );
}
