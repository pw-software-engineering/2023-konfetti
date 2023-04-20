import 'dart:convert';

import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/backend_communication/model/organizer.dart';
import 'package:ticketer/backend_communication/model/tax_type.dart';

void main() {
  test(
    'Should serialize organizer object',
    () {
      // given
      Organizer org = Organizer("Januszex.pl", "Warszawa", TaxType.KRS, "12345",
          "Januszex", "jan.nowak@pw.edu.pl", "A1b2c3d4", "+48600900600");
      String expected = '{"companyName":"Januszex.pl","address":"Warszawa",'
          '"taxIdType":2,"taxId":"12345","displayName":"Januszex","email":"jan.nowak@pw.edu.pl",'
          '"password":"A1b2c3d4","phoneNumber":"+48600900600"}';

      // when
      var json = jsonEncode(org);

      // then
      assert(json == expected);
    },
  );
}
