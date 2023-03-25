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
  group("Test JWT Token parsing", () {
    var inputsToExpected = {
      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SWQiOiJiZGMzNjdmZC1kNGRlLTRiNWQtYmVjMi1kNjA1ZmQxMmE4OTEiLCJyb2xlIjoiVXNlciIsIm5iZiI6MTY3OTUyNjEwNiwiZXhwIjoxNjc5NTQwNTA2LCJpYXQiOjE2Nzk1MjYxMDZ9.PXgKAaju2fEZ-ZRkiic5FfwfjpmXL03OhBciRibn1J0":
          JWTTokenParsed(
              accountId: "bdc367fd-d4de-4b5d-bec2-d605fd12a891",
              role: AccountType.User,
              nbf: DateTime.fromMillisecondsSinceEpoch(1000 * 1679526106),
              exp: DateTime.fromMillisecondsSinceEpoch(1000 * 1679540506),
              iat: DateTime.fromMillisecondsSinceEpoch(1000 * 1679526106)),
      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SWQiOiI1Yjk2YTUyZi0zNTMzLTQ1ODEtYTEzYi0xNjdhYmI5YTk0NGYiLCJyb2xlIjoiVXNlciIsIm5iZiI6MTY3OTY3MjA3NiwiZXhwIjoxNjc5Njg2NDc2LCJpYXQiOjE2Nzk2NzIwNzZ9.Wvjfm-uxaTMKQeLsdDZB_9dhV4NV9R2hyamWJf1xOLs":
          JWTTokenParsed(
              accountId: "5b96a52f-3533-4581-a13b-167abb9a944f",
              role: AccountType.User,
              nbf: DateTime.fromMillisecondsSinceEpoch(1000 * 1679672076),
              exp: DateTime.fromMillisecondsSinceEpoch(1000 * 1679686476),
              iat: DateTime.fromMillisecondsSinceEpoch(1000 * 1679672076)),
      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SWQiOiI4ZjAwNjA5OS0xZmRhLTQyNjEtOTI0OC0zZDE2ZTg0MDcyZWYiLCJyb2xlIjoiT3JnYW5pemVyIiwibmJmIjoxNjc5Njc0NDY0LCJleHAiOjE2Nzk2ODg4NjQsImlhdCI6MTY3OTY3NDQ2NH0.GuNXvvlGdCCB9tIVLDsIqJh3K33cgW4cSKN42L7XBp8":
          JWTTokenParsed(
              accountId: "8f006099-1fda-4261-9248-3d16e84072ef",
              role: AccountType.Organizer,
              nbf: DateTime.fromMillisecondsSinceEpoch(1000 * 1679674464),
              exp: DateTime.fromMillisecondsSinceEpoch(1000 * 1679688864),
              iat: DateTime.fromMillisecondsSinceEpoch(1000 * 1679674464)),
      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SWQiOiJhMThiODVhNy0zYmMzLTQ2Y2QtOTQwMC0yYWFiZTgzOTQ4YmIiLCJyb2xlIjoiT3JnYW5pemVyIiwibmJmIjoxNjc5Njc0NTU5LCJleHAiOjE2Nzk2ODg5NTksImlhdCI6MTY3OTY3NDU1OX0.IhNn3laHxxWlN1P6VCiwr6CTFPO2XilOK2o71ScPobM":
          JWTTokenParsed(
              accountId: "a18b85a7-3bc3-46cd-9400-2aabe83948bb",
              role: AccountType.Organizer,
              nbf: DateTime.fromMillisecondsSinceEpoch(1000 * 1679674559),
              exp: DateTime.fromMillisecondsSinceEpoch(1000 * 1679688959),
              iat: DateTime.fromMillisecondsSinceEpoch(1000 * 1679674559)),
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
