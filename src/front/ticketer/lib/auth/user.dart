import 'package:ticketer/model/user_type.dart';

class User {
  late UserType type;

  User(String userType) {
    type = UserType.values.firstWhere((e) => e.name == userType);
  }
}
