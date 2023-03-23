import 'package:ticketer/model/account_type.dart';

class Account {
  late AccountType type;

  Account(String accountType) {
    type = AccountType.values.firstWhere((e) => e.name == accountType);
  }
}
