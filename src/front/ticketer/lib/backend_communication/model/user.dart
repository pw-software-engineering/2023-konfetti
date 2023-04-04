
class User {
  String firstName;
  String lastName;
  String birthDate;
  String email;
  String password;

  User(this.firstName, this.lastName, this.birthDate, this.email, this.password);

  Map<String, dynamic> toJson() => {
    'firstName': firstName,
    'lastName': lastName,
    'birthDate': birthDate,
    'email': email,
    'password': password,
  };
}
