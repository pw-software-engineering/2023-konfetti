import 'dart:async';
import 'dart:convert';

import 'package:ticketer/auth/user.dart';
import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/http.dart' as http;

class AuthModel extends ChangeNotifier {
  final storage = const FlutterSecureStorage();
  String? _token;
  bool isAuthorized = false;

  init() async {
    _token = await storage.read(key: 'token');

    isAuthorized = _token != null;

    notifyListeners();
  }

  login(String token) async {
    storage.write(key: 'token', value: token);

    _token = token;
    isAuthorized = true;

    notifyListeners();
  }

  logout() {
    _token = null;
    isAuthorized = false;
    storage.delete(key: 'token');

    notifyListeners();
  }
}

class AuthProvider {
  var authModel = AuthModel();

  Auth() async {
    authModel.init();
  }

  final StreamController<User?> _controller = StreamController<User?>();

  Future<void> logInWithEmailAndPassword({
    required String email,
    required String password,
  }) async {
    if (authModel.isAuthorized) {
      throw Exception("How did You get here?");
    } else {
      var url = Uri.parse('http://localhost:8080/login');
      var response = await http.post(url);
      var decodedResponse = jsonDecode(response.body) as Map;

      authModel.login(decodedResponse['access_token']);

      print("Logged in");

      _controller.add(User());
    }
  }

  Future<void> logOut() async {
    if (authModel.isAuthorized) {
      authModel.logout();
      print("Logged out");
      _controller.add(null);
    } else {
      throw Exception("How did You get here?");
    }
  }

  Stream<User?> get authStateChanges => _controller.stream;
}

class Auth {
  static final Auth _singleton = Auth._internal();
  final AuthProvider _provider;

  factory Auth() {
    return _singleton;
  }

  Auth._internal() : _provider = AuthProvider();

  Future<void> logInWithEmailAndPassword({
    required String email,
    required String password,
  }) async {
    await _provider.logInWithEmailAndPassword(email: email, password: password);
  }

  Future<void> logOut() async {
    await _provider.logOut();
  }

  Stream<User?> get authStateChanges => _provider.authStateChanges;
}
