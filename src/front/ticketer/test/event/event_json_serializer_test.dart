import 'dart:convert';

import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/event_status.dart';
import 'package:ticketer/backend_communication/model/sector.dart';

void main() {
  test(
    'Should serialize event object',
    () {
      // given
      Sector s1 = Sector("VIP", 240.48, 10, 12);
      Sector s2 = Sector("Basic", 119.90, 100, 50);
      Event event = Event(null, "My Event", "My description", "Warsaw",
          "2023-03-30T01:00:00.000Z", List.from([s1, s2]), EventStatus.Opened);

      String expected =
          '{"id":null,"name":"My Event","description":"My description","location":"Warsaw",'
          '"date":"2023-03-30T01:00:00.000Z","sectors":'
          '[{"name":"VIP","priceInSmallestUnit":24048,"numberOfColumns":12,"numberOfRows":10},'
          '{"name":"Basic","priceInSmallestUnit":11990,"numberOfColumns":50,"numberOfRows":100}],'
          '"status":3}';

      // when
      var json = jsonEncode(event);
      print(json);

      // then
      assert(json == expected);
    },
  );
}
