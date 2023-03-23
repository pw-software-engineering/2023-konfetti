import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/model/credentials.dart';
import 'package:ticketer/pages/page_user_register.dart';

Widget _homeWidget() {
  return MaterialApp(
    home: UserRegisterPage(
      credentials: Credentials("jan.nowak@pw.edu.pl", "123456"),
    ),
  );
}

void main() {
  testWidgets(
    'Should render user registration page',
    (WidgetTester tester) async {
      // given
      await tester.pumpWidget(_homeWidget());

      // when then
      expect(
          find.widgetWithText(TextFormField, "First Name"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "Last Name"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "Birth Date"), findsOneWidget);
    },
  );

  testWidgets(
    'Should validate fields on user registration page',
    (WidgetTester tester) async {
      // given
      await tester.pumpWidget(_homeWidget());

      // when
      await tester.enterText(
          find.widgetWithText(TextFormField, "First Name"), "");
      await tester.enterText(
          find.widgetWithText(TextFormField, "Last Name"), "");
      await tester.enterText(
          find.widgetWithText(TextFormField, "Birth Date"), "");
      await tester.tap(find.widgetWithText(ElevatedButton, "Submit"));
      await tester.pump();

      // then
      expect(find.text("Please enter your first name"), findsOneWidget);
      expect(find.text("Please enter your last name"), findsOneWidget);
      expect(find.text("Please choose your birth date"), findsOneWidget);
    },
  );
}
