import 'package:ticketer/backend_communication/model/account_type.dart';

class Account {
  final AccountType type;
  final String accountId;
  final String token;

  Account(AccountType role, this.accountId, this.token) : type = role;
}
