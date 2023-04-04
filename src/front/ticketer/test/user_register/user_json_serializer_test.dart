import 'dart:convert';

import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/backend_communication/model/user.dart';

void main() {
  test(
    'Should serialize user object',
    () {
      // given
      User user =
          User("Jan", "Nowak", "2001-01-01", "jan.nowak@pw.edu.pl", "A1b2c3d4");
      String expected = '{"firstName":"Jan","lastName":"Nowak",'
          '"birthDate":"2001-01-01","email":"jan.nowak@pw.edu.pl",'
          '"password":"A1b2c3d4"}';

      // when
      var json = jsonEncode(user);

      // then
      assert(json == expected);
    },
  );
}
