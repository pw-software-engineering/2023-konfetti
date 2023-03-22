import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';

import 'package:ticketer/main.dart';

void main() {
  testWidgets('Change login to register form smoke test',
      (WidgetTester tester) async {
    // Build our app and trigger a frame.
    await tester.pumpWidget(const MyApp());

    // Verify that we see a login page
    expect(find.widgetWithText(ElevatedButton, "Login"), findsOneWidget);
    expect(find.widgetWithText(ElevatedButton, "Register"), findsNothing);
    expect(find.widgetWithText(TextFormField, "e-mail"), findsOneWidget);

    // Tap the "Register instead" button and trigger a frame.
    await tester.tap(find.widgetWithText(TextButton, "Register instead"));
    await tester.pump();

    // Verify that we see a register page
    expect(find.widgetWithText(ElevatedButton, "Login"), findsNothing);
    expect(find.widgetWithText(ElevatedButton, "Register"), findsOneWidget);
    expect(find.widgetWithText(TextFormField, "e-mail"), findsOneWidget);
  });
}
