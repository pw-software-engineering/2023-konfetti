import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/pages/organizer/organizer_landing_page.dart';

Widget _homeWidget() {
  return const MaterialApp(
    home: OrganizerLandingPage()
  );
}

void main() {
  testWidgets(
    'Should render sidebar for organizer',
        (WidgetTester tester) async {
      // given
      await tester.pumpWidget(_homeWidget());
      final ScaffoldState state = tester.firstState(find.byType(Scaffold));
      state.openDrawer();
      await tester.pumpAndSettle();
      // when then
      expect(find.textContaining("Logged on"), findsOneWidget);
      expect(find.textContaining("Edit"), findsOneWidget);
      expect(find.textContaining("Sign out"), findsOneWidget);
    },
  );
}
