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
    'Should render organizer landing page',
    (WidgetTester tester) async {
      // given
      await tester.pumpWidget(_homeWidget());

      // when then
      expect(find.text("Company name"), findsOneWidget);
      expect(find.text("Address"), findsOneWidget);
      expect(find.text("Display name"), findsOneWidget);
      expect(find.text("Tax info"), findsOneWidget);
      expect(find.text("Phone"), findsOneWidget);
    },
  );
}
