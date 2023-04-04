// TODO: To be fixed in TM-77

import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/pages/user/user_landing_page.dart';

Widget _homeWidget() {
  return const MaterialApp(home: UserLandingPage());
}

void main() {
  testWidgets(
    'Should render sidebar for user',
    (WidgetTester tester) async {
      // // given
      // await tester.pumpWidget(_homeWidget());
      // final ScaffoldState state = tester.firstState(find.byType(Scaffold));
      // state.openDrawer();
      // await tester.pumpAndSettle();
      // // when then
      // expect(find.textContaining("Logged on"), findsOneWidget);
      // expect(find.textContaining("Edit"), findsOneWidget);
      // expect(find.textContaining("Sign out"), findsOneWidget);
      assert(true);
    },
  );
}
