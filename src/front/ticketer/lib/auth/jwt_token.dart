import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:ticketer/backend_communication/model/account_type.dart';

class Token {
  final String token;

  final AccountType role;
  final DateTime expire;
  final DateTime issued;
  final DateTime notBefore;
  final String accountId;

  Token._(
      {required this.accountId,
      required this.expire,
      required this.issued,
      required this.notBefore,
      required this.role,
      required this.token});

  factory Token(String token) {
    Map<String, dynamic> decodedToken;
    try {
      decodedToken = JwtDecoder.decode(token);
    } catch (e) {
      throw ArgumentError("Error when parsing token: ${e.toString()}");
    }

    try {
      AccountType role = _setRole(decodedToken["role"]);
      DateTime expire = _setExpire(decodedToken["exp"]);
      DateTime issued = _setIssued(decodedToken["iat"]);
      DateTime notBefore = _setNotValidBefore(decodedToken["nbf"]);
      String accountId = decodedToken["AccountId"];

      return Token._(
          accountId: accountId,
          expire: expire,
          issued: issued,
          notBefore: notBefore,
          role: role,
          token: token);
    } catch (e) {
      throw ArgumentError(
          "Error when converting token JSON to data: ${e.toString()}");
    }
  }

  static AccountType _setRole(String roleString) {
    try {
      return AccountTypeConverter.toEnum(roleString);
    } catch (e) {
      throw ArgumentError(
          "Could not pars '$roleString' to AccountType for 'role' field");
    }
  }

  static DateTime _setExpire(int expireEpoch) {
    try {
      return DateTime.fromMillisecondsSinceEpoch(expireEpoch * 1000);
    } catch (e) {
      throw ArgumentError(
          "Could not pars '$expireEpoch' to DateTime for 'exp' field");
    }
  }

  static DateTime _setIssued(int issuedEpoch) {
    try {
      return DateTime.fromMillisecondsSinceEpoch(issuedEpoch * 1000);
    } catch (e) {
      throw ArgumentError(
          "Could not pars '$issuedEpoch' to DateTime for 'iat' field");
    }
  }

  static DateTime _setNotValidBefore(int notValidBeforeEpoch) {
    try {
      return DateTime.fromMillisecondsSinceEpoch(notValidBeforeEpoch * 1000);
    } catch (e) {
      throw ArgumentError(
          "Could not pars '$notValidBeforeEpoch' to DateTime for 'nbf' field");
    }
  }
}
