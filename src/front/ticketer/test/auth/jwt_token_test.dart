import 'package:flutter_test/flutter_test.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/model/account_type.dart';

class JWTTokenParsed {
  final String accountId;
  final AccountType role;
  final DateTime nbf;
  final DateTime exp;
  final DateTime iat;

  JWTTokenParsed(
      {required this.accountId,
      required this.role,
      required this.nbf,
      required this.exp,
      required this.iat});
}

void main() {
  group("formatDay should format dates correctly:", () {
    var inputsToExpected = {
      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SWQiOiJiZGMzNjdmZC1kNGRlLTRiNWQtYmVjMi1kNjA1ZmQxMmE4OTEiLCJyb2xlIjoiVXNlciIsIm5iZiI6MTY3OTUyNjEwNiwiZXhwIjoxNjc5NTQwNTA2LCJpYXQiOjE2Nzk1MjYxMDZ9.PXgKAaju2fEZ-ZRkiic5FfwfjpmXL03OhBciRibn1J0":
          JWTTokenParsed(
              accountId: "bdc367fd-d4de-4b5d-bec2-d605fd12a891",
              role: AccountType.User,
              nbf: DateTime.fromMillisecondsSinceEpoch(1000 * 1679526106),
              exp: DateTime.fromMillisecondsSinceEpoch(1000 * 1679540506),
              iat: DateTime.fromMillisecondsSinceEpoch(1000 * 1679526106)),
    };
    inputsToExpected.forEach((input, expected) {
      test("$input -> $expected", () {
        var parsed = Token(input);
        assert(parsed.token == input);
        assert(parsed.accountId == expected.accountId);
        assert(parsed.role == expected.role);
        assert(parsed.expire == expected.exp);
        assert(parsed.issued == expected.iat);
        assert(parsed.notBefore == expected.nbf);
      });
    });
  });
}
