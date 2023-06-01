class User {
  String firstName;
  String lastName;
  String birthDate;
  String email;
  String? password; // User data fetched from backend does not contain password field.

  User(
      this.firstName, this.lastName, this.birthDate, this.email, this.password);

  Map<String, dynamic> toJson() => {
        'firstName': firstName,
        'lastName': lastName,
        'birthDate': birthDate,
        'email': email,
        'password': password,
      };

  factory User.fromJson(Map<String, dynamic> json) {
    return User(
        json['firstName'],
        json['lastName'],
        json['birthDate'],
        json['email'],
        null
    );
  }
}

class UserUpdate {
  String firstName;
  String lastName;
  String birthDate;
  String email;

  UserUpdate(this.firstName, this.lastName, this.birthDate, this.email);

  Map<String, dynamic> toJson() => {
        'firstName': firstName,
        'lastName': lastName,
        'birthDate': birthDate,
        'email': email,
      };


}
