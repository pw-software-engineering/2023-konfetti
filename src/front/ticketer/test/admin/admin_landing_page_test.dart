import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/pages/admin/landing/admin_landing_page.dart';

Widget _homeWidget() {
  return const MaterialApp(
      home: AdminLandingPage()
  );
}

void main() {
  testWidgets(
    'Should render organiser list',
        (WidgetTester tester) async {
      // given
      await tester.pumpWidget(_homeWidget());
      int n = 3; // TODO: [TM-29] mock organizer fetching

      // when then
      expect(find.textContaining("Approve"), findsNWidgets(n));
      expect(find.textContaining("Reject"), findsNWidgets(n));
    },
  );
}
