import 'dart:async';
import 'dart:convert';
import 'dart:developer';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:http/http.dart';
import 'package:ticketer/auth/account.dart';
import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:ticketer/model/account_type.dart';
import 'package:ticketer/model/credentials.dart';

import 'package:jwt_decoder/jwt_decoder.dart';

class AuthModel extends ChangeNotifier {
  final storage = const FlutterSecureStorage();
  String? _token;
  bool get isAuthorized => _token != null;

  init() async {
    try {
      _token = await storage.read(key: 'token');
      log("Retrived token from storage");
    } catch (e) {
      log("Error when retriving token: ${e.toString()}");
    }

    notifyListeners();
  }

  login(String token) async {
    storage.write(key: 'token', value: token);

    _token = token;

    notifyListeners();
  }

  logout() {
    _token = null;
    storage.delete(key: 'token');

    notifyListeners();
  }

  Future<String?> getToken() async {
    return await storage.read(key: 'token');
  }
}

class AuthProvider {
  var authModel = AuthModel();

  AuthProvider() : _controller = StreamController<Account?>();

  Account? getCurrentAccount() {
    return Account(AccountType.Organizer.name);
  }

  init() async {
    await authModel.init();
    if (authModel.isAuthorized) {
      if (authModel._token == null) return;
      String token = authModel._token ?? "";

      Map<String, dynamic> decodedToken = JwtDecoder.decode(token);

      log("Logged in as ${decodedToken["role"]}");
      log(token);

      _controller.add(Account(decodedToken["role"]));
    }
  }

  final StreamController<Account?> _controller;

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
        log("Response ${response.statusCode} : ${response.reasonPhrase}");
        return;
      }

      // sanity check
      log('${response.statusCode} : ${response.body}');

      var decodedResponse = jsonDecode(response.body) as Map;

      await authModel.login(decodedResponse['accessToken']);

      Map<String, dynamic> decodedToken =
          JwtDecoder.decode(decodedResponse['accessToken']);

      log("Logged in as ${decodedToken["role"]}");

      log(decodedResponse['accessToken']);

      _controller.add(Account(decodedToken["role"]));
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

  Stream<Account?> get authStateChanges => _controller.stream;
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

  Stream<Account?> get authStateChanges => _provider.authStateChanges;

  Account? get getCurrentUser => _provider.getCurrentAccount();
}
