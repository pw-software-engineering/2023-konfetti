import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/backend_communication/model/account_type.dart';

void main() {
  test('Should serialize account type enum', () {
    for (var type in AccountType.values) {
      String str = type.toString().substring("AccountType.".length);
      assert(type == AccountTypeConverter.toEnum(str));
    }
  });
}
