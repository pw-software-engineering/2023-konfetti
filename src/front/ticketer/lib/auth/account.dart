import 'package:ticketer/model/account_type.dart';

class Account {
  late AccountType type;

  Account(String accountType) {
    AccountType type = AccountType.values.firstWhere((e) => e.name == accountType);
    type = type;
  }
}
