import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/credentials.dart';
import 'package:ticketer/backend_communication/model/organizer_account.dart';
import 'package:ticketer/backend_communication/model/tax_type.dart';
import 'package:ticketer/backend_communication/model/user.dart';

void main() async {
  group('BackendCommunication tests', () {
    test('Without initialization BackendCommunication() should throw exception',
        () {
      assert(BackendCommunication().isInitialized == false);
      var organizer = OrganizerAccount(
          "companyName",
          "address",
          TaxType.NIP,
          "123123123",
          "displayName",
          "email@email.com",
          "password",
          "123123123");
      var user = User(
          "firstName", "lastName", "2020-10-10", "email@email.com", "password");
      expect(() => BackendCommunication().postCall("account/login"),
          throwsA(anything));
      expect(() => BackendCommunication().organizer.register(organizer),
          throwsA(anything));
      expect(() => BackendCommunication().organizer.update(organizer),
          throwsA(anything));
      expect(
          () => BackendCommunication().user.register(user), throwsA(anything));
      expect(() => BackendCommunication().user.update(user), throwsA(anything));
      expect(
          () => BackendCommunication()
              .account
              .login(Credentials("email@email.com", "password")),
          throwsA(anything));
    });
  });
}
