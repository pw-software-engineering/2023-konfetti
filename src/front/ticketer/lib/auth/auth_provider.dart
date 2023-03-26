import 'dart:async';
import 'dart:convert';
import 'dart:developer';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:ticketer/auth/account.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/logic/dio_interceptors.dart';
import 'package:ticketer/backend_communication/model/credentials.dart';

import 'package:dio/dio.dart';
import 'package:ticketer/backend_communication/model/organizer.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/backend_communication/model/user.dart';
import 'package:tuple/tuple.dart';

class AuthProvider {
  final storage = const FlutterSecureStorage();
  bool _isInitialized = false;
  bool get isInitialized => _isInitialized;
  AuthProvider() : _controller = StreamController<Account?>();
  final StreamController<Account?> _controller;
  final dio = Dio();
  static const String loginEndpoint = '/account/login';
  static const String organizerRegisterEndpoint = '/organizer/register';
  static const String userRegisterEndpoint = '/user/register';
  static const Map<String, dynamic> headers = <String, String>{
    "Access-Control-Allow-Origin": "*",
    'Content-Type': 'application/json',
    'Accept': '*/*'
  };

  Account? _currentAccount;

  Account? get getCurrentAccount => _currentAccount;

  // Initialize provider by checking if we have stored any valid token
  init() async {
    if (_isInitialized) return;
    String? url = dotenv.env['BACKEND_URL'];

    if (url == null) {
      throw Exception("Missing dotenv file");
    }

    // Configure request sending
    dio.options.baseUrl = url;
    dio.options.headers = headers;
    dio.options.receiveDataWhenStatusError = true;
    dio.interceptors.add(CustomInterceptors());

    String? token = await _fetchTokenFromStorage();

    _authToken(token);

    _isInitialized = true;
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

  Future<String?> _fetchTokenFromStorage() async {
    return await storage.read(key: 'token');
  }

  // Sends call to API with login credentials, throws error if something goes
  // wrong. If OK returns JWT Token as String
  Future<Tuple2<Response, ResponseCode>> _sentLogInRequest(
      Credentials credentials) async {
    var response = await BackendCommunication()
        .postCall(loginEndpoint, data: jsonEncode(credentials));
    return response;
  }

  Future<ResponseCode> logInWithEmailAndPassword({
    required String email,
    required String password,
  }) async {
    Credentials cred = Credentials(email, password);

    try {
      var token = await _sentLogInRequest(cred);
      if ((token.item2.value) == 200) {
        _authToken(token.item1.data['accessToken']);
      }
      return token.item2;
    } catch (e) {
      log("Error when trying to log-in: ${e.toString()}");
      return ResponseCode.noResponseCode;
    }
  }

  Future<void> logOut() async {
    storage.delete(key: 'token');
    _currentAccount = null;
    _controller.add(null);
  }

  Stream<Account?> get authStateChanges => _controller.stream;

  Future<ResponseCode> registerOrginizer(Organizer organizer) async {
    try {
      var token = await BackendCommunication()
          .postCall(organizerRegisterEndpoint, data: jsonEncode(organizer));
      return token.item2;
    } catch (e) {
      log("Error when trying to log-in: ${e.toString()}");
      return ResponseCode.noResponseCode;
    }
  }

  Future<ResponseCode> registerUser(User user) async {
    try {
      var token = await BackendCommunication()
          .postCall(userRegisterEndpoint, data: jsonEncode(user));
      return token.item2;
    } catch (e) {
      log("Error when trying to log-in: ${e.toString()}");
      return ResponseCode.noResponseCode;
    }
  }
}
