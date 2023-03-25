// ignore_for_file: constant_identifier_names

enum AccountType {
  User,
  Organizer,
  Admin,
}

class AccountTypeConverter {
  static AccountType toEnum(String type) {
    try {
      return AccountType.values
          .firstWhere((e) => e.toString() == 'AccountType.$type');
    } catch (_) {
      throw ArgumentError("Could not pars '$type' to AccountType enum");
    }
  }
}
