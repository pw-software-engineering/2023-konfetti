import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/pages/organizer/organizer_landing_page.dart';

Widget _homeWidget() {
  return const MaterialApp(
    home: OrganizerLandingPage(),
  );
}

void main() {
  testWidgets(
    'Should render event creation form',
    (WidgetTester tester) async {
      // given
      await tester.pumpWidget(_homeWidget());

      // when then
      expect(find.widgetWithText(TextFormField, "Event name"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "Event description"),
          findsOneWidget);
      expect(
          find.widgetWithText(TextFormField, "Event location"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "Event date"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "Event time"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "Sector name"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "No of rows"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "No of cols"), findsOneWidget);
      expect(find.widgetWithText(TextFormField, "Price \$"), findsOneWidget);
      expect(find.widgetWithText(ElevatedButton, "Add"), findsOneWidget);
      expect(find.widgetWithText(ElevatedButton, "Remove"), findsOneWidget);
      expect(find.widgetWithText(ElevatedButton, "Create"), findsOneWidget);
    },
  );
}
