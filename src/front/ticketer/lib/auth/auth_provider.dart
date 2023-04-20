import 'dart:async';
import 'dart:developer';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:ticketer/auth/account.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:ticketer/auth/jwt_token.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/credentials.dart';

import 'package:ticketer/backend_communication/model/organizer_account.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/backend_communication/model/user.dart';

class AuthProvider {
  final storage = const FlutterSecureStorage();
  bool _isInitialized = false;
  bool get isInitialized => _isInitialized;
  AuthProvider() : _controller = StreamController<Account?>();
  final StreamController<Account?> _controller;
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
  Future<void> init({bool skipSavedToken = false}) async {
    if (_isInitialized) return;
    if (!BackendCommunication().isInitialized) {
      throw Exception("Backend communication object is not initilized");
    }
    if (!skipSavedToken) {
      String? url = dotenv.env['BACKEND_URL'];

      if (url == null) {
        throw Exception("Missing dotenv file");
      }

      String? token = await _fetchTokenFromStorage();

      _authToken(token);
    }

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

      if (jwtToken.isValid) {
        _currentAccount =
            Account(jwtToken.role, jwtToken.accountId, jwtToken.token);
        _pushTokenToStorage(token);
        // Sent notification that user is logged in
        _controller.add(_currentAccount);
      }
    }
  }

  Future<void> _pushTokenToStorage(String token) async {
    await storage.write(key: 'token', value: token);
  }

  Future<String?> _fetchTokenFromStorage() async {
    return await storage.read(key: 'token');
  }

  Future<ResponseCode> logInWithEmailAndPassword({
    required String email,
    required String password,
  }) async {
    Credentials cred = Credentials(email, password);

    try {
      var token = await BackendCommunication().account.login(cred);
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

  Future<ResponseCode> registerOrginizer(OrganizerAccount organizer) async {
    try {
      var token = await BackendCommunication().organizer.register(organizer);
      return token.item2;
    } catch (e) {
      log("Error when trying to log-in: ${e.toString()}");
      return ResponseCode.noResponseCode;
    }
  }

  Future<ResponseCode> registerUser(User user) async {
    try {
      var token = await BackendCommunication().user.register(user);
      return token.item2;
    } catch (e) {
      log("Error when trying to log-in: ${e.toString()}");
      return ResponseCode.noResponseCode;
    }
  }
}
