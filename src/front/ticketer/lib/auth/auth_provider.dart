import 'dart:async';
import 'dart:convert';
import 'dart:developer';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:ticketer/auth/account.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/model/credentials.dart';

import 'package:dio/dio.dart';

class AuthProvider {
  final storage = const FlutterSecureStorage();
  bool initialized = false;
  AuthProvider() : _controller = StreamController<Account?>();
  final StreamController<Account?> _controller;
  final dio = Dio();
  static const String loginEndpoint = '/account/login';
  static const Map<String, dynamic> headers = <String, String>{
    "Access-Control-Allow-Origin": "*",
    'Content-Type': 'application/json',
    'Accept': '*/*'
  };

  Account? _currentAccount;

  Account? get getCurrentAccount => _currentAccount;

  // Initialize provider by checking if we have stored any valid token
  init() async {
    String? url = dotenv.env['BACKEND_URL'];

    if (url == null) {
      throw Exception("Missing dotenv file");
    }

    // Configure request sending
    dio.options.baseUrl = url;
    dio.options.headers = headers;

    String? token = await _fetchTokenToStorage();

    _authToken(token);

    initialized = true;
  }

  // Check if token is valid and propagate logged in state
  // Debug skips checking if token is correct in terms of expiration dates
  void _authToken(String? token) async {
    if (token != null) {
      Token jwtToken;
      try {
        jwtToken = Token(token);
      } catch (e) {
        log("Failed to parse token: ${e.toString()}");
        return;
      }

      if (isTokenValid(jwtToken)) {
        _currentAccount =
            Account(jwtToken.role, jwtToken.accountId, jwtToken.token);
        _pushTokenToStorage(token);
        // Sent notification that user is logged in
        _controller.add(_currentAccount);
      }
    }
  }

  bool isTokenValid(Token? jwtToken) =>
      jwtToken != null &&
      jwtToken.expire.isAfter(DateTime.now()) &&
      jwtToken.issued.isBefore(DateTime.now()) &&
      jwtToken.notBefore.isBefore(DateTime.now());

  Future<void> _pushTokenToStorage(String token) async {
    await storage.write(key: 'token', value: token);
  }

  Future<String?> _fetchTokenToStorage() async {
    return await storage.read(key: 'token');
  }

  // Sends call to API with login credentials, throws error if something goes
  // wrong. If OK returns JWT Token as String
  Future<String?> _sentLogInRequest(Credentials credentials) async {
    Response response;

    try {
      response = await dio.post(loginEndpoint, data: jsonEncode(credentials));
    } catch (e) {
      log(e.toString());
      throw Exception("Connection error: ${e.toString()}");
    }
    if (response.statusCode != 200) {
      // Something to do with it later
      log("Response ${response.statusCode} : ${response.statusMessage}");
      throw Exception(
          "Response ${response.statusCode} : ${response.statusMessage}");
    }

    return response.data['accessToken'];
  }

  Future<void> logInWithEmailAndPassword({
    required String email,
    required String password,
  }) async {
    Credentials cred = Credentials(email, password);

    try {
      var token = await _sentLogInRequest(cred);
      _authToken(token);
    } catch (e) {
      log("Error when trying to log-in: ${e.toString()}");
    }
  }

  Future<void> logOut() async {
    storage.delete(key: 'token');
    _currentAccount = null;
    _controller.add(null);
  }

  Stream<Account?> get authStateChanges => _controller.stream;
}
