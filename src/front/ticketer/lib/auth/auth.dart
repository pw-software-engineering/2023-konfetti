import 'dart:async';
import 'dart:convert';
import 'dart:developer';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:http/http.dart';
import 'package:ticketer/auth/user.dart';
import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:ticketer/model/credentials.dart';

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

  AuthProvider() : _controller = StreamController<User?>();

  init() async {
    await authModel.init();
    if (authModel.isAuthorized) {
      _controller.add(User());
    }
  }

  final StreamController<User?> _controller;

  Future<void> logInWithEmailAndPassword({
    required String email,
    required String password,
  }) async {
    if (authModel.isAuthorized) {
      throw Exception("How did You get here?");
    } else {
      Credentials cred = Credentials(email, password);
      String? url = dotenv.env['BACKEND_URL'];
      Response response;
      try {
        response = await post(
          Uri.http(url!, '/account/login'),
          headers: <String, String>{
            "Access-Control-Allow-Origin": "*",
            'Content-Type': 'application/json',
            'Accept': '*/*'
          },
          body: jsonEncode(cred),
        );
      } catch (e) {
        log(e.toString());
        return;
      }

      if (response.statusCode != 200) {
        // Something to do with it later
        log("Resposne ${response.statusCode} : ${response.reasonPhrase}");
        return;
      }

      // sanity check
      log('${response.statusCode} : ${response.body}');

      var decodedResponse = jsonDecode(response.body) as Map;

      authModel.login(decodedResponse['accessToken']);

      log("Logged in");
      log(decodedResponse['accessToken']);

      _controller.add(User());
    }
  }

  Future<void> logOut() async {
    if (authModel.isAuthorized) {
      authModel.logout();
      log("Logged out");
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

  Auth._internal() : _provider = AuthProvider() {
    _provider.init();
  }

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
