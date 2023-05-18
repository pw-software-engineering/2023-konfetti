class Credentials {
  String email;
  String password;

  Credentials(this.email, this.password);

  Map<String, dynamic> toJson() => {
        'email': email,
        'password': password,
      };
}

class Password {
  String password;

  Password(this.password);

  Map<String, dynamic> toJson() => {
        'password': password,
      };
}
