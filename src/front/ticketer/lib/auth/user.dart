import 'package:ticketer/model/user_type.dart';

class User {
  late UserType type;

  User(String userType) {
    UserType type = UserType.values.firstWhere((e) => e.name == userType);
    type = type;
  }
}
