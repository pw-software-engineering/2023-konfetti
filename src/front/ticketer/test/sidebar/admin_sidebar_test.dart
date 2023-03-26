import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/pages/admin/admin_landing_page.dart';

Widget _homeWidget() {
  return const MaterialApp(
    home: AdminLandingPage()
  );
}

void main() {
  testWidgets(
    'Should render sidebar for admin',
        (WidgetTester tester) async {
      // given
      await tester.pumpWidget(_homeWidget());
      final ScaffoldState state = tester.firstState(find.byType(Scaffold));
      state.openDrawer();
      await tester.pumpAndSettle();
      // when then
      expect(find.textContaining("Logged on as admin"), findsOneWidget);
      expect(find.textContaining("Sign out"), findsOneWidget);
    },
  );
}
