import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/backend_communication/model/organizer_account.dart';
import 'package:ticketer/backend_communication/model/tax_type.dart';
import 'package:ticketer/backend_communication/model/user.dart';

void main() async {
  group('Auth tests', () {
    test('Without initialization Auth() should throw exception', () {
      expect(
          () => Auth().logInWithEmailAndPassword(
              email: "email@email.com", password: "123123Aa"),
          throwsA(anything));
      expect(
          () => Auth().registerOrganizer(OrganizerAccount(
              "companyName",
              "address",
              TaxType.NIP,
              "123123123",
              "displayName",
              "email@email.com",
              "password",
              "123123123")),
          throwsA(anything));
      expect(
          () => Auth().registerUser(User("firstName", "lastName", "2020-10-10",
              "email@email.com", "password")),
          throwsA(anything));
      expect(() => Auth().logOut(), throwsA(anything));
    });
  });
}
