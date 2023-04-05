import 'dart:convert';
import 'dart:io';

import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/backend_communication/logic/organizer/communication_organizer.dart';
import 'package:ticketer/backend_communication/model/organizer.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/backend_communication/model/tax_type.dart';
import 'package:ticketer/pages/admin/landing/admin_landing_page.dart';
import 'package:mockito/annotations.dart';
import 'package:mockito/mockito.dart';
import 'package:tuple/tuple.dart';



@GenerateNiceMocks([MockSpec<OrganizerCommunication>()])
import 'admin_landing_page_test.mocks.dart';

Widget _homeWidget() {
  return const MaterialApp(
      home: AdminLandingPage()
  );
}

final noOrgResponseBody = utf8.encode(jsonEncode({
  "items": [],
  "totalCount": 2,
}));
final moreOrgResponseBody = utf8.encode(jsonEncode({
  "items": [
    Organizer("1","Januszex.pl", "Warszawa", TaxType.KRS, "12345",
        "Januszex", "jan.nowak@pw.edu.pl", "+48600900600"),
    Organizer("2","Januszex.pl", "Warszawa", TaxType.KRS, "12345",
        "Januszex", "jan.nowak@pw.edu.pl", "+48600900600")
  ],
  "totalCount": 2,
}));

void main() {
  setUp(() {
  });
/*
  testWidgets(
    'Should render info when no organizers',
        (WidgetTester tester) async {
          // given
          OrganizerCommunication orgCom = MockOrganizerCommunication();
          when(OrganizerCommunication().listToVerify(anything, anything)).thenAnswer((_) => Future (() =>
              Tuple2<Response, ResponseCode>({"data": noOrgResponseBody} as Response, ResponseCode.allGood)
          ));
          await tester.pumpWidget(_homeWidget());
          // when then
          expect(find.textContaining("No new applications to show!"), findsOneWidget);
    },
  );

  testWidgets(
    'Should render organiser list',
        (WidgetTester tester) async {
            // given
            await tester.pumpWidget(_homeWidget());
            await tester.pumpAndSettle();
            // when then
            expect(find.textContaining("Accept"), findsNWidgets(2));
        },
  );*/
}
