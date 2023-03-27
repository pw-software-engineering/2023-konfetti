class Credentials {
  String email;
  String password;

  Credentials(this.email, this.password);

  Map<String, dynamic> toJson() => {
        'email': email,
        'password': password,
      };
}
