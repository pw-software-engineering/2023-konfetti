import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';

import 'package:ticketer/main.dart';
import 'package:ticketer/pages/page_login.dart';

Widget _homeWidget() {
  return const MaterialApp(
    home: LoginPage(),
  );
}

void main() {
  testWidgets('Should validate too short password on registration screen',
      (WidgetTester tester) async {
    // given
    await tester.pumpWidget(_homeWidget());
    await tester.tap(find.widgetWithText(TextButton, "Register instead"));
    await tester.pump();

    // when
    await tester.enterText(
        find.widgetWithText(TextFormField, "Password"), "1fW3b");
    await tester.tap(find.widgetWithText(ElevatedButton, "Register as user"));
    await tester.pump();

    // then
    expect(
        find.text("Password length needs to be betweeen 8 and 32 characters"),
        findsOneWidget);
  });

  testWidgets('Should validate invalid password on registration screen',
      (WidgetTester tester) async {
    // given
    await tester.pumpWidget(const MyApp());
    await tester.tap(find.widgetWithText(TextButton, "Register instead"));
    await tester.pump();

    // when
    await tester.enterText(
        find.widgetWithText(TextFormField, "Password"), "terte4egb");
    await tester.tap(find.widgetWithText(ElevatedButton, "Register as user"));
    await tester.pump();

    // then
    expect(find.text("Password must contain digit, small and capital letter"),
        findsOneWidget);
  });
}
