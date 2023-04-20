import 'dart:convert';

import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/backend_communication/model/organizer_account.dart';
import 'package:ticketer/backend_communication/model/tax_type.dart';

void main() {
  test(
    'Should serialize organizer object',
    () {
      // given
      OrganizerAccount org = OrganizerAccount("Januszex.pl", "Warszawa", TaxType.KRS, "12345",
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

  test(
    'Should deserialize organizer object',
    () {
      // given
      Map<String, dynamic> json = {"companyName":"Januszex.pl","address":"Warszawa",
          "taxIdType":2,"taxId":"12345","displayName":"Januszex","email":"jan.nowak@pw.edu.pl",
          "password":"A1b2c3d4","phoneNumber":"+48600900600"};
      OrganizerAccount expected = OrganizerAccount("Januszex.pl", "Warszawa", TaxType.KRS, "12345",
          "Januszex", "jan.nowak@pw.edu.pl", "A1b2c3d4", "+48600900600");

      // when
      var org = OrganizerAccount.fromJson(json);

      // then
      assert(org.companyName == expected.companyName);
      assert(org.address == expected.address);
      assert(org.taxIdType == expected.taxIdType);
      assert(org.taxId == expected.taxId);
      assert(org.displayName == expected.displayName);
      assert(org.email == expected.email);
      assert(org.password == expected.password);
      assert(org.phoneNumber == expected.phoneNumber);
    },
  );
}
